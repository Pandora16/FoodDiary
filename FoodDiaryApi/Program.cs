
using Microsoft.AspNetCore.Hosting;

namespace FoodDiaryApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Создаем и запускаем хост приложения
            CreateHostBuilder(args).Build().Run();
        }

        // Настраиваем хост для ASP.NET Core приложения
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // Используем класс Startup для настройки приложения
                    webBuilder.UseStartup<Startup>();
                    // Указываем порт 80 для обработки HTTP-запросов
                    // Порт 80 стандартным для HTTP, делает приложение доступным для users без необходимости указывать порт в URL
                    webBuilder.UseUrls("http://*:80");
                });
    }
}
