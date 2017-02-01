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
    
    public partial class Title
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Title()
        {
          
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }
        public int DepartmentID { get; set; }
        public string Name { get; set; }
        public string Initials { get; set; }
        public bool IsActive { get; set; }
        public int LocationID { get; set; }
        public Nullable<int> ResourceTypeID { get; set; }
    
        public virtual Department Department { get; set; }
        public virtual Location Location { get; set; }
       
        public virtual ResourceType ResourceType { get; set; }
       
    }
}