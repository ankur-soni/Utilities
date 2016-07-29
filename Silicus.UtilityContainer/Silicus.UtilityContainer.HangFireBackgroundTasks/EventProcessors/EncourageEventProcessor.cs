using Silicus.UtilityContainer.HangFireBackgroundTasks.Interface;
using System;
using Silicus.UtilityContainer.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Configuration;

namespace HangFireBackgroundTasks.EventProcessors
{
    public class EncourageEventProcessor : IEventProcessor
    {
       
        public void Process(EventType eventType)
        {
            switch (eventType)
            {
                case EventType.LockNomination:

                    DateTime currentDate = System.DateTime.Now;
                    var firstDayOfMonth = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, 1);
                    var rersult = ConfigurationManager.AppSettings["NoOfDaysManager"];
                    var expireDateManager = firstDayOfMonth.AddDays(Convert.ToDouble(ConfigurationManager.AppSettings["NoOfDaysManager"].ToString()));
                    if (currentDate == expireDateManager || currentDate < expireDateManager)
                    {
                        LockNomination();
                    }

                    break;
                case EventType.LockReview:
                   LockReview();
                   break;
                default:
                    break;
            }
        }
        
        private void LockNomination()
        {
            const string URL = @"https://localhost:44324/api/nominationapi/lock";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            var response = client.PostAsync(URL, null).Result;
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!
                var result = response.Content.ReadAsStringAsync().Result;

            }
            else
            {

            }
        }
        private void LockReview()
        {
            const string URL = @"https://localhost:44324/api/nominationapi/reviewlock";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            var response = client.PostAsync(URL, null).Result;
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!
                var result = response.Content.ReadAsStringAsync().Result;

            }
            else
            {

            }
        }


        //private void LockNomination()
        //{
        //    DateTime currentDate = System.DateTime.Now;
        //    var firstDayOfMonth = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, 1);

        //    #region Manager

        //    var expireDateManager = firstDayOfMonth.AddDays(Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["NoOfDaysManager"].ToString()));

        //    if (currentDate == expireDateManager || currentDate < expireDateManager)
        //    {
        //        //set IsLocked = true for "Nomination" Table

        //        var allNominations = _nominationService.GetAllNominations();
        //        //nominations = nominations.Where(x => x.NominationDate!=null &&  Object.Equals((x.NominationDate.Value.Month), (DateTime.Now.Month - 1)));

        //        // var nominations = allNominations.Where(x => (x.NominationDate != null) && (Object.Equals((x.NominationDate.Value.Month), (DateTime.Now.Month - 1))));
        //        var nominations = allNominations.Where(x => x.NominationDate.Value.Month.Equals(DateTime.Now.Month - 1)).ToList();
        //        //nominations = nominations.Where(x => x.NominationDate !=null && (x.NominationDate.Value.Month == (DateTime.Now.Month - 1)));
        //        foreach (var nomination in nominations)
        //        {
        //            if (nomination != null)
        //            {
        //                nomination.IsLocked = true;
        //                _nominationService.UpdateNomination(nomination);
        //                //ReviewsLockedNotificationToReviewers reviewsLockedNotificationToReviewers = new ReviewsLockedNotificationToReviewers();
        //                //reviewsLockedNotificationToReviewers.Process();
        //                //emmail to manager

        //            }
        //        }
        //    }
        //    else
        //    {
        //        //ignore
        //    }
        //    #endregion

        //    #region Reviewer

        //    var expireDateReviewer = firstDayOfMonth.AddDays(Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["NoOfDaysManager"].ToString()) + Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["NoOfDaysReviewer"].ToString()));

        //    if (currentDate == expireDateReviewer || currentDate < expireDateReviewer)
        //    {
        //        var reviews = _reviewService.GetAllReview();


        //        foreach (var review in reviews)
        //        {
        //            if (review != null)
        //            {
        //                var reviewrow = _nominationService.GetAllNominations().Where(x => x.Id.Equals(review.NominationId)).ToList().First().NominationDate;


        //                if ((DateTime.Now.Month - 1).Equals(reviewrow.Value.Month))
        //                {
        //                    review.IsLocked = true;
        //                    _reviewService.UpdateReview(review);
        //                    //ReviewsLockedNotificationToAdmin reviewsLockedNotificationToAdmin = new ReviewsLockedNotificationToAdmin();
        //                    //reviewsLockedNotificationToAdmin.Process();
        //                    //email to reviewer
        //                }

        //                //  review.NominationId

        //            }
        //        }
        //    }
        //    else
        //    {
        //        //ignore
        //    }
        //    #endregion
        //}
    }
}
