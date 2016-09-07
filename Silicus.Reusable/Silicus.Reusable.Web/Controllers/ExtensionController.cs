using System.Linq;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;

using Silicus.FrameworxProject.Services.Interfaces;
using Silicus.FrameworxProject.Models;
using PagedList;
using System.Collections.Generic;
using Silicus.Reusable.Web.Models.ViewModel;

namespace Silicus.FrameworxProject.Web.Controllers
{
    [Authorize]
    public class ExtensionController : Controller
    {
        private readonly IExtensionCodeService _extensionCodeService;

        public ExtensionController(IExtensionCodeService extensionCodeService)
        {
            _extensionCodeService = extensionCodeService;
            //textInfo = new CultureInfo("en-US", false).TextInfo;
        }

        // GET: Extension
        public ActionResult Index()
        {
            ViewData["CodeTypes"] = new MultiSelectList(_extensionCodeService.GetAllCodeTypes(), "Id", "Name");
            return View();
        }

        public ActionResult AddExtensionCode()
        {
            ViewData["CodeTypes"] = new MultiSelectList(_extensionCodeService.GetAllCodeTypes(), "Id", "Name");
            return View("addExtensionMethod");
        }

        [HttpPost]
        public ActionResult AddExtensionCode(ExtensionSolution extensionSolution)
        {
            try
            {
                extensionSolution.CreationDate = System.DateTime.Now;
                extensionSolution.FrequentSearchedCount = 0;
                _extensionCodeService.AddExtensionSolution(extensionSolution);
                return RedirectToAction("ShowExtensionCode");
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

        public ActionResult EditExtensionCode(int id)
        {
            var result = _extensionCodeService.GetExtensionMethodById(id);
            ViewData["CodeTypes"] = new MultiSelectList(_extensionCodeService.GetAllCodeTypes(), "Id", "Name");
            return View(result);
        }

        [HttpPost]
        public ActionResult EditExtensionCode(ExtensionSolution extensionSolution)
        {
            try
            {
                extensionSolution.CreationDate = System.DateTime.Now;
                _extensionCodeService.EditExtensionSolution(extensionSolution);
                return RedirectToAction("ExtensionDetail", new { id = extensionSolution.Id });
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

        public ActionResult AddOtherUsefullCode()
        {
            ViewData["CodeTypes"] = new MultiSelectList(_extensionCodeService.GetAllCodeTypes(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult AddOtherUsefullCode(OtherCode otherCode)
        {
            try
            {
                otherCode.CreationDate = System.DateTime.Now;
                otherCode.FrequentSearchedCount = 0;
                _extensionCodeService.AddOtherCode(otherCode);
                return RedirectToAction("ShowOtherCode");
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

        public ActionResult EditOtherUsefullCode(int id)
        {
            var result = _extensionCodeService.GetOtherCodeMethodById(id);
            ViewData["CodeTypes"] = new MultiSelectList(_extensionCodeService.GetAllCodeTypes(), "Id", "Name");
            return View(result);
        }

        [HttpPost]
        public ActionResult EditOtherUsefullCode(OtherCode otherCode)
        {
            try
            {
                otherCode.CreationDate = System.DateTime.Now;
                _extensionCodeService.EditOtherCode(otherCode);
                return RedirectToAction("ShowOtherCodeMethodDetail", new { id = otherCode.Id });
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

        public ActionResult ShowExtensionCode(string sortOrder, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = sortOrder == "date_desc" ?  "Date" : "date_desc";
            ViewBag.FrequentSearchedCountSortParm = sortOrder == "count_desc" ?  "count" : "count_desc";

           // List<ExtensionCodeViewModel> myList = new List<ExtensionCodeViewModel>();
            var extensioncodeList = _extensionCodeService.GetAllExtensionSolution().ToList();
            switch (sortOrder)
            {
                case "count":
                    extensioncodeList = extensioncodeList.OrderBy(s => s.FrequentSearchedCount).ToList();
                    break;
                case "count_desc":
                    extensioncodeList = extensioncodeList.OrderByDescending(s => s.FrequentSearchedCount).ToList();
                    break;
                case "Date":
                    extensioncodeList = extensioncodeList.OrderBy(s => s.CreationDate).ToList();
                    break;
                case "date_desc":
                    extensioncodeList = extensioncodeList.OrderByDescending(s => s.CreationDate).ToList();
                    break;
                default:
                    extensioncodeList = extensioncodeList.OrderByDescending(s => s.CreationDate).ToList();
                    break;
            }
            return View(extensioncodeList.ToList().ToPagedList(page ?? 1, 3));
        }

        public PartialViewResult FrequentSearchedExtension(int? page)
        {
            var ExtensionList = _extensionCodeService.GetAllExtensionSolution().ToList();
            //var list = ExtensionList.OrderByDescending(x => x.FrequentSearchedCount).ToList();

            var list = ExtensionList.OrderByDescending(x => x.FrequentSearchedCount).ToPagedList(page ?? 1, 3);
            return PartialView("_ExtensionCodeList",list);
        }
        

        public PartialViewResult newestExtension(int? page)
        {
            var ExtensionList = _extensionCodeService.GetAllExtensionSolution().ToList();
            var list = ExtensionList.OrderByDescending(x => x.CreationDate).ToPagedList(page ?? 1, 6);
            return PartialView("_ExtensionCodeList", list);
        }

        public ActionResult ExtensionDetail(int id)
        {
            var result = _extensionCodeService.GetExtensionMethodById(id);
            result.FrequentSearchedCount++;
            _extensionCodeService.ExtensionFrequentSearchedCountUpdate(result);

            return View(result);
        }

        public ActionResult SearchExtensionMethodByTitle(string searchString,int? page)
        {
            searchString = searchString.ToLower();
            var ExtensionMethodList = _extensionCodeService.GetAllExtensionSolution().ToList();
            ExtensionMethodList = ExtensionMethodList.Where(s => s.MethodName.ToLower().Contains(searchString)).ToList();

            if (ExtensionMethodList.Count() != 0)
            {
                return View("SearchExtensionMethodByTitle", ExtensionMethodList.ToPagedList(page ?? 1, 3));
            }
            else
            {
                ViewBag.ErrorMessage = "Oops! There is no such Extension Method";
                return View("SearchExtensionMessage");
            }
        }

        public ActionResult ShowOtherCode(string sortOrder , int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = sortOrder == "date_desc" ? "Date" : "date_desc";
            ViewBag.FrequentSearchedCountSortParm = sortOrder == "count_desc" ? "count" : "count_desc";

            var othercodeList = _extensionCodeService.GetAllOtherCodeList().ToList();
            switch (sortOrder)
            {
                case "count":
                    othercodeList = othercodeList.OrderBy(s => s.FrequentSearchedCount).ToList();
                    break;
                case "count_desc":
                    othercodeList = othercodeList.OrderByDescending(s => s.FrequentSearchedCount).ToList();
                    break;
                case "Date":
                    othercodeList = othercodeList.OrderBy(s => s.CreationDate).ToList();
                    break;
                case "date_desc":
                    othercodeList = othercodeList.OrderByDescending(s => s.CreationDate).ToList();
                    break;
                default:
                    othercodeList = othercodeList.OrderByDescending(s => s.CreationDate).ToList();
                    break;
            }
            return View(othercodeList.ToList().ToPagedList(page ?? 1, 3));
        }

        public ActionResult ShowOtherCodeMethodDetail(int id)
        {
            var result = _extensionCodeService.GetOtherCodeMethodById(id);
            result.FrequentSearchedCount++;
            _extensionCodeService.OtherCodeFrequentSearchedCountUpdate(result);
            return View(result);
        }

        public PartialViewResult OtherCodeFrequentSearched(int? page)
        {
            var otherCodeList = _extensionCodeService.GetAllOtherCodeList().ToList();
            var list = otherCodeList.OrderByDescending(x => x.FrequentSearchedCount).ToPagedList(page ?? 1, 3);
            return PartialView("_OtherCodeList", list);
        }


        public PartialViewResult OtherCodenewest(int? page)
        {
            var otherCodeList = _extensionCodeService.GetAllOtherCodeList().ToList();
            var list = otherCodeList.OrderByDescending(x => x.CreationDate).ToPagedList(page ?? 1, 3);
            return PartialView("_OtherCodeList", list);
        }

        public ActionResult SearchOtherCodeMethodByTitle(string searchString, int? page)
        {
            searchString = searchString.ToLower();
            var CodeMethodList = _extensionCodeService.GetAllOtherCodeList().ToList();
            CodeMethodList = CodeMethodList.Where(s => s.MethodName.ToLower().Contains(searchString)).ToList();
            if (CodeMethodList.Count() != 0)
            {
                return View(CodeMethodList.ToPagedList(page ?? 1, 3));
            }
            else
            {
                ViewBag.ErrorMessage = "Oops! There is no such Code Method";
                return View("SearchExtensionMessage");
            }
        }
    }
}