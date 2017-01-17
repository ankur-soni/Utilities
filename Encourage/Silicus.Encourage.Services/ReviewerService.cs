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
    public class ReviewerService : IReviewerService
    {
        private readonly IEncourageDatabaseContext _encourageDatabaseContext;

        public ReviewerService(IDataContextFactory dataContextFactory)
        {
            _encourageDatabaseContext = dataContextFactory.CreateEncourageDbContext();

        }
        public bool AddReviewer(int userId)
        {
            try
            {
                if (!_encourageDatabaseContext.Query<Reviewer>().Any(r => r.UserId == userId))
                {
                    _encourageDatabaseContext.Add<Reviewer>(new Reviewer
                    {
                        UserId = userId
                    });
                    _encourageDatabaseContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }
    }
}
