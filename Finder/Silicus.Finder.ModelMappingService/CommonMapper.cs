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
            if (user != null)
            {
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
                employee.ProjectId = EngagementIds(user);
                if (employee.ProjectId.Count > 0)
                {
                   
                     var engagements =  EmployeeProjects(employee.ProjectId);
                     var projects = new List<Project>();
                     foreach (var engagement in engagements)
                     {
                         projects.Add(MapEngagementToProjectForEmployee(engagement));
                     }
                     employee.Projects = projects;
                   
                }

            }
            
            return employee;
        }




        public Project MapEngagementToProjectForEmployee(Engagement engagement)
        {
            var project = new Project();
            var commonDbContext = GetCommonDataBAseContext();

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

            var userInEngagement = commonDbContext.Query<EngagementUserPermission>().Where(model => model.EngagementID == engagement.ID).Select(model => model.UserID).ToList();
            project.EmployeeIds = userInEngagement.ToArray();

            
            return project;
        }




        public List<Engagement> EmployeeProjects(IList<int> engagementIds)
        {
            var engagements = new List<Engagement>();


            foreach (var engagementID in engagementIds)
            {
                engagements.Add(GetCommonDataBAseContext().Query<Engagement>().Where(e => e.ID == engagementID).SingleOrDefault());
            }

            return engagements;

        }


        public IList<int> EngagementIds(User user)
        {
           
            var resource = GetCommonDataBAseContext().Query<Resource>().Where(r => r.UserID == user.ID).SingleOrDefault();
            var resourceHistoryIds = GetCommonDataBAseContext().Query<ResourceHistory>().Where(rh => rh.ResourceID == resource.ID);
            var engagementRole = GetCommonDataBAseContext().Query<EngagementRole>();

            var engagementIdsOfCurrentUser = new List<int>();

            foreach (var resourceHistoryId in resourceHistoryIds)
            {
                var engagementIDs = engagementRole.Where(er => er.ResourceHistoryID == resourceHistoryId.ID).ToList();
                if (engagementIDs.Count>0)
                {
                    foreach (var engagementID in engagementIDs)
                    {
                        engagementIdsOfCurrentUser.Add(engagementID.EngagementID); 
                    }
                    
                }
            }
            
            
            
            return engagementIdsOfCurrentUser;
        }



        public Models.DataObjects.Role MapRoleToRole(Silicus.UtilityContainer.Models.DataObjects.Role role)
        {
            Silicus.Finder.Models.DataObjects.Role findersRole = new Models.DataObjects.Role();
            findersRole.RoleId = role.ID;
            findersRole.RoleName = role.Name;
            findersRole.Description = role.Name;

            return findersRole;
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

            var userInEngagement = commonDbContext.Query<EngagementUserPermission>().Where(model => model.EngagementID == engagement.ID).Select(model => model.UserID).ToList();
            project.EmployeeIds = userInEngagement.ToArray();

            foreach (int userId in userInEngagement)
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
