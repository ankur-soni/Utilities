using AutoMapper;
using AutoMapper.Mappers;
using Silicus.FrameworxProject.Models;
using Silicus.Reusable.Web.Models;
using Silicus.Reusable.Web.Models.ViewModel;
using System;

namespace Silicus.Reusable.Web.Mappings
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return " ViewModelToDomainMapping"; }
        }

        [Obsolete]
        protected override void Configure()
        {
            CreateMap<FrameworxFeedbackViewModel, FrameworxFeedback>();
        }
    }
}