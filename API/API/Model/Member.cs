using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace API.Model
{
    public class Member
    {
        public string Id { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public string? ImageUrl { get; set; }
        public required string Username { get; set; }
        public DateOnly Created { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
        public DateOnly LastActive { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
        public required string Gender { get; set; }
        public string? Description { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }

        //Navigation property to User
        [ForeignKey(nameof(Id))]
        public User User { get; set; } = null!;
        // Navigation property to Photos
        public List<Photo> Photos { get; set; } = [];
    }
}
