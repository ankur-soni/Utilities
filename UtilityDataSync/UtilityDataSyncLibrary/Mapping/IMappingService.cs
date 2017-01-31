namespace UtilityDataSyncLibrary.Mapping
{
    public interface IMappingService
    {
        TDest Map<TSrc, TDest>(TSrc source) where TDest : class;
        void Map<TSrc, TDest>(TSrc source, TDest destination);
    }
}
