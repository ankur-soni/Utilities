using Silicus.Ensure.Models.Constants;
using Silicus.Ensure.Models.DataObjects;
using System.Collections.Generic;

namespace Silicus.Ensure.Services.Interfaces
{
    public interface IQuestionService
    {
        IEnumerable<Question> GetQuestion();

        Question GetSingleQuestion(int id);

        int Add(Question question);

        void Update(Question question);

        void Delete(int id);

        IList<string> GenerateQuestionList(string tag, long duration, Proficiency competency);
    }
}
