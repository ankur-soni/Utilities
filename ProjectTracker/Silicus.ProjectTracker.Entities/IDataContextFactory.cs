namespace Silicus.ProjectTracker.Entities
{
    public interface IDataContextFactory
    {
        IDataContext Create(ConnectionType connetionType);
    }
}