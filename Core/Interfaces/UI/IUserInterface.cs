using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.UI
{
    public interface IUserInterface
    {
        Task WriteMessageAsync(string message);
        Task<string> ReadInputAsync();
    }
}
