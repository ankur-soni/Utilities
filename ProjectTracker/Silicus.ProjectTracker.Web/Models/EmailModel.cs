using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Silicus.ProjectTracker.Web.Models
{
    [Serializable]
    public class EmailModel
    {
        [Required]
        //[Display(Name = "Enter Name")]
        public string Name { get; set; }

        [Required]
        //[Display(Name = "Enter Email")]
        public string Email { get; set; }
               
    }
}