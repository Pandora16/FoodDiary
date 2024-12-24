using Microsoft.EntityFrameworkCore;

namespace DataBase
{
    public interface IFoodDbContextFactory
    {
        FoodDbContext CreateDbContext();
    }
}
