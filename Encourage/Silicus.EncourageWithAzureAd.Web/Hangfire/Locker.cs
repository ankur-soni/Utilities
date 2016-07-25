﻿using Silicus.Encourage.Services.Interface;
using System;
using System.Linq;

namespace Silicus.EncourageWithAzureAd.Web.Hangfire
{
    public class Locker 
    {
        private INominationService _nominationService;
        private IReviewService _reviewService;
        public Locker(INominationService nominationService,IReviewService reviewService)
        {
            if (nominationService == null)
            {
                throw new ArgumentNullException("nominationService");
            }
            else if(reviewService == null)
            {
                throw new ArgumentNullException("reviewService");
            }
            
            _nominationService = nominationService;
            _reviewService = reviewService;
        }
        /// <summary>
        /// Method for locking nomination and review period.
        /// </summary>
        public void setLockForNomination()
        {
            DateTime currentDate = System.DateTime.Now;
            var firstDayOfMonth = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, 1);
            
            #region Manager

            var expireDateManager = firstDayOfMonth.AddDays(Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["NoOfDaysManager"].ToString()));

            if (currentDate == expireDateManager || currentDate < expireDateManager)
            {
                //set IsLocked = true for "Nomination" Table

                var allNominations = _nominationService.GetAllNominations();
                //nominations = nominations.Where(x => x.NominationDate!=null &&  Object.Equals((x.NominationDate.Value.Month), (DateTime.Now.Month - 1)));

                // var nominations = allNominations.Where(x => (x.NominationDate != null) && (Object.Equals((x.NominationDate.Value.Month), (DateTime.Now.Month - 1))));
                var nominations = allNominations.Where(x => x.NominationDate.Value.Month.Equals(DateTime.Now.Month - 1)).ToList();
                //nominations = nominations.Where(x => x.NominationDate !=null && (x.NominationDate.Value.Month == (DateTime.Now.Month - 1)));
                foreach (var nomination in nominations)
                {
                    if (nomination != null)
                    {
                        nomination.IsLocked = true;
                        _nominationService.UpdateNomination(nomination);

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

                        if ((DateTime.Now.Month - 1).Equals(reviewrow.Value.Month))
                        {
                            review.IsLocked = true;
                            _reviewService.UpdateReview(review);
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