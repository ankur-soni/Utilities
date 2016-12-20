using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Silicus.Reusable.Web.Mappings;

namespace Silicus.Reusable.Web
{
    [ExcludeFromCodeCoverage]
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<DomainToViewModelMappingProfile>();
                x.AddProfile<ViewModelToDomainMappingProfile>();
            });
        }
    }
}