namespace ReadSynch.Dtos
{
    public class UserBookDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? CoverImageUrl { get; set; }
        public string? Status { get; set; }
        public bool IsFavorite { get; set; }
        public int Rating { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
    }
}
