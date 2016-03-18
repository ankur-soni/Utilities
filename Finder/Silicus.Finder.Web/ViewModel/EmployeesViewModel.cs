using System.Collections.Generic;

namespace Silicus.Finder.Web.ViewModel
{
    public class EmployeesViewModel
    {
        public List<EmployeesListViewModel> Employees { get; set; }
        public EmployeeSearchCriteriaViewModel SearchCriteria { get; set; }
    }
}