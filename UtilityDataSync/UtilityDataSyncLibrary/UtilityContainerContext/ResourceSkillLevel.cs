//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UtilityDataSyncLibrary
{
    using System;
    using System.Collections.Generic;
    
    public partial class ResourceSkillLevel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }
        public int ResourceID { get; set; }
        public int SkillID { get; set; }
        public Nullable<byte> SkillLevel { get; set; }
    
        public virtual Resource Resource { get; set; }
        public virtual Skill Skill { get; set; }
    }
}
