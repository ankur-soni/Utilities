using System;
using System.Linq;
using Silicus.ProjectTracker.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Silicus.ProjectTracker.Web.Hubs
{
    [HubName("trackerHub")]
    public class TrackerHub : Hub, ITrackerHub
    {
        public void UpdateDashboard()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<TrackerHub>();
            context.Clients.All.UpdateDashboard();
        }

        public void UpdateUserDashboard()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<TrackerHub>();
            context.Clients.All.UpdateUserDashboard();
        }
    }
}