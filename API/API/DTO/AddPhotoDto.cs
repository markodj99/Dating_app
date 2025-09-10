using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class AddPhotoDto
    {
        [Required]
        public IFormFile File { get; set; } = default!;
    }
}
