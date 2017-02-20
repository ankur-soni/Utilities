using AutoMapper;
using Silicus.Encourage.Models;
using Silicus.EncourageWithAzureAd.Web.Models;
using System.Diagnostics.CodeAnalysis;

namespace Silicus.EncourageWithAzureAd.Web.Mapping
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
#pragma warning disable CS0618 // Type or member is obsolete
            Mapper.CreateMap<NominationViewModel, Nomination>();
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }
}