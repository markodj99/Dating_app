using API.DTO;
using API.Interface;
using API.Model;
using API.Repository.IRepository;
using System.Security.Cryptography;
using System.Text;

namespace API.Service
{
    public class AccountService : IAccountService
    {
        public bool PasswordsMatch(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            for (var i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != storedHash[i]) return false;
            }

            return true;
        }

        public User CreateNewUser(RegisterDto registerDto)
            => new()
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                Member = new Member
                {
                    DateOfBirth = DateOnly.FromDateTime(registerDto.DateOfBirth),
                    UserName = registerDto.UserName,
                    Gender = registerDto.Gender,
                    City = registerDto.City,
                    Country = registerDto.Country,
                }
            };

        public async Task<CookieOptions> SetRefreshTokenCookie(User user, string refreshToken, IUnitOfWork uow)
        {
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpire = DateTime.UtcNow.AddDays(5);
            await uow.AccountRepository.UpdateAsync(user);
            await uow.Complete();

            return new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7),
            };
        }
    }
}
