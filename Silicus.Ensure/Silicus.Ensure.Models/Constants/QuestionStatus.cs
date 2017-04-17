using System.ComponentModel;

namespace Silicus.Ensure.Models.Constants
{
    public enum QuestionStatus
    {
        [Description("Ready For Review")]
        ReadyForReview=1,
        Approved,
        [Description("On Hold")]
        OnHold,
        Rejected

    }
}
