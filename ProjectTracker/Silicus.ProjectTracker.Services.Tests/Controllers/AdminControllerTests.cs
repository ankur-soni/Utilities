using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Silicus.ProjectTracker.Entities;
using Silicus.ProjectTracker.Models.DataObjects;
using Moq;
using Silicus.ProjectTracker.Services;

namespace Silicus.ProjectTracker.Services.Tests
{
    [TestClass()]
    public class AdminControllerTests
    {
        //[TestMethod()]
        //public void GetProjectDetailsTest()
        //{          
            
        //    var dataContextFactory = new Mock<IDataContextFactory>();
        //    var dataContext = new Mock<IDataContext>();

        //    var list = new List<ProjectDetail>
        //    {
        //        new ProjectDetail
        //        {
        //          ProjectName = "Project1",
        //          Status = "Red"
        //        },
        //        new ProjectDetail
        //        {
        //          ProjectName = "Project2",
        //          Status = "Green"
        //        }
        //    };

        //    dataContext.Setup(a => a.Query<ProjectDetail>()).Returns(list.AsQueryable());

        //    dataContextFactory.Setup(s => s.Create(ConnectionType.Ip)).Returns(dataContext.Object);
        //    var target = GetTargetController(dataContextFactory);

        //    // Act
        //    var expectedList = target.GetProjectDetails();

        //    // Assert
        //    Assert.AreEqual(list.Count, expectedList.Count());
        //    dataContext.VerifyAll();
        //}

        //private static ProjectDetailService GetTargetController(Mock<IDataContextFactory> mockFactory = null, Mock<IProjectDetailService> mockSearchCriteriaService = null)
        //{
        //    Mock<IDataContextFactory> factory = mockFactory ?? new Mock<IDataContextFactory>();
        //    Mock<IProjectDetailService> searchCriteriaService = mockSearchCriteriaService ?? new Mock<IProjectDetailService>();

        //    return new ProjectDetailService(factory.Object);
        //}
    }
}
