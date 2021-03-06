﻿using Silicus.FrameworxProject.Models;
using Silicus.FrameworxProject.Services.Interfaces;
using Silicus.Reusable.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;

namespace Silicus.FrameworxDashboard.Web.Controllers
{
    [Authorize]
    public class FrameworxProjectController : Controller
    {
        private readonly IFrameworxProjectService _frameworxProjectService;
        private readonly ICommonDbService _commonDbService;

        public FrameworxProjectController(IFrameworxProjectService frameworxProjectService, ICommonDbService commonDbService)
        {
            _frameworxProjectService = frameworxProjectService;
            _commonDbService = commonDbService;
            //textInfo = new CultureInfo("en-US", false).TextInfo;
        }
        
        public ActionResult Dashboard()
        {
            List<Frameworx> frameworkList = _frameworxProjectService.GetAllFrameworx();
            var CategoryList = _frameworxProjectService.GetAllCategories();

            var allComponents = frameworkList.Join(CategoryList,
                wo => wo.CategoryId,
                p => p.Id,
                (order, plan) => new { order.Id, order.Title, order.SourceCodeLink, order.DemoLink, order.HtmlDescription, plan.Name }
                ).Select(m => new FrameworxViewModel
                {
                    id = m.Id,
                    Name = m.Name,
                    Title = m.Title,
                    HtmlDescription = m.HtmlDescription,
                    DemoLink = m.DemoLink,
                    SourceCodeLink = m.SourceCodeLink

                }).ToList();
            return View(allComponents);
        }

        [HttpGet]
        public ActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCategory(FrameworxCategory category)
        {
            try
            {
                _frameworxProjectService.AddCategory(category);
                return RedirectToAction("AddComponent");
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
            Frameworx framework = _frameworxProjectService.FrameworkDetail(id);
            var userId = _commonDbService.FindUserIdFromEmail(User.Identity.Name);
            FrameworxViewModel frameworxViewModel = new FrameworxViewModel()
            {
                Name = framework.Category.Name,
                Title = framework.Title,
                HtmlDescription = framework.HtmlDescription,
                SourceCodeLink = framework.SourceCodeLink,
                Likes = framework.Likes.Count,
                IsLiked = framework.Likes.Any(l => l.UserId == userId),
                OwnerId = framework.OwnerId,
                Credits = string.Join(",", framework.Credits.Select(c => c.Name).ToList())
            };

            frameworxViewModel.Credits = string.IsNullOrWhiteSpace(frameworxViewModel.Credits) ? string.Empty : frameworxViewModel.Credits + ".";

            if (frameworxViewModel.IsLiked)
            {
                frameworxViewModel.LikeId = framework.Likes.FirstOrDefault(l => l.UserId == userId).Id;
            }

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
            if (query.ToList().Count() != 0)
            {
                return View("SearchComponent", query.ToList());
            }
            else
            {
                ViewBag.ErrorMessage = "Oops! There is no such Component";
                return View("SearchComponentMessage", query.ToList());
            }
        }

        public ActionResult LikeComponent(int componentId)
        {
            var userId = _commonDbService.FindUserIdFromEmail(User.Identity.Name);
            int likeId = _frameworxProjectService.AddFrameworxLike(new FrameworxLike()
            {
                FrameworxId = componentId,
                UserId = userId.Value

            });
            return Json(new { likeId = likeId }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UnLikeComponent(int likeId)
        {
            _frameworxProjectService.RemoveFrameworxLike(new FrameworxLike()
            {
                Id = likeId

            });
            return Json(true, JsonRequestBehavior.AllowGet);
        }


    }
}