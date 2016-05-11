using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Encourage.Models.DataObjects
{
   public class ReviewerComment
    {
        public int Id { get; set; }
        public int NominationId { get; set; }
        public int CriteriaId { get; set; }
        public int ReviewerId { get; set; }
        public string Comment { get; set; }

        [ForeignKey("NominationId")]
        public virtual Nomination Nomination { get; set; }

        [ForeignKey("CriteriaId")]
        public virtual Criteria Criteria { get; set; }

        [ForeignKey("ReviewerId")]
        public virtual Reviewer Reviewer { get; set; }
    }
}
