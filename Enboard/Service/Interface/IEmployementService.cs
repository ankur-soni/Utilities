using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Data;

namespace Service
{
    public interface IEmployementService : IService<EmploymentDetail>
    {
        List<EmploymentDetail> GetEmploymnetByUser(int UserID);

        bool DeleteEmploymnetDetail(int ID, string userName);

        string GetLatestEmploymentNo( int userId);

        bool GetCurrentEmploymentStatus(int userId);

        bool CheckSelectedEmploymentStatus(int userId, int employmentDetID);

        List<Master_Currency> GetCurrencies();
    }
}
