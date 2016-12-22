using System.Collections.Generic;
using System.Linq;
using Silicus.Ensure.Entities;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Services.Interfaces;
using Silicus.Ensure.Models.Constants;

namespace Silicus.Ensure.Services
{
    public class UserService : IUserService
    {
        private readonly IDataContext _context;

        public UserService(IDataContextFactory dataContextFactory)
        {
            _context = dataContextFactory.Create(ConnectionType.Ip);
        }

        public IEnumerable<User> GetUserDetailsAll()
        {
            return _context.Query<User>();
        }

        public IEnumerable<User> GetUserDetails()
        {
            return _context.Query<User>().Where(x => x.IsDeleted == false);
        }

        public int Add(User User)
        {
            _context.Add(User);
            return User.UserId;
        }

        public void Update(User User)
        {
            if (User.Role != null && User.Role.ToLower() == RoleName.Candidate.ToString().ToLower())
            {
                if (User.FirstName != null && User.Address != null && User.LastName != null)
                {
                    _context.Update(User);
                }
            }
            else if (User.Role != null && (User.Role.ToLower() == RoleName.Admin.ToString().ToLower() || User.Role.ToLower() == RoleName.Panel.ToString().ToLower()))
            {
                _context.Update(User);
            }
        }

        public void Delete(User User)
        {
            if (User.FirstName != null && User.LastName != null && User.Role != null)
            {
                _context.Delete(User);
            }
        }

        public User GetUserById(int userId)
        {
            return _context.Query<User>().FirstOrDefault(x => x.UserId == userId && x.IsDeleted == false);
        }

        public User GetUserByEmail(string email)
        {
            return _context.Query<User>().FirstOrDefault(x => x.Email == email && x.IsDeleted == false);
        }

        public IEnumerable<User> GetUserByRole(string role)
        {
            return _context.Query<User>().Where(x => x.Role == role && x.IsDeleted == false);
        }

        public dynamic GetTestSuiteDetailsOfUser(int? userId)
        {
            var result = (from ts in _context.Query<UserTestSuite>().Where(x => x.UserId == userId)
                          join t in _context.Query<TestSuite>() on ts.TestSuiteId equals t.TestSuiteId
                          select new
                          {
                              t.TestSuiteName,
                              ts.UserTestSuiteId,
                              ts.ObjectiveCount,
                              ts.PracticalCount,
                              ts.MaxScore,
                              ts.Duration
                          }).FirstOrDefault();
            return result;
        }

        public dynamic GetTestSuiteDetailsWithQuestions(int? userTestSuiteId)
        {
            var result = (from td in _context.Query<UserTestDetails>().Where(x => x.UserTestSuite.TestSuiteId == userTestSuiteId)
                          join q in _context.Query<Question>() on td.QuestionId equals q.Id
                          select new
                          {
                              q.Id,
                              q.QuestionDescription,
                              q.QuestionType,
                              q.OptionCount,
                              q.Option1,
                              q.Option2,
                              q.Option3,
                              q.Option4,
                              q.Option5,
                              q.Option6,
                              q.Option7,
                              q.Option8,
                              q.Marks,
                              q.CorrectAnswer,
                              q.Answer
                          }).ToList();
            return result;
        }
    }
}

