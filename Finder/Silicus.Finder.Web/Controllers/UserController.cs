using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity.Owin;  
using Silicus.Finder.Services.Interfaces;
using Silicus.Finder.Web.Mappings;
using Silicus.Finder.Web.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Silicus.Finder.IdentityWrapper;
using Silicus.Finder.IdentityWrapper.Models;
using Silicus.Finder.Models.DataObjects;

namespace Silicus.Finder.Web.Controllers
{
    public class UserController : Controller
    {
        //private readonly IUserService _userService;
        //private readonly IMappingService _mappingService;

        //private ApplicationUserManager _userManager;

        //public ApplicationUserManager UserManager
        //{
        //    get
        //    {
        //        return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //    }
        //    set
        //    {
        //        _userManager = value;
        //    }
        //}

        //private ApplicationRoleManager _roleManager;
        //public ApplicationRoleManager RoleManager
        //{
        //    get
        //    {
        //        return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
        //    }
        //    private set
        //    {
        //        _roleManager = value;
        //    }
        //}

        //public UserController(IUserService userService, IMappingService mappingService)
        //{
        //    _userService = userService;
        //    _mappingService = mappingService;
        //}

        //public ActionResult Dashboard()
        //{

        //    return View();
        //}

        //public async Task<ActionResult> GetUserDetails([DataSourceRequest] DataSourceRequest request)
        //{
        //    var userlist = _userService.GetUserDetails().ToArray();

        //    var viewModels = _mappingService.Map<User[], UserViewModel[]>(userlist);

        //    for (int j = 0; j < viewModels.Count(); j++)
        //    {
        //        var userDetails = await UserManager.FindByEmailAsync(viewModels[j].Email);
        //        if (userDetails != null)
        //        {
        //            var viewUsersRole = await UserManager.GetRolesAsync(userDetails.Id);
        //            viewModels[j].Role = viewUsersRole.FirstOrDefault();
        //        }
        //    }

        //    DataSourceResult result = viewModels.ToDataSourceResult(request);
        //    return Json(result);
        //}

        //[AcceptVerbs(HttpVerbs.Post)]
        //public async Task<ActionResult> CreateUser(UserViewModel vuser)
        //{
        //    var user = new ApplicationUser { UserName = vuser.Email, Email = vuser.Email };
        //    var userResult = await UserManager.CreateAsync(user, vuser.NewPassword);
        //    if (userResult.Succeeded)
        //    {
        //        vuser.IdentityUserId = new Guid(user.Id);
        //        var result = await UserManager.AddToRoleAsync(user.Id, vuser.Role);
        //        if (!result.Succeeded)
        //        {
        //            ModelState.AddModelError("", result.Errors.First());
        //            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
        //        }
                
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", userResult.Errors.First());
        //        ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
        //        return View();

        //    }
            
        //    var organizationUserDomainModel =
        //                      _mappingService.Map<UserViewModel, User>(vuser);

        //    //ModelState.SetModelValue("IdentityUserId", new ValueProviderResult(i.ToString(),i.ToString(),new CultureInfo("en-US")));

        //    return Json(_userService.Add(organizationUserDomainModel));
        //}

        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult UpdateUser(User user)
        //{
        //    if (user != null && ModelState.IsValid)
        //    {
        //        _userService.Update(user);
        //        return Json(1);
        //    }

        //    return Json(-1);
        //}

        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult DeleteUser(User user)
        //{
        //    if (user != null && ModelState.IsValid)
        //    {
        //        _userService.Delete(user);
        //        return Json(1);
        //    }

        //    return Json(-1);
        //}
    }
}
