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
                    var managerDays = Convert.ToInt32(ConfigurationManager.AppSettings["NoOfDaysManager"]);
                    DateTime expireDateManager = firstDayOfMonth.AddDays(Convert.ToDouble(ConfigurationManager.AppSettings["NoOfDaysManager"].ToString()));                 
                    HolidayService holidayService = new HolidayService();
                    var holidayData = holidayService.GetCurrentMonthHolidays();

                    if (holidayData.Any())
                    {
                        var fallingHolidaysformanager = from temp in holidayData
                                              where temp <= managerDays
                                              select temp;

                        var count2 = fallingHolidaysformanager.Count();
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

                    HolidayService holiday = new HolidayService();
                    var days = holiday.GetCurrentMonthHolidays();

                    if (days.Any())
                    {
                        var fallingHoliday = from temp in days
                                             where temp <= noOfDaysManager
                                               select temp;

                        count = fallingHoliday.Count();
                    }
                    var dayCount = 0;
                    noOfDaysManager += count;
                    var noOfDaysReviewer = Convert.ToInt32(ConfigurationManager.AppSettings["NoOfDaysReviewer"]);
                    var expireDateReviewer = firstDayOfMonth.AddDays(Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["NoOfDaysReviewer"].ToString()) + noOfDaysManager);
                    var reviewercount = expireDateReviewer.Day;
                    var reviewerdays = reviewercount - noOfDaysReviewer;
                    //HolidayService holidayService2 = new HolidayService();
                    var holidays = holiday.GetCurrentMonthHolidays();

                    if (holidays.Any())
                    {
                        var fallingHolidaysReviewer = from temp in holidays
                                               where temp <= reviewercount && temp >= reviewerdays
                                                      select temp;

                        if (fallingHolidaysReviewer.Any())
                        {
                            dayCount = fallingHolidaysReviewer.Count();
                        }
                        expireDateReviewer = expireDateReviewer.AddDays(dayCount);
                    }

                    if (currentDate == expireDateReviewer || currentDate > expireDateReviewer)
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
