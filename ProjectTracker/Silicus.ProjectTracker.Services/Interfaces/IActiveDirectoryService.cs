using Silicus.ProjectTracker.Models.DataObjects;
using System.Collections.Generic;

namespace Silicus.ProjectTracker.Services.Interfaces
{
    public interface IActiveDirectoryService
    {
        List<ActiveDirectory> GetActiveDirectoryUsers(string user = "");

        bool VerifyLoggedInUser(string username,string password);

        string VerifyGroupPolicy(string username);
    }
}

