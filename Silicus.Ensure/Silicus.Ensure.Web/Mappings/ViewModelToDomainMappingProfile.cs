using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Silicus.Ensure.Models;
using Silicus.Ensure.Web.Models;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Models.Constants;
using System;
using Silicus.Ensure.Models.Test;
using Silicus.Ensure.Web.Models.Test;
using Newtonsoft.Json;
using Silicus.Ensure.Web.Models.ReviewQuestion;
using Silicus.Ensure.Models.ReviewQuestion;
using Silicus.Ensure.Models.JobVite;
using Silicus.Ensure.Web.Models.JobVite;

namespace Silicus.Ensure.Web.Mappings
{
    [ExcludeFromCodeCoverage]
    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToDomainMappingProfile"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<Model, EntityA>();
            Mapper.CreateMap<UserViewModel, UserBusinessModel>()
                 .ForMember(dest => dest.Technology, opt => opt.MapFrom(s => s.SkillTags != null ? string.Join(",", s.SkillTags.ToArray()) : ""));
            Mapper.CreateMap<TestSuiteViewModel, TestSuite>();//.ForMember(o => o., b => b.MapFrom(z => z.FirstName + " " + z.LastName));
            Mapper.CreateMap<TestSuite, TestSuiteViewModel>();//.ForMember(o => o.PositionName, Enum.Parse(Competency,"1").ToString();
            Mapper.CreateMap<QuestionModel, Question>();
            Mapper.CreateMap<Question, QuestionModel>();
            Mapper.CreateMap<TestSuiteQuestionModel, Question>();
            Mapper.CreateMap<Question, TestSuiteQuestionModel>();
            Mapper.CreateMap<TestSuiteCandidateModel, UserTestSuite>();
            Mapper.CreateMap<UserTestSuite, TestSuiteCandidateModel>();
            Mapper.CreateMap<TestSuiteQuestionModel, object>();
            Mapper.CreateMap<object, TestSuiteQuestionModel>();
            Mapper.CreateMap<PanelMemberDetailViewModel, PanelMemberDetail>()
                .ForMember(dest => dest.PanelIds, opt => opt.MapFrom(s => (String.Join(",", s.Panel))));
            Mapper.CreateMap<PanelMemberDetailViewModel, UserDetailViewModel>();

            Mapper.CreateMap<RecruiterMemberDetailViewModel, RecruiterMembersDetail>()
                .ForMember(dest => dest.TagIds, opt => opt.MapFrom(s => (String.Join(",", s.Tag))));
            Mapper.CreateMap<RecruiterMemberDetailViewModel, UserDetailViewModel>();

            Mapper.CreateMap<ContainerUserViewModel, Silicus.UtilityContainer.Models.DataObjects.User>();
            Mapper.CreateMap<UserDetailViewModel, Silicus.UtilityContainer.Models.DataObjects.User>()
                .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(s => (s.Email)))
                .ForMember(dest => dest.ID, opt => opt.MapFrom(s => (s.UserId)))
                .ForMember(dest => dest.PrimaryRoleID, opt => opt.MapFrom(s => (s.RoleId)));
            Mapper.CreateMap<TestDetailsViewModel, TestDetailsBusinessModel>();
            Mapper.CreateMap<ReviewerQuestionViewModel, TestDetailsBusinessModel>();
            Mapper.CreateMap<TestSummaryBusinessModel, TestSummaryViewModel>();

            Mapper.CreateMap<CandidateInfoViewModel, CandidateInfoBusinessModel>();
            Mapper.CreateMap<CandidateHistoryViewModel, UserBusinessModel>();
            Mapper.CreateMap<TechnologyViewModel, TechnologyBusinessModel>();
            Mapper.CreateMap<ReviewQuestionViewModel, ReviewQuestionBusinessModel>();
            Mapper.CreateMap<TabSelectionViewModel, TabSelectionBusinessModel>();
            Mapper.CreateMap<JobViteCandidateViewModel, JobViteCandidateBusinessModel>();
            Mapper.CreateMap<RequisitionViewModel, RequisitionBusinessModel>();
            Mapper.CreateMap<AssignTestViewModel, AssignTestBusinessModel>();
        }
    }
}