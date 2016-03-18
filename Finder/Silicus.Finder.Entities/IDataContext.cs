using System;
using System.Collections.Generic;
using System.Linq;

namespace Silicus.Finder.Entities
{
    //using Eda.RDBI.Models.DataObjects;

    /// <summary>
    /// This interface provides an abstraction for accessing a data source.
    /// </summary>
    public interface IDataContext : IDisposable
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
        /// The changes are committed to the data store once.
        /// </remarks>
        void AddAll<T>(IEnumerable<T> items) where T : class;

        /// <summary>
        /// Inserts a collection of data items to the data store.
        /// </summary>
        /// <remarks>
        /// The changes are committed to the data store once.
        /// </remarks>
        void BulkAddAll<T>(IEnumerable<T> items) where T : class;

        /// <summary>
        /// Update a data item in the data store.
        /// </summary>
        int Update<T>(T item) where T : class;

        int Attach<T>(T item) where T : class;
        /// <summary>
        /// Update a collection of data items to the data store.
        /// </summary>
        /// <remarks>
        /// The changes are committed to the data store once.
        /// </remarks>
        void UpdateAll<T>(IEnumerable<T> items) where T : class;

        /// <summary>
        /// Allow repositories to control when SaveChanges() is called
        /// </summary>
        int SaveChanges();

        void Delete<T>(T item) where T : class;

        void DeleteAll<T>(IEnumerable<T> items) where T : class;

        T TryAdd<T>(T state) where T : class;

        void CriteriaBasedSearch(string projectName, string projectCode);
    }
}