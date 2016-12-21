using Silicus.UtilityContainer.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.FrameworxProject.Services.Interfaces
{
    public interface ICommonDbService
    {
        Silicus.UtilityContainer.Entities.ICommonDataBaseContext GetCommonDataBaseContext();
        string FindDisplayNameFromEmail(string email);
        int FindUserIdFromEmail(string userName);
        User GetUser(int userId);
    }
}
