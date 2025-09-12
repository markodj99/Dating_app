using API.DTO;
using API.Model;
using System.Security.Cryptography;

namespace API.Interface
{
    public interface IAccountService
    {
        bool PasswordsMatch(string password, byte[] storedHash, byte[] storedSalt);
        User CreateNewUser(HMACSHA512 hmac, RegisterDto registerDto);
    }
}
