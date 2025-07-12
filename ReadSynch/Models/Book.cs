using System.ComponentModel.DataAnnotations;

namespace ReadSynch.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        public string? ISBN { get; set; }
        [Required]
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Genre { get; set; }
        public string? Description { get; set; }
        public int? PageCount { get; set; }
        public string? CoverImageUrl { get; set; }
    }
}
