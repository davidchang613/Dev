using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using Microsoft.AspNet.Identity.Owin;
using System.Net.Http;

namespace WebAPISite.Attributes
{
    public class AuthorizeUserRoleAttribute : AuthorizeAttribute
    {

        //private AuthorizeUserRoleAttribute
        private ApplicationUserManager _UserManager = null;

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            bool isUserInRole = false;

            if (base.IsAuthorized(actionContext))
            {
                IPrincipal principal = actionContext.RequestContext.Principal;

                _UserManager = actionContext.Request.GetOwinContext().GetUserManager<ApplicationUserManager>();

                var userId = _UserManager.FindByEmailAsync(principal.Identity.Name).Result.Id;

                var y = Task.Run(async () => await _UserManager.GetRolesAsync(userId)).Result;

                return y.ToList().Where(p => p.ToString().Contains("TwoFactorVerified")).Count() == 0 ? false : true;

            }
            return isUserInRole;
        }
    }
}
