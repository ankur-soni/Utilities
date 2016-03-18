namespace Silicus.Finder.Entities
{
    public interface IDataContextFactory
    {
        IDataContext Create(ConnectionType connetionType);
    }
}