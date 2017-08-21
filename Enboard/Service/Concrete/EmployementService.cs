using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Repository;


namespace Service
{
    public class EmployementService : IEmployementService
    {
        private IEmployementRepository _IEmployementRepository;

        public EmployementService(IEmployementRepository IEmployementRepository)
        {
            this._IEmployementRepository = IEmployementRepository;
        }

        public IEnumerable<EmploymentDetail> GetAll(EmploymentDetail obj, string[] param, string spName)
        {
            return _IEmployementRepository.GetAll(obj, param, spName);
        }

        public bool Insert(EmploymentDetail obj, string[] param, string spName)
        {
            return _IEmployementRepository.Insert(obj, param, spName);
        }

        public EmploymentDetail GetById(object Id)
        {
            return _IEmployementRepository.GetById(Id);
        }

        public bool Update(EmploymentDetail obj, string[] param, string spName)
        {
            return _IEmployementRepository.Update(obj, param, spName);
        }

        public bool UpdateById(object Id)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            return _IEmployementRepository.Save();
        }

        public List<EmploymentDetail> GetEmploymnetByUser(int UserID)
        {
            List<EmploymentDetail> data = null;

            using (IPDEntities ctx = new IPDEntities())
            {
                data = ctx.EmploymentDetails.Where(m => m.UserID == UserID && m.IsActive == true).ToList();

            }
            return data;
        }

        public bool DeleteEmploymnetDetail(int ID, string userName)
        {
            bool result;

            using (IPDEntities ctx = new IPDEntities())
            {
                try
                {

                    EmploymentDetail employmnetDetails = ctx.EmploymentDetails.Where(m => m.EmploymentDetID == ID).FirstOrDefault();
                    employmnetDetails.IsActive = false;
                    employmnetDetails.UpdatedDate = DateTime.Now;
                    employmnetDetails.UpdatedBy = userName;
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


        public string GetLatestEmploymentNo(int userId)
        {
            string result = string.Empty;

            using (IPDEntities ctx = new IPDEntities())
            {
                try
                {

                    List<EmploymentDetail> employmentDetails = ctx.EmploymentDetails.Where(m => m.UserID == userId).ToList();
                    if (employmentDetails != null && employmentDetails.Count > 0)
                    {
                        long MaxEmpId = employmentDetails.Max(m => m.EmploymentDetID);
                        string latestEmpNo = employmentDetails.Where(m => m.EmploymentDetID == MaxEmpId).Select(m => m.EmployementNo).FirstOrDefault();
                        if (!string.IsNullOrWhiteSpace(latestEmpNo))
                        {
                            int latestno = Convert.ToInt32(latestEmpNo.Remove(0, 13));
                            latestno = latestno + 1;
                            result = "EmployementNo" + latestno;
                        }
                    }
                    else
                    {
                        result = "EmployementNo1";
                    }
                }
                catch (Exception ex)
                {

                    result = string.Empty;
                }

            }
            return result;
        }

        public bool GetCurrentEmploymentStatus(int userId)
        {
            bool result;

            using (IPDEntities ctx = new IPDEntities())
            {
                try
                {

                    EmploymentDetail employmnetDetails = ctx.EmploymentDetails.Where(m => m.UserID == userId && m.IsActive == true && m.IsCurrentEmployment == true).FirstOrDefault();

                    if (employmnetDetails != null)
                        result = true;
                    else { result = false; }
                }
                catch (Exception ex)
                {

                    result = false;
                }

            }

            return result;

        }



        public bool CheckSelectedEmploymentStatus(int userId, int employmentDetID)
        {
            bool result;

            using (IPDEntities ctx = new IPDEntities())
            {
                try
                {

                    EmploymentDetail employmnetDetails = ctx.EmploymentDetails.Where(m => m.UserID == userId && m.IsActive == true && m.IsCurrentEmployment == true && m.EmploymentDetID == employmentDetID).FirstOrDefault();

                    if (employmnetDetails != null)
                        result = true;
                    else { result = false; }
                }
                catch (Exception ex)
                {

                    result = false;
                }

            }

            return result;

        }

        public List<Master_Currency> GetCurrencies()
        {
            List<Master_Currency> data = null;

            using (IPDEntities ctx = new IPDEntities())
            {
                data = ctx.Master_Currency.ToList();
            }
            return data;

        }
    }
}
