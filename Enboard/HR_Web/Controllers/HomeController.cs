using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using System.Web.Security;

using Service;
using Data;

namespace HR_Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IUserService _IUserService;
        LoginDetails _Logindetails;
        public HomeController(IUserService IUserService)
        {
            this._IUserService = IUserService;
          
            _Logindetails = new LoginDetails();
           
        }

        public ActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginDetails model)
        {
            if (ModelState.IsValid)
            {
                var user = _IUserService.GetAll(null, null, "").ToList();


                var res = user.Where(u => u.Password == model.Password).SingleOrDefault(); //&& u.UserName == model.UserName
                if (res != null)
                {
                    FormsAuthentication.SetAuthCookie(res.FirstName, false);
                    return RedirectToAction("Test","Home");
                }

                ModelState.AddModelError("", "Invalid Login");
                return View("Login");
            }
            else
            {
                return View("Login");
            }

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
               
        [HttpGet]
        public ActionResult PersonalDetails()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }

        /// <summary>
        /// Code change - Added Welcome controller action for welcome page
        /// </summary>
        /// <returns></returns>
        //public JsonResult Welcome()
        public ActionResult Welcome()
        {
            var content = _IUserService.GetLatestWelcomeNote();
            ViewBag.HtmlStr = content;
            return View();
        }

        
        /// <summary>
        /// Code change - Added Welcome controller action for editing welcome page
        /// </summary>
        /// <returns></returns>
        public ActionResult WelcomeEdit(string success)
        {
            var userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
            WelcomeModel model = new WelcomeModel();
            var content = _IUserService.GetWelcomeNote(userId);
            model.WelcomeNote = content;

            if (success == "success")
            { ViewBag.EditWelcomeSuccess = "Welcome message updated successfully."; }
            else { ViewBag.EditWelcomeSuccess = ""; }
            
            return View(model);
        }

        /// <summary>
        /// Code change - Added Welcome controller action for saving welcome page
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveWelcomeText(WelcomeModel model)
        {
            var userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
            var userName = System.Web.HttpContext.Current.User.Identity.Name.Split('|')[0];
            
            model.UserID = userId;
            model.UpdatedBy = userName;
            model.CreatedBy = userName;

            var result = _IUserService.UpdateWelcomeMessage(model);

            return RedirectToAction("WelcomeEdit", new { success = "success" });
        }
    }
}