using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class PhotoDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Url { get; set; } = "";
        public string? PublicId { get; set; }
        [Required]
        public string MemberId { get; set; } = null!;
        [Required]
        public bool IsApproved { get; set; }
    }
}
