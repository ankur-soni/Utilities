using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.UtilityContainer.Models.DataObjects
{
    public class Engagement
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int ClientID { get; set; }
        public string Code { get; set; }
        public int EngagementTypeID { get; set; }
        public string Stage { get; set; }
        public int LocationID { get; set; }
        public int EngagementManagerID { get; set; }
        public int PrimaryProjectManagerID { get; set; }
        public System.DateTime BeginDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public Nullable<decimal> HoursPerDay { get; set; }
        public string Description { get; set; }
        public bool IsOpenForTime { get; set; }
        public string TimeApprover { get; set; }
        public Nullable<int> TimeApproverID { get; set; }
    }
}
