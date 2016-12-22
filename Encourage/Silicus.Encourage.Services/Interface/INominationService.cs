using Silicus.Encourage.Models;
using Silicus.UtilityContainer.Models.DataObjects;
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
        List<Nomination> GetAllSubmittedAndSavedNominationsByCurrentUser(int managerID);
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
        List<Nomination> GetAllSubmitedReviewedNominations(int reviewerId, bool forCurrentMonth);
        List<Review> GetAllSubmitedReviewsForCurrentNomination(int nominationId);
        List<Nomination> GetAllSavedNominations();
        Nomination GetNomination(int nominationId);
        void UpdateNomination(Nomination model);
        void DeletePrevoiusManagerComments(int nominationId);
        void DiscardNomination(int nominationId);
        bool checkReviewIsDrafted(int nominationId);
        List<Award> LockNominations(List<int> awardIds);
        //bool IsNominationLocked();
        List<Award> UnLockNominations(List<int> awardIds);
        List<Award> GetAwardstoUnLockOrUnlock(string status);
        int GetNominationCountByManagerIdForSOM(int managerId, DateTime startDate, DateTime endDate, int awardId);
        int GetNominationCountByManagerIdForPINNACLE(int managerId, DateTime startDate, int awardId);
        List<Award> GetNominationLockStatus();
        FrequencyMaster GetAwardFrequencyByFrequencyCode(string frequencyCode);
        FrequencyMaster GetAwardFrequencyById(int id);

        #region Saved Nominations List

        string GetAwardNameByAwardId(int awardId);
        User GetNomineeDetails(int userId);
        List<Nomination> GetAllSubmittedAndSavedNominationsByCurrentUserAndMonth(int managerID, bool forCurrentMonth);

        #endregion
        void UpdateFinalScore(int nominationId);
    }
}
