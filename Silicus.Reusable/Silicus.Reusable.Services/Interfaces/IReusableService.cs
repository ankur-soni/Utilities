using Silicus.Reusable.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Silicus.Reusable.Services.Interfaces
{
   public interface IReusableService
    {
        List<Frameworx> GetAllFrameworks();
        Frameworx FrameworkDetail(int id);
    }
}
