using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace Silicus.ProjectTracker.Web.UserMembership
{
	[ExcludeFromCodeCoverage]
	public class MembershipService : IMembershipService
	{
		public MembershipUser GetUserDetails(string emailAddress)
		{
			return Membership.GetUser(emailAddress);
		}

		public MembershipUser GetUserDetails(Guid membershipId)
		{
			return Membership.GetUser(membershipId);
		}

		public List<SelectListItem> GetRoleForUser(string emailAddress)
		{
			string[] rolesArray = Roles.GetRolesForUser(emailAddress);
			return rolesArray.Select(t => new SelectListItem { Text = t, Value = t }).ToList();
		}

		public MembershipUser CreateMembershipUser(string username, string password)
		{
			MembershipCreateStatus status;
			return Membership.CreateUser(username, password, username, "Question", "Answer", true, out status);
		}

		public List<SelectListItem> GetUserRoles()
		{
			string[] rolesArray = Roles.GetAllRoles();
			List<SelectListItem> userRoleList =
				rolesArray.Select(t => new SelectListItem { Text = t, Value = t }).ToList();
			var item = userRoleList.First(x => x.Value == "RIGDIG");
			userRoleList.Remove(item);
			return userRoleList;
		}

		public void CreateUserRole(string roleName)
		{
			if (!Roles.RoleExists(roleName))
			{
				Roles.CreateRole(roleName);
			}
		}

		public void AssignUserToRole(string username, string roleName)
		{
			if (Roles.RoleExists(roleName))
			{
				string[] rolesArray = Roles.GetRolesForUser(username);
				if (rolesArray.Length > 0)
				{
					Roles.RemoveUserFromRoles(username, rolesArray);
				}
				Roles.AddUserToRole(username, roleName);
			}
		}

		public bool ValidateUserCredentials(string username, string password)
		{
			return Membership.ValidateUser(username, password);
		}

		public void SetAuthenticationCookie(string username, bool rememberMe)
		{
			FormsAuthentication.SetAuthCookie(username, rememberMe);
		}

		public void SignOut()
		{
			FormsAuthentication.SignOut();
		}

		//public OrganizationUser GetUserDetailsByEmailId(string emailId)
		//{
		//    MembershipUser membershipUser =  Membership.GetUser(emailId);
		//    return this.context.Query<OrganizationUser>().Single(o => membershipUser != null && o.MembershipId == new Guid(membershipUser.ProviderUserKey.ToString()));
		//}

		public int GetAdminCountByMembershipIds(IEnumerable<Guid> membershipIds)
		{
			return membershipIds.Select(membershipId => GetUserDetails(membershipId)).Count(user => Roles.IsUserInRole(user.UserName, "ADMIN") || Roles.IsUserInRole(user.UserName, "RIGDIG"));
		}

		public int GetQSUserCountByMembershipIds(IEnumerable<Guid> membershipIds)
		{
			return membershipIds.Select(membershipId => GetUserDetails(membershipId)).Count(user => Roles.IsUserInRole(user.UserName, "QS User"));
		}

		public int GetDSUserCountByMembershipIds(IEnumerable<Guid> membershipIds)
		{
			return membershipIds.Select(membershipId => GetUserDetails(membershipId)).Count(user => Roles.IsUserInRole(user.UserName, "DS User"));
		}

		public IEnumerable<MembershipUser> GetAdminsByMembershipIds(IEnumerable<Guid> membershipIds)
		{
			return
				membershipIds.Select(membershipId => GetUserDetails(membershipId))
					.Where(user => Roles.IsUserInRole(user.UserName, "ADMIN") || Roles.IsUserInRole(user.UserName, "RIGDIG"));
		}

		public void UpdateUser(MembershipUser membershipUser)
		{
			Membership.UpdateUser(membershipUser);
		}

        public bool DeleteUser(string username)
        {
            var result = Membership.DeleteUser(username, true);
            return result;
        }

	}
}