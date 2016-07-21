﻿using Silicus.Reusable.Models;
using Silicus.Reusable.Services.Interfaces;
using System.Linq;
using System.Web.Mvc;



namespace Silicus.Reusable.Web.Controllers
{
    public class ReusableController : Controller
    {
        private readonly IReusableService _reusableService;
        
        //private readonly TextInfo textInfo;

        public ReusableController(IReusableService reusableService)
        {
            _reusableService = reusableService;
            //textInfo = new CultureInfo("en-US", false).TextInfo;
        }

        // GET: Reusable
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult GetAllList(int id)
        {
            if (_reusableService == null)
                return RedirectToAction("Index");

            var frameworkList = _reusableService.GetAllFrameworks(id);
            return View(frameworkList.ToList());
        }

        public ActionResult GetAllCategories()
        {
            if (_reusableService == null)
                return RedirectToAction("Index");

            var CategoryList = _reusableService.GetAllCategories();
            return View(CategoryList.ToList());
        }

        // GET: Reusable/Details/5
        public ActionResult Details(int id)
        {
            Frameworx framework = _reusableService.FrameworkDetail(id);

            return View(framework);
        }

        // GET: Reusable/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reusable/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Reusable/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Reusable/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Reusable/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Reusable/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
