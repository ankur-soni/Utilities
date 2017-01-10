using Silicus.UtilityContainer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silicus.UtilityContainer.Models.DataObjects;
using Silicus.UtilityContainer.Entities;

namespace Silicus.UtilityContainer.Services
{
   public class SuperUserService : ISuperUserService
    {
       private readonly ICommonDataBaseContext _commmonDBContext;
        public SuperUserService(ICommonDataBaseContext commonDbContext)
        {
            _commmonDBContext = commonDbContext;
        }

        public List<SuperUser> GetAllSuperUsers()
        {
            return  _commmonDBContext.Query<SuperUser>().ToList();
        }
    }
}
