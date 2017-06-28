using Silicus.Ensure.Models.JobVite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Ensure.Services.Interfaces
{
    public interface IJobViteInteraction
    {
        IEnumerable<JobViteCandidateBusinessModel> GetCandidateList();
    }
}
