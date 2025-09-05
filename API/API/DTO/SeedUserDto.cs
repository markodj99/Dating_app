using API.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.DTO
{
    public class SeedUserDto
    {
        public required string Id { get; set; }
        public required string Email { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string? ImageUrl { get; set; }
        public required string Username { get; set; }
        public DateOnly Created { get; set; }
        public DateOnly LastActive { get; set; }
        public required string Gender { get; set; }
        public string? Description { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
    }
}
