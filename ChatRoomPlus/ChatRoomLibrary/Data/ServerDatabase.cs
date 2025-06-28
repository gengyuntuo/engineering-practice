using ChatRoomLibrary.Model;
using System.Collections.Generic;
using System.Data.SQLite;

namespace ChatRoomLibrary.Data
{
    public class IServerDatabaseFactory
    {
        public static IServerDatabase CreateSqliteServerDatabase()
        {
            return new SqliteServerDatabase("server.db");
        }
    }
    public interface IServerDatabase
    {
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User?> GetUserByUsernamePasswordAsync(string username, string passwordHash);
        Task<int> CreateUserAsync(User user);
        Task<List<User>> GetUserListByUsernameAsync(string username);
        Task<List<User>> GetFriendsByUserIdAsync(int userId);
        Task<int> CreateFriendAsync(Friendship friendship);
        Task<int> CreateMessageAsync(Message message);
        Task<List<Message>> GetUnreadMessageByUserIdAsync(int userId);
        Task<int> MarkMessageAsReadAsync(int userId, List<int> messageIds);
        Task<Config?> GetConfigByKeyAsync(string key);
        Task<int> UpsertConfigAsync(Config config);
    }

    public class SqliteServerDatabase : AbstractSqliteDatabase, IServerDatabase
    {
        private readonly UserDataAccess _userDataAccess;
        private readonly MessageDataAccess _messageDataAccess;
        private readonly ConfigDataAccess _configDataAccess;
        private readonly FriendshipDataAccess _friendshipDataAccess;

        public SqliteServerDatabase(string databasePath) : base(databasePath)
        {
            _userDataAccess = new UserDataAccess(_connection);
            _messageDataAccess = new MessageDataAccess(_connection);
            _configDataAccess = new ConfigDataAccess(_connection);
            _friendshipDataAccess = new FriendshipDataAccess(_connection);
        }

        protected override void InitializeDatabase()
        {
            try
            {
                // 创建用户表（如果不存在）
                var createTableQuery = @"
                    -- 创建配置表
                    CREATE TABLE IF NOT EXISTS Configs (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Key TEXT NOT NULL UNIQUE,   -- 配置键（唯一标识）
                        Value TEXT NOT NULL,        -- 配置值
                        Description TEXT,           -- 配置描述
                        Type TEXT DEFAULT 'string', -- 配置类型：string, int, bool, datetime
                        IsSystem BOOLEAN DEFAULT 0, -- 是否为系统配置（不可删除）
                        CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                        UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                    );

                    -- 创建索引加速查询
                    CREATE INDEX IF NOT EXISTS idx_configs_key ON Configs(Key);

                    -- 创建用户表
                    CREATE TABLE IF NOT EXISTS Users (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Username TEXT NOT NULL UNIQUE,
                        PasswordHash TEXT NOT NULL,
                        CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                        UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                    );
                    -- 创建系统用户: 发布系统消息
                    INSERT INTO Users (Id, Username, PasswordHash)
                    VALUES (0, 'system', '')
                    ON CONFLICT (Username) DO NOTHING;

                    -- 创建索引加速查询
                    CREATE INDEX IF NOT EXISTS idx_users_username ON Users(Username);

                    -- 创建好友关系表（双向关系）
                    CREATE TABLE IF NOT EXISTS Friendships (
                        UserId INTEGER NOT NULL,
                        FriendId INTEGER NOT NULL,
                        IsConfirmed BOOLEAN DEFAULT 0,
                        CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                        ConfirmedAt TIMESTAMP,
                        PRIMARY KEY (UserId, FriendId),
                        FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
                        FOREIGN KEY (FriendId) REFERENCES Users(Id) ON DELETE CASCADE
                    );

                    -- 创建索引加速查询
                    CREATE INDEX IF NOT EXISTS idx_friendships_user ON Friendships(UserId);
                    CREATE INDEX IF NOT EXISTS idx_friendships_friend ON Friendships(FriendId);

                    -- 创建消息表
                    CREATE TABLE IF NOT EXISTS Messages (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        SenderId INTEGER NOT NULL,
                        ReceiverId INTEGER NOT NULL,
                        Content TEXT NOT NULL,
                        IsSystem BOOLEAN DEFAULT 0, -- 系统消息，不展示
                        SentAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                        ReadAt TIMESTAMP,
                        FOREIGN KEY (SenderId) REFERENCES Users(Id) ON DELETE CASCADE,
                        FOREIGN KEY (ReceiverId) REFERENCES Users(Id) ON DELETE CASCADE
                    );

                    -- 创建索引加速查询
                    CREATE INDEX IF NOT EXISTS idx_messages_sender ON Messages(SenderId);
                    CREATE INDEX IF NOT EXISTS idx_messages_receiver ON Messages(ReceiverId);
                    CREATE INDEX IF NOT EXISTS idx_messages_sent_at ON Messages(SentAt);
                    ";

                using var command = new SQLiteCommand(createTableQuery, _connection);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"初始化数据库时出错: {ex.Message}");
                throw;
            }
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _userDataAccess.GetUserByUsernameAsync(username);
        }

        public async Task<int> CreateUserAsync(User user)
        {
            return await _userDataAccess.CreateUserAsync(user);
        }

        public async Task<User?> GetUserByUsernamePasswordAsync(string username, string passwordHash)
        {
            return await _userDataAccess.GetUserByUsernamePasswordAsync(username, passwordHash);
        }

        public async Task<List<User>> GetFriendsByUserIdAsync(int userId)
        {
            return await _friendshipDataAccess.GetFriendsByUserIdAsync(userId);
        }
        public async Task<int> CreateFriendAsync(Friendship friendship)
        {
            return await _friendshipDataAccess.CreateFriendAsync(friendship);
        }
        public async Task<int> CreateMessageAsync(Message message)
        {
            return await _messageDataAccess.CreateMessageAsync(message);
        }
        public async Task<Config?> GetConfigByKeyAsync(string key)
        {
            return await _configDataAccess.GetConfigByKeyAsync(key);
        }
        public async Task<int> UpsertConfigAsync(Config config)
        {
            return await _configDataAccess.UpsertConfigAsync(config);
        }

        public async Task<List<User>> GetUserListByUsernameAsync(string username)
        {
            return await _userDataAccess.GetUserListByUsernameAsync(username);
        }
        public async Task<List<Message>> GetUnreadMessageByUserIdAsync(int userId)
        {
            return await _messageDataAccess.GetUnreadMessageByUserIdAsync(userId);
        }
        public async Task<int> MarkMessageAsReadAsync(int userId, List<int> messageIds)
        {
            return await _messageDataAccess.MarkMessageAsReadAsync(userId, messageIds);
        }

    }
}
