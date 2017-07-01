using System.Collections.Generic;
using System.Linq;
using Silicus.Ensure.Entities;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Services.Interfaces;
using Silicus.Ensure.Models.Constants;
using System;
using Silicus.Ensure.Models.Test;
using Silicus.Ensure.Models;
using Silicus.Ensure.Models.JobVite;
using Newtonsoft.Json;

namespace Silicus.Ensure.Services
{
    public class UserService : IUserService
    {
        private readonly IDataContext _context;
        private IPanelService _panelService;
        //private IPositionService _positionService;

        public UserService(IDataContextFactory dataContextFactory, IPanelService panelService)
        {
            _context = dataContextFactory.Create(ConnectionType.Ip);
            _panelService = panelService;
            //_positionService = positionService;
        }

        public IEnumerable<UserBusinessModel> GetUserDetailsAll()
        {
            List<UserBusinessModel> userModel = new List<UserBusinessModel>();
            var users = _context.Query<User>();

            //var candidateList = (from candidate in users

            //                     select new UserBusinessModel()
            //                     {
            //                         FirstName = candidate.FirstName,
            //                         LastName = candidate.LastName,
            //                         Email = candidate.Email,
            //                         CandidateStatus = candidate.,
            //                         JobViteId = candidate.application.eId,
            //                         Position = candidate.application.job.title
            //                     }).ToList();

            return userModel;
        }

        //public IEnumerable<UserBusinessModel> GetUserDetails()
        //{

        //    return GetUserDetailsAll();
        //}

        public int Add(UserBusinessModel userModel)
        {
            var user = UserFromBusinessModel(userModel);
            _context.Add(user);
            return user.UserId;
        }

        //public void Update(UserBusinessModel userModel)
        //{
        //    if (userModel.FirstName != null && userModel.LastName != null)
        //    {
        //        var applicationdetail = UserFromBusinessModelForUpdate(userModel);
        //        _context.AttachAndMakeStateModified(applicationdetail);
        //        _context.AttachAndMakeStateModified(applicationdetail.User);
        //        _context.SaveChanges();

        //    }
        //}

        //public void UpdateUserAndCreateNewApplication(UserBusinessModel userModel)
        //{
        //    if (userModel.FirstName != null && userModel.LastName != null)
        //    {
        //        var userDetails = MapUserDetails(userModel);
        //      //  var userApplicationDetailsDetails = MapApplicationDetails(userModel);

        //       // _context.Add(userApplicationDetailsDetails);
        //       // _context.Update(userDetails);
        //        // _context.SaveChanges();

        //    }
        //}

        //public void Delete(int userId)
        //{
        //    var user = _context.Query<User>().FirstOrDefault(y => y.UserId == userId);
        //    if (user != null)
        //    {
        //        user.IsDeleted = true;
        //    }
        //    //var userDetails = GetLatesUserApplication(userId);
        //    if (userDetails != null)
        //    {
        //        userDetails.CandidateStatus = CandidateStatus.Archived;
        //    }
        //    _context.Update(user);
        //    _context.Update(userDetails);
        //}

        //public UserApplicationDetails GetUserApplicationDetailsById(int userApplicationId)
        //{
        //    return _context.Query<UserApplicationDetails>().FirstOrDefault(x => x.UserApplicationDetailsId == userApplicationId);
        //}


        //public int GetUserLastestApplicationId(int userId)
        //{
        //    return _context.Query<User>().FirstOrDefault(x => x.UserId == userId).UserApplicationDetails.LastOrDefault().UserApplicationDetailsId;


        //}

        //public void UpdateUserApplicationTestDetails(int UserApplicationDetailsId)
        //{
        //    var result = _context.Query<UserApplicationDetails>().FirstOrDefault(x => x.UserApplicationDetailsId == UserApplicationDetailsId);
        //    if (result != null)
        //    {
        //        result.CandidateStatus = CandidateStatus.TestSubmitted;
        //        _context.Update(result);
        //        _context.SaveChanges();
        //    }
        //}

        //public UserApplicationDetails GetLatesUserApplication(int userId)
        //{
        //    return _context.Query<UserApplicationDetails>().Where(x => x.UserId == userId).OrderByDescending(y => y.CreatedDate).FirstOrDefault();
        //}

        //public UserBusinessModel GetUserById(int userId)
        //{
        //    var user = _context.Query<User>().FirstOrDefault(x => x.UserId == userId);
        //   // return UserToBusinessModel(user);
        //}


        //public List<UserBusinessModel> GetUserWithAllApplicationDetails(int userId)
        //{
        //    var user = _context.Query<User>().FirstOrDefault(x => x.UserId == userId && x.IsDeleted == false);
        //    return UserToMutipleApplicationsBusinessModel(user);
        //}

        //public List<UserBusinessModel> GetUserDetails(int userId)
        //{
        //    var user = _context.Query<User>().FirstOrDefault(x => x.UserId == userId);
        //    return UserToMutipleApplicationsBusinessModel(user);
        //}

        //public UserBusinessModel GetUserByUserApplicationId(int UserApplicationDetailId)
        //{
        //    //var applicationDetail = _context.Query<UserApplicationDetails>().FirstOrDefault(y => y.UserApplicationDetailsId == UserApplicationDetailId);
        //    //if (applicationDetail != null)
        //    //{
        //    //    var user = _context.Query<User>().FirstOrDefault(x => x.UserId == applicationDetail.UserId && x.IsDeleted == false);
        //    //    return UserToBusinessModel(user);
        //    //}
        //    return null;
        //}

        //public IEnumerable<UserBusinessModel> GetCandidates(string firstName, String lastName, DateTime dob)
        //{
        //    var users = _context.Query<User>().Where(x => x.FirstName == firstName
        //    && x.LastName == lastName
        //    && x.DateOfBirth == dob.Date
        //    ).ToList();
        //    List<UserBusinessModel> userModel = new List<UserBusinessModel>();
        //    foreach (var user in users)
        //    {
        //        var objUser = UserToBusinessModel(user);
        //        userModel.Add(objUser);
        //    }
        //    return userModel;
        //}

        //public UserBusinessModel GetUserByEmail(string email)
        //{
        //    var user = _context.Query<User>().FirstOrDefault(x => x.Email == email);
        //    return UserToBusinessModel(user);
        //}

        public IEnumerable<User> GetUserByRole(string role)
        {
            //need to correct for role
            //return _context.Query<User>().Where(x => x.Role == role && x.IsDeleted == false);
            return _context.Query<User>().Where(x => x.IsDeleted == false);
        }

        public dynamic GetTestSuiteDetailsOfUser(int? UserApplicationId)
        {
            var result = (from ts in _context.Query<UserTestSuite>().Where(x => x.UserApplicationId == UserApplicationId)
                          join t in _context.Query<TestSuite>() on ts.TestSuiteId equals t.TestSuiteId
                          select new
                          {
                              t.TestSuiteId,
                              t.TestSuiteName,
                              ts.UserTestSuiteId,
                              ts.ObjectiveCount,
                              ts.PracticalCount,
                              ts.MaxScore,
                              ts.Duration
                          }).FirstOrDefault();
            return result;
        }

        public IEnumerable<UserTestSuite> GetAllTestSuiteDetails()
        {
            return _context.Query<UserTestSuite>();
        }

        public dynamic GetTestSuiteDetailsWithQuestions(int? userTestSuiteId)
        {
            var result = (from td in _context.Query<UserTestDetails>().Where(x => x.UserTestSuite.TestSuiteId == userTestSuiteId)
                          join q in _context.Query<Question>() on td.QuestionId equals q.Id
                          select new
                          {
                              q.Id,
                              q.QuestionDescription,
                              q.QuestionType,
                              q.OptionCount,
                              q.Option1,
                              q.Option2,
                              q.Option3,
                              q.Option4,
                              q.Option5,
                              q.Option6,
                              q.Option7,
                              q.Option8,
                              q.Marks,
                              q.CorrectAnswer,
                              q.Answer
                          }).ToList();
            return result;
        }
        //public CandidateInfoBusinessModel GetCandidateInfo(UserBusinessModel user)
        //{
        //    return new CandidateInfoBusinessModel
        //    {
        //        Name = user.FirstName + " " + user.LastName,
        //        DOB = user.DOB,
        //        RequisitionId = user.RequisitionId,
        //        Position = user.Position,
        //        TotalExperience = ConvertExperienceIntoDecimal(user.TotalExperienceInYear, user.TotalExperienceInMonth),
        //        TotalExperienceInMonth = user.TotalExperienceInMonth,
        //        TotalExperienceInYear = user.TotalExperienceInYear
        //    };
        //}


        //private List<UserBusinessModel> UserToMutipleApplicationsBusinessModel(User user)
        //{
        //    List<UserBusinessModel> userApplicationList = new List<UserBusinessModel>();
        //    if (user != null)
        //    {
        //        if (user.UserApplicationDetails != null)
        //        {
        //            var applicationDetailsList = user.UserApplicationDetails.OrderByDescending(y => y.CreatedDate);
        //            foreach (var applicationDetails in applicationDetailsList)
        //            {
        //                if (applicationDetails != null)
        //                {
        //                    var objUser = new UserBusinessModel();
        //                    var position = applicationDetails.Position;
        //                    objUser.Position = position == null ? "" : position.PositionName;
        //                    objUser.CandidateStatus = applicationDetails.CandidateStatus.ToString();
        //                    objUser.ClientName = applicationDetails.ClientName;
        //                    objUser.CurrentCompany = applicationDetails.CurrentCompany;
        //                    objUser.CurrentTitle = applicationDetails.CurrentTitle;
        //                    objUser.RelevantExperienceInMonth = applicationDetails.RelevantExperienceInMonth;
        //                    objUser.RelevantExperienceInYear = applicationDetails.RelevantExperienceInYear;
        //                    objUser.RequisitionId = applicationDetails.RequisitionId;
        //                    objUser.ResumeName = applicationDetails.ResumeName;
        //                    objUser.ResumePath = applicationDetails.ResumePath;
        //                    objUser.Technology = applicationDetails.Technology;
        //                    objUser.TestStatus = applicationDetails.CandidateStatus.ToString();
        //                    objUser.TotalExperienceInMonth = applicationDetails.TotalExperienceInMonth;
        //                    objUser.TotalExperienceInYear = applicationDetails.TotalExperienceInYear;
        //                    objUser.UserApplicationId = applicationDetails.UserApplicationDetailsId;
        //                    if (applicationDetails.PanelMemberId != null && applicationDetails.PanelMemberId > 0)
        //                        objUser.PanelId = applicationDetails.PanelMemberId.ToString();

        //                    if (applicationDetails.RecruiterMemberId != null && applicationDetails.RecruiterMemberId > 0)
        //                        objUser.RecruiterId = applicationDetails.RecruiterMemberId.ToString();
        //                    objUser.ApplicationDate = applicationDetails.CreatedDate;

        //                    objUser.DOB = user.DateOfBirth;
        //                    objUser.Email = user.Email;
        //                    objUser.FirstName = user.FirstName;
        //                    objUser.Gender = user.Gender;
        //                    objUser.IdentityUserId = user.IdentityUserId;
        //                    objUser.LastName = user.LastName;
        //                    objUser.MiddleName = user.MiddleName;
        //                    objUser.IsDeleted = user.IsDeleted;
        //                    objUser.ProfilePhotoFilePath = user.ProfilePhotoFilePath;
        //                    objUser.ContactNumber = user.ContactNumber;
        //                    objUser.CurrentLocation = user.CurrentLocation;
        //                    objUser.Role = RoleName.Candidate.ToString();
        //                    objUser.UserId = user.UserId;

        //                    userApplicationList.Add(objUser);
        //                }
        //            }
        //        }
        //    }


        //    return userApplicationList;
        //}


        #region Private methods
        //private UserBusinessModel UserToBusinessModel(User user)
        //{
        //    var objUser = new UserBusinessModel();
        //    if (user != null)
        //    {
        //        objUser.DOB = user.DateOfBirth;
        //        objUser.Email = user.Email;
        //        objUser.FirstName = user.FirstName;
        //        objUser.Gender = user.Gender;
        //        objUser.IdentityUserId = user.IdentityUserId;
        //        objUser.LastName = user.LastName;
        //        objUser.MiddleName = user.MiddleName;
        //        objUser.IsDeleted = user.IsDeleted;
        //        objUser.ProfilePhotoFilePath = user.ProfilePhotoFilePath;
        //        objUser.ContactNumber = user.ContactNumber;
        //        objUser.CurrentLocation = user.CurrentLocation;
        //        objUser.Role = RoleName.Candidate.ToString();
        //        objUser.UserId = user.UserId;
        //        objUser.CreatedDate = user.CreatedDate;
        //        //if (user.UserApplicationDetails != null)
        //        {
        //            //if (user.UserApplicationDetails != null && user.UserApplicationDetails.Count > 1)
        //            //{
        //            //    objUser.HasHistory = true;
        //            //}
        //           // var applicationDetails = user.UserApplicationDetails.OrderByDescending(y => y.CreatedDate).FirstOrDefault();
        //            //if (applicationDetails != null)
        //            {
        //                var position = applicationDetails.Position;
        //                objUser.Position = position == null ? "" : position.PositionName;
        //                objUser.CandidateStatus = applicationDetails.CandidateStatus.ToString();
        //                objUser.ClientName = applicationDetails.ClientName;
        //                objUser.CurrentCompany = applicationDetails.CurrentCompany;
        //                objUser.CurrentTitle = applicationDetails.CurrentTitle;
        //                objUser.RelevantExperienceInMonth = applicationDetails.RelevantExperienceInMonth;
        //                objUser.RelevantExperienceInYear = applicationDetails.RelevantExperienceInYear;
        //                objUser.RequisitionId = applicationDetails.RequisitionId;
        //                objUser.ResumeName = applicationDetails.ResumeName;
        //                objUser.ResumePath = applicationDetails.ResumePath;
        //                objUser.Technology = applicationDetails.Technology;
        //                objUser.TestStatus = applicationDetails.CandidateStatus.ToString();
        //                objUser.TotalExperienceInMonth = applicationDetails.TotalExperienceInMonth;
        //                objUser.TotalExperienceInYear = applicationDetails.TotalExperienceInYear;
        //                objUser.UserApplicationId = applicationDetails.UserApplicationDetailsId;
        //                if (applicationDetails.PanelMemberId != null && applicationDetails.PanelMemberId > 0)
        //                    objUser.PanelId = applicationDetails.PanelMemberId.ToString();

        //                if (applicationDetails.RecruiterMemberId != null && applicationDetails.RecruiterMemberId > 0)
        //                    objUser.RecruiterId = applicationDetails.RecruiterMemberId.ToString();

        //            }
        //        }

        //    }
        //    return objUser;
        //}

        private User UserFromBusinessModel(UserBusinessModel objUser)
        {
            User user;
            if (objUser.UserId > 0)
            {
                user = _context.Query<User>().FirstOrDefault(y => y.UserId == objUser.UserId);
            }
            else
            {
                user = new User();
            }
           // user.DateOfBirth = objUser.DOB;
            user.Email = objUser.Email;
            user.FirstName = objUser.FirstName;
            user.Gender = objUser.Gender;
            user.IdentityUserId = objUser.IdentityUserId;
            user.LastName = objUser.LastName;
            user.MiddleName = objUser.MiddleName;
            user.UserId = objUser.UserId;
            //user.CurrentLocation = objUser.CurrentLocation;
           // user.ContactNumber = objUser.ContactNumber;
            //user.ProfilePhotoFilePath = objUser.ProfilePhotoFilePath;
            //UserApplicationDetails applicationDetails;
            //if (user.UserApplicationDetails != null && user.UserApplicationDetails.Count > 0 && !objUser.IsCandidateReappear)
            //{
            //    applicationDetails = user.UserApplicationDetails.Where(x => x.UserId == user.UserId)
            //        .OrderByDescending(y => y.CreatedDate).FirstOrDefault();
            //    applicationDetails.CandidateStatus = string.IsNullOrWhiteSpace(objUser.CandidateStatus) ? applicationDetails.CandidateStatus : (CandidateStatus)Enum.Parse(typeof(CandidateStatus), objUser.CandidateStatus);
            //}
            //else
            //{
            //    applicationDetails = new UserApplicationDetails();
            //    user.CreatedDate = DateTime.Now;
            //    applicationDetails.CandidateStatus = CandidateStatus.New;
            //}

            //int panelId = 0;
            //if (int.TryParse(objUser.PanelId, out panelId) && panelId > 0)
            //{
            //    applicationDetails.PanelMemberId = panelId;
            //}

            //int RecruiterMemberId;
            //if (int.TryParse(objUser.RecruiterId, out RecruiterMemberId) && RecruiterMemberId > 0)
            //{
            //    applicationDetails.RecruiterMemberId = RecruiterMemberId;
            //}
            //var position = _positionService.GetPositionByName(objUser.Position);
            //if (position != null)
            //applicationDetails.PositionId = 1;


            //applicationDetails.ClientName = objUser.ClientName;

            //applicationDetails.CurrentCompany = objUser.CurrentCompany;
            //applicationDetails.CurrentTitle = objUser.CurrentTitle;
            //applicationDetails.RelevantExperienceInMonth = objUser.RelevantExperienceInMonth;
            //applicationDetails.RelevantExperienceInYear = objUser.RelevantExperienceInYear;
            //applicationDetails.RequisitionId = objUser.RequisitionId;
            //applicationDetails.ResumeName = objUser.ResumeName;
            //applicationDetails.ResumePath = objUser.ResumePath;
            //applicationDetails.Technology = objUser.Technology;
            //applicationDetails.TotalExperienceInMonth = objUser.TotalExperienceInMonth;
            //applicationDetails.TotalExperienceInYear = objUser.TotalExperienceInYear;
            //applicationDetails.CreatedDate = DateTime.Now;

            //if (user.UserApplicationDetails == null)
            //{
            //    user.UserApplicationDetails = new List<UserApplicationDetails>();
            //}
            //user.UserApplicationDetails.Add(applicationDetails);
            return user;
        }

        //private UserApplicationDetails UserFromBusinessModelForUpdate(UserBusinessModel objUser)
        //{

        //    UserApplicationDetails applicationDetails;
        //    if (objUser.UserId > 0)
        //    {
        //        applicationDetails = GetLatesUserApplication(objUser.UserId);
        //    }
        //    else
        //    {
        //        applicationDetails = new UserApplicationDetails();
        //    }
        //    if (applicationDetails.User == null)
        //    {
        //        applicationDetails.User = new User();
        //    }
        //    applicationDetails.User.DateOfBirth = objUser.DOB;
        //    applicationDetails.User.Email = objUser.Email;
        //    applicationDetails.User.FirstName = objUser.FirstName;
        //    applicationDetails.User.Gender = objUser.Gender;
        //    applicationDetails.User.IdentityUserId = objUser.IdentityUserId;
        //    applicationDetails.User.LastName = objUser.LastName;
        //    applicationDetails.User.MiddleName = objUser.MiddleName;
        //    applicationDetails.User.UserId = objUser.UserId;
        //    applicationDetails.User.CurrentLocation = objUser.CurrentLocation;
        //    applicationDetails.User.ContactNumber = objUser.ContactNumber;
        //    applicationDetails.User.ProfilePhotoFilePath = objUser.ProfilePhotoFilePath;

        //    int panelId;
        //    if (int.TryParse(objUser.PanelId, out panelId) && panelId > 0)
        //    {
        //        applicationDetails.PanelMemberId = panelId;
        //    }
        //    int RecruiterMemberId;
        //    if (int.TryParse(objUser.RecruiterId, out RecruiterMemberId) && RecruiterMemberId > 0)
        //    {
        //        applicationDetails.RecruiterMemberId = RecruiterMemberId;
        //    }


        //    var position = _positionService.GetPositionByName(objUser.Position);
        //    if (position != null)
        //        applicationDetails.PositionId = position.PositionId;

        //    applicationDetails.CandidateStatus = string.IsNullOrWhiteSpace(objUser.CandidateStatus) ? applicationDetails.CandidateStatus : (CandidateStatus)Enum.Parse(typeof(CandidateStatus), objUser.CandidateStatus);
        //    applicationDetails.ClientName = objUser.ClientName;

        //    applicationDetails.CurrentCompany = objUser.CurrentCompany;
        //    applicationDetails.CurrentTitle = objUser.CurrentTitle;
        //    applicationDetails.RelevantExperienceInMonth = objUser.RelevantExperienceInMonth;
        //    applicationDetails.RelevantExperienceInYear = objUser.RelevantExperienceInYear;
        //    applicationDetails.RequisitionId = objUser.RequisitionId;
        //    applicationDetails.ResumeName = objUser.ResumeName;
        //    applicationDetails.ResumePath = objUser.ResumePath;
        //    applicationDetails.Technology = objUser.Technology;
        //    applicationDetails.TotalExperienceInMonth = objUser.TotalExperienceInMonth;
        //    applicationDetails.TotalExperienceInYear = objUser.TotalExperienceInYear;
        //    applicationDetails.CreatedDate = DateTime.Now;
        //    applicationDetails.User.CreatedDate = DateTime.Now;
        //    return applicationDetails;
        //}

        //private User MapUserDetails(UserBusinessModel objUser)
        //{
        //    User user;
        //    if (objUser.UserId > 0)
        //    {
        //        user = _context.Query<User>().FirstOrDefault(y => y.UserId == objUser.UserId);
        //    }
        //    else
        //    {
        //        user = new User();
        //    }
        //    user.DateOfBirth = objUser.DOB;
        //    user.Email = objUser.Email;
        //    user.FirstName = objUser.FirstName;
        //    user.Gender = objUser.Gender;
        //    user.IdentityUserId = objUser.IdentityUserId;
        //    user.LastName = objUser.LastName;
        //    user.MiddleName = objUser.MiddleName;
        //    user.UserId = objUser.UserId;
        //    user.CurrentLocation = objUser.CurrentLocation;
        //    user.ContactNumber = objUser.ContactNumber;
        //   // user.ProfilePhotoFilePath = objUser.ProfilePhotoFilePath;

        //    return user;
        //}

        //private UserApplicationDetails MapApplicationDetails(UserBusinessModel objUser)
        //{
        //    UserApplicationDetails applicationDetails;
        //    if (objUser.UserId > 0 && !objUser.IsCandidateReappear)
        //    {
        //        applicationDetails = GetLatesUserApplication(objUser.UserId);
        //        applicationDetails.CandidateStatus = string.IsNullOrWhiteSpace(objUser.CandidateStatus) ? applicationDetails.CandidateStatus : (CandidateStatus)Enum.Parse(typeof(CandidateStatus), objUser.CandidateStatus);
        //    }
        //    else
        //    {
        //        applicationDetails = new UserApplicationDetails();
        //        applicationDetails.CandidateStatus = CandidateStatus.New;
        //    }
        //    applicationDetails.UserId = objUser.UserId;

        //    int panelId;
        //    if (int.TryParse(objUser.PanelId, out panelId) && panelId > 0)
        //    {
        //        applicationDetails.PanelMemberId = panelId;
        //    }
        //    var position = _positionService.GetPositionByName(objUser.Position);
        //    if (position != null)
        //        applicationDetails.PositionId = position.PositionId;


        //    applicationDetails.ClientName = objUser.ClientName;

        //    applicationDetails.CurrentCompany = objUser.CurrentCompany;
        //    applicationDetails.CurrentTitle = objUser.CurrentTitle;
        //    applicationDetails.RelevantExperienceInMonth = objUser.RelevantExperienceInMonth;
        //    applicationDetails.RelevantExperienceInYear = objUser.RelevantExperienceInYear;
        //    applicationDetails.RequisitionId = objUser.RequisitionId;
        //    applicationDetails.ResumeName = objUser.ResumeName;
        //    applicationDetails.ResumePath = objUser.ResumePath;
        //    applicationDetails.Technology = objUser.Technology;
        //    applicationDetails.TotalExperienceInMonth = objUser.TotalExperienceInMonth;
        //    applicationDetails.TotalExperienceInYear = objUser.TotalExperienceInYear;
        //    applicationDetails.CreatedDate = DateTime.Now;
        //    return applicationDetails;
        //}

        private decimal ConvertExperienceIntoDecimal(int totalExperienceInYear, int totalExperienceInMonth)
        {
            if (totalExperienceInMonth > 0)
            {
                return totalExperienceInYear + (decimal)(totalExperienceInMonth / 12.0);
            }
            else
                return totalExperienceInYear;
        }
        #endregion

        #region JobVite
//        public List<JobViteCandidateBusinessModel> GetCandidatesFromJobVite()
//        {
//            #region JsonString 
//            // BuildMyString.com generated code. Please enjoy your string responsibly.

//            string candidatesString = "[" +
//"{" +
//"\"address\": \"100 Thistle St\"," +
//"\"address2\": \"\"," +
//"\"application\": {" +
//"\"comments\": null," +
//"\"customField\": []," +
//"\"disposition\": null," +
//"\"eId\": \"pXbHWiwy\"," +
//"\"gender\": \"Undefined\"," +
//"\"hireDate\": null," +
//"\"job\": {" +
//"\"company\": \"Chester Group\"," +
//"\"customField\": [" +
//"{" +
//"\"key\": \"Coding Test\"," +
//"\"value\": \"Test A\"" +
//"}" +
//"]," +
//"\"department\": \"\"," +
//"\"eId\": \"oLMz1fwR\"," +
//"\"hiringManagers\": [" +
//"{" +
//"\"employeeId\": null," +
//"\"firstName\": \"Christie\"," +
//"\"lastName\": \"Cheung-Chester\"," +
//"\"userId\": \"syTmqgwY\"," +
//"\"userName\": \"chestergroupceo@gmail.com\"" +
//"}" +
//"]," +
//"\"location\": \"San Francisco\"," +
//"\"recruiters\": [" +
//"{" +
//"\"employeeId\": null," +
//"\"firstName\": \"Christie\"," +
//"\"lastName\": \"Cheung-Chester\"," +
//"\"userId\": \"syTmqgwY\"," +
//"\"userName\": \"chestergroupceo@gmail.com\"" +
//"}" +
//"]," +
//"\"requisitionId\": \"001\"," +
//"\"subsidiaryId\": \"fnV9Vfw6\"," +
//"\"title\": \"Dog Focused Web Developer\"" +
//"}," +
//"\"lastUpdatedDate\": 1443682800000," +
//"\"race\": \"Decline to Self Identify\"," +
//"\"resume\": {" +
//"\"content\": \"\"," +
//"\"format\": \"Text\"" +
//"}," +
//"\"source\": \"Christie Cute-Chester\"," +
//"\"sourceType\": \"Recruiter\"," +
//"\"startDate\": null," +
//"\"status\": null," +
//"\"veteranStatus\": \"Undefined\"," +
//"\"workflowState\": \"Background Check - Talentwise\"" +
//"}," +
//"\"city\": \"San Francisco\"," +
//"\"companyName\": \"\"," +
//"\"country\": \"USA\"," +
//"\"eId\": \"e2JcehwX\"," +
//"\"email\": \"chestergroupceo@gmail.com\"," +
//"\"firstName\": \"Charlie\"," +
//"\"homePhone\": \"650-555-1000\"," +
//"\"lastName\": \"Candidate\"," +
//"\"location\": \"San Francisco, California United States\"," +
//"\"postalCode\": \"94121\"," +
//"\"state\": \"California\"," +
//"\"title\": \"\"," +
//"\"workPhone\": \"650-444-2000\"," +
//"\"workStatus\": \"None\"" +
//"}," +
//"{" +
//"\"address\": \"100 Thistle St\"," +
//"\"address2\": \"\"," +
//"\"application\": {" +
//"\"comments\": null," +
//"\"customField\": []," +
//"\"disposition\": null," +
//"\"eId\": \"po7g3iwB\"," +
//"\"gender\": \"Undefined\"," +
//"\"hireDate\": null," +
//"\"job\": {" +
//"\"company\": \"Chester Group\"," +
//"\"customField\": [" +
//"{" +
//"\"key\": \"Assessment Type\"," +
//"\"value\": \"Test A\"" +
//"}" +
//"]," +
//"\"department\": \"Pet Sitting & Dog Walking\"," +
//"\"eId\": \"oITr1fwN\"," +
//"\"hiringManagers\": [" +
//"{" +
//"\"employeeId\": null," +
//"\"firstName\": \"Christie\"," +
//"\"lastName\": \"Cute-Chester\"," +
//"\"userId\": \"syTmqgwY\"," +
//"\"userName\": \"chestergroupceo@gmail.com\"" +
//"}" +
//"]," +
//"\"location\": \"San Mateo\"," +
//"\"recruiters\": [" +
//"{" +
//"\"employeeId\": null," +
//"\"firstName\": \"Christie\"," +
//"\"lastName\": \"Cute-Chester\"," +
//"\"userId\": \"syTmqgwY\"," +
//"\"userName\": \"chestergroupceo@gmail.com\"" +
//"}" +
//"]," +
//"\"requisitionId\": \"\"," +
//"\"subsidiaryId\": \"fnV9Vfw6\"," +
//"\"title\": \"Dog Walker\"" +
//"}," +
//"\"lastUpdatedDate\": 1443682800000," +
//"\"race\": \"Decline to Self Identify\"," +
//"\"resume\": null," +
//"\"source\": \"Christie Cute-Chester\"," +
//"\"sourceType\": \"Recruiter\"," +
//"\"startDate\": null," +
//"\"status\": null," +
//"\"veteranStatus\": \"Undefined\"," +
//"\"workflowState\": \"Background Check - Talentgreat\"" +
//"}," +
//"\"city\": \"San Francisco\"," +
//"\"companyName\": \"\"," +
//"\"country\": \"USA\"," +
//"\"eId\": \"e2JcehwX\"," +
//"\"email\": \"chestergroupceo@gmail.com\"," +
//"\"firstName\": \"Charlie\"," +
//"\"homePhone\": \"650-555-1000\"," +
//"\"lastName\": \"Candidate\"," +
//"\"location\": \"San Francisco, California United States\"," +
//"\"postalCode\": \"94121\"," +
//"\"state\": \"California\"," +
//"\"title\": \"\"," +
//"\"workPhone\": \"650-444-2000\"," +
//"\"workStatus\": \"None\"" +
//"}" +
//"]";



//            #endregion


//            var deserializedCandidates = JsonConvert.DeserializeObject<List<JobViteCandidateBusinessModel>>(candidatesString);
//            var cand2 = deserializedCandidates.FirstOrDefault();
//            cand2.FirstName = "ABC";
//            cand2.LastName = "XYZ";
//            deserializedCandidates.Add(cand2);
//            foreach (var candidate in deserializedCandidates)
//            {
//                var forDemo = DateTime.Now.Millisecond.ToString();
//                candidate.FirstName = forDemo + candidate.FirstName;
//                candidate.LastName = forDemo + candidate.LastName;
//                candidate.Email = forDemo + candidate.Email;
//            }
//            return deserializedCandidates;
//        }

        public List<RequisitionBusinessModel> GetAllRequistions()
        {
            // BuildMyString.com generated code. Please enjoy your string responsibly.

            var requisitionJson = "[" +
            "{" +
            "\"applyLink\": null," +
            "\"approveDate\": null," +
            "\"bonus\": \"\"," +
            "\"briefDescription\": \"Dog Talent Agent - identify cute dogs\"," +
            "\"category\": \"Customer Service\"," +
            "\"closeDate\": 253402237799000," +
            "\"company\": null," +
            "\"companyId\": \"qGbaVfwG\"," +
            "\"customField\": null," +
            "\"department\": \"Walking\"," +
            "\"description\": \"Keen eye to spot cute and well behaved dogs for modelling opportunities\"," +
            "\"distribution\": true," +
            "\"eId\": \"oaGp2fw1\"," +
            "\"eeoCategory\": null," +
            "\"emailLanguage\": null," +
            "\"endDate\": null," +
            "\"evaluationFormName\": null," +
            "\"filledOn\": null," +
            "\"hiringManagers\": [" +
            "{" +
            "\"email\": \"haleyhiringmanager@gmail.com\"," +
            "\"employeeId\": null," +
            "\"firstName\": \"Haley\"," +
            "\"lastName\": \"Hiringmanager\"," +
            "\"userId\": \"s4g5wgwG\"," +
            "\"userName\": null" +
            "}" +
            "]," +
            "\"internalOnly\": true," +
            "\"jobLink\": null," +
            "\"jobSource\": \"Manual\"," +
            "\"jobState\": \"Open\"," +
            "\"jobType\": \"Full-Time\"," +
            "\"lastUpdatedDate\": 1452676228403," +
            "\"location\": \"San Mateo\"," +
            "\"locationCity\": \"San Mateo\"," +
            "\"locationCountry\": \"United States\"," +
            "\"locationPostalCode\": \"94403\"," +
            "\"locationState\": \"California\"," +
            "\"postingType\": \"Limited Access\"," +
            "\"preInterviewFormName\": null," +
            "\"primaryHiringManagerEmail\": \"haleyhiringmanager@gmail.com\"," +
            "\"primaryRecruiterEmail\": null," +
            "\"private\": true," +
            "\"putOnHoldDate\": null," +
            "\"recruiters\": null," +
            "\"referralBonus\": null," +
            "\"region\": \"San Francisco Bay Area\"," +
            "\"requisitionId\": \"00015 \"," +
            "\"sentDate\": 1452676228403," +
            "\"startDate\": null," +
            "\"subsidiaryId\": null," +
            "\"subsidiaryName\": null," +
            "\"title\": \"Dog Talent Agent - 3\"," +
            "\"workflow\": \"General\"" +
            "}" +
            "]";


            var deserializedRequistions = JsonConvert.DeserializeObject<List<RequisitionBusinessModel>>(requisitionJson);
            var reqsn2 = deserializedRequistions.FirstOrDefault();
            reqsn2.RequisitionId = "00016";
            reqsn2.Title = "Demo Requisition 2";
            deserializedRequistions.Add(reqsn2);
            return deserializedRequistions;
        }
        #endregion
    }
}

