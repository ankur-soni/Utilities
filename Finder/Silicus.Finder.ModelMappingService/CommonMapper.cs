using Silicus.Finder.ModelMappingService.Interfaces;
using Silicus.Finder.Models.DataObjects;
using Silicus.UtilityContainer.Models.DataObjects;
using Silicus.UtilityContainerr.Entities;
using System;

namespace Silicus.Finder.ModelMappingService
{
  public  class CommonMapper : ICommonMapper
    {
        //Silicus.UtilityContainerr.Entities.ICommonDataBaseContext _utilityCommonDbContext;

        //public CommonMapper()
        //{
        //    Silicus.UtilityContainerr.Entities.IDataContextFactory dataContextFactory = new Silicus.UtilityContainerr.Entities.DataContextFactory();
        //    _utilityCommonDbContext = dataContextFactory.CreateCommonDBContext();
        //}

        public ICommonDataBaseContext GetCommonDataBAseContext()
        {
            // ICommonDataBaseContext _commonDatabaseContext=null;
            IDataContextFactory _dataContextFactory =new DataContextFactory();
            return _dataContextFactory.CreateCommonDBContext();
        }

        public Employee MapUserToEmployee(User user)
        {
            var employee = new Employee();
            employee.EmployeeId = user.ID;
            employee.EmployeeCode = user.EmployeeID;
            employee.FirstName = user.FirstName;
            employee.MiddleName = user.MiddleName;
            employee.LastName = user.LastName;
            //employee.Contact.EmailAddress = user.EmailAddress;
            //employee.Contact.MobileNumber = Convert.ToInt64(user.MobilePhone);
           // employee.Contact.PhoneNumber = user.OfficePhone;
            employee.Role = user.PrimaryRoleID.ToString();
            return employee;

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
            project.ArchiveDate = engagement.EndDate;


            return project;
        }

        public SkillSet MapSkillToSkillSet(Silicus.UtilityContainer.Models.DataObjects.Skill skill)
        {
            throw new NotImplementedException();
        }
    }
}
