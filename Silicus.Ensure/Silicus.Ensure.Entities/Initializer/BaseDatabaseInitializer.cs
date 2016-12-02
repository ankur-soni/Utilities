﻿using System.Configuration;
using System.IO;
using System.Reflection;
using Silicus.Ensure.Models.DataObjects;
using System;
using Silicus.Ensure.Models.Constants;

namespace Silicus.Ensure.Entities.Initializer
{
    public static class BaseDatabaseInitializer
    {
        private static readonly string DropConnectionScript = "Silicus.Ensure.Entities.DatabaseScripts.DropConnection.sql";
        private static readonly string IndexScriptLocation = "Silicus.Ensure.Entities.DatabaseScripts.SqlIndexes.sql";
        private static readonly string DatabaseName = ConfigurationManager.AppSettings["DBName"];
        private static readonly string MembershipDatabaseName = ConfigurationManager.AppSettings["MembershipDBName"];

        public static void Seed(SilicusIpDataContext context)
        {
            //DropExistingConnectionToDatabase(context, DatabaseName);

            //AddIndexes(context, DatabaseName);
            context.Add(new User
           {
               FirstName = "Test",
               LastName = "Admin",
               NewPassword = "testadmin@123",
               ConfirmPassword = "testadmin@123",
               Address = "Test",
               Email = "testadmin@test.com",
               IdentityUserId = new System.Guid("2D3B545B-50F6-45D4-B916-23D276B3DB00"),
               Role = "ADMIN",
               IsActive = true

           });
            context.Add(new User
            {
                FirstName = "Nilkanth",
                LastName = "Kapale",
                NewPassword = "Nilkanth@123",
                ConfirmPassword = "Nilkanth@123",
                Address = "Sangvi",
                Email = "nilkanthKapale10@gmail.com",
                IdentityUserId = new System.Guid("88B34980-EDD3-420C-8387-621B629A4E9B"),
                Role = "Panel",
                IsActive = true

            });
            context.Add(new User
            {
                FirstName = "Lorem",
                LastName = "Lipsum",
                NewPassword = "Candidate@123",
                ConfirmPassword = "Candidate@123",
                Address = "Pune",
                Email = "lorem@gmail.com",
                IdentityUserId = new System.Guid("50cad72d-4ff3-48ce-ba32-44675cfd2744"),
                Role = "CANDIDATE",
                Gender = "Male",
                Department = "Delivery",
                TestStatus = Convert.ToString(TestStatus.NotAssigned),
                Position = "Sr.Developer",
                Experience = "5",
                IsActive = true

            });
            context.Add(new Project
            {
                ProjectDescription = "Landmark"
            });

            context.Add(new Project
            {
                ProjectDescription = "MorningStar"
            });

            context.Add(new ProjectDetail
            {
                ProjectName = "Landmark",
                Status = "Red"
            });

            context.Add(new ProjectDetail
            {
                ProjectName = "MorningStar",
                Status = "Green"
            });

            context.Add(new Manager
            {
                ManagerName = "Shailendra"
            });

            context.Add(new Manager
            {
                ManagerName = "Sulekha"
            });

            context.Add(new Position
            {
                PositionName = "Dot Net Developer"
            });

            context.Add(new Tags
            {
                TagName = "ASP.NET",
                Description = "ASP.Net related questions",
                IsActive = true
            });

            context.Add(new Tags
            {
                TagName = "MVC5",
                Description = "ASP.Net MVC related questions.",
                IsActive = true
            });

            context.Add(new TestSuite
            {
                TestSuiteName = "Dot Net Developer",
                Position = 1,
                Duration = 30,
                Competency = 2,
                IsDeleted = false,
                PrimaryTags = "1,2",
                Weights = "70,30",
                Status = 1,

            });

            context.Add(new TestSuite
            {
                TestSuiteName = "MVC Developer",
                Position = 1,
                Duration = 30,
                Competency = 2,
                IsDeleted = false,
                PrimaryTags = "1,2",
                Weights = "30,70",
                Status = 1,
            });

            context.Add(new Question
            {
                QuestionType = 1,
                QuestionDescription = "Which component handle <strong>Memory management</strong> in ASP.NET?",
                AnswerType = 1,
                Option1 = "CLR",
                Option2 = "CTS",
                Option3 = "CLS",
                Option4 = "BCL",
                CorrectAnswer = "1",
                Answer = null,
                Tags = "1,2",
                Competency = 1,
                Duration = 1,
                Marks = 1,
                IsPublishd = true,
                IsDeleted = false,
                CreatedOn = DateTime.Now,
                CreatedBy = 0,
                ModifiedOn = DateTime.Now,
                ModifiedBy = 0
            });

            context.Add(new Question
            {
                QuestionType = 2,
                QuestionDescription = "Write program of get name of employee from id from list using <strong>lymbda expression</strong>.",
                AnswerType = 1,
                Option1 = null,
                Option2 = null,
                Option3 = null,
                Option4 = null,
                CorrectAnswer = null,
                Answer = "List&lt;Emp&gt; emp = emp.Select(x=&gt;x.Id==2).Name;",
                Tags = "1",
                Competency = 1,
                Duration = 1,
                Marks = 2,
                IsPublishd = true,
                IsDeleted = false,
                CreatedOn = DateTime.Now,
                CreatedBy = 0,
                ModifiedOn = DateTime.Now,
                ModifiedBy = 0
            });
            context.Add(new Question
            {
                QuestionType = 1,
                QuestionDescription = "Web.config file is used...",
                AnswerType = 1,
                Option1 = "Configures the time that the server-side codebehind module is called",
                Option2 = "To store the global information and variable definitions for the application",
                Option3 = "To configure the web server",
                Option4 = "To configure the web browser",
                CorrectAnswer = "2",
                Answer = null,
                Tags = "1",
                Competency = 2,
                Duration = 1,
                Marks = 2,
                IsPublishd = true,
                IsDeleted = false,
                CreatedOn = DateTime.Now,
                CreatedBy = 0,
                ModifiedOn = DateTime.Now,
                ModifiedBy = 0
            });
            context.Add(new Question
            {
                QuestionType = 1,
                QuestionDescription = "Which of the following object is not an ASP component?",
                AnswerType = 1,
                Option1 = "LinkCounter",
                Option2 = "Counter",
                Option3 = "AdRotator",
                Option4 = "File Access",
                CorrectAnswer = "1,2",
                Answer = null,
                Tags = "1",
                Competency = 2,
                Duration = 1,
                Marks = 1,
                IsPublishd = true,
                IsDeleted = false,
                CreatedOn = DateTime.Now,
                CreatedBy = 0,
                ModifiedOn = DateTime.Now,
                ModifiedBy = 0
            });
            context.Add(new Question
            {
                QuestionType = 1,
                QuestionDescription = "The first event triggers in an aspx page is.",
                AnswerType = 1,
                Option1 = "Page_Init()",
                Option2 = "Page_Load()",
                Option3 = "Page_click()",
                Option4 = "Page_load()",
                CorrectAnswer = "1",
                Answer = null,
                Tags = "1",
                Competency = 2,
                Duration = 1,
                Marks = 1,
                IsPublishd = true,
                IsDeleted = false,
                CreatedOn = DateTime.Now,
                CreatedBy = 0,
                ModifiedOn = DateTime.Now,
                ModifiedBy = 0
            });
            context.Add(new Question
            {
                QuestionType = 2,
                QuestionDescription = "How do we create a FileSystemObject?",
                AnswerType = 1,
                Option1 = null,
                Option2 = null,
                Option3 = null,
                Option4 = null,
                CorrectAnswer = null,
                Answer = "Server.CreateObject('Scripting.FileSystemObject')",
                Tags = "1,2",
                Competency = 1,
                Duration = 1,
                Marks = 2,
                IsPublishd = true,
                IsDeleted = false,
                CreatedOn = DateTime.Now,
                CreatedBy = 0,
                ModifiedOn = DateTime.Now,
                ModifiedBy = 0
            });
        }

        private static void AddIndexes(SilicusIpDataContext context, string databaseName)
        {
            context.Database.CreateIfNotExists();

            var sqlContent = Content(IndexScriptLocation);

            var modifiedSqlScript = sqlContent.Replace("@DatabaseName", databaseName);

            context.Database.ExecuteSqlCommand(modifiedSqlScript);
        }

        private static void DropExistingConnectionToDatabase(SilicusIpDataContext context, string databaseName)
        {
            var sqlContent = Content(DropConnectionScript);

            var modifiedSqlScript = sqlContent.Replace("@DatabaseName", databaseName);

            context.Database.ExecuteSqlCommand(modifiedSqlScript);
        }

        private static string Content(string fileLocation)
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(fileLocation))
            {
                if (stream == null)
                {
                    return string.Empty;
                }

                var streamReader = new StreamReader(stream);

                return streamReader.ReadToEnd();
            }
        }
    }
}
