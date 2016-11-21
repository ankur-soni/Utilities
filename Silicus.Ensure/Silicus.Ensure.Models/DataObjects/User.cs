using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Silicus.Ensure.Models.DataObjects
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        //[Required(ErrorMessage = "Name is required!")]
        public string FirstName { get; set; }

       // [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        //[Required(ErrorMessage = "This field is required.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        //[Required(ErrorMessage = "This field is required.")]
        [CompareAttribute("NewPassword", ErrorMessage = "Passwords don't match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        
        //[Required]
        public string Address { get; set; }
        
        //[Required]
        public bool IsActive { get; set; }
        
        //[Required]
        public string Role { get; set; }

       // [Required]
        public string Email { get; set; }

        public string Gender { get; set; }

        public string Position { get; set; }

        public string Experience { get; set; }

        public string Department { get; set; }

        public string TestStatus { get; set; }

        public string ResumePath { get; set; }
        public Guid IdentityUserId { get; set; }
    }
}
