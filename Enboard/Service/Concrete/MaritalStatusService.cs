using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Repository;

namespace Service
{
   public class MaritalStatusService :IMaritalStatusService
    {
        private IMaritalStatusRepository _IMaritalStatusRepository;

        public MaritalStatusService(IMaritalStatusRepository IMaritalStatusRepository)
        {
            this._IMaritalStatusRepository = IMaritalStatusRepository;
        }

        public IEnumerable<Master_MaritalStatus> GetAll(Master_MaritalStatus obj, string[] param, string spName)
        {
            return _IMaritalStatusRepository.GetAll(obj, param, spName);
        }

        public bool Insert(Master_MaritalStatus obj, string[] param, string spName)
        {
            throw new NotImplementedException();
        }

        public Master_MaritalStatus GetById(object Id)
        {
            throw new NotImplementedException();
        }

        public bool Update(Master_MaritalStatus obj, string[] param, string spName)
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
