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
    public class ProjectDetailServiceTest
    {
        //[TestMethod]
        //public void GetProjectDetails_ProjectsExists_ReturnCorrectCount()
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
        //    //var target = GetTargetService(dataContextFactory);

        //    // Act
        //    //var expectedList = target.GetProjectDetails();

        //    // Assert
        //    //Assert.AreEqual(list.Count, expectedList.Count());
        //    dataContext.VerifyAll();
        //}

        //private static ProjectDetailService GetTargetService(Mock<IDataContextFactory> mockFactory = null)
        //{
        //    Mock<IDataContextFactory> factory = mockFactory ?? new Mock<IDataContextFactory>();
        //    return new ProjectDetailService(factory.Object);
        //}
    }
}
