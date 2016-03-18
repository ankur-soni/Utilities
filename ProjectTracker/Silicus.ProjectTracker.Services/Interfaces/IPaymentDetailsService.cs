using Silicus.ProjectTracker.Models.DataObjects;
using System.Collections.Generic;

namespace Silicus.ProjectTracker.Services.Interfaces
{
    public interface IPaymentDetailsService
    {
        IList<PaymentDetails> GetPaymentDetails(int projectId, int WeekId);

        int SavePaymentDetails(IList<PaymentDetails> PaymentDetails, ProjectStatus projectStatus, int weekId, string userName);
    }
}
