using Silicus.Encourage.DAL;
using Silicus.Encourage.DAL.Interfaces;
using System.Reflection;
using LightInject;
using Silicus.Encourage.Services.Interface;
using Silicus.Encourage.Services;
using Silicus.UtilityContainer.Security.Interface;
using Silicus.UtilityContainer.Security;
using Silicus.EncourageWithAzureAd.Web;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(LightInjectWebCommon), "CreateContainer")]

namespace Silicus.EncourageWithAzureAd.Web
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
            container.Register<IAwardService, AwardService>();
            container.Register<IAuthentication, Authentication>();
            container.Register<IUserSecurityService,UserSecurityService>();
            container.Register<Silicus.UtilityContainer.Entities.IDataContextFactory, Silicus.UtilityContainer.Entities.DataContextFactory>();
            container.Register<ICommonDbService, CommonDbService>();
            container.Register<INominationService, NominationService>();
            container.Register<IEncourageDatabaseContext, EncourageDatabaseContext>();
            container.Register<IReviewService, ReviewService>();
            container.Register<IResultService, ResultService>();
        }

    }
}