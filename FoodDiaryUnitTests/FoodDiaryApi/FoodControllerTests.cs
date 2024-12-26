using Core.Interfaces.Services;
using Core.Models;
using FoodDiaryApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using System.Diagnostics.CodeAnalysis;

namespace FoodDiaryUnitTests.FoodDiaryApi
{
    [ExcludeFromCodeCoverage]
    public class FoodControllerTests
    {
        private readonly Mock<IFoodService> _foodServiceMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<ICalculateStatisticsService> _calculateStatisticsServiceMock;
        private readonly FoodController _controller;

        public FoodControllerTests()
        {
            _foodServiceMock = new Mock<IFoodService>();
            _userServiceMock = new Mock<IUserService>();
            _calculateStatisticsServiceMock = new Mock<ICalculateStatisticsService>();

            _controller = new FoodController(
                _foodServiceMock.Object,
                _userServiceMock.Object,
                _calculateStatisticsServiceMock.Object
            );
        }

        [Fact]
        public async Task AddFood_ShouldReturnCreatedAtAction_WhenFoodIsValid()
        {
            // Arrange
            var food = new Food { Id = 1, Name = "Apple", Calories = 95 };

            _foodServiceMock
                .Setup(service => service.AddFoodAsync(It.IsAny<Food>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddFood(food);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(FoodController.GetAllFoods), createdResult.ActionName);
            Assert.Equal(food, createdResult.Value);
        }

        [Fact]
        public async Task AddFood_ShouldReturnBadRequest_WhenFoodIsNull()
        {
            // Arrange
            Food food = null;

            // Act
            var result = await _controller.AddFood(food);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid food data.", badRequestResult.Value);
        }

        [Fact]
        public async Task GetAllFoods_ShouldReturnOkResult_WithListOfFoods()
        {
            // Arrange
            var foods = new List<Food>
            {
                new Food { Id = 1, Name = "Apple", Calories = 95 },
                new Food { Id = 2, Name = "Banana", Calories = 105 }
            };

            _foodServiceMock
                .Setup(service => service.GetAllFoodsAsync())
                .ReturnsAsync(foods);

            // Act
            var result = await _controller.GetAllFoods();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(foods, okResult.Value);
        }

        [Fact]
        public async Task GetStatistics_ShouldReturnOkResult_WithStatistics_WhenValidInput()
        {
            // Arrange
            var userName = "John";
            var choice = "weekly";
            var user = new User { Name = userName };
            var users = new List<User> { user };
            var statistics = "Sample Statistics";

            _userServiceMock
                .Setup(service => service.LoadUsersAsync())
                .ReturnsAsync(users);

            _calculateStatisticsServiceMock
                .Setup(service => service.CalculateStatistic(user, choice))
                .ReturnsAsync(statistics);

            // Act
            var result = await _controller.GetStatistics(userName, choice);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(statistics, okResult.Value);
        }

        [Fact]
        public async Task GetStatistics_ShouldReturnBadRequest_WhenInputIsInvalid()
        {
            // Arrange
            string userName = null;
            string choice = null;

            // Act
            var result = await _controller.GetStatistics(userName, choice);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid user name or choice.", badRequestResult.Value);
        }

        [Fact]
        public async Task GetStatistics_ShouldReturnNotFound_WhenUserNotFound()
        {
            // Arrange
            var userName = "NonExistentUser";
            var choice = "weekly";
            var users = new List<User>();

            _userServiceMock
                .Setup(service => service.LoadUsersAsync())
                .ReturnsAsync(users);

            // Act
            var result = await _controller.GetStatistics(userName, choice);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User not found.", notFoundResult.Value);
        }
    }
}
