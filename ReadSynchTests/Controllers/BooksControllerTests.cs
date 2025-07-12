using Microsoft.EntityFrameworkCore;
using ReadSynch.Controllers;
using ReadSynch.Data;
using ReadSynch.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReadSynch.Dtos;

namespace ReadSynchTests.Controllers
{
    public class BooksControllerTests
    {
        [Fact]
        public async Task GetBooks_ReturnsUserBooks()
        {
            //Assert
            var context = GetContext();
            var user = new User { Username = "testUser", PasswordHash = "testHash" };
            var book1 = new Book()
            {
                ISBN = "1123456789123",
                Title = "Test1",
                Author = "Author",
                Genre = "Fiction",
                Description = "Description",
                PageCount = 100,
                CoverImageUrl = "url"
            };
            var book2 = new Book()
            {
                ISBN = "2123456789123",
                Title = "Test2",
                Author = "Author",
                Genre = "Fiction",
                Description = "Description",
                PageCount = 100,
                CoverImageUrl = "url"
            };
            var userBook1 = new UserBook
            {
                Status = "Not Read",
                UserId = user.Id,
                BookId = book1.Id,
                Book = book1
            };
            var userBook2 = new UserBook
            {
                Status = "Not Read",
                UserId = user.Id,
                BookId = book2.Id,
                Book = book2
            };
            user.Books.Add(userBook1);
            user.Books.Add(userBook2);

            context.Users.Add(user);
            context.Books.Add(book1);
            context.Books.Add(book2);
            context.UserBooks.Add(userBook1);
            context.UserBooks.Add(userBook2);
            await context.SaveChangesAsync();

            var controller = GetControllerWithUser(1, context);

            //Act
            var result = await controller.GetBooks();

            //Arrange
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var booksResults = (IEnumerable<UserBookDto>)okResult.Value;
            Assert.Equal(2, booksResults.Count());
            Assert.Equal("Test1", booksResults.First().Title);
        }

        [Fact]
        public async Task GetBook_ReturnsBookDetails()
        {
            //Assert
            var context = GetContext();
            var user = new User { Username = "testUser", PasswordHash = "testHash" };
            var book = new Book()
            {
                ISBN = "1123456789123",
                Title = "Test",
                Author = "Author",
                Genre = "Fiction",
                Description = "Description",
                PageCount = 100,
                CoverImageUrl = "url"
            };
            var userBook = new UserBook
            {
                Status = "Not Read",
                UserId = user.Id,
                BookId = book.Id,
                Book = book
            };
            user.Books.Add(userBook);

            context.Users.Add(user);
            context.Books.Add(book);
            context.UserBooks.Add(userBook);
            await context.SaveChangesAsync();

            var controller = GetControllerWithUser(1, context);

            //Act
            var result = await controller.GetBook(book.Id);

            //Arrange
            var bookResult = Assert.IsType<UserBookDetailsDto>(result.Value);
            Assert.Equal("Test", bookResult.Title);
        }

        [Fact]
        public async Task GetBook_ReturnsNotFound_ForInvalidBook()
        {
            //Assert
            var context = GetContext();
            var user = new User { Username = "testUser", PasswordHash = "testHash" };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            var controller = GetControllerWithUser(1, context);

            //Act
            var result = await controller.GetBook(100);

            //Arrange
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task UpdateStatus_UpdatesValidBook()
        {
            //Assert
            var context = GetContext();
            var user = new User { Username = "testUser", PasswordHash = "testHash" };

            var book = new Book()
            {
                ISBN = "1123456789123",
                Title = "Test",
                Author = "Author",
                Genre = "Fiction",
                Description = "Description",
                PageCount = 100,
                CoverImageUrl = "url"
            };
            var userBook = new UserBook
            {
                Status = "Not Read",
                UserId = user.Id,
                BookId = book.Id,
                Book = book
            };
            user.Books.Add(userBook);

            context.Users.Add(user);
            context.Books.Add(book);
            context.UserBooks.Add(userBook);
            await context.SaveChangesAsync();

            var controller = GetControllerWithUser(1, context);
            var updatedStatus = new UpdateDto { Status = "Reading" };

            //Act
            var result = await controller.UpdateStatus(userBook.Id, updatedStatus);

            //Arrange
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(updatedStatus.Status, context.UserBooks.FirstOrDefault(ub => ub.Id == userBook.Id).Status);
        }

        [Fact]
        public async Task UpdateStatus_ReturnsNotFound_ForInvalidBook()
        {
            //Assert
            var context = GetContext();
            var user = new User { Username = "testUser", PasswordHash = "testHash" };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            var controller = GetControllerWithUser(1, context);
            var updatedStatus = new UpdateDto { Status = "Reading" };

            //Act
            var result = await controller.UpdateStatus(100, updatedStatus);

            //Arrange
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateRating_UpdatesValidBook()
        {
            //Assert
            var context = GetContext();
            var user = new User { Username = "testUser", PasswordHash = "testHash" };

            var book = new Book()
            {
                ISBN = "1123456789123",
                Title = "Test",
                Author = "Author",
                Genre = "Fiction",
                Description = "Description",
                PageCount = 100,
                CoverImageUrl = "url"
            };
            var userBook = new UserBook
            {
                Status = "Not Read",
                UserId = user.Id,
                BookId = book.Id,
                Book = book
            };
            user.Books.Add(userBook);

            context.Users.Add(user);
            context.Books.Add(book);
            context.UserBooks.Add(userBook);
            await context.SaveChangesAsync();

            var controller = GetControllerWithUser(1, context);
            var updatedRating = new UpdateDto { Rating = 4 };

            //Act
            var result = await controller.UpdateRating(userBook.Id, updatedRating);

            //Arrange
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(updatedRating.Rating, context.UserBooks.FirstOrDefault(ub => ub.Id == userBook.Id).Rating);
        }

        [Fact]
        public async Task UpdateRating_ReturnsNotFound_ForInvalidBook()
        {
            //Assert
            var context = GetContext();
            var user = new User { Username = "testUser", PasswordHash = "testHash" };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            var controller = GetControllerWithUser(1, context);
            var updatedRating = new UpdateDto { Rating = 4 };

            //Act
            var result = await controller.UpdateRating(100, updatedRating);

            //Arrange
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task PostBook_AddsValidBook()
        {
            //Assert
            var context = GetContext();
            var user = new User { Username = "testUser", PasswordHash = "testHash" };
            var testBook = new BookDto
            {
                GoogleBookId = "1",
                ISBN = "1123456789123",
                Title = "Test",
                Author = "Author",
                Genre = "Fiction",
                Description = "Description",
                PageCount = 100,
                CoverImageUrl = "url"
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            var controller = GetControllerWithUser(1, context);

            //Act
            var result = await controller.PostBook(testBook);

            //Arrange
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var bookResult = Assert.IsType<Book>(createdResult.Value);
            Assert.Equal("Test", bookResult.Title);
        }

        [Fact]
        public async Task PostBook_DoesNotAddDuplicateBook()
        {
            //Assert
            var context = GetContext();
            var user = new User { Username = "testUser", PasswordHash = "testHash" };
            
            var book = new Book()
            {
                ISBN = "1123456789123",
                Title = "Test",
                Author = "Author",
                Genre = "Fiction",
                Description = "Description",
                PageCount = 100,
                CoverImageUrl = "url"
            };
            var userBook = new UserBook
            {
                Status = "Not Read",
                UserId = user.Id,
                BookId = book.Id,
                Book = book
            };
            user.Books.Add(userBook);

            context.Users.Add(user);
            context.Books.Add(book);
            context.UserBooks.Add(userBook);
            await context.SaveChangesAsync();

            var testBook = new BookDto
            {
                GoogleBookId = "1",
                ISBN = "1123456789123",
                Title = "Test",
                Author = "Author",
                Genre = "Fiction",
                Description = "Description",
                PageCount = 100,
                CoverImageUrl = "url"
            };

            var controller = GetControllerWithUser(1, context);

            //Act
            var result = await controller.PostBook(testBook);

            //Arrange
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task DeleteBook_DeletesUserBook()
        {
            //Assert
            var context = GetContext();
            var user = new User { Username = "testUser", PasswordHash = "testHash" };

            var book = new Book()
            {
                ISBN = "1123456789123",
                Title = "Test",
                Author = "Author",
                Genre = "Fiction",
                Description = "Description",
                PageCount = 100,
                CoverImageUrl = "url"
            };
            var userBook = new UserBook
            {
                Status = "Not Read",
                UserId = user.Id,
                BookId = book.Id,
                Book = book
            };
            user.Books.Add(userBook);

            context.Users.Add(user);
            context.Books.Add(book);
            context.UserBooks.Add(userBook);
            await context.SaveChangesAsync();

            var testBook = new UserBookDto
            {
                BookId = 1
            };

            var controller = GetControllerWithUser(1, context);

            //Act
            var result = await controller.DeleteBook(testBook);

            //Arrange
            Assert.IsType<NoContentResult>(result);
            Assert.Empty(context.UserBooks.Where(ub => ub.Id == testBook.Id));
        }

        [Fact]
        public async Task DeleteBook_ReturnsNotFound_ForInvalidBook()
        {
            //Assert
            var context = GetContext();
            var user = new User { Username = "testUser", PasswordHash = "testHash" };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            var testBook = new UserBookDto
            {
                BookId = 1
            };

            var controller = GetControllerWithUser(1, context);

            //Act
            var result = await controller.DeleteBook(testBook);

            //Arrange
            Assert.IsType<NotFoundResult>(result);
        }

        private static BooksController GetControllerWithUser(int userId, ReadSynchContext context)
        {
            var controller = new BooksController(context);
            var user = new ClaimsPrincipal(new ClaimsIdentity
            (new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                }
            ));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
            return controller;
        }

        private static ReadSynchContext GetContext()
        {
            var testDbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<ReadSynchContext>()
                .UseInMemoryDatabase(databaseName: testDbName)
                .Options;
            return new ReadSynchContext(options);
        }
    }
}
