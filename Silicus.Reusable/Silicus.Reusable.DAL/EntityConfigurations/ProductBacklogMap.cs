using Silicus.FrameworxProject.Models;
using System.Data.Entity.ModelConfiguration;

namespace Silicus.FrameworxProject.DAL.EntityConfigurations
{
    internal class ProductBacklogMap : EntityTypeConfiguration<ProductBacklog>
    {
        public ProductBacklogMap()
        {
            HasKey(o => o.Id);
        }
    }
}
