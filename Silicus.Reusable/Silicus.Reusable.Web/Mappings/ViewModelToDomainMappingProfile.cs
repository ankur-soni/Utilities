using AutoMapper;
using AutoMapper.Mappers;
using Silicus.FrameworxProject.Models;
using Silicus.Reusable.Web.Models;
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
            Mapper.Initialize(cfg => {                
                cfg.CreateMap<FrameworxViewModel, Frameworx>();
            });
        }
    }
}