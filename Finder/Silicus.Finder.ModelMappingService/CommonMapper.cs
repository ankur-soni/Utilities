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


        public Employee MapBasicPropertiesOfUserToEmployee(User user)
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
                employee.Title = GetUserTitle(user);
                employee.IsActive = UserIsActive(user);
                employee.EmployeeType = GetResourceType(user);
                
                var resourceId= GetCommonDataBAseContext().Query<Resource>().Where(r => r.UserID == user.ID).SingleOrDefault().ID;
                var joiningDate = GetCommonDataBAseContext().Query<ResourceHistory>().Where(rh => rh.ResourceID == resourceId).FirstOrDefault().EffectiveDate;
                employee.SilicusExperienceInMonths = (DateTime.Now.Month-joiningDate.Month)+12*(DateTime.Now.Year-joiningDate.Year);

                employee.Contact = contact;
                employee.Role = user.PrimaryRoleID.ToString();
                }
            return employee;
        }

        public Employee MapUserToEmployee(User user)
        {
            var employee = MapBasicPropertiesOfUserToEmployee(user);

            employee.SkillId = GetSkillIds(user);



             //if (employee.SkillId.Count > 0)
             //{
             //    var skills = EmployeeSkills(employee.SkillId);
             //    var skillSets = new List<SkillSet>();
             //    foreach (var skill in skills)
             //    {
             //        skillSets.Add(MapSkillToSkillSet(skill));
             //    }
             //    employee.SkillSets = skillSets;

             //}
           
                employee.ProjectId = GetEngagementIds(user);

                if (employee.ProjectId.Count > 0)
                {
                    var engagements = EmployeeProjects(employee.ProjectId);
                    var projects = new List<Project>();
                    foreach (var engagement in engagements)
                    {
                        projects.Add(MapBasicPropertiesOfEngagementToProject(engagement));
                    }
                    employee.Projects = projects;
                }
            
            return employee;
        }



        public bool UserIsActive(User user)
        {
            var resourceId = GetCommonDataBAseContext().Query<Resource>().Where(r => r.UserID == user.ID).SingleOrDefault().ID;
            var isActive = GetCommonDataBAseContext().Query<ResourceHistory>().Where(rh => rh.ResourceID == resourceId).FirstOrDefault().IsActive;
            return isActive;
        }

        public string GetUserTitle(User user)
        {
            var resourceId = GetCommonDataBAseContext().Query<Resource>().Where(r => r.UserID == user.ID).SingleOrDefault().ID;
            var titleId = GetCommonDataBAseContext().Query<ResourceHistory>().Where(rh => rh.ResourceID == resourceId).FirstOrDefault().TitleID;
            var title = GetCommonDataBAseContext().Query<Silicus.UtilityContainer.Models.DataObjects.Title>().Where(t => t.ID == titleId).SingleOrDefault().Name;
            return title;

            }

        public string GetResourceType(User user)
        {
             var resourceId = GetCommonDataBAseContext().Query<Resource>().Where(r => r.UserID == user.ID).SingleOrDefault().ID;
            var resourceTypeId = GetCommonDataBAseContext().Query<ResourceHistory>().Where(rh => rh.ResourceID == resourceId).FirstOrDefault().ResourceTypeID;
            var resourceType = GetCommonDataBAseContext().Query<ResourceType>().Where(rt=>rt.ID==resourceTypeId).SingleOrDefault().Name;

            return resourceType;
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


        public List<Skill> EmployeeSkills(IList<int> skillIds)
        {
            var skills = new List<Skill>();

            foreach (var skillId in skillIds)
            {
                skills.Add(GetCommonDataBAseContext().Query<Skill>().Where(s => s.ID == skillId).SingleOrDefault());
            }
            return skills;

        }

        public IList<int> GetEngagementIds(User user)
        {
            var resource = GetCommonDataBAseContext().Query<Resource>().Where(r => r.UserID == user.ID).SingleOrDefault();
            var resourceHistoryIds = GetCommonDataBAseContext().Query<ResourceHistory>().Where(rh => rh.ResourceID == resource.ID);
            var engagementRole = GetCommonDataBAseContext().Query<EngagementRole>();

            var engagementIdsOfCurrentUser = new List<int>();

            foreach (var resourceHistoryId in resourceHistoryIds)
            {
                var engagementIDs = engagementRole.Where(er => er.ResourceHistoryID == resourceHistoryId.ID).ToList();
                if (engagementIDs.Count > 0)
                {
                    foreach (var engagementID in engagementIDs)
                    {
                        engagementIdsOfCurrentUser.Add(engagementID.EngagementID);
                    }
                }
            }
            return engagementIdsOfCurrentUser;
        }

        public IList<int> GetSkillIds(User user)
        {
            var resource = GetCommonDataBAseContext().Query<Resource>().Where(r => r.UserID == user.ID).SingleOrDefault();
            var resourceSkillLevels = GetCommonDataBAseContext().Query<ResourceSkillLevel>().Where(rh => rh.ResourceID == resource.ID).ToList();
            var skillIds = new List<int>();

            if (resourceSkillLevels.Count>0)
            {
                foreach (var resourceSkillLevel in resourceSkillLevels)
                {
                    skillIds.Add(resourceSkillLevel.SkillID);
                }  
            }



            return skillIds;
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
            var project = MapBasicPropertiesOfEngagementToProject(engagement);
            var commonDbContext = GetCommonDataBAseContext();

            var userInEngagement = from engagementRole in commonDbContext.Query<EngagementRole>()
                                   join resourceHistory in commonDbContext.Query<ResourceHistory>() on engagementRole.ResourceHistoryID equals resourceHistory.ID
                                   join resource in commonDbContext.Query<Resource>() on resourceHistory.ResourceID equals resource.ID
                                   join user in commonDbContext.Query<User>() on resource.UserID equals user.ID
                                   where engagementRole.EngagementID == engagement.ID
                                   select user;

            foreach (User user in userInEngagement)
                project.Employees.Add(MapBasicPropertiesOfUserToEmployee(user));

            return project;
        }

        public Project MapBasicPropertiesOfEngagementToProject(Engagement engagement)
        {
            var project = new Project();
            var commonDbContext = GetCommonDataBAseContext();

            project.ProjectId = engagement.ID;
            project.ProjectName = engagement.Name;
            project.ProjectCode = engagement.Code;
            project.Description = engagement.Description;
            //project.ProjectType=engagement.   
            project.EngagementType = engagement.ContractTypeID == null ? Silicus.Finder.Models.DataObjects.EngagementType.None : (Silicus.Finder.Models.DataObjects.EngagementType)engagement.ContractTypeID;

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

        public SkillSet MapSkillToEmployee(SkillSet skillSet)
        {
            var _commonDBContext = GetCommonDataBAseContext();
            var skill = _commonDBContext.Query<Skill>().Where(x => x.ID == skillSet.SkillSetId).FirstOrDefault();
            var resourceList = _commonDBContext.Query<Resource>().ToList();
            var resourceSkillList = _commonDBContext.Query<ResourceSkillLevel>().ToList();
            var userSkillList = new List<ResourceSkillLevel>();
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
            skillSet.Employees.Add(MapBasicPropertiesOfUserToEmployee (resource.User));
                    }

                }

            }
            return skillSet;
        }

        public SkillSet MapSkillToSkillSet(Skill skill)
        {
            var _commonDBContext = GetCommonDataBAseContext();
            
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
           
            return skillSet;
        }
    }
}
