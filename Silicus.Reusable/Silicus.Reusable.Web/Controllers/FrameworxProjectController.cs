using Silicus.FrameworxProject.Models;
using Silicus.FrameworxProject.Services.Interfaces;
using Silicus.Reusable.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;



namespace Silicus.FrameworxDashboard.Web.Controllers
{

    public class FrameworxProjectController : Controller
    {
        private readonly IFrameworxProjectService _frameworxProjectService;

        public FrameworxProjectController(IFrameworxProjectService frameworxProjectService)
        {
            _frameworxProjectService = frameworxProjectService;
            //textInfo = new CultureInfo("en-US", false).TextInfo;
        }

        [HttpGet]
        public ActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCategory(Category category)
        {
            try
            {
                _frameworxProjectService.AddCategory(category);
                return RedirectToAction("AddComponent");
            }
            catch (DbUpdateException ex)
            {
                ViewBag.message = "Category Name already exists";
                return View();
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult AddComponent()
        {
            ViewData["Categories"] = new MultiSelectList(_frameworxProjectService.GetAllCategories(), "Id", "Name");
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        public ActionResult AddComponent(Frameworx newFrameworx)
        {
            try
            {
                _frameworxProjectService.AddFrameworx(newFrameworx);
                return RedirectToAction("GetAllCategories");
            }
            catch (DbUpdateException ex)
            {
                ViewBag.message = "Category Name already exists";
                return View(ex);
            }
            catch
            {
                return View();
            }
        }

        public ActionResult GetAllCategories()
        {
            if (_frameworxProjectService == null)
                return RedirectToAction("Index");

            var CategoryList = _frameworxProjectService.GetAllCategories();
            return View(CategoryList.ToList());
        }

        public ActionResult GetAllList(int id)
        {
            if (_frameworxProjectService == null)
                return RedirectToAction("Index");

            var frameworkList = _frameworxProjectService.GetAllFrameworxs(id);
            return View(frameworkList.ToList());
        }

        public ActionResult Details(int id)
        {
            List<Frameworx> frameworkList = _frameworxProjectService.GetAllFrameworx();
            var CategoryList = _frameworxProjectService.GetAllCategories();

            FrameworxViewModel frameworxViewModel = new FrameworxViewModel();
            var results = frameworkList.Join(CategoryList,
                wo => wo.CategoryId,
                p => p.Id,
                (order, plan) => new { order.Id, order.Title, order.SourceCodeLink, order.DemoLink, order.HtmlDescription, plan.Name }
                ).ToList();

            var result = results.Where(p => p.Id == id).FirstOrDefault();

            frameworxViewModel.Name = result.Name;
            frameworxViewModel.Title = result.Title;
            frameworxViewModel.HtmlDescription = result.HtmlDescription;

            frameworxViewModel.DemoLink = result.DemoLink;
            frameworxViewModel.SourceCodeLink = result.SourceCodeLink;

            return View(frameworxViewModel);
        }

        public ActionResult SearchComponent(string searchString)
        {
            searchString = searchString.ToLower();
            List<Frameworx> frameworkList = _frameworxProjectService.GetAllFrameworx();
            var CategoryList = _frameworxProjectService.GetAllCategories();
            frameworkList = frameworkList.Where(s => s.Title.ToLower().Contains(searchString)).ToList();

            var query = (from frameworx in frameworkList
                         join catagory in CategoryList
                         on frameworx.CategoryId equals catagory.Id
                         where frameworx.CategoryId == catagory.Id
                         select catagory).Distinct().ToList();

            foreach (var item in query)
            {
                item.Frameworxs = frameworkList.Where(s => s.CategoryId == item.Id).ToList();
            }
            return View("SearchComponent", query.ToList());
        }
    }
}