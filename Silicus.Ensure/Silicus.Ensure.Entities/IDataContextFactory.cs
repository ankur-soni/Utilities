namespace Silicus.Ensure.Entities
{
    public interface IDataContextFactory
    {
        IDataContext Create(ConnectionType connetionType);
    }
}