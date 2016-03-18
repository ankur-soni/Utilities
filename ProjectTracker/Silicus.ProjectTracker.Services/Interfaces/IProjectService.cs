using Silicus.ProjectTracker.Models.DataObjects;
using System.Collections.Generic;

namespace Silicus.ProjectTracker.Services.Interfaces
{
    public interface IProjectService
    {
        IEnumerable<Project> GetProjects();
        
        int AddProject(Project project, string userName);

        int UpdateProject(Project project, string userName);

        void DeleteProject(Project project, string userName);

        Project GetProjectById(int projectId);

        IEnumerable<Status> AllStatus { get; }
       
        IEnumerable<Project> GetProjectList();

        IEnumerable<Project> GetProjectsByUsername(string UserName);

        int UpdateProjectSummary(Project summaryDetails);
                
        Project GetProjectSummary(int projectId);

        ProjectStatus GetProjectStatus(int projectId, int weekId);

        Project GetProjectByProjectName(string projectName);
    }
}
