using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Encourage.Models.DataObjects
{
    public class Award
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public int FrequencyId { get; set; }

        [ForeignKey("FrequencyId")]
        public virtual FrequencyMaster Frequency { get; set; }
    }

}
