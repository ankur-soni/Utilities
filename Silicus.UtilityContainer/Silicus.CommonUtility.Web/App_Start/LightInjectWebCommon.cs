﻿using LightInject;
using Silicus.UtilityContainer.Security;
using Silicus.UtilityContainer.Security.Interface;
using Silicus.UtilityContainer.Services.Interfaces;
using Silicus.UtilityContainer.Services;
using Silicus.UtilityContainer.Web.App_Start;
using System.Reflection;

[assembly: WebActivator.PostApplicationStartMethod(typeof(LightInjectWebCommon), "CreateContainer")]

namespace Silicus.UtilityContainer.Web.App_Start
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
            container.Register<IAuthentication, Authentication>();
            container.Register<IUtilityService, UtilityService>();
            container.Register<IUserService, UserService>();
            container.Register<IRoleService, RoleService>();
        }

    }
}