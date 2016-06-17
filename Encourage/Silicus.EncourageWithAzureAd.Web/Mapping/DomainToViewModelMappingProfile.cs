using AutoMapper;
using Silicus.Encourage.Models;
using Silicus.EncourageWithAzureAd.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;

namespace Silicus.EncourageWithAzureAd.Web.Mapping
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
            Mapper.CreateMap<Nomination,NominationViewModel>();
        }
    }
}