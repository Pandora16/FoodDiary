using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.UI
{
    public interface IUserInputManager
    {
        Task<MealTimes> GetMealTimeAsync();
        Task<int> GetPositiveIntegerAsync(string message);
        Task<double> GetPositiveDoubleAsync(string message);
        Task<string> GetGenderAsync();
        Task<string> GetActivityLevelAsync();
        Task<string> GetUserNameAsync(string v);
    }
}
