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
    
    public partial class EmployeeFamilyDetail
    {
        public long FamDetID { get; set; }
        public long UserID { get; set; }
        public int RelationshipID { get; set; }
        public string FullName { get; set; }
        public string Occupation { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string PlaceofBirth { get; set; }
        public string Gender { get; set; }
        public string Dependent { get; set; }
        public string BloodGroup { get; set; }
        public string Nationality { get; set; }
        public string Address { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> EmergencyContact { get; set; }
        public string ContactNumber { get; set; }
        public string CountryCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<bool> IsEmergencyContact { get; set; }
    
        public virtual Master_Relation Master_Relation { get; set; }
        public virtual LoginDetail LoginDetail { get; set; }
    }
}
