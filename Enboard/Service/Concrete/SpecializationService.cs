using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Repository;

namespace Service
{
   public class SpecializationService:ISpecializationService
    {
        private ISpecializationRepository _ISpecializationRepository;

        public SpecializationService(ISpecializationRepository ISpecializationRepository)
        {
            this._ISpecializationRepository = ISpecializationRepository;
        }

        public IEnumerable<Master_Specialization> GetAll(Master_Specialization obj, string[] param, string spName)
        {
            return _ISpecializationRepository.GetAll(obj, param, spName);
        }

        public bool Insert(Master_Specialization obj, string[] param, string spName)
        {
            throw new NotImplementedException();
        }

        public Master_Specialization GetById(object Id)
        {
            throw new NotImplementedException();
        }

        public bool Update(Master_Specialization obj, string[] param, string spName)
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
