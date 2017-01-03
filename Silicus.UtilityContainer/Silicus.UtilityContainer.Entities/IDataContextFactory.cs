using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.UtilityContainer.Entities
{
    public interface IDataContextFactory
    {
        ICommonDataBaseContext CreateCommonDBContext();
    }
}
