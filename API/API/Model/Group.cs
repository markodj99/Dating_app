using System.ComponentModel.DataAnnotations;

namespace API.Model
{
    public class Group(string name)
    {
        [Key]
        public string Name { get; set; } = name;

        // Nav property
        public ICollection<Connection> Connections { get; set; } = [];
    }
}
