using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Encourage.Services.Interface
{
  public interface ICommonDbService
    {
      Silicus.UtilityContainer.Entities.ICommonDataBaseContext GetCommonDataBaseContext(); 
    }
}
