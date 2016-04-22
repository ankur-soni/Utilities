using Aspose.Cells;
using Silicus.Finder.Entities;
using Silicus.Finder.ModelMappingService.Interfaces;
using Silicus.Finder.Models.DataObjects;
using Silicus.Finder.Models.Models;
using Silicus.Finder.Services.Interfaces;
using Silicus.UtilityContainer.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Silicus.Finder.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IDataContext _context;
        IEmployeeService _employeeService;
        Silicus.UtilityContainerr.Entities.ICommonDataBaseContext _utilityCommonDbContext;
        private readonly ICommonMapper _commonMapper;

        public ProjectService(IDataContextFactory dataContextFactory, IEmployeeService employeeService, ICommonMapper commonMapper)
        {
            _context = dataContextFactory.Create(ConnectionType.Ip);
            _employeeService = employeeService;
            _commonMapper = commonMapper;
            _utilityCommonDbContext = _commonMapper.GetCommonDataBAseContext();
        }

        public int AddProject(Project project)
        {
            _context.Add(project);
            return project.ProjectId;
        }

        public int UpdateProject(Project project)
        {
            _context.Update<Project>(project);
            return project.ProjectId;
        }

        public void ArchiveProject(int projectId)
        {
            var project = _context.Query<Project>().FirstOrDefault(p => p.ProjectId == projectId);
            project.ArchiveDate = DateTime.Now.Date;
            _context.Update<Project>(project);
        }

        public IEnumerable<Project> GetProjects()
        {
            var engagementList = _utilityCommonDbContext.Query<Engagement>().ToList();
            var projectList = new List<Project>();

            foreach (Engagement engagement in engagementList)
            {
                var project = _commonMapper.MapEngagementToProject(engagement);
                projectList.Add(project);
            }
            //   var projectList = _context.Query<Project>().Where(p => p.ArchiveDate == null).ToList();
            return projectList;
        }

        public Project GetProjectById(int? projectId)
        {
            //  var project = _context.Query<Project>().Where(model => model.ProjectId == projectId).FirstOrDefault();
            var engagement = _utilityCommonDbContext.Query<Engagement>().Where(model => model.ID == projectId).FirstOrDefault();
            var project = _commonMapper.MapEngagementToProject(engagement);
            return project;
        }

        public List<Project> GetProjectsByCriteria(ProjectSearchCriteriaModel criteria)
        {
            // var allProjects = GetProjects().ToList();
            var projectList = GetProjects().ToList(); ;

            if (projectList.Count() != 0)
            {

                if (!string.IsNullOrEmpty(criteria.Status))
                    projectList = (projectList.Where(p => p.Status.GetDescription() == criteria.Status)).ToList();

                if (!string.IsNullOrEmpty(criteria.ProjectType))
                    projectList = (projectList.Where(p => p.ProjectType == criteria.ProjectType)).ToList();

                if (!string.IsNullOrEmpty(criteria.ProjectManager))
                {
                    var employeeIds = _employeeService.GetEmployeeByName(criteria.ProjectManager).Select(pm => pm.EmployeeId).ToList();
                    projectList = projectList.Where(p => employeeIds.Contains(Convert.ToInt32(p.ProjectManagerId))).ToList();
                }

                if (!string.IsNullOrEmpty(criteria.EngagementManager))
                {
                    var employeeIds = _employeeService.GetEmployeeByName(criteria.EngagementManager).Select(pm => pm.EmployeeId).ToList();
                    projectList = projectList.Where(p => employeeIds.Contains(Convert.ToInt32(p.EngagementManagerId))).ToList();
                }

                // //projectList = projectList.Where(p => p.Status.GetDescription().Equals(string.IsNullOrEmpty(criteria.Status) ? p.Status.GetDescription(): criteria.Status.Trim())
                // //    && p.ProjectType.Equals(string.IsNullOrEmpty(criteria.ProjectType) ? p.ProjectType : criteria.ProjectType.Trim())).ToList();

                // projectList = (projectList.Where(p => p.Status.GetDescription() == criteria.Status) ).ToList();

                //// projectList = projectList.Where(p => p.ProjectType==criteria.ProjectType).ToList();

                // if (!string.IsNullOrEmpty(criteria.EngagementManager) && projectList.Count() != 0)
                // {
                //     var employeeIds = _employeeService.GetEmployeeByName(criteria.EngagementManager).Select(pm => pm.EmployeeId).ToList();
                //     projectList = projectList.Where(p => employeeIds.Contains(Convert.ToInt32(p.EngagementManagerId))).ToList();
                // }

                // if (!string.IsNullOrEmpty(criteria.ProjectManager) && projectList.Count() != 0)
                // {
                //     var employeeIds = _employeeService.GetEmployeeByName(criteria.ProjectManager).Select(pm => pm.EmployeeId).ToList();
                //     projectList = projectList.Where(p => employeeIds.Contains(Convert.ToInt32(p.ProjectManagerId))).ToList();
                // }
            }

            return projectList;
        }

        public IEnumerable<Project> GetProjectsByName(string projectName)
        {
            var engagementList = _utilityCommonDbContext.Query<Engagement>().Where(engagement => engagement.Name.Contains(projectName.Trim())).ToList();
            var projectList = new List<Project>();

            foreach (Engagement engagement in engagementList)
                projectList.Add(_commonMapper.MapEngagementToProject(engagement));

            if (projectList.Count == 0)
            {
                var engagementsSearchedById = _utilityCommonDbContext.Query<Engagement>().Where(engagement => engagement.Code.Contains(projectName.Trim())).ToList();
                if (engagementsSearchedById.Count > 0)
                {
                    foreach (Engagement engagement in engagementsSearchedById)
                        projectList.Add(_commonMapper.MapEngagementToProject(engagement));
                }
            }
            return projectList;
        }

        public Project GetProjectDetails(int projectId)
        {
            //var project = _context.Query<Project>().FirstOrDefault(p => p.ProjectId == projectId);
            var engagement = _utilityCommonDbContext.Query<Engagement>().Where(model => model.ID == projectId).FirstOrDefault();
            var project = _commonMapper.MapEngagementToProject(engagement);
            return project;
        }

        public int GetProjectsCount()
        {
            int count = _utilityCommonDbContext.Query<Engagement>().Count();
            return count;
        }

        public List<SkillSet> GetAllSkills()
        {
            var skills = _context.Query<SkillSet>().ToList();
            return skills;
        }

        public SkillSet GetSkillSetById(int? projectId)
        {
            var skillset = _context.Query<SkillSet>().Where(model => model.SkillSetId == projectId).FirstOrDefault();
            return skillset;
        }

        public List<Employee> GetAllEmployees()
        {
            var allUsers = _utilityCommonDbContext.Query<User>().ToList();
            var allEmployeeList = new List<Employee>();

            foreach (var user in allUsers)
                allEmployeeList.Add(_commonMapper.MapUserToEmployee(user));

            // var allEmployeeList = _context.Query<Employee>().ToList();

            //foreach (Employee emp in allEmployeeList)
            //{
            //    if (emp.EmployeeTitles.Count > 0)
            //    {
            //        var empTitle = emp.EmployeeTitles.Where(title => title.IsCurrent == true).SingleOrDefault().Title;
            //        emp.Title = empTitle.Name;
            //        emp.TitleId = empTitle.TitleId;
            //    }
            //}

            return allEmployeeList;
        }

        public List<Employee> GetAllManagers()
        {
            var allEmployeeList = GetAllEmployees();
            var managers = allEmployeeList.Where(emp => emp.TitleId == 1).ToList();
            return managers;
        }


        public Employee GetEmployeeById(int? projectId)
        {
            var employee = _context.Query<Employee>().Where(model => model.EmployeeId == projectId).FirstOrDefault();
            return employee;
        }

        public IEnumerable<Employee> GetEmployeesAssignedToProject(int projectId)
        {
            var employeesOnProject = _context.Query<Project>().Where(model => model.ProjectId == projectId).First().Employees;
            return employeesOnProject;
        }

        public int AllocateEmployeesToProject(int projectId, int[] employeeIds)
        {
            var project = _context.Query<Project>().FirstOrDefault(p => p.ProjectId == projectId);
            _context.Attach<Project>(project);

            foreach (int empId in employeeIds)
            {
                var employeeToAllocate = _context.Query<Employee>().Where(model => model.EmployeeId == empId).FirstOrDefault();
                _context.Attach<Employee>(employeeToAllocate);
                project.Employees.Add(employeeToAllocate);
            }

            var updatedProjectId = _context.Update<Project>(project);
            return updatedProjectId;
        }

        public int DeallocateEmployeeFromProject(int empId, int projectId)
        {
            Project project = _context.Query<Project>().Where(model => model.ProjectId == projectId).FirstOrDefault();
            Employee employeeToRemove = project.Employees.Where(model => model.EmployeeId == empId).FirstOrDefault();

            _context.Attach<Project>(project);
            _context.Attach<Employee>(employeeToRemove);
            var isEmployeeRemoved = project.Employees.Remove(employeeToRemove);

            var updatedProjectId = _context.Update<Project>(project);
            return updatedProjectId;
        }

        public int AddSkillToProject(int[] skillIds, int projectID)
        {
            var project = _context.Query<Project>().FirstOrDefault(p => p.ProjectId == projectID);
            _context.Attach<Project>(project);

            foreach (int skillId in skillIds)
            {
                var skillToAdd = _context.Query<SkillSet>().Where(model => model.SkillSetId == skillId).FirstOrDefault();
                _context.Attach<SkillSet>(skillToAdd);
                project.SkillSets.Add(skillToAdd);
            }
            var updatedProjectId = _context.Update<Project>(project);
            return updatedProjectId;
        }

        public int RemoveSkillFromProject(int skillId, int projectId)
        {
            var project = _context.Query<Project>().Where(model => model.ProjectId == projectId).FirstOrDefault();
            var skillToRemove = _context.Query<SkillSet>().Where(model => model.SkillSetId == skillId).FirstOrDefault();

            _context.Attach<Project>(project);
            _context.Attach<SkillSet>(skillToRemove);
            var isRemoved = project.SkillSets.Remove(skillToRemove);

            var updatedProjectId = _context.Update<Project>(project);
            return updatedProjectId;
        }

        public int GetProjectCountBySkill(int skillSetId)
        {
            var projectCount = _context.Query<Project>().Where(model => model.SkillSets.Any(r => r.SkillSetId == skillSetId)).Count();
            return projectCount;
        }


        //Import Projects..
        public List<Project> ImportProjectsFromExcel(string path)
        {
            LoadOptions loadOptionForXlsx = new LoadOptions(LoadFormat.Xlsx);
            Workbook workbook = new Workbook(path, loadOptionForXlsx);
            char column = 'A';
            var project = new Project();
            var projects = new List<Project>();
            var dbProjects = GetProjects();


            var rowcount = workbook.Worksheets["Sheet1"].Cells.MaxDataRow;



            for (int index = 2; index <= rowcount + 1; )
            {

            NewRow:
                var cell = workbook.Worksheets["Sheet1"].Cells[column + index.ToString()];
                switch (column)
                {

                    case 'A':
                        project.ProjectName = cell.StringValue;
                        ++column;
                        break;
                    case 'B':
                        project.ProjectCode = cell.StringValue;
                        ++column;
                        break;
                    case 'C':
                        project.Description = cell.StringValue;
                        ++column;
                        break;
                    case 'D':
                        //   project.ProjectType = ParseEnum<ProjectType>(cell.StringValue);
                        ++column;
                        break;
                    case 'E':
                        project.EngagementType = ParseEnum<Silicus.Finder.Models.DataObjects.EngagementType>(cell.StringValue);
                        ++column;
                        break;

                    case 'F':
                        project.EngagementManagerId = cell.IntValue;
                        ++column;
                        break;

                    case 'G':
                        project.ProjectManagerId = cell.IntValue;
                        ++column;
                        break;
                    case 'H':
                        project.Status = ParseEnum<Status>(cell.StringValue);
                        ++column;
                        break;
                    case 'I':
                        project.StartDate = cell.DateTimeValue;
                        ++column;
                        break;
                    case 'J':
                        project.ExpectedEndDate = cell.DateTimeValue;
                        ++column;
                        break;
                    case 'K': project.ActualEndDate = cell.DateTimeValue;
                        ++column;
                        break;


                    case 'L':
                        project.AdditionalNotes = cell.StringValue;
                        ++column;
                        break;



                    case 'M':

                        foreach (var dbproject in dbProjects)
                        {
                            if (dbproject.ProjectCode == project.ProjectCode)
                            {

                                ErrorLog(@"D:\Error.txt", "Problem while updating row " + index + " in Project Code");
                                project = new Project();
                                ++index;
                                column = 'A';
                                goto NewRow;
                            }


                        }

                        AddProject(project);
                        project = AddSkills(project, cell.StringValue);
                        project = new Project();
                        ++index;
                        column = 'A';
                        break;




                }
            }
            return projects;
        }

        private Project AddSkills(Project targetProject, string skills)
        {
            _context.Attach<Project>(targetProject);
            var skillList = SeparateAllSkills(skills);

            var skillSet = new List<int>();
            var dbSkills = GetAllSkills();

            foreach (var skillName in skillList)
            {
                foreach (var skill in dbSkills)
                {
                    if (skill.Name.ToLower() == skillName.ToLower())
                    {

                        skillSet.Add(skill.SkillSetId);
                        //  skillSet[i] = skill.SkillSetId;
                        // i++;
                    }
                }

            }

            //  AddSkillToProject(skillSet.ToArray(), targetProject.ProjectId);

            // AttachEntity(targetProject, skillSet);
            foreach (int skillId in skillSet)
            {
                if (skillId >= 1)
                {
                    var skillToAdd = GetSkillSetById(skillId);

                    try
                    {
                        _context.Attach<SkillSet>(skillToAdd);
                        targetProject.SkillSets.Add(skillToAdd);
                        _context.Update<Project>(targetProject);

                    }
                    catch (Exception ex)
                    {
                        ErrorLog(@"D:\Error.txt", "Problem while updating the skills for project with ProjectId " + targetProject.ProjectId);

                    }


                }
            }
            return targetProject;
        }


        private List<string> SeparateAllSkills(string line)
        {
            var skillList = line.Split(',').ToList();
            var AllSkills = new List<string>();
            foreach (var skill in skillList)
            {
                if (skill.Contains('/'))
                {

                    var version = skill.Split(' ');
                    string name = "";
                    int i = 0;
                    while (version[i].Contains('/') == false)
                    {
                        name = name + ' ' + version[i];
                        i++;
                    }
                    var x = version[i].Split('/');
                    for (int j = 0; j < x.Length; j++)
                    {
                        AllSkills.Add(name + ' ' + x[j]);
                    }

                }
                else
                {
                    AllSkills.Add(skill);
                }

            }
            return AllSkills;
        }


        public void ErrorLog(string pathName, string errorMessage)
        {
            StreamWriter streamwriter = new StreamWriter(pathName, true);
            streamwriter.WriteLine(errorMessage);
            streamwriter.Flush();
            streamwriter.Close();
        }



        public List<string> AddAllProjects(List<Project> projects)
        {
            var count = 0;
            var projectcodefailedtoadd = new List<string>();

            foreach (Project project in projects)
            {
                try
                {

                    _context.Add<Project>(project);
                    count++;
                }
                catch (Exception ex)
                {
                    projectcodefailedtoadd.Add(project.ProjectCode);
                }
            }

            return projectcodefailedtoadd;
        }

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

    }
}