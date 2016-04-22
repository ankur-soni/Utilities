using AutoMapper;
using Glimpse.Core.Tab;
using Silicus.Finder.Models.DataObjects;
using Silicus.Finder.Services.Interfaces;
using Silicus.Finder.Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using Silicus.Finder.ModelMappingService.Interfaces;
using Silicus.Finder.Web.Filters;

namespace Silicus.Finder.Web.Controllers
{
    public class TechnologyController : Controller
    {
        private readonly ISkillSetService _skillSetService;
        private readonly ICommonMapper _commonMapper;

        public TechnologyController(ISkillSetService skillSetService, ICommonMapper commonMapper)
        {
            _skillSetService = skillSetService;
            _commonMapper = commonMapper;
        }
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult Create()
        {
            var skillset = new SkillSet();
            return View();
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpPost]
        public ActionResult Create(SkillSet skillSet)
        {
            try
            {
                _skillSetService.Add(skillSet);
                return RedirectToAction("GetAllSkillSet");
            }
            catch(DbUpdateException ex)
            {
                ViewBag.message = "Skill Name already exists";
                return View();
            }
            catch
            {
                return View();
            }               
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int SkillSetId)
        {
            _skillSetService.DeleteSkillSet(SkillSetId);
            return RedirectToAction("GetAllSkillSet");
        }

        public ActionResult Details(int skillSetId)
        {
            var selectedSkill = _skillSetService.GetSkillSetById(skillSetId);
            var skillSetListViewModel = new SkillSetViewModel();
            Mapper.Map(selectedSkill, skillSetListViewModel);
            return PartialView("_Details", skillSetListViewModel);
       }

        //[Authorize(Roles = "Admin,Manager,User")]
        [CustomAuthorizeAttribute(AllowedRole = "Admin, Manager, User")]
        public ActionResult GetAllSkillSet()
        {
            var skillSetList =  _skillSetService.GetAllSkills();
            foreach(var skillSet in skillSetList)
            {
                _commonMapper.MapSkillToEmployee(skillSet);
            }
            List<SkillSetViewModel> skillSetListViewModel = new List<SkillSetViewModel>();
            Mapper.Map(skillSetList, skillSetListViewModel);
            return View(skillSetListViewModel);
        }

        [CustomAuthorizeAttribute(AllowedRole = "Admin, Manager, User")]
        public ActionResult GetSkillSetListByName(string name)
        {
            //if (ModelState.IsValid)
            //{
            var skillSetList = _skillSetService.GetSkillSetListByName(name);
            List<SkillSetViewModel> skillSetListViewModel = new List<SkillSetViewModel>();
            Mapper.Map(skillSetList, skillSetListViewModel);

            if (skillSetListViewModel.Count == 0)
            {
                //ViewBag.Message = "Incorrect Technology Name! Please refine your search.";
                //return View("TechnologyNotFound");
                return RedirectToAction("RecordNotFound", "Projects");
            }
            return View("GetAllSkillSet", skillSetListViewModel);
            //}
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpGet]
        public ActionResult Edit(int SkillSetId)
        {
            var selectedSkillSet = _skillSetService.GetSkillSetById(SkillSetId);
            var selectedSkillSetViewModel = new SkillSetViewModel();
            Mapper.Map(selectedSkillSet, selectedSkillSetViewModel);
            return View(selectedSkillSetViewModel);
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        public ActionResult Edit(SkillSet selectedSkillSetViewModel)
        {
            _skillSetService.EditSkillSet(selectedSkillSetViewModel);
            ViewBag.UpdatedskillSet = selectedSkillSetViewModel.Name;
            return RedirectToAction("GetAllSkillSet");
        }

        [HttpPost]
        public ActionResult ImportSkillsFromExcel(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    string path = Path.Combine(Server.MapPath("~/DataFile"), Path.GetFileName(file.FileName));
                    file.SaveAs(path);

                    var skills = _skillSetService.ImportSkillsFromExcel(path);
                    var failedRecords = _skillSetService.AddAllSkills(skills);

                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }

                    var message = string.Empty;
                    foreach (string failedRecordMessage in failedRecords)
                        message = message + failedRecordMessage + ",";
                    Session["ImportSkill"] = "Skills added successfully. Records failed to add:" + message;
                    //ViewBag.Message = "Skill Records added successfully..Records(Employee Codes) failed to add:" + message;
                }

                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            System.Diagnostics.Trace.TraceInformation("Property: {0} Error: {1}",
                                                    validationError.PropertyName,
                                                    validationError.ErrorMessage);
                        }
                    }
                }

                catch (Exception ex)
                {
                    Session["ImportSkill"] = "ERROR:Wrong Fromat.." + ex.Message.ToString();
                }
            }
            else
            {
                Session["ImportSkill"] = "You have not specified a file..";
            }
            return RedirectToAction("GetAllSkillSet");
        }
    }
}