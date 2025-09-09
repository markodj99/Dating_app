using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class RegisterDto
    {
        [Required]
        [MinLength(4)]
        [MaxLength(20)]
        public string Username { get; set; } = "";

        [Required]
        [EmailAddress]
        [MinLength(4)]
        public string Email { get; set; } = "";

        [Required]
        [MinLength(4)]
        [MaxLength(20)]
        public string Password { get; set; } = "";

        [Required]
        [MinLength(4)]
        [MaxLength(20)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = "";

        [Required]
        public string Gender { get; set; } = "";

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string City { get; set; } = "";

        [Required]
        [MinLength(2    )]
        [MaxLength(20)]
        public string Country { get; set; } = "";
    }
}
