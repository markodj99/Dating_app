using API.Model;

namespace API.Interface
{
    public interface ITokenService
    {
        string CreateJWTToken(User user);
    }
}
