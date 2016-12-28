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

    public partial class Criteria
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Criteria()
        {
            this.AwardCriterias = new HashSet<AwardCriteria>();
            this.ManagerComments = new HashSet<ManagerComment>();
            this.ReviewerComments = new HashSet<ReviewerComment>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public int AwardId { get; set; }

        public virtual Award Award { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AwardCriteria> AwardCriterias { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ManagerComment> ManagerComments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReviewerComment> ReviewerComments { get; set; }
    }
}
