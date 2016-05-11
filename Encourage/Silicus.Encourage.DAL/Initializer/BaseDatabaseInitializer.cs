using Silicus.Encourage.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Encourage.DAL.Initializer
{
   public class BaseDatabaseInitializer
    {
       public void Seed(EncourageDatabaseContext context)
       {
           var MonthlyFrequency = new FrequencyMaster { FrequencyPeriod="Monthly",Code="MONTHLY"};
           context.Add<FrequencyMaster>(MonthlyFrequency);

           var EOM = new Award { Name = "Employee Of The Month", Code = "EOM", FrequencyId = 1 };
           context.Add<Award>(EOM);

           context.SaveChanges();
       }
    }
}
