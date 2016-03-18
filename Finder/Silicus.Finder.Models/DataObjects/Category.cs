using System.Collections.Generic;

namespace Silicus.Finder.Models.DataObjects
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Asset> Assets { get; set; }
    }
}
