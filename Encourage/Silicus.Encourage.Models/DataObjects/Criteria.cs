using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Encourage.Models.DataObjects
{
   public class Criteria
    {
       public int Id { get; set; }
       public int AwardId { get; set; }
       public int Title { get; set; }

       [ForeignKey("AwardId")]
       public virtual Award Award { get; set; }
    }
}
