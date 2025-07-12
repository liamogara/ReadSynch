using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReadSynch.Data;
using ReadSynch.Dtos;
using ReadSynch.Models;
using Microsoft.AspNetCore.Authorization;

namespace ReadSynch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ReadSynchContext _context;

        public BooksController(ReadSynchContext context)
        {
            _context = context;
        }

        // GET: api/Books/my-books
        [HttpGet("my-books")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserBookDto>>> GetBooks()
        {
            if (!Int32.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
                return Unauthorized();

            var userBooks = await _context.UserBooks.Where(ub => ub.UserId == userId)
                .Include(ub => ub.Book)
                .ToListAsync();

            var books = userBooks.Select(ub => new UserBookDto
            {
                Id = ub.Id,
                Title = ub.Book.Title,
                Author = ub.Book.Author,
                CoverImageUrl = ub.Book.CoverImageUrl,
                Status = ub.Status,
                IsFavorite = ub.IsFavorite,
                Rating = ub.Rating,
                UserId = ub.UserId,
                BookId = ub.BookId
            }).ToList();

            return Ok(books);
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UserBookDetailsDto>> GetBook(int id)
        {
            if (!Int32.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
                return Unauthorized();

            var userBook = await _context.UserBooks.Include(userBook => userBook.Book)
                .FirstOrDefaultAsync(ub => ub.UserId == userId && ub.BookId == id);

            if (userBook == null)
            {
                return NotFound();
            }

            var book = new UserBookDetailsDto
            {
                Id = userBook.Id,
                Title = userBook.Book.Title,
                Author = userBook.Book.Author,
                Genre = userBook.Book.Genre,
                Description = userBook.Book.Description,
                PageCount = userBook.Book.PageCount,
                CoverImageUrl = userBook.Book.CoverImageUrl,
                Status = userBook.Status,
                IsFavorite = userBook.IsFavorite,
                Rating = userBook.Rating,
                UserId = userBook.UserId,
                BookId = userBook.BookId
            };

            return book;
        }

        // PUT: api/Books/5/status
        [HttpPut("{id}/status")]
        [Authorize]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateDto dto)
        {
            if (dto.Status is null)
                return BadRequest();

            var userBook = await _context.UserBooks.FirstOrDefaultAsync(ub => ub.Id == id);

            if (userBook == null)
            {
                return NotFound();
            }

            userBook.Status = dto.Status;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/Books/5/rating
        [HttpPut("{id}/rating")]
        [Authorize]
        public async Task<IActionResult> UpdateRating(int id, [FromBody] UpdateDto dto)
        {
            if (dto.Rating is null)
                return BadRequest();

            var userBook = await _context.UserBooks.FirstOrDefaultAsync(ub => ub.Id == id);

            if (userBook == null)
            {
                return NotFound();
            }

            userBook.Rating = (int)dto.Rating;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Books
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Book>> PostBook(BookDto bookDto)
        {
            if (!Int32.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
                return Unauthorized();

            Book book = await _context.Books.FirstOrDefaultAsync(b => b.ISBN == bookDto.ISBN);
            if (book == null)
            {
                book = new Book
                {
                    ISBN = bookDto.ISBN,
                    Title = bookDto.Title,
                    Author = bookDto.Author,
                    Genre = bookDto.Genre,
                    Description = bookDto.Description,
                    PageCount = bookDto.PageCount,
                    CoverImageUrl = bookDto.CoverImageUrl
                };

                _context.Books.Add(book);
                await _context.SaveChangesAsync();
            }

            UserBook userBook;
            if (await _context.UserBooks.AnyAsync(b => b.BookId == book.Id && b.UserId == userId))
            {
                return BadRequest();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            userBook = new UserBook
            {
                Status = "Not Read",
                UserId = user.Id,
                BookId = book.Id
            };
            user.Books.Add(userBook);
            _context.UserBooks.Add(userBook);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.Id }, book);
        }

        // DELETE: api/Books
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteBook(UserBookDto userBookDto)
        {
            var userBook = await _context.UserBooks.FirstOrDefaultAsync(ub => ub.BookId == userBookDto.BookId);
            if (userBook == null)
            {
                return NotFound();
            }
            _context.UserBooks.Remove(userBook);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
