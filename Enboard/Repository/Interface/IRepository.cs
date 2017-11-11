using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
   public interface IRepository<T>
    {
       IEnumerable<T> GetAll(T obj, string[] param, string spName);

       bool Insert(T obj, string[] param, string spName);


       T GetById(object Id);
       bool Update(T obj, string[] param, string spName);
       bool UpdateById(object Id);
       bool Save();       
             
    }
}
