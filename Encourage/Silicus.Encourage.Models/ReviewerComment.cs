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
    
    public partial class ReviewerComment
    {
        public int Id { get; set; }
        public int NominationId { get; set; }
        public int CriteriaId { get; set; }
        public int ReviewerId { get; set; }
        public string Comment { get; set; }
    
        public virtual Criteria Criterion { get; set; }
        public virtual Nomination Nomination { get; set; }
        public virtual Reviewer Reviewer { get; set; }
    }
}
