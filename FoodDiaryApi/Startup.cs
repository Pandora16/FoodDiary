using DataBase;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Services.Business;
using Services.Utility;

namespace FoodDiaryApi
{
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
            optionsBuilder.UseSqlite("Data Source=C:\\Users\\cemad\\source\\repos\\FoodDiary\\DataBase\\foodDiary.db");

            // Регистрация фабрики как Singleton
            services.AddSingleton<IFoodDbContextFactory>(provider =>
            {
                return new DbContextFactory(optionsBuilder.Options);
            });

            // Регистрация репозиториев и сервисов как Singleton
            services.AddSingleton<IFoodRepository, FoodRepository>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IFoodService, FoodService>();
            services.AddSingleton<ICalculateStatisticsService, CalculateStatisticsService>();
            services.AddSingleton<ICalorieCalculator, CalorieCalculator>();

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FoodDiary API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "FoodDiary API V1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
