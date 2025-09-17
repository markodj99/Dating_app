using API.Model;
using Microsoft.AspNetCore.Identity;

namespace API.Repository.IRepository
{
    public interface IAccountRepository
    {
        Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
        Task UpdateAsync(User user);
        Task<User?> GetUserByIdAsync(string id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<IdentityResult> AddUser(User user, string password);
        Task AddRoleToUser(User user);
        Task<bool> PasswordsMatch(User user, string password);
        Task<bool> UserNameExistsAsync(string username);
        Task<bool> EmailExistsAsync(string email);
        Task Logout(string userId);
    }
}
