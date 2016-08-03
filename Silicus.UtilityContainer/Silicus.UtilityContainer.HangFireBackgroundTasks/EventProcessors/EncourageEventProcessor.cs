using Silicus.UtilityContainer.HangFireBackgroundTasks.Interface;
using System;
using Silicus.UtilityContainer.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Configuration;
using HangFireBackgroundTasks.Services;
using System.Linq;

namespace HangFireBackgroundTasks.EventProcessors
{
    public class EncourageEventProcessor : IEventProcessor
    {
        EncourageEmailProcessor emial = new EncourageEmailProcessor();


        public void Process(EventType eventType)
        {

            DateTime currentDate = System.DateTime.Now;
            var firstDayOfMonth = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, 1);
            switch (eventType)
            {
                case EventType.LockNomination:
                    var result = Convert.ToInt32(ConfigurationManager.AppSettings["NoOfDaysManager"]);
                    DateTime expireDateManager = firstDayOfMonth.AddDays(Convert.ToDouble(ConfigurationManager.AppSettings["NoOfDaysManager"].ToString()));                 
                    HolidayService holidayService = new HolidayService();
                    var holidayData = holidayService.GetCurrentMonthHolidays();

                    if (holidayData.Any())
                    {
                        var fallingHolidays = from temp in holidayData
                                              where temp <= result
                                              select temp;

                        var count2 = fallingHolidays.Count();
                        expireDateManager = expireDateManager.AddDays(count2);
                    }

                    if (currentDate == expireDateManager || currentDate > expireDateManager)
                    {
                        LockNomination();
                    }

                    break;
                case EventType.LockReview:

                    int count = 0;
                    var noOfDaysManager = Convert.ToInt32(ConfigurationManager.AppSettings["NoOfDaysManager"]);

                    HolidayService holidayService1 = new HolidayService();
                    var holidayData1 = holidayService1.GetCurrentMonthHolidays();

                    if (holidayData1.Any())
                    {
                        var fallingHolidays1 = from temp in holidayData1
                                               where temp <= noOfDaysManager
                                               select temp;

                        count = fallingHolidays1.Count();
                    }
                    var count1 = 0;
                    noOfDaysManager += count;
                    var noOfDaysReviewer = Convert.ToInt32(ConfigurationManager.AppSettings["NoOfDaysReviewer"]);
                    var expireDateReviewer2 = firstDayOfMonth.AddDays(Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["NoOfDaysReviewer"].ToString()) + noOfDaysManager);
                    var reviewercount = expireDateReviewer2.Day;
                    var reviewercount2 = reviewercount - noOfDaysReviewer;
                    HolidayService holidayService2 = new HolidayService();
                    var holidayData2 = holidayService1.GetCurrentMonthHolidays();

                    if (holidayData2.Any())
                    {
                        var fallingHolidays2 = from temp in holidayData2
                                               where temp <= reviewercount && temp >= reviewercount2
                                               select temp;

                        if (fallingHolidays2.Any())
                        {
                         count1 = fallingHolidays2.Count();
                        }
                        expireDateReviewer2 = expireDateReviewer2.AddDays(count1);
                    }

                    if (currentDate == expireDateReviewer2 || currentDate > expireDateReviewer2)
                    {
                        LockReview();
                    }
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
                emial.Process(EventType.SendReviewNominationEmail);

            }
            else
            {

            }
        }
        private void LockReview()
        {
            const string URL = @"https://localhost:44324/api/reviewnominationapi/reviewlock";

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
                emial.Process(EventType.SendAdminNominationEmail);
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
