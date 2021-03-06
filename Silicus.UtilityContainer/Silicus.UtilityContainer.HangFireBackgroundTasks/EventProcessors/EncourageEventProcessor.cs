﻿using Silicus.UtilityContainer.HangFireBackgroundTasks.Interface;
using System;
using Silicus.UtilityContainer.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Configuration;
using HangFireBackgroundTasks.Services;
using System.Linq;
using Silicus.FrameWorx.Logger;
using Silicus.UtilityContainer.Models.Enumerations;
using HangFireBackgroundTasks.Enums;

namespace HangFireBackgroundTasks.EventProcessors
{
    public class EncourageEventProcessor : IEventProcessor
    {
        EncourageEmailProcessor emailProcessor = new EncourageEmailProcessor();
        ILogger _logger = new DatabaseLogger("name=LoggerDataContext", Type.GetType(string.Empty), (Func<DateTime>)(() => DateTime.UtcNow), string.Empty);


        public void Process(EventType eventType,EventProcess eventProcess,string awardName)
        {
            _logger.Log("EncourageEventProcessor-Process");
            switch (eventProcess)
            {
                case EventProcess.LockEvent:
                    LockEvent(eventType, awardName);
                    break;
                case EventProcess.UnLockEvent:
                    UnLockeEvent(eventType, awardName);
                    break;
                default:
                    break;
            }
        }

        private void LockNomination(string awardName)
        {
            try
            {
                _logger.Log("EncourageEventProcessor-LockNomination-try");
                // string URL = @"https://silicusencouragewithazureadweb.azurewebsites.net/api/nominationapi/LockNominations?frequencyCode=" + frequencyCode.ToString();

                string URL = @"https://localhost:44324/api/nominationapi/LockNominations?awardName=" + awardName;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(URL);

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                // List data response.
                var response = client.GetAsync(URL).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    var result = response.Content.ReadAsStringAsync().Result;
                    UnLockReviews(awardName);
                    emailProcessor.Process(EventType.SendReviewNominationEmail,awardName);

                }
                
            }
            catch (Exception ex)
            {
                _logger.Log("EncourageEventProcessor-LockNomination-" + ex.Message);
                throw;
            }

        }

        private void UnLockNominations(string awardName)
        {
            try
            {
                _logger.Log("EncourageEventProcessor-UnLockNomination-try");
              // string URL = @"https://silicusencouragewithazureadweb.azurewebsites.net/api/nominationapi/UnLockNominations?frequencyCode=" + frequencyCode.ToString();

                string URL = @"https://localhost:44324/api/nominationapi/UnLockNominations?awardName=" + awardName;

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(URL);

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                // List data response.
                var response = client.GetAsync(URL).Result;
                
                _logger.Log("EncourageEventProcessor-UnlockNomination-statusCode-" + response.IsSuccessStatusCode);
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    var result = response.Content.ReadAsStringAsync().Result;
                    emailProcessor.Process(EventType.SendNominationEmail,awardName);

                }

            }
            catch (Exception ex)
            {
                _logger.Log("EncourageEventProcessor-LockNomination-" + ex.Message);
                throw;
            }

        }

        private void LockReview(string awardName)
        {
           // string URL = @"https://silicusencouragewithazureadweb.azurewebsites.net/api/reviewnominationapi/lockreview?frequencyCode=" + frequencyCode.ToString();

            string URL = @"https://localhost:44324/api/reviewnominationapi/lockreview?awardName=" + awardName;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            var response = client.GetAsync(URL).Result;
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!
                var result = response.Content.ReadAsStringAsync().Result;
                emailProcessor.Process(EventType.SendAdminNominationEmail,awardName);
            }
            
        }

        private void UnLockReviews(string awardName)
        {
            //string URL = @"https://silicusencouragewithazureadweb.azurewebsites.net/api/reviewnominationapi/UnLockReview?frequencyCode=" + frequencyCode.ToString();

            string URL = @"https://localhost:44324/api/reviewnominationapi/UnLockReview?awardName=" + awardName;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            var response = client.GetAsync(URL).Result;
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!
                var result = response.Content.ReadAsStringAsync().Result;
               // emailProcessor.Process(EventType.SendReviewNominationEmail);
            }

        }


        private void UnLockeEvent(EventType eventType, string awardName)
        {
            _logger.Log("EncourageEventProcessor-UnLockeEvent");
            switch (eventType)
            {

                case EventType.UnLockNominations:
                    try
                    {
                        UnLockNominations(awardName);

                    }
                    catch (Exception ex)
                    {
                        _logger.Log("EncourageEventProcessor-LockEvent-UnLockNominationsEvent " + ex.Message);
                    }
                    break;
                case EventType.UnLockReviews:
                    try
                    {
                        UnLockReviews(awardName);
                    }
                    catch (Exception ex)
                    {
                        _logger.Log("EncourageEventProcessor-Event-LockEvent-UnLockReviewsEvent");
                    }
                    break;
                default:
                    break;
            }
        }

        #region Lock Nominations/Review
        private void LockEvent(EventType eventType,string awardName)
        {
            var day = ConfigurationManager.AppSettings["FirstDayOfCurrentMonth"];
            var firstDayOfCurrentMonth = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, Convert.ToInt32(day));
            _logger.Log("EncourageEventProcessor-LockEvent");

            using (HolidayService holidayService = new HolidayService())
            {
                var holidays = holidayService.GetCurrentMonthHolidays();
                switch (eventType)
                {
                    #region Lock Nomination
                    case EventType.LockNomination:
                        var managerDays = Convert.ToInt32(ConfigurationManager.AppSettings["NoOfDaysManager"]);
                        DateTime lastDateForNomination = firstDayOfCurrentMonth.AddDays(Convert.ToDouble(ConfigurationManager.AppSettings["NoOfDaysManager"].ToString()));

                        if (holidays.Any())
                        {
                            var fallingHolidaysForManager = from holidaysInRange in holidays
                                                            where holidaysInRange <= managerDays
                                                            select holidaysInRange;

                            var count2 = fallingHolidaysForManager.Count();
                            lastDateForNomination = lastDateForNomination.AddDays(count2);
                        }

                        if (System.DateTime.Now == lastDateForNomination || System.DateTime.Now > lastDateForNomination)
                        {
                            LockNomination(awardName);
                        }
                        break;
                    #endregion

                    #region Lock Review
                    case EventType.LockReview:
                        int count = 0;
                        var noOfDaysManager = Convert.ToInt32(ConfigurationManager.AppSettings["NoOfDaysManager"]);

                        if (holidays.Any())
                        {
                            var fallingHoliday = from holidaysInRange in holidays
                                                 where holidaysInRange <= noOfDaysManager
                                                 select holidaysInRange;

                            count = fallingHoliday.Count();
                        }

                        var dayCount = 0;
                        noOfDaysManager += count;
                        var noOfDaysReviewer = Convert.ToInt32(ConfigurationManager.AppSettings["NoOfDaysReviewer"]);
                        var expireDateReviewer = firstDayOfCurrentMonth.AddDays(Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["NoOfDaysReviewer"].ToString()) + noOfDaysManager);
                        var reviewercount = expireDateReviewer.Day;
                        var reviewerdays = reviewercount - noOfDaysReviewer;

                        if (holidays.Any())
                        {
                            var fallingHolidaysReviewer = from fallingHolidaysReviewerInRange in holidays
                                                          where fallingHolidaysReviewerInRange <= reviewercount && fallingHolidaysReviewerInRange >= reviewerdays
                                                          select fallingHolidaysReviewerInRange;

                            if (fallingHolidaysReviewer.Any())
                            {
                                dayCount = fallingHolidaysReviewer.Count();
                            }

                            expireDateReviewer = expireDateReviewer.AddDays(dayCount);
                        }

                        if (System.DateTime.Now == expireDateReviewer || System.DateTime.Now > expireDateReviewer)
                        {
                            LockReview(awardName);
                        }
                        break;
                    #endregion
                    default:
                        break;
                }

            }
        }
        #endregion
    }
}
