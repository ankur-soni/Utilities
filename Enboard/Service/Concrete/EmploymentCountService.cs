using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using Data;

namespace Service.Concrete
{
    public class EmploymentCountService : IEmploymentCountService
    {
        private IEmploymentCountRepository _employmentCountRepository;

        public EmploymentCountService(IEmploymentCountRepository employmentCountRepository)
        {
            this._employmentCountRepository = employmentCountRepository;
        }

        public Data.EmploymentCount GetEmploymentCountByUserId(int userId)
        {
            EmploymentCount employmentCount = new EmploymentCount();
            using (IPDEntities ctx = new IPDEntities())
            {
                employmentCount = ctx.EmploymentCounts.Where(m => m.UserID == userId).FirstOrDefault();
            }
            return employmentCount;
        }

        public IEnumerable<Data.EmploymentCount> GetAll(Data.EmploymentCount obj, string[] param, string spName)
        {
            return _employmentCountRepository.GetAll(obj, param, spName);
        }

        public bool Insert(Data.EmploymentCount obj, string[] param, string spName)
        {
            return _employmentCountRepository.Insert(obj, param, spName);
        }

        public bool Update(Data.EmploymentCount obj, string[] param, string spName)
        {
            return _employmentCountRepository.Update(obj, param, spName);
        }

        public bool Save()
        {
            return _employmentCountRepository.Save();
        }

        public bool UpdateById(object Id)
        {
            throw new NotImplementedException();
        }

        public Data.EmploymentCount GetById(object Id)
        {
            throw new NotImplementedException();
        }
    }
}
