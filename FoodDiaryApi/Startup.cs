using DataBase;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Services.Business;
using Services.Utility;
using System.Diagnostics.CodeAnalysis;

namespace FoodDiaryApi
{
    [ExcludeFromCodeCoverage]
    // главный класс для настройки приложения
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Создание DbContextOptions вручную
            var optionsBuilder = new DbContextOptionsBuilder<FoodDbContext>();
            // Используется база данных SQLite
            optionsBuilder.UseSqlite("Data Source=C:\\Users\\79217\\source\\repos\\FoodDiary\\DataBase\\foodDiary.db");

            // Регистрация фабрики как Singleton
            services.AddSingleton<IFoodDbContextFactory>(provider =>
            {
                return new DbContextFactory(optionsBuilder.Options);
            });

            // Регистрация репозиториев и сервисов как Singleton
            // 1) AddScoped - объект хранит данные, которые должны быть уникальны для каждой операции
            // 2) AddSingleton - Объект создается один раз за время жизни приложения и используется для всех последующих запросов
            // 3) Transient - Новый экземпляр объекта создается каждый раз, когда он запрашивается.
            // Даже в рамках одного запроса, если сервис запрашивается несколько раз, получим разные экземпляры

            // Почему AddSingleton?
            // Приложение консольное. Оно запускается, выполняет задачи и завершается.
            // Все операции выполняются в одном процессе, поэтому нет необходимости создавать новые объекты для каждой операции.
            // Использование одного экземпляра для всех операций повышает производительность, т.к. нет необходимости в создании новых объектов при каждом запросе.
            // Сервисы (например, StatisticsService) не хранят данных, уникальных для каждой операции.
            // Они предоставляют функционал (например, статистика), который можно безопасно использовать многократно. 

            // Использование Singleton снижает нагрузку на систему за счет многократного использования одного объекта для бизнес-логики.
            services.AddSingleton<IFoodRepository, FoodRepository>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IFoodService, FoodService>();
            services.AddSingleton<ICalculateStatisticsService, CalculateStatisticsService>();
            services.AddSingleton<ICalorieCalculator, CalorieCalculator>();

            // Добавляем поддержку контроллеров в приложение
            services.AddControllers();
            // Настраиваем генерацию документации Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FoodDiary API", Version = "v1" });
            });
        }

        // Swagger — это инструмент для автоматической генерации документации и тестирования REST API.
        // Он отображает доступные методы API, параметры, типы данных и возможные ответы
        public void Configure(IApplicationBuilder app)
        {
            // Добавляем поддержку маршрутизаци
            app.UseRouting();
            app.UseAuthorization();
            // Настраиваем конечные точки API (маршруты контроллеров)
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            // Включаем Swagger для документирования API
            app.UseSwagger();
            // Указываем путь к Swagger-документу и задаем параметры интерфейса Swagger
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "FoodDiary API V1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
