using Silicus.Encourage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Encourage.Services.Interface
{
   public interface IReviewService
    {
        IEnumerable<Review> GetReviewsForNomination(int nominationID);
        void UpdateReview(Review model);
        List<Review> GetAllReview();
        bool LockReview(List<int> awardIds);
        bool UnLockReview(List<int> awardIds);
        void DeletePrevoiusReviewerComments(int reviewerId, int nominationID);
       List<Award> GetReviewLockStatus();

    }
}
