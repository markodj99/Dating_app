using API.Model;
using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class MemberDto
    {
        [Required]
        public string Id { get; set; } = "";
        [Required]
        public DateOnly DateOfBirth { get; set; }
        public string? ImageUrl { get; set; }
        [Required]
        public string Username { get; set; } = "";
        [Required]
        public DateTime Created { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        [Required]
        public string Gender { get; set; } = "";
        [Required]
        public string? Description { get; set; }
        [Required]
        public string City { get; set; } = "";
        [Required]
        public  string Country { get; set; } = "";
        [Required]
        public List<Photo> Photos { get; set; } = [];
    }
}
