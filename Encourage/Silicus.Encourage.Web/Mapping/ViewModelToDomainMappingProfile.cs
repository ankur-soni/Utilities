using AutoMapper;
using Silicus.Encourage.Models;
using Silicus.Encourage.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;

namespace Silicus.Encourage.Web.Mapping
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
            Mapper.CreateMap<NominationViewModel, Nomination>();
        }
    }
}