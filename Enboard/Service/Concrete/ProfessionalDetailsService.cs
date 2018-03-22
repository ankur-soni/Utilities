using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Repository;

namespace Service
{
   public class ProfessionalDetailsService:IProfessionalDetailsService
    {
       private IProfessionalDetailsRepository _IProfessionalDetailsRepository;

       public ProfessionalDetailsService(IProfessionalDetailsRepository IProfessionalDetailsRepository)
       {
           this._IProfessionalDetailsRepository = IProfessionalDetailsRepository;
       }
        public IEnumerable<EmployeeProfessionalDetail> GetAll(EmployeeProfessionalDetail obj, string[] param, string spName)
        {
            return _IProfessionalDetailsRepository.GetAll(obj, param, spName);
        }

        public bool Insert(EmployeeProfessionalDetail obj, string[] param, string spName)
        {
            return _IProfessionalDetailsRepository.Insert(obj, param, spName);
        }

        public EmployeeProfessionalDetail GetById(object Id)
        {
            throw new NotImplementedException();
        }

        public bool Update(EmployeeProfessionalDetail obj, string[] param, string spName)
        {
            return _IProfessionalDetailsRepository.Update(obj, param, spName);
        }

        public bool UpdateById(object Id)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        
        public List<GetProffesionalDetails_Result> GetProffesionalDetails()
        {
            List<GetProffesionalDetails_Result> data = null;

            using (IPDEntities ctx = new IPDEntities())
            {
                data = ctx.GetProffesionalDetails().ToList();

            }
            return data;
        }

        public List<Master_Designation> GetDesignationList()
        {
            List<Data.Master_Designation> data = null;

            using (IPDEntities ctx = new IPDEntities())
            {
                data = ctx.Master_Designation.Where(m=>m.IsActive==true).ToList();

            }
            return data;
        }

        public List<Master_Department> GetDepartMentList()
        {
            List<Data.Master_Department> data = null;

            using (IPDEntities ctx = new IPDEntities())
            {
                data = ctx.Master_Department.Where(m => m.IsActive == true).ToList();

            }
            return data;
        }

        public bool UpdateProfessionalDetails(EmployeeMaster empobj , EmployeeProfessionalDetail profobj)
        {           

            using (IPDEntities ctx = new IPDEntities())
            {

                var employeedata = ctx.EmployeeMasters.Where(m => m.EmpNo == empobj.EmpNo).FirstOrDefault();
                if (employeedata != null)
                {
                    employeedata.EmployeeName = empobj.EmployeeName;
                    employeedata.EmpNo = empobj.EmpNo;
                }
                var proffesionaldata = ctx.EmployeeProfessionalDetails.Where(m => m.EmpProfID == profobj.EmpProfID).FirstOrDefault();
                if (proffesionaldata != null)
                {
                    proffesionaldata.TotalExprInMonths = profobj.TotalExprInMonths;
                    proffesionaldata.TotalExprInYears = profobj.TotalExprInYears;
                    proffesionaldata.DesignationID = profobj.DesignationID;
                    proffesionaldata.DepartmentID = profobj.DepartmentID;
                }
                else
                {
                    EmployeeProfessionalDetail profInsertObj = new EmployeeProfessionalDetail();
                    if (profobj.UserID != 0) //dont save details if userid is not available
                    {
                        profInsertObj.TotalExprInMonths = profobj.TotalExprInMonths;
                        profInsertObj.TotalExprInYears = profobj.TotalExprInYears;
                        profInsertObj.DesignationID = profobj.DesignationID;
                        profInsertObj.DepartmentID = profobj.DepartmentID;
                        profInsertObj.UserID = profobj.UserID;
                        ctx.EmployeeProfessionalDetails.Add(profInsertObj);
                    }
                }
                ctx.SaveChanges();
            }
            return true;
        }
    }
}
