using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Silicus.Ensure.Models;
using Silicus.Ensure.Web.Models;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Models.Constants;
using System;
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
            Mapper.CreateMap<UserViewModel, User>();
            Mapper.CreateMap<User,UserViewModel>();
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
        }
    }
}