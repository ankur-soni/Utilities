using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Silicus.Ensure.Web.Models
{
    public class RecruiterMemberDetailViewModel : UserDetailViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Tag is required.")]
        public List<string> Tag { get; set; }
    }
}