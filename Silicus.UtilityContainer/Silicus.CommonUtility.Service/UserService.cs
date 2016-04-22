using Silicus.UtilityContainer.Models.DataObjects;
using Silicus.UtilityContainer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silicus.UtilityContainerr.Entities;

namespace Silicus.UtilityContainer.Services
{
    public class UserService : IUserService
    {
        //private readonly SilicusUtilityContext _context;
        private readonly IUtilityService _utilityService;
        private readonly ICommonDataBaseContext _commonDBContext;

        public UserService(IUtilityService utilityService, IDataContextFactory dataContextFactory)
        {
            _commonDBContext = dataContextFactory.CreateCommonDBContext();
            _utilityService = utilityService;
        }


        public List<User> GetAllUsers()
        {
            return _commonDBContext.Query<User>().OrderBy(user => user.DisplayName).ToList();
        }

        public void AddRolesToUserForAUtility(UtilityUserRoles newUserRole)
        {
            var userRole = _commonDBContext.Query<UtilityUserRoles>().Where(x => x.UserId == newUserRole.UserId && x.UtilityId == newUserRole.UtilityId).FirstOrDefault();
            if (userRole != null)
            {
                _commonDBContext.Delete(userRole);
            }
            _commonDBContext.Add(newUserRole);
        }

        
    }
}
