using System.Diagnostics.CodeAnalysis;
using AutoMapper;

namespace UtilityDataSyncLibrary.Mapping
{
    [ExcludeFromCodeCoverage]
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
                {
                    x.AddProfile<EntityToBusinessModelMappingProfile>();
                });
        }
    }
}