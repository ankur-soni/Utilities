using System;

namespace Silicus.ProjectTracker.Core
{
    public static class DateTimeExtensions
    {
        public static DateTime GetPreviousWeek(this DateTime currentWeek)
        {
            return currentWeek.Subtract(new TimeSpan(7, 0, 0, 0));
        }
    }
}
