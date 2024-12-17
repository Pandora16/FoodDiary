

namespace Core.Interfaces.Services
{
    public interface IFoodDiaryManager
    {
        Task InitializerAsync(IUserDataInitializer userDataInitializer);
    }

}
