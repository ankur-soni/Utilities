using Silicus.FrameworxProject.Web.App_Start;
using LightInject;
using Silicus.FrameworxProject.DAL;
using Silicus.FrameworxProject.DAL.Interfaces;
using System.Reflection;

using Silicus.FrameworxProject.Services.Interfaces;
using Silicus.FrameworxProject.Services;
using AutoMapper;
using Silicus.Reusable.Web.Mappings;
using Silicus.FrameWorx.Logger;
using System;

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
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DomainToViewModelMappingProfile>();
                cfg.AddProfile<ViewModelToDomainMappingProfile>();
            });
          
            container.Register<IDataContextFactory, DataContextFactory>();
            container.Register<IFrameworxProjectService, FrameworxProjectService>();
            container.Register<IExtensionCodeService, ExtensionCodeService>();
            container.Register<ICommonDbService, CommonDbService>();
            container.Register<IFrameworxFeedbackService, FrameworxFeedbackService>();
            container.Register<IProductBacklogService, ProductBacklogService>();
            container.Register<IMapper>((factory) => mapperConfiguration.CreateMapper());
            //container.Register<Silicus.UtilityContainer.Entities.ICommonDataBaseContext, Silicus.UtilityContainer.Entities.CommonDataBaseContext>();
            container.Register<Silicus.UtilityContainer.Entities.IDataContextFactory, Silicus.UtilityContainer.Entities.DataContextFactory>();
            container.Register<ILogger>((factory) => new DatabaseLogger("name=LoggerDataContext", Type.GetType(string.Empty), (Func<DateTime>)(() => DateTime.UtcNow), string.Empty));
        }

    }
}