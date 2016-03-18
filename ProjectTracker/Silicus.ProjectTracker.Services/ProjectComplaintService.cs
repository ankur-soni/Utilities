using System;
using System.Linq;
using System.Collections.Generic;
using Silicus.ProjectTracker.Core;
using Silicus.ProjectTracker.Entities;
using Silicus.ProjectTracker.Models.DataObjects;
using Silicus.ProjectTracker.Services.Interfaces;

namespace Silicus.ProjectTracker.Services
{   
    public class ProjectComplaintService : IProjectComplaintService
    {
        private readonly IDataContext context;
        private IGenericService _genericService;

        public ProjectComplaintService(IDataContextFactory dataContextFactory, IGenericService genericService)
        {
            this.context = dataContextFactory.Create(ConnectionType.Ip);
            _genericService = genericService;
        }

        public IEnumerable<ProjectComplaint> GetProjectComplaints(int projectId, int weekId)
        {
            int WeekId = _genericService.GetWeekIdFromMasterTable(weekId, DateTime.Now.Year);

            var currentWeek = _genericService.GetWeek(DateTime.Now.GetPreviousWeek());
            int currentWeekId = _genericService.GetWeekIdFromMasterTable(currentWeek, DateTime.Now.Year);

            IEnumerable<ProjectComplaint> projectComplaints = null;
            projectComplaints = this.context.Query<ProjectComplaint>().Where(s => s.ProjectId == projectId && s.IsActive == true).ToList();

            if (WeekId != 0)
            {
                projectComplaints = projectComplaints.Where(p => p.WeekId == WeekId).ToList();
            }

            if (projectComplaints.Count() == 0)
            {
                if (WeekId >= currentWeekId)
                {
                    ProjectComplaint prevWeekData = this.context.Query<ProjectComplaint>().Where(p => p.ProjectId == projectId).OrderByDescending(p => p.WeekId).FirstOrDefault();
                     if (prevWeekData != null)
                     {
                         projectComplaints = this.context.Query<ProjectComplaint>().Where(p => p.ProjectId == projectId && p.IsActive == true && p.WeekId == prevWeekData.WeekId).OrderByDescending(p => p.WeekId).ToList();
                         foreach (var item in projectComplaints)
                         {
                             item.ComplaintId = 0;
                         }
                     }
                }

            }

            return projectComplaints;
           
        }

        public int SaveProjectsComplaints(IList<ProjectComplaint> ProjectComplaintDetails, ProjectStatus ProjectStatus, string userName, int weekId)
        {
            try
            {
                // Save sprint details
                if (ProjectComplaintDetails != null)
                {
                    //Insert Data
                    foreach (var lst in ProjectComplaintDetails)
                    {
                        //New record inserted from the grid
                        if (lst.ComplaintId == 0)
                        {
                            lst.CreatedDate = DateTime.Now;
                            lst.CreatedBy = userName;
                            lst.ModifiedBy = userName;
                            lst.ModifiedDate = DateTime.Now;
                            lst.WeekId = weekId;
                            this.context.Add(lst);
                        }
                        //update the existing record 
                        else if (lst.ComplaintId != 0)
                        {
                            lst.ModifiedBy = userName;
                            lst.ModifiedDate = DateTime.Now;
                            this.context.Update(lst);
                        }
                    }

                    //Delete those sprints which are deleted from frontend
                    var lstSummaryProj = context.Query<ProjectComplaint>()
                                         .Where(ps => ps.ProjectId == ProjectStatus.ProjectId && ps.WeekId == weekId).ToList();

                    foreach (var lstDeleteProj in lstSummaryProj)
                    {
                        var isPresent = false;
                        foreach (var lst in ProjectComplaintDetails)
                        {
                            if (lstDeleteProj.ComplaintId == lst.ComplaintId)
                            {
                                isPresent = true;

                            }

                        }
                        if (isPresent == false)
                        {
                            lstDeleteProj.IsActive = false;
                            this.context.Update(lstDeleteProj);

                        }

                    }

                }
                return 1;
            }
            catch (Exception)
            {
                return -1;

            }

        }

       
    }
}
