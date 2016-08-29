using Silicus.UtilityContainer.Services;
using Silicus.UtilityContainer.Services.Interfaces;

using System.Reflection;
using Silicus.UtilityContainer.Entities;
using LightInject;
using Silicus.UtilityContainer.Web;
using Silicus.UtilityContainer.Security.Interface;
using Silicus.UtilityContainer.Security;
using Silicus.Encourage.Services.Interface;
using Silicus.Encourage.Services;
using Silicus.FrameWorx.Logger;
using System;

//[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(LightInjectWebCommon), "CreateContainer")]

namespace Silicus.UtilityContainer.Web
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
            container.Register<INominationService, NominationService>();
            container.Register<ILogger>((factory) => new DatabaseLogger("name=LoggerDataContext", Type.GetType(string.Empty), (Func<DateTime>)(() => DateTime.UtcNow), string.Empty));

        }

    }
}