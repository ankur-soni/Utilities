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
         private readonly ILocalDataBaseContext _localDBContext;

         public UtilityService(IDataContextFactory dataContextFactory)
        {
            _localDBContext = dataContextFactory.CreateLocalDBContext();
        }
        
       public List<Utility> GetAllUtilities()
       {
           return _localDBContext.Query<Utility>().ToList();
       }

       public Utility FindUtility(int id)
       {
           return _localDBContext.Query<Utility>().Where(x => x.Id == id).FirstOrDefault();
       }
    }
}
