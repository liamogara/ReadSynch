using Microsoft.AspNetCore.Mvc;
using Moq;
using ReadSynch.Controllers;
using ReadSynch.Dtos;
using ReadSynch.Interfaces;

namespace ReadSynchTests.Controllers
{
    public class BookSearchControllerTests
    {
        [Fact]
        public async Task BookSearch_ReturnsValidBookList()
        {
            //Arrange
            var mockBookService = new Mock<IBookService>();

            var testBook1 = new BookDto
            {
                GoogleBookId = "1",
                ISBN = "1123456789123",
                Title = "Test1",
                Author = "Author1",
                Genre = "Fiction",
                Description = "Description1",
                PageCount = 100,
                CoverImageUrl = "url1"
            };

            var testBook2 = new BookDto
            {
                GoogleBookId = "2",
                ISBN = "2123456789123",
                Title = "Test2",
                Author = "Author2",
                Genre = "Non-Fiction",
                Description = "Description2",
                PageCount = 100,
                CoverImageUrl = "url2"
            };

            mockBookService.Setup(service => service.Search(It.IsAny<string>()))
                .ReturnsAsync(new List<BookDto>{ testBook1, testBook2 });

            var controller = new BookSearchController(mockBookService.Object);

            //Act
            var result = await controller.BookSearch("testQuery");

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Contains(testBook1, (List<BookDto>)okResult.Value);
            Assert.Contains(testBook2, (List<BookDto>)okResult.Value);
        }
    }
}
