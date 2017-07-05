using Silicus.Ensure.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Silicus.Ensure.Web.Models
{
    public class UserDetailViewModel
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Designation { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }

        public string EmployeeId { get; set; }

        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int UserId { get; set; }
    }

    public class EmployeeTestSuitAssignementViewmodel
    {
        public List<SelectListItem> EmployeeList { get; set; }

        public List<int> SelectedEmployeeId { get; set; }

        public IEnumerable<TestSuite> TestSuitList { get; set; }

        [Required(ErrorMessage = "Reviewer is required")]
        public int ReviewerId { get; set; }
    }
}