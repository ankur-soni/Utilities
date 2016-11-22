using System.Collections.Generic;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Services.Interfaces;
using Silicus.Ensure.Web.Controllers;
using Silicus.Ensure.Models.Constants;
using System.Collections;
using System.Linq;

namespace Silicus.Ensure.Web.Tests.Controllers
{
    [TestClass]    
    public class QuestionBankControllerTest
    {
        List<Question> questinBank = new List<Question>
            {
                new Question
                {
                  Competency=(int)Competency.Expert,
                  QuestionType=1,
                  Duration=1,
                  Tags="1"
                },
                new Question
                {
                  Competency=(int)Competency.Expert,
                  QuestionType=1,
                  Duration=2,
                  Tags="2"
                },
                new Question
                {
                  Competency=(int)Competency.Expert,
                  QuestionType=1,
                  Duration=5,
                  Tags="1"
                },
                new Question
                {
                  Competency=(int)Competency.Intermediate,
                  QuestionType=1,
                  Duration=1,
                  Tags="3"
                },
                new Question
                {
                  Competency=(int)Competency.Intermediate,
                  QuestionType=2,
                  Duration=2,
                  Tags="5"
                },
                new Question
                {
                  Competency=(int)Competency.Intermediate,
                  QuestionType=1,
                  Duration=10,
                  Tags="2"
                },
                new Question
                {
                  Competency=(int)Competency.Beginner,
                  QuestionType=2,
                  Duration=5,
                  Tags="1"
                },
                new Question
                {
                  Competency=(int)Competency.Beginner,
                  QuestionType=1,
                  Duration=3,
                  Tags="2"
                }
            };
        string tag = "1";
        int competency = (int)Competency.Expert;
        int minutes = 10;

        [TestMethod]
        public void QuestionBank_Exist()
        {            
            //Assert
            Assert.AreNotEqual(0, questinBank.Count, "Question Bank is empty.");
        }

        [TestMethod]
        public void QuestionBank_ExistsQuesionByTag()
        {            
            var questions = questinBank.FindAll(x => x.Tags == tag);
            //Assert
            Assert.AreNotEqual(0, questions.Count,"Questions are not available for selected tag.");
        }

        [TestMethod]
        public void QuestionBank_ExistsQuesionByTagAndCompetency()
        {
            var questions = questinBank.FindAll(x => x.Tags == tag && x.Competency == competency);
            //Assert
            Assert.AreNotEqual(0, questions.Count, "Questions are not available for selected tag and competency.");
        }

        [TestMethod]
        public void QuestionBank_AboveOrEqualToDuration_ByTag()
        {
            var totalDuration = questinBank.Where(x => x.Tags == tag).Select(x=>x.Duration).Sum();
            
            //Assert
            Assert.IsTrue(totalDuration >= minutes, "Questions are not availabe to above duration");
        }

        [TestMethod]
        public void QuestionBank_BelowOrEqualDuration_ByTag()
        {
            var totalDuration = questinBank.Where(x => x.Tags == tag).Select(x => x.Duration).Sum();

            //Assert
            Assert.IsTrue(totalDuration >= minutes - (minutes * 15 / 100), "Questions are not availabe to below duration.");
        }

        [TestMethod]
        public void QuestionBank_AboveOrEqualDuration_ByTagAndCompetency()
        {
            var totalDuration = questinBank.Where(x => x.Tags == tag && x.Competency == competency).Select(x => x.Duration).Sum();

            //Assert
            Assert.IsTrue(totalDuration >= minutes, "Questions are not availabe to above duration.");
        }

        [TestMethod]
        public void QuestionBank_BelowOrEqualDuration_ByTagAndCompetency()
        {
            var totalDuration = questinBank.Where(x => x.Tags == tag && x.Competency == competency).Select(x => x.Duration).Sum();

            //Assert
            Assert.IsTrue(totalDuration >= minutes - (minutes * 15 / 100), "Questions are not availabe to below needed duration.");
        }
    }
}
