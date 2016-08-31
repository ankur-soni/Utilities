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

namespace HangFireBackgroundTasks.EventProcessors
{
    public class EncourageEventProcessor : IEventProcessor
    {
        EncourageEmailProcessor emailProcessor = new EncourageEmailProcessor();
        ILogger _logger = new DatabaseLogger("name=LoggerDataContext", Type.GetType(string.Empty), (Func<DateTime>)(() => DateTime.UtcNow), string.Empty);


        public void Process(EventType eventType,EventProcess eventProcess)
        {
            _logger.Log("EncourageEventProcessor-Process");
            switch (eventProcess)
            {
                case EventProcess.LockEvent:
                    LockEvent(eventType);
                    break;
                case EventProcess.UnLockEvent:
                    UnLockeEvent(eventType);
                    break;
                default:
                    break;
            }
        }

        private void LockNomination()
        {
            try
            {
                _logger.Log("EncourageEventProcessor-LockNomination-try");
                const string URL = @"https://silicusencouragewithazureadweb.azurewebsites.net/api/nominationapi/LockNominations";

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
                    UnLockReviews();
                    emailProcessor.Process(EventType.SendReviewNominationEmail);

                }
                
            }
            catch (Exception ex)
            {
                _logger.Log("EncourageEventProcessor-LockNomination-" + ex.Message);
                throw;
            }

        }

        private void UnLockNominations()
        {
            try
            {
                _logger.Log("EncourageEventProcessor-UnLockNomination-try");
                const string URL = @"https://silicusencouragewithazureadweb.azurewebsites.net/api/nominationapi/UnLockNominations";

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
                    emailProcessor.Process(EventType.SendNominationEmail);

                }

            }
            catch (Exception ex)
            {
                _logger.Log("EncourageEventProcessor-LockNomination-" + ex.Message);
                throw;
            }

        }

        private void LockReview()
        {
            const string URL = @"https://silicusencouragewithazureadweb.azurewebsites.net/api/reviewnominationapi/lockreview";

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
                emailProcessor.Process(EventType.SendAdminNominationEmail);
            }
            
        }

        private void UnLockReviews()
        {
            const string URL = @"https://silicusencouragewithazureadweb.azurewebsites.net/api/reviewnominationapi/UnLockReview";

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


        private void UnLockeEvent(EventType eventType)
        {
            _logger.Log("EncourageEventProcessor-UnLockeEvent");
            switch (eventType)
            {

                case EventType.UnLockNominations:
                    try
                    {
                        UnLockNominations();

                    }
                    catch (Exception ex)
                    {
                        _logger.Log("EncourageEventProcessor-LockEvent-UnLockNominationsEvent " + ex.Message);
                    }
                    break;
                case EventType.UnLockReviews:
                    try
                    {
                        UnLockReviews();
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
        private void LockEvent(EventType eventType)
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
                            LockNomination();
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
                            LockReview();
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