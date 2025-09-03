using API.Data;
using API.DTO;
using API.Extension;
using API.Interface;
using API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    public class AccountController(AppDbContext _context, ITokenService _tokenService) : BaseAPIController
    {
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UsernameExists(registerDto.Username)) return BadRequest("Username already in use.");
            if (await EmailExists(registerDto.Email)) return BadRequest("Email address already in use.");

            using var hmac = new HMACSHA512();
            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user.UserToUserDto(_tokenService));
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Email.ToLower() == loginDto.Email.ToLower());
            if (user is null) return Unauthorized("Invalid email address.");

            if (!PasswordsMatch(loginDto.Password, user.PasswordHash, user.PasswordSalt)) return Unauthorized("Invalid password.");

            return Ok(user.UserToUserDto(_tokenService));
        }

        private async Task<bool> UsernameExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.Username.ToLower().Equals(username.ToLower()));
        }

        private async Task<bool> EmailExists(string email)
        {
            return await _context.Users.AnyAsync(x => x.Email.ToLower().Equals(email.ToLower()));
        }

        private bool PasswordsMatch(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            for (var i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != storedHash[i]) return false;
            }

            return true;
        }
    }
}
