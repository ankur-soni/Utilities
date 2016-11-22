using Silicus.FrameworxProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.FrameworxProject.Services.Interfaces
{
    public interface IProductBacklogService
    {
        List<ProductBacklog> GetAllProductBacklog();
    }
}
