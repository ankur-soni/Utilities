using System.Collections.Generic;
using System.Linq;
using Silicus.Ensure.Entities;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Services.Interfaces;

namespace Silicus.Ensure.Services
{
    public class ProjectDetailService : IProjectDetailService
    {
        private readonly IDataContext _context;

        public ProjectDetailService(IDataContextFactory dataContextFactory)
        {
            _context = dataContextFactory.Create(ConnectionType.Ip);
        }

        public IEnumerable<ProjectDetail> GetProjectDetails()
        {
            var productDetailsList = _context.Query<ProjectDetail>().ToList();
            return productDetailsList;

        }

        public int Add(ProjectDetail projectDetail)
        {
            _context.Add(projectDetail);
            return projectDetail.ProjectDetailId;
        }

        public void Update(ProjectDetail projectDetail)
        {           
            if (projectDetail.ProjectName != null && projectDetail.Status != null)
            {
                _context.Update(projectDetail);
            }
        }

        public void Delete(ProjectDetail projectDetail)
        {
            if (projectDetail.ProjectName != null && projectDetail.Status != null)
            {
                _context.Delete(projectDetail);
            }
        }
    }
}

