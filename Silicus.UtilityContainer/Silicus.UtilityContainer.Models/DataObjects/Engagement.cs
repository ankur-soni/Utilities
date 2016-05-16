using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.UtilityContainer.Models.DataObjects
{
    public partial class Engagement
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int ClientID { get; set; }
        public string Code { get; set; }
        public int CostCenterID { get; set; }
        public int EngagementTypeID { get; set; }
        public int CurrencyID { get; set; }
        public string Stage { get; set; }
        public int LocationID { get; set; }
        public int EngagementManagerID { get; set; }
        public int PrimaryProjectManagerID { get; set; }
        public System.DateTime BeginDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public Nullable<decimal> HoursPerDay { get; set; }
        public string Description { get; set; }
        public string ResourcesAllowedToEnterTime { get; set; }
        public Nullable<int> ResourcesAllowedToEnterTimeFromCostCenterID { get; set; }
        public bool IsOpenForTime { get; set; }
        public bool IsOpenForTimeTransfers { get; set; }
        public bool IsResourceAllowedToReportTimeForAssignedTask { get; set; }
        public bool IsTimecardDescriptionRequired { get; set; }
        public string TimeApprover { get; set; }
        public Nullable<int> TimeApproverID { get; set; }
        public string DefaultSchedulingMode { get; set; }
        public Nullable<int> ContractTypeID { get; set; }
        public Nullable<decimal> ContractAmount { get; set; }
        public string RevenueRecognitionMethod { get; set; }
        public Nullable<System.DateTime> RevenueWillBeEarnedBy { get; set; }
        public Nullable<bool> IsHoldBack { get; set; }
        public Nullable<decimal> HoldbackAmount { get; set; }
        public Nullable<decimal> HoldbackPercentage { get; set; }
        public string CostContractTerms { get; set; }
        public Nullable<decimal> CostContractAmount { get; set; }
        public string CustomerPONumber { get; set; }
        public string PaymentTerm { get; set; }
        public string ClientPO { get; set; }
        public string OrderGUID { get; set; }
        public Nullable<int> BillingCycleID { get; set; }
    }
}
