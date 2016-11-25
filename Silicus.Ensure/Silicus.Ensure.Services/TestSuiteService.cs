using System.Collections.Generic;
using System.Linq;
using Silicus.Ensure.Entities;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Services.Interfaces;
using System;
using System.Configuration;
using Silicus.Ensure.Models.Constants;

namespace Silicus.Ensure.Services
{
    public class TestSuiteService : ITestSuiteService
    {
        private readonly IDataContext _context;

        public TestSuiteService(IDataContextFactory dataContextFactory)
        {
            _context = dataContextFactory.Create(ConnectionType.Ip);
        }

        public IEnumerable<TestSuite> GetTestSuiteDetails()
        {
            return _context.Query<TestSuite>();
        }

        public int Add(TestSuite TestSuite)
        {
            _context.Add(TestSuite);
            return TestSuite.TestSuiteId;
        }

        public void Update(TestSuite TestSuite)
        {
            if (TestSuite.TestSuiteName != null)
            {                
                _context.Update(TestSuite);
            }
        }

        public void Delete(TestSuite TestSuite)
        {
            if (TestSuite.TestSuiteName != null)
            {
                TestSuite.IsDeleted = true;
                TestSuite.ModifiedDate = System.DateTime.UtcNow;
                _context.Update(TestSuite);
            }
        }

        public IEnumerable<UserTestSuite> GetUserTestSuite()
        {
            return _context.Query<UserTestSuite>();
        }

        public int AddUserTestSuite(UserTestSuite UserTestSuite)
        {
            _context.Add(UserTestSuite);
            return UserTestSuite.UserTestSuiteId;
        }

        public void UpdateUserTestSuite(UserTestSuite UserTestSuite)
        {
            if (UserTestSuite.UserId > 0)
            {                
                _context.Update(UserTestSuite);
            }
        }

        public void DeleteUserTestSuite(UserTestSuite UserTestSuite)
        {
            if (UserTestSuite.UserId > 0)
            {
                _context.Delete(UserTestSuite);
            }
        }

        public TestSuite GetTestSuitById(int testSuiteId)
        {
            return _context.Query<TestSuite>().Where(x => x.TestSuiteId == testSuiteId).FirstOrDefault();
        }

        public UserTestSuite GetUserTestSuiteByUserId(int userId)
        {
            return _context.Query<UserTestSuite>().Where(x => x.UserId == userId).FirstOrDefault();
        }

        public UserTestSuite GetUserTestSuiteId(int userTestSuiteId)
        {
            return _context.Query<UserTestSuite>().Where(x => x.UserTestSuiteId == userTestSuiteId).FirstOrDefault();
        }


        public void UpdateUserTestDetails(UserTestDetails UserTestDetails)
        {
            _context.Update(UserTestDetails);

        }

        public int AddUserTestDetails(UserTestDetails UserTestDetails)
        {
            _context.Add(UserTestDetails);
            return UserTestDetails.TestDetailId;
        }


        public UserTestDetails GetUserTestDetailsId(int? userTestDetailsId)
        {
            return _context.Query<UserTestDetails>().Where(x => x.TestDetailId == userTestDetailsId).FirstOrDefault();
        }

        public dynamic GetUserTestDetailsByUserTestSuitId(int? userTestSuitId)
        {
            var result = (from a in _context.Query<UserTestDetails>()
                              .Where(x => x.UserTestSuite.UserTestSuiteId == userTestSuitId)
                          join b in _context.Query<Question>()
                          on a.QuestionId equals b.Id
                          select new
                          {
                              a.TestDetailId,
                              a.Answer,
                              b.Id,
                              b.QuestionType,
                              b.AnswerType,
                              b.QuestionDescription,
                              b.Option1,
                              b.Option2,
                              b.Option3,
                              b.Option4,
                              b.Marks,
                          }).OrderBy(o => o.QuestionType).ToList();
            return result;
        }

        public int ActiveteSuite(UserTestSuite userTestSuite, TestSuite testSuite)
        {
            int optionalQuestions = Convert.ToInt32(ConfigurationManager.AppSettings["OptionalQuestion"]);
            int practicalQuestions = Convert.ToInt32(ConfigurationManager.AppSettings["PracticalQuestion"]);
            Random random = new Random();
            int index = 0, requiredMinutes = 0, minutes = 0, tryCount = 0, currentTagDuration = 0;
            UserTestDetails testSuiteDetail;
            List<TestSuiteTag> testSuiteTags;
            List<UserTestDetails> testSuiteDetails = new List<UserTestDetails>();
            List<Question> questions = new List<Question>();

            var questionBank = _context.Query<Question>().ToList();
            GetTestSuiteTags(testSuite, out testSuiteTags);

            foreach (var tag in testSuiteTags)
            {
                minutes = 0; currentTagDuration = 0; tryCount = 0;
                var questionList = questionBank.Where(x => x.Tags.Split(',').Contains(Convert.ToString(tag.TagId)) && !questions.Any(y => y.Id == x.Id)).ToList();
                if (questionList.Sum(x => x.Duration) >= tag.Minutes)
                {
                    //Optional Questions
                    var optionalQuestion = questionList.Where(x => !questions.Any(y => y.Id == x.Id) && x.QuestionType == 1 && x.Competency == testSuite.Competency);
                    requiredMinutes = tag.Minutes * Convert.ToInt32(optionalQuestions) / 100;
                    if (optionalQuestion.Sum(x => x.Duration) > requiredMinutes)
                    {
                        do
                        {
                            optionalQuestion = questionList.Where(x => !questions.Any(y => y.Id == x.Id) && x.QuestionType == 1 && x.Competency == testSuite.Competency);
                            index = random.Next(optionalQuestion.Count());
                            if (index != 0)
                            {
                                Question question = optionalQuestion.ElementAt(index);
                                questions.Add(question);
                                minutes += question.Duration;
                            }
                            else
                            {
                                tryCount += 1;
                                if (tryCount > 3)
                                {
                                    break;
                                }
                            }

                        } while (minutes <= requiredMinutes);
                    }
                    else
                    {
                        optionalQuestion = questionList.Where(x => !questions.Any(y => y.Id == x.Id) && x.QuestionType == 1 && x.Competency == testSuite.Competency);
                        foreach (var question in optionalQuestion)
                        {
                            questions.Add(question);
                            minutes += question.Duration;
                        }
                        //If other competency needs.
                        if (minutes < requiredMinutes)
                        {
                            do
                            {
                                optionalQuestion = questionList.Where(x => !questions.Any(y => y.Id == x.Id) && x.QuestionType == 1 && x.Competency != testSuite.Competency);
                                index = random.Next(optionalQuestion.Count());
                                if (index != 0)
                                {
                                    Question question = optionalQuestion.ElementAt(index);
                                    questions.Add(question);
                                    minutes += question.Duration;

                                    if (minutes >= requiredMinutes)
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    tryCount += 1;
                                    if (tryCount > 5)
                                    {
                                        break;
                                    }
                                }

                            } while (minutes <= requiredMinutes);
                        }
                    }

                    //Practical Questions
                    currentTagDuration += minutes;
                    requiredMinutes = tag.Minutes - minutes;
                    minutes = 0; tryCount = 0;
                    var practicalQuestion = questionList.Where(x => !questions.Any(y => y.Id == x.Id) && x.QuestionType == 2 && x.Competency == testSuite.Competency);
                    if (practicalQuestion.Sum(x => x.Duration) > requiredMinutes)
                    {
                        do
                        {
                            practicalQuestion = questionList.Where(x => !questions.Any(y => y.Id == x.Id) && x.QuestionType == 2 && x.Competency == testSuite.Competency);
                            index = random.Next(practicalQuestion.Count());
                            if (index != 0)
                            {
                                Question question = practicalQuestion.ElementAt(index);
                                questions.Add(question);
                                minutes += question.Duration;
                            }
                            else
                            {
                                tryCount += 1;
                                if (tryCount > 5)
                                {
                                    break;
                                }
                            }
                        } while (requiredMinutes >= minutes);
                    }
                    else
                    {
                        foreach (var question in practicalQuestion.Where(x => !questions.Any(y => y.Id == x.Id) && x.QuestionType == 2 && x.Competency == testSuite.Competency))
                        {
                            questions.Add(question);
                            minutes += question.Duration;
                        }
                        //If other competency needs.
                        if (minutes < requiredMinutes)
                        {
                            do
                            {
                                practicalQuestion = questionList.Where(x => !questions.Any(y => y.Id == x.Id) && x.QuestionType == 2 && x.Competency != testSuite.Competency);
                                index = random.Next(optionalQuestion.Count());
                                if (index != 0)
                                {
                                    Question question = optionalQuestion.ElementAt(index);
                                    questions.Add(question);
                                    minutes += question.Duration;
                                    if (minutes >= requiredMinutes)
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    tryCount += 1;
                                    if (tryCount > 5)
                                    {
                                        break;
                                    }
                                }
                            } while (minutes <= requiredMinutes);
                        }
                    }
                    currentTagDuration += minutes;
                    tryCount = 0;
                    //If duration not completed
                    if (currentTagDuration < tag.Minutes)
                    {
                        do
                        {
                            var allQuestions = questionList.Where(x => !questions.Any(y => y.Id == x.Id));
                            index = random.Next(allQuestions.Count());
                            if (index != 0)
                            {
                                Question question = allQuestions.ElementAt(index);
                                questions.Add(question);
                                minutes += question.Duration;
                            }
                            else
                            {
                                tryCount += 1;
                                if (tryCount > 5)
                                {
                                    break;
                                }
                            }
                        } while (minutes <= requiredMinutes);
                    }
                }
            }
            //Attach Questions
            foreach (var question in questions)
            {
                testSuiteDetail = new UserTestDetails();
                testSuiteDetail.QuestionId = question.Id;
                testSuiteDetails.Add(testSuiteDetail);
            }
            userTestSuite.UserTestDetails = testSuiteDetails;
            userTestSuite.ObjectiveCount = questions.Where(x => x.QuestionType == 1).Count();
            userTestSuite.PracticalCount = questions.Where(x => x.QuestionType == 2).Count();
            userTestSuite.MaxScore = questions.Sum(x => x.Marks);
            userTestSuite.StatusId = Convert.ToInt32(TestStatus.Assigned);
            return AddUserTestSuite(userTestSuite);
        }

        private void GetTestSuiteTags(TestSuite testSuite, out List<TestSuiteTag> testSuiteTags)
        {
            TestSuiteTag testSuiteTagViewModel;
            testSuiteTags = new List<TestSuiteTag>();
            var tagList = _context.Query<Tags>();
            string[] tags = testSuite.PrimaryTags.Split(',');
            string[] weights = testSuite.Weights.Split(',');

            for (int i = 0; i < tags.Length; i++)
            {
                testSuiteTagViewModel = new TestSuiteTag();
                testSuiteTagViewModel.TagId = Convert.ToInt32(tags[i]);               
                testSuiteTagViewModel.Weightage = Convert.ToInt32(weights[i]);
                testSuiteTagViewModel.Minutes = testSuite.Duration * Convert.ToInt32(weights[i]) / 100;
                testSuiteTags.Add(testSuiteTagViewModel);
            }
        }
    }
}
