using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Silicus.ProjectTracker.Core.Interfaces;

namespace Silicus.ProjectTracker.Web.Mappings
{
    public class MappingService : IMappingService
    {
        [ExcludeFromCodeCoverage]
        public TDest Map<TSrc, TDest>(TSrc source) where TDest : class
        {
            return Mapper.Map<TSrc, TDest>(source);
        }
    }
}