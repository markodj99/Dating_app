using API.Model;

namespace API.Interface
{
    public interface ITokenService
    {
        Task<string> CreateJWTToken(User user);
        string GenerateRefreshToken();
    }
}
