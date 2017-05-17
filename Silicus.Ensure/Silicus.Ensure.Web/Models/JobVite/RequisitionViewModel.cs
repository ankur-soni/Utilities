using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.Ensure.Web.Models.JobVite
{
    public class HiringManager
    {
        public string Email { get; set; }
        public object EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserId { get; set; }
        public object UserName { get; set; }
    }
    public class Status
    {
        public int code { get; set; }
        public List<object> messages { get; set; }
    }

    public class RequisitionViewModel
    {
        public object ApplyLink { get; set; }
        public object ApproveDate { get; set; }
        public string Bonus { get; set; }
        public string BriefDescription { get; set; }
        public string Category { get; set; }
        public long CloseDate { get; set; }
        public object Company { get; set; }
        public string CompanyId { get; set; }
        public object CustomField { get; set; }
        public string Department { get; set; }
        public string Description { get; set; }
        public bool Distribution { get; set; }
        public string EId { get; set; }
        public object EeoCategory { get; set; }
        public object EmailLanguage { get; set; }
        public object EndDate { get; set; }
        public object EvaluationFormName { get; set; }
        public object FilledOn { get; set; }
        //public List<HiringManager> HiringManagers { get; set; }
        public bool InternalOnly { get; set; }
        public object JobLink { get; set; }
        public string JobSource { get; set; }
        public string JobState { get; set; }
        public string JobType { get; set; }
        public long LastUpdatedDate { get; set; }
        public string Location { get; set; }
        public string LocationCity { get; set; }
        public string LocationCountry { get; set; }
        public string LocationPostalCode { get; set; }
        public string LocationState { get; set; }
        public string PostingType { get; set; }
        public object PreInterviewFormName { get; set; }
        public string PrimaryHiringManagerEmail { get; set; }
        public object PrimaryRecruiterEmail { get; set; }
        public bool @Private { get; set; }
        public object PutOnHoldDate { get; set; }
        public object Recruiters { get; set; }
        public object ReferralBonus { get; set; }
        public string Region { get; set; }
        public string RequisitionId { get; set; }
        public long SentDate { get; set; }
        public object StartDate { get; set; }
        public object SubsidiaryId { get; set; }
        public object SubsidiaryName { get; set; }
        public string Title { get; set; }
        public string Workflow { get; set; }
    }
}