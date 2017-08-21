using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ServiceBase<T>:IService<T>
    {
        public IEnumerable<T> GetAll(T obj, string[] param, string spName)
        {
            throw new NotImplementedException();
        }

        public T Get(T obj, string[] param, string spName)
        {
            throw new NotImplementedException();
        }

        public bool Insert(T obj, string[] param, string spName)
        {
            throw new NotImplementedException();
        }

        public void Delete(T obj, string[] param, string spName)
        {
            throw new NotImplementedException();
        }

        public void DeleteById(object Id)
        {
            throw new NotImplementedException();
        }

        public T GetById(object Id)
        {
            throw new NotImplementedException();
        }

        public bool Update(T obj, string[] param, string spName)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }


        public bool UpdateById(object Id)
        {
            throw new NotImplementedException();
        }
    }
}
