using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Data;

namespace Service
{
    public interface IEducationService : IService<EmployeeEducationDetail>
    {
        List<GetEducationList_Result> GetEducationList(object UserID);

        bool DeleteEducationDetail(int ID, string userName);

        List<EmployeeEducationDetail> GetEmployeeEducationDetailsByUserID(int UserID);

        bool UpdateEmployeeEducation(EmployeeEducationDetail obj);
        List<Master_EducationCategory> GetEducationcategoryListByUserId(int userId);
    }
}
