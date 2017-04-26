using Silicus.Ensure.Models;
using System.Collections.Generic;

namespace Silicus.Ensure.Services.Interfaces
{
    public interface ITechnologyService
    {
        IEnumerable<TechnologyBusinessModel> GetAllTechnologies();
        int? Add(TechnologyBusinessModel technology);
        int? Update(TechnologyBusinessModel technology);
        TechnologyBusinessModel GetTechnologyByName(string name);
        bool IsTechnologyAssosiatedWithQuetion(string technologyName);
        TechnologyBusinessModel GetTechnologyById(int technologyId);
        IDictionary<int, int> GetAllTechnologiesWithQuestionCount(int userId);
    }
}
