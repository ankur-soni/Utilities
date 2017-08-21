using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Repository;

namespace Service
{
   public class EmployeeService:IEmployeeService
    {
        private IEmployeeRepository _IEmployeeRepository;

        public EmployeeService(IEmployeeRepository IEmployeeRepository)
        {
            this._IEmployeeRepository = IEmployeeRepository;
        }

        public IEnumerable<EmployeeMaster> GetAll(EmployeeMaster obj, string[] param, string spName)
        {
            return _IEmployeeRepository.GetAll(obj, param, spName);
        }

        public bool Insert(EmployeeMaster obj, string[] param, string spName)
        {
            throw new NotImplementedException();
        }

        public EmployeeMaster GetById(object Id)
        {
           return _IEmployeeRepository.GetById(Id);
        }

        public bool Update(EmployeeMaster obj, string[] param, string spName)
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
