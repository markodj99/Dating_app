using API.Model;
using System.Security.Claims;

namespace API.Extension
{
    public static class ClaimsPrincipalExtension
    {
        public static string GetMemberId(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new Exception("Can not get memberId from ClaimsPrincipal");
        }
    }
}
