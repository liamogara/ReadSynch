namespace ReadSynch.Dtos
{
    public class GoogleBooksResponseDto
    {
        public List<Item>? Items { get; set; }
    }

    public class Item
    {
        public string? Id { get; set; }
        public VolumeInfo? VolumeInfo { get; set; }
    }
    public class VolumeInfo
    {
        public string? Title { get; set; }
        public List<string>? Authors { get; set; }
        public string? Description { get; set; }
        public List<IndustryIdentifier>? IndustryIdentifiers { get; set; }
        public int? PageCount { get; set; }
        public List<string>? Categories { get; set; }
        public ImageLinks? ImageLinks { get; set; }
    }

    public class IndustryIdentifier
    {
        public string? Type { get; set; }
        public string? Identifier { get; set; }

    }

    public class ImageLinks
    {
        public string? Thumbnail { get; set; }
    }
}
