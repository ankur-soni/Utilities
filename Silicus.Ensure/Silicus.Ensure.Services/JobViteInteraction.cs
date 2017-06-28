using Silicus.Ensure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silicus.Ensure.Models.JobVite;

namespace Silicus.Ensure.Services
{
    public class JobViteInteraction : IJobViteInteraction
    {
        public IEnumerable<JobViteCandidateBusinessModel> GetCandidateList()
        {
            throw new NotImplementedException();
        }
    }
}
