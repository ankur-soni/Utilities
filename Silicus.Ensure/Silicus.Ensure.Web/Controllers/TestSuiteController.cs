using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity.Owin;
using Silicus.Ensure.Entities.Identity;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Services.Interfaces;
using Silicus.Ensure.Web.Mappings;
using Silicus.Ensure.Web.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Silicus.Ensure.Web.Controllers
{
    public class TestSuiteController : Controller
    {
        public ActionResult Add(Int32 tagId = 0)
        {
            if (tagId == 0)
                return View("../Admin/TestSuiteAdd");
            else
                return View("../Admin/TestSuiteAdd");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Save(TestSuite testSuite)
        {
            //if (tagId == 0)
            //    return View("../Admin/TestSuiteAdd");
            //else
                return View("../Admin/TestSuiteAdd");
        }
    }
}