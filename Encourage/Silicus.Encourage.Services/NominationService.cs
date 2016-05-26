using Silicus.Encourage.DAL.Interfaces;
using Silicus.Encourage.Models;
using Silicus.Encourage.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Encourage.Services
{
    public class NominationService : INominationService
    {
        private readonly IDataContextFactory _dataContextFactory;
        private readonly IEncourageDatabaseContext _encourageDatabaseContext;
        public NominationService(IDataContextFactory dataContextFactory)
        {
            _dataContextFactory = dataContextFactory;
            _encourageDatabaseContext = _dataContextFactory.CreateEncourageDbContext();
        }

        public List<Nomination> GetAllNominations()
        {
            return _encourageDatabaseContext.Query<Nomination>("ManagerComments").ToList();
        }

        public List<Nomination> GetAllSubmitedNominations()
        {
            return _encourageDatabaseContext.Query<Nomination>().Where(N => N.IsSubmitted == true).ToList();
        }

        public List<Nomination> GetAllSavedNominations()
        {
            return _encourageDatabaseContext.Query<Nomination>("ManagerComments").Where(nomination => nomination.IsSubmitted == false).ToList();
        }

        public Nomination GetNomination(int nominationId)
        {
           return _encourageDatabaseContext.Query<Nomination>("ManagerComments").Where(nomination => nomination.Id == nominationId).SingleOrDefault();
        }

        public void UpdateNomination(Nomination model)
        {

           _encourageDatabaseContext.Update<Nomination>(model);
        }

        public void DeletePrevoiusManagerComments(int nominationID)
        {
            var managerCommentsToDelete = _encourageDatabaseContext.Query<ManagerComment>().Where(comment => comment.NominationId == nominationID).ToList();

            foreach (var managerComments in managerCommentsToDelete)
                _encourageDatabaseContext.Delete<ManagerComment>(managerComments);
        }

        public void DiscardNomination(int nominationId)
        {
            DeletePrevoiusManagerComments(nominationId);
            var nominationToDelete=_encourageDatabaseContext.Query<Nomination>().Where(nomination => nomination.Id == nominationId).SingleOrDefault();
            _encourageDatabaseContext.Delete<Nomination>(nominationToDelete);
        }

    }
}
