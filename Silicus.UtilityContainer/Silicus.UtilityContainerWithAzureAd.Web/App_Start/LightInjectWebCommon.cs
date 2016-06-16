using Silicus.UtilityContainer.Services;
using Silicus.UtilityContainer.Services.Interfaces;

using System.Reflection;
using Silicus.UtilityContainer.Entities;
using LightInject;
using Silicus.UtilityContainerWithAzureAd.Web;
using Silicus.UtilityContainer.Security.Interface;
using Silicus.UtilityContainer.Security;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(LightInjectWebCommon), "CreateContainer")]

namespace Silicus.UtilityContainerWithAzureAd.Web
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
            container.Register<IAuthentication, Authentication>();
            container.Register<IAuthorization, Authorization>();
            container.Register<IUtilityUserRoleService, UtilityUserRoleService>();
            container.Register<IUtilityService, UtilityService>();
            container.Register<IRoleService, RoleService>();
            container.Register<IUserService, UserService>();
            container.Register<IUserSecurityService, UserSecurityService>();
        }

    }
}