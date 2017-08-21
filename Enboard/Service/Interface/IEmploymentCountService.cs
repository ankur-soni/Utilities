using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Data;

namespace Service
{
    public interface IEmploymentCountService : IService<EmploymentCount>
    {
        EmploymentCount GetEmploymentCountByUserId(int userId);
    }
}
