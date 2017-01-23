using Silicus.UtilityContainer.Models.DataObjects;
using System.Collections;
using System.Collections.Generic;

namespace Silicus.FrameworxProject.Services.Interfaces
{
    public interface ICommonDbService
    {
        Silicus.UtilityContainer.Entities.ICommonDataBaseContext GetCommonDataBaseContext();
        string FindDisplayNameFromEmail(string email);
        int? FindUserIdFromEmail(string userName);
        User GetUser(int userId);
        IEnumerable<User> GetAllUsers();
    }
}
