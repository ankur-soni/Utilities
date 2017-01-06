using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Silicus.Ensure.Web.Models
{
    public class PanelMemberDetailViewModel:UserDetailViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public List<string> PanelIds { get; set; }
    }
}