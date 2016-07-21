using Silicus.Encourage.DAL;
using Silicus.Encourage.Services;
using Silicus.Encourage.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.EncourageWithAzureAd.Web.Hangfire
{
    public class Locker
    {
        //public Locker(string connectionString) : base(connectionString)
        //{
        //}
        private INominationService _nominationService;
        private IReviewService _reviewService;
        public Locker(INominationService nominationService,IReviewService reviewService)
        {
            //if (nominationService == null)
            //{
            //    throw new ArgumentNullException("nominationService");
            //}

            _nominationService = nominationService;
            _reviewService = reviewService;
        }

        public void setLockForNomination()
        {
            #region Manager

            DateTime currentDate = System.DateTime.Now;
            var firstDayOfMonth = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, 1);

            var expireDateManager = firstDayOfMonth.AddDays(Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["NoOfDaysManager"].ToString()));

            if (currentDate == expireDateManager || currentDate >= expireDateManager)
            {
                //set IsLocked = true for "Nomination" Table

                var nominations = _nominationService.GetAllNominations();
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

            if (currentDate == expireDateReviewer || currentDate >= expireDateReviewer)
            {
                var reviews = _reviewService.GetAllReview();
                foreach (var review in reviews)
                {
                    if (review != null)
                    {
                        review.IsLocked = true;
                        _reviewService.UpdateReview(review);
                    }
                }
                //set IsLocked = true for "Reviewer" Table
            }
            else
            {
                //ignore
            }
            #endregion

        }
    }
}