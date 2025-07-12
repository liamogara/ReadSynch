using ReadSynch.Dtos;
using ReadSynch.Interfaces;

namespace ReadSynch.Services
{
    public class GoogleBookService : IBookService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GoogleBookService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://www.googleapis.com");
            _apiKey = configuration["ApiKeys:GoogleBooks"];
        }

        public async Task<IEnumerable<BookDto>> Search(string query)
        {
            var searchParam = $"books/v1/volumes?q={query}&key={_apiKey}";
            var response = await _httpClient.GetAsync(searchParam);
            
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseData = await response.Content.ReadFromJsonAsync<GoogleBooksResponseDto>();

            if (responseData.Items == null)
                return null;

            return responseData.Items.Select(book => new BookDto
            {
                GoogleBookId = book.Id,
                ISBN = book.VolumeInfo.IndustryIdentifiers?
                    .FirstOrDefault(i => i.Type == "ISBN_13")?.Identifier,
                Title = book.VolumeInfo.Title,
                Author = book.VolumeInfo.Authors?.FirstOrDefault(),
                Genre = book.VolumeInfo.Categories?.FirstOrDefault(),
                Description = book.VolumeInfo.Description,
                PageCount = book.VolumeInfo.PageCount,
                CoverImageUrl = book.VolumeInfo.ImageLinks?.Thumbnail
            });
        }
    }
}
