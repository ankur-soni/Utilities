using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Repository;

namespace Service
{
  public  class RoleService :IRoleService
    {
        private IRoleRepository _IRoleRepository;

        public RoleService(IRoleRepository IRoleRepository)
        {
            this._IRoleRepository = IRoleRepository;
        }

        public IEnumerable<Master_Role> GetAll(Master_Role obj, string[] param, string spName)
        {
            return _IRoleRepository.GetAll(obj, param, spName);
        }

        public bool Insert(Master_Role obj, string[] param, string spName)
        {
            throw new NotImplementedException();
        }

        public Master_Role GetById(object Id)
        {
            return _IRoleRepository.GetById(Id);
        }

        public bool Update(Master_Role obj, string[] param, string spName)
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
