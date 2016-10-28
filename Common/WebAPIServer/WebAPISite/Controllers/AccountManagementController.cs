using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebAPIModel;

namespace WebAPISite.Controllers
{
    [Authorize]
    [RoutePrefix("api/AccountManagement")]
    public class AccountManagementController : BaseAccountController
    {
        //
        // POST: /Manage/ChangePassword
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("model is not valid");
            }

            var user = await this.UserManager.FindByEmailAsync(model.Email);

            var result = await this.UserManager.ChangePasswordAsync(user.Id, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return Ok(new { Message ="Your password has been changed." });
            }
            else
            {
                this.GetErrorResult(result);
            }
            return Ok();
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("AddPhoneNumber")]
        public async Task<IHttpActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model");
            }

            var user = await this.UserManager.FindByEmailAsync(model.Email);

            // Generate the token and send it
            var code = await this.UserManager.GenerateChangePhoneNumberTokenAsync(user.Id, model.Number);
            if (this.UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await this.UserManager.SmsService.SendAsync(message);
            }
            return Ok(new { PhoneNumber = model.Number }); // RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("GetUserData")]
        public async Task<IHttpActionResult> GetUserData(LoginInfoViewModel model)
        {

            var userId = this.SignInManager.GetVerifiedUserId();
            var user = await this.UserManager.FindByEmailAsync(model.Email);

            return Ok(new ManageProfileViewModel
            {
                username = user.Email,
                PhoneNumber = user.PhoneNumber,
                TwoFactor = user.TwoFactorEnabled
            }); // RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("VerifyPhoneNumber")]
        public async Task<IHttpActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model");
            }

            var user = await this.UserManager.FindByEmailAsync(model.Email);

            var res = await this.UserManager.ChangePhoneNumberAsync(user.Id, model.PhoneNumber, model.Code);
            if (res.Succeeded)
            {
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: true, rememberBrowser: false);

                }

                return Ok(user);
            }
            else
            {
                ModelState.AddModelError("", "Failed to verify phone");
                return this.GetErrorResult(res);
            }
            // If we got this far, something failed, redisplay form

            //return Ok(model);//View(model);
        }
    }

}
