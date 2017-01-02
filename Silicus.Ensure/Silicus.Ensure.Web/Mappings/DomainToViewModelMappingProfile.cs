//using Eda.RDBI.Web.ViewModel;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Silicus.Ensure.Web.Models;
using Silicus.Ensure.Models.DataObjects;

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
        }
    }
}