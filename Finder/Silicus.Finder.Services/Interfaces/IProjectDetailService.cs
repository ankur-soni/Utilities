using System.Collections.Generic;
using Silicus.Finder.Models.DataObjects;

namespace Silicus.Finder.Services.Interfaces
{
    public interface IProjectDetailService
    {
        IEnumerable<ProjectDetail> GetProjectDetails();

        int Add(ProjectDetail projectDetail);

        void Update(ProjectDetail projectDetail);

        void Delete(ProjectDetail projectDetail);
    }
}
