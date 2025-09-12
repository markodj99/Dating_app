using API.Model;

namespace API.Repository.IRepository
{
    public interface IAccountRepository
    {
        Task<User?> GetUserByIdAsync(string id);
        Task<User?> GetUserByEmailAsync(string email);
        void AddUser(User user);
        Task<bool> UsernameExistsAsync(string username);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> SaveAllChangesAsync();
    }
}
