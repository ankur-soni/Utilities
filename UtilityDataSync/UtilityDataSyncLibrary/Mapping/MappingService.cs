using AutoMapper;

namespace UtilityDataSyncLibrary.Mapping
{
    public class MappingService : IMappingService
    {

        public TDest Map<TSrc, TDest>(TSrc source) where TDest : class
        {
            return Mapper.Map<TSrc, TDest>(source);
        }
        public void Map<TSrc, TDest>(TSrc source, TDest destination)
        {
            Mapper.Map(source, destination);
        }
    }
}

