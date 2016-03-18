using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Silicus.ProjectTracker.Core;
using Silicus.ProjectTracker.Entities;
using Silicus.ProjectTracker.Models.DataObjects;
using Silicus.ProjectTracker.Services.Interfaces;

namespace Silicus.ProjectTracker.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IDataContext context;
        private IGenericService _genericService;

        public ProjectService(IDataContextFactory dataContextFactory, IGenericService genericService)
        {
            this.context = dataContextFactory.Create(ConnectionType.Ip);
            _genericService = genericService;
        }

        public IEnumerable<Project> GetProjects()
        {
            var projects = this.context.Query<Project>();
            return projects;
        }

        public int AddProject(Project project, string userName)
        {
            project.CreatedDate = DateTime.Now;
            project.CreatedBy = userName;
            project.ModifiedDate = DateTime.Now;
            project.ModifiedBy = userName;
            this.context.Add(project);
            return project.ProjectId;
        }

        public int UpdateProject(Project project, string userName)
        {
            var dbProject = this.context.Query<Project>().FirstOrDefault(p => p.ProjectId == project.ProjectId);
            project.CreatedDate = dbProject.CreatedDate;
            project.CreatedBy = dbProject.CreatedBy;
            project.ModifiedDate = DateTime.Now;
            project.ModifiedBy = userName;
            this.context.Update(project);
            return project.ProjectId;
        }

        public void DeleteProject(Project project, string userName)
        {
            if (project.ProjectName != null)
            {
                var dbProject = this.context.Query<Project>().FirstOrDefault(p => p.ProjectId == project.ProjectId);
                project.CreatedDate = dbProject.CreatedDate;
                project.CreatedBy = dbProject.CreatedBy;
                project.ModifiedDate = DateTime.Now;
                project.ModifiedBy = userName;
                project.IsActive = false;
                this.context.Update(project);
            }
        }

        public Project GetProjectById(int projectId)
        {
            return this.context.Query<Project>().FirstOrDefault(p => p.ProjectId == projectId);
        }

        public Project GetProjectByProjectName(string projectName)
        {
            return this.context.Query<Project>().FirstOrDefault(p => p.ProjectName.ToLower().Contains(projectName.ToLower()));
        }

        public IEnumerable<Status> AllStatus
        {
            get
            {
                return this.context.Query<Status>();
            }
        }
               
        public IEnumerable<Project> GetProjectList()
        {
            IEnumerable<Project> projects = this.context.Query<Project>();
            return projects;
        }

        public IEnumerable<Project> GetProjectsByUsername(string userName)
        {
            var userProject = context.Query<ProjectMapping>().Where(um => um.UserName == userName);

            var projects = context.Query<Project>()
                .Where(p => p.ProjectId == (userProject.Where(uu => uu.ProjectId == p.ProjectId)).FirstOrDefault().ProjectId);

            return projects;
        }

        public Project GetProjectSummary(int projectId)
        {
            var projects = context.Query<Project>().Where(p => p.ProjectId == projectId).FirstOrDefault();
            return projects;
        }

        public int UpdateProjectSummary(Project summaryDetails)
        {
            this.context.Update(summaryDetails);

            int result = this.context.SaveChanges();

            if (result == -1)
            {
                return result;
            }

            return summaryDetails.ProjectId;
        }

        public ProjectStatus GetProjectStatus(int projectId, int weekId)
        {
            int WeekId = _genericService.GetWeekIdFromMasterTable(weekId, DateTime.Now.Year);
            
            var currentWeek = _genericService.GetWeek(DateTime.Now.GetPreviousWeek());
            int currentWeekId = _genericService.GetWeekIdFromMasterTable(currentWeek, DateTime.Now.Year);

            var projectStatus = this.context.Query<ProjectStatus>().Where(p => p.ProjectId == projectId);
            if (WeekId != 0)
            {
                projectStatus = projectStatus.Where(p => p.WeekId == WeekId);                  
            }

            if (projectStatus.Count() == 0)
            {
                if (WeekId >= currentWeekId)
                {
                    ProjectStatus prevWeekData = this.context.Query<ProjectStatus>().Where(p => p.ProjectId == projectId).OrderByDescending(p => p.WeekId).FirstOrDefault();
                    if (prevWeekData != null)
                    {
                        projectStatus = this.context.Query<ProjectStatus>().Where(p => p.ProjectId == projectId && p.WeekId == prevWeekData.WeekId).OrderByDescending(p => p.WeekId);
                    }
                }

            }
            return projectStatus.FirstOrDefault();
        }

        //public bool IsDuplicateProject(Project project)
        //{
        //    var projects = this.context.Query<Project>().Where(p => p.ProjectName.ToUpper() == project.ProjectName.ToUpper()).FirstOrDefault();
        //    if (projects != null)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
    }
}
