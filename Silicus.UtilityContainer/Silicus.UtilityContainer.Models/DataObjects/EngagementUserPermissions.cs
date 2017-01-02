using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Silicus.UtilityContainer.Models.DataObjects
{
   
    public partial class EngagementUserPermission
    {
        public int ID { get; set; }
        public Nullable<int> EngagementID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<bool> CanActAsPM { get; set; }
        public Nullable<bool> IncludeOnEmailList { get; set; }
        public bool IsEngagementManager { get; set; }
    }
}
