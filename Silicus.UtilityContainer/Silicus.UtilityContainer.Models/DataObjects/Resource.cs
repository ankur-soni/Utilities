//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Silicus.UtilityContainer.Models.DataObjects
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    
    public partial class Resource
    {
        public int ID { get; set; }
        [ForeignKey("User")]
        public int UserID { get; set; }

        public virtual User User { get; set; }
        public bool IsTimecardsRequireApproval { get; set; }
        public Nullable<int> DirectManager1ID { get; set; }
        public bool CanDirectManager1ApproveTime { get; set; }
        public bool CanDirectManager1ApproveExpense { get; set; }
        public bool CanDirectManager1ApproveSkills { get; set; }
        public Nullable<int> DirectManager2ID { get; set; }
        public bool CanDirectManager2ApproveTime { get; set; }
        public bool CanDirectManager2ApproveExpense { get; set; }
        public bool CanDirectManager2ApproveSkills { get; set; }
        public bool IsShared { get; set; }
        public Nullable<System.DateTime> SkillsLastUpdatedOn { get; set; }
        public Nullable<System.DateTime> TerminationDate { get; set; }
        public Nullable<System.DateTime> RehireDate { get; set; }
    }
}