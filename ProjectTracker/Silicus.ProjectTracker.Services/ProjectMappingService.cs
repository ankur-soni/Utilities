using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using Silicus.ProjectTracker.Entities;
using Silicus.ProjectTracker.Models.DataObjects;
using Silicus.ProjectTracker.Services.Interfaces;

namespace Silicus.ProjectTracker.Services
{
    public class ProjectMappingService :IProjectMappingService
    {
        private readonly IDataContext context;

        public ProjectMappingService(IDataContextFactory dataContextFactory)
        {
            this.context = dataContextFactory.Create(ConnectionType.Ip);
        }

        public IEnumerable<string> GetAssignedProjects(string userName)
        {
            return GetAssignedProjectsByUserId(userName).Select(s => s.ProjectId.ToString(CultureInfo.InvariantCulture));
        }

        public IEnumerable<ProjectMapping> GetAssignedProjectsByUserId(string userName)
        {
            IEnumerable<ProjectMapping> project = null;
            project = this.context.Query<ProjectMapping>().Where(s => s.UserName == userName && s.IsDeleted == false).ToList();
            
            return project;
        }

        public bool SaveProjectsToUser(List<string> selectedValues, string user)
        {
            var list = new List<int>();

            if (string.IsNullOrEmpty(selectedValues.First()))
            {
                list = null;
            }
            else
            {
                list = selectedValues.Select(int.Parse).ToList();
            }

            List<ProjectMapping> projectList = new List<ProjectMapping>();
            ProjectMapping project = new ProjectMapping();
            projectList = this.context.Query<ProjectMapping>().Where(s => s.UserName == user && s.IsDeleted == false).ToList();
          
            if (projectList != null)
            {
                foreach (ProjectMapping prj in projectList)
                {
                    prj.IsDeleted = true;
                    prj.ModifiedBy = user;
                    prj.ModifiedDate = DateTime.Now;
                    this.context.Update(prj);
                }
            }

            if (list != null)
            {
                foreach (int lst in list)
                {
                    project.IsDeleted = false;
                    project.ProjectId = lst;
                    project.UserName = user;
                    project.CreatedBy = user;
                    project.CreatedDate = DateTime.Now;
                    project.ModifiedBy = user;
                    project.ModifiedDate = DateTime.Now;
                    this.context.Add(project);
                }
            }
            
          
            return true;
        }

        public bool IsProjectMapped(int projectId,string userName)
        {
            IEnumerable<ProjectMapping> projectMappingList = this.context.Query<ProjectMapping>().Where(s => s.ProjectId == projectId && s.IsDeleted == false && s.UserName == userName);
            
            if (projectMappingList.Any())
            {
                return true;
            }
            else
            {               
                projectMappingList = this.context.Query<ProjectMapping>().Where(s => s.ProjectId == projectId && s.IsDeleted == false && s.UserName != userName);
                if (projectMappingList.Any())
                {
                    return false;
                }
                else
                {
                    return true;
                }
              
              
            }

        }

        public int GetCountsOfAssignedProject()
        {
            IEnumerable<Project> projectList = this.context.Query<Project>().Where(p => p.IsActive == true);
            IEnumerable<ProjectMapping> projectMappingList = this.context.Query<ProjectMapping>().Where(p => p.IsDeleted == false);

            var counts = (from p in projectList
                          join pm in projectMappingList on p.ProjectId equals pm.ProjectId into ppm
                          from pm in ppm.DefaultIfEmpty()
                          select p).Count();

            return counts;
                         
        }

        public int GetCountsOfNonAssignedProject()
        {
            IEnumerable<Project> projectList = this.context.Query<Project>().Where(p => p.IsActive == true);
            IEnumerable<ProjectMapping> projectMappingList = this.context.Query<ProjectMapping>().Where(p => p.IsDeleted == false);

            var counts = (from p in projectList
                          select p.ProjectId).Except(from pm in projectMappingList select pm.ProjectId).Count();

            return counts;

        }
        
    }
}
