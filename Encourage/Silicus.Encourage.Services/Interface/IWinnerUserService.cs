using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Silicus.Encourage.Models;
using Silicus.Encourage.Services.Models;

namespace Silicus.Encourage.Services.Interface
{
    public interface IWinnerUserService
    {
        List<UserWinningHistory> GetMyWinningHistory(int userId,int awardId = 0);
        List<Shortlist> GetAllWinners();
        List<Nomination> GetAllWinnernominations(List<Shortlist> winnerList, int awardId = 0);
        decimal GetAverageScore(int nominationId);
        List<Nomination> GetCurrentUsersWinnerNominations(List<Nomination> allWinnerNominations, int userId);

    }
}
