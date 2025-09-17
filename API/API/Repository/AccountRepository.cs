using API.Model;
using API.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class AccountRepository(UserManager<User> _userManager) : IAccountRepository
    {
        public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken) 
            => await _userManager.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken && x.RefreshTokenExpire > DateTime.UtcNow);
        public async Task UpdateAsync(User user) => await _userManager.UpdateAsync(user);

        public async Task<User?> GetUserByIdAsync(string id) => await _userManager.FindByIdAsync(id);

        public async Task<User?> GetUserByEmailAsync(string email) => await _userManager.FindByEmailAsync(email);

        public async Task<IdentityResult> AddUser(User user, string password) => await _userManager.CreateAsync(user, password);

        public async Task AddRoleToUser(User user) => await _userManager.AddToRoleAsync(user, "Member");

        public async Task<bool> PasswordsMatch(User user, string password) => await _userManager.CheckPasswordAsync(user, password);

        public async Task<bool> UserNameExistsAsync(string username) 
            => await _userManager.Users.AnyAsync(x => x.UserName!.ToLower().Equals(username.ToLower()));

        public async Task<bool> EmailExistsAsync(string email) => await _userManager.Users.AnyAsync(x => x.Email!.ToLower().Equals(email.ToLower()));

        public async Task Logout(string userId) =>
            await _userManager.Users.Where(x => x.Id == userId)
            .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.RefreshToken, _ => null).SetProperty(x => x.RefreshTokenExpire, _ => null));

        public async Task<List<User>?> GetUsersAsync() => await _userManager.Users.OrderBy(x => x.Email).ToListAsync();

        public async Task<IList<string>?> GetRolesForAUserAsync(User user) => await _userManager.GetRolesAsync(user);

        public async Task<IdentityResult> AddToRolesAsync(User user, IEnumerable<string> roles) => await _userManager.AddToRolesAsync(user, roles);
        public async Task<IdentityResult> RemoveFromRolesAsync(User user, IEnumerable<string> roles) 
            => await _userManager.RemoveFromRolesAsync(user, roles);
    }
}
