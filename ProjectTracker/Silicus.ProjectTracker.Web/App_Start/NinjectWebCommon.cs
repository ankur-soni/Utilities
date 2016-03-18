using System;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using Silicus.ProjectTracker.Auditing;
using Silicus.ProjectTracker.Core.Interfaces;
using Silicus.ProjectTracker.Entities;
using Silicus.ProjectTracker.Logger;
using Silicus.ProjectTracker.Services;
using Silicus.ProjectTracker.Web;
using Silicus.ProjectTracker.Web.Mappings;
using Silicus.ProjectTracker.Web.UserMembership;
using Silicus.ProjectTracker.Services.Interfaces;
using Silicus.ProjectTracker.Web.Hubs;

[assembly: WebActivator.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(NinjectWebCommon), "Stop")]

namespace Silicus.ProjectTracker.Web
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
                .WithConstructorArgument("connectionString", "name=ProjectTrackerLoggerDataContext")
                .WithConstructorArgument("dateGetter", (Func<DateTime>)(() => DateTime.UtcNow))
                .WithConstructorArgument("type", Type.GetType(string.Empty));                      
            kernel.Bind<IDataContextFactory>().To<DataContextFactory>();
            kernel.Bind<IProjectService>().To<ProjectService>();            
            kernel.Bind<IMappingService>().To<MappingService>();
            kernel.Bind<IMembershipService>().To<MembershipService>();
            kernel.Bind<ISmtpClient>().To<SmtpClientWrapper>();
            kernel.Bind<IEmailService>().To<EmailService>();
            kernel.Bind<ICookieHelper>().To<CookieHelper>();
            kernel.Bind<IActiveDirectoryService>().To<ActiveDirectoryService>();
            kernel.Bind<IProjectMappingService>().To<ProjectMappingService>();
            kernel.Bind<IProjectComplaintService>().To<ProjectComplaintService>();            
            kernel.Bind<IProjectSummaryService>().To<ProjectSummaryService>();
            kernel.Bind<IProjectResourceService>().To<ProjectResourceService>();
            kernel.Bind<IPaymentDetailsService>().To<PaymentDetailsService>();
            kernel.Bind<IChangeRequestDetailsService>().To<ChangeRequestDetailsService>();
            kernel.Bind<IInfrastructureDetailsService>().To<InfrastructureDetailsService>();
            kernel.Bind<IUserDashboardService>().To<UserDashboardService>();
            kernel.Bind<IAdminDashboardService>().To<AdminDashboardService>();
            kernel.Bind<IGenericService>().To<GenericService>();
            kernel.Bind<ITrackerHub>().To<TrackerHub>();
            kernel.Bind<IAuditManager>().To<AuditManager>()
                .WithConstructorArgument("connectionString", "name=ProjectTrackerAuditingDataContext");
        }
    }
}
