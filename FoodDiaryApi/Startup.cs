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
            services.AddDbContext<FoodDbContext>(options =>
                options.UseSqlite("Data Source=C:\\Users\\cemad\\source\\repos\\FoodDiary\\DataBase\\foodDiary.db"));

            services.AddScoped<IFoodRepository, FoodRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFoodService, FoodService>();
            services.AddScoped<ICalculateStatisticsService, CalculateStatisticsService>();
            services.AddScoped<ICalorieCalculator, CalorieCalculator>();

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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shopper API V1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
