using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DataBase
{
    public class FoodDbContext : DbContext
    {
        private readonly string _connectionString;
        public FoodDbContext(DbContextOptions<FoodDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Food> Foods { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Food>().ToTable("Foods");
        }
    }
}
