
using Microsoft.AspNetCore.Hosting;

namespace FoodDiaryApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // ������� � ��������� ���� ����������
            CreateHostBuilder(args).Build().Run();
        }

        // ����������� ���� ��� ASP.NET Core ����������
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // ���������� ����� Startup ��� ��������� ����������
                    webBuilder.UseStartup<Startup>();
                    // ��������� ���� 80 ��� ��������� HTTP-��������
                    // ���� 80 ����������� ��� HTTP, ������ ���������� ��������� ��� users ��� ������������� ��������� ���� � URL
                    webBuilder.UseUrls("http://*:80");
                });
    }
}
