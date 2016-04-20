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
                employee.ProjectId = EngagementUserPermissionToProject(employee);
                if (employee.ProjectId.Count > 0)
                {
                    employee.Projects = EmployeeProjects(employee.ProjectId);
                }

            }

            return employee;
        }


        public List<Project> EmployeeProjects(IList<int> projectIds)
        {
            var projects = new List<Project>();

            // var engagements = GetCommonDataBAseContext().Query<Engagement>();

            foreach (var projectId in projectIds)
            {
                projects.Add(MapEngagementToProject(GetCommonDataBAseContext().Query<Engagement>().Where(e => e.ID == projectId).SingleOrDefault()));
              
            }
            return projects;

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

            foreach (int userId in userInEngagement)
            {
                var user = commonDbContext.Query<User>().Where(model => model.ID == userId).FirstOrDefault();
                project.Employees.Add(MapUserToEmployee(user));
            }

            return project;
        }


        public SkillSet MapSkillToSkillSet(Skill skill)
        {
            var _commonDBContext = GetCommonDataBAseContext();
            var resourceList = _commonDBContext.Query<Resource>().ToList();
            var resourceSkillList = _commonDBContext.Query<ResourceSkillLevel>().ToList();
            var userSkillList = new List<ResourceSkillLevel>();
            var skillSet = new SkillSet();
            skillSet.SkillSetId = skill.ID;
            if (skill.Parent == null)
            {
                skillSet.Name = skill.Name;
            }
            else
            {
                skillSet.Name = skill.Parent.Name;
                skillSet.Description = skill.Name;
            }
            foreach (var resourceSkill in resourceSkillList)
            {
                if (resourceSkill.SkillID == skill.ID)
                {
                    userSkillList.Add(resourceSkill);
                }
            }
            foreach (var user in userSkillList)
            {
                foreach (var resource in resourceList)
                {
                    if (user.ResourceID == resource.ID)
                    {
                        skillSet.Employees.Add(MapUserToEmployee(resource.User));
                    }

                }

            }
            return skillSet;
        }
    }
}
