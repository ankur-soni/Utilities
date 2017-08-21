using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Repository;

namespace Service
{
  public class ClassService:IClassService
    {
      private IClassRepository _IClassRepository;

      public ClassService(IClassRepository IClassRepository)
      {
          this._IClassRepository = IClassRepository;
      }

        public IEnumerable<Master_Class> GetAll(Master_Class obj, string[] param, string spName)
        {
            return _IClassRepository.GetAll(obj, param, spName);
        }

        public bool Insert(Master_Class obj, string[] param, string spName)
        {
            throw new NotImplementedException();
        }

        public Master_Class GetById(object Id)
        {
            throw new NotImplementedException();
        }

        public bool Update(Master_Class obj, string[] param, string spName)
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
