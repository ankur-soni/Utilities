using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Ensure.Models.DataObjects
{
    public class PanelMemberDetail : UserDetails
    {
        [Key]
        public int Id { get; set; }
        public string PanelIds { get; set; }
    }
}
