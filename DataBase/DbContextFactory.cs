using Microsoft.EntityFrameworkCore;

namespace DataBase
{
    //  Зачем фабрика?
    //  Фабрика — объект, который создаёт экземпляры других объектов
    //  1) EF Core-контекст не является потокобезопасным ( а у нас асинхронные методы ), поэтому лучше создавать отдельный экземпляр для каждой операции (например AddFoodAsync)
    //  2) Когда завершается работа с контекстом, он автоматически освобождает ресурсы, связанные с подключением к базе данных
    //  3) Каждая операция (например, добавление, чтение данных) создаёт свой собственный экземпляр контекста. Это гарантирует, что операции не будут мешать друг другу.
    public class DbContextFactory : IFoodDbContextFactory
    {
        private readonly DbContextOptions<FoodDbContext> _options;

        // Метод инициализирует фабрику, принимая настройки для контекста базы данных (DbContextOptions<FoodDbContext>), и сохраняет их в поле _options
        public DbContextFactory(DbContextOptions<FoodDbContext> options)
        {
            _options = options;
        }
        // Метод создаёт и возвращает новый экземпляр контекста базы данных (FoodDbContext)
        // Вызов этого метода возвращает готовый объект, который можно использовать для работы с таблицами базы данных
        public FoodDbContext CreateDbContext()
        {
            return new FoodDbContext(_options);
        }
    }
}
