using Silicus.FrameworxProject.Models;
using System.Collections.Generic;

namespace Silicus.FrameworxProject.Services.Interfaces
{
    public interface IProductBacklogService
    {
        List<ProductBacklog> GetAllProductBacklog();
    }
}
