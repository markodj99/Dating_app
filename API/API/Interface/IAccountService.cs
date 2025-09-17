using API.DTO;
using API.Model;
using API.Repository.IRepository;

namespace API.Interface
{
    public interface IAccountService
    {
        bool PasswordsMatch(string password, byte[] storedHash, byte[] storedSalt);
        User CreateNewUser(RegisterDto registerDto);
        Task<CookieOptions> SetRefreshTokenCookie(User user, string refreshToken, IUnitOfWork uow);
    }
}
