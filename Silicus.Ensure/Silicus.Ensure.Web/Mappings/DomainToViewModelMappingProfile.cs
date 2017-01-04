//using Eda.RDBI.Web.ViewModel;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Silicus.Ensure.Web.Models;
using Silicus.Ensure.Models.DataObjects;
using System;

namespace Silicus.Ensure.Web.Mappings
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
            Mapper.CreateMap<User, UserViewModel>();
            Mapper.CreateMap<TestSuite, TestSuiteViewModel>();
            Mapper.CreateMap<Silicus.UtilityContainer.Models.DataObjects.User, ContainerUserViewModel>();
            Mapper.CreateMap<PanelMemberDetail, PanelMemberDetailViewModel>()
                .ForMember(dest => dest.PanelIds, opt => opt.MapFrom(s => (s.PanelIds.Split(','))));
            Mapper.CreateMap<UserDetailViewModel, PanelMemberDetailViewModel>();
            Mapper.CreateMap<Silicus.UtilityContainer.Models.DataObjects.User, UserDetailViewModel>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(s => (s.EmailAddress)))
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(s => (s.PrimaryRoleID)))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(s => (s.ID)))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(s => (s.LastName+" "+ s.FirstName)));
        }
    }
}