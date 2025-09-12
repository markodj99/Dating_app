using API.DTO;
using API.Interface;
using API.Model;
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

        public User CreateNewUser(HMACSHA512 hmac, RegisterDto registerDto)
            => new()
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
