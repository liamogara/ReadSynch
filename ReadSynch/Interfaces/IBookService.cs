using ReadSynch.Dtos;

namespace ReadSynch.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> Search(string query);
    }
}
