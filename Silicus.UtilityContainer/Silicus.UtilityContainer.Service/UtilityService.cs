using Silicus.UtilityContainer.Models.DataObjects;
using Silicus.UtilityContainer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silicus.UtilityContainer.Entities;
using System.Drawing;
using System.Web;
using System.IO;
using Silicus.UtilityContainer.Models.ViewModels;

namespace Silicus.UtilityContainer.Services
{
    public class UtilityService : IUtilityService
    {
         private readonly ICommonDataBaseContext _commmonDBContext;

         public UtilityService(IDataContextFactory dataContextFactory)
        {
            _commmonDBContext = dataContextFactory.CreateCommonDBContext();
        }
        
       public List<Utility> GetAllUtilities()
       {
           return _commmonDBContext.Query<Utility>().OrderBy(utility => utility.Name).ToList();
       }

       public Utility FindUtility(int utilityId)
       {
           return _commmonDBContext.Query<Utility>().Where(x => x.Id == utilityId).FirstOrDefault();
       }

        public List<UtilityRole> GetAllRolesForAnUtility(int utilityId)
       {
            if (_commmonDBContext.Query<UtilityRole>().Where(utility => utility.UtilityID == utilityId) != null)
            {
                return _commmonDBContext.Query<UtilityRole>().Where(utility => utility.UtilityID == utilityId).ToList();
            }
            return new List<UtilityRole>();
        }

        public void SaveUtilityRole(UtilityRoleViewModel newUtilityRole)
        {
            var existingUtilityRoles = GetAllRolesForAnUtility(newUtilityRole.UtilityId);
            var newRolesForUtility = new List<UtilityRole>();


            foreach (var item in newUtilityRole.RoleIds)
            {
                if (existingUtilityRoles.Find( x => x.RoleID == item) == null)
                {
                    _commmonDBContext.Add(new UtilityRole { UtilityID = newUtilityRole.UtilityId, RoleID = item });

                }
            }
            RemoveRolesFromUtility(newUtilityRole);
           
        }


        private void RemoveRolesFromUtility(UtilityRoleViewModel newUtilityRole)
        {
            var existingUtilityrRoles = GetAllRolesForAnUtility(newUtilityRole.UtilityId);

            foreach (var existingUtilityrRole in existingUtilityrRoles)
            {
                var result = newUtilityRole.RoleIds.Find(x => x.Equals(existingUtilityrRole.RoleID));

                if (result == 0)
                {
                    var recordToDelete = _commmonDBContext.Query<UtilityRole>().Where(x => x.UtilityID == existingUtilityrRole.UtilityID && 
                     x.RoleID == existingUtilityrRole.RoleID).FirstOrDefault();
                    _commmonDBContext.Delete(recordToDelete);
                }
            }

        }
    }
}
