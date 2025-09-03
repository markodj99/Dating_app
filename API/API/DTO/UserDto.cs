using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class UserDto
    {
        [Required]
        public required string Id { get; set; }
        [Required]
        public required string Username { get; set; }
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string Token { get; set; }
        public string? ImageUrl { get; set; }
    }
}
