using Silicus.Finder.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Finder.Services.Comparable.EmployeeComparable
{
    public class EmployeeEqualityComparer:IEqualityComparer<Employee>
    {
        public int GetHashCode(Employee emp)
        {
            return emp.EmployeeId.GetHashCode();
        }

        public bool Equals(Employee emp1,Employee emp2)
        {
            if (object.ReferenceEquals(emp1, emp2))
                return true;
            if (emp1 == null || emp2 == null)
                return false;
            return emp1.EmployeeId.Equals(emp2.EmployeeId);
         }

    }
}
