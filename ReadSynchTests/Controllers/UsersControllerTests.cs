using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReadSynch.Controllers;
using ReadSynch.Data;
using ReadSynch.Dtos;
using ReadSynch.Models;

namespace ReadSynchTests.Controllers
{
    public class UsersControllerTests
    {
        [Fact]
        public async Task GetUsers_ReturnsCorrectAmountOfUsers()
        {
            //Assert
            var context = GetContext();
            context.Users.Add(new User { Username = "testUser1", PasswordHash = "testHash1" });
            context.Users.Add(new User { Username = "testUser1", PasswordHash = "testHash1" });
            await context.SaveChangesAsync();

            var controller = new UsersController(context);

            //Act
            var result = await controller.GetUsers();

            //Arrange
            Assert.Equal(2, result.Value.Count());
        }

        [Fact]
        public async Task GetUser_ReturnsCorrectUser()
        {
            //Assert
            var context = GetContext();
            var user = new User { Username = "testUser1", PasswordHash = "testHash1" };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var controller = new UsersController(context);

            //Act
            var result = await controller.GetUser(user.Id);

            //Arrange
            Assert.Equal(user.Name, result.Value.Name);
        }

        [Fact]
        public async Task GetUser_ReturnsNotFound_ForInvalidUser()
        {
            //Assert
            var context = GetContext();

            var controller = new UsersController(context);

            //Act
            var result = await controller.GetUser(100);

            //Arrange
            Assert.IsType<NotFoundResult>(result.Result);

        }

        [Fact]
        public async Task PostUser_AddsValidUser()
        {
            //Assert
            const string testName = "test";
            const string testUsername = "testUser";
            const string testPassword = "testPass";
            var testUserDto = new UserDto
            {
                Name = testName,
                Username = testUsername,
                Password = testPassword
            };
            var context = GetContext();

            var controller = new UsersController(context);

            //Act
            var result = await controller.PostUser(testUserDto);

            //Arrange
            var created = Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdUser = Assert.IsType<User>(created.Value);
            Assert.Equal(testName, createdUser.Name);
        }

        [Fact]
        public async Task PostUser_DoesNotAddDuplicateUser()
        {
            //Assert
            const string testName = "test";
            const string testUsername = "testUser";
            const string testPassword = "testPass";
            var testUserDto = new UserDto
            {
                Name = testName,
                Username = testUsername,
                Password = testPassword
            };

            var context = GetContext();
            context.Users.Add(new User
            {
                Username = testUsername,
                PasswordHash = testPassword
            });
            await context.SaveChangesAsync();

            var controller = new UsersController(context);

            //Act
            var result = await controller.PostUser(testUserDto);

            //Arrange
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task DeleteUser_DeletesUser()
        {
            //Assert
            const string testName = "test";
            const string testUsername = "testUser";
            const string testPassword = "testPass";
            var user = new User
            {
                Name = testName,
                Username = testUsername,
                PasswordHash = testPassword
            };

            var context = GetContext();
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var controller = new UsersController(context);

            //Act
            var result = await controller.DeleteUser(user.Id);

            //Arrange
            Assert.IsType<NoContentResult>(result);
            Assert.Empty(context.Users.Where(u => u.Id == user.Id));
        }

        [Fact]
        public async Task DeleteUser_ReturnsNotFound_ForInvalidUser()
        {
            //Assert
            var context = GetContext();

            var controller = new UsersController(context);

            //Act
            var result = await controller.DeleteUser(100);

            //Arrange
            Assert.IsType<NotFoundResult>(result);
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
