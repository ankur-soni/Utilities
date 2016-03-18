using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Silicus.Finder.Entities;
using Silicus.Finder.Models.DataObjects;
using Silicus.Finder.Services;
using Silicus.Finder.Services.Interfaces;
using Silicus.Finder.Web;
using Silicus.Finder.Web.Controllers;
using LightInject;

namespace Silicus.Finder.Integration.Tests
{
    /// <summary>
    /// No mock object to be used in these cases. All real object with dependencies setup properly.
    /// </summary>
    [TestClass]
    public class ProjectDetailTest
    {
        //private IKernel _kernel;
        private IServiceContainer _serviceContainer;

        [TestInitialize]
        public void MyTestInitialize()
        {
            // Custom changes done to dependency setup code to fetch the kernal
            // here.
            _serviceContainer = LightInjectWebCommon.CreateContainer();

            var dataContextFactory = _serviceContainer.GetInstance<IDataContextFactory>();

            using (IDataContext dataContext = dataContextFactory.Create(ConnectionType.Ip))
            {
                DeleteExistingDataFromDb(dataContext);
            }
        }

        //[TestMethod]
        //public void AddNotification_NotificationAdded_AddNotificationInDb()
        //{
        //    var projectDetailService = new ProjectDetailService(new DataContextFactory());        

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

        //    var data = target.GetProjectList(new DataSourceRequest()) as JsonResult;
        //    var actualList = ((DataSourceResult) (data.Data)).Data as List<ProjectDetail>;

        //    //Assert
        //    Assert.AreEqual(2, actualList.Count);
        //}

        private void DeleteExistingDataFromDb(IDataContext dataContext)
        {
            var projectDetails = dataContext.Query<ProjectDetail>().ToList();

            if (projectDetails.Count > 0)
            {
                dataContext.DeleteAll(projectDetails);
            }
        }
    }
}
