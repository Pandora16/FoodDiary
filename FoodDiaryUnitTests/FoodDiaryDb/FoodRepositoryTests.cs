using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Core.Interfaces.Repositories;
using Core.Models;
using DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FoodDiaryUnitTests.DataBase
{
    [ExcludeFromCodeCoverage]
    public class FoodRepositoryTests : IDisposable
    {
        private readonly FoodRepository _foodRepository;
        private readonly Mock<IFoodDbContextFactory> _mockDbContextFactory;
        private readonly Mock<ILogger<FoodRepository>> _mockLogger;
        private readonly DbContextOptions<FoodDbContext> _dbContextOptions;

        public FoodRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<FoodDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _mockDbContextFactory = new Mock<IFoodDbContextFactory>();
            _mockLogger = new Mock<ILogger<FoodRepository>>();
            _foodRepository = new FoodRepository(_mockDbContextFactory.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task AddFoodAsync_ShouldAddFoodToDatabase()
        {
            var food = new Food
            {
                Id = 1,
                Name = "Test Food",
                UserName = "TestUser",
                Calories = 100,
                Proteins = 10,
                Fats = 5,
                Carbohydrates = 15,
                MealTime = MealTimes.Breakfast,
                Date = DateTime.Now
            };

            _mockDbContextFactory.Setup(f => f.CreateDbContext()).Returns(new FoodDbContext(_dbContextOptions));

            await _foodRepository.AddFoodAsync(food);

            using var context = new FoodDbContext(_dbContextOptions);
            var foods = await context.Foods.ToListAsync();
            Assert.Single(foods);
            Assert.Equal("Test Food", foods[0].Name);
        }

        [Fact]
        public async Task GetAllFoodsAsync_ShouldReturnAllFoods()
        {
            var food1 = new Food
            {
                Id = 1,
                Name = "Test Food 1",
                UserName = "TestUser 1",
                Calories = 100,
                Proteins = 10,
                Fats = 5,
                Carbohydrates = 15,
                MealTime = MealTimes.Breakfast,
                Date = DateTime.Now
            };

            var food2 = new Food
            {
                Id = 2,
                Name = "Test Food 2",
                UserName = "TestUser 2",
                Calories = 200,
                Proteins = 20,
                Fats = 10,
                Carbohydrates = 30,
                MealTime = MealTimes.Lunch,
                Date = DateTime.Now
            };

            using (var context = new FoodDbContext(_dbContextOptions))
            {
                await context.Foods.AddRangeAsync(food1, food2);
                await context.SaveChangesAsync();
            }

            _mockDbContextFactory.Setup(f => f.CreateDbContext()).Returns(new FoodDbContext(_dbContextOptions));

            var result = await _foodRepository.GetAllFoodsAsync();

            Assert.Equal(2, result.Count);
            Assert.Equal("Test Food 1", result[0].Name);
            Assert.Equal("Test Food 2", result[1].Name);
        }

        [Fact]
        public async Task AddUserAsync_ShouldAddUserToDatabase()
        {
            var user = new User
            {
                Id = 1,
                Name = "Test User",
                Height = 180,
                Weight = 75.0,
                Age = 30,
                Gender = "м",
                ActivityLevel = "Средний",
                BMR = 1800,
                TargetCalories = 2200
            };

            _mockDbContextFactory.Setup(f => f.CreateDbContext()).Returns(new FoodDbContext(_dbContextOptions));

            await _foodRepository.AddUserAsync(user);

            using var context = new FoodDbContext(_dbContextOptions);
            var users = await context.Users.ToListAsync();
            Assert.Single(users);
            Assert.Equal("Test User", users[0].Name);
        }

        [Fact]
        public async Task LoadUsersAsync_ShouldReturnAllUsers()
        {
            var user1 = new User
            {
                Id = 1,
                Name = "Test User 1",
                Height = 180,
                Weight = 75.0,
                Age = 30,
                Gender = "м",
                ActivityLevel = "Средний",
                BMR = 1800,
                TargetCalories = 2200
            };

            var user2 = new User
            {
                Id = 2,
                Name = "Test User 2",
                Height = 175,
                Weight = 70.0,
                Age = 25,
                Gender = "ж",
                ActivityLevel = "Высокий",
                BMR = 1600,
                TargetCalories = 2000
            };

            using (var context = new FoodDbContext(_dbContextOptions))
            {
                await context.Users.AddRangeAsync(user1, user2);
                await context.SaveChangesAsync();
            }

            _mockDbContextFactory.Setup(f => f.CreateDbContext()).Returns(new FoodDbContext(_dbContextOptions));

            var result = await _foodRepository.LoadUsersAsync();

            Assert.Equal(2, result.Count);
            Assert.Equal("Test User 1", result[0].Name);
            Assert.Equal("Test User 2", result[1].Name);
        }

        [Fact]
        public async Task UserExistsAsync_ShouldReturnTrueIfUserExists()
        {
            var user = new User
            {
                Id = 1,
                Name = "Test User",
                Height = 180,
                Weight = 75.0,
                Age = 30,
                Gender = "м",
                ActivityLevel = "Средний",
                BMR = 1800,
                TargetCalories = 2200
            };

            using (var context = new FoodDbContext(_dbContextOptions))
            {
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();
            }

            _mockDbContextFactory.Setup(f => f.CreateDbContext()).Returns(new FoodDbContext(_dbContextOptions));

            var exists = await _foodRepository.UserExistsAsync("Test User");

            Assert.True(exists);
        }

        [Fact]
        public async Task UserExistsAsync_ShouldReturnFalseIfUserDoesNotExist()
        {
            _mockDbContextFactory.Setup(f => f.CreateDbContext()).Returns(new FoodDbContext(_dbContextOptions));

            var exists = await _foodRepository.UserExistsAsync("Nonexistent User");

            Assert.False(exists);
        }

        public void Dispose()
        {
            using var context = new FoodDbContext(_dbContextOptions);
            context.Database.EnsureDeleted();
        }
    }
}
