using Silicus.Finder.Entities;
using Silicus.Finder.Models.DataObjects;
using Silicus.Finder.Services.Comparable.EmployeeComparable;
using Silicus.Finder.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System;
using Aspose.Cells;
//using Silicus.Finder.IdentityWrapper;
using Silicus.Finder.Models.Models;
using Silicus.Finder.ModelMappingService.Interfaces;
using Silicus.UtilityContainer.Models.DataObjects;

namespace Silicus.Finder.Services
{
    public class EmployeeService : IEmployeeService
    {
       
        private readonly ICommonMapper _commonMapper;

        public EmployeeService(ICommonMapper commonMapper)
        {
            _commonMapper = commonMapper;
        }

        public List<Employee> GetAllEmployees()
        {
            var context = _commonMapper.GetCommonDataBAseContext();

            EmployeeSortByEmpCode employeeSortByEmpCode = new EmployeeSortByEmpCode();
            var userList = context.Query<User>();
            var employeeList = new List<Employee>();
            foreach (var user in userList)
            {
                employeeList.Add(_commonMapper.MapUserToEmployee(user));
            }
            // employeeList = AttachTitleToEmployee(employeeList);
            employeeList.Sort(employeeSortByEmpCode);
            return employeeList;
        }

        public List<Employee> GetEmployeesByCriteria(EmployeeSearchCriteriaModel criteria)
        {
            var employeeList = GetAllEmployees();

            if (employeeList.Count() != 0)
            {
             

               employeeList = employeeList.Where(e => e.Projects.Any(p => p.ProjectName.Equals(string.IsNullOrEmpty(criteria.Project) ? p.ProjectName : criteria.Project.Trim())) && ( e.Title == criteria.Title)
                   &&(e.EmployeeType.ToString().Equals(string.IsNullOrEmpty(criteria.EmployeeType) ? e.EmployeeType.ToString() : criteria.EmployeeType.Trim()))
                   ).ToList();

               

                //if (!string.IsNullOrEmpty(criteria.Title) && employeeList.Count() != 0)
                //{
                //    var titleId = context.Query<Title>().Where(t => t.Name.Equals(criteria.Title.Trim())).Select(t => t.TitleId).FirstOrDefault();
                //    employeeList = employeeList.Where(e => e.EmployeeTitles.Any(et => et.TitleId.Equals(titleId))).ToList();
                //}
            }
            return employeeList;
        }

        public List<Employee> GetEmployeeByName(string name)
        {


            EmployeeSortByEmpCode employeeSortByEmpCode = new EmployeeSortByEmpCode();
            List<Employee> employeeList = new List<Employee>();
            if (name != null)
            {
                string _name = name.Trim().ToLower();

                var usersByName = _commonMapper.GetCommonDataBAseContext().Query<User>().ToList().Where(u => u.DisplayName.Trim().ToLower().Contains(_name)).ToList();


                foreach (var user in usersByName)
                {
                    employeeList.Add(_commonMapper.MapBasicPropertiesOfUserToEmployee(user));
                }


                if (usersByName.Count == 0)
                {

                    var usersByCode = _commonMapper.GetCommonDataBAseContext().Query<User>().ToList().Where(u => u.EmployeeID.ToLower().Contains(_name)).ToList();
                    var empList = new List<Employee>();
                    foreach (var user in usersByCode)
                    {
                        empList.Add(_commonMapper.MapBasicPropertiesOfUserToEmployee(user));
                    }
                    //  employeeList = AttachTitleToEmployee(employeeList);
                    empList.Sort(employeeSortByEmpCode);
                    return empList;
                }

            }
            //  employeeList = AttachTitleToEmployee(employeeList);
            employeeList.Sort(employeeSortByEmpCode);
            return employeeList;
        }

               
        //public List<SkillSet> GetAllSkillSets()
        //{
        //    return context.Query<SkillSet>().ToList();
        //}

        public Employee GetEmployeeById(string employeeId)
        {
            var context = _commonMapper.GetCommonDataBAseContext();
            var user = context.Query<User>().Where(emp => (emp.EmployeeID == employeeId)).SingleOrDefault();
            // employee.TitleId = context.Query<EmployeeTitles>().Where(emp => emp.EmployeeId == employeeId && emp.IsCurrent == true).Select(t => t.TitleId).SingleOrDefault();
            //employee.Title = GetEmployeeTitle(employeeId);
            return _commonMapper.MapUserToEmployee(user);
        }


        public int GetEmployeesCount()
        {
            int count = _commonMapper.GetCommonDataBAseContext().Query<User>().Count();
            return count;
        }


        //public SkillSet GetSkillSetById(int skillSetId)
        //{
        //    return context.Query<SkillSet>().Where(emp => emp.SkillSetId == skillSetId).First();

        //}

        //public Title GetTitleById(int titleId)
        //{
        //    return context.Query<Title>().Where(title => title.TitleId == titleId).First();
        //}

       

       
        //public int GetProjectCountBySkill(int skillSetId)
        //{
        //    var employeeCount = context.Query<Employee>().Where(model => model.SkillSets.Any(r => r.SkillSetId == skillSetId)).Count();
        //    return employeeCount;
        //}

       
        //public IEnumerable<Project> GetProjectsByName(string projectName)
        //{
        //    List<Project> projectList = new List<Project>();
        //    if (projectName != null)
        //    {
        //        string name = projectName.Trim().ToLower();
        //        projectList = context.Query<Project>().Where((project => project.ProjectName.ToLower().Contains(name))).ToList();
        //    }
        //    return projectList;
        //}

        


        //public Employee GetEmployeeByEmployeeCode(string employeeCode)
        //{
        //    var employee = context.Query<Employee>().Where(emp => (emp.EmployeeCode == employeeCode && emp.IsActive == true)).SingleOrDefault();
        //    employee.Title = GetEmployeeTitle(employee.EmployeeId);
        //    return employee;
        //}
        

        //        public List<Title> GetAllTitles()
        //        {
        //            return context.Query<Title>().ToList();
        //        }



        //        private string GetEmployeeTitle(int employeeId)
        //        {
        //            var titleName = context.Query<EmployeeTitles>().Where(title => title.EmployeeId == employeeId && title.IsCurrent == true).Select(title => title.Title.Name).SingleOrDefault();
        //            return titleName;
        //        }

        //        private List<Employee> AttachTitleToEmployee(List<Employee> employeeList)
        //        {
        //            foreach (var employee in employeeList)
        //            {
        //                employee.Title = GetEmployeeTitle(employee.EmployeeId);
        //    }
        //            return employeeList;
        //}

        //        private int GetTitleIdByName(string titleName)
        //        {
        //            var titleId = context.Query<Title>().Where(title => title.Name.Equals(titleName,
        //                   StringComparison.OrdinalIgnoreCase)).Select(t=>t.TitleId).FirstOrDefault();
        //            return titleId;
        //        }

      
        //private List<string> SeparateAllSkills(string line)
        //{
        //    var skillList = line.Split(',').ToList();
        //    var AllSkills = new List<string>();
        //    foreach (var skill in skillList)
        //    {
        //        if (skill.Contains('/'))
        //        {

        //            var version = skill.Split(' ');
        //            string name = "";
        //            int i = 0;
        //            while (version[i].Contains('/') == false)
        //            {
        //                name = name + ' ' + version[i];
        //                i++;
        //            }
        //            var x = version[i].Split('/');
        //            for (int j = 0; j < x.Length; j++)
        //            {
        //                AllSkills.Add(name + ' ' + x[j]);
        //            }

        //        }
        //        else
        //        {
        //            AllSkills.Add(skill);
        //        }

        //    }
        //    return AllSkills;
        //}
    }

}

