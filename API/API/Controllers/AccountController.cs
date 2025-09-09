using API.Data;
using API.DTO;
using API.Interface;
using API.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Hlp = API.Util.AccountControllerHelpers;

namespace API.Controllers
{
    public class AccountController(AppDbContext _context, ITokenService _tokenService) : BaseAPIController
    {
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await Hlp.UsernameExists(registerDto.Username, _context)) return BadRequest("Username already in use.");
            if (await Hlp.EmailExists(registerDto.Email, _context)) return BadRequest("Email address already in use.");
            if (!registerDto.Password.Equals(registerDto.ConfirmPassword)) return BadRequest("Passwords must match.");

            using var hmac = new HMACSHA512();
            var user = Hlp.CreateNewUser(hmac, registerDto);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(ToDto.UserToUserDto(user, _tokenService.CreateJWTToken(user)));
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Email.ToLower() == loginDto.Email.ToLower());
            if (user is null) return Unauthorized("Invalid email address.");

            if (!Hlp.PasswordsMatch(loginDto.Password, user.PasswordHash, user.PasswordSalt)) return Unauthorized("Invalid password.");

            return Ok(ToDto.UserToUserDto(user, _tokenService.CreateJWTToken(user)));
        }
    }
}
