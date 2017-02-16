using System.Collections.Generic;
using System.Linq;
using Silicus.Ensure.Entities;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Services.Interfaces;
using Silicus.Ensure.Models.Constants;
using System;
using Silicus.Ensure.Models.Test;
using Silicus.Ensure.Models;

namespace Silicus.Ensure.Services
{
    public class UserService : IUserService
    {
        private readonly IDataContext _context;
        private IPanelService _panelService;
        private IPositionService _positionService;

        public UserService(IDataContextFactory dataContextFactory, IPanelService panelService, IPositionService positionService)
        {
            _context = dataContextFactory.Create(ConnectionType.Ip);
            _panelService = panelService;
            _positionService = positionService;
        }

        public IEnumerable<UserBusinessModel> GetUserDetailsAll()
        {
            List<UserBusinessModel> userModel = new List<UserBusinessModel>();
            var users = _context.Query<User>().ToList();
            var panelMeberDetails = _context.Query<PanelMemberDetail>();
            foreach (var user in users)
            {
                var objUser = UserToBusinessModel(user);
                int panelId = 0;
                if (int.TryParse(objUser.PanelId, out panelId))
                {
                    var panelDetails = panelMeberDetails.FirstOrDefault(y => y.UserId == panelId);
                    if (panelDetails != null)
                        objUser.PanelName = panelDetails.LastName + " " + panelDetails.FirstName;
                }
                userModel.Add(objUser);
            }
            return userModel;
        }



        public IEnumerable<UserBusinessModel> GetUserDetails()
        {

            return GetUserDetailsAll().Where(x => x.IsDeleted == false);
        }



        public int Add(UserBusinessModel userModel)
        {
            var user = UserFromBusinessModel(userModel);
            _context.Add(user);
            return user.UserId;
        }

        public void Update(UserBusinessModel userModel)
        {
            if (userModel.FirstName != null && userModel.LastName != null)
            {
                var applicationdetail = UserFromBusinessModelForUpdate(userModel);
                // var user = UserFromBusinessModel(userModel);
                // //_context.Entry(user.UserApplicationDetails.or).State = System.Data.EntityState.Modified;

                // var rs = GetUserApplicationDetailsById(user.UserApplicationDetails.FirstOrDefault().UserApplicationDetailsId);
                // rs.RequisitionId = "zzz1";
                // rs.User.FirstName = "z121";

                // _context.AttachAndMakeStateModified(rs);
                // _context.AttachAndMakeStateModified(rs.User);
                //// _context.Update(rs);
                //  //_context.UpdateUser(rs);
                // //_context.Update(user);
                // _context.SaveChanges();
                _context.AttachAndMakeStateModified(applicationdetail);
                _context.AttachAndMakeStateModified(applicationdetail.User);
                _context.SaveChanges();

            }
            //if (User.Role != null && User.Role.ToLower() == RoleName.Candidate.ToString().ToLower())
            //{
            //    if (User.FirstName != null && User.LastName != null)
            //    {
            //        _context.Update(User);
            //    }
            //}
            //else if (User.Role != null && (User.Role.ToLower() == RoleName.Admin.ToString().ToLower() || User.Role.ToLower() == RoleName.Panel.ToString().ToLower()))
            //{
            //    _context.Update(User);
            //}
        }

        public void Delete(int userId)
        {
            var user = _context.Query<User>().FirstOrDefault(y => y.UserId == userId);
            if (user != null)
            {
                user.IsDeleted = true;
                _context.Update(user);
            }
        }

        public UserApplicationDetails GetUserApplicationDetailsById(int userApplicationId)
        {
            return _context.Query<UserApplicationDetails>().FirstOrDefault(x => x.UserApplicationDetailsId == userApplicationId);
        }

        public void UpdateUserApplicationTestDetails(int UserApplicationDetailsId)
        {
            var result = _context.Query<UserApplicationDetails>().FirstOrDefault(x => x.UserApplicationDetailsId == UserApplicationDetailsId);
            if (result != null)
            {
                result.CandidateStatus = CandidateStatus.TestSubmitted;
                _context.Update(result);
                _context.SaveChanges();
            }
        }

        public UserApplicationDetails GetLatesUserApplication(int userId)
        {
            return _context.Query<UserApplicationDetails>().Where(x => x.UserId == userId).OrderByDescending(y => y.CreatedDate).FirstOrDefault();
        }

        public UserBusinessModel GetUserById(int userId)
        {
            var user = _context.Query<User>().FirstOrDefault(x => x.UserId == userId && x.IsDeleted == false);
            return UserToBusinessModel(user);
        }

        public UserBusinessModel GetUserByUserApplicationId(int UserApplicationDetailId)
        {
            var applicationDetail = _context.Query<UserApplicationDetails>().FirstOrDefault(y => y.UserApplicationDetailsId == UserApplicationDetailId);
            if (applicationDetail != null)
            {
                var user = _context.Query<User>().FirstOrDefault(x => x.UserId == applicationDetail.UserId && x.IsDeleted == false);
                return UserToBusinessModel(user);
            }
            return null;
        }

        public IEnumerable<UserBusinessModel> GetCandidates(string firstName, String lastName, DateTime dob)
        {
            var users = _context.Query<User>().Where(x => string.Equals(x.FirstName,firstName, StringComparison.OrdinalIgnoreCase)
            && string.Equals(x.LastName, lastName)
            && x.DateOfBirth.Date==dob.Date);
            List<UserBusinessModel> userModel = new List<UserBusinessModel>();
            foreach (var user in users)
            {
                var objUser = UserToBusinessModel(user);
                //int panelId = 0;
                //if (int.TryParse(objUser.PanelId, out panelId))
                //{
                //    var panelDetails = panelMeberDetails.FirstOrDefault(y => y.UserId == panelId);
                //    if (panelDetails != null)
                //        objUser.PanelName = panelDetails.LastName + " " + panelDetails.FirstName;
                //}
                userModel.Add(objUser);
            }
            return userModel;
        }

        public UserBusinessModel GetUserByEmail(string email)
        {
            var user = _context.Query<User>().FirstOrDefault(x => x.Email == email && x.IsDeleted == false);
            return UserToBusinessModel(user);
        }

        public IEnumerable<User> GetUserByRole(string role)
        {
            //need to correct for role
            //return _context.Query<User>().Where(x => x.Role == role && x.IsDeleted == false);
            return _context.Query<User>().Where(x => x.IsDeleted == false);
        }

        public dynamic GetTestSuiteDetailsOfUser(int? userId)
        {
            var result = (from ts in _context.Query<UserTestSuite>().Where(x => x.UserApplicationId == userId)
                          join t in _context.Query<TestSuite>() on ts.TestSuiteId equals t.TestSuiteId
                          select new
                          {
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
        public CandidateInfoBusinessModel GetCandidateInfo(UserBusinessModel user)
        {
            return new CandidateInfoBusinessModel
            {
                Name = user.FirstName + " " + user.LastName,
                DOB = user.DOB,
                RequisitionId = user.RequisitionId,
                Position = user.Position,
                TotalExperience = ConvertExperienceIntoDecimal(user.TotalExperienceInYear, user.TotalExperienceInMonth)
            };
        }


        #region Private methods
        private UserBusinessModel UserToBusinessModel(User user)
        {
            var objUser = new UserBusinessModel();
            if (user.UserApplicationDetails != null)
            {
                var applicationDetails = user.UserApplicationDetails.OrderByDescending(y => y.CreatedDate).FirstOrDefault();
                if (applicationDetails != null)
                {
                    var position = applicationDetails.Position;
                    objUser.Position = position == null ? "" : position.PositionName;
                    objUser.CandidateStatus = applicationDetails.CandidateStatus.ToString();
                    objUser.ClientName = applicationDetails.ClientName;
                    objUser.ContactNumber = user.ContactNumber;
                    objUser.CurrentCompany = applicationDetails.CurrentCompany;
                    objUser.CurrentLocation = user.CurrentLocation;
                    objUser.CurrentTitle = applicationDetails.CurrentTitle;
                    objUser.RelevantExperienceInMonth = applicationDetails.RelevantExperienceInMonth;
                    objUser.RelevantExperienceInYear = applicationDetails.RelevantExperienceInYear;
                    objUser.RequisitionId = applicationDetails.RequisitionId;
                    objUser.ResumeName = applicationDetails.ResumeName;
                    objUser.ResumePath = applicationDetails.ResumeName;
                    objUser.Technology = applicationDetails.Technology;
                    objUser.TestStatus = applicationDetails.CandidateStatus.ToString();
                    objUser.TotalExperienceInMonth = applicationDetails.TotalExperienceInMonth;
                    objUser.TotalExperienceInYear = applicationDetails.TotalExperienceInYear;
                    objUser.UserApplicationId = applicationDetails.UserApplicationDetailsId;

                    if (applicationDetails.PanelMemberId != null && applicationDetails.PanelMemberId > 0)
                        objUser.PanelId = applicationDetails.PanelMemberId.ToString();

                }
            }


            objUser.DOB = user.DateOfBirth;
            objUser.Email = user.Email;
            objUser.FirstName = user.FirstName;
            objUser.Gender = user.Gender;
            objUser.IdentityUserId = user.IdentityUserId;
            objUser.LastName = user.LastName;
            objUser.MiddleName = user.MiddleName;
            objUser.IsDeleted = user.IsDeleted;

            objUser.Role = RoleName.Candidate.ToString();

            objUser.UserId = user.UserId;

            return objUser;
        }

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
            user.DateOfBirth = objUser.DOB;
            user.Email = objUser.Email;
            user.FirstName = objUser.FirstName;
            user.Gender = objUser.Gender;
            user.IdentityUserId = objUser.IdentityUserId;
            user.LastName = objUser.LastName;
            user.MiddleName = objUser.MiddleName;
            user.UserId = objUser.UserId;
            user.CurrentLocation = objUser.CurrentLocation;
            user.ContactNumber = objUser.ContactNumber;

            UserApplicationDetails applicationDetails;
            if (user.UserApplicationDetails != null && user.UserApplicationDetails.Count > 0)
            {
                applicationDetails = user.UserApplicationDetails.Where(x => x.UserId == user.UserId)
                    .OrderByDescending(y => y.CreatedDate).FirstOrDefault();
            }
            else
            {
                applicationDetails = new UserApplicationDetails();
                user.CreatedDate = DateTime.Now;
            }

            int panelId = 0;
            if (int.TryParse(objUser.PanelId, out panelId) && panelId > 0)
            {
                applicationDetails.PanelMemberId = panelId;
            }

            var position = _positionService.GetPositionByName(objUser.Position);
            if (position != null)
                applicationDetails.PositionId = position.PositionId;

            applicationDetails.CandidateStatus = string.IsNullOrWhiteSpace(objUser.CandidateStatus) ? applicationDetails.CandidateStatus : (CandidateStatus)Enum.Parse(typeof(CandidateStatus), objUser.CandidateStatus);
            applicationDetails.ClientName = objUser.ClientName;

            applicationDetails.CurrentCompany = objUser.CurrentCompany;
            applicationDetails.CurrentTitle = objUser.CurrentTitle;
            applicationDetails.RelevantExperienceInMonth = objUser.RelevantExperienceInMonth;
            applicationDetails.RelevantExperienceInYear = objUser.RelevantExperienceInYear;
            applicationDetails.RequisitionId = objUser.RequisitionId;
            applicationDetails.ResumeName = objUser.ResumeName;
            applicationDetails.ResumePath = objUser.ResumePath;
            applicationDetails.Technology = objUser.Technology;
            applicationDetails.TotalExperienceInMonth = objUser.TotalExperienceInMonth;
            applicationDetails.TotalExperienceInYear = objUser.TotalExperienceInYear;
            applicationDetails.CreatedDate = DateTime.Now;

            if (user.UserApplicationDetails == null)
            {
                user.UserApplicationDetails = new List<UserApplicationDetails>();
            }
            user.UserApplicationDetails.Add(applicationDetails);
            return user;
        }

        private UserApplicationDetails UserFromBusinessModelForUpdate(UserBusinessModel objUser)
        {

            UserApplicationDetails applicationDetails;
            if (objUser.UserId > 0)
            {
                applicationDetails = GetLatesUserApplication(objUser.UserId);
                //user.UserApplicationDetails.Where(x => x.UserId == user.UserId).OrderByDescending(y => y.CreatedDate).FirstOrDefault();

            }
            else
            {
                applicationDetails = new UserApplicationDetails();
            }


            //User user;
            if (applicationDetails.User == null)
            {
                applicationDetails.User = new User();
            }
            applicationDetails.User.DateOfBirth = objUser.DOB;
            applicationDetails.User.Email = objUser.Email;
            applicationDetails.User.FirstName = objUser.FirstName;
            applicationDetails.User.Gender = objUser.Gender;
            applicationDetails.User.IdentityUserId = objUser.IdentityUserId;
            applicationDetails.User.LastName = objUser.LastName;
            applicationDetails.User.MiddleName = objUser.MiddleName;
            applicationDetails.User.UserId = objUser.UserId;
            applicationDetails.User.CurrentLocation = objUser.CurrentLocation;
            applicationDetails.User.ContactNumber = objUser.ContactNumber;


            int panelId = 0;
            if (int.TryParse(objUser.PanelId, out panelId) && panelId > 0)
            {
                applicationDetails.PanelMemberId = panelId;
            }


            var position = _positionService.GetPositionByName(objUser.Position);
            if (position != null)
                applicationDetails.PositionId = position.PositionId;

            applicationDetails.CandidateStatus = string.IsNullOrWhiteSpace(objUser.CandidateStatus) ? applicationDetails.CandidateStatus : (CandidateStatus)Enum.Parse(typeof(CandidateStatus), objUser.CandidateStatus);
            applicationDetails.ClientName = objUser.ClientName;

            applicationDetails.CurrentCompany = objUser.CurrentCompany;
            applicationDetails.CurrentTitle = objUser.CurrentTitle;
            applicationDetails.RelevantExperienceInMonth = objUser.RelevantExperienceInMonth;
            applicationDetails.RelevantExperienceInYear = objUser.RelevantExperienceInYear;
            applicationDetails.RequisitionId = objUser.RequisitionId;
            applicationDetails.ResumeName = objUser.ResumeName;
            applicationDetails.ResumePath = objUser.ResumePath;
            applicationDetails.Technology = objUser.Technology;
            applicationDetails.TotalExperienceInMonth = objUser.TotalExperienceInMonth;
            applicationDetails.TotalExperienceInYear = objUser.TotalExperienceInYear;
            applicationDetails.CreatedDate = DateTime.Now;

            //if (user.UserApplicationDetails == null)
            //{
            //    user.UserApplicationDetails = new List<UserApplicationDetails>();
            //}
            // user.UserApplicationDetails.Add(applicationDetails);

            applicationDetails.User.CreatedDate = DateTime.Now;
            // user.CreatedBy = DateTime.Now;
            return applicationDetails;
        }


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
    }
}

