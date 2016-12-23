using AutoMapper;
using Silicus.FrameworxProject.Models;
using Silicus.FrameworxProject.Services.Interfaces;
using Silicus.Reusable.Web.Models;
using Silicus.Reusable.Web.Models.ViewModel;
using System;
using System.Web.Mvc;

namespace Silicus.Reusable.Web.Controllers
{
    [Authorize]
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
            frameworxFeedbackViewModel.UserEmail = User.Identity.Name;
            frameworxFeedbackViewModel.UserName = _commonDbService.FindDisplayNameFromEmail(frameworxFeedbackViewModel.UserEmail);
            frameworxFeedbackViewModel.FrameworxId = frameworxViewModel.id;
            frameworxFeedbackViewModel.FeedBackFor = string.IsNullOrWhiteSpace(frameworxViewModel.Title) ? Constants.InformationNotAvailableText : frameworxViewModel.Title;
            return PartialView("_FeedbackForm", frameworxFeedbackViewModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveFeedback(FrameworxFeedbackViewModel frameworxFeedbackViewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = _commonDbService.FindUserIdFromEmail(User.Identity.Name);
                if (userId == null)
                {
                    throw new Exception("Error while submiiting feedback");
                }
                frameworxFeedbackViewModel.UserId = userId.Value;
                var newFeedbackDetailsModel = _mapper.Map<FrameworxFeedbackViewModel, FrameworxFeedback>(frameworxFeedbackViewModel);
                //Call service's SaveFeedbackDetails method to save feedback details 
                _frameworxFeedbackService.SaveFeedbackDetails(newFeedbackDetailsModel);
                return Json(true);
            }

            return Json(false);
        }

        public ActionResult OpenContactOwnerForm(int ownerId)
        {
            Silicus.UtilityContainer.Models.DataObjects.User user = _commonDbService.GetUser(ownerId);
            FrameworxOwnerViewModel frameworxOwnerViewModel = new FrameworxOwnerViewModel();
            frameworxOwnerViewModel.Name = user != null ? string.IsNullOrEmpty(user.DisplayName) ? Constants.InformationNotAvailableText : user.DisplayName : Constants.InformationNotAvailableText;
            frameworxOwnerViewModel.Email = user != null ? string.IsNullOrEmpty(user.EmailAddress) ? Constants.InformationNotAvailableText : user.EmailAddress : Constants.InformationNotAvailableText;
            frameworxOwnerViewModel.OfficePhone = user != null ? string.IsNullOrEmpty(user.OfficePhone) ? Constants.InformationNotAvailableText : user.OfficePhone : Constants.InformationNotAvailableText;
            return PartialView("_ContactOwner", frameworxOwnerViewModel);
        }

        #endregion
    }
}