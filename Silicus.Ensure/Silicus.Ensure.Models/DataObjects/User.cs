using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Silicus.Ensure.Models.DataObjects
{
    public class User
    {
        public int UserId { get; set; }

        public Guid IdentityUserId { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string CurrentLocation { get; set; }

        public string ContactNumber { get; set; }

        public string ProfilePhotoFilePath { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }

       // [ForeignKey("UserApplicationDetailsRefId")]
        public virtual ICollection<UserApplicationDetails> UserApplicationDetails { get; set; } 
    }
}
