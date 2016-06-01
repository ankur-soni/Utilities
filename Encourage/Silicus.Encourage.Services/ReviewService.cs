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
    public class ReviewService : IReviewService
    {
        private readonly IDataContextFactory _dataContextFactory;
        private readonly IEncourageDatabaseContext _encourageDatabaseContext;
        private readonly Silicus.UtilityContainer.Entities.ICommonDataBaseContext _commonDataBaseContext;
        private readonly ICommonDbService _commonDbService;

        public ReviewService(Silicus.Encourage.DAL.Interfaces.IDataContextFactory dataContextFactory, ICommonDbService commonDbService)
        {
            _dataContextFactory = dataContextFactory;
            _commonDbService = commonDbService;
            _commonDataBaseContext = _commonDbService.GetCommonDataBaseContext();
            _encourageDatabaseContext = _dataContextFactory.CreateEncourageDbContext();

        }

        public IEnumerable<Review> GetReviewsForNomination(int nominationID)
        {
            var reviews = _encourageDatabaseContext.Query<Review>("ReviewerComments").Where(review => review.NominationId == nominationID).ToList();
            return reviews;
        }

       
    }
}
