using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.UtilityContainer.Models
{
    public enum EventType
    {
        SendNominationEmail,
        LockNomination,
        UnLockNominations,
        SendReviewNominationEmail,
        LockReview,
        UnLockReviews,
        SendAdminNominationEmail,
    }
}
