using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Silicus.Finder.Models.DataObjects
{
    public class Contact
    {
        [Key]
        [ScaffoldColumn(false)]
        public int ContactId { get; set; }

        [Index("IX_Unique_EmailAddress", 1, IsUnique = true)]
        [Required(ErrorMessage = "Email can't be blank")]
        [StringLength(30)]
        [Column(Order = 2)]
        
        [Remote("DoesEmailExist", "Employee", HttpMethod = "POST", ErrorMessage = "Email address already exists.")]
        [Display(Name = "Email Id")]
        public string EmailAddress { get; set; } // have to make it unique 

        [StringLength(30)]
        [Column(Order = 1)]
        [Display(Name = "Skype Id")]
        public string Skype { get; set; }
        
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; }

        //public virtual Employee Employee { get; set; }
    }
}