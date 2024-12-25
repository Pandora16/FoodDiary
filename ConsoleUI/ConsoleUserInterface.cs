using Core.Interfaces.UI;
using System.Diagnostics.CodeAnalysis;

namespace ConsoleUI
{
    [ExcludeFromCodeCoverage]
    public class ConsoleUserInterface : IUserInterface
    {
        public async Task<string> ReadInputAsync()
        {
            return await Task.Run(() => Console.ReadLine());
        }

        public async Task WriteMessageAsync(string message)
        {
            await Task.Run(() => Console.WriteLine(message));
        }
    }
}
