//using Eda.RDBI.Web.ViewModel;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Silicus.Ensure.Web.Models;
using Silicus.Ensure.Models.DataObjects;
using System;
using Silicus.Ensure.Web.Application;
using Silicus.Ensure.Models.Test;
using Silicus.Ensure.Web.Models.Test;
using Silicus.Ensure.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using Silicus.Ensure.Models.ReviewQuestion;
using Silicus.Ensure.Web.Models.ReviewQuestion;
using Silicus.Ensure.Models.JobVite;
using Silicus.Ensure.Web.Models.JobVite;
using Silicus.Ensure.Web.Models.Employee;

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
            Mapper.CreateMap<UserBusinessModel, UserViewModel>()
                .ForMember(dest => dest.ResumeDisplayName, opt => opt.MapFrom(s =>
                    !string.IsNullOrWhiteSpace(s.ResumeName) ?
                    (s.ResumeName.Contains(AppConstants.ResumeNameSeparationCharacter) && s.ResumeName.Length >= s.ResumeName.IndexOf(AppConstants.ResumeNameSeparationCharacter) + 1
                    ? s.ResumeName.Substring(s.ResumeName.IndexOf(AppConstants.ResumeNameSeparationCharacter) + 1) : "")
                    : "")
                    )
                    .ForMember(dest => dest.SkillTags, opt => opt.MapFrom(s => !string.IsNullOrWhiteSpace(s.Technology) ? s.Technology.Split(',') : null)
                    );
            Mapper.CreateMap<TestSuite, TestSuiteViewModel>();
            Mapper.CreateMap<EmployeeTestSuite, EmployeeTestSuitViewModel>();
            Mapper.CreateMap<Silicus.UtilityContainer.Models.DataObjects.User, ContainerUserViewModel>();
            Mapper.CreateMap<PanelMemberDetail, PanelMemberDetailViewModel>()
                .ForMember(dest => dest.Panel, opt => opt.MapFrom(s => (s.PanelIds.Split(','))));
            Mapper.CreateMap<UserDetailViewModel, PanelMemberDetailViewModel>();

            Mapper.CreateMap<RecruiterMembersDetail, RecruiterMemberDetailViewModel>()
                .ForMember(dest => dest.Tag, opt => opt.MapFrom(s => (s.TagIds.Split(','))));
            Mapper.CreateMap<UserDetailViewModel, RecruiterMemberDetailViewModel>();

            Mapper.CreateMap<Silicus.UtilityContainer.Models.DataObjects.User, UserDetailViewModel>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(s => (s.EmailAddress)))
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(s => (s.PrimaryRoleID)))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(s => (s.ID)))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(s => (s.LastName + " " + s.FirstName)));
            Mapper.CreateMap<EmployeeTestSuite, TestSuiteEmployeeModel>();
            Mapper.CreateMap<QuestionNavigationBusinessModel, QuestionNavigationViewModel>();
            Mapper.CreateMap<TestDetailsBusinessModel, TestDetailsViewModel>();
            Mapper.CreateMap<TestDetailsBusinessModel, ReviewerQuestionViewModel>();
            Mapper.CreateMap<TestSummaryBusinessModel, TestSummaryViewModel>();
            Mapper.CreateMap<UserBusinessModel, CandidateHistoryViewModel>().ForMember(dest => dest.ResumeDisplayName, opt => opt.MapFrom(s =>
                  !string.IsNullOrWhiteSpace(s.ResumeName) ?
                  (s.ResumeName.Contains(AppConstants.ResumeNameSeparationCharacter) && s.ResumeName.Length >= s.ResumeName.IndexOf(AppConstants.ResumeNameSeparationCharacter) + 1
                  ? s.ResumeName.Substring(s.ResumeName.IndexOf(AppConstants.ResumeNameSeparationCharacter) + 1) : "")
                  : "")
                    );
            Mapper.CreateMap<CandidateInfoBusinessModel, CandidateInfoViewModel>();
            Mapper.CreateMap<TechnologyBusinessModel, TechnologyViewModel>();
            Mapper.CreateMap<ReviewQuestionBusinessModel, ReviewQuestionViewModel>();
            Mapper.CreateMap<TabSelectionBusinessModel, TabSelectionViewModel>();
            Mapper.CreateMap<JobViteCandidateBusinessModel, JobViteCandidateViewModel>();
            Mapper.CreateMap<RequisitionBusinessModel,RequisitionViewModel>();
            Mapper.CreateMap<AssignTestViewModel, AssignTestBusinessModel>();
        }
    }
}