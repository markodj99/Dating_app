using API.Data;
using API.DTO;
using API.Model;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Util
{
    public class AccountControllerHelpers
    {
        public static async Task<bool> UsernameExists(string username, AppDbContext context)
        {
            return await context.Users.AnyAsync(x => x.Username.ToLower().Equals(username.ToLower()));
        }

        public static async Task<bool> EmailExists(string email, AppDbContext context)
        {
            return await context.Users.AnyAsync(x => x.Email.ToLower().Equals(email.ToLower()));
        }

        public static bool PasswordsMatch(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            for (var i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != storedHash[i]) return false;
            }

            return true;
        }

        public static User CreateNewUser(HMACSHA512 hmac, RegisterDto registerDto)
        {
            return new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key,
                Member = new Member
                {
                    DateOfBirth = DateOnly.FromDateTime(registerDto.DateOfBirth),
                    Username = registerDto.Username,
                    Gender = registerDto.Gender,
                    City = registerDto.City,
                    Country = registerDto.Country,
                }
            };
        }
    }
}
