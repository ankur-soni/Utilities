//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UtilityDataSyncLibrary
{
    using System;
    using System.Collections.Generic;
    
    public partial class vwExt_ResourceHistory
    {
        public int ID { get; set; }
        public int ResourceID { get; set; }
        public System.DateTime EffectiveDate { get; set; }
        public int ResourceTypeID { get; set; }
        public int LocationID { get; set; }
        public int TitleID { get; set; }
        public bool IsBillable { get; set; }
        public bool IsTemporary { get; set; }
        public bool IsActive { get; set; }
        public bool CanApproveOwnTime { get; set; }
        public bool IsTrackingMissingTime { get; set; }
        public bool IsSendingMissingTimeEmail { get; set; }
        public Nullable<bool> IsOvertimeAllowed { get; set; }
        public Nullable<int> LeavePolicyID { get; set; }
    }
}