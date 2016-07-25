using Silicus.Encourage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Encourage.Services.Interface
{
    public interface IResultService
    {
        void ShortlistNomination(int nominationId);
        void UnShortlistNomination(int nominationId);
        void SelectWinner(int nominationId,string winningComment);
        int IsShortlistedOrWinner(int nominationId);
        string GetAwardComments(int Id);
    }
}
