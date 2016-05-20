using Silicus.Encourage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Encourage.Services.Interface
{
    public interface INominationService
    {
        List<Nomination> GetAllNominations();
        List<Nomination> GetAllSubmitedNominations();
        List<Nomination> GetAllSavedNominations();
        Nomination GetNomination(int nominationId);
    }
}
