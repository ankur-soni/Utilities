using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Encourage.Models.DataObjects
{
    public class Winner
    {
        public int Id { get; set; }
        public int NominationId { get; set; }
        public DateTime WinningDate { get; set; }

        [ForeignKey("NominationId")]
        public Nomination Nominartion { get; set; }

    }
}
