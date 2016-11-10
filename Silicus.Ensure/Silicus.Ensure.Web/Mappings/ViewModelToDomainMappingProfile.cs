﻿using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Silicus.Ensure.Models;
using Silicus.Ensure.Web.Models;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Models.Constants;
using System;
namespace Silicus.Ensure.Web.Mappings
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
            Mapper.CreateMap<Model, EntityA>();
            Mapper.CreateMap<UserViewModel, User>();            
            Mapper.CreateMap<TestSuiteViewModel, TestSuite>();          
        }
    }
}