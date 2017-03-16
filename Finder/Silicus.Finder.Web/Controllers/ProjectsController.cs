using AutoMapper;
using Silicus.Finder.Models.DataObjects;
using Silicus.Finder.Models.Models;
using Silicus.Finder.Services.Comparable.EmployeeComparable;
using Silicus.Finder.Services.Comparable.SkillsComparable;
using Silicus.Finder.Services.Interfaces;
using Silicus.Finder.Web.Filters;
using Silicus.Finder.Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Silicus.Finder.Web.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly ISkillSetService _skillSetService;
        private readonly IEmployeeService _employeeService;

        public ProjectsController(IProjectService projectService, ISkillSetService skillSetService)
        {
            _projectService = projectService;
            _skillSetService = skillSetService;
        }

        [HttpPost]
        [Authorize]
        public ActionResult SearchEmployeeByName(string name, int projectId)
        {
            List<EmployeesListViewModel> employeesListViewModel = new List<EmployeesListViewModel>();
            if (ModelState.IsValid)
            {
                var currentProject = _projectService.GetProjectById(projectId);
                var employees = _projectService.GetAllEmployees();
                var employeesNotAssignedToThisProject = employees.Except(currentProject.Employees, new EmployeeEqualityComparer());
                if (name != null)
                {
                    string _name = name.Trim().ToLower();
                    var employeeList = employeesNotAssignedToThisProject.Where(e => e.FullName.ToLower().Contains(_name)).ToList();
                    Mapper.Map(employeeList, employeesListViewModel);
                }
            }
            ViewBag.ProjectID = projectId;

            return View("GetAllEmployeeList", employeesListViewModel);
        }

      // [CustomAuthorizeAttribute(AllowedRole = "Admin, Manager, User")]
        public ActionResult GetProjects()
        {
            ViewData["Employees"] = _projectService.GetAllEmployees();
            var projectsList = _projectService.GetProjects();

            var projectListViewModel = new List<ProjectListViewModel>();
            Mapper.Map(projectsList, projectListViewModel);

            var projects = new ProjectsViewModel { Projects = projectListViewModel, SearchCriteria = new ProjectSearchCriteriaViewModel() };
            return View("List", projects);
        }

       // [CustomAuthorizeAttribute(AllowedRole = "Admin, Manager, User")]
        public ActionResult GetProjectsByCriteria(ProjectSearchCriteriaViewModel criteria)
        {
            IEnumerable<Project> projectsList;

            var searchCriteriaModel = new ProjectSearchCriteriaModel();
            Mapper.Map(criteria, searchCriteriaModel);

            ViewData["Employees"] = _projectService.GetAllEmployees();
            projectsList = _projectService.GetProjectsByCriteria(searchCriteriaModel);

            var projectListViewModel = new List<ProjectListViewModel>();
            Mapper.Map(projectsList, projectListViewModel);

            var projects = new ProjectsViewModel { Projects = projectListViewModel, SearchCriteria = new ProjectSearchCriteriaViewModel() };

            return View("List", projects);
        }

        //[CustomAuthorizeAttribute(AllowedRole = "Admin, Manager, User")]
        public ActionResult GetProjectsByName(string projectName)
        {
            IEnumerable<Project> projectsList;

            ViewData["Employees"] = _projectService.GetAllEmployees();
            ViewData["name"] = projectName;

            projectsList = _projectService.GetProjectsByName(projectName);

            var projectListViewModel = new List<ProjectListViewModel>();
            Mapper.Map(projectsList, projectListViewModel);

            var projects = new ProjectsViewModel { Projects = projectListViewModel, SearchCriteria = new ProjectSearchCriteriaViewModel() };

            return View("List", projects);
        }

        public ActionResult RecordNotFound()
        {
            ViewBag.Message = "Incorrect Name! Please refine your search.";
            return View("RecordNotFound");
        }

       // [CustomAuthorizeAttribute(AllowedRole = "Admin, Manager, User")]
        public ActionResult GetProjectDetails(int projectId)
        {
            ViewData["Employees"] = _projectService.GetAllEmployees();
            var project = _projectService.GetProjectDetails(projectId);
            var projectViewModel = new ProjectDetailsViewModel();
            Mapper.Map(project, projectViewModel);
            return PartialView("_Details", projectViewModel);
        }

        //[CustomAuthorizeAttribute(AllowedRole = "Admin, Manager, User")]
        public ActionResult ProjectDetails(int projectId)
        {
            ViewData["Employees"] = _projectService.GetAllEmployees();

            var project = _projectService.GetProjectDetails(projectId);
            System.Web.HttpContext.Current.Session["PrjId"] = project.ProjectId;
            var projectViewModel = new ProjectDetailsViewModel();
            Mapper.Map(project, projectViewModel);
            return PartialView("ProjectDetails", projectViewModel);
        }

        [ChildActionOnly]
        //[CustomAuthorizeAttribute(AllowedRole = "Admin, Manager, User")]
        public ActionResult GetProjectCountBySkill(int skillSetId)
        {
            var projectCount = _projectService.GetProjectCountBySkill(skillSetId);
            return PartialView(projectCount);
        }
    }
}
