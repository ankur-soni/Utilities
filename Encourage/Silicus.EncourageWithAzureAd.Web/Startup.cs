﻿using Owin;


namespace Silicus.EncourageWithAzureAd.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
