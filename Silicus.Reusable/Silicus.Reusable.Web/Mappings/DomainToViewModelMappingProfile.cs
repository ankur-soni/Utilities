using AutoMapper;
using System;

namespace Silicus.Reusable.Web.Mappings
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return " DomainToViewModelMapping"; }
        }

        [Obsolete]
        protected override void Configure()
        {           
        }
    }
}