using ReadSynch.Dtos;
using ReadSynch.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ReadSynch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookSearchController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookSearchController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // GET: api/BookSearch
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<BookDto>>> BookSearch(string query)
        {
            var result = await _bookService.Search(query);

            return Ok(result);
        }
    }
}
