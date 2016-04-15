using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.UtilityContainerr.Entities
{
    public interface IDataContextFactory
    {
        ICommonDataBaseContext CreateCommonDBContext();
    }
}
