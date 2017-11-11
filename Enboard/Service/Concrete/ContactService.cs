using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Repository;

namespace Service
{
   public class ContactService:IContactService
    {
       private IContactRepository _IContactRepository;

       public ContactService(IContactRepository IContactRepository)
       {
           this._IContactRepository = IContactRepository;
       }

        public IEnumerable<EmployeeContactDetail> GetAll(EmployeeContactDetail obj, string[] param, string spName)
        {
            return _IContactRepository.GetAll(obj, param, spName);
        }

        public bool Insert(EmployeeContactDetail obj, string[] param, string spName)
        {
            return _IContactRepository.Insert(obj, param, spName);
        }

        public EmployeeContactDetail GetById(object Id)
        {
            return _IContactRepository.GetById(Id);
        }

        public bool Update(EmployeeContactDetail obj, string[] param, string spName)
        {
            return _IContactRepository.Update(obj, param, spName);
        }

        public bool UpdateById(object Id)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            return _IContactRepository.Save();
        }
    }
}
