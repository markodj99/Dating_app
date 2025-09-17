using API.DTO;
using API.Extension;
using API.Interface;
using API.Model;
using API.Repository.IRepository;
using API.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountController(IUnitOfWork _uow, ITokenService _tokenService, IAccountService _accountService) : BaseApiController
    {
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await _uow.AccountRepository.UserNameExistsAsync(registerDto.UserName)) return BadRequest("UserName is already in use.");
            if (await _uow.AccountRepository.EmailExistsAsync(registerDto.Email)) return BadRequest("Email address is already in use.");
            if (!registerDto.Password.Equals(registerDto.ConfirmPassword)) return BadRequest("Passwords must match.");

            var user = _accountService.CreateNewUser(registerDto);
            var result = await _uow.AccountRepository.AddUser(user, registerDto.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors) ModelState.AddModelError("identity", error.Description);
                return ValidationProblem();
            }
            await _uow.AccountRepository.AddRoleToUser(user);

            return Ok(await SetResponseAndReturnUserDto(user));
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _uow.AccountRepository.GetUserByEmailAsync(loginDto.Email);
            if (user is null) return Unauthorized("Invalid email address.");
            if (!await _uow.AccountRepository.PasswordsMatch(user, loginDto.Password)) return Unauthorized("Wrong password.");

            return Ok(await SetResponseAndReturnUserDto(user));
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<UserDto>> RefreshToken()
        {
            var refreshedToken = Request.Cookies["refreshToken"];
            if (refreshedToken is null) return NoContent();

            var user = await _uow.AccountRepository.GetUserByRefreshTokenAsync(refreshedToken);
            if (user is null) return Unauthorized();

            return Ok(await SetResponseAndReturnUserDto(user));
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            await _uow.AccountRepository.Logout(User.GetMemberId());
            Response.Cookies.Delete("refreshToken");
            return Ok();
        }

        private async Task<UserDto> SetResponseAndReturnUserDto(User user)
        {
            var refreshToken = _tokenService.GenerateRefreshToken();
            var cookieOptions = await _accountService.SetRefreshTokenCookie(user, refreshToken, _uow);
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
            return ToDto.UserToUserDto(user, await _tokenService.CreateJWTToken(user));
        }
    }
}
