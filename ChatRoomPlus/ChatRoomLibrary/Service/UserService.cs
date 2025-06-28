using ChatRoomLibrary.Data;
using ChatRoomLibrary.Model;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoomLibrary.Service
{
    public class UserService
    {
        private readonly IServerDatabase _dataAccess = IServerDatabaseFactory.CreateSqliteServerDatabase();

        public async Task<User> RegisterUserAsync(RegisterModel registerModel)
        {
            // 检查用户名是否已存在
            var existingUser = await _dataAccess.GetUserByUsernameAsync(registerModel.Username);
            if (existingUser != null)
            {
                throw new ArgumentException("用户名已存在");
            }

            // 创建新用户
            var user = new User
            {
                Username = registerModel.Username,
                PasswordHash = HashPassword(registerModel.Password),
            };

            // 保存到数据库
            user.Id = await _dataAccess.CreateUserAsync(user);
            return user;
        }

        public async Task<List<User>> GetFriendsByUserIdAsync(int userId)
        {
            return await _dataAccess.GetFriendsByUserIdAsync(userId);
        }

        public async Task<User?> GetUserByUsernameAsycn(string username)
        {
            return await _dataAccess.GetUserByUsernameAsync(username);
        }

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="username">用户名称，如果用户为null或者为空则查询全部</param>
        /// <returns></returns>
        public async Task<List<User>> GetUserListByUsernameAsync(string username)
        {
            return await _dataAccess.GetUserListByUsernameAsync(username);
        }

        public async Task CreateFriendshipAsync(int userFirst, int userSecond)
        {
            await _dataAccess.CreateFriendAsync(new() { UserId = userFirst, FriendId = userSecond, IsConfirmed = true });
            await _dataAccess.CreateFriendAsync(new() { UserId = userSecond, FriendId = userFirst, IsConfirmed = true });
        }

        public async Task CancelFriendshipAsync(int userFirst, int userSecond)
        {
            await _dataAccess.CreateFriendAsync(new() { UserId = userFirst, FriendId = userSecond, IsConfirmed = false });
            await _dataAccess.CreateFriendAsync(new() { UserId = userSecond, FriendId = userFirst, IsConfirmed = false });
        }

        public async Task<User?> AuthenticateUserAsync(string username, string password)
        {
            var user = await _dataAccess.GetUserByUsernameAsync(username);
            if (user == null)
            {
                return null;
            }

            // 验证密码
            if (!VerifyPasswordHash(password, user.PasswordHash))
            {
                return null;
            }

            return user;
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            var hashOfInput = HashPassword(password);
            return StringComparer.OrdinalIgnoreCase.Compare(hashOfInput, storedHash) == 0;
        }
    }
}