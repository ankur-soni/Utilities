//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Silicus.Encourage.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Nomination
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Nomination()
        {
            this.ManagerComments = new HashSet<ManagerComment>();
            this.ReviewerComments = new HashSet<ReviewerComment>();
            this.Reviews = new HashSet<Review>();
            this.Shortlists = new HashSet<Shortlist>();
        }
    
        public int Id { get; set; }
        public int AwardId { get; set; }
        public int ManagerId { get; set; }
        public int UserId { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public Nullable<System.DateTime> NominationDate { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<bool> IsLocked { get; set; }
        public Nullable<bool> IsSubmitted { get; set; }
        public byte[] UserImage { get; set; }
        public string Comment { get; set; }
    
        public virtual Award Award { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ManagerComment> ManagerComments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReviewerComment> ReviewerComments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Review> Reviews { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Shortlist> Shortlists { get; set; }
    }
}
