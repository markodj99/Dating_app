namespace API.Model
{
    public class Photo
    {
        public required int Id { get; set; }
        public required string Url { get; set; }
        public string? PublicId { get; set; }

        // Navigation property to Member
        public Member Member { get; set; } = null!;
    }
}
