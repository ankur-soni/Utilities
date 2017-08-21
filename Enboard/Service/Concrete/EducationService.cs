using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Repository;
using System.Data.Entity;

namespace Service
{
    public class EducationService : IEducationService
    {
        private IEducationRepository _IEducationRepository;

        public EducationService(IEducationRepository _IEducationRepository)
        {
            this._IEducationRepository = _IEducationRepository;

        }

        public IEnumerable<EmployeeEducationDetail> GetAll(EmployeeEducationDetail obj, string[] param, string spName)
        {
            return _IEducationRepository.GetAll(obj, param, spName);
        }

        public bool Insert(EmployeeEducationDetail obj, string[] param, string spName)
        {
            return _IEducationRepository.Insert(obj, param, spName);
        }

        public EmployeeEducationDetail GetById(object Id)
        {
            return _IEducationRepository.GetById(Id);
        }

        public bool Update(EmployeeEducationDetail obj, string[] param, string spName)
        {
            return _IEducationRepository.Update(obj, param, spName);
        }

        public bool UpdateById(object Id)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            return _IEducationRepository.Save();
        }

        public List<GetEducationList_Result> GetEducationList(object UserID)
        {

            List<GetEducationList_Result> data = null;

            using (IPDEntities ctx = new IPDEntities())
            {
                data = ctx.GetEducationList((int)UserID).ToList();

            }
            return data;

        }

        public List<EmployeeEducationDetail> GetEmployeeEducationDetailsByUserID(int UserID)
        {
            List<EmployeeEducationDetail> data = null;

            using (IPDEntities ctx = new IPDEntities())
            {
                data = ctx.EmployeeEducationDetails.Where(m => m.UserID == UserID && m.IsActive == true).ToList();

            }
            return data;
        }

        public bool DeleteEducationDetail(int ID, string userName)
        {
            bool result;
            using (IPDEntities ctx = new IPDEntities())
            {
                try
                {

                    EmployeeEducationDetail educationdetail = ctx.EmployeeEducationDetails.Where(m => m.EduDetID == ID).FirstOrDefault();
                    educationdetail.IsActive = false;
                    educationdetail.UpdatedDate = DateTime.Now;
                    educationdetail.UpdatedBy = userName;
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

        public bool UpdateEmployeeEducation(EmployeeEducationDetail obj)
        {
            using (var context = new IPDEntities())
            {

                //db.SaveChanges();
                var local = context.EmployeeEducationDetails.Find(obj.EduDetID);
                context.Entry(local).CurrentValues.SetValues(obj);
                context.Entry(local).State = EntityState.Modified;
                bool status = Save();
                return status;
            }
        }


        public List<Master_EducationCategory> GetEducationcategoryListByUserId(int userId)
        {
            List<Master_EducationCategory> catlist = new List<Master_EducationCategory>();
            using (var context = new IPDEntities())
            {
                List<AdminEducationCategoryForUser> result = context.AdminEducationCategoryForUsers.Where(x => x.UserID == userId && x.IsActive == true).ToList();
                var master_EducationCategory = context.Master_EducationCategory;

                if (result.Any() && result.Count != 0 && master_EducationCategory.Any() && master_EducationCategory.Count() != 0)
                {
                    catlist = (from item in result
                               join eCat in master_EducationCategory
                                   on item.EducationCategoryId equals eCat.EducationCategoryID
                               select eCat).ToList();
                }
                if (context.EmployeeEducationDetails.Any() && context.EmployeeEducationDetails.Count() != 0)
                {
                    foreach (var item in context.EmployeeEducationDetails.Where(x => x.IsActive == true && x.UserID == userId).ToList())
                    {
                        catlist.Remove(catlist.ToList().FirstOrDefault(x => x.EducationCategoryID == item.EducationCategoryID));
                    }
                }
                return catlist;
            }
        }
    }
}
