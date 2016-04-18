using Silicus.Finder.ModelMappingService.Interfaces;
using Silicus.Finder.Models.DataObjects;
using System;

namespace Silicus.Finder.ModelMappingService
{
    public class CommonMapper : ICommonMapper
    {
       

        //public Employee MapUserToEmployee(User user)
        //{
        //    throw new NotImplementedException();
        //}

        //public Project MapEngagementToProject(Engagement engagement)
        //{
        //    var project = new Project();
        //    project.ProjectId = engagement.ID;
        //    project.ProjectName = engagement.Name;
        //    project.ProjectCode = engagement.Code;
        //    project.Description = engagement.Description;
        //  //  project.ProjectType=engagement.   
        //    project.EngagementType = (EngagementType)engagement.ContractTypeID;     
        //   // project.Status=
        //    project.StartDate = engagement.BeginDate;
        //    //project.ExpectedEndDate=engagement.
        //    project.ActualEndDate = engagement.EndDate;
        //    project.ArchiveDate=engagement.EndDate 


        //    return project;
        //}

        public SkillSet MapSkillToSkillSet(Skill skill)
        {
            var skillSet = new SkillSet();
            skillSet.Name = skill.Name;
            //skillSet.Employees=
            // throw new NotImplementedException();
            return skillSet;
        }
    }
}
