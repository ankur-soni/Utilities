using Hangfire;
using LightInject;
using Silicus.Encourage.Services;
using Silicus.Encourage.Services.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Silicus.UtilityContainer.Web
{
    public class ContainerJobActivator : JobActivator
    {
        private IServiceContainer _container;

        public ContainerJobActivator(IServiceContainer container)
        {
            _container = container;
        }

        public override object ActivateJob(Type type)
        {
            _container.Register<INominationService, NominationService>();
            return null;
        }
    }
}