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
            //  _FrameworxProjectDatabaseContext.Query<Frameworx>().Where(m => m.CategoryId.Equals(id)).ToList();
            return _FrameworxProjectDatabaseContext.Query<Frameworx>().Where(m => m.CategoryId.Equals(id)).ToList();
        }

        public List<Frameworx> GetAllFrameworx()
        {
            return _FrameworxProjectDatabaseContext.Query<Frameworx>().ToList();
        }

        public Frameworx FrameworkDetail(int id)
        {
            return _FrameworxProjectDatabaseContext.Query<Frameworx>("Likes", "Category").Where(m => m.Id == id).SingleOrDefault();
        }

        public List<FrameworxCategory> GetAllCategories()
        {
            return _FrameworxProjectDatabaseContext.Query<FrameworxCategory>().ToList();//Poulate Business Model h
        }

        public void AddCategory(FrameworxCategory category)
        {
            _FrameworxProjectDatabaseContext.Add(category);
        }

        public void AddFrameworx(Frameworx frameworx)
        {
            _FrameworxProjectDatabaseContext.Add(frameworx);
        }

        public int AddFrameworxLike(FrameworxLike frameworxLike)
        {
            return _FrameworxProjectDatabaseContext.Add(frameworxLike).Id;
        }

        public void RemoveFrameworxLike(FrameworxLike frameworxLike)
        {
            _FrameworxProjectDatabaseContext.Delete(frameworxLike);
        }
    }
}
