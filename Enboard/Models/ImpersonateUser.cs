using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ImpersonateUser
    {
        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Please enter email")]
        [EmailAddress(ErrorMessage = "The email address is not valid")]
        public string Email { get; set; }

    }
}
