//using Eda.RDBI.Web.ViewModel;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Silicus.Finder.Models;
using Silicus.Finder.Models.DataObjects;
using Silicus.Finder.Web.ViewModel;
using Silicus.Finder.Web.Controllers;
using Silicus.Finder.IdentityWrapper.Models;

namespace Silicus.Finder.Web.Mappings
{
    [ExcludeFromCodeCoverage]
    public class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToViewModelMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<EntityA, Model>();

            //Mapper.CreateMap<Employee, EmployeeNameViewModel>();
            Mapper.CreateMap<Employee, EmployeeViewModel>();
            Mapper.CreateMap<Project,ProjectListViewModel>();
            Mapper.CreateMap<Employee, EmployeesListViewModel>();
            Mapper.CreateMap<Project, ProjectDetailsViewModel>();
            Mapper.CreateMap<Project, ProjectCreateViewModel>().ReverseMap();
            Mapper.CreateMap<Project, ProjectEditViewModel>();
            Mapper.CreateMap<ApplicationUser, ApplicationUserViewModel>();
            Mapper.CreateMap<SkillSet, SkillSetViewModel>();
            Mapper.CreateMap<SkillSet, ProjectSkillSetDetailsViewModel>();
            Mapper.CreateMap<Employee, ProjectEmployeeDetailsViewModel>();
            Mapper.CreateMap<Employee, EmployeeSelectViewModel>();
            Mapper.CreateMap<Employee, EmployeeCreateViewModel>();
            Mapper.CreateMap<EmployeeCreateViewModel, Employee>();
            Mapper.CreateMap<Employee, EmployeeEditViewModel>();
            Mapper.CreateMap<Contact, CreateContactViewModel>();
            Mapper.CreateMap<CubicleLocation, CubicleLocationCreateViewModel>();
            Mapper.CreateMap<Contact, UpdateContactViewModel>();
            Mapper.CreateMap<CubicleLocation, UpdateCubicleLocationViewModel>();
            Mapper.CreateMap<CubicleLocation, CubicleLocationViewModel>();
            Mapper.CreateMap<Contact, ContactViewModel>();
            // Example for member to member mapping
            //Mapper.CreateMap<OrganizationUser, OrganizationUserDataAccessViewModel>()
            //    .ForMember(o => o.Name, b => b.MapFrom(z => z.FirstName + " " + z.LastName));
            //Mapper.CreateMap<Order, OrderViewModel>()
            //    .ForMember(o => o.OrderDescription, b => b.MapFrom(z => z.Description))
            //                .ForMember(o => o.OrderId, b => b.MapFrom(z =>
            //                z.Id));
        }
    }
}