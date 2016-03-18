using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using Silicus.Finder.Models.DataObjects;

namespace Silicus.Finder.Web.ViewModel
{
    public class CreateContactViewModel
    {
        [Key]
        [ScaffoldColumn(false)]
        public int ContactId { get; set; }

        [Index("IX_Unique_EmailAddress", 1, IsUnique = true)]
        [Required(ErrorMessage = "Email can't be blank")]
        [StringLength(30)]
        [Column(Order = 2)]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        [Remote("DoesEmailExist", "Employee", HttpMethod = "POST", ErrorMessage = "Email address already exists.")]
        [Display(Name = "Email Id *")]
        public string EmailAddress { get; set; } // have to make it unique 

        [StringLength(30)]
        [Column(Order = 1)]
        [Display(Name = "Skype Id")]
        public string Skype { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        
        [Display(Name = "Mobile Number")]
        public Int64? MobileNumber { get; set; }
    }
}