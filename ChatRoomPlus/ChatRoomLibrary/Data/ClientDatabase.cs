using ChatRoomLibrary.Model;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoomLibrary.Data
{
    public class IClientDatabaseFactory
    {
        public static IClientDatabase CreateSqliteDatabase()
        {
            return new SqliteClientDatabase("client.db");
        }
    }

    public interface IClientDatabase
    {
        Task<Config?> GetConfigByKeyAsync(string key);
        Task<int> UpsertConfigAsync(Config config);
        Task<List<Message>> GetUnreadMessageByUserIdAsync(int userId);
        Task<List<Message>> GetLocalMessageByFriendUserIdAsync(int userId, int friendUserId);
        Task<int> CreateMessageAsync(Message message);
        Task<int> MarkMessageAsReadAsync(int userId, List<int> messageIds);
        Task<List<MessageStatisticItem>> GetMessageStatisticByUserIdAsync(int userId);

    }

    public class SqliteClientDatabase : AbstractSqliteDatabase, IClientDatabase
    {
        private readonly ConfigDataAccess _configDataAccess;
        private readonly MessageDataAccess _messageDataAccess;
        public SqliteClientDatabase(string databasePath) : base(databasePath)
        {
            _configDataAccess = new(_connection);
            _messageDataAccess = new MessageDataAccess(_connection);
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
                Console.WriteLine($"初始化数据库时出错: {ex.Message}");
                throw;
            }
        }
        public async Task<Config?> GetConfigByKeyAsync(string key)
        {
            return await _configDataAccess.GetConfigByKeyAsync(key);
        }
        public async Task<int> UpsertConfigAsync(Config config)
        {
            return await _configDataAccess.UpsertConfigAsync(config);
        }
        public async Task<List<Message>> GetUnreadMessageByUserIdAsync(int userId)
        {
            return await _messageDataAccess.GetUnreadMessageByUserIdAsync(userId);
        }
        public async Task<List<Message>> GetLocalMessageByFriendUserIdAsync(int userId, int friendUserId)
        {
            return await _messageDataAccess.GetRelativeMessageByUserIdAsync(userId, friendUserId);
        }
        public async Task<int> CreateMessageAsync(Message message)
        {
            return await _messageDataAccess.CreateMessageAsync(message);
        }

        public async Task<int> MarkMessageAsReadAsync(int userId, List<int> messageIds)
        {
            return await _messageDataAccess.MarkMessageAsReadAsync(userId, messageIds);
        }

        public async Task<List<MessageStatisticItem>> GetMessageStatisticByUserIdAsync(int userId)
        {
            return await _messageDataAccess.GetMessageStatisticByUserIdAsync(userId);
        }
    }


}
