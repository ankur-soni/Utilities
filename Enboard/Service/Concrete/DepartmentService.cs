using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;

using Data;

namespace Service
{
    public class DepartmentService : IDepartmentService
    {
        private IDepartmentService _departmentRepository;

        public DepartmentService(IDepartmentService DepartmentRepository)
        {
            this._departmentRepository = DepartmentRepository;
        }





        public bool Save()
        {
            throw new NotImplementedException();
        }



        public IEnumerable<Master_Department> GetAll(Master_Department obj, string[] param, string spName)
        {
            return _departmentRepository.GetAll(null, null, "");
        }


        public bool Insert(Master_Department obj, string[] param, string spName)
        {
            throw new NotImplementedException();
        }

        public Master_Department GetById(object Id)
        {

            throw new NotImplementedException();
        }

        public bool Update(Master_Department obj, string[] param, string spName)
        {
            throw new NotImplementedException();
        }


        public bool UpdateById(object Id)
        {
            throw new NotImplementedException();
        }
        
    }
}
