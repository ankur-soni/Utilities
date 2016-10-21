using System.Diagnostics.CodeAnalysis;
using AutoMapper;

namespace Silicus.Ensure.Web.Mappings
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