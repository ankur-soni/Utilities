using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Repository;

namespace Service
{
  public class UniversityService:IUniversityService
    {
        private IUniversityRepository _IUniversityRepository;

        public UniversityService(IUniversityRepository IUniversityRepository)
        {
            this._IUniversityRepository = IUniversityRepository;
        }
        
        public IEnumerable<Master_University> GetAll(Master_University obj, string[] param, string spName)
        {
            return _IUniversityRepository.GetAll(obj, param, spName);
        }

        public bool Insert(Master_University obj, string[] param, string spName)
        {
            throw new NotImplementedException();
        }

        public Master_University GetById(object Id)
        {
            throw new NotImplementedException();
        }

        public bool Update(Master_University obj, string[] param, string spName)
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
