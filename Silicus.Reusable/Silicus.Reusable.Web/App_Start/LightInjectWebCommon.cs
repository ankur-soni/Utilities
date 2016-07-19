using Silicus.Reusable.Web.App_Start;
using LightInject;
using Silicus.Reusable.DAL;
using Silicus.Reusable.DAL.Interfaces;
using System.Reflection;

using Silicus.Reusable.Services;
using Silicus.Reusable.Services.Interfaces;

using Silicus.Reusable.Web;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(LightInjectWebCommon), "CreateContainer")]

namespace Silicus.Reusable.Web.App_Start
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
            container.Register<IReusableService, ReusableService>();
            container.Register<Silicus.UtilityContainer.Entities.IDataContextFactory, Silicus.UtilityContainer.Entities.DataContextFactory>();
        }

    }
}