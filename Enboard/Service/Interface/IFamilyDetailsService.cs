using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IFamilyDetailsService : IService<EmployeeFamilyDetail>
    {
        List<EmployeeFamilyDetail> GetEmployeeFamilyDetailsByUserID(int UserID);
        bool DeleteEmployeeFamilyDetails(long FamDetID, string userName);
    }
}
