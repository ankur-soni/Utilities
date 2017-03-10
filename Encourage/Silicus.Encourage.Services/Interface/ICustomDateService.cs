using Silicus.Encourage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Encourage.Services.Interface
{
   public interface ICustomDateService
    {
        DateTime GetCustomDate(int awardId);
        List<CustomDate> GetAllCustomDates();
        bool SetCustomDate(int awardId, int month, int year,int monthsToSubtract, bool isApplicable);
        bool ReSetCustomDate(int awardId);
        CustomDate CustomDateDetailsForAward(int awardId);
    }
}
