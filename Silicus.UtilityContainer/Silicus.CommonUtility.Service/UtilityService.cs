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
    public class UtilityService : IUtilityService
    {
        private readonly SilicusUtilityContext _context;

        public UtilityService()
        {
            _context = new SilicusUtilityContext();
        }
       public List<Utility> GetAllUtilities()
       {
           return _context.Utilities.ToList();
       }
       public Utility FindUtility(int id)
       {
          return _context.Utilities.Find(id);
       }
    }
}
