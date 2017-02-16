using Silicus.Encourage.Services.Interface;
using System;
using System.Linq;

namespace Silicus.EncourageWithAzureAd.Web.Hangfire
{
    public class Locker
    {
        private readonly INominationService _nominationService;
        private readonly IReviewService _reviewService;
        public Locker(INominationService nominationService, IReviewService reviewService)
        {
            if (nominationService == null)
            {
                throw new ArgumentNullException("nominationService");
            }
            else if (reviewService == null)
            {
                throw new ArgumentNullException("reviewService");
            }

            _nominationService = nominationService;
            _reviewService = reviewService;
        }
        /// <summary>
        /// Method for locking nomination and review period.
        /// </summary>
        public void SetLockForNomination()
        {
            DateTime currentDate = System.DateTime.Now;
            var firstDayOfMonth = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, 1);

            #region Manager

            var expireDateManager = firstDayOfMonth.AddDays(Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["NoOfDaysManager"].ToString()));

            if (currentDate == expireDateManager || currentDate < expireDateManager)
            {
                var allNominations = _nominationService.GetAllNominations();
                var nominations = allNominations.Where(x => (x.NominationDate.Value.Month.Equals(currentDate.Month - 1) && x.NominationDate.Value.Year.Equals(currentDate.Month > 1 ? currentDate.Year : currentDate.Year - 1))).ToList();

                foreach (var nomination in nominations)
                {
                    if (nomination != null)
                    {
                        nomination.IsLocked = true;
                        _nominationService.UpdateNomination(nomination);
                    }
                }
            }
            else
            {
                //ignore
            }
            #endregion

            #region Reviewer

            var expireDateReviewer = firstDayOfMonth.AddDays(Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["NoOfDaysManager"].ToString()) + Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["NoOfDaysReviewer"].ToString()));

            if (currentDate == expireDateReviewer || currentDate < expireDateReviewer)
            {
                var reviews = _reviewService.GetAllReview();


                foreach (var review in reviews)
                {
                    if (review == null)
                    {
                        continue;
                    }

                    var nomination = _nominationService.GetAllNominations().FirstOrDefault(x => x.Id.Equals(review.NominationId));
                    if (nomination == null)
                    {
                        continue;
                    }

                    var reviewrow = nomination.NominationDate;

                    if ((currentDate.Month - 1).Equals(reviewrow.Value.Month) && (currentDate.Month > 1 ? (currentDate.Year).Equals(reviewrow.Value.Year) : (currentDate.Year - 1).Equals(reviewrow.Value.Year)))
                    {
                        review.IsLocked = true;
                        _reviewService.UpdateReview(review);
                    }
                }
            }
            else
            {
                //ignore
            }
            #endregion

        }
    }
}