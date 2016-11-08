using Silicus.Ensure.Entities;
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
            return _context.Query<Question>();
        }

        public int Add(Question Question)
        {
            _context.Add(Question);
            return Question.Id;
        }

        public void Update(Question Question)
        {       
                _context.Update(Question);          
        }

        public void Delete(Question Question)
        {         
                _context.Delete(Question);         
        }
    }
}
