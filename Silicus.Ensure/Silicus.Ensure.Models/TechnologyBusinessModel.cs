using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Ensure.Models
{
    public class TechnologyBusinessModel
    {
        public int TechnologyId { get; set; }
        
        public string TechnologyName { get; set; }
        
        public string Description { get; set; }
        
        public bool IsActive { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }
    }
}
