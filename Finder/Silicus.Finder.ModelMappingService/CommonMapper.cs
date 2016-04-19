using Silicus.Finder.ModelMappingService.Interfaces;
using Silicus.Finder.Models.DataObjects;
using Silicus.UtilityContainer.Models.DataObjects;
using Silicus.UtilityContainerr.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public Models.DataObjects.Role MapRoleToRole(Silicus.UtilityContainer.Models.DataObjects.Role role)
        { 
            Silicus.Finder.Models.DataObjects.Role findersRole=new Models.DataObjects.Role();
             findersRole.RoleId= role.ID;
            findersRole.RoleName=role.Name;
            findersRole.Description = role.Name ;

             return findersRole;
        }

        public Employee MapUserToEmployee(User user)
        {
            var employee = new Employee();
            var contact = new Contact();
            contact.EmailAddress = user.EmailAddress;
            contact.MobileNumber = Convert.ToInt64( user.MobilePhone);
            contact.PhoneNumber = user.OfficePhone;
            
            employee.EmployeeId = user.ID;
            employee.EmployeeCode = user.EmployeeID;
            employee.FirstName = user.FirstName;
            employee.MiddleName = user.MiddleName;
            employee.LastName = user.LastName;
            employee.Contact = contact;
            employee.Role = user.PrimaryRoleID.ToString();
            employee.ProjectId = EngagementUserPermissionToProject(employee);
            if (employee.ProjectId.Count>0)
            {
                employee.Projects = EmployeeProjects(employee.ProjectId);
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
                if (item.UserID==employee.EmployeeId)
                {
                    if (item.EngagementID !=null )
                    {
                        ProjectIds.Add(Convert.ToInt32( item.EngagementID));
                    }
                   
                }
            }
           
            return ProjectIds;

        }

        public Project MapEngagementToProject(Engagement engagement)
        {
            var project = new Project();
            project.ProjectId = engagement.ID;
            project.ProjectName = engagement.Name;
            project.ProjectCode = engagement.Code;
            project.Description = engagement.Description;
          //  project.ProjectType=engagement.  
            if (engagement.ContractTypeID != null)
            {
                project.EngagementType = (EngagementType)engagement.ContractTypeID;    
            }
              
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
