﻿using Silicus.UtilityContainer.Models.DataObjects;
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
        private readonly ILocalDataBaseContext _localDBContext;

        public UserService(IUtilityService utilityService, IDataContextFactory dataContextFactory)
        {
            _commonDBContext = dataContextFactory.CreateCommonDBContext();
            _utilityService = utilityService;
            _localDBContext = dataContextFactory.CreateLocalDBContext();
        }

        public List<User> GetAllUsers()
        {
            return _commonDBContext.Query<User>().ToList();
        }

        public void AddRolesToUserForAUtility(UserRole newUserRole)
        {
            var userRole = _localDBContext.Query<UserRole>().Where(x => x.UserId == newUserRole.UserId && x.UtilityId == newUserRole.UtilityId).FirstOrDefault();
            if (userRole != null)
            {
                _localDBContext.Delete(userRole);
            }
            _localDBContext.Add(newUserRole);
        }

        
    }
}
