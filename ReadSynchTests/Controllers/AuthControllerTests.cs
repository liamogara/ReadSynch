using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ReadSynch.Controllers;
using ReadSynch.Data;
using ReadSynch.Dtos;
using ReadSynch.Models;
using Microsoft.AspNetCore.Mvc;

namespace ReadSynchTests.Controllers
{
    public class AuthControllerTests
    {
        [Fact]
        public async Task Login_ReturnsToken_ForValidUser()
        {
            //Arrange
            var user = "testUser";
            var pass = "testPass";

            var hasher = new PasswordHasher<string>();
            var hashedPass = hasher.HashPassword(user, pass);

            var context = GetContextWithUser(user, hashedPass);
            var config = GetJwtConfig();
            var controller = new AuthController(context, config);

            //Act
            var result = await controller.Login(new LoginDto
            {
                Username = user,
                Password = pass
            });

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Contains("token", okResult.Value.ToString());
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_ForInvalidUser()
        {
            //Arrange
            var user = "testUser";
            var pass = "testPass";

            var hasher = new PasswordHasher<string>();
            var hashedPass = hasher.HashPassword(user, pass);

            var context = GetContextWithUser(user, hashedPass);
            var config = GetJwtConfig();
            var controller = new AuthController(context, config);

            //Act
            var result = await controller.Login(new LoginDto
            {
                Username = "invalidUser",
                Password = pass
            });

            //Assert
            Assert.IsType<UnauthorizedResult>(result.Result);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_ForIncorrectPassword()
        {
            //Arrange
            var user = "testUser";
            var pass = "testPass";

            var hasher = new PasswordHasher<string>();
            var hashedPass = hasher.HashPassword(user, pass);

            var context = GetContextWithUser(user, hashedPass);
            var config = GetJwtConfig();
            var controller = new AuthController(context, config);

            //Act
            var result = await controller.Login(new LoginDto
            {
                Username = user,
                Password = "incorrectPass"
            });

            //Assert
            Assert.IsType<UnauthorizedResult>(result.Result);
        }

        private static ReadSynchContext GetContextWithUser(string username, string passwordHash)
        {
            var testDbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<ReadSynchContext>()
                .UseInMemoryDatabase(databaseName: testDbName)
                .Options;
            var context = new ReadSynchContext(options);
            context.Users.Add(new User
            {
                Username = username,
                PasswordHash = passwordHash,
                Id = 1
            });
            context.SaveChanges();
            return context;
        }

        private static IConfiguration GetJwtConfig()
        {
            var settings = new Dictionary<string, string>
            {
                { "Jwt:Key", "testKey123456789012345678901234567890" },
                { "Jwt:Issuer", "testIssuer" },
                { "Jwt:Audience", "testAudience" }
            };
            return new ConfigurationBuilder()
                .AddInMemoryCollection(settings)
                .Build();
        }
    }
}
