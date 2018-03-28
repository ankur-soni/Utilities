using System.Collections.Generic;

namespace Service
{
    public interface IService<T>
    {
        IEnumerable<T> GetAll(T obj, string[] param, string spName);

        bool Insert(T obj, string[] param, string spName);


        T GetById(object Id);
        bool Update(T obj, string[] param, string spName);

        bool UpdateById(object Id);
        bool Save();

    }
}
