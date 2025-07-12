using System.ComponentModel.DataAnnotations;

namespace ReadSynch.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? PasswordHash { get; set; }
        public string? Name { get; set; }
        public HashSet<UserBook> Books { get; set; } = new HashSet<UserBook>();
        
    }
}
