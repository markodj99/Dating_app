using API.Data;
using API.Model;
using API.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class AccountRepository(AppDbContext _context) : IAccountRepository
    {
        public async Task<User?> GetUserByIdAsync(string id) => await _context.Users.FirstOrDefaultAsync(u => u.Id.Equals(id));
        
        public async Task<User?> GetUserByEmailAsync(string email) 
            => await _context.Users.SingleOrDefaultAsync(x => x.Email.ToLower().Equals(email.ToLower()));

        public void AddUser(User user) => _context.Users.Add(user);

        public async Task<bool> UsernameExistsAsync(string username) 
            => await _context.Users.AnyAsync(x => x.Username.ToLower().Equals(username.ToLower()));

        public async Task<bool> EmailExistsAsync(string email) => await _context.Users.AnyAsync(x => x.Email.ToLower().Equals(email.ToLower()));

        public async Task<bool> SaveAllChangesAsync() => await _context.SaveChangesAsync() > 0;
    }
}
