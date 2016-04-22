using AutoMapper;
using Silicus.Finder.Services.Interfaces;
using Silicus.Finder.Web.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Silicus.Finder.Models.DataObjects;
using System.Data.Entity.Infrastructure;
//using Silicus.Finder.IdentityWrapper;
using Silicus.Finder.Services.Comparable.SkillsComparable;
using Silicus.Finder.Services.Comparable.ProjectComparable;
using System.Web;
using System.IO;
using System;
using System.Data.Entity.Validation;
using System.Diagnostics;
using Silicus.Finder.Models.Models;
using Silicus.Finder.Web.Filters;

namespace Silicus.Finder.Web.Controllers
{
    //[Authorize(Roles="Admin")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;

        private readonly ISkillSetService _skillSetService;

        private readonly IRolesService _roleService;

        public EmployeeController(IRolesService roleService, IEmployeeService employeeService, ISkillSetService skillSetService)
        {
            _employeeService = employeeService;
            _skillSetService = skillSetService;
            _roleService = roleService;

        }

        [CustomAuthorizeAttribute(AllowedRole = "Admin, Manager, User")]
        [ValidateInput(false)]
        public ActionResult SearchEmployeeByName(string name)
        {
            List<EmployeesListViewModel> employeesListViewModel = new List<EmployeesListViewModel>();
            if (ModelState.IsValid)
            {
                var employeeList = _employeeService.GetEmployeeByName(name);
                Mapper.Map(employeeList, employeesListViewModel);
            }
            var employees = new EmployeesViewModel { Employees = employeesListViewModel, SearchCriteria = new EmployeeSearchCriteriaViewModel() };
            return View("GetAllEmployeesList", employees);
        }

        [CustomAuthorizeAttribute(AllowedRole = "Admin, Manager, User")]
        public ActionResult GetAllEmployees()
        {
            var employeesList = _employeeService.GetAllEmployees();
            var employeesListViewModel = new List<EmployeesListViewModel>();
            Mapper.Map(employeesList, employeesListViewModel);

            var employees = new EmployeesViewModel { Employees = employeesListViewModel, SearchCriteria = new EmployeeSearchCriteriaViewModel() };

            return View("GetAllEmployeesList", employees);
        }


        [CustomAuthorizeAttribute(AllowedRole = "Admin, Manager, User")]
        public ActionResult GetEmployeesByCriteria(EmployeeSearchCriteriaViewModel criteria)
        {
            var searchCriteriaModel = new EmployeeSearchCriteriaModel();
            Mapper.Map(criteria, searchCriteriaModel);

            var employeeList = _employeeService.GetEmployeesByCriteria(searchCriteriaModel);

            var employeeListViewModel = new List<EmployeesListViewModel>();
            Mapper.Map(employeeList, employeeListViewModel);

            var employees = new EmployeesViewModel { Employees = employeeListViewModel, SearchCriteria = new EmployeeSearchCriteriaViewModel() };

            return View("GetAllEmployeesList", employees);
        }


        [CustomAuthorizeAttribute(AllowedRole = "Admin, Manager, User")]
        public ActionResult Details(string id)
        {
            // _roleService.GetRoleDetails();
            var selectedEmployee = _employeeService.GetEmployeeById(id);
            var employeeViewModel = new EmployeeViewModel();
            ViewBag.selectedEmployeesRole = _roleService.GetRoleById(Convert.ToInt32(selectedEmployee.Role)).RoleName;
            Mapper.Map(selectedEmployee, employeeViewModel);
            ViewBag.EmployeesList = _employeeService.GetAllEmployees();
            return PartialView("_Details", employeeViewModel);

        }

        [CustomAuthorizeAttribute(AllowedRole = "Admin, Manager, User")]
        public ActionResult EmployeeDetails(string id)
        {
            var selectedEmployee = _employeeService.GetEmployeeById(id);
            var employeeViewModel = new EmployeeViewModel();
            Mapper.Map(selectedEmployee, employeeViewModel);
            ViewBag.EmployeesList = _employeeService.GetAllEmployees();
            return PartialView("EmployeeDetails", employeeViewModel);

        }


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
