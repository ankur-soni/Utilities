using System;
using System.Linq;
using System.Collections.Generic;
using Silicus.ProjectTracker.Core;
using Silicus.ProjectTracker.Entities;
using Silicus.ProjectTracker.Models.DataObjects;
using Silicus.ProjectTracker.Services.Interfaces;

namespace Silicus.ProjectTracker.Services
{
    public class ProjectResourceService : IProjectResourceService
    {
        private readonly IDataContext context;
        private IGenericService _genericService;

        public ProjectResourceService(IDataContextFactory dataContextFactory, IGenericService genericService)
        {
            this.context = dataContextFactory.Create(ConnectionType.Ip);
            _genericService = genericService;
        }

        public IList<ProjectResourceUtilization> GetProjectResources(int projectId, int WeekId)
        {
            int weekId = _genericService.GetWeekIdFromMasterTable(WeekId, DateTime.Now.Year);

            var currentWeek = _genericService.GetWeek(DateTime.Now.GetPreviousWeek());
            int currentWeekId = _genericService.GetWeekIdFromMasterTable(currentWeek, DateTime.Now.Year);

            var projectResources = this.context.Query<ProjectResourceUtilization>()
                .Where(p => p.ProjectId == projectId && p.IsActive == true).ToList();

            if (WeekId != 0)
            {
                projectResources = projectResources.Where(p => p.WeekId == weekId).ToList();
            }

            if (projectResources.Count() == 0)
            {
                if (weekId >= currentWeekId)
                {
                    ProjectResourceUtilization prevWeekData = this.context.Query<ProjectResourceUtilization>().Where(p => p.ProjectId == projectId).OrderByDescending(p => p.WeekId).FirstOrDefault();
                    if (prevWeekData != null)
                    {
                        projectResources = this.context.Query<ProjectResourceUtilization>().Where(p => p.ProjectId == projectId && p.IsActive == true && p.WeekId == prevWeekData.WeekId).OrderByDescending(p => p.WeekId).ToList();
                        foreach (var item in projectResources)
                        {
                            item.ProjectResourceId = 0;
                        }
                    }
                }

            }

            return projectResources;
        }

        public int SaveProjectResources(IList<ProjectResourceUtilization> ProjectResourceDetails, ProjectStatus projectStatus, int weekId, string userName)
        {
            try
            {
                if (ProjectResourceDetails != null)
                {
                    foreach (var lst in ProjectResourceDetails)
                    {
                        if (lst.ProjectResourceId == 0)
                        {
                            lst.CreatedBy = userName;
                            lst.CreatedDate = DateTime.Now;
                            lst.ModifiedBy = userName;
                            lst.ModifiedDate = DateTime.Now;
                            lst.WeekId = weekId;
                            this.context.Add(lst);
                        }
                        else if (lst.ProjectResourceId != 0)
                        {
                            lst.ModifiedBy = userName;
                            lst.ModifiedDate = DateTime.Now;
                            lst.WeekId = weekId;
                            this.context.Update(lst);
                        }

                    }

                    var dbProjectResources =  context.Query<ProjectResourceUtilization>()
                                             .Where(ps => ps.ProjectId == projectStatus.ProjectId
                                              && ps.WeekId == weekId).ToList();

                    foreach (var dbProjectResource in dbProjectResources)
                    {
                        var isPresent = false;
                        foreach (var lst in ProjectResourceDetails)
                        {
                            if (dbProjectResource.ProjectResourceId == lst.ProjectResourceId)
                            {
                                isPresent = true;
                            }
                        }

                        if (isPresent == false)
                        {
                            dbProjectResource.ModifiedBy = userName;
                            dbProjectResource.ModifiedDate = DateTime.Now;
                            dbProjectResource.IsActive = false;
                            this.context.Update(dbProjectResource);
                         
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
