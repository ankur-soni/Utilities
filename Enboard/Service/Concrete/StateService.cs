using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Repository;

namespace Service
{
    public class StateService : IStateService
    {
        private IStateRepository _IStateRepository;

        public StateService(IStateRepository IStateRepository)
        {
            this._IStateRepository = IStateRepository;
        }

        public IEnumerable<Master_State> GetAll(Master_State obj, string[] param, string spName)
        {
            return _IStateRepository.GetAll(obj, param, spName);
        }

        public bool Insert(Master_State obj, string[] param, string spName)
        {
            throw new NotImplementedException();
        }

        public Master_State GetById(object Id)
        {
            throw new NotImplementedException();
        }

        public bool Update(Master_State obj, string[] param, string spName)
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
