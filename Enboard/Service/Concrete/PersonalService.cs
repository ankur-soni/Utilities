using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using Data;

namespace Service
{
   public class PersonalService:IPersonalService
    {
        private IPersonalRepository _personalRepository;

        public PersonalService(IPersonalRepository PersonalRepository)
        {
            this._personalRepository = PersonalRepository;
        }


        public IEnumerable<Data.EmployeePersonalDetail> GetAll(Data.EmployeePersonalDetail obj, string[] param, string spName)
        {
            return _personalRepository.GetAll(obj, param, spName);
        }

        public bool Insert(Data.EmployeePersonalDetail obj, string[] param, string spName)
        {
            return _personalRepository.Insert(obj, param, spName);
        }

        public Data.EmployeePersonalDetail GetById(object Id)
        {
           return _personalRepository.GetById(Id);
        }

        public EmployeePersonalDetail GetPersonalDetailsByUserId(int userId)
        {
            EmployeePersonalDetail personalDetails = new EmployeePersonalDetail();
            using (IPDEntities ctx = new IPDEntities())
            {
                personalDetails = ctx.EmployeePersonalDetails.Where(m => m.UserID == userId).FirstOrDefault();
            }
            return personalDetails;
        }

        public bool Update(Data.EmployeePersonalDetail obj, string[] param, string spName)
        {
            return _personalRepository.Update(obj, param, spName);
        }

        public bool Save()
        {
            return _personalRepository.Save();
        }


        public bool UpdateById(object Id)
        {
            throw new NotImplementedException();
        }
    }
}
