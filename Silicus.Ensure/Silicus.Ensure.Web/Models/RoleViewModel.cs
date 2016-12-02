using System;
using System.ComponentModel.DataAnnotations;

namespace Silicus.Ensure.Web.Models
{
    public class RoleViewModel
    {
        public string RoleId { get; set; }

        [Required]
        public string RoleName { get; set; }
        [Required]
        public string Description { get; set; }
    }
}