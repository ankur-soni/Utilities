using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Repository;

namespace Service
{
   public class DisciplineService:IDisciplineService
    {
        private IDisciplineRepository _IDisciplineRepository;

        public DisciplineService(IDisciplineRepository IDisciplineRepository)
        {
            this._IDisciplineRepository = IDisciplineRepository;
        }

        public IEnumerable<Master_Discipline> GetAll(Master_Discipline obj, string[] param, string spName)
        {
            return _IDisciplineRepository.GetAll(obj, param, spName);
        }

        public bool Insert(Master_Discipline obj, string[] param, string spName)
        {
            throw new NotImplementedException();
        }

        public Master_Discipline GetById(object Id)
        {
            throw new NotImplementedException();
        }

        public bool Update(Master_Discipline obj, string[] param, string spName)
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
