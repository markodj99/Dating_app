using API.DTO;
using API.Interface;
using API.Model;
using API.Service;

namespace API.Extension
{
    public static class UserExtensions
    {
        public static UserDto UserToUserDto(this User user, ITokenService tokenService)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Token = tokenService.CreateJWTToken(user)
            };
        }
    }
}
