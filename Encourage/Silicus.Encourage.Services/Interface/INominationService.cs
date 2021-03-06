﻿using Silicus.Encourage.Models;
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
        List<Nomination> GetAllSubmitedReviewedNominations(int reviewerId, bool forCurrentMonth, int awardId);
        List<Review> GetAllSubmitedReviewsForCurrentNomination(int nominationId);
        List<Nomination> GetAllSavedNominations();
        Nomination GetNomination(int nominationId);
        void UpdateNomination(Nomination model);
        void DeletePrevoiusManagerComments(int nominationId);
        void DiscardNomination(int nominationId);
        bool CheckReviewIsDrafted(int nominationId);
        List<Award> LockNominations(List<int> awardIds);
        List<Award> UnLockNominations(List<int> awardIds);
        List<Award> GetAwardstoUnLockOrUnlock(string status);
        int GetNominationCountByManagerIdForSom(int managerId, DateTime startDate, DateTime endDate, int awardId);
        int GetNominationCountByManagerIdForPinnacle(int managerId, DateTime startDate, int awardId);
        List<Award> GetNominationLockStatus();
        bool GetAwardNominationLockStatus(int awardId);
        FrequencyMaster GetAwardFrequencyByFrequencyCode(string frequencyCode);
        FrequencyMaster GetAwardFrequencyById(int id);
        List<User> GetAllResources();
        List<Nomination> GetNominationsByDate(int month, int year, int awardId);
        List<User> GetAllResourcesForOtherReason(int awardId,int managerId);
        #region Saved Nominations List
        string GetAwardNameByAwardId(int awardId);
        User GetNomineeDetails(int userId);
        List<Nomination> GetAllSubmittedAndSavedNominationsByCurrentUserAndMonth(int managerId, bool forCurrentMonth,int awardId);
        #endregion
        void UpdateFinalScore(int nominationId);
    }
}
