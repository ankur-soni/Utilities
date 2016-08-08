using Silicus.FrameworxProject.Models;
using Silicus.FrameworxProject.Services.Interfaces;
using System;
using System.Collections.Generic;

using Silicus.FrameworxProject.DAL.Interfaces;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Silicus.FrameworxProject.Services
{
    public class FrameworxProjectService : IFrameworxProjectService
    {
        private readonly IDataContextFactory _dataContextFactory;
        private readonly IFrameworxProjectDatabaseContext _FrameworxProjectDatabaseContext;

        public FrameworxProjectService(Silicus.FrameworxProject.DAL.Interfaces.IDataContextFactory dataContextFactory)
        {
            _dataContextFactory = dataContextFactory;
            _FrameworxProjectDatabaseContext = _dataContextFactory.CreateFrameworxProjectDbContext();
        }

        public List<Frameworx> GetAllFrameworxs(int id)
        {
            return _FrameworxProjectDatabaseContext.Query<Frameworx>().Where(m=>m.CategoryId == id).ToList();
        }

        public List<Frameworx> GetAllFrameworx()
        {
            return _FrameworxProjectDatabaseContext.Query<Frameworx>().ToList();
        }

        public Frameworx FrameworkDetail(int id)
        {
            return _FrameworxProjectDatabaseContext.Query<Frameworx>().Where(m => m.Id == id).SingleOrDefault();
        }

        public List<Category> GetAllCategories()
        {
            return _FrameworxProjectDatabaseContext.Query<Category>().ToList();//Poulate Business Model h
        }

        public void AddCategory(Category category)
        {
            _FrameworxProjectDatabaseContext.Add(category);
        }

        public void AddFrameworx(Frameworx frameworx)
        {
            _FrameworxProjectDatabaseContext.Add(frameworx);
        }
    }
}
