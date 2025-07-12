using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using ReadSynch.Dtos;
using ReadSynch.Services;

namespace ReadSynchTests.Services
{
    public class GoogleBookServiceTests
    {
        [Fact]
        public async Task Search_ReturnsBooks_WhenApiResponseIsValid()
        {
            // Arrange
            var expectedBook = new GoogleBooksResponseDto
            {
                Items = new List<Item>
                {
                    new ()
                    {
                        Id = "1",
                        VolumeInfo = new VolumeInfo
                        {
                            Title = "Test",
                            Authors = new List<string> { "Author" },
                            Categories = new List<string> { "Fiction" },
                            Description = "Description",
                            PageCount = 100,
                            ImageLinks = new ImageLinks { Thumbnail = "url" },
                            IndustryIdentifiers = new List<IndustryIdentifier>()
                            {
                                new () { Type = "ISBN_13", Identifier = "0123456789123" }
                            }
                        }
                    }
                }
            };

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(expectedBook))
                });

            var httpClient = new HttpClient(handlerMock.Object);
            var configMock = new Mock<IConfiguration>();
            configMock.Setup(c => c["ApiKeys:GoogleBooks"]).Returns("test-key");

            var service = new GoogleBookService(httpClient, configMock.Object);

            // Act
            var result = await service.Search("test");

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Test", result.First().Title);
            Assert.Equal(100, result.First().PageCount);
        }

        [Fact]
        public async Task Search_ReturnsNull_WhenApiResponseIsInvalid()
        {
            // Arrange
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest
                });

            var httpClient = new HttpClient(handlerMock.Object);
            var configMock = new Mock<IConfiguration>();
            configMock.Setup(c => c["ApiKeys:GoogleBooks"]).Returns("test-key");

            var service = new GoogleBookService(httpClient, configMock.Object);

            // Act
            var result = await service.Search("test");

            // Assert
            Assert.Null(result);
        }
    }
}
