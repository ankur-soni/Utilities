using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Repository;

namespace Service
{
  public  class EmpSkillsService :IEmpSkillsService
    {
      private IEmpSkillsRepository _IEmpSkillsRepository;

      public EmpSkillsService(IEmpSkillsRepository IEmpSkillsRepository)
      {
          this._IEmpSkillsRepository = IEmpSkillsRepository;
      }

        public IEnumerable<EmployeeSkillDetail> GetAll(EmployeeSkillDetail obj, string[] param, string spName)
        {
            return _IEmpSkillsRepository.GetAll(obj, param, spName);
        }

        public bool Insert(EmployeeSkillDetail obj, string[] param, string spName)
        {
            return _IEmpSkillsRepository.Insert(obj,param,spName);
        }

        public EmployeeSkillDetail GetById(object Id)
        {
            return _IEmpSkillsRepository.GetById(Id);
        }

        public bool Update(EmployeeSkillDetail obj, string[] param, string spName)
        {
            return _IEmpSkillsRepository.Update(obj, param, spName);
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
