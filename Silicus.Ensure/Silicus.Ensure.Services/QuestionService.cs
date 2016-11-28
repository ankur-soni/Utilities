using Silicus.Ensure.Entities;
using Silicus.Ensure.Models.Constants;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Ensure.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IDataContext _context;

        public QuestionService(IDataContextFactory dataContextFactory)
        {
            _context = dataContextFactory.Create(ConnectionType.Ip);
        }

        public IEnumerable<Question> GetQuestion()
        {
            return _context.Query<Question>().Where(x => x.IsDeleted == false).OrderByDescending(x => x.Id);
        }

        public Question GetSingleQuestion(int id)
        {
            return _context.Query<Question>().Where(x => x.Id == id).FirstOrDefault();
        }

        public int Add(Question Question)
        {
            _context.Add(Question);
            return Question.Id;
        }

        public void Update(Question Question)
        {
            if (Question != null)
                _context.Update(Question);
        }

        public void Delete(int id)
        {
            if (id != 0)
            {
                Question Que = GetSingleQuestion(id);
                Que.IsDeleted = true;
                _context.Update(Que);
            }
        }

        public IList<string> GenerateQuestionList(string tag, long duration, Competency competency)
        {
            throw new NotImplementedException();
        }
    }
}
