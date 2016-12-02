using Silicus.Ensure.Entities;
using Silicus.Ensure.Models.Constants;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

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
            return _context.Query<Question>().Where(x => x.Id == id).First();
        }

        public int Add(Question question)
        {
            _context.Add(question);
            return question.Id;
        }

        public void Update(Question question)
        {
            if (question != null)
                _context.Update(question);
        }

        public void Delete(int id)
        {
            if (id != 0)
            {
                Question que = GetSingleQuestion(id);
                que.IsDeleted = true;
                _context.Update(que);
            }
        }

        public IList<string> GenerateQuestionList(string tag, long duration, Proficiency competency)
        {
            throw new NotImplementedException();
        }
    }
}
