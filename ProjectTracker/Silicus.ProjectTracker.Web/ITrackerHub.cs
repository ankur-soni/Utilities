using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.ProjectTracker.Web
{
    public interface ITrackerHub
    {
        void UpdateDashboard();

        void UpdateUserDashboard();
    }
}