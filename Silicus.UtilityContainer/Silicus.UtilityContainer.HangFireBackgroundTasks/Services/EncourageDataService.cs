using Silicus.Encourage.DAL;
using Silicus.Encourage.DAL.Interfaces;
using Silicus.Encourage.Models;
using Silicus.UtilityContainer.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.UtilityContainer.HangFireBackgroundTasks.Services
{
    public class EncourageDataService
    {
        public IDataContextFactory contextFactory ;
        public IEncourageDatabaseContext context ;
        public EncourageDataService()
        {
            contextFactory = new DataContextFactory();
            context = contextFactory.CreateEncourageDbContext();
         
        } 

        public List<WinnerData> GetWinnerData()
        {
            
            var allWinners = context.Query<Shortlist>().Where(shortlist => shortlist.IsWinner == true && shortlist.WinningDate.Value.Month==DateTime.Now.Month && shortlist.WinningDate.Value.Year == DateTime.Now.Year).ToList();
            var winnersList = new List<WinnerData>();

            using (Silicus.UtilityContainer.Entities.ICommonDataBaseContext _commonDbContext = new Silicus.UtilityContainer.Entities.DataContextFactory().CreateCommonDBContext())
            { 
                foreach (var winner in allWinners)
                {
                    var nominationOfWinnner = context.Query<Nomination>().Where(nomination => nomination.Id == winner.NominationId).FirstOrDefault();

                    var userName = _commonDbContext.Query<User>().Where(user => user.ID == nominationOfWinnner.UserId).FirstOrDefault().DisplayName;
                    var awardName = context.Query<Award>().Where(award => award.Id == nominationOfWinnner.AwardId).FirstOrDefault().Name;
                    var managerName = _commonDbContext.Query<User>().Where(user => user.ID == nominationOfWinnner.ManagerId).FirstOrDefault().DisplayName;
                    var projectName = _commonDbContext.Query<Engagement>().Where(enagegement => enagegement.ID == nominationOfWinnner.ProjectID).FirstOrDefault().Name;

                    var awardPeriod = nominationOfWinnner.NominationDate.Value.ToString("MMMM")+" - " + nominationOfWinnner.NominationDate.Value.Year.ToString();
                    var winnerData = new WinnerData()
                    {
                        Name = userName,
                        AwardName = awardName,
                        AwardPeriod = awardPeriod,
                        ManagerName = managerName,
                        ProjectName = projectName
                    };

                    winnersList.Add(winnerData);
                }
            }

            return winnersList;
        }
    }
}
