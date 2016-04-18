using System;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Silicus.Finder.IdentityWrapper;
using Silicus.FrameWorx.Auditing;
using Silicus.FrameWorx.Logger;
using Silicus.Finder.Entities;
using Silicus.Finder.Services;
using Silicus.Finder.Services.Interfaces;
using Silicus.Finder.Web;
using Silicus.Finder.Web.Mappings;

using LightInject;
using LightInject.Web;
using LightInject.Mvc;
using System.Reflection;
using WebGrease;
using Silicus.Finder.ModelMappingService.Interfaces;
using Silicus.Finder.ModelMappingService;

[assembly: WebActivator.PostApplicationStartMethod(typeof(LightInjectWebCommon), "CreateContainer")]

namespace Silicus.Finder.Web
{
    public static class  LightInjectWebCommon
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
            container.Register<IProjectDetailService, ProjectDetailService>();
            container.Register<IProjectService, ProjectService>();
            container.Register<IUserService, UserService>();
            container.Register<IRolesService, RolesService>();
            container.Register<IMappingService, MappingService>();
            container.Register<ISmtpClient, SmtpClientWrapper>();
            container.Register<IEmailService, EmailService>();
            container.Register<IEmployeeService, EmployeeService>();
            container.Register<ISkillSetService, SkillSetService>();
            container.Register<ICookieHelper, CookieHelper>();
            container.Register<ILogger>((factory) => new DatabaseLogger("name=FinderLoggerDataContext", Type.GetType(string.Empty), (Func<DateTime>)(() => DateTime.UtcNow), string.Empty));
            container.Register<IAuditManager>((factory) => new AuditManager("name=FinderAuditingDataContext"));
            container.Register<ICommonMapper, CommonMapper>();
            container.Register<IUserManager, UserManager>(new PerRequestLifeTime());
        }
    }
}