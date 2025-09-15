using Microsoft.AspNetCore.Identity;

namespace API.Model
{
    public class User : IdentityUser
    {
        public string? ImageUrl { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpire { get; set; }

        // Navigation property to Member
        public Member Member { get; set; } = null!;
    }
}
