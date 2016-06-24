using Silicus.UtilityContainer.Models.DataObjects;
using Silicus.UtilityContainer.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.UtilityContainer.Services.Interfaces
{
    public interface IUserService
    {
        User GetUserByID(int ID);
        List<User> GetAllUsers();
        void AddRolesToUserForAUtility(UtilityUserRoleViewModel newUserRole);
        bool CheckForFirstLoginByEmail(string email);
        User FindUserByEmail(string email);
        //    void AddRoleToUserForAllUtility(User user);
        string FindDisplayNameFromEmail(string email);

       List<String> GetAllManagersEmailAddresses();
    }
}
