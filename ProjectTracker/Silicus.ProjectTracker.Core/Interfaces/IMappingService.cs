namespace Silicus.ProjectTracker.Core.Interfaces
{
    public interface IMappingService
    {
        TDest Map<TSrc, TDest>(TSrc source) where TDest : class;
    }
}