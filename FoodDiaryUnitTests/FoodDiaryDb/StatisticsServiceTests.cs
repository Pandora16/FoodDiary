using Core.Interfaces.Services;
using Core.Interfaces.UI;
using Core.Models;
using Moq;
using Services.Business;
using System.Diagnostics.CodeAnalysis;
using Xunit;

[ExcludeFromCodeCoverage]
public class StatisticsServiceTests
{
    [Fact]
    public async Task ShowStatisticsAsync_ValidInput_CallsCalculateStatistic()
    {
        // Arrange
        var user = new User { TargetCalories = 2000 };
        var calculateStatisticsMock = new Mock<ICalculateStatisticsService>();
        calculateStatisticsMock
            .Setup(service => service.CalculateStatistic(user, "1"))
            .ReturnsAsync("Статистика за день: 1500 ккал.");

        var userInterfaceMock = new Mock<IUserInterface>();
        userInterfaceMock.Setup(ui => ui.ReadInputAsync()).ReturnsAsync("1");

        var statisticsService = new StatisticsService(userInterfaceMock.Object, calculateStatisticsMock.Object);

        // Act
        await statisticsService.ShowStatisticsAsync(user);

        // Assert
        userInterfaceMock.Verify(ui => ui.WriteMessageAsync(It.IsAny<string>()), Times.AtLeastOnce);
        calculateStatisticsMock.Verify(service => service.CalculateStatistic(user, "1"), Times.Once);
    }

    [Fact]
    public async Task ShowStatisticsAsync_InvalidChoice_ShowsErrorMessage()
    {
        // Arrange
        var user = new User();
        var calculateStatisticsMock = new Mock<ICalculateStatisticsService>();
        var userInterfaceMock = new Mock<IUserInterface>();
        userInterfaceMock.SetupSequence(ui => ui.ReadInputAsync())
            .ReturnsAsync("invalid")
            .ReturnsAsync("1");

        var statisticsService = new StatisticsService(userInterfaceMock.Object, calculateStatisticsMock.Object);

        // Act
        await statisticsService.ShowStatisticsAsync(user);

        // Assert
        userInterfaceMock.Verify(ui => ui.WriteMessageAsync("Ошибка! Пожалуйста, введите число от 1 до 3."), Times.Once);
    }
}
