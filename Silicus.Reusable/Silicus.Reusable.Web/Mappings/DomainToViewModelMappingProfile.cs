using AutoMapper;
using Silicus.FrameworxProject.Models;
using Silicus.Reusable.Web.Models.ViewModel;
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
            CreateMap<ProductBacklog, ProductBacklogViewModel>();
        }
    }
}