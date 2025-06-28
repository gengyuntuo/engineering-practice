
using ChatRoomLibrary.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoomLibrary.Data
{
    public abstract class AbstractSqliteDatabase
    {
        protected readonly string _connectionString;
        protected SQLiteConnection _connection;
        protected bool _disposed = false;


        public AbstractSqliteDatabase(string databasePath)
        {
            _connectionString = $"Data Source={databasePath};Version=3;";
            _connection = new SQLiteConnection(_connectionString);
            _connection.Open();
            InitializeDatabase();
        }

        protected abstract void InitializeDatabase();

    }

    /// <summary>
    /// 配置表
    /// </summary>
    public class ConfigDataAccess(SQLiteConnection _connection)
    {
        public async Task<Config?> GetConfigByKeyAsync(string key)
        {
            using var command = new SQLiteCommand(
                @"SELECT Id, Key, Value, Description, Type, IsSystem, CreatedAt, UpdatedAt
                  FROM Configs
                  WHERE Key = @Key", _connection);
            command.Parameters.AddWithValue("@Key", key);
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Config
                {
                    Id = reader.GetInt32(0),
                    Key = reader.GetString(1),
                    Value = reader.GetString(2),
                    Description = reader.GetString(3),
                    Type = reader.GetString(4),
                    IsSystem = reader.GetBoolean(5),
                    CreatedAt = reader.GetDateTime(6),
                    UpdatedAt = reader.GetDateTime(7),
                };
            }
            return null;
        }

        public async Task<int> UpsertConfigAsync(Config config)
        {
            using var command = new SQLiteCommand(
                @"INSERT INTO Configs (Key, Value, Description, Type, IsSystem)
                  VALUES (@Key, @Value, @Description, @Type, @IsSystem)
                  ON CONFLICT (Key) DO UPDATE
                  SET Value = @Value,
                      Description = @Description,
                      Type = @Type,
                      IsSystem = @IsSystem;SELECT changes();", _connection);
            command.Parameters.AddWithValue("@Key", config.Key);
            command.Parameters.AddWithValue("@Value", config.Value);
            command.Parameters.AddWithValue("@Description", config.Description);
            command.Parameters.AddWithValue("@Type", config.Type);
            command.Parameters.AddWithValue("@IsSystem", config.IsSystem);
            return Convert.ToInt32(await command.ExecuteScalarAsync());
        }
    }

    /// <summary>
    /// 用户表
    /// </summary>
    public class UserDataAccess(SQLiteConnection _connection)
    {
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            using var command = new SQLiteCommand(
                @"SELECT Id, Username, PasswordHash, CreatedAt, UpdatedAt
                  FROM Users WHERE Username = @Username", _connection);
            command.Parameters.AddWithValue("@Username", username);
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new User
                {
                    Id = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    PasswordHash = reader.GetString(2),
                    CreatedAt = reader.GetDateTime(3),
                    UpdatedAt = reader.GetDateTime(4),
                };
            }
            return null;
        }

        public async Task<User?> GetUserByUsernamePasswordAsync(string username, string passwordHash)
        {
            using var command = new SQLiteCommand(
                @"SELECT Id, Username, PasswordHash, CreatedAt, UpdatedAt
                  FROM Users WHERE Username = @Username AND PasswordHash = @PasswordHash", _connection);
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@PasswordHash", passwordHash);
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new User
                {
                    Id = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    PasswordHash = reader.GetString(2),
                    CreatedAt = reader.GetDateTime(3),
                    UpdatedAt = reader.GetDateTime(4),
                };
            }
            return null;
        }

        public async Task<int> CreateUserAsync(User user)
        {
            using var command = new SQLiteCommand(
                @"INSERT INTO Users (Username, PasswordHash)
                  VALUES (@Username, @PasswordHash);
                  SELECT last_insert_rowid();", _connection);
            command.Parameters.AddWithValue("@Username", user.Username);
            command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
            return Convert.ToInt32(await command.ExecuteScalarAsync());
        }

        internal async Task<List<User>> GetUserListByUsernameAsync(string username)
        {
            using var command = new SQLiteCommand(
                @"SELECT Id, Username, PasswordHash, CreatedAt, UpdatedAt
                  FROM Users WHERE Username LIKE @Username", _connection);
            command.Parameters.AddWithValue("@Username", $"%{username ?? ""}%");
            using var reader = await command.ExecuteReaderAsync();
            List<User> users = [];
            while (await reader.ReadAsync())
            {
                users.Add(new User
                {
                    Id = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    PasswordHash = reader.GetString(2),
                    CreatedAt = reader.GetDateTime(3),
                    UpdatedAt = reader.GetDateTime(4),
                });
            }
            return users;
        }
    }

    /// <summary>
    /// 好友关系
    /// </summary>
    public class FriendshipDataAccess(SQLiteConnection _connection)
    {
        public async Task<List<User>> GetFriendsByUserIdAsync(int userId)
        {
            List<User> firends = [];
            using var command = new SQLiteCommand(
                @"SELECT UserId, FriendId, IsConfirmed, Username
                  FROM Friendships tf LEFT JOIN Users tu ON tf.FriendId = tu.Id
                  WHERE UserId = @Id AND IsConfirmed", _connection);
            command.Parameters.AddWithValue("@Id", userId);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                firends.Add(new User
                {
                    Id = reader.GetInt32(1),
                    Username = reader.GetString(3),
                });
            }
            return firends;
        }
        public async Task<int> CreateFriendAsync(Friendship friendship)
        {
            using var command = new SQLiteCommand(
                @"INSERT INTO Friendships (UserId, FriendId, IsConfirmed)
                  VALUES (@UserId, @FriendId, @IsConfirmed)
                  ON CONFLICT (UserId, FriendId) DO UPDATE
                  SET IsConfirmed = @IsConfirmed;SELECT changes();", _connection);
            command.Parameters.AddWithValue("@UserId", friendship.UserId);
            command.Parameters.AddWithValue("@FriendId", friendship.FriendId);
            command.Parameters.AddWithValue("@IsConfirmed", friendship.IsConfirmed);
            return Convert.ToInt32(await command.ExecuteScalarAsync());
        }
    }

    /// <summary>
    /// 消息
    /// </summary>
    public class MessageDataAccess(SQLiteConnection _connection)
    {
        public async Task<List<Message>> GetUnreadMessageByUserIdAsync(int userId)
        {
            List<Message> messages = [];
            using var command = new SQLiteCommand(
                @"SELECT Id, SenderId, ReceiverId, Content, IsSystem, SentAt, ReadAt
                  FROM Messages
                  WHERE ReceiverId = @Id AND ReadAt IS NULL
                  ORDER BY SentAt
                  LIMIT 20", _connection);
            command.Parameters.AddWithValue("@Id", userId);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var Id = reader.GetInt32(0);
                var SenderId = reader.GetInt32(1);
                var ReceiverId = reader.GetInt32(2);
                var Content = reader.GetString(3);
                var IsSystem = reader.GetBoolean(4);
                var SentAt = reader.GetDateTime(5);
                DateTime? ReadAt = reader.IsDBNull(6) ? null : reader.GetDateTime(6);
                var message = new Message
                {
                    Id = reader.GetInt32(0),
                    SenderId = reader.GetInt32(1),
                    ReceiverId = reader.GetInt32(2),
                    Content = reader.GetString(3),
                    IsSystem = reader.GetBoolean(4),
                    SentAt = reader.GetDateTime(5),
                    ReadAt = reader.IsDBNull(6) ? null : reader.GetDateTime(6),
                };
                messages.Add(message);
            }
            return messages;
        }
        public async Task<int> CreateMessageAsync(Message message)
        {
            using var command = new SQLiteCommand(
                @"INSERT INTO Messages (SenderId, ReceiverId, Content, IsSystem, SentAt)
                  VALUES (@SenderId, @ReceiverId, @Content, @IsSystem, @SentAt);
                  SELECT last_insert_rowid();", _connection);
            command.Parameters.AddWithValue("@SenderId", message.SenderId);
            command.Parameters.AddWithValue("@ReceiverId", message.ReceiverId);
            command.Parameters.AddWithValue("@Content", message.Content);
            command.Parameters.AddWithValue("@IsSystem", message.IsSystem);
            command.Parameters.AddWithValue("@SentAt", message.SentAt);
            return Convert.ToInt32(await command.ExecuteScalarAsync());
        }

        public async Task<int> MarkMessageAsReadAsync(int userId, List<int> messageIds)
        {
            int affectRows = 0;
            foreach (var messageId in messageIds)
            {
                using var command = new SQLiteCommand(
                @"UPDATE Messages SET ReadAt = datetime('now', 'localtime')
                  WHERE ReceiverId = @ReceiverId AND Id = @Id;
                  SELECT changes();", _connection);
                command.Parameters.AddWithValue("@ReceiverId", userId);
                command.Parameters.AddWithValue("@Id", messageId);
                affectRows += Convert.ToInt32(await command.ExecuteScalarAsync());
            }
            return affectRows;
        }

        public async Task<List<Message>> GetRelativeMessageByUserIdAsync(int userId, int friendUserId)
        {
            List<Message> messages = [];
            using var command = new SQLiteCommand(
                @"SELECT Id, SenderId, ReceiverId, Content, IsSystem, SentAt, ReadAt
                  FROM Messages
                  WHERE (ReceiverId = @ReceiverId AND SenderId = @SenderId)
                        OR (ReceiverId = @ReceiverId2 AND SenderId = @SenderId2)
                  ORDER BY SentAt
                  LIMIT 100", _connection);
            command.Parameters.AddWithValue("@ReceiverId", userId);
            command.Parameters.AddWithValue("@SenderId", friendUserId);
            command.Parameters.AddWithValue("@ReceiverId2", friendUserId);
            command.Parameters.AddWithValue("@SenderId2", userId);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var message = new Message
                {
                    Id = reader.GetInt32(0),
                    SenderId = reader.GetInt32(1),
                    ReceiverId = reader.GetInt32(2),
                    Content = reader.GetString(3),
                    IsSystem = reader.GetBoolean(4),
                    SentAt = reader.GetDateTime(5),
                    ReadAt = reader.IsDBNull(6) ? null : reader.GetDateTime(6),
                };
                messages.Add(message);
            }
            return messages;
        }

        public async Task<List<MessageStatisticItem>> GetMessageStatisticByUserIdAsync(int userId)
        {
            List<MessageStatisticItem> result = [];
            using var command = new SQLiteCommand(
                @"SELECT SenderId, COUNT(*) AS total, COUNT(ReadAt) AS read
                  FROM Messages
                  WHERE ReceiverId = @ReceiverId
                  GROUP BY SenderId
                  ", _connection);
            command.Parameters.AddWithValue("@ReceiverId", userId);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var statisticItem = new MessageStatisticItem
                {
                    SenderId = reader.GetInt32(0),
                    TotalMessage = reader.GetInt32(1),
                    ReadMessage = reader.GetInt32(2),
                    UnReadMessage = reader.GetInt32(1) - reader.GetInt32(2),
                };
                result.Add(statisticItem);
            }
            return result;
        }
    }
}
