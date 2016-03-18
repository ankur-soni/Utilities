using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.ProjectTracker.Web.ViewModel
{
    public class ProjectsTotalCountsViewModel
    {
        public int NoOfProjects { get; set; }

        public int NoOfManagers{ get; set; }

        public int NoOfAssignedProjects { get; set; }

        public int NoOfUnAssignedProjects { get; set; }

        public IList<ProjectStatusAdminDashBoardModel> ProjectStatusAdminDashBoardModel { get; set; }
    }
}