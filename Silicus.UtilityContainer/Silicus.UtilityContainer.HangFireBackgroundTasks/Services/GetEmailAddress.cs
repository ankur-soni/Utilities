using Silicus.UtilityContainer.Entities;
using Silicus.UtilityContainer.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.UtilityContainer.HangFireBackgroundTasks.Services
{
   public static class GetEmailAddress
    {
        public static List<string> GetEmailAddressForRoles(int roleId)
        {
            IDataContextFactory dataContextFactory = new DataContextFactory();
            var _commonDBContext = dataContextFactory.CreateCommonDBContext();
            var allManagerUseId = _commonDBContext.Query<UtilityUserRoles>().Where(x => x.RoleId == roleId).Select(x => x.UserId).ToList();  //roleid for Reviewer=367
            var manageremailAdddresses = new List<string>();

            foreach (var id in allManagerUseId)
            {
                manageremailAdddresses.Add(_commonDBContext.Query<User>().Where(user => user.ID == id).SingleOrDefault().EmailAddress);
            }

            return manageremailAdddresses;
        }

        public static List<string> GetEmailAddressesOfAllUsers()
        {
            IDataContextFactory dataContextFactory = new DataContextFactory();
            var allUsersEmailAddresses = dataContextFactory.CreateCommonDBContext().Query<User>().Select(user => user.EmailAddress).ToList();
            return allUsersEmailAddresses;
        }

    }
}
