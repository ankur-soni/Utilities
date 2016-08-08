using Silicus.FrameworxProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Silicus.FrameworxProject.Services.Interfaces
{
   public interface IFrameworxProjectService
    {
        List<Frameworx> GetAllFrameworxs(int id);

        List<Frameworx> GetAllFrameworx();

        Frameworx FrameworkDetail(int id);

        List<Category> GetAllCategories();

        void AddCategory(Category category);

        void AddFrameworx(Frameworx frameworx);
    }
}
