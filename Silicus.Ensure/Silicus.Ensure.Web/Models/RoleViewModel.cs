using System;
using System.ComponentModel.DataAnnotations;

namespace Silicus.Ensure.Web.Models
{
    public class RoleViewModel
    {
        public int RoleId { get; set; }

        [Required]
        public string RoleName { get; set; }
        [Required]
        public string Description { get; set; }
    }
}