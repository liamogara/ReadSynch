namespace ReadSynch.Dtos
{
    public class UserBookDetailsDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Genre { get; set; }
        public string? Description { get; set; }
        public int? PageCount { get; set; }
        public string? CoverImageUrl { get; set; }
        public string? Status { get; set; }
        public bool IsFavorite { get; set; }
        public int Rating { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
    }
}
