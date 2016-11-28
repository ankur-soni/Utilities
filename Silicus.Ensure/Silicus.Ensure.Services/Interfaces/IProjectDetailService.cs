using System.Collections.Generic;
using Silicus.Ensure.Models.DataObjects;

namespace Silicus.Ensure.Services.Interfaces
{
    public interface IProjectDetailService
    {
        IEnumerable<ProjectDetail> GetProjectDetails();

        int Add(ProjectDetail projectDetail);

        void Update(ProjectDetail projectDetail);

        void Delete(ProjectDetail projectDetail);
    }
}
