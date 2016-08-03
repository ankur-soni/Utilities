using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.UtilityContainer.Models.DataObjects
{
    public class Holiday
    {
        [Key]
        public int ID { get; set; }
        public DateTime EventDate { get; set; }
        public string EventName { get; set; }
    }
}
