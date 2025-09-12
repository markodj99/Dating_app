using API.DTO;
using API.Interface;
using API.Repository.IRepository;
using API.Util;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace API.Controllers
{
    public class AccountController(IAccountRepository _repo, ITokenService _tokenService, IAccountService _accountService) : BaseApiController
    {
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await _repo.UsernameExistsAsync(registerDto.Username)) return BadRequest("Username is already in use.");
            if (await _repo.EmailExistsAsync(registerDto.Password)) return BadRequest("Email address is already in use.");

            using var hmac = new HMACSHA512();
            var user = _accountService.CreateNewUser(hmac, registerDto);

            _repo.AddUser(user);
            if (await _repo.SaveAllChangesAsync()) return Ok(ToDto.UserToUserDto(user, _tokenService.CreateJWTToken(user)));
            return BadRequest("Could not register. Please try again later.");
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _repo.GetUserByEmailAsync(loginDto.Email);
            if (user is null) return Unauthorized("Invalid email address.");

            if (!_accountService.PasswordsMatch(loginDto.Password, user.PasswordHash, user.PasswordSalt)) return Unauthorized("Invalid password.");
            return Ok(ToDto.UserToUserDto(user, _tokenService.CreateJWTToken(user)));
        }
    }
}
