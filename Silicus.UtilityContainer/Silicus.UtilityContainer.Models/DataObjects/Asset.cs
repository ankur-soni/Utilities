using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.UtilityContainer.Models.DataObjects
{
    public class Asset
    {
            public int AssetId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }

            public virtual ICollection<Category> Categories { get; set; }
        
    }
}
