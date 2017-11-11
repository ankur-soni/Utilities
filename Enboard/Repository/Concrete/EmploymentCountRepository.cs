using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Repository.Interface;

namespace Repository
{
    public class EmploymentCountRepository : RepositoryBase<EmploymentCount>, IEmploymentCountRepository
    {
    }
}
