using Silicus.Reusable.Models;
using Silicus.Reusable.Services.Interfaces;
using System;
using System.Collections.Generic;

using Silicus.Reusable.DAL.Interfaces;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Silicus.Reusable.Services
{
    public class ReusableService : IReusableService
    {
        private readonly IDataContextFactory _dataContextFactory;
        private readonly IReusableDatabaseContext _reusableDatabaseContext;

        public ReusableService(Silicus.Reusable.DAL.Interfaces.IDataContextFactory dataContextFactory)
        {
            _dataContextFactory = dataContextFactory;
            _reusableDatabaseContext = _dataContextFactory.CreateReusableDbContext();
        }

        public List<Framework> GetAllFrameworks()
        {
            return _reusableDatabaseContext.Query<Framework>().ToList();
        }
        public Framework FrameworkDetail(int id)
        {
            return _reusableDatabaseContext.Query<Framework>().Where(m => m.Id == id).SingleOrDefault();
        }
    }
}
