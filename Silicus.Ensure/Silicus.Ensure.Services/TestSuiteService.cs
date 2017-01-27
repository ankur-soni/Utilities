using System.Collections.Generic;
using System.Linq;
using Silicus.Ensure.Entities;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Services.Interfaces;
using System;
using System.Configuration;
using Silicus.Ensure.Models.Constants;
using Silicus.Ensure.Models.Test;

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

        public TestSuite GetTestSuiteByName(string testSuiteName)
        {
            return _context.Query<TestSuite>().FirstOrDefault(y => y.TestSuiteName.Equals(testSuiteName, StringComparison.InvariantCultureIgnoreCase));
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

        public void UpdateUserTestDetails(UserTestDetails userTestDetails)
        {
            if (string.IsNullOrWhiteSpace(userTestDetails.Answer) || userTestDetails.Answer.Equals("undefined", StringComparison.OrdinalIgnoreCase))
            {
                userTestDetails.IsViewedOnly = true;
            }
            _context.Update(userTestDetails);

        }

        public int AddUserTestDetails(UserTestDetails UserTestDetails)
        {
            _context.Add(UserTestDetails);
            return UserTestDetails.TestDetailId;
        }

        public UserTestDetails GetUserTestDetailsId(int? userTestDetailsId)
        {
            return _context.Query<UserTestDetails>().Where(x => x.TestDetailId == userTestDetailsId).First();
        }

        public IEnumerable<UserTestDetails> GetUserTestDetailsListByUserTestSuitId(int userTestSuitId)
        {
            return _context.Query<UserTestDetails>().Where(x => x.UserTestSuite.UserTestSuiteId == userTestSuitId);
        }

        public int GetQuestionType(int questionId)
        {
            var question = _context.Query<Question>().FirstOrDefault(ques => ques.Id == questionId);
            if (question != null)
            {
                return question.QuestionType;
            }
            else
                return 0;
        }

        public bool IsAllQuestionEvaluated(int? userTestSuitId)
        {
            if (userTestSuitId != null)
            {
                var testDetails = _context.Query<UserTestDetails>().Where(x => x.UserTestSuite.UserTestSuiteId == userTestSuitId);
                var isAllQueustionReviewed = !testDetails.Any(y => y.Mark == null);
                return isAllQueustionReviewed;
            } return false;

        }

        public TestDetailsBusinessModel GetUserTestDetailsByUserTestSuitId(int? userTestSuitId, int? questionNumber, int questionType)
        {
            var questionNumberList = GetQuestionsByUserTestSuiteId(userTestSuitId, questionType);
            questionNumber = questionNumber == null && questionNumberList.Count > 0 ? (int?)questionNumberList.ElementAtOrDefault(0) : questionNumber;
            if (questionNumber == null)
            {
                return null;
            }
            var index = questionNumberList.IndexOf((int)questionNumber);
            int? previousQuestionId = questionNumberList.ElementAtOrDefault(index - 1);
            int? nextQuestionId = questionNumberList.ElementAtOrDefault(index + 1);
            var result = (from a in _context.Query<UserTestDetails>()
                              .Where(x => x.UserTestSuite.UserTestSuiteId == userTestSuitId)
                          join b in _context.Query<Question>()
                          on a.QuestionId equals b.Id
                          where b.Id == questionNumber
                          select new TestDetailsBusinessModel
                          {
                              TestDetailId = a.TestDetailId,
                              Answer = a.Answer,
                              ReviwerMark = a.Mark,
                              QuestionId = b.Id,
                              QuestionType = b.QuestionType,
                              AnswerType = b.AnswerType,
                              QuestionDescription = b.QuestionDescription,
                              OptionCount = b.OptionCount,
                              Option1 = b.Option1,
                              Option2 = b.Option2,
                              Option3 = b.Option3,
                              Option4 = b.Option4,
                              Option5 = b.Option5,
                              Option6 = b.Option6,
                              Option7 = b.Option7,
                              Option8 = b.Option8,
                              Marks = b.Marks
                          }).FirstOrDefault();
            if (result == null)
            {
                return null;
            }
            result.PreviousQuestionId = index <= 0 ? null : previousQuestionId;
            result.NextQuestionId = index >= questionNumberList.Count - 1 ? null : nextQuestionId;
            if (questionType == (int)QuestionType.Practical && result.NextQuestionId == null)
            {
                result = SetFirstObjectiveQuestionAsNextQuestion(result, userTestSuitId);
            }
            if (questionType == (int)QuestionType.Objective && result.PreviousQuestionId == null)
            {
                result = SetLastPracticalQuestionAsPreviousQuestion(result, userTestSuitId);
            }
            return result;
        }

        private TestDetailsBusinessModel SetFirstObjectiveQuestionAsNextQuestion(TestDetailsBusinessModel result, int? userTestSuitId)
        {
            var objectiveQuestions = GetQuestionsByUserTestSuiteId(userTestSuitId, (int)QuestionType.Objective);
            result.NextQuestionId = objectiveQuestions != null && objectiveQuestions.Count > 0 ? (int?)objectiveQuestions.ElementAtOrDefault(0) : null;
            return result;
        }
        private TestDetailsBusinessModel SetLastPracticalQuestionAsPreviousQuestion(TestDetailsBusinessModel result, int? userTestSuitId)
        {
            var practicalQuestions = GetQuestionsByUserTestSuiteId(userTestSuitId, (int)QuestionType.Practical);
            result.PreviousQuestionId = practicalQuestions != null && practicalQuestions.Count > 0 ? (int?)practicalQuestions.ElementAtOrDefault(practicalQuestions.Count - 1) : null;
            return result;
        }

        private List<int> GetQuestionsByUserTestSuiteId(int? userTestSuitId, int questionType)
        {
            return (from a in _context.Query<UserTestDetails>()
                                  .Where(x => x.UserTestSuite.UserTestSuiteId == userTestSuitId)
                    join b in _context.Query<Question>().Where(ques => ques.QuestionType == questionType)
                    on a.QuestionId equals b.Id
                    select
                    (
                        b.Id
                    )).OrderBy(questionId => questionId).ToList();
        }

        public int AssignSuite(UserTestSuite userTestSuite, TestSuite testSuite)
        {
            int optionalQuestions = Convert.ToInt32(testSuite.OptionalQuestion);
            int practicalQuestions = Convert.ToInt32(testSuite.PracticalQuestion);
            Random random = new Random();
            int index = 0, requiredMinutes = 0, minutes = 0;
            UserTestDetails testSuiteDetail;
            List<TestSuiteTag> testSuiteTags;
            List<UserTestDetails> testSuiteDetails = new List<UserTestDetails>();
            List<Question> questions = new List<Question>();

            var questionBank = _context.Query<Question>().ToList();
            GetTestSuiteTags(testSuite, out testSuiteTags);

            foreach (var tag in testSuiteTags)
            {
                minutes = 0;
                var questionList = questionBank.Where(x => x.Tags.Split(',').Contains(Convert.ToString(tag.TagId)) && !questions.Any(y => y.Id == x.Id)).ToList();
                if (questionList.Sum(x => x.Duration) >= tag.Minutes)
                {
                    //Optional Questions
                    var optionalQuestion = questionList.Where(x => !questions.Any(y => y.Id == x.Id) && x.QuestionType == 1 && x.ProficiencyLevel == tag.Proficiency);
                    requiredMinutes = tag.Minutes * Convert.ToInt32(optionalQuestions) / 100;
                    if (optionalQuestion.Sum(x => x.Duration) >= requiredMinutes)
                    {
                        do
                        {
                            optionalQuestion = questionList.Where(x => !questions.Any(y => y.Id == x.Id) && x.QuestionType == 1 && x.ProficiencyLevel == tag.Proficiency);
                            index = random.Next(optionalQuestion.Count());
                            Question question = optionalQuestion.ElementAtOrDefault(index);
                            questions.Add(question);
                            minutes += question.Duration;
                        } while (minutes <= requiredMinutes);
                    }

                    //Practical Questions                    
                    requiredMinutes = tag.Minutes - minutes;
                    minutes = 0;
                    var practicalQuestion = questionList.Where(x => !questions.Any(y => y.Id == x.Id) && x.QuestionType == 2 && x.ProficiencyLevel == tag.Proficiency);
                    if (practicalQuestion.Sum(x => x.Duration) >= requiredMinutes)
                    {
                        do
                        {
                            practicalQuestion = questionList.Where(x => !questions.Any(y => y.Id == x.Id) && x.QuestionType == 2 && x.ProficiencyLevel == tag.Proficiency);
                            index = random.Next(practicalQuestion.Count());
                            Question question = practicalQuestion.ElementAtOrDefault(index);
                            questions.Add(question);
                            minutes += question.Duration;
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
            userTestSuite.Duration = testSuite.Duration;
            userTestSuite.StatusId = Convert.ToInt32(TestStatus.Assigned);
            return AddUserTestSuite(userTestSuite);
        }

        public IEnumerable<Question> GetPriview(TestSuite testSuite)
        {
            int optionalQuestions = Convert.ToInt32(testSuite.OptionalQuestion);
            int practicalQuestions = Convert.ToInt32(testSuite.PracticalQuestion);
            Random random = new Random();
            int index = 0, requiredMinutes = 0, minutes = 0;
            List<TestSuiteTag> testSuiteTags;
            List<Question> questions = new List<Question>();

            var questionBank = _context.Query<Question>().ToList();
            GetTestSuiteTags(testSuite, out testSuiteTags);

            foreach (var tag in testSuiteTags)
            {
                minutes = 0;
                var questionList = questionBank.Where(x => x.Tags.Split(',').Contains(Convert.ToString(tag.TagId)) && !questions.Any(y => y.Id == x.Id)).ToList();
                if (questionList.Sum(x => x.Duration) >= tag.Minutes)
                {
                    //Optional Questions
                    var optionalQuestion = questionList.Where(x => !questions.Any(y => y.Id == x.Id) && x.QuestionType == 1 && x.ProficiencyLevel == tag.Proficiency);
                    requiredMinutes = tag.Minutes * Convert.ToInt32(optionalQuestions) / 100;
                    if (optionalQuestion.Sum(x => x.Duration) >= requiredMinutes)
                    {
                        do
                        {
                            optionalQuestion = questionList.Where(x => !questions.Any(y => y.Id == x.Id) && x.QuestionType == 1 && x.ProficiencyLevel == tag.Proficiency);
                            index = random.Next(optionalQuestion.Count());
                            Question question = optionalQuestion.ElementAtOrDefault(index);
                            questions.Add(question);
                            minutes += question.Duration;
                        } while (minutes <= requiredMinutes);
                    }

                    //Practical Questions                    
                    requiredMinutes = tag.Minutes - minutes;
                    minutes = 0;
                    var practicalQuestion = questionList.Where(x => !questions.Any(y => y.Id == x.Id) && x.QuestionType == 2 && x.ProficiencyLevel == tag.Proficiency);
                    if (practicalQuestion.Sum(x => x.Duration) >= requiredMinutes)
                    {
                        do
                        {
                            practicalQuestion = questionList.Where(x => !questions.Any(y => y.Id == x.Id) && x.QuestionType == 2 && x.ProficiencyLevel == tag.Proficiency);
                            index = random.Next(practicalQuestion.Count());
                            Question question = practicalQuestion.ElementAtOrDefault(index);
                            questions.Add(question);
                            minutes += question.Duration;
                        } while (minutes <= requiredMinutes);
                    }
                }
            }
            return questions;
        }

        private void GetTestSuiteTags(TestSuite testSuite, out List<TestSuiteTag> testSuiteTags)
        {
            TestSuiteTag testSuiteTagViewModel;
            testSuiteTags = new List<TestSuiteTag>();
            var tagList = _context.Query<Tags>();
            string[] tags = testSuite.PrimaryTags.Split(',');
            string[] weights = testSuite.Weights.Split(',');
            string[] proficiency = testSuite.Proficiency.Split(',');

            for (int i = 0; i < tags.Length; i++)
            {
                testSuiteTagViewModel = new TestSuiteTag();
                testSuiteTagViewModel.TagId = Convert.ToInt32(tags[i]);
                testSuiteTagViewModel.Weightage = Convert.ToInt32(weights[i]);
                testSuiteTagViewModel.Minutes = testSuite.Duration * Convert.ToInt32(weights[i]) / 100;
                testSuiteTagViewModel.Proficiency = Convert.ToInt32(proficiency[i]);
                testSuiteTags.Add(testSuiteTagViewModel);
            }
        }

        public void TestSuiteActivation()
        {
            int optionalQuestions = 0;
            int practicalQuestions = 0;
            List<TestSuite> updateList = new List<TestSuite>();
            List<TestSuiteTag> testSuiteTags;
            bool isReady = true;
            Random random;
            int requiredMinutes = 0, minutes = 0, index = 0;
            List<Question> questions;
            try
            {
                var testSuites = _context.Query<TestSuite>().Where(x => x.IsDeleted == false);
                var questionBank = _context.Query<Question>().ToList();

                foreach (var testSuite in testSuites)
                {
                    isReady = true;
                    optionalQuestions = testSuite.OptionalQuestion;
                    practicalQuestions = testSuite.PracticalQuestion;

                    questions = new List<Question>();
                    GetTestSuiteTags(testSuite, out testSuiteTags);
                    foreach (var tag in testSuiteTags)
                    {
                        minutes = 0;
                        requiredMinutes = tag.Minutes * Convert.ToInt32(optionalQuestions) / 100;
                        var optionalQuestion = questionBank.Where(x => !questions.Any(y => y.Id == x.Id) && x.QuestionType == 1 && x.ProficiencyLevel == tag.Proficiency && x.Tags.Split(',').Contains(Convert.ToString(tag.TagId)));
                        if (optionalQuestion.Sum(x => x.Duration) >= requiredMinutes)
                        {
                            do
                            {
                                random = new Random();
                                optionalQuestion = questionBank.Where(x => !questions.Any(y => y.Id == x.Id) && x.QuestionType == 1 && x.ProficiencyLevel == tag.Proficiency && x.Tags.Split(',').Contains(Convert.ToString(tag.TagId)));
                                index = random.Next(optionalQuestion.Count());
                                Question question = optionalQuestion.ElementAtOrDefault(index);
                                questions.Add(question);
                                minutes += question.Duration;
                            } while (minutes < requiredMinutes);

                            minutes = 0;
                            requiredMinutes = tag.Minutes * Convert.ToInt32(practicalQuestions) / 100;
                            var practicalQuestion = questionBank.Where(x => !questions.Any(y => y.Id == x.Id) && x.QuestionType == 2 && x.ProficiencyLevel == tag.Proficiency && x.Tags.Split(',').Contains(Convert.ToString(tag.TagId)));
                            if (practicalQuestion.Sum(x => x.Duration) >= requiredMinutes)
                            {
                                do
                                {
                                    random = new Random();
                                    practicalQuestion = questionBank.Where(x => !questions.Any(y => y.Id == x.Id) && x.QuestionType == 2 && x.ProficiencyLevel == tag.Proficiency && x.Tags.Split(',').Contains(Convert.ToString(tag.TagId)));
                                    index = random.Next(practicalQuestion.Count());
                                    Question question = practicalQuestion.ElementAtOrDefault(index);
                                    questions.Add(question);
                                    minutes += question.Duration;
                                } while (minutes < requiredMinutes);
                            }
                            else
                            {
                                testSuite.Status = Convert.ToInt32(TestSuiteStatus.Pending);
                                isReady = false;
                                break;
                            }
                        }
                        else
                        {
                            testSuite.Status = Convert.ToInt32(TestSuiteStatus.Pending);
                            isReady = false;
                            break;
                        }
                    }

                    if (isReady == true)
                    {
                        testSuite.Status = Convert.ToInt32(TestSuiteStatus.Ready);
                    }

                    updateList.Add(testSuite);
                }
            }
            catch
            {
            }
            _context.UpdateAll(updateList);
        }


        public UserTestSuite GetUserTestSuiteByUdi_TestSuitId(int userId, int testsuitId)
        {
            return _context.Query<UserTestSuite>().Where(x => x.UserId == userId && x.TestSuiteId == testsuitId).FirstOrDefault();
        }

        public List<int> GetAllUserIdsForTestSuite(int testSuiteId)
        {
            return _context.Query<UserTestSuite>().Where(x => x.TestSuiteId == testSuiteId).Select(user => user.UserId).ToList();
        }

        public QuestionNavigationBusinessModel GetNavigationDetails(int userTestSuiteId)
        {
            var navigationDetails = new QuestionNavigationBusinessModel();
            navigationDetails.Practical = GetQuestionNavigationDetails(userTestSuiteId, (int)QuestionType.Practical);
            navigationDetails.Objective = GetQuestionNavigationDetails(userTestSuiteId, (int)QuestionType.Objective);
            return navigationDetails;
        }

        private List<QuestionNavigationBasics> GetQuestionNavigationDetails(int userTestSuiteId, int questionType)
        {
            return (from a in _context.Query<UserTestDetails>()
                                  .Where(x => x.UserTestSuite.UserTestSuiteId == userTestSuiteId)
                    join b in _context.Query<Question>().Where(ques => ques.QuestionType == questionType)
                    on a.QuestionId equals b.Id
                    select new QuestionNavigationBasics
                    {
                        QuestionId = b.Id,
                        IsViewedOnly = a.IsViewedOnly,
                        IsAnswered = !(a.Answer.Equals(null) || a.Answer.Trim().Equals("")),
                        IsReviewed = !a.Mark.Equals(null)
                    }).OrderBy(question => question.QuestionId).ToList();
        }
    }
}
