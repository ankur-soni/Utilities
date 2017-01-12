using AutoMapper;
using System.Diagnostics.CodeAnalysis;

namespace Silicus.EncourageWithAzureAd.Web.Mapping
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