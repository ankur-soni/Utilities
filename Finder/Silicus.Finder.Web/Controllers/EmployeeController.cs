using AutoMapper;
using Silicus.Finder.Services.Interfaces;
using Silicus.Finder.Web.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Silicus.Finder.Models.DataObjects;
using System.Data.Entity.Infrastructure;
using Silicus.Finder.IdentityWrapper;
using Silicus.Finder.Services.Comparable.SkillsComparable;
using Silicus.Finder.Services.Comparable.ProjectComparable;
using System.Web;
using System.IO;
using System;
using System.Data.Entity.Validation;
using System.Diagnostics;
using Silicus.Finder.Models.Models;

namespace Silicus.Finder.Web.Controllers
{
    //[Authorize(Roles="Admin")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;

        private readonly ISkillSetService _skillSetService;

        public EmployeeController(IEmployeeService employeeService, ISkillSetService skillSetService)
        {
            _employeeService = employeeService;
            _skillSetService = skillSetService;
        }

        //[HttpPost]
        //public ActionResult SearchEmployeeByName(string name)
        //{
        //    List<EmployeesListViewModel> employeesListViewModel = new List<EmployeesListViewModel>();
        //    if (ModelState.IsValid)
        //    {
        //        var employeeList = _employeeService.GetEmployeeByName(name);
        //        Mapper.Map(employeeList, employeesListViewModel);
        //    }
        //    var employees = new EmployeesViewModel { Employees = employeesListViewModel, SearchCriteria = new EmployeeSearchCriteriaViewModel() };
        //    return View("GetAllEmployeesList", employees);
        //}

        public ActionResult GetAllEmployees()
        {
            var employeesList = _employeeService.GetAllEmployees();
            var employeesListViewModel = new List<EmployeesListViewModel>();
            Mapper.Map(employeesList, employeesListViewModel);

            var employees = new EmployeesViewModel { Employees = employeesListViewModel,  SearchCriteria = new EmployeeSearchCriteriaViewModel()};

            return View("GetAllEmployeesList", employees);
        }


        //public ActionResult GetEmployeesByCriteria(EmployeeSearchCriteriaViewModel criteria)
        //{
        //    var searchCriteriaModel = new EmployeeSearchCriteriaModel();
        //    Mapper.Map(criteria, searchCriteriaModel);

        //    var employeeList = _employeeService.GetEmployeesByCriteria(searchCriteriaModel);

        //    var employeeListViewModel = new List<EmployeesListViewModel>();
        //    Mapper.Map(employeeList, employeeListViewModel);

        //    var employees = new EmployeesViewModel { Employees = employeeListViewModel, SearchCriteria = new EmployeeSearchCriteriaViewModel() };

        //    return View("GetAllEmployeesList", employees);
        //}
        
        //[Authorize(Roles = "Admin")]
        //[HttpGet]
        //public ActionResult Create()
        //{
        //    var userManager = new UserManager();
        //    var newEmployee = new Employee();
        //    var newViewModelEmployee = new EmployeeCreateViewModel();
        //    Mapper.Map(newEmployee, newViewModelEmployee);
        //    ViewData["Roles"] = new SelectList(userManager.GetAllRoles(), "Name", "Name");
        //    ViewData["Projects"] = new MultiSelectList(_employeeService.GetAllProjects(), "ProjectId", "ProjectName");
        //    ViewData["Skills"] = new MultiSelectList(_employeeService.GetAllSkillSets(), "SkillSetId", "Name");
        //    ViewData["Titles"] = new SelectList(_employeeService.GetAllTitles(), "TitleId", "Name");
        //    //ViewBag.Skills = new MultiSelectList(_employeeService.GetAllRoles(), "Id", "Name");
        //    return View(newViewModelEmployee);
        //}

        //// POST: Employee/Create
        //[HttpPost]
        //public ActionResult Create(Employee newEmployee)
        //{
        //    try
        //    {
        //        var userManager = new UserManager();
        //        ViewData["Roles"] = new SelectList(userManager.GetAllRoles(), "Name", "Name");
        //        ViewData["Projects"] = new MultiSelectList(_employeeService.GetAllProjects(), "ProjectId", "ProjectName");
        //        ViewData["Skills"] = new MultiSelectList(_employeeService.GetAllSkillSets(), "SkillSetId", "Name");
        //        ViewData["Titles"] = new SelectList(_employeeService.GetAllTitles(), "TitleId", "Name");
                
        //        var membershipId = userManager.CreateUserIfNotExist(newEmployee.Contact.EmailAddress, "Silicus@123");
        //        userManager.AssignRoleToUser(membershipId, newEmployee.Role);
        //        newEmployee.MembershipId = membershipId;
        //        _employeeService.SaveEmployee(newEmployee);
        //        TempData["AlertMessage"] = newEmployee.FirstName + " Added successfully ";
        //        return RedirectToAction("GetAllEmployees");
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        var exceptionMessage = ex.InnerException.InnerException.Message;
        //        if (exceptionMessage.Contains("Cannot insert duplicate key row in object 'dbo.Contact' with unique index 'IX_Unique_EmailAddress'"))
        //        {
        //            ViewBag.message = "Email Address aready exists";
        //         }
        //        if (exceptionMessage.Contains("Cannot insert duplicate key row in object 'dbo.CubicleLocation' with unique index 'IX_Unique_CubicleLocation'"))
        //        {
        //            ViewBag.message = "cubicle location already assigned";
        //        }
        //        return View();
        //    }
        //    catch (DbEntityValidationException dbEx)
        //    {
        //        foreach (var validationErrors in dbEx.EntityValidationErrors)
        //        {
        //            foreach (var validationError in validationErrors.ValidationErrors)
        //            {
        //                Trace.TraceInformation("Property: {0} Error: {1}",
        //                                        validationError.PropertyName,
        //                                        validationError.ErrorMessage);
        //            }
        //        }
        //        return View();
        //    }
        //    }



        //[Authorize(Roles = "Admin, Manager")]
        //[HttpGet]
        //public ActionResult Edit(int employeeId)
        //{
        //    var selectedEmployee = _employeeService.GetEmployeeById(employeeId);
        //    var userManager = new UserManager();
        //    ViewData["Roles"] = new SelectList(userManager.GetAllRoles(), "Name", "Name");
        //    ViewData["Titles"] = new SelectList(_employeeService.GetAllTitles(), "TitleId", "Name", selectedEmployee.TitleId);
        //    var newViewModelEditEmployee = new EmployeeEditViewModel();
        //    System.Web.HttpContext.Current.Session["EmployeeList"] = _employeeService.GetAllEmployees();
        //    Mapper.Map(selectedEmployee, newViewModelEditEmployee);
        //    return View(newViewModelEditEmployee);
        //}

        //// POST: Employee/Edit
        //[HttpPost]
        //public ActionResult Edit(Employee selectedEmployee)
        //{
        //    try
        //    { 
                
        //        var userManager = new UserManager();
        //        ViewData["Roles"] = new SelectList(userManager.GetAllRoles(), "Name", "Name");
        //        ViewData["Titles"] = new SelectList(_employeeService.GetAllTitles(), "TitleId", "Name");
        //        _employeeService.EditEmployee(selectedEmployee);
        //        TempData["AlertMessage"] = "Employee : " + selectedEmployee.EmployeeCode + " Edited successfully ";
        //        return RedirectToAction("GetAllEmployees");
        //    }

        //    catch (DbUpdateException ex)
        //    {
        //        var targetEmployee = _employeeService.GetEmployeeById(selectedEmployee.EmployeeId);
        //        selectedEmployee.Projects = targetEmployee.Projects;
        //        selectedEmployee.ProjectId = targetEmployee.ProjectId;
        //        selectedEmployee.SkillSets = targetEmployee.SkillSets;
        //        selectedEmployee.SkillId = targetEmployee.SkillId;
        //        var newViewModelEditEmployee = new EmployeeEditViewModel();
        //        Mapper.Map(selectedEmployee, newViewModelEditEmployee);

        //        var exceptionMessage = ex.InnerException.InnerException.Message;
        //        if (exceptionMessage.Contains("Cannot insert duplicate key row in object 'dbo.Contact' with unique index 'IX_Unique_EmailAddress'"))
        //        {
        //            ViewBag.message = "Email Address aready exists";
        //        }
        //        if (exceptionMessage.Contains("Cannot insert duplicate key row in object 'dbo.CubicleLocation' with unique index 'IX_Unique_CubicleLocation'"))
        //        {
        //            ViewBag.message = "cubicle location already assigned";
        //        }

        //        return View(newViewModelEditEmployee);
        //    }
        //    catch (DbEntityValidationException dbEx)
        //    {
        //        foreach (var validationErrors in dbEx.EntityValidationErrors)
        //        {
        //            foreach (var validationError in validationErrors.ValidationErrors)
        //            {
        //                Trace.TraceInformation("Property: {0} Error: {1}",
        //                                        validationError.PropertyName,
        //                                        validationError.ErrorMessage);
        //            }
        //        }
        //        return View();
        //    }
        //    catch
        //    {
        //        return View("Error");
        //    }

        //}

        //[Authorize(Roles = "Admin, Manager")]
        //[HttpGet]
        //public ActionResult AddProjectToEmployee(int employeeId)
        //{
        //    ViewBag.EmployeeID = employeeId;

        //    var employee = _employeeService.GetEmployeeById(employeeId);
        //    var allProjects = _employeeService.GetAllProjects();
        //    var currentlyAddedProjects = employee.Projects;
        //    var projectList = allProjects.Except(currentlyAddedProjects, new ProjectEqualityComparer());
        //    return View(projectList);
        //}
        //[HttpPost]
        //public ActionResult AddProjectToEmployee(IList<int> projectIds, int currentEmployeeId)
        //{
        //    ViewData["Projects"] = new MultiSelectList(_employeeService.GetAllProjects(), "ProjectId", "ProjectName");
        //    try
        //    {
        //        _employeeService.AddProjectToEmployee(projectIds, currentEmployeeId);
            
        //        return RedirectToAction("Edit", new { employeeId = currentEmployeeId });
        //    }
        //    catch
        //    {
        //        return View("Error");
        //    }

        //}

        //[Authorize(Roles = "Admin, Manager")]
        //[HttpGet]
        //public ActionResult AddSkillSetToEmployee(int employeeId)
        //{
        //    ViewBag.EmployeeID = employeeId;
        //    var employee = _employeeService.GetEmployeeById(employeeId);
        //    var allSkills = _employeeService.GetAllSkillSets();
        //    var currentlyAddedSkills = employee.SkillSets;
        //    var skillSetList = allSkills.Except(currentlyAddedSkills, new SkillsEqualityComparer());
        //    return View(skillSetList);
        //}
        //[HttpPost]
        //public ActionResult AddSkillSetToEmployee(IList<int> skillIds, int currentEmployeeId)
        //{
            
        //    try
        //    {
        //        _employeeService.AddSkillToEmployee(skillIds, currentEmployeeId);
        //         return RedirectToAction("Edit", new { employeeId = currentEmployeeId });
        //    }
        //    catch
        //    {
        //        return View("Error");
        //    }

        //}

        public ActionResult Details(string id)
        {
            var selectedEmployee = _employeeService.GetEmployeeById(id);
            var employeeViewModel = new EmployeeViewModel();
            Mapper.Map(selectedEmployee, employeeViewModel);
            ViewBag.EmployeesList = _employeeService.GetAllEmployees();
            return PartialView("_Details", employeeViewModel);

        }

        public ActionResult EmployeeDetails(string id)
        {
            var selectedEmployee = _employeeService.GetEmployeeById(id);
            var employeeViewModel = new EmployeeViewModel();
            Mapper.Map(selectedEmployee, employeeViewModel);
            ViewBag.EmployeesList = _employeeService.GetAllEmployees();
            return PartialView("EmployeeDetails", employeeViewModel);

        }

        //[Authorize(Roles = "Admin, Manager")]
        //public void Delete(int employeeId)
        //{
        //    _employeeService.Delete(employeeId);
        //    var emoloyees = GetAllEmployees();
        //}

        //[Authorize(Roles = "Admin, Manager")]
        //public ActionResult DeallocateProject(int employeeId, int projectId)
        //{
        //    try
        //    {
        //        _employeeService.DeallocateProject(employeeId, projectId);
        //        //TempData["AlertMessage"] = "project removed from employee  ";
        //        return RedirectToAction("Edit", new { employeeId = employeeId });
        //    }
        //    catch
        //    {
        //        return View("Error");
        //    }
        //}

        //[Authorize(Roles = "Admin, Manager")]
        //public ActionResult DeallocateSkillFromEmployee(int employeeId, int skillId)
        //{
        //    try
        //    {
        //        _employeeService.DeallocateSkillFromEmployee(employeeId, skillId);
        //        //TempData["AlertMessage"] = "project removed from employee  ";
        //        return RedirectToAction("Edit", new { employeeId = employeeId });
        //    }
        //    catch
        //    {
        //        return View("Error");
        //    }
        //}

        //[ChildActionOnly]
        //public ActionResult GetEmployeeCountBySkill(int skillSetId)
        //{
        //    var projectCount = _employeeService.GetProjectCountBySkill(skillSetId);
        //    return PartialView(projectCount);
        //}

        //public ActionResult SearchForTechnologyToAddToEmployee(string technologyName, int employeeId)
        //{
        //    ViewBag.EmployeeID = employeeId;
        //    var employee = _employeeService.GetEmployeeById(employeeId);
        //    var allSkills = _employeeService.GetAllSkillSets();
        //    var currentlyAddedSkills = employee.SkillSets;
        //    var searchedTechnology = _skillSetService.GetSkillSetListByName(technologyName).Except(currentlyAddedSkills, new SkillsEqualityComparer());
        //    return View("AddSkillSetToEmployee", searchedTechnology);

        //}
        //public ActionResult SearchForProjectToAddToEmployee(string projectName, int employeeId)
        //{
        //    ViewBag.EmployeeID = employeeId;
        //    var employee = _employeeService.GetEmployeeById(employeeId);
        //    var allProjects = _employeeService.GetAllProjects();
        //    var currentlyAddedProjects = employee.Projects;
        //    var searchedProjects = _employeeService.GetProjectsByName(projectName).Except(currentlyAddedProjects, new ProjectEqualityComparer());
        //    return View("AddProjectToEmployee", searchedProjects);

        //}

        //[Authorize(Roles = "Admin, Manager")]
        //[HttpGet]
        //public ActionResult CreateRewards()
        //{
        //    var newRewards = new RewardsAndRecognition();
        //    return View(newRewards);
        //}

        // POST: Employee/Create
        //[HttpPost]
        //public ActionResult CreateRewards(RewardsAndRecognition newRewards)
        //{
        //    try
        //    {
        //        _employeeService.SaveReward(newRewards);
        //        return RedirectToAction("GetAllEmployees");
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        ViewBag.message = ex.InnerException.Message;
        //        return View();
        //    }
        //    catch
        //    {
        //        return View("Error");
        //    }
        //}

        //[Authorize(Roles = "Admin, Manager")]
        //[HttpGet]
        //public ActionResult AddRewardToEmployee(int employeeId)
        //{
        //    var targetRmployee = _employeeService.GetEmployeeById(employeeId);
        //    ViewData["Rewards"] = new SelectList(_employeeService.GetAllReward(), "RewardsAndRecognitionId", "RewardsAndRecognitionName");
        //    ViewBag.Employee = targetRmployee.FullName;
        //    var employeeToBeRewarded = new EmployeeRewards();
        //    employeeToBeRewarded.EmployeeId = targetRmployee.EmployeeId;
        //    return View(employeeToBeRewarded);
        //}

        //// POST: Employee/Create
        //[HttpPost]
        //public ActionResult AddRewardToEmployee(EmployeeRewards employeeToBeRewarded)
        //{
        //    try
        //    {
        //        ViewData["Rewards"] = new SelectList(_employeeService.GetAllReward(), "RewardsAndRecognitionId", "RewardsAndRecognitionName");
        //        _employeeService.SaveEmployeeReward(employeeToBeRewarded);
        //        return RedirectToAction("Edit", new { employeeId = employeeToBeRewarded.EmployeeId });
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        ViewBag.message = ex.InnerException.Message;
        //        return View();
        //    }
        //    catch
        //    {
        //        return View("Error");
        //    }
        //}

        //[HttpGet]
        //public ActionResult ImportEmployeesFromExcel()
        //{
        //    //return View();
        //    return PartialView("_ImportEmployeeeFromExcel");
        //}

        //[HttpPost]
        //public ActionResult ImportEmployeesFromExcel(HttpPostedFileBase file)
        // {
        //    if (file != null && file.ContentLength > 0)
        //    {
        //        try
        //        {
        //            string path = Path.Combine(Server.MapPath("~/DataFile"),Path.GetFileName(file.FileName));
        //            file.SaveAs(path);
                    
        //            var employees = _employeeService.ImportEmployeesFromExcel(path);
        //            var failedRecords=_employeeService.AddAllEmployees(employees);
                    
        //            if (System.IO.File.Exists(path))
        //            {
        //                System.IO.File.Delete(path);
        //            }

        //            var message=string.Empty;
        //            foreach (string failedRecordMessage in failedRecords)
        //                message = message + failedRecordMessage+",";

        //            Session["ImportEmployee"]= "Employee Records added successfully.. Records(Employee Codes) failed to add:"+failedRecords.Count + message;
        //        }

        //        catch (DbEntityValidationException dbEx)
        //        {
        //            foreach (var validationErrors in dbEx.EntityValidationErrors)
        //            {
        //                foreach (var validationError in validationErrors.ValidationErrors)
        //                {
        //                    Trace.TraceInformation("Property: {0} Error: {1}",
        //                                            validationError.PropertyName,
        //                                            validationError.ErrorMessage);
        //                }
        //            }
        //        }

        //        catch (Exception ex)
        //        {
        //            Session["ImportEmployee"] = "ERROR:Wrong Fromat.." + ex.Message.ToString();
        //        }
        //        }
        //    else
        //    {
        //        Session["ImportEmployee"] = "You have not specified a file..";
        //    }
        //    return RedirectToAction("GetAllEmployees");
        //}

        //[HttpPost]
        //public JsonResult DoesEmailExist(string EmailAddress)
        //{

        //    var user = _employeeService.GetEmployeeByEmailAddress(EmailAddress);

        //    return Json(user == null);
        //}

        //[HttpPost]
        //public JsonResult DoesUserCodeExist(string EmployeeCode)
        //{

        //    var user = _employeeService.GetEmployeeByEmployeeCodeForValidation(EmployeeCode);

        //    return Json(user == null);
        //}

        [HttpPost]
        public ActionResult ValidateDateEqualOrLess(DateTime GivenOn)
        {
            // validate your date here and return True if validated
            if (GivenOn <= DateTime.Now)
            {
                return Json(true);
            }
            return Json(false);
        }
        


         [HttpPost]
         public ActionResult CheckForSilicusExperience(EmployeeCreateViewModel model)//int SilicusExperienceInMonths, int TotalExperienceInMonths)
         {
                if (model.TotalExperienceInMonths < model.SilicusExperienceInMonths)
                {
                    return Json(false);
                }
                return Json(true);
        }
    }
}
