using Silicus.Encourage.DAL.Interfaces;
using Silicus.Encourage.Models;
using Silicus.Encourage.Services.Comparer;
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

        public List<Nomination> GetAllSubmitedNonreviewedNominations()
        {
            var alreadyReviewedRecords = _encourageDatabaseContext.Query<Review>().ToList();
            var finalNomination = new List<Nomination>();
            var allNominations = _encourageDatabaseContext.Query<Nomination>().Where(N => N.IsSubmitted == true).ToList();

            foreach (var item in alreadyReviewedRecords)
            {
                finalNomination.Add(_encourageDatabaseContext.Query<Nomination>().Where(N => N.IsSubmitted == true && N.Id == item.NominationId).FirstOrDefault());
                
            }
            foreach (var item in finalNomination)
	        {
                allNominations.RemoveAll(r => r.Id == item.Id);
	        }


            return allNominations;
        }

        public List<Nomination> GetAllSubmitedReviewedNominations()
        {
            var alreadyReviewedRecords = _encourageDatabaseContext.Query<Review>().ToList();
            var finalNominations = new List<Nomination>();
           

            foreach (var item in alreadyReviewedRecords)
            {
                var nomination = _encourageDatabaseContext.Query<Nomination>().Where(N => N.IsSubmitted == true && N.Id == item.NominationId).FirstOrDefault();
                nomination.IsSubmitted = item.IsSubmited;
                finalNominations.Add(nomination);

            }



            return finalNominations;
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
            var nominationToDelete = _encourageDatabaseContext.Query<Nomination>().Where(nomination => nomination.Id == nominationId).SingleOrDefault();
            _encourageDatabaseContext.Delete<Nomination>(nominationToDelete);
        }

    }
}
