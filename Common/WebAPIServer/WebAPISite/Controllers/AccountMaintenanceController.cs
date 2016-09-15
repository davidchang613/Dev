using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebAPIModel;

namespace WebAPISite.Controllers
{
    public class AccountMaintenanceController : Controller
    {
        private ApplicationUserManager _userManager;

        // GET: AccountMaintenance
        public ActionResult Index()
        {
            return View();
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [System.Web.Http.AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {

            if (userId == null || code == null)
            {
                return View("Error");
            }

            var result = await UserManager.ConfirmEmailAsync(userId, code);

            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string code, string UserId)
        {
            return code == null || UserId == null ? View("Error") : View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var resultconfirmemail = await UserManager.VerifyUserTokenAsync(model.UserId, "ResetPassword", model.Code);

            var user = await UserManager.FindByIdAsync(model.UserId);

            if (user == null || !resultconfirmemail || user.Email.ToLower() != model.Email.ToLower())
            {
                // Don't reveal that the user does not exist

                ModelState.AddModelError("err", "User information is not valid. Please try again.");
                return View();
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "AccountConfirm");
            }
            AddErrors(result);
            return View();
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}