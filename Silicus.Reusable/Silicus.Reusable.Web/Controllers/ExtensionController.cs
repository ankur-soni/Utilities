using System.Linq;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;

using Silicus.FrameworxProject.Services.Interfaces;
using Silicus.FrameworxProject.Models;
using PagedList;
using System.Collections.Generic;
using Silicus.Reusable.Web.Models.ViewModel;
using System;
using Silicus.UtilityContainer.Models.DataObjects;
using Silicus.UtilityContainer.Entities;
using Microsoft.IdentityModel.Protocols;
using Silicus.FrameworxProject.Web.Filters;

namespace Silicus.FrameworxProject.Web.Controllers
{
    [Authorize]
    public class ExtensionController : Controller
    {
        private readonly IExtensionCodeService _extensionCodeService;
        // private readonly ICommonDbService _commonDbService;
        private readonly ICommonDataBaseContext _commonDataBaseContext = new CommonDataBaseContext(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

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

        // [CustomeAuthorize(AllowedRole = "Reviewer")]
        public ActionResult AddExtensionCode()
        {
            ViewData["CodeTypes"] = new MultiSelectList(_extensionCodeService.GetAllCodeTypes(), "Id", "Name");
            return View("addExtensionMethod");
        }

        [HttpPost]
        // [CustomeAuthorize(AllowedRole = "Reviewer")]
        public ActionResult AddExtensionCode(ExtensionSolution extensionSolution)
        {
            try
            {
                var userEmailAddress = User.Identity.Name;
                extensionSolution.CreationDate = System.DateTime.Now;
                extensionSolution.FrequentSearchedCount = 0;
                User user = new User();
                user = _commonDataBaseContext.Query<User>().Where(u => u.EmailAddress == userEmailAddress).FirstOrDefault();
                List<UtilityUserRoles> Reviewers = new List<UtilityUserRoles>();
                Reviewers = _commonDataBaseContext.Query<UtilityUserRoles>().Where(u => u.UtilityId == 4 && u.RoleId == 371).ToList();
                extensionSolution.UserDisplayName = user.DisplayName;
                extensionSolution.userid = user.ID;
                _extensionCodeService.AddExtensionSolution(extensionSolution);

                List<ExtensionSolution> ExtensionSolutionList = new List<ExtensionSolution>();
                ExtensionSolutionList = _extensionCodeService.GetAllExtensionSolution();

                var countofReviewer = ExtensionSolutionList.Count() % Reviewers.Count();
                var SelectedReviewer = Reviewers.ElementAt(countofReviewer);
                extensionSolution.reviewerid = SelectedReviewer.UserId;

                _extensionCodeService.EditExtensionSolution(extensionSolution);
                EmailFormModel model = new EmailFormModel();
                model.FromName = "Devendra Birthare";
                model.FromEmail = "birthare06@gmail.com";
                model.ToEmail = "devendra.birthare@silicus.com";
                model.Message = "This is a sample message" + extensionSolution.reviewerid;
                _extensionCodeService.EmailSendToReviewer(model);
                return RedirectToAction("ShowMyExtensionCode");
            }
            catch (DbUpdateException ex)
            {
                ViewBag.message = "Category Name already exists";
                return View(ex);
            }
            catch (Exception e)
            {
                return View(e);
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
                _extensionCodeService.EditExtensionSolution(extensionSolution);
                extensionSolution.ReviewFlag = true;
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

        public ActionResult ReviewExtensionCode(int id)
        {
            var result = _extensionCodeService.GetExtensionMethodById(id);
            ViewData["CodeTypes"] = new MultiSelectList(_extensionCodeService.GetAllCodeTypes(), "Id", "Name");
            return View(result);
        }

        [HttpPost]
        public ActionResult ReviewExtensionCode(ExtensionSolution extensionSolution)
        {
            try
            {

                extensionSolution.ReviewFlag = true;
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
                var userEmailAddress = User.Identity.Name;
                User user = new User();
                user = _commonDataBaseContext.Query<User>().Where(u => u.EmailAddress == userEmailAddress).FirstOrDefault();
                otherCode.UserDisplayName = user.DisplayName;
                otherCode.userid = user.ID;
                otherCode.CreationDate = System.DateTime.Now;
                otherCode.FrequentSearchedCount = 0;
                _extensionCodeService.AddOtherCode(otherCode);


                List<UtilityUserRoles> Reviewers = new List<UtilityUserRoles>();
                Reviewers = _commonDataBaseContext.Query<UtilityUserRoles>().Where(u => u.UtilityId == 4 && u.RoleId == 371).ToList();


                List<OtherCode> OtherCodeList = new List<OtherCode>();
                OtherCodeList = _extensionCodeService.GetAllOtherCodeList();

                var countofReviewer = OtherCodeList.Count() % Reviewers.Count();
                var SelectedReviewer = Reviewers.ElementAt(countofReviewer);
                otherCode.reviewerid = SelectedReviewer.UserId;
                _extensionCodeService.EditOtherCode(otherCode);

                EmailFormModel model = new EmailFormModel();
                model.FromName = "Devendra Birthare";
                model.FromEmail = "birthare06@gmail.com";
                model.ToEmail = "devendra.birthare@silicus.com";
                model.Message = "This is a sample message" + otherCode.reviewerid;
                _extensionCodeService.EmailSendToReviewer(model);

                return RedirectToAction("ShowMyOtherCode");
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

        public ActionResult ReviewOtherCodeMethod(int id)
        {
            var result = _extensionCodeService.GetOtherCodeMethodById(id);
            ViewData["CodeTypes"] = new MultiSelectList(_extensionCodeService.GetAllCodeTypes(), "Id", "Name");
            return View(result);
        }

        [HttpPost]
        public ActionResult ReviewOtherCodeMethod(OtherCode otherCode)
        {
            try
            {
                otherCode.ReviewFlag = true;
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
            ViewBag.DateSortParm = sortOrder == "date_desc" ? "Date" : "date_desc";
            ViewBag.FrequentSearchedCountSortParm = sortOrder == "count_desc" ? "count" : "count_desc";

            // List<ExtensionCodeViewModel> myList = new List<ExtensionCodeViewModel>();
            var extensioncodeList = _extensionCodeService.GetAllApprovedExtensionSolution().ToList();
            if (extensioncodeList.Count != 0)
            {
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
            else
            {
                ViewBag.CodeData = "Extension Code";
                return View("Nolistpage");
            }
        }

        public ActionResult ShowMyExtensionCode(string sortOrder, int? page)
        {
            var userEmailAddress = User.Identity.Name;
            User user = new User();
            user = _commonDataBaseContext.Query<User>().Where(u => u.EmailAddress == userEmailAddress).FirstOrDefault();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = sortOrder == "date_desc" ? "Date" : "date_desc";
            ViewBag.FrequentSearchedCountSortParm = sortOrder == "count_desc" ? "count" : "count_desc";

            // List<ExtensionCodeViewModel> myList = new List<ExtensionCodeViewModel>();
            var extensioncodeList = _extensionCodeService.GetMyAllExtensionSolution(user.ID).ToList();
            if (extensioncodeList.Count != 0)
            {
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
            else
            {
                ViewBag.CodeData = "Extension Code";
                return View("Nolistpage");
            }
        }

        public ActionResult ReviewExtensionCodeList(string sortOrder, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = sortOrder == "date_desc" ? "Date" : "date_desc";
            ViewBag.FrequentSearchedCountSortParm = sortOrder == "count_desc" ? "count" : "count_desc";

            var userEmailAddress = User.Identity.Name;
            User user = new User();
            user = _commonDataBaseContext.Query<User>().Where(u => u.EmailAddress == userEmailAddress).FirstOrDefault();

            var extensioncodeList = _extensionCodeService.GetAllReviewExtensionSolution(user.ID).ToList();
            if (extensioncodeList.Count != 0)
            {
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
            else
            {
                ViewBag.CodeData = "Extension Code";
                return View("Nolistpage");
            }
        }
        public PartialViewResult FrequentSearchedExtension(int? page)
        {
            string ShowExtensionType = (TempData["ShowExtensionType"]).ToString();
            var userEmailAddress = User.Identity.Name;
            User user = new User();
            user = _commonDataBaseContext.Query<User>().Where(u => u.EmailAddress == userEmailAddress).FirstOrDefault();
            List<ExtensionSolution> ExtensionList = new List<ExtensionSolution>();
            if (ShowExtensionType == "ShowMyExtensionCodeList")
            {
                ExtensionList = _extensionCodeService.GetMyAllExtensionSolution(user.ID).ToList();
            }
            else if (ShowExtensionType == "ShowAllExtensionCodeList")
            {
                ExtensionList = _extensionCodeService.GetAllReviewExtensionSolution(user.ID).ToList();
            }
            else
            {
                ExtensionList = _extensionCodeService.GetAllApprovedExtensionSolution().ToList();
            }

            var list = ExtensionList.OrderByDescending(x => x.FrequentSearchedCount).ToPagedList(page ?? 1, 3);
            return PartialView("_ExtensionCodeList", list);
        }


        public PartialViewResult newestExtension(int? page)
        {
            string ShowExtensionType = (TempData["ShowExtensionType"]).ToString();
            var userEmailAddress = User.Identity.Name;
            User user = new User();
            user = _commonDataBaseContext.Query<User>().Where(u => u.EmailAddress == userEmailAddress).FirstOrDefault();
            List<ExtensionSolution> ExtensionList = new List<ExtensionSolution>();
            if (ShowExtensionType == "ShowMyExtensionCodeList")
            {
                ExtensionList = _extensionCodeService.GetMyAllExtensionSolution(user.ID).ToList();
            }
            else if (ShowExtensionType == "ShowAllExtensionCodeList")
            {
                ExtensionList = _extensionCodeService.GetAllReviewExtensionSolution(user.ID).ToList();
            }
            else
            {
                ExtensionList = _extensionCodeService.GetAllApprovedExtensionSolution().ToList();
            }
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

        public ActionResult SearchExtensionMethodByTitle(string searchString, int? page)
        {
            searchString = searchString.ToLower();
            var userEmailAddress = User.Identity.Name;
            User user = new User();
            user = _commonDataBaseContext.Query<User>().Where(u => u.EmailAddress == userEmailAddress).FirstOrDefault();
            var ExtensionMethodList = _extensionCodeService.GetAllApprovedExtensionSolution().ToList();
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

        public ActionResult ShowOtherCode(string sortOrder, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = sortOrder == "date_desc" ? "Date" : "date_desc";
            ViewBag.FrequentSearchedCountSortParm = sortOrder == "count_desc" ? "count" : "count_desc";

            var othercodeList = _extensionCodeService.GetAllApprovedOtherCodeList().ToList();
            if (othercodeList.Count() != 0)
            {
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
            else
            {
                ViewBag.CodeData = "Other Code";
                return View("Nolistpage");
            }
        }

        public ActionResult ShowMyOtherCode(string sortOrder, int? page)
        {
            var userEmailAddress = User.Identity.Name;
            User user = new User();
            user = _commonDataBaseContext.Query<User>().Where(u => u.EmailAddress == userEmailAddress).FirstOrDefault();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = sortOrder == "date_desc" ? "Date" : "date_desc";
            ViewBag.FrequentSearchedCountSortParm = sortOrder == "count_desc" ? "count" : "count_desc";

            var othercodeList = _extensionCodeService.GetMyAllOtherCodeList(user.ID).ToList();
            if (othercodeList.Count() != 0)
            {
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
            else
            {
                ViewBag.CodeData = "Other Code";
                return View("Nolistpage");
            }
        }

        public ActionResult ReviewOtherCodeList(string sortOrder, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = sortOrder == "date_desc" ? "Date" : "date_desc";
            ViewBag.FrequentSearchedCountSortParm = sortOrder == "count_desc" ? "count" : "count_desc";

            var userEmailAddress = User.Identity.Name;
            User user = new User();
            user = _commonDataBaseContext.Query<User>().Where(u => u.EmailAddress == userEmailAddress).FirstOrDefault();

            var othercodeList = _extensionCodeService.GetAllReviewOtherCodeList(user.ID).ToList();
            if (othercodeList.Count() != 0)
            {
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
            else
            {
                ViewBag.CodeData = "Other Code";
                return View("Nolistpage");
            }
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
            string ShowExtensionType = (TempData["ShowExtensionType"]).ToString();
            var userEmailAddress = User.Identity.Name;
            User user = new User();
            user = _commonDataBaseContext.Query<User>().Where(u => u.EmailAddress == userEmailAddress).FirstOrDefault();
            List<OtherCode> OtherCodeList = new List<OtherCode>();
            if (ShowExtensionType == "GetMyAllOtherCodeList")
            {
                OtherCodeList = _extensionCodeService.GetMyAllOtherCodeList(user.ID).ToList();
            }
            else if (ShowExtensionType == "GetAllOtherCodeList")
            {
                OtherCodeList = _extensionCodeService.GetAllApprovedOtherCodeList().ToList();
            }
            else
            {
                OtherCodeList = _extensionCodeService.GetAllReviewOtherCodeList(user.ID).ToList();
            }
            var list = OtherCodeList.OrderByDescending(x => x.FrequentSearchedCount).ToPagedList(page ?? 1, 3);
            return PartialView("_OtherCodeList", list);
        }

        public PartialViewResult OtherCodenewest(int? page)
        {
            string ShowExtensionType = (TempData["ShowExtensionType"]).ToString();
            var userEmailAddress = User.Identity.Name;
            User user = new User();
            user = _commonDataBaseContext.Query<User>().Where(u => u.EmailAddress == userEmailAddress).FirstOrDefault();
            List<OtherCode> OtherCodeList = new List<OtherCode>();
            if (ShowExtensionType == "GetMyAllOtherCodeList")
            {
                OtherCodeList = _extensionCodeService.GetMyAllOtherCodeList(user.ID).ToList();
            }
            else if (ShowExtensionType == "GetAllOtherCodeList")
            {
                OtherCodeList = _extensionCodeService.GetAllApprovedOtherCodeList().ToList();
            }
            else
            {
                OtherCodeList = _extensionCodeService.GetAllReviewOtherCodeList(user.ID).ToList();
            }
            var list = OtherCodeList.OrderByDescending(x => x.CreationDate).ToPagedList(page ?? 1, 3);
            return PartialView("_OtherCodeList", list);
        }

        public ActionResult SearchOtherCodeMethodByTitle(string searchString, int? page)
        {
            searchString = searchString.ToLower();
            var CodeMethodList = _extensionCodeService.GetAllApprovedOtherCodeList().ToList();
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