namespace ReadSynch.Dtos
{
    public class BookDto
    {
        public string? GoogleBookId { get; set; }
        public string? ISBN { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Genre { get; set; }
        public string? Description { get; set; }
        public int? PageCount { get; set; }
        public string? CoverImageUrl { get; set; }
    }
}
