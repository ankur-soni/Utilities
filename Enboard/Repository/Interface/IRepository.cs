using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Repository
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll(T obj, string[] param, string spName);

        bool Insert(T obj, string[] param, string spName);


        T GetById(object Id);
        bool Update(T obj, string[] param, string spName);
        bool UpdateById(object Id);


        bool Update(T entity, Expression<Func<T, object>> property);
        bool Save();

    }
}
