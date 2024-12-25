using System.Diagnostics.CodeAnalysis;
using Core.Models;
using DataBase;
using Microsoft.EntityFrameworkCore;
using Xunit;

[ExcludeFromCodeCoverage]
public class FoodDbContextTests
{
    [Fact]
    public void OnModelCreating_CreatesTablesCorrectly()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<FoodDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        using var context = new FoodDbContext(options);

        // Act
        var usersEntity = context.Model.FindEntityType(typeof(User));
        var foodsEntity = context.Model.FindEntityType(typeof(Food));

        // Assert
        Assert.NotNull(usersEntity);
        Assert.Equal("Users", usersEntity.GetTableName());

        Assert.NotNull(foodsEntity);
        Assert.Equal("Foods", foodsEntity.GetTableName());
    }
}
