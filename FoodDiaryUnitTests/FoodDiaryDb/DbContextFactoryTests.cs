using Microsoft.EntityFrameworkCore;
using DataBase;
using Xunit;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class DbContextFactoryTests
{
    [Fact]
    public void CreateDbContext_ReturnsNewInstance()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<FoodDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;
        var factory = new DbContextFactory(options);

        // Act
        var context1 = factory.CreateDbContext();
        var context2 = factory.CreateDbContext();

        // Assert
        Assert.NotNull(context1);
        Assert.NotNull(context2);
        Assert.NotSame(context1, context2); // Each call should return a new instance
    }
}
