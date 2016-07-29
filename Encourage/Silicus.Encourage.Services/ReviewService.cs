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
        private readonly INominationService _nominationService;

        public ReviewService(Silicus.Encourage.DAL.Interfaces.IDataContextFactory dataContextFactory, ICommonDbService commonDbService, INominationService nominationService)
        {
            _dataContextFactory = dataContextFactory;
            _commonDbService = commonDbService;
            _commonDataBaseContext = _commonDbService.GetCommonDataBaseContext();
            _encourageDatabaseContext = _dataContextFactory.CreateEncourageDbContext();
            _nominationService = nominationService;

        }

        public IEnumerable<Review> GetReviewsForNomination(int nominationID)
        {
            var reviews = _encourageDatabaseContext.Query<Review>("ReviewerComments").Where(review => review.NominationId == nominationID && review.IsSubmited == true).ToList();
            return reviews;
        }
        public void UpdateReview(Review model)
        {
            _encourageDatabaseContext.Update<Review>(model);
        }
        public List<Review> GetAllReview()
        {
            return _encourageDatabaseContext.Query<Review>("ReviewerComments").ToList();
        }
        public bool LockReview()
        {
            DateTime currentDate = System.DateTime.Now;
            var currentReview = GetAllReview();
            foreach (var review in currentReview)
            {
                if (review != null)
                {
                    var reviewrow = _nominationService.GetAllNominations().Where(x => x.Id.Equals(review.NominationId)).ToList().First().NominationDate;


                    if ((currentDate.Month - 1).Equals(reviewrow.Value.Month) && (currentDate.Month > 1 ? (currentDate.Year).Equals(reviewrow.Value.Year) : (currentDate.Year - 1).Equals(reviewrow.Value.Year)))
                    {
                        review.IsLocked = true;
                        UpdateReview(review);
                        return true;
                    }
                }

            }
            return false;
        }
    }
}
