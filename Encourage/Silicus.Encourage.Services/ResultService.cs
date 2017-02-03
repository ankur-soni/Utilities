using Silicus.Encourage.DAL.Interfaces;
using Silicus.Encourage.Models;
using Silicus.Encourage.Services.Interface;
using Silicus.UtilityContainer.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Encourage.Services
{
    public class ResultService : IResultService
    {
        private readonly IEncourageDatabaseContext _encourageDatabaseContext;
        private readonly Silicus.UtilityContainer.Entities.ICommonDataBaseContext _commonDataBaseContext;
        private readonly ICustomDateService _customDateService;
        public ResultService(Silicus.Encourage.DAL.Interfaces.IDataContextFactory dataContextFactory, ICommonDbService commonDbService, ICustomDateService customDateService)
        {
            _commonDataBaseContext = commonDbService.GetCommonDataBaseContext();
            _encourageDatabaseContext = dataContextFactory.CreateEncourageDbContext();
            _customDateService = customDateService;
        }

        public void ShortlistNomination(int nominationId, int adminId)
        {
            var shortlistedNomination = new Shortlist()
            {
                IsWinner = false,
                NominationId = nominationId,
                HrAdminsFeedback = string.Empty,
                AdminId = adminId
            };
            _encourageDatabaseContext.Add<Shortlist>(shortlistedNomination);
        }

        public void UnShortlistNomination(int nominationId)
        {
            var shortlistedNomination = _encourageDatabaseContext.Query<Shortlist>().Where(model => model.NominationId == nominationId).SingleOrDefault();
            if (shortlistedNomination != null)
                _encourageDatabaseContext.Delete<Shortlist>(shortlistedNomination);
        }

        public void SelectWinner(int nominationId, string winningComment, string hrAdminsFeedback, int adminId)
        {
            var shortlistedNomination = _encourageDatabaseContext.Query<Shortlist>().Where(model => model.NominationId == nominationId).SingleOrDefault();
            var currentNomination = new Nomination();
            if (shortlistedNomination != null)
            {
                currentNomination = _encourageDatabaseContext.Query<Nomination>().Where(x => x.Id == shortlistedNomination.NominationId).FirstOrDefault();
            }

            var customDate = _customDateService.GetCustomDate(currentNomination.AwardId);

            if (shortlistedNomination != null)
            {
                shortlistedNomination.IsWinner = true;
                //shortlistedNomination.WinningDate = DateTime.Now.Date;
                shortlistedNomination.WinningDate = customDate.Date;
                shortlistedNomination.WinningComment = winningComment;
                shortlistedNomination.HrAdminsFeedback = hrAdminsFeedback;
                shortlistedNomination.AdminId = adminId;
                _encourageDatabaseContext.Update<Shortlist>(shortlistedNomination);
            }
            else
            {
                var winner = new Shortlist()
                {
                    IsWinner = true,
                    NominationId = nominationId,
                    //WinningDate = DateTime.Now.Date,
                    WinningDate = customDate.Date,
                    WinningComment = winningComment,
                    HrAdminsFeedback = hrAdminsFeedback,
                    AdminId = adminId
                };
                _encourageDatabaseContext.Add<Shortlist>(winner);
            }
        }

        public int IsShortlistedOrWinner(int nominationId)
        {
            var shortlisted = _encourageDatabaseContext.Query<Shortlist>().Where(model => model.NominationId == nominationId).SingleOrDefault();

            // if returns 1 then it is winner,and if returns 2 then it is shortlisted already but not winner

            if (shortlisted != null)
            {
                if (shortlisted.IsWinner == true)
                {
                    return 1;
                }
                return 2;
            }
            else
                return 0;
        }

        public string GetAwardComments(int WinnerId)
        {
            var shortlistedNomination = _encourageDatabaseContext.Query<Shortlist>().Where(model => model.NominationId == WinnerId).FirstOrDefault();
            if (shortlistedNomination != null)
            {
                var comments = shortlistedNomination.WinningComment;

                return comments;
            }

            return string.Empty;
        }

        public string GetHrAdminsFeedbackForEmployee(int loggedInAdminsId, int nominationId)
        {
            var data = _encourageDatabaseContext.Query<Shortlist>().Where(s => s.NominationId == nominationId && s.AdminId == loggedInAdminsId).FirstOrDefault();
            var hrAdminsComment = string.Empty;
            if (data != null)
            {
                hrAdminsComment = data.HrAdminsFeedback;
            }
            return hrAdminsComment;
        }

        public string GetLoggedInUserName(string emailId)
        {
            var data = _commonDataBaseContext.Query<User>().Where(u => u.EmailAddress == emailId).FirstOrDefault();
            if (data != null)
            {
                return data.FirstName + " " + data.LastName;
            }
            return string.Empty;
        }

    }
}
