using System.Linq;

namespace Silicus.ProjectTracker.Auditing
{
    /// <summary>
    /// This interface provides an abstraction for accessing a data source.
    /// </summary>
    public interface IDataContext
    {
        /// <summary>
        /// Returns a <see cref="IQueryable{T}"/> data for a given
        /// entity.
        /// </summary>
        IQueryable<T> Query<T>() where T : class;

        /// <summary>
        /// Inserts a given item to the data store.
        /// </summary>
        T Add<T>(T item) where T : class;
    }
}
