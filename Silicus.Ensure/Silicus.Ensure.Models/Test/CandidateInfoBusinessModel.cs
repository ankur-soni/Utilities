using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Ensure.Models.Test
{
    public class CandidateInfoBusinessModel
    {
        public string Name { get; set; }
        public string RequisitionId { get; set; }
        public decimal TotalExperience { get; set; }
        public string Position { get; set; }
        public DateTime DOB { get; set; }
    }
}
