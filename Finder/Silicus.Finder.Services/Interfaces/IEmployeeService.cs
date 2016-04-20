using Silicus.Finder.Models.DataObjects;
using Silicus.Finder.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Finder.Services.Interfaces
{
    public interface IEmployeeService
    {
        //Employee GetEmployee(int id);
       //List<Employee> GetEmployeeByName(string name);
       List<Employee> GetAllEmployees();
       //List<Employee> GetEmployeesByCriteria(EmployeeSearchCriteriaModel criteria);
       //void SaveEmployee(Employee newEmployee);
       Employee GetEmployeeById(string employeeId);
       //Employee GetEmployeeByEmployeeCode(string employeeCode);
       //Employee GetEmployeeByEmployeeCodeForValidation(string employeeCode);
       //Employee GetEmployeeByEmailAddress(string emailAddress);
       //List<Project> GetAllProjects();
       //List<SkillSet> GetAllSkillSets();
       //Project GetProjectById(int projectId);
       //IEnumerable<Project> GetProjectsByName(string projectName);
       //SkillSet GetSkillSetById(int skillSetId);
       //void EditEmployee(Employee selectedEmployee);
       //void DeallocateProject(int employeeId, int projectId);
       //void DeallocateSkillFromEmployee(int employeeId, int skillId);
       //void AddSkillToEmployee(IList<int> skillIds, int currentEmployeeId);
       //void AddProjectToEmployee(IList<int> projectIds, int currentEmployeeId); 
       //void Delete(int id);
       //void SaveReward(RewardsAndRecognition newRewards);
       //int GetProjectCountBySkill(int skillSetId);
       //List<RewardsAndRecognition> GetAllReward();
       //void SaveEmployeeReward(EmployeeRewards employeeToBeRewarded);
       //List<Employee> ImportEmployeesFromExcel(string path);
       //List<string> AddAllEmployees(List<Employee> employees);
       //List<Title> GetAllTitles();
       int GetEmployeesCount();
    }
}
