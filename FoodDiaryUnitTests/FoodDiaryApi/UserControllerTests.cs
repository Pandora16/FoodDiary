using Core.Interfaces.Services;
using Core.Models;
using FoodDiaryApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace FoodDiaryUnitTests.FoodDiaryApi
{
    [ExcludeFromCodeCoverage]
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _controller = new UserController(_userServiceMock.Object);
        }

        [Fact]
        public async Task AddUser_ShouldReturnCreatedAtAction_WhenUserIsValid()
        {
            // Arrange
            var user = new User { Id = 1, Name = "John", Age = 30 };

            _userServiceMock
                .Setup(service => service.AddUserAsync(It.IsAny<User>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddUser(user);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(UserController.LoadUsers), createdResult.ActionName);
            Assert.Equal(user, createdResult.Value);
        }

        [Fact]
        public async Task AddUser_ShouldReturnBadRequest_WhenUserIsNull()
        {
            // Arrange
            User user = null;

            // Act
            var result = await _controller.AddUser(user);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid user data.", badRequestResult.Value);
        }

        [Fact]
        public async Task LoadUsers_ShouldReturnOkResult_WithListOfUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Name = "John", Age = 30 },
                new User { Id = 2, Name = "Jane", Age = 25 }
            };

            _userServiceMock
                .Setup(service => service.LoadUsersAsync())
                .ReturnsAsync(users);

            // Act
            var result = await _controller.LoadUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(users, okResult.Value);
        }

        [Fact]
        public async Task LoadUsers_ShouldReturnOkResult_WithEmptyList_WhenNoUsersExist()
        {
            // Arrange
            var users = new List<User>();

            _userServiceMock
                .Setup(service => service.LoadUsersAsync())
                .ReturnsAsync(users);

            // Act
            var result = await _controller.LoadUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(users, okResult.Value);
        }
    }
}
