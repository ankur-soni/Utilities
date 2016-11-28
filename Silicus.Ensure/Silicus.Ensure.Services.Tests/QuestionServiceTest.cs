using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Silicus.Ensure.Entities;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Services;

namespace Silicus.Ensure.Services.Tests
{
    [TestClass]
    public class QuestionServiceTest
    {
        List<Question> questionBank = new List<Question>
            {

            };

        [TestMethod]
        public void GenerateQuestionList_IncorrectTag_ZeroQuestion()
        {

        }

        [TestMethod]
        public void GenerateQuestionList_DurationTooLess_SingleQuestion()
        {

        }

        [TestMethod]
        public void GenerateQuestionList_DurationTooLarge_ReturnAllQuestion()
        {

        }
    }
}
