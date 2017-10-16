using Data;
using Repository;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Concrete
{
    public class FamilyDetailsService : IFamilyDetailsService
    {
        private IFamilyDetailsRepository _IFamilyDetailsRepository;

        public FamilyDetailsService(IFamilyDetailsRepository IFamilyDetailsRepository)
        {
            this._IFamilyDetailsRepository = IFamilyDetailsRepository;
        }

        public IEnumerable<EmployeeFamilyDetail> GetAll(EmployeeFamilyDetail obj, string[] param, string spName)
        {
            return _IFamilyDetailsRepository.GetAll(obj, param, spName);
        }

        public bool Insert(EmployeeFamilyDetail obj, string[] param, string spName)
        {
            return _IFamilyDetailsRepository.Insert(obj, param, spName);
        }

        public EmployeeFamilyDetail GetById(object Id)
        {
            return _IFamilyDetailsRepository.GetById(Id);
        }

        public bool Update(EmployeeFamilyDetail obj, string[] param, string spName)
        {
            return _IFamilyDetailsRepository.Update(obj, param, spName);
        }

        public bool UpdateById(object Id)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            return _IFamilyDetailsRepository.Save();
        }

        /// <summary>
        /// Get family details
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<EmployeeFamilyDetail> GetEmployeeFamilyDetailsByUserID(int UserID)
        {
            List<EmployeeFamilyDetail> data = null;

            using (IPDEntities ctx = new IPDEntities())
            {
                data = ctx.EmployeeFamilyDetails.Where(m => m.UserID == UserID && m.IsActive==true).ToList();

            }
            return data;
        }

        /// <summary>
        /// Soft delete family details
        /// </summary>
        /// <param name="FamDetID"></param>
        /// <returns></returns>
        public bool DeleteEmployeeFamilyDetails(long FamDetID, string userName)
        {
            bool result;

            using (IPDEntities ctx = new IPDEntities())
            {
                try
                {

                    EmployeeFamilyDetail familyDetails = ctx.EmployeeFamilyDetails.Where(m => m.FamDetID == FamDetID).FirstOrDefault();
                    familyDetails.IsActive = false;
                    familyDetails.UpdatedDate = DateTime.UtcNow;
                    familyDetails.UpdatedBy = userName;

                    ctx.SaveChanges();

                    result = true;
                }
                catch (Exception ex)
                {

                    result = false;
                }
            }
            return result;
        }

    }
}
