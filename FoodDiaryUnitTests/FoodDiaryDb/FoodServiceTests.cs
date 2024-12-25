using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Core.Interfaces.Repositories;
using Core.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Services.Business;
using Xunit;

[ExcludeFromCodeCoverage]
public class FoodServiceTests
{
    [Fact]
    public async Task AddFoodAsync_ValidFood_AddsFood()
    {
        // Arrange
        var foodRepositoryMock = new Mock<IFoodRepository>();
        var loggerMock = new Mock<ILogger<FoodService>>();
        var foodService = new FoodService(foodRepositoryMock.Object, loggerMock.Object);

        var food = new Food { Name = "Apple", Calories = 95, UserName = "test_user" };

        foodRepositoryMock
            .Setup(repo => repo.UserExistsAsync(food.UserName))
            .ReturnsAsync(true);

        // Act
        await foodService.AddFoodAsync(food);

        // Assert
        foodRepositoryMock.Verify(repo => repo.AddFoodAsync(food), Times.Once);
    }

    [Fact]
    public async Task AddFoodAsync_UserDoesNotExist_ThrowsException()
    {
        // Arrange
        var foodRepositoryMock = new Mock<IFoodRepository>();
        var loggerMock = new Mock<ILogger<FoodService>>();
        var foodService = new FoodService(foodRepositoryMock.Object, loggerMock.Object);

        var food = new Food { Name = "Apple", Calories = 95, UserName = "unknown_user" };

        foodRepositoryMock
            .Setup(repo => repo.UserExistsAsync(food.UserName))
            .ReturnsAsync(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => foodService.AddFoodAsync(food));
        Assert.Equal("Пользователь с именем unknown_user не существует.", exception.Message);
    }

    [Fact]
    public async Task GetAllFoodsAsync_ReturnsAllFoods()
    {
        // Arrange
        var foodRepositoryMock = new Mock<IFoodRepository>();
        var loggerMock = new Mock<ILogger<FoodService>>();
        var foodService = new FoodService(foodRepositoryMock.Object, loggerMock.Object);

        var foods = new List<Food>
        {
            new Food { Name = "Apple", Calories = 95 },
            new Food { Name = "Banana", Calories = 105 }
        };

        foodRepositoryMock
            .Setup(repo => repo.GetAllFoodsAsync())
            .ReturnsAsync(foods);

        // Act
        var result = await foodService.GetAllFoodsAsync();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, f => f.Name == "Apple");
        Assert.Contains(result, f => f.Name == "Banana");
    }
}
