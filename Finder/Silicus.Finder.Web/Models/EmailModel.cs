using System;
using System.ComponentModel.DataAnnotations;

namespace Silicus.Finder.Web.Models
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