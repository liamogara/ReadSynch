using System.ComponentModel.DataAnnotations;

namespace ReadSynch.Models
{
    public class UserBook
    {
        public int Id { get; set; }
        public string? Status { get; set; }
        public bool IsFavorite { get; set; } = false;
        public int Rating { get; set; } = 0;
        [Required]
        public int UserId { get; set; }
        [Required]
        public int BookId { get; set; }

        public Book Book { get; set; }
    }
}
