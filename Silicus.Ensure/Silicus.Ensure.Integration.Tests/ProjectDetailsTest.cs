using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Silicus.Ensure.Entities;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Services.Interfaces;
using Silicus.Ensure.Web;
using Silicus.Ensure.Web.Controllers;

namespace Silicus.Ensure.Integration.Tests
{
    /// <summary>
    /// No mock object to be used in these cases. All real object with dependencies setup properly.
    /// </summary>
    [TestClass]
    public class ProjectDetailTest
    {
        private IKernel _kernel;

        //[TestInitialize]
        //public void MyTestInitialize()
        //{
        //    // Custom changes done to dependency setup code to fetch the kernal
        //    // here.
        //    _kernel = NinjectWebCommon.CreateKernel();

        //    var dataContextFactory = _kernel.Get<IDataContextFactory>();

        //    using (IDataContext dataContext = dataContextFactory.Create(ConnectionType.Ip))
        //    {
        //        DeleteExistingDataFromDb(dataContext);
        //    }
        //}

        //[TestMethod]
        //public void AddNotification_NotificationAdded_AddNotificationInDb()
        //{
        //    var projectDetailService = _kernel.Get<IProjectDetailService>();

        //    var target = new ProjectController(projectDetailService);

        //    // Act
        //    target.CreateProject(new ProjectDetail
        //    {
        //        ProjectName = "Katana",
        //        Status = "Green"
        //    });

        //    target.CreateProject(new ProjectDetail
        //    {
        //        ProjectName = "KRE",
        //        Status = "Yello"
        //    });

        //    var data = target.GetProjectDetails(new DataSourceRequest()) as JsonResult;
        //    var actualList = ((DataSourceResult) (data.Data)).Data as List<ProjectDetail>;

        //    //Assert
        //    Assert.AreEqual(2, actualList.Count);
        //}

        //private void DeleteExistingDataFromDb(IDataContext dataContext)
        //{
        //    var projectDetails = dataContext.Query<ProjectDetail>().ToList();

        //    if (projectDetails.Count > 0)
        //    {
        //        dataContext.DeleteAll(projectDetails);
        //    }
        //}
    }
}
