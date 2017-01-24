﻿//using Eda.RDBI.Web.ViewModel;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Silicus.Ensure.Web.Models;
using Silicus.Ensure.Models.DataObjects;
using System;
using Silicus.Ensure.Web.Application;
using Silicus.Ensure.Models.Test;
using Silicus.Ensure.Web.Models.Test;

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
            Mapper.CreateMap<User, UserViewModel>()
                .ForMember(dest => dest.ResumeDisplayName, opt => opt.MapFrom(s =>
                    !string.IsNullOrWhiteSpace(s.ResumeName) ?
                    (s.ResumeName.Contains(AppConstants.ResumeNameSeparationCharacter) && s.ResumeName.Length >= s.ResumeName.IndexOf(AppConstants.ResumeNameSeparationCharacter) + 1
                    ? s.ResumeName.Substring(s.ResumeName.IndexOf(AppConstants.ResumeNameSeparationCharacter) + 1) : "")
                    : "")
                    );
            Mapper.CreateMap<TestSuite, TestSuiteViewModel>();
            Mapper.CreateMap<Silicus.UtilityContainer.Models.DataObjects.User, ContainerUserViewModel>();
            Mapper.CreateMap<PanelMemberDetail, PanelMemberDetailViewModel>()
                .ForMember(dest => dest.PanelIds, opt => opt.MapFrom(s => (s.PanelIds.Split(','))));
            Mapper.CreateMap<UserDetailViewModel, PanelMemberDetailViewModel>();

            Mapper.CreateMap<RecruiterMembersDetail, RecruiterMemberDetailViewModel>()
                .ForMember(dest => dest.TagIds, opt => opt.MapFrom(s => (s.TagIds.Split(','))));
            Mapper.CreateMap<UserDetailViewModel, RecruiterMemberDetailViewModel>();

            Mapper.CreateMap<Silicus.UtilityContainer.Models.DataObjects.User, UserDetailViewModel>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(s => (s.EmailAddress)))
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(s => (s.PrimaryRoleID)))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(s => (s.ID)))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(s => (s.LastName + " " + s.FirstName)));

            Mapper.CreateMap<QuestionNavigationBusinessModel, QuestionNavigationViewModel>();
            Mapper.CreateMap<TestDetailsBusinessModel, TestDetailsViewModel>();
        }
    }
}