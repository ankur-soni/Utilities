using System.Collections.Generic;
using System.Linq;

namespace Silicus.ProjectTracker.Logger
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

        /// <summary>
        /// Inserts a collection of data items to the data store.
        /// </summary>
        /// <remarks>
        /// The changes are commited to the data store once.
        /// </remarks>
        void AddAll<T>(IEnumerable<T> items) where T : class;

        /// <summary>
        /// Update a data item in the data store.
        /// </summary>
        void Update<T>(T item) where T : class;
    }
}
