using System.Collections.Generic;

namespace Silicus.Finder.Models.DataObjects
{
    public class Asset
    {
        public int AssetId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}