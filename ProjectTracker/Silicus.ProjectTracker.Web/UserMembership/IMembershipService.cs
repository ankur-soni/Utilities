using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;

namespace Silicus.ProjectTracker.Web.UserMembership
{
    public interface IMembershipService
    {
        MembershipUser GetUserDetails(string emailAddress);
        MembershipUser GetUserDetails(Guid membershipId);
        MembershipUser CreateMembershipUser(string username, string password);
        List<SelectListItem> GetUserRoles();
        List<SelectListItem> GetRoleForUser(string emailAddress);
        void CreateUserRole(string roleName);
        void AssignUserToRole(string username, string roleName);
        bool ValidateUserCredentials(string username, string password);
        void SetAuthenticationCookie(string username, bool rememberMe);
        void SignOut();
		int GetAdminCountByMembershipIds(IEnumerable<Guid> membershipIds);
        int GetQSUserCountByMembershipIds(IEnumerable<Guid> membershipIds);
		int GetDSUserCountByMembershipIds(IEnumerable<Guid> activeUserIds);
	    IEnumerable<MembershipUser> GetAdminsByMembershipIds(IEnumerable<Guid> membershipIds);
        void UpdateUser(MembershipUser membershipUser);
        bool DeleteUser(string username);
     

	}
}


