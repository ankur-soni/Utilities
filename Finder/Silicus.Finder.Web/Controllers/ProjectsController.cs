using AutoMapper;
using Silicus.Finder.Models.DataObjects;
using Silicus.Finder.Models.Models;
using Silicus.Finder.Services.Comparable.EmployeeComparable;
using Silicus.Finder.Services.Comparable.SkillsComparable;
using Silicus.Finder.Services.Interfaces;
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

        [HttpGet]
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult AllocateEmployeesToProject(int projectId)
        {
            ViewBag.ProjectId = projectId;
            var currentProject = _projectService.GetProjectById(projectId);
            var employees = _projectService.GetAllEmployees();
            var employeesNotAssignedToThisProject = employees.Except(currentProject.Employees, new EmployeeEqualityComparer());

            var employeesListViewModel = new List<EmployeeSelectViewModel>();
            Mapper.Map(employeesNotAssignedToThisProject, employeesListViewModel);

            return View(employeesListViewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AllocateEmployeesToProject(int[] empIDs, int currentProjectId)
        {
            var updatedProjectId = _projectService.AllocateEmployeesToProject(currentProjectId, empIDs);
            return RedirectToAction("EditProject", new { projectId = updatedProjectId });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult DeallocateProjectEmployee(int empId, int projectId)
        {
            var updatedProjectId = _projectService.DeallocateEmployeeFromProject(empId, projectId);
            return RedirectToAction("EditProject", new { projectId = projectId });
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult AddSkillsToProject(int projectId)
        {
            ViewBag.ProjectId = projectId;
            var project = _projectService.GetProjectById(projectId);
            var allSkills = _projectService.GetAllSkills();
            var currentlyAddedSkills = project.SkillSets;

            var skillSetList = allSkills.Except(currentlyAddedSkills, new SkillsEqualityComparer());
            return View(skillSetList);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddSkillsToProject(int[] skillIds, int currentProjectId)
        {
            var updatedProjectId = _projectService.AddSkillToProject(skillIds, currentProjectId);
            return RedirectToAction("EditProject", new { projectId = currentProjectId });
        }

        [Authorize(Roles = "Admin, Manager")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RemoveSkillFromProject(int skillId, int projectId)
        {
            var updatedProjectId = _projectService.RemoveSkillFromProject(skillId, projectId);
            return RedirectToAction("EditProject", new { projectId = projectId });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateProject()
        {
            ViewBag.Managers = new SelectList(_projectService.GetAllManagers(), "EmployeeId", "FullName");
            ViewBag.Skills = new SelectList(_projectService.GetAllSkills(), "SkillSetId", "Name");

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateProject(ProjectCreateViewModel projectCreateViewModel)
        {          
            try
            {
                Project project = new Project();
                Mapper.Map(projectCreateViewModel, project);

                var projectId = _projectService.AddProject(project);
                if (projectId >= 0)
                {
                    TempData["AlertMessage"] = project.ProjectName + " Having Project Code : " + project.ProjectCode + " Created Successfully.";
                }
                return RedirectToAction("GetProjects");
            }
            catch (DbUpdateException ex)
            {
                ViewBag.Employees = new SelectList(_projectService.GetAllEmployees(), "EmployeeId", "FullName");
                ViewBag.Skills = new SelectList(_projectService.GetAllSkills(), "SkillSetId", "Name");
                ViewBag.message = "ProjectCode already exists";
                return View("CreateProject");
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
                return View("ServerError");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult EditProject(int projectId)
        {
            var project = _projectService.GetProjectById(projectId);

            ViewBag.EngManager = new SelectList(_projectService.GetAllEmployees(), "EmployeeId", "FullName", project.EngagementManagerId);
            ViewBag.projManager = new SelectList(_projectService.GetAllEmployees(), "EmployeeId", "FullName", project.ProjectManagerId);

            var projectViewModel = new ProjectEditViewModel();
            Mapper.Map(project, projectViewModel);

            return View(projectViewModel);
        }

        [HttpPost]
        public ActionResult EditProject(Project project)
        {
            try
            {
            var updatedProjectId = _projectService.UpdateProject(project);
            if (updatedProjectId >= 0)
            {
                TempData["AlertMessage"] = project.ProjectName + " Project Updated Successfully.";
            }
            return RedirectToAction("GetProjects");
        }
            catch (DbUpdateException ex)
            {
                ViewBag.EngManager = new SelectList(_projectService.GetAllEmployees(), "EmployeeId", "FullName", project.EngagementManagerId);
                ViewBag.projManager = new SelectList(_projectService.GetAllEmployees(), "EmployeeId", "FullName", project.ProjectManagerId);
                ViewBag.message = "ProjectCode already exists";

                var projectViewModel = new ProjectEditViewModel();
                Mapper.Map(project, projectViewModel);

                return View("EditProject", projectViewModel);
            }            
        }

        public ActionResult GetProjects()
        {
            ViewData["Employees"] = _projectService.GetAllEmployees();
            var projectsList = _projectService.GetProjects();

            var projectListViewModel = new List<ProjectListViewModel>();
            Mapper.Map(projectsList, projectListViewModel);

            var projects = new ProjectsViewModel { Projects = projectListViewModel, SearchCriteria = new ProjectSearchCriteriaViewModel() };
            return View("List", projects);
        }
        
        //public ActionResult GetProjectsByCriteria(ProjectSearchCriteriaViewModel criteria)
        //{
        //    IEnumerable<Project> projectsList;

        //    var searchCriteriaModel = new ProjectSearchCriteriaModel();
        //    Mapper.Map(criteria, searchCriteriaModel);

        //    ViewData["Employees"] = _projectService.GetAllEmployees();

        //    projectsList = _projectService.GetProjectsByCriteria(searchCriteriaModel);

        //    var projectListViewModel = new List<ProjectListViewModel>();
        //    Mapper.Map(projectsList, projectListViewModel);

        //    var projects = new ProjectsViewModel { Projects = projectListViewModel, SearchCriteria = new ProjectSearchCriteriaViewModel() };

        //    return View("List", projects);
        //}

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

        public ActionResult GetProjectDetails(int projectId)
        {
            ViewData["Employees"] = _projectService.GetAllEmployees();
            var project = _projectService.GetProjectDetails(projectId);
            var projectViewModel = new ProjectDetailsViewModel();
            Mapper.Map(project, projectViewModel);
            return PartialView("_Details", projectViewModel);
        }


        public ActionResult ProjectDetails(int projectId)
        {
            ViewData["Employees"] = _projectService.GetAllEmployees();

            var project = _projectService.GetProjectDetails(projectId);
            System.Web.HttpContext.Current.Session["PrjId"] = project.ProjectId;
            var projectViewModel = new ProjectDetailsViewModel();
            Mapper.Map(project, projectViewModel);
            return PartialView("ProjectDetails", projectViewModel);
        }


        [Authorize(Roles = "Admin, Manager")]
        public ActionResult ArchiveProject(int projectId)
        {
            _projectService.ArchiveProject(projectId);
            return RedirectToAction("GetProjects");
        }

        [ChildActionOnly]
        public ActionResult GetProjectCountBySkill(int skillSetId)
        {
            var projectCount = _projectService.GetProjectCountBySkill(skillSetId);
            return PartialView(projectCount);
        }

        [HttpGet]
        public ActionResult SearchForTechnologyToAddInProject(string technologyName, int projectId)
        {
            ViewBag.ProjectId = projectId;
            var project = _projectService.GetProjectById(projectId);
            var allSkills = _projectService.GetAllSkills();
            var currentlyAddedSkills = project.SkillSets;
            // var skillSetList = allSkills.Except(currentlyAddedSkills, new SkillsEqualityComparer());
            var searchedTechnology = _skillSetService.GetSkillSetListByName(technologyName).Except(currentlyAddedSkills, new SkillsEqualityComparer());
            return View("AddSkillsToProject", searchedTechnology);

        }


        [HttpPost]
        public ActionResult ImportProjectsFromExcel(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    string path = Path.Combine(Server.MapPath("~/DataFile"), Path.GetFileName(file.FileName));
                    file.SaveAs(path);

                    var projects = _projectService.ImportProjectsFromExcel(path);

                //    var failedRecords = _projectService.AddAllProjects(projects);

                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }

                    //var message = string.Empty;
                    //foreach (string failedRecordMessage in failedRecords)
                    //    message = message + failedRecordMessage + ",";

                    ViewBag.Message = "Project Records added successfully..";
                }

                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}",
                                                    validationError.PropertyName,
                                                    validationError.ErrorMessage);
                        }
                    }
                }

                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:Wrong Fromat.." + ex.Message.ToString();
                }
            }
            else
            {
                ViewBag.Message = "You have not specified a file..";
            }
            return RedirectToAction("GetProjects");
        }
    }
}
