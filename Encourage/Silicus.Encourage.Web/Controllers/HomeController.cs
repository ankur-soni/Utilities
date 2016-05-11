using Silicus.Encourage.Services;
using Silicus.Encourage.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Silicus.Encourage.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAwardService _awardService;
        public HomeController(IAwardService awardService)
        {
            _awardService = awardService;
        }

        public ActionResult Index()
        {

            var awards = _awardService.GetAllAwards();
            return View();
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
    }
}