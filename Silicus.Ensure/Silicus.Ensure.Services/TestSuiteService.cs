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
            if (UserTestSuite.UserApplicationId > 0)
            {
                _context.Update(UserTestSuite);
                _context.AttachAndMakeStateModified(UserTestSuite);
                _context.SaveChanges();
            }
        }

        public void DeleteUserTestSuite(UserTestSuite UserTestSuite)
        {
            if (UserTestSuite.UserApplicationId > 0)
            {
                _context.Delete(UserTestSuite);
            }
        }

        public TestSuite GetTestSuitById(int testSuiteId)
        {
            return _context.Query<TestSuite>().Where(x => x.TestSuiteId == testSuiteId).FirstOrDefault();
        }

        public UserTestSuite GetUserTestSuiteByUserApplicationId(int applicationId)
        {
            return _context.Query<UserTestSuite>().Where(x => x.UserApplicationId == applicationId).FirstOrDefault();
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
            _context.AttachAndMakeStateModified(userTestDetails);
            _context.Update(userTestDetails);
            _context.SaveChanges();
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
                //var testDetails = _context.Query<UserTestDetails>().Where(x => x.UserTestSuite.UserTestSuiteId == userTestSuitId);
                var testDetails = from a in _context.Query<UserTestDetails>()
                                               .Where(x => x.UserTestSuite.UserTestSuiteId == userTestSuitId)
                                  join b in _context.Query<Question>()
                                  on a.QuestionId equals b.Id
                                  where b.Id == a.QuestionId && b.QuestionType == (int)QuestionType.Practical
                                  select new TestDetailsBusinessModel
                                  {
                                      ReviwerMark = a.Mark,
                                      Comment = a.ReviwerComment
                                  };
                var isAllQueustionReviewed = testDetails==null || !testDetails.Any(y => y.ReviwerMark == null || y.Comment == null || y.Comment.Trim() == "");
                return isAllQueustionReviewed;
            }
            return false;

        }

        public TestDetailsBusinessModel GetUserTestDetailsByUserTestSuitId(int? userTestSuitId, int? questionNumber, int questionType, QuestionType testStartWithQuestionType = QuestionType.Objective)
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
                              CorrectAnswer = b.CorrectAnswer,
                              Comment = a.ReviwerComment,
                              Marks = b.Marks,
                              DisplayQuestionNumber = index + 1,
                              IsViewedOnly = a.IsViewedOnly,
                              IsAnswered = !(a.Answer.Equals(null) || a.Answer.Trim().Equals(""))
                          }).FirstOrDefault();
            if (result == null)
            {
                return null;
            }
            result.PreviousQuestionId = index <= 0 ? null : previousQuestionId;
            result.NextQuestionId = index >= questionNumberList.Count - 1 ? null : nextQuestionId;

            if (testStartWithQuestionType == QuestionType.Practical)
            {
                if (questionType == (int)QuestionType.Practical && result.NextQuestionId == null)
                {
                    var objectiveQuestions = GetQuestionsByUserTestSuiteId(userTestSuitId, (int)QuestionType.Objective);
                    result.NextQuestionId = GetFirstOrLastQuestionId(objectiveQuestions, QuestionType.Objective);
                }
                if (questionType == (int)QuestionType.Objective && result.PreviousQuestionId == null)
                {
                    var practicalQuestions = GetQuestionsByUserTestSuiteId(userTestSuitId, (int)QuestionType.Practical);
                    result.PreviousQuestionId = GetFirstOrLastQuestionId(practicalQuestions, QuestionType.Practical);
                }
            }
            else if (testStartWithQuestionType == QuestionType.Objective)
            {
                if (questionType == (int)QuestionType.Objective && result.NextQuestionId == null)
                {
                    var practicalQuestions = GetQuestionsByUserTestSuiteId(userTestSuitId, (int)QuestionType.Practical);
                    result.NextQuestionId = practicalQuestions.Any() ? practicalQuestions?.ElementAtOrDefault(0) : null;
                }
                if (questionType == (int)QuestionType.Practical && result.PreviousQuestionId == null)
                {
                    var objectiveQuestions = GetQuestionsByUserTestSuiteId(userTestSuitId, (int)QuestionType.Objective);
                    result.PreviousQuestionId = objectiveQuestions.Any() ? objectiveQuestions?.ElementAtOrDefault(objectiveQuestions.Count - 1) : null;
                }
            }
            return result;
        }


        private int? GetFirstOrLastQuestionId(List<int> questionIds, QuestionType type)
        {
            if (questionIds != null)
            {
                if (type == QuestionType.Practical)
                {
                    return questionIds.Any() ? (int?)questionIds.ElementAtOrDefault(questionIds.Count - 1) : null;
                }
                else if (type == QuestionType.Objective)
                {
                    return questionIds.Any() ? (int?)questionIds.ElementAtOrDefault(0) : null;
                }
            }
            return null;
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
            var testSuiteDetails = new List<UserTestDetails>();
            UserTestDetails testSuiteDetail;
            var questions = GenerateQuestionSet(testSuite);
            foreach (var question in questions)
            {
                testSuiteDetail = new UserTestDetails();
                testSuiteDetail.QuestionId = question.Id;
                testSuiteDetails.Add(testSuiteDetail);
            }
            userTestSuite.UserTestDetails = testSuiteDetails;
            userTestSuite.ObjectiveCount = questions.Count(x => x.QuestionType == 1);
            userTestSuite.PracticalCount = questions.Count(x => x.QuestionType == 2);
            userTestSuite.MaxScore = questions.Sum(x => x.Marks);
            userTestSuite.Duration = testSuite.Duration;
            userTestSuite.StatusId = Convert.ToInt32(CandidateStatus.TestAssigned);
            return AddUserTestSuite(userTestSuite);
        }

        private List<Question> GenerateQuestionSet(TestSuite testSuite)
        {
            int optionalQuestions = Convert.ToInt32(testSuite.OptionalQuestion);
            Random random = new Random();
            int index, requiredMinutes, minutes;
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
                            if (practicalQuestion != null && practicalQuestion.Any())
                            {
                                index = random.Next(practicalQuestion.Count());
                                Question question = practicalQuestion.ElementAtOrDefault(index);
                                if (question != null)
                                {
                                    questions.Add(question);
                                    minutes += question.Duration;
                                }
                            }
                            else
                            {
                                break;
                            }
                        } while (minutes <= requiredMinutes);
                    }
                }
            }
            return questions;
        }

        public List<Question> GetPreview(PreviewTestBusinessModel previewTest)
        {
            var questionIds = new List<int>();
            var tempPreviewTestDetails = new TempPreviewTest();
            var questions = GenerateQuestionSet(previewTest.TestSuite);
            if (questions.Any())
            {
                questions = questions.OrderBy(ques => ques.Id).ToList();
                questionIds.AddRange(questions.Select(question => question.Id));
                tempPreviewTestDetails.QuestionIds = string.Join(",", questionIds);
                tempPreviewTestDetails.ViewerId = previewTest.ViewerId;
                tempPreviewTestDetails.CandidateId = previewTest.CandidateId;
                tempPreviewTestDetails.TestSuiteId = previewTest.TestSuite.TestSuiteId;
            }
            UpsertTempPreviewTest1(tempPreviewTestDetails);
            return questions;
        }

        private void UpsertTempPreviewTest1(TempPreviewTest tempPreviewTestDetails)
        {
            var tempPreviewTestPrevious = GetTempPreviewTest(tempPreviewTestDetails);
            if (tempPreviewTestPrevious != null)
            {
                tempPreviewTestPrevious.QuestionIds = tempPreviewTestDetails.QuestionIds;
                _context.Update(tempPreviewTestPrevious);
            }
            else
            {
                _context.Add(tempPreviewTestDetails);
            }
        }

        private TempPreviewTest GetTempPreviewTest(TempPreviewTest tempPreviewTestDetails)
        {
            return _context.Query<TempPreviewTest>().FirstOrDefault(temp => temp.ViewerId == tempPreviewTestDetails.ViewerId
                && temp.CandidateId == tempPreviewTestDetails.CandidateId && temp.TestSuiteId == tempPreviewTestDetails.TestSuiteId);
        }


        public TestDetailsBusinessModel GetUserTestDetailsByViewerId(PreviewTestBusinessModel previewTest, int? questionNumber, int questionType)
        {
            List<int> allquestionsForPreview = GetQuestionsForPreview(previewTest);
            var questionNumberList = FilterQuestionsByType(allquestionsForPreview, (QuestionType)questionType);
            questionNumber = questionNumber == null && questionNumberList.Count > 0 ? questionNumberList.ElementAtOrDefault(0) : questionNumber;
            if (questionNumber == null)
            {
                return null;
            }
            var index = questionNumberList.IndexOf((int)questionNumber);
            int? previousQuestionId = questionNumberList.ElementAtOrDefault(index - 1);
            int? nextQuestionId = questionNumberList.ElementAtOrDefault(index + 1);
            var result = (from b in _context.Query<Question>()
                          where b.Id == questionNumber
                          select new TestDetailsBusinessModel
                          {
                              QuestionId = b.Id,
                              Marks = b.Marks,
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
                              DisplayQuestionNumber = index + 1
                          }).FirstOrDefault();
            if (result == null)
            {
                return null;
            }
            result.PreviousQuestionId = index <= 0 ? null : previousQuestionId;
            result.NextQuestionId = index >= questionNumberList.Count - 1 ? null : nextQuestionId;
            if (questionType == (int)QuestionType.Practical && result.PreviousQuestionId == null)
            {
                List<int> objectiveQuestions = FilterQuestionsByType(allquestionsForPreview, QuestionType.Objective);
                result.PreviousQuestionId = objectiveQuestions?.ElementAtOrDefault(objectiveQuestions.Count - 1);
            }
            if (questionType == (int)QuestionType.Objective && result.NextQuestionId == null)
            {
                List<int> practicalQuestions = FilterQuestionsByType(allquestionsForPreview, QuestionType.Practical);
                result.NextQuestionId = practicalQuestions?.ElementAtOrDefault(0);
            }
            return result;
        }

        private List<int> FilterQuestionsByType(List<int> questionIds, QuestionType questionType)
        {
            return (from b in _context.Query<Question>()
                    where questionIds.Contains(b.Id) && b.QuestionType == (int)questionType
                    select b.Id).OrderBy(ques => ques).ToList();
        }

        private List<int> GetQuestionsForPreview(PreviewTestBusinessModel previewTestBusinessModel)
        {
            var questionIds = new List<int>();
            var previewTest = new TempPreviewTest { TestSuiteId = previewTestBusinessModel.TestSuite.TestSuiteId, ViewerId = previewTestBusinessModel.ViewerId, CandidateId = previewTestBusinessModel.CandidateId };
            var previewTestDetails = GetTempPreviewTest(previewTest);
            if (previewTestDetails != null)
            {
                if (!string.IsNullOrWhiteSpace(previewTestDetails.QuestionIds))
                {
                    questionIds = previewTestDetails.QuestionIds.Split(',').Select(int.Parse).ToList();
                }
            }
            return questionIds.OrderBy(ques => ques).ToList();
        }

        private void GetTestSuiteTags(TestSuite testSuite, out List<TestSuiteTag> testSuiteTags)
        {
            TestSuiteTag testSuiteTagViewModel;
            testSuiteTags = new List<TestSuiteTag>();
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
                var testSuites = _context.Query<TestSuite>().Where(x => !x.IsDeleted);
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

                    if (isReady)
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
            return _context.Query<UserTestSuite>().Where(x => x.UserApplicationId == userId && x.TestSuiteId == testsuitId).FirstOrDefault();
        }

        public List<int> GetAllUserIdsForTestSuite(int testSuiteId)
        {
            return _context.Query<UserTestSuite>().Where(x => x.TestSuiteId == testSuiteId).Select(user => user.UserApplicationId).ToList();
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
                        IsReviewed = !a.Mark.Equals(null),
                        IsCorrect = !a.Mark.Equals(null) && a.Mark > 0,
                        QuestionDescription = b.QuestionDescription
                    }).OrderBy(question => question.QuestionId).ToList();
        }

        public TestSummaryBusinessModel GetTestSummary(int userTestSuiteId)
        {
            decimal percentageConstant = 100.00M;
            var testDetails = new TestSummaryBusinessModel { Practical = new TestSummaryBasicDetails(), Objective = new TestSummaryBasicDetails() };
            var userTestDetails = (from userTestDetail in _context.Query<UserTestDetails>().Where(detail => detail.UserTestSuite.UserTestSuiteId == userTestSuiteId)
                                   join question in _context.Query<Question>() on userTestDetail.QuestionId equals question.Id
                                   select new { questionDetails = userTestDetail, maximumMarks = question.Marks, questionType = question.QuestionType });
            foreach (var basicDetails in userTestDetails)
            {
                if (basicDetails.questionDetails != null)
                {
                    if (basicDetails.questionType == (int)QuestionType.Practical)
                    {
                        testDetails.Practical.TotalQuestionCount++;
                        testDetails.Practical.MaximumMarks += basicDetails.maximumMarks;
                        if (basicDetails.questionDetails.Mark != null && basicDetails.questionDetails.Mark > 0)
                        {
                            testDetails.Practical.MarksObtained += (int)basicDetails.questionDetails.Mark;
                            testDetails.Practical.CorrectAnswersCount++;
                        }
                    }
                    else if (basicDetails.questionType == (int)QuestionType.Objective)
                    {
                        testDetails.Objective.TotalQuestionCount++;
                        testDetails.Objective.MaximumMarks += basicDetails.maximumMarks;
                        if (basicDetails.questionDetails.Mark != null && basicDetails.questionDetails.Mark > 0)
                        {
                            testDetails.Objective.MarksObtained += (int)basicDetails.questionDetails.Mark;
                            testDetails.Objective.CorrectAnswersCount++;
                        }
                    }
                }
            }
            testDetails.Practical.IncorrectAnswersCount = testDetails.Practical.TotalQuestionCount - testDetails.Practical.CorrectAnswersCount;
            testDetails.Objective.IncorrectAnswersCount = testDetails.Objective.TotalQuestionCount - testDetails.Objective.CorrectAnswersCount;
            testDetails.TotalMaximumMarks = testDetails.Practical.MaximumMarks + testDetails.Objective.MaximumMarks;
            testDetails.TotalObtainedMarks = testDetails.Practical.MarksObtained + testDetails.Objective.MarksObtained;
            testDetails.Percentage = testDetails.TotalObtainedMarks != 0 ? (decimal)testDetails.TotalObtainedMarks / testDetails.TotalMaximumMarks * percentageConstant : 0;
            return testDetails;
        }


        #region Get TestDetails By Test Suit

        public TestDetailsBusinessModel GetTestDetailsByTestSuit(PreviewTestBusinessModel previewTest, int? questionNumber, int questionType)
        {
            List<int> allquestionsForPreview = GetQuestionsForPreviewTestSuite(previewTest);
            var questionNumberList = FilterQuestionsByType(allquestionsForPreview, (QuestionType)questionType);
            questionNumber = questionNumber == null && questionNumberList.Count > 0 ? questionNumberList.ElementAtOrDefault(0) : questionNumber;
            if (questionNumber == null)
            {
                return null;
            }
            var index = questionNumberList.IndexOf((int)questionNumber);
            int? previousQuestionId = questionNumberList.ElementAtOrDefault(index - 1);
            int? nextQuestionId = questionNumberList.ElementAtOrDefault(index + 1);
            var result = (from b in _context.Query<Question>()
                          where b.Id == questionNumber
                          select new TestDetailsBusinessModel
                          {
                              QuestionId = b.Id,
                              Marks = b.Marks,
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
                              DisplayQuestionNumber = index + 1
                          }).FirstOrDefault();
            if (result == null)
            {
                return null;
            }
            result.PreviousQuestionId = index <= 0 ? null : previousQuestionId;
            result.NextQuestionId = index >= questionNumberList.Count - 1 ? null : nextQuestionId;
            if (questionType == (int)QuestionType.Practical && result.PreviousQuestionId == null)
            {
                List<int> objectiveQuestions = FilterQuestionsByType(allquestionsForPreview, QuestionType.Objective);
                result.PreviousQuestionId = objectiveQuestions?.ElementAtOrDefault(objectiveQuestions.Count - 1);
            }
            if (questionType == (int)QuestionType.Objective && result.NextQuestionId == null)
            {
                List<int> practicalQuestions = FilterQuestionsByType(allquestionsForPreview, QuestionType.Practical);
                result.NextQuestionId = practicalQuestions?.ElementAtOrDefault(0);
            }
            return result;
        }


        private List<int> GetQuestionsForPreviewTestSuite(PreviewTestBusinessModel previewTestBusinessModel)
        {
            var questionIds = new List<int>();
            var previewTest = new TempPreviewTest { TestSuiteId = previewTestBusinessModel.TestSuite.TestSuiteId, ViewerId = previewTestBusinessModel.ViewerId };
            var previewTestDetails = GetTempPreviewTestSuite(previewTest);
            if (previewTestDetails != null)
            {
                if (!string.IsNullOrWhiteSpace(previewTestDetails.QuestionIds))
                {
                    questionIds = previewTestDetails.QuestionIds.Split(',').Select(int.Parse).ToList();
                }
            }
            return questionIds.OrderBy(ques => ques).ToList();
        }

        private TempPreviewTest GetTempPreviewTestSuite(TempPreviewTest tempPreviewTestDetails)
        {
            return _context.Query<TempPreviewTest>().FirstOrDefault(temp => temp.ViewerId == tempPreviewTestDetails.ViewerId
               && temp.TestSuiteId == tempPreviewTestDetails.TestSuiteId);
        }
        #endregion
    }
}
