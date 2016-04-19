using Silicus.Finder.ModelMappingService.Interfaces;
using Silicus.Finder.Models.DataObjects;
using Silicus.UtilityContainer.Models.DataObjects;
using Silicus.UtilityContainerr.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Silicus.Finder.ModelMappingService
{
    public class CommonMapper : ICommonMapper
    {
        //Silicus.UtilityContainerr.Entities.ICommonDataBaseContext _utilityCommonDbContext;

        //public CommonMapper()
        //{
        //    Silicus.UtilityContainerr.Entities.IDataContextFactory dataContextFactory = new Silicus.UtilityContainerr.Entities.DataContextFactory();
        //    _utilityCommonDbContext = dataContextFactory.CreateCommonDBContext();
        //}

        public ICommonDataBaseContext GetCommonDataBAseContext()
        {
            IDataContextFactory _dataContextFactory = new DataContextFactory();
            return _dataContextFactory.CreateCommonDBContext();
        }

        public Employee MapUserToEmployee(User user)
        {
            var employee = new Employee();
            var contact = new Contact();
            contact.EmailAddress = user.EmailAddress;
            contact.MobileNumber = Convert.ToInt64(user.MobilePhone);
            contact.PhoneNumber = user.OfficePhone;

            employee.EmployeeId = user.ID;
            employee.EmployeeCode = user.EmployeeID;
            employee.FirstName = user.FirstName;
            employee.MiddleName = user.MiddleName;
            employee.LastName = user.LastName;
            employee.Contact = contact;
            employee.Role = user.PrimaryRoleID.ToString();
            employee.ProjectId = EngagementUserPermissionToProject(employee);
            return employee;
        }

        public IList<int> EngagementUserPermissionToProject(Employee employee)
        {
            var ProjectIds = new List<int>();
            var projectMappingToUSer = GetCommonDataBAseContext().Query<EngagementUserPermission>();
            foreach (var item in projectMappingToUSer)
            {
                if (item.UserID == employee.EmployeeId)
                {
                    if (item.EngagementID != null)
                    {
                        ProjectIds.Add(Convert.ToInt32(item.EngagementID));
                    }

                }
            }
            return ProjectIds;
        }

        public Project MapEngagementToProject(Engagement engagement)
        {
            var project = new Project();
            var commonDbContext=GetCommonDataBAseContext();

            project.ProjectId = engagement.ID;

            project.ProjectName = engagement.Name;

            project.ProjectCode = engagement.Code;

            project.Description = engagement.Description;

            //project.ProjectType=engagement.   

            project.EngagementType = engagement.ContractTypeID == null ? EngagementType.None : (EngagementType)engagement.ContractTypeID;

            if (engagement.BeginDate > DateTime.Now)
                project.Status = Status.Not_Started;
            else if (engagement.EndDate > DateTime.Now && engagement.BeginDate < DateTime.Now)
                project.Status = Status.On_Going;
            else if (engagement.EndDate < DateTime.Now)
                project.Status = Status.Completed;

            project.StartDate = engagement.BeginDate;

            //project.ExpectedEndDate=engagement.

            project.ActualEndDate = engagement.EndDate;

            //project.ArchiveDate=engagement.

            project.EngagementManagerId = engagement.EngagementManagerID;

            project.ProjectManagerId = engagement.PrimaryProjectManagerID;

            //project.AdditionalNotes=engagement.

            //project.skillSetId=

            var userInEngagement = commonDbContext.Query<EngagementUserPermission>().Where(model => model.EngagementID == engagement.ID).Select(model=>model.ID).ToList();
         //   project.EmployeeIds = userInEngagement.ToArray();
           
            foreach(int userId in userInEngagement)
            {
                var user = commonDbContext.Query<User>().Where(model => model.ID == userId).FirstOrDefault();
                project.Employees.Add(MapUserToEmployee(user));
            }

            return project;
        }


        public SkillSet MapSkillToSkillSet(Silicus.UtilityContainer.Models.DataObjects.Skill skill)
        {
            throw new NotImplementedException();
        }
    }
}
