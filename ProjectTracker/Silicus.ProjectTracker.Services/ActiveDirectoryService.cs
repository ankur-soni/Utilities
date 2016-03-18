using System;
using System.Collections.Generic;
using Silicus.ProjectTracker.Core;
using System.Runtime.InteropServices;
using Silicus.ProjectTracker.Entities;
using Silicus.ProjectTracker.Models.DataObjects;
using Silicus.ProjectTracker.Services.Interfaces;
using System.DirectoryServices.AccountManagement;
using System.Web.Configuration;

namespace Silicus.ProjectTracker.Services
{

    public class ActiveDirectoryService : IActiveDirectoryService
    {

        [DllImport("ADVAPI32.dll", EntryPoint = "LogonUserW", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);
        
        private readonly IDataContext context;

        List<ActiveDirectory> adUsers = new List<ActiveDirectory>();

        public ActiveDirectoryService(IDataContextFactory dataContextFactory)
        {
            this.context = dataContextFactory.Create(ConnectionType.Ip);
        }

        public List<ActiveDirectory> GetActiveDirectoryUsers(string user = "")
        {
            string adEmail = WebConfigurationManager.AppSettings["ADDefaultUserEmail"];
            string adPassword = WebConfigurationManager.AppSettings["ADDefaultUserPassword"];

            PrincipalContext ctx = new PrincipalContext(ContextType.Domain, Silicus.ProjectTracker.Core.Constants.DomainName, null, ContextOptions.SimpleBind, adEmail, adPassword);
            
            if (user == "User")
            {
                adUsers = GetSpecificGroupUsers(ctx, Constants.UserGroup);
            }
            else
            {
                adUsers = GetSpecificGroupUsers(ctx, Constants.AdminGroup);
                adUsers = GetSpecificGroupUsers(ctx, Constants.UserGroup);
            }

            ctx.Dispose();       
            return adUsers;
        }

        public List<ActiveDirectory> GetSpecificGroupUsers(PrincipalContext ctx,string groupName)
        {
            GroupPrincipal grp = GroupPrincipal.FindByIdentity(ctx, IdentityType.SamAccountName, groupName);

            if (grp != null)
            {
                foreach (Principal p in grp.GetMembers(false))
                {
                    if (p.DisplayName != null)
                    {
                        adUsers.Add(new ActiveDirectory()
                         {
                             UserName = p.DisplayName,
                             DisplayName = p.SamAccountName
                        });
                                              
                    }
                }

                grp.Dispose();
                                
            }

            return adUsers;
        }

        public bool VerifyLoggedInUser(string username, string password = "")
        {
            string domainName = GetDomainName(username); // Extract domain name form provide DomainUsername e.g Domainname\Username
            string userName = GetUsername(username);  // Extract user name from provided DomainUsername e.g Domainname\Username
            IntPtr token = IntPtr.Zero;

            //userName, domainName and Password parameters are very obvious.
            //dwLogonType (3rd paramter): I used LOGON32_LOGON_INTERACTIVE, This logon type is intended for users who will be interactively using the computer, such as a user being logged on by a terminal server, remote shell, or similar process. This logon type has the additional expense of caching logon information for disconnected operations. For more details about this parameter please see http://msdn.microsoft.com/en-us/library/aa378184(VS.85).aspx
            //dwLogonProvider (4th parameter) : I used LOGON32_PROVIDER_DEFAUL, This provider use the standard logon provider for the system. The default security provider is negotiate, unless you pass NULL for the domain name and the user name is not in UPN format. In this case, the default provider is NTLM. For more details about this parameter please see http://msdn.microsoft.com/en-us/library/aa378184(VS.85).aspx
            //phToken (5th parameter): A pointer to a handle variable that receives a handle to a token that represents the specified user. We can use this handler for impersonation purpose. 
            bool result = LogonUser(userName, domainName, password, 2, 0, ref token);
            return result;

        }

        public string VerifyGroupPolicy(string username)
        {
            string adEmail = WebConfigurationManager.AppSettings["ADDefaultUserEmail"];
            string adPassword = WebConfigurationManager.AppSettings["ADDefaultUserPassword"];

            // set up domain context
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain, Silicus.ProjectTracker.Core.Constants.DomainName, null, ContextOptions.SimpleBind, adEmail, adPassword);

            // find a user
            UserPrincipal user = UserPrincipal.FindByIdentity(ctx, username);

            // find the group in question
            GroupPrincipal groupAdmin = GroupPrincipal.FindByIdentity(ctx, Constants.AdminGroup);
            GroupPrincipal groupUser = GroupPrincipal.FindByIdentity(ctx, Constants.UserGroup);

            if (user != null)
            {
                if (groupAdmin != null && groupUser != null)
                {
                    // check if user is member of that admin group
                    if (user.IsMemberOf(groupUser))
                    {
                        return Constants.UserRole;
                    }
                    // check if user is member of that user group
                    else if (user.IsMemberOf(groupAdmin))
                    {
                        return Constants.AdminRole;

                    }
                    else
                    {
                        return string.Empty;

                    }

                }
                else
                {
                    return string.Empty;

                }
                
            }
            else
            {
              
                return string.Empty;
            }
                       
        }

        /// <summary>
        /// Parses the string to pull the domain name out.
        /// </summary>
        /// <param name="usernameDomain">The string to parse that must contain the domain in either the domain\username or UPN format username@domain</param>
        /// <returns>The domain name or "" if not domain is found.</returns>
        public static string GetDomainName(string usernameDomain)
        {
            if (string.IsNullOrEmpty(usernameDomain))
            {
                throw (new ArgumentException("Argument can't be null.", "usernameDomain"));
            }
            if (usernameDomain.Contains("\\"))
            {
                int index = usernameDomain.IndexOf("\\");
                return usernameDomain.Substring(0, index);
            }
            else if (usernameDomain.Contains("@"))
            {
                int index = usernameDomain.IndexOf("@");
                return usernameDomain.Substring(index + 1);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Parses the string to pull the user name out.
        /// </summary>
        /// <param name="usernameDomain">The string to parse that must contain the username in either the domain\username or UPN format username@domain</param>
        /// <returns>The username or the string if no domain is found.</returns>
        public static string GetUsername(string usernameDomain)
        {
            if (string.IsNullOrEmpty(usernameDomain))
            {
                throw (new ArgumentException("Argument can't be null.", "usernameDomain"));
            }
            if (usernameDomain.Contains("\\"))
            {
                int index = usernameDomain.IndexOf("\\");
                return usernameDomain.Substring(index + 1);
            }
            else if (usernameDomain.Contains("@"))
            {
                int index = usernameDomain.IndexOf("@");
                return usernameDomain.Substring(0, index);
            }
            else
            {
                return usernameDomain;
            }
        }

    }
}
