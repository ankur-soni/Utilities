using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Encourage.Models.DataObjects
{
   public class AwardCriteria
    {
        public int Id { get; set; }

        public int CriteriaId { get; set; }

        public int AwardId { get; set; }

       [ForeignKey("CriteriaId")]
        public Criteria Criteria { get; set; }

       [ForeignKey("AwardId")]
       public Award Award { get; set; }
    }
}
