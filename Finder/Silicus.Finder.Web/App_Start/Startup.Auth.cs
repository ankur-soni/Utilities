//using System;
//using System.Configuration;
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.Owin;
//using Microsoft.Owin;
//using Microsoft.Owin.Security.Cookies;
//using Owin;
//using Silicus.Finder.Entities;  
////using Silicus.Finder.IdentityWrapper.Entities;
////using Silicus.Finder.IdentityWrapper;
////using Silicus.Finder.IdentityWrapper.Models;

//namespace Silicus.Finder.Web
//{
//    public partial class Startup
//    {
//        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
//        public void ConfigureAuth(IAppBuilder app)
//        {
//            // Configure the db context, user manager and role manager to use a single instance per request
//            //app.CreatePerOwinContext(IdentityDbContext.Create);
//            //app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
//            //app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);
//            //app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

//            // Enable the application to use a cookie to store information for the signed in user
//            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
//            // Configure the sign in cookie
//            app.UseCookieAuthentication(new CookieAuthenticationOptions
//            {
//                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
//                LoginPath = new PathString("/Account/Login"),
//                Provider = new CookieAuthenticationProvider
//                {
//                    // Enables the application to validate the security stamp when the user logs in.
//                    // This is a security feature which is used when you change a password or add an external login to your account.  
//                    ////OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
//                    ////    validateInterval: TimeSpan.FromMinutes(30),
//                    ////    regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager)),
//                },
//                SlidingExpiration = false,
//                ExpireTimeSpan = TimeSpan.FromDays(365)

//            });
//            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

//            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
//            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

//            // Enables the application to remember the second login verification factor such as phone or email.
//            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
//            // This is similar to the RememberMe option when you log in.
//            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

//            // Uncomment the following lines to enable logging in with third party login providers
//            //app.UseMicrosoftAccountAuthentication(
//            //    clientId: "",
//            //    clientSecret: "");

//            //app.UseTwitterAuthentication(
//            //   consumerKey: "",
//            //   consumerSecret: "");

//            //app.UseFacebookAuthentication(
//            //   appId: "",
//            //   appSecret: "");

//            app.UseGoogleAuthentication(
//                clientId: ConfigurationManager.AppSettings["GoogClientID"],
//                clientSecret: ConfigurationManager.AppSettings["GoogClientSecret"]);
//        }
//    }
//}

//----------------------------------------------------------------------------------------------
//    Copyright 2014 Microsoft Corporation
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//----------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// The following using statements were added for this sample.
using Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Configuration;
using System.Globalization;
using System.Threading.Tasks;

namespace Silicus.Finder.Web
{
    public partial class Startup
    {
        //
        // The Client ID is used by the application to uniquely identify itself to Azure AD.
        // The Metadata Address is used by the application to retrieve the signing keys used by Azure AD.
        // The AAD Instance is the instance of Azure, for example public Azure or Azure China.
        // The Authority is the sign-in URL of the tenant.
        // The Post Logout Redirect Uri is the URL where the user will be redirected after they sign out.
        //
        private static string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        private static string aadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
        private static string tenant = ConfigurationManager.AppSettings["ida:Tenant"];
        private static string postLogoutRedirectUri = ConfigurationManager.AppSettings["ida:PostLogoutRedirectUri"];

        string authority = String.Format(CultureInfo.InvariantCulture, aadInstance, tenant);

        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = clientId,
                    Authority = authority,
                    PostLogoutRedirectUri = postLogoutRedirectUri,
                    RedirectUri = postLogoutRedirectUri,
                    Notifications = new OpenIdConnectAuthenticationNotifications
                    {
                        AuthenticationFailed = context =>
                        {
                            context.HandleResponse();
                            context.Response.Redirect("/Error?message=" + context.Exception.Message);
                            return Task.FromResult(0);
                        }
                    }
                });
        }
    }
}