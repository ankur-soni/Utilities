using Silicus.Finder.Models.DataObjects;
using Silicus.Finder.Models.Models;
using System.Collections.Generic;

namespace Silicus.Finder.Services.Interfaces
{
    public interface IProjectService
    {
        int AddProject(Project project);

        int UpdateProject(Project project);

        void ArchiveProject(int projectId);

        IEnumerable<Project> GetProjects();

        Project GetProjectById(int? projectId);
        
        List<Project> GetProjectsByCriteria(ProjectSearchCriteriaModel criteria);

        IEnumerable<Project> GetProjectsByName(string projectName);

        Project GetProjectDetails(int projectId);

        List<SkillSet> GetAllSkills();

        SkillSet GetSkillSetById(int? projectId);

        List<Employee> GetAllEmployees();

        List<Employee> GetAllManagers();

        Employee GetEmployeeById(int? projectId);

        IEnumerable<Employee> GetEmployeesAssignedToProject(int projectId);

        int AllocateEmployeesToProject(int projectId, int[] employeeIds);

        int DeallocateEmployeeFromProject(int empId, int projectId);

        int AddSkillToProject(int[] skillIds, int projectID);

        int RemoveSkillFromProject(int skillId, int projectId);

        int GetProjectCountBySkill(int skillSetId);

        List<string> AddAllProjects(List<Project> projects);
     
        List<Project> ImportProjectsFromExcel(string path);

        int GetProjectsCount();
    }
}