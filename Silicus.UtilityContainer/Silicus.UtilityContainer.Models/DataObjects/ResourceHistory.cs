using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.UtilityContainer.Models.DataObjects
{
    public partial class ResourceHistory
    {
        public int ID { get; set; }
        public int ResourceID { get; set; }
        public System.DateTime EffectiveDate { get; set; }
        public int ResourceTypeID { get; set; }
        public int CostCenterID { get; set; }
        public int LocationID { get; set; }
        public int TitleID { get; set; }
        public bool IsBillable { get; set; }
        public bool IsTemporary { get; set; }
        public bool IsActive { get; set; }
        public bool CanApproveOwnTime { get; set; }
        public bool IsTrackingMissingTime { get; set; }
        public bool IsSendingMissingTimeEmail { get; set; }
        public bool IsOverrideStandardResourceDirectCost { get; set; }
        public Nullable<int> CurrencyID { get; set; }
        public decimal DefaultHourlyCost { get; set; }
        public Nullable<int> LeavePolicyID { get; set; }
    }
}
