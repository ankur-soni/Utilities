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
        // private readonly IDataContext context;
        private readonly ICommonMapper _commonMapper;

        //public EmployeeService(IDataContextFactory dataContextFactory)
        //{
        //    this.context = dataContextFactory.Create(ConnectionType.Ip);
        //}


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
                //employeeList = employeeList.Where(e => e.Projects.Any(p => p.ProjectName.Equals(string.IsNullOrEmpty(criteria.Project) ? p.ProjectName : criteria.Project.Trim()))
                //    && e.SkillSets.Any(s => s.Name.Equals(string.IsNullOrEmpty(criteria.SkillSet) ? s.Name : criteria.SkillSet.Trim()))
                //    && e.EmployeeType.ToString().Equals(string.IsNullOrEmpty(criteria.EmployeeType) ? e.EmployeeType.ToString() : criteria.EmployeeType.Trim()))
                //    .ToList();

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

        //public List<Project> GetAllProjects()
        //{
        //    return context.Query<Project>().ToList();
        //}

        //public Project GetProjectById(int projectId)
        //{
        //    return context.Query<Project>().Where(pro => pro.ProjectId == projectId).First();
        //}

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

        //public void EditEmployee(Employee selectedEmployee)
        //{
        //    Employee targetEmployee = GetEmployeeById(selectedEmployee.EmployeeId);

        //    selectedEmployee.ContactId = targetEmployee.ContactId;
        //    selectedEmployee.Contact.ContactId = targetEmployee.Contact.ContactId;
        //    if (selectedEmployee.Contact.EmailAddress != targetEmployee.Contact.EmailAddress)
        //    { 
        //        var userManager = new UserManager();
        //        userManager.ChangeUserNameOfUser(selectedEmployee.Contact.EmailAddress, targetEmployee.Contact.EmailAddress);
        //    }
        //    context.Update<Contact>(selectedEmployee.Contact);
        //    selectedEmployee.CubicleLocationId = targetEmployee.CubicleLocationId;
        //    selectedEmployee.CubicleLocation.CubicleLocationId = targetEmployee.CubicleLocation.CubicleLocationId;
        //    context.Update<CubicleLocation>(selectedEmployee.CubicleLocation);
        //    selectedEmployee = EditEmployeeTitle(selectedEmployee);
        //    //context.Update<EmployeeTitles>(selectedEmployee.EmployeeTitles);
        //    context.Update<Employee>(selectedEmployee);
        //}
        //public void DeallocateSkillFromEmployee(int employeeId, int skillId)
        //{
        //    var targetEmployee = GetEmployeeById(employeeId);
        //    var targetSkill = GetSkillSetById(skillId);
        //    context.Update<Contact>(targetEmployee.Contact);
        //    context.Update<CubicleLocation>(targetEmployee.CubicleLocation);

        //    context.Attach<Employee>(targetEmployee);
        //    context.Attach<SkillSet>(targetSkill);
        //    targetEmployee.SkillSets.Remove(targetSkill);
        //    context.Update<Employee>(targetEmployee);

        //}

        //public void DeallocateProject(int employeeId, int projectId)
        //{
        //    var targetEmployee = GetEmployeeById(employeeId);
        //    var targetProject = GetProjectById(projectId);
        //    context.Update<Contact>(targetEmployee.Contact);
        //    context.Update<CubicleLocation>(targetEmployee.CubicleLocation);

        //    context.Attach<Employee>(targetEmployee);
        //    context.Attach<Project>(targetProject);
        //    targetEmployee.Projects.Remove(targetProject);
        //    context.Update<Employee>(targetEmployee);
        //}

        //public void Delete(int id)
        //{
        //    var userManager = new UserManager();
        //    var selectedEmployee = GetEmployeeById(id);
        //    userManager.DeleteUser(selectedEmployee.Contact.EmailAddress);
        //    selectedEmployee.IsActive = false;
        //    selectedEmployee.ArchieveDate = DateTime.Now.Date;
        //    context.Update(selectedEmployee);
        //}

        //public int GetProjectCountBySkill(int skillSetId)
        //{
        //    var employeeCount = context.Query<Employee>().Where(model => model.SkillSets.Any(r => r.SkillSetId == skillSetId)).Count();
        //    return employeeCount;
        //}

        //public void AddSkillToEmployee(IList<int> skillIds, int currentEmployeeId)
        //{
        //    var targetEmployee = GetEmployeeById(currentEmployeeId);
        //    context.Attach<Employee>(targetEmployee);

        //    foreach (int skillId in skillIds)
        //    {
        //        var skillToAdd = GetSkillSetById(skillId);
        //        context.Attach<SkillSet>(skillToAdd);
        //        targetEmployee.SkillSets.Add(skillToAdd);
        //    }
        //    context.Update<Employee>(targetEmployee);

        //}

        //public void AddProjectToEmployee(IList<int> projectIds, int currentEmployeeId)
        //{
        //    var targetEmployee = GetEmployeeById(currentEmployeeId);
        //    context.Attach<Employee>(targetEmployee);

        //    foreach (int projectId in projectIds)
        //    {
        //        var projectToAdd = GetProjectById(projectId);
        //        context.Attach<Project>(projectToAdd);
        //        targetEmployee.Projects.Add(projectToAdd);
        //    }
        //    context.Update<Employee>(targetEmployee);
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

        //public void SaveReward(RewardsAndRecognition newRewards)
        //{
        //    context.Add<RewardsAndRecognition>(newRewards);
        //}

        //public List<RewardsAndRecognition> GetAllReward()
        //{
        //    return context.Query<RewardsAndRecognition>().ToList();
        //}

        //public void SaveEmployeeReward(EmployeeRewards employeeToBeRewarded)
        //{
        //    context.Add<EmployeeRewards>(employeeToBeRewarded);

        //}

        //public Employee GetEmployeeByEmployeeCode(string employeeCode)
        //{
        //    var employee = context.Query<Employee>().Where(emp => (emp.EmployeeCode == employeeCode && emp.IsActive == true)).SingleOrDefault();
        //    employee.Title = GetEmployeeTitle(employee.EmployeeId);
        //    return employee;
        //}
        //public Employee GetEmployeeByEmployeeCodeForValidation(string employeeCode)
        //{
        //    return context.Query<Employee>().Where(emp => (emp.EmployeeCode == employeeCode)).SingleOrDefault();


        //}

        //public Employee GetEmployeeByEmailAddress(string emailAddress)
        //{
        //    return context.Query<Employee>().Where(emp => (emp.Contact.EmailAddress == emailAddress && emp.IsActive == true)).SingleOrDefault();
        //}


        //public List<Employee> ImportEmployeesFromExcel(string path)
        //{
        //    LoadOptions loadOptionForXlsx = new LoadOptions(LoadFormat.Xlsx);
        //    Workbook workbook = new Workbook(path, loadOptionForXlsx);
        //    char column = 'A';
        //    int index = 2;
        //    var employee = new Employee();
        //    var employees = new List<Employee>();
        //    var cubicleLocation = new CubicleLocation();
        //    var contact = new Contact();
        //    var rowcount = workbook.Worksheets["Sheet1"].Cells.MaxDataRow;
        //    Start:
        //    while ( index <= rowcount + 1 )
        //    {
        //        try { 
        //        var cell = workbook.Worksheets["Sheet1"].Cells[column + index.ToString()];
        //        switch (column)
        //        {
        //            case 'A':
        //                employee.EmployeeCode = cell.StringValue;
        //                ++column;
        //                break;
        //            case 'B':
        //                employee.FirstName = cell.StringValue;
        //                ++column;
        //                break;
        //            case 'C':
        //                employee.LastName = cell.StringValue;
        //                ++column;
        //                break;
        //            case 'D':
        //                employee.MiddleName = cell.StringValue;
        //                ++column;
        //                break;
        //            case 'E':
        //                employee.Gender = ParseEnum<Gender>(cell.StringValue);
        //                ++column;
        //                break;
        //            case 'F':
        //                employee.EmployeeType = ParseEnum<EmployeeType>(cell.StringValue);
        //                ++column;
        //                break;
        //            case 'G':
        //                employee.TitleId = GetTitleIdByName(cell.StringValue);
        //                employee = AddTitle(employee);
        //                ++column;
        //                break;
        //            case 'H':
        //                employee.HighestQualification = cell.StringValue;
        //                ++column;
        //                break;
        //            case 'I':
        //                employee.IsActive = cell.BoolValue;
        //                ++column;
        //                break;
        //            case 'J':
        //                employee.TotalExperienceInMonths = cell.IntValue;
        //                ++column;
        //                break;
        //            case 'K': employee.SilicusExperienceInMonths = cell.IntValue;
        //                ++column;
        //                break;

        //            case 'L':
        //                cubicleLocation.Building = cell.StringValue;
        //                ++column;
        //                break;
        //            case 'M':
        //                cubicleLocation.FloorNumber = cell.IntValue;
        //                ++column;
        //                break;
        //            case 'N':
        //                cubicleLocation.DeskNumber = cell.StringValue;
        //                employee.CubicleLocation = cubicleLocation;
        //                cubicleLocation = new CubicleLocation();
        //                ++column;
        //                break;
        //            case 'O':
        //                contact.EmailAddress = cell.StringValue;
        //                ++column;
        //                break;
        //            case 'P':
        //                contact.Skype = cell.StringValue;
        //                ++column;
        //                break;
        //            case 'Q':
        //                contact.PhoneNumber = cell.StringValue;
        //                ++column;
        //                break;
        //            case 'R':
        //                contact.MobileNumber = (long)cell.DoubleValue;
        //                employee.Contact = contact;
        //                contact = new Contact();
        //                ++column;
        //                break;

        //            case 'S':
        //                employee = AddSkills(employee, cell.StringValue);
        //                CreateUser(employee, "User");
        //                employees.Add(employee);
        //                employee = new Employee();
        //                ++index;
        //                column = 'A';
        //                break;

        //        }
        //        }
        //        catch
        //        {
        //            employee = new Employee();
        //            ++index;
        //            column = 'A';
        //            goto Start;
        //        }
        //    }
        //    return employees;
        //}

        //        Employee CreateUser(Employee employee, string roleName)
        //        {
        //            employee.Role = roleName;
        //            var userManager = new UserManager();
        //            var membershipId = userManager.CreateUserIfNotExist(employee.Contact.EmailAddress, "Silicus@123");
        //            userManager.AssignRoleToUser(membershipId, employee.Role);
        //            employee.MembershipId = membershipId;
        //            return employee;
        //        }

        //        public List<string> AddAllEmployees(List<Employee> employees)
        //        {
        //           var count = 0;
        //           var empCodeFailedToAdd = new List<string>();
        //           var employee = new Employee();
        //Start:
        //           while(count<employees.Count)
        //           {
        //               employee = employees.ElementAt(count);
        //                try
        //                {
        //                    context.Add<Employee>(employee);
        //                    count++;
        //                }
        //                catch (Exception ex)
        //        {
        //            empCodeFailedToAdd.Add(employee.EmployeeCode);
        //            employee = new Employee();
        //                   count++;
        //                    goto Start;

        //                }
        //            }

        //            return empCodeFailedToAdd;
        //        }

        //        public static T ParseEnum<T>(string value)
        //        {
        //            return (T)Enum.Parse(typeof(T), value, true);
        //        }

        //        public List<Title> GetAllTitles()
        //        {
        //            return context.Query<Title>().ToList();
        //        }

        //        private Employee AddTitle(Employee newEmployee)
        //        {
        //            newEmployee.EmployeeTitles = new List<EmployeeTitles>();
        //            newEmployee.EmployeeTitles.Add(
        //            new EmployeeTitles
        //            {
        //                TitleId = newEmployee.TitleId,
        //                IsCurrent = true
        //            });
        //            return newEmployee;
        //        }

        //        private Employee EditEmployeeTitle(Employee selectedEmployee)
        //        {
        //            var employeeTitles = context.Query<EmployeeTitles>().Where(title => title.EmployeeId == selectedEmployee.EmployeeId).ToList();
        //            selectedEmployee.EmployeeTitles = employeeTitles;
        //            foreach (var item in selectedEmployee.EmployeeTitles)
        //            {
        //                context.Attach<EmployeeTitles>(item);
        //                item.IsCurrent = false;
        //            }

        //            var newEmployeeTitle = new EmployeeTitles
        //            {
        //                EmployeeId = selectedEmployee.EmployeeId,
        //                TitleId = selectedEmployee.TitleId,
        //                IsCurrent = true
        //            };

        //            context.Add<EmployeeTitles>(newEmployeeTitle);

        //            selectedEmployee.EmployeeTitles.Add(newEmployeeTitle);
        //            return selectedEmployee;
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

        //private Employee AddSkills(Employee targetEmployee, string skills)
        //{
        //    int index = 0;
        //    var skillList = SeparateAllSkills(skills);
        //    var skillSet = new List<int>();
        //    foreach (var skillName in skillList)
        //    {
        //        var skillInstance = context.Query<SkillSet>().Where(skill => skill.Name.ToLower().Equals(skillName.ToLower())).Select(S => S.SkillSetId).FirstOrDefault();
        //        skillSet.Add(skillInstance);
        //    }
        //Start:
        //    while (index<skillSet.Count)
        //    {
        //        if (skillSet[index] >= 1)
        //        {
        //            var skillId = skillSet[index];
        //            try
        //            {
        //                var skillToAdd = GetSkillSetById(skillId);
        //                context.Attach<SkillSet>(skillToAdd);
        //                targetEmployee.SkillSets.Add(skillToAdd);
        //                index++;
        //            }
        //            catch
        //            {
        //                index++;
        //                goto Start;
        //            }
        //        }
        //        else
        //        {
        //            index++;
        //        }
        //    }
        //    return targetEmployee;
        //}
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

