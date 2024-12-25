using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DataBase;
using Core.Interfaces.Repositories;
using System;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class TestSetup
{
    public ServiceProvider ServiceProvider { get; private set; }

    public TestSetup()
    {
        var services = new ServiceCollection();

        // Регистрация зависимостей для тестов
        services.AddDbContext<FoodDbContext>(options =>
            options.UseInMemoryDatabase("TestDatabase"));
        services.AddSingleton<IFoodRepository, FoodRepository>();

        ServiceProvider = services.BuildServiceProvider();
    }
}

