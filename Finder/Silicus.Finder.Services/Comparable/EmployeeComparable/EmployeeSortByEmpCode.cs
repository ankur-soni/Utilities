using Silicus.Finder.Models.DataObjects;
using System;
using System.Collections.Generic;

namespace Silicus.Finder.Services.Comparable.EmployeeComparable
{
    class EmployeeSortByEmpCode : IComparer<Employee>
    {
        public int Compare(Employee employee, Employee _employee)
        {
            return string.Compare(employee.EmployeeCode, _employee.EmployeeCode);
            throw new NotImplementedException();
        }
             
    }
}