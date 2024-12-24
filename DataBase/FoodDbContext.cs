using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DataBase
{
    // Контекст базы данных (DbContext) — это инструмент для работы с данными
    public class FoodDbContext : DbContext
    {
        private readonly string _connectionString;
        //Конструктор FoodDbContext принимает DbContextOptions, который содержит настройки подключения к базе данных
        public FoodDbContext(DbContextOptions<FoodDbContext> options) : base(options)
        {
        }
        // DbSet<Food> и DbSet<User> - таблицы в базе данных, связанные с моделями Food и User
        public DbSet<User> Users { get; set; }
        public DbSet<Food> Foods { get; set; }

        // Метод вызывается при создании модели базы данных
        // protected - доступен только внутри самого класса и его наследников, т.к. предназначен для настройки модели базы данных в EF Core
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // указывает EF Core, что сущность User должна быть сопоставлена с таблицей Users
            modelBuilder.Entity<User>().ToTable("Users");
            // аналогично для сущности Food и таблицы Foods
            modelBuilder.Entity<Food>().ToTable("Foods");
        }
    }
}
