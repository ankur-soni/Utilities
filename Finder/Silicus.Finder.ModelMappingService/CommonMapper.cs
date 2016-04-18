using Silicus.Finder.ModelMappingService.Interfaces;
using Silicus.Finder.Models.DataObjects;
using Silicus.UtilityContainer.Models.DataObjects;
using System;

namespace Silicus.Finder.ModelMappingService
{
    class CommonMapper : ICommonMapper
    {
        Silicus.UtilityContainerr.Entities.ICommonDataBaseContext _utilityCommonDbContext;

        public CommonMapper()
        {
            Silicus.UtilityContainerr.Entities.IDataContextFactory dataContextFactory = new Silicus.UtilityContainerr.Entities.DataContextFactory();
            _utilityCommonDbContext = dataContextFactory.CreateCommonDBContext();
        }

        public Employee MapUserToEmployee(User user)
        {
            throw new NotImplementedException();
        }

        public Project MapEngagementToProject(Engagement engagement)
        {
            var project = new Project();
            project.ProjectId = engagement.ID;
            project.ProjectName = engagement.Name;
            project.ProjectCode = engagement.Code;
            project.Description = engagement.Description;
          //  project.ProjectType=engagement.   
            project.EngagementType = (EngagementType)engagement.ContractTypeID;     
           // project.Status=
            project.StartDate = engagement.BeginDate;
            //project.ExpectedEndDate=engagement.
            project.ActualEndDate = engagement.EndDate;
            project.ArchiveDate=engagement.EndDate 


            return project;
        }

        public SkillSet MapSkillToSkillSet(Skill skill)
        {
            throw new NotImplementedException();
        }
    }
}
