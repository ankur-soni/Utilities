using System;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using Silicus.FrameWorx.Auditing;
using Silicus.FrameWorx.Logger;
using Silicus.Ensure.Entities;
using Silicus.Ensure.Services;
using Silicus.Ensure.Services.Interfaces;
using Silicus.Ensure.Web;
using Silicus.Ensure.Web.Mappings;

[assembly: WebActivator.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(NinjectWebCommon), "Stop")]

namespace Silicus.Ensure.Web
{
    [ExcludeFromCodeCoverage]
    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        public static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<ILogger>().To<DatabaseLogger>()
                .WithConstructorArgument("connectionString", "name=SilicusLoggerDataContext")
                .WithConstructorArgument("type", Type.GetType(string.Empty))
                .WithConstructorArgument("dateGetter", (Func<DateTime>)(() => DateTime.UtcNow))
                .WithConstructorArgument("className", string.Empty);

            kernel.Bind<IDataContextFactory>().To<DataContextFactory>();
            kernel.Bind<IProjectDetailService>().To<ProjectDetailService>();
            kernel.Bind<IProjectService>().To<ProjectService>();
            kernel.Bind<IUserService>().To<UserService>();
            kernel.Bind<IRolesService>().To<RolesService>();
            kernel.Bind<IMappingService>().To<MappingService>();
            kernel.Bind<ISmtpClient>().To<SmtpClientWrapper>();
            kernel.Bind<IEmailService>().To<EmailService>();
            kernel.Bind<IQuestionService>().To<QuestionService>();

            kernel.Bind<ICookieHelper>().To<CookieHelper>();

            kernel.Bind<IAuditManager>().To<AuditManager>();
            kernel.Bind<ITagsService>().To<TagService>();
            kernel.Bind<ITestSuiteService>().To<TestSuiteService>();
            kernel.Bind<IPositionService>().To<PositionService>()
                .WithConstructorArgument("connectionString", "name=SilicusAuditingDataContext");
        }
    }
}
