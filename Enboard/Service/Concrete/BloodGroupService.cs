using Data;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Concrete
{
    public class BloodGroupService : IBloodGroupService
    {
        private IBloodGroupService _IBloodGroupService;

        public BloodGroupService(IBloodGroupService IBloodGroupRepository)
        {
            this._IBloodGroupService = IBloodGroupRepository;
        }

        public IEnumerable<Master_Bloodgroup> GetAll(Master_Bloodgroup obj, string[] param, string spName)
        {
            return _IBloodGroupService.GetAll(obj, param, spName);
        }

        public bool Insert(Master_Bloodgroup obj, string[] param, string spName)
        {
            throw new NotImplementedException();
        }

        public Master_Bloodgroup GetById(object Id)
        {
            throw new NotImplementedException();
        }

        public bool Update(Master_Bloodgroup obj, string[] param, string spName)
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
