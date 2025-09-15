using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.Model
{
    public class Member
    {
        public string Id { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public string? ImageUrl { get; set; }
        public required string UserName { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public required string Gender { get; set; }
        public string? Description { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }

        //Navigation property to User
        [ForeignKey(nameof(Id))]
        //[JsonIgnore] ovo isto moze
        public User User { get; set; } = null!;
        // Navigation property to Photos
        public List<Photo> Photos { get; set; } = [];

        [JsonIgnore]
        public List<MemberLike> LikedByMembers { get; set; } = [];
        [JsonIgnore]
        public List<MemberLike> LikedMembers { get; set; } = [];

        [JsonIgnore]
        public List<Message> MessagesSent { get; set; } = [];
        [JsonIgnore]
        public List<Message> MessagesReceived { get; set; } = [];
    }
}
