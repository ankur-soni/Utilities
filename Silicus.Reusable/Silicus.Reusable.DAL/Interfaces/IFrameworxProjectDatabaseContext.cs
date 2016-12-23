using System;
using System.Linq;

namespace Silicus.FrameworxProject.DAL.Interfaces
{
    public interface IFrameworxProjectDatabaseContext : IDisposable
    {
        T Add<T>(T item) where T : class;
        int Update<T>(T item) where T : class;
        IQueryable<T> Query<T>() where T : class;
        IQueryable<T> Query<T>(string property) where T : class;
        IQueryable<T> Query<T>(string property1, string property2) where T : class;
        IQueryable<T> Query<T>(string property1, string property2, string property3) where T : class;
        void Delete<T>(T item) where T : class;
        int SaveChanges();
    }
}
