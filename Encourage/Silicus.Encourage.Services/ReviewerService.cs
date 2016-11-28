using Silicus.Encourage.DAL.Interfaces;
using Silicus.Encourage.Models;
using Silicus.Encourage.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Encourage.Services
{
    public class ReviewerService: IReviewerService
    {
        private readonly IEncourageDatabaseContext _encourageDatabaseContext;
        private readonly IDataContextFactory _dataContextFactory;

        public ReviewerService(IDataContextFactory dataContextFactory)
        {
            _dataContextFactory = dataContextFactory;
            _encourageDatabaseContext = dataContextFactory.CreateEncourageDbContext();

        }
        public bool addReviewer(int userId)
        {
            try
            {
                _encourageDatabaseContext.Add<Reviewer>(new Reviewer { UserId = userId });
                _encourageDatabaseContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }
    }
}
