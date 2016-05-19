﻿using Silicus.Encourage.Services;
using Silicus.Encourage.Services.Interface;
using Silicus.Encourage.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Silicus.Encourage.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IAwardService _awardService;
        private readonly ICommonDbService _commonDbService;
        public DashboardController(IAwardService awardService,ICommonDbService commonDbService)
        {
            _awardService = awardService;
        _commonDbService = commonDbService;
        }

        [CustomeAuthorize(AllowedRole="User,Manager,Admin,Reviewer")]
        public ActionResult Dashboard()
        {
                        
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