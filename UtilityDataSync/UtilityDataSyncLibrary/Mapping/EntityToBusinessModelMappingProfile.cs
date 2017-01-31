using System;
using AutoMapper;

namespace UtilityDataSyncLibrary.Mapping
{
    public class EntityToBusinessModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "EntityToBusinessModel"; }
        }

        protected override void Configure()
        {
            CreateMap<vwExt_Client, Client>();
            CreateMap<vwExt_Company, Company>();
            CreateMap<vwExt_Department, Department>();
            CreateMap<vwExt_Engagement, Engagement>();
            CreateMap<vwExt_EngagementRoles, EngagementRoles>();
            CreateMap<vwExt_EngagementTaskTypes, EngagementTaskTypes>();
            CreateMap<vwExt_EngagementType, EngagementType>();
            CreateMap<vwExt_Location, Location>();
            CreateMap<vwExt_Resource, Resource>();
            CreateMap<vwExt_ResourceHistory, ResourceHistory>();
            CreateMap<vwExt_ResourceSkillLevel, ResourceSkillLevel>();
            CreateMap<vwExt_Skill, Skill>();
            CreateMap<vwExt_Title, Title>();
            CreateMap<vwExt_User, User>();
            CreateMap<vwExt_ResourceType, ResourceType>();
        }

        public static object GetDefault(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }
    }
}
