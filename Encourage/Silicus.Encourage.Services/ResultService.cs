﻿using Silicus.Encourage.DAL.Interfaces;
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
            var shortlistedNomination = _encourageDatabaseContext.Query<Shortlist>().SingleOrDefault(model => model.NominationId == nominationId);
            if (shortlistedNomination != null)
            {
                _encourageDatabaseContext.Delete<Shortlist>(shortlistedNomination);
            }
        }

        public void SelectWinner(int nominationId, string winningComment, string hrAdminsFeedback, int adminId)
        {
            var shortlistedNomination = _encourageDatabaseContext.Query<Shortlist>().SingleOrDefault(model => model.NominationId == nominationId);
            var currentNomination = shortlistedNomination != null ? _encourageDatabaseContext.Query<Nomination>().FirstOrDefault(x => x.Id == shortlistedNomination.NominationId) : _encourageDatabaseContext
                .Query<Nomination>().FirstOrDefault( x => x.Id == nominationId);
            if (currentNomination != null)
            {
                var customDate = _customDateService.GetCustomDate(currentNomination.AwardId);

                if (shortlistedNomination != null)
                {
                    shortlistedNomination.IsWinner = true;
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
            {
                return 0;
            }
        }

        public string GetAwardComments(int winnerId)
        {
            var comments = string.Empty;
            var firstOrDefault = _encourageDatabaseContext.Query<Shortlist>().FirstOrDefault(model => model.NominationId == winnerId);
            if (firstOrDefault != null)
            {
                comments = firstOrDefault.WinningComment;
            }
            return comments;
        }

        public string GetHrAdminsFeedbackForEmployee(int loggedInAdminsId, int nominationId)
        {
            var data = _encourageDatabaseContext.Query<Shortlist>().FirstOrDefault(s => s.NominationId == nominationId && s.AdminId == loggedInAdminsId);
            var hrAdminsComment = string.Empty;
            if (data != null)
            {
                hrAdminsComment = data.HrAdminsFeedback;
            }
            return hrAdminsComment;
        }

        public string GetLoggedInUserName(string emailId)
        {
            var data = _commonDataBaseContext.Query<User>().FirstOrDefault(u => u.EmailAddress == emailId);
            if (data != null)
            {
                return data.FirstName + " " + data.LastName;
            }
            return null;
        }
    }
}
