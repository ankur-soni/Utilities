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
    
    public partial class EmployeeSkillDetail
    {
        public long Id { get; set; }
        public Nullable<long> UserId { get; set; }
        public Nullable<int> SkillId { get; set; }
        public Nullable<int> ExprInYears { get; set; }
        public Nullable<int> ExprInMonths { get; set; }
        public string OtherSkill { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}