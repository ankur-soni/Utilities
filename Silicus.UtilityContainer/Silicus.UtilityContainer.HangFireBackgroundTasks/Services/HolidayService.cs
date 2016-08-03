using Silicus.UtilityContainer.Entities;
using Silicus.UtilityContainer.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangFireBackgroundTasks.Services
{
    
    public class HolidayService
    {
        public IDataContextFactory contextFactory;
        public ICommonDataBaseContext context;
        public HolidayService()
        {
            contextFactory = new DataContextFactory();
            context = contextFactory.CreateCommonDBContext();
        }

        public List<int> GetCurrentMonthHolidays()
        {
           // var a =context.Query<Holiday>().Where(p => p.EventDate.Month == DateTime.Now.Month && p.EventDate.Year == DateTime.Now.Year).Select(p => p.EventDate.Day).ToList();

            return context.Query<Holiday>().Where(p => p.EventDate.Month == DateTime.Now.Month && p.EventDate.Year == DateTime.Now.Year).Select(p=>p.EventDate.Day).ToList();
        }
    }
}
