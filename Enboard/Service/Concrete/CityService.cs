using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Repository;

namespace Service
{
   public class CityService :ICityService
    {
       private ICityRepository _ICityRepository;

       public CityService(ICityRepository ICityRepository)
       {
          this._ICityRepository = ICityRepository;
       }

        public IEnumerable<Master_City> GetAll(Master_City obj, string[] param, string spName)
        {
           return _ICityRepository.GetAll(obj, param, spName);
        }

        public bool Insert(Master_City obj, string[] param, string spName)
        {
            throw new NotImplementedException();
        }

        public Master_City GetById(object Id)
        {
            throw new NotImplementedException();
        }

        public bool Update(Master_City obj, string[] param, string spName)
        {
            throw new NotImplementedException();
        }

        public bool UpdateById(object Id)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }
    }
}
