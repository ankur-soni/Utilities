using Silicus.Encourage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Encourage.Services.Interface
{
    public interface INominationService
    {
        List<Nomination> GetAllNominations();
        List<Nomination> GetAllSubmitedNonreviewedNominations(int reviewerId);
        Nomination GetReviewNomination(int nominationId);
        List<ManagerComment> GetManagerCommentsForNomination(int nominationId);
        List<Criteria> GetCriteriaForNomination(int nominationId);
        int GetReviewerIdOfCurrentNomination(string email);
        string GetNomineeNameOfCurrentNomination(int nominationId);
        string GetManagerNameOfCurrentNomination(int nominationId);
        string GetProjectNameOfCurrentNomination(int nominationId);
        string GetDeptNameOfCurrentNomination(int nominationId);
        void AddReviewForCurrentNomination(Review review);
        void AddReviewerCommentsForCurrentNomination(ReviewerComment revrComment);
        string GetAwardMonthAndYear(int nominationId);
        string GetAwardName(int nominationId);

        List<Nomination> GetAllSubmitedReviewedNominations(int reviewerId);
        List<Nomination> GetAllSavedNominations();
        Nomination GetNomination(int nominationId);
        void UpdateNomination(Nomination model);
        void DeletePrevoiusManagerComments(int nominationID);
        void DiscardNomination(int nominationId);
    }
}
