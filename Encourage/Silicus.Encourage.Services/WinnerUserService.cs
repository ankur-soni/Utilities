using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Silicus.Encourage.DAL.Interfaces;
using Silicus.Encourage.Models;
using Silicus.Encourage.Services.Interface;
using Silicus.Encourage.Services.Models;

namespace Silicus.Encourage.Services
{
    public class WinnerUserService : IWinnerUserService
    {
        private readonly IEncourageDatabaseContext _context;
        private readonly INominationService _nominationService;
        private readonly IReviewService _reviewService;
        public WinnerUserService(IDataContextFactory context, INominationService nominationService, IReviewService reviewService)
        {
            _context = context.CreateEncourageDbContext();
            _nominationService = nominationService;
            _reviewService = reviewService;
        }
        public List<UserWinningHistory> GetMyWinningHistory(int userId, int awardId = 0)
        {
            var winners = GetAllWinners();
            var allWinnerNominations = GetAllWinnernominations(winners, awardId);
            var currentUsersWinnerNominations = GetCurrentUsersWinnerNominations(allWinnerNominations, userId);
            var userWinningHistoryList = new List<UserWinningHistory>();

            if (currentUsersWinnerNominations.Count > 0)
            {
                currentUsersWinnerNominations.ForEach(x =>
                {
                    if (x.NominationDate != null)
                    {
                        var firstOrDefault = winners.FirstOrDefault(y => y.NominationId == x.Id);
                        if (firstOrDefault != null)
                            userWinningHistoryList.Add(new UserWinningHistory()
                            {
                                AwardId = x.AwardId,
                                AwardMonth = x.NominationDate.Value.Month.ToString(),
                                AwardYear = x.NominationDate.Value.Year.ToString(),
                                AverageScore = GetAverageScore(x.Id),
                                AdminComment = firstOrDefault.HrAdminsFeedback,
                                ManagerComment = x.ProjectID != null ? x.Comment : x.OtherNominationReason,
                                NominationId = x.Id
                            });
                    }
                });
            }
            return userWinningHistoryList;
        }

        public List<Shortlist> GetAllWinners()
        {
            return _context.Query<Shortlist>().Where(x => x.IsWinner == true ).ToList();
        }

        public decimal GetAverageScore(int nominationId)
        {
            var reviews = _reviewService.GetReviewsForNomination(nominationId);
            var nomination = _nominationService.GetNomination(nominationId);
            var totalCreditPoints = 0.0m;
            foreach (var r in reviews)
            {
                foreach (var rc in r.ReviewerComments)
                {
                    var managerCommnet = nomination.ManagerComments.FirstOrDefault(mc => mc.CriteriaId == rc.CriteriaId);
                    totalCreditPoints += (Convert.ToInt32(rc.Credit) * (managerCommnet != null ? managerCommnet.Weightage : 0) / 100m);
                }
            }
            return totalCreditPoints;
        }

        public List<Nomination> GetAllWinnernominations(List<Shortlist> winnerList, int awardId = 0)
        {
            var allWinnerNominations = new List<Nomination>();
            if (awardId > 0)
            {
                winnerList.ToList().ForEach(x =>
                {
                    var winnerNomination =
                        _context.Query<Nomination>().FirstOrDefault(y => y.Id == x.NominationId && y.AwardId == awardId);
                    if (winnerNomination != null)
                    {
                    allWinnerNominations.Add(winnerNomination);
                    }
                });
            }
            else
            {
                winnerList.ToList().ForEach(x =>
                {
                    var winnerNomination = _context.Query<Nomination>().FirstOrDefault(y => y.Id == x.NominationId);
                    if (winnerNomination != null)
                    {
                        allWinnerNominations.Add(winnerNomination);
                    }
                });
            }
            return allWinnerNominations;
        }

        public List<Nomination> GetCurrentUsersWinnerNominations(List<Nomination> allWinnerNominations, int userId)
        {
            var currentUsersWinnerNominations = new List<Nomination>();
            allWinnerNominations.ForEach(x =>
            {
                if (x.UserId == userId)
                {
                    currentUsersWinnerNominations.Add(x);
                }
            });
            return currentUsersWinnerNominations;
        }
    }
}
