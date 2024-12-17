using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Interfaces.UI;
using Core.Models;

namespace Services.Business
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IUserInterface _userInterface;
        private readonly ICalculateStatisticsService _calculateStatisticsService;

        public StatisticsService(IUserInterface userInterface, ICalculateStatisticsService calculateStatisticsService)
        {
            _userInterface = userInterface;
            _calculateStatisticsService = calculateStatisticsService;
        }

        public async Task ShowStatisticsAsync(User user)
        {
            string choice;
            while (true)
            {
                await _userInterface.WriteMessageAsync("\nВыберите период для отображения статистики:");
                await _userInterface.WriteMessageAsync("1. За день");
                await _userInterface.WriteMessageAsync("2. За неделю");
                await _userInterface.WriteMessageAsync("3. За месяц");
                await _userInterface.WriteMessageAsync("Ваш выбор (1-3): ");

                choice = await _userInterface.ReadInputAsync();

                if (choice == "1" || choice == "2" || choice == "3")
                {
                    break;
                }
                else
                {
                    await _userInterface.WriteMessageAsync("Ошибка! Пожалуйста, введите число от 1 до 3.");
                }
            }

            await _userInterface.WriteMessageAsync(await _calculateStatisticsService.CalculateStatistic(user, choice));
        }
    }
}
