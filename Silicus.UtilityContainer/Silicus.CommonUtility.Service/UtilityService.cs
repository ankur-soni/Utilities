using Silicus.UtilityContainer.Models.DataObjects;
using Silicus.UtilityContainer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silicus.UtilityContainerr.Entities;
using System.Drawing;
using System.Web;
using System.IO;

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

       public Utility FindUtility(int id)
       {
           return _commmonDBContext.Query<Utility>().Where(x => x.Id == id).FirstOrDefault();
       }
    }
}
