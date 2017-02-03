using Silicus.Encourage.DAL.Interfaces;
using Silicus.Encourage.Models;
using Silicus.Encourage.Services.Enums;
using Silicus.Encourage.Services.Interface;
using Silicus.UtilityContainer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Encourage.Services
{
    public class CustomDateService : ICustomDateService
    {

        private readonly IEncourageDatabaseContext _encourageDbcontext;

        public CustomDateService(DAL.Interfaces.IDataContextFactory contextFactory)
        {
            _encourageDbcontext = contextFactory.CreateEncourageDbContext();
        }
        public DateTime GetCustomDate(int awardId)
        {
            var data = _encourageDbcontext.Query<CustomDate>().FirstOrDefault(x => x.AwardId == awardId);
            var award = _encourageDbcontext.Query<Award>().FirstOrDefault(x => x.Id == awardId);

            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month,1);
            var somDate = currentDate.AddMonths(-1);
            var pinnacleDate = currentDate.AddMonths(-12);

            if (data != null)
            {
                if (award != null && data.IsApplicable)
                {
                    var month = data.Month == null ? DateTime.Today.Month : (int)data.Month;
                    var year = data.Year == null ? DateTime.Today.Year : (int)data.Year;
                    return new DateTime(year, month, 1);

                }
                else if (award != null && !data.IsApplicable)
                {
                    return somDate;

                }
                
            }
            else if (award != null)
            {
                var awardFrequency = _encourageDbcontext.Query<FrequencyMaster>().FirstOrDefault(x => x.Id == award.FrequencyId);

                if (awardFrequency.Code == FrequencyCode.MON.ToString())
                {
                    return somDate;
                }
                else if (awardFrequency.Code == FrequencyCode.YEAR.ToString())
                {
                    return pinnacleDate;
                }
            }

            return currentDate;
        }

        public bool ReSetCustomDate(int awardId)
        {
            var customDate = _encourageDbcontext.Query<CustomDate>().FirstOrDefault(x => x.AwardId == awardId);
            if (customDate != null)
            {
                _encourageDbcontext.Delete(customDate);
                return true;
            }
            return false;
        }

        public bool SetCustomDate(int awardId, int month, int year, int monthsToSubtract, bool isApplicable)
        {
            var customDatesForAward = _encourageDbcontext.Query<CustomDate>().FirstOrDefault(x => x.AwardId == awardId);
            if (customDatesForAward != null)
            {
                if (month > 0)
                {
                    customDatesForAward.Month = month;
                    customDatesForAward.Year = year;
                    customDatesForAward.MonthsToSubtract = monthsToSubtract;
                    customDatesForAward.IsApplicable = isApplicable;
                    _encourageDbcontext.Update(customDatesForAward);
                    return true;
                }
                else if (year > 0)
                {
                    customDatesForAward.Year = year;
                    customDatesForAward.IsApplicable = isApplicable;
                    customDatesForAward.MonthsToSubtract = monthsToSubtract;
                    _encourageDbcontext.Update(customDatesForAward);
                    return true;
                }
            }

            if (month > 0)
            {
                _encourageDbcontext.Add(new CustomDate { AwardId = awardId, Month = month, Year = year, MonthsToSubtract = monthsToSubtract, IsApplicable = isApplicable });
                return true;
            }
            else if (year > 0)
            {
                _encourageDbcontext.Add(new CustomDate { AwardId = awardId, Month = DateTime.Today.Month, Year = year, MonthsToSubtract = monthsToSubtract, IsApplicable = isApplicable });
                return true;
            }
            return false;


        }

        public CustomDate CustomDateDetailsForAward(int awardId)
        {
            return _encourageDbcontext.Query<CustomDate>().FirstOrDefault(c => c.AwardId == awardId);
        }
    }
}
