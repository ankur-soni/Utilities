//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class EmployeeContactDetail
    {
        public long ConDetID { get; set; }
        public long UserID { get; set; }
        public string PermanantAddLine1 { get; set; }
        public string PermanantAddLine2 { get; set; }
        public string PermanantAddLine3 { get; set; }
        public int PermanantCityID { get; set; }
        public string OtherPermanantCity { get; set; }
        public int PermanantCountryID { get; set; }
        public int PermanantStateID { get; set; }
        public string OtherPermanantState { get; set; }
        public string PermanantZipcode { get; set; }
        public string CurrentAddLine1 { get; set; }
        public string CurrentAddLine2 { get; set; }
        public string CurrentAddLine3 { get; set; }
        public int CurrentCityID { get; set; }
        public string OtherCurrentCity { get; set; }
        public int CurrentCountryID { get; set; }
        public int CurrentStateID { get; set; }
        public string OtherCurrentState { get; set; }
        public string CurrentZipcode { get; set; }
        public string HomePhone { get; set; }
        public string WorkPhone { get; set; }
        public string MobileNumber { get; set; }
        public string OfficialEmail { get; set; }
        public string Email { get; set; }
        public string AnotherContact { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<bool> IsBothAddSame { get; set; }
    
        public virtual Master_City Master_City { get; set; }
        public virtual Master_City Master_City1 { get; set; }
        public virtual Master_Country Master_Country { get; set; }
        public virtual Master_Country Master_Country1 { get; set; }
        public virtual Master_Country Master_Country2 { get; set; }
        public virtual Master_State Master_State { get; set; }
        public virtual Master_State Master_State1 { get; set; }
        public virtual LoginDetail LoginDetail { get; set; }
    }
}