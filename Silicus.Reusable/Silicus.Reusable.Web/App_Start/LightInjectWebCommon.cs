using Silicus.FrameworxProject.Web.App_Start;
using LightInject;
using Silicus.FrameworxProject.DAL;
using Silicus.FrameworxProject.DAL.Interfaces;
using System.Reflection;

using Silicus.FrameworxProject.Services.Interfaces;
using Silicus.FrameworxProject.Services;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(LightInjectWebCommon), "CreateContainer")]

namespace Silicus.FrameworxProject.Web.App_Start
{
    public static class LightInjectWebCommon
    {
        public static IServiceContainer CreateContainer()
        {
            IServiceContainer container = new ServiceContainer();
            container.Register<IServiceContainer, ServiceContainer>();
            InitializeContainer(container);
            container.RegisterControllers(Assembly.GetExecutingAssembly());
            container.EnableMvc();
            return container;
        }

        private static void InitializeContainer(IServiceContainer container)
        {
            container.Register<IDataContextFactory, DataContextFactory>();
            container.Register<IFrameworxProjectService, FrameworxProjectService>();
            container.Register<IExtensionCodeService, ExtensionCodeService>();
            //container.Register<Silicus.UtilityContainer.Entities.ICommonDataBaseContext, Silicus.UtilityContainer.Entities.CommonDataBaseContext>();
            container.Register<Silicus.UtilityContainer.Entities.IDataContextFactory, Silicus.UtilityContainer.Entities.DataContextFactory>();
        }

    }
}