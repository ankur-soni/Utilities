using AutoMapper;
using Silicus.FrameworxProject.Models;
using Silicus.FrameworxProject.Services.Interfaces;
using Silicus.Reusable.Web.Models;
using Silicus.Reusable.Web.Models.ViewModel;
using System;
using System.Web.Mvc;

namespace Silicus.Reusable.Web.Controllers
{
    public class FrameworxFeedbackController : Controller
    {
        #region Private Variables
        private readonly IFrameworxFeedbackService _frameworxFeedbackService;
        private readonly ICommonDbService _commonDbService;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor
        public FrameworxFeedbackController(IFrameworxFeedbackService frameworxFeedbackService, ICommonDbService commonDbService, IMapper mapper)
        {
            _frameworxFeedbackService = frameworxFeedbackService;
            _commonDbService = commonDbService;
            _mapper = mapper;
        }
        #endregion

        #region Public Methods

        public ActionResult OpenFeedbackForm(FrameworxViewModel frameworxViewModel)
        {
            FrameworxFeedbackViewModel frameworxFeedbackViewModel = new FrameworxFeedbackViewModel();
            Silicus.UtilityContainer.Models.DataObjects.User user = _commonDbService.GetUser(frameworxViewModel.OwnerId);
            if (user != null)
            {
                frameworxFeedbackViewModel.OwnerName = user.DisplayName;
                frameworxFeedbackViewModel.OwnerEmail = user.EmailAddress;
            }

            frameworxFeedbackViewModel.FrameworxId = frameworxViewModel.id;
            frameworxFeedbackViewModel.FeedBackFor = frameworxViewModel.Title;
            return PartialView("_FeedbackForm", frameworxFeedbackViewModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveFeedback(FrameworxFeedbackViewModel frameworxFeedbackViewModel)
        {
            if (ModelState.IsValid)
            {
                frameworxFeedbackViewModel.UserId = _commonDbService.FindUserIdFromEmail(User.Identity.Name);
                var newFeedbackDetailsModel = _mapper.Map<FrameworxFeedbackViewModel, FrameworxFeedback>(frameworxFeedbackViewModel);
                //Call service's SaveFeedbackDetails method to save feedback details 
                _frameworxFeedbackService.SaveFeedbackDetails(newFeedbackDetailsModel);
                return Json(true);
            }

            return Json(false);
        }

        #endregion
    }
}