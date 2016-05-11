using LightInject;
using Silicus.Encourage.DAL;
using Silicus.Encourage.DAL.Interfaces;
using Silicus.Encourage.Web.App_Start;
using System.Reflection;


[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(LightInjectWebCommon), "CreateContainer")]

namespace Silicus.Encourage.Web.App_Start
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
        }

    }
}