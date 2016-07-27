using HangFireBackgroundTasks.Interface;
using Silicus.Encourage.DAL.Interfaces;
using Silicus.Encourage.Services;
using Silicus.Encourage.Services.Interface;
using Silicus.UtilityContainer.HangFireBackgroundTasks.EventProcessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangFireBackgroundTasks.EventProcessors
{
    public class SetLockEventProcessor : ILockingEventProcessor 
    {

        //private  IDataContextFactory dataContextFactory;
        //private  ICommonDbService commonDbService;

        private INominationService _nominationService;
        private IReviewService _reviewService;
        //public SetLockEventProcessor()
        //{
        //}

        public SetLockEventProcessor(INominationService nominationService, IReviewService reviewService)
        {
            _nominationService = nominationService;
            _reviewService = reviewService;
        }
        public void setLockForNomination()
        {
            DateTime currentDate = System.DateTime.Now;
            var firstDayOfMonth = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, 1);

            #region Manager

            var expireDateManager = firstDayOfMonth.AddDays(Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["NoOfDaysManager"].ToString()));

            if (currentDate == expireDateManager || currentDate > expireDateManager)
            {
                var allNominations = _nominationService.GetAllNominations();
                // var nominations = allNominations.Where(x => x.NominationDate.Value.Month.Equals(DateTime.Now.Month - 1)).ToList();
                var nominations = allNominations.Where(x => (x.NominationDate.Value.Month.Equals(currentDate.Month - 1) && x.NominationDate.Value.Year.Equals(currentDate.Month > 1 ? currentDate.Year : currentDate.Year - 1))).ToList();
                foreach (var nomination in nominations)
                {
                    if (nomination != null)
                    {
                        nomination.IsLocked = true;
                        _nominationService.UpdateNomination(nomination);
                        ReviewsLockedNotificationToReviewers reviewsLockedNotificationToReviewers = new ReviewsLockedNotificationToReviewers();
                        reviewsLockedNotificationToReviewers.Process();
                        //emmail to manager
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
                    if (review != null)
                    {
                        var reviewrow = _nominationService.GetAllNominations().Where(x => x.Id.Equals(review.NominationId)).ToList().First().NominationDate;

                        //if ((DateTime.Now.Month - 1).Equals(reviewrow.Value.Month))
                        //{
                        if ((currentDate.Month - 1).Equals(reviewrow.Value.Month) && (currentDate.Month > 1 ? (currentDate.Year).Equals(reviewrow.Value.Year) : (currentDate.Year - 1).Equals(reviewrow.Value.Year)))
                        {
                            review.IsLocked = true;
                            _reviewService.UpdateReview(review);
                            ReviewsLockedNotificationToAdmin reviewsLockedNotificationToAdmin = new ReviewsLockedNotificationToAdmin();
                            reviewsLockedNotificationToAdmin.Process();
                            //email to reviewer
                        }
                        //  review.NominationId
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
