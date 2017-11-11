using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Repository;

namespace Service
{
  public class CollegeService:ICollegeService
    {
        private ICollegeRepository _ICollegeRepository;

        public CollegeService(ICollegeRepository ICollegeRepository)
        {
            this._ICollegeRepository = ICollegeRepository;
        }

        public IEnumerable<Master_College> GetAll(Master_College obj, string[] param, string spName)
        {
            return _ICollegeRepository.GetAll(obj, param, spName);
        }

        public bool Insert(Master_College obj, string[] param, string spName)
        {
            throw new NotImplementedException();
        }

        public Master_College GetById(object Id)
        {
            throw new NotImplementedException();
        }

        public bool Update(Master_College obj, string[] param, string spName)
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
