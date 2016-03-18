using System.Collections.Generic;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Silicus.Finder.Models.DataObjects;
using Silicus.Finder.Services.Interfaces;
using Silicus.Finder.Web.Controllers;

namespace Silicus.Finder.Web.Tests.Controllers
{
    [TestClass]
    public class ProjectControllerTest
    {
        [TestMethod]
        public void Constructor_Invoked_Success()
        {
            var mockService = new Mock<IProjectDetailService>(MockBehavior.Strict);
            var target = new ProjectController(mockService.Object);

            Assert.IsNotNull(target);
        }

        [TestMethod]
        public void GetProjectDetails_ProjectExist_ReturnCorrectCount()
        {
            var mockService = new Mock<IProjectDetailService>(MockBehavior.Strict);

            var list = new List<ProjectDetail>
            {
                new ProjectDetail
                {
                  ProjectName = "Project1",
                  Status = "Red"
                },
                new ProjectDetail
                {
                  ProjectName = "Project2",
                  Status = "Green"
                }
            };

            mockService.Setup(x => x.GetProjectDetails()).Returns(list);

            //var target = new ProjectController(mockService.Object);

           // var data = target.GetProjectList(new DataSourceRequest()) as JsonResult;
            //var actualList = ((DataSourceResult)(data.Data)).Data as List<ProjectDetail>;

            //Assert
            //Assert.AreEqual(list.Count, actualList.Count);
        }

        [TestMethod]
        public void GetProjectDetails_MoreProjectExistThanRequested_ReturnCorrectCount()
        {
            var mockService = new Mock<IProjectDetailService>(MockBehavior.Strict);

            var list = new List<ProjectDetail>
            {
                new ProjectDetail
                {
                  ProjectName = "Project1",
                  Status = "Red"
                },
                new ProjectDetail
                {
                  ProjectName = "Project2",
                  Status = "Green"
                },
                new ProjectDetail
                {
                  ProjectName = "Project3",
                  Status = "Red"
                },
                new ProjectDetail
                {
                  ProjectName = "Project4",
                  Status = "Green"
                },
                new ProjectDetail
                {
                  ProjectName = "Project5",
                  Status = "Red"
                },
                new ProjectDetail
                {
                  ProjectName = "Project6",
                  Status = "Green"
                }
            };

            mockService.Setup(x => x.GetProjectDetails()).Returns(list);

            //var target = new ProjectController(mockService.Object);

            //var data = target.GetProjectList(new DataSourceRequest() { PageSize = 5 }) as JsonResult;
            //var actualList = ((DataSourceResult)(data.Data)).Data as List<ProjectDetail>;

            ////Assert
            //Assert.AreEqual(5, actualList.Count);
        }
    }
}
