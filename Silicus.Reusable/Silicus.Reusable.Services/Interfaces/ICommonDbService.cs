using Silicus.UtilityContainer.Models.DataObjects;

namespace Silicus.FrameworxProject.Services.Interfaces
{
    public interface ICommonDbService
    {
        Silicus.UtilityContainer.Entities.ICommonDataBaseContext GetCommonDataBaseContext();
        string FindDisplayNameFromEmail(string email);
        int? FindUserIdFromEmail(string userName);
        User GetUser(int userId);
    }
}
