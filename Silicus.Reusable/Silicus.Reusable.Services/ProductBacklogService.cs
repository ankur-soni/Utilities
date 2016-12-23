using Silicus.FrameworxProject.DAL.Interfaces;
using Silicus.FrameworxProject.Models;
using Silicus.FrameworxProject.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Silicus.FrameworxProject.Services
{
    public class ProductBacklogService: IProductBacklogService
    {
        private readonly IDataContextFactory _dataContextFactory;
        private readonly IFrameworxProjectDatabaseContext _FrameworxProjectDatabaseContext;

        public ProductBacklogService(Silicus.FrameworxProject.DAL.Interfaces.IDataContextFactory dataContextFactory)
        {
            _dataContextFactory = dataContextFactory;
            _FrameworxProjectDatabaseContext = _dataContextFactory.CreateFrameworxProjectDbContext();
        }

        public List<ProductBacklog> GetAllProductBacklog()
        {
            return _FrameworxProjectDatabaseContext.Query<ProductBacklog>().ToList();
        }
    }
}