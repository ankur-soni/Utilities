using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Data;

namespace Service
{
   public interface IProfessionalDetailsService:IService<EmployeeProfessionalDetail>
    {

       List<GetProffesionalDetails_Result> GetProffesionalDetails();
       List<Master_Department> GetDepartMentList();
       List<Master_Designation> GetDesignationList();
      //  bool UpdateDesignationList(string designationName);
       bool UpdateProfessionalDetails(EmployeeMaster empobj, EmployeeProfessionalDetail profobj);
    }
}
