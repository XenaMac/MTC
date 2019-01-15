using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FSP.Common;
using FSP.Web.Filters;
using FSP.Web.Models;
using System.Net.Mail;
using System.Configuration;
using System.Net;
using FSP.Web.Helpers;

namespace FSP.Web.Controllers
{
    public class AccountController : Controller
    {
       

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return ContextDependentView();
        }
       
        [AllowAnonymous]
        [HttpPost]
        public JsonResult JsonLogin(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.Email, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.Email, model.RememberMe);
                    return Json(new { success = true, redirect = returnUrl });
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed
            return Json(new { errors = GetErrorsFromModelState() });
        }
       
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.Email, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.Email, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Either the user name or password provided is incorrect or your account has not been yet approved.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        
        [AllowAnonymous]
        public ActionResult Register()
        {
            return ContextDependentView();
        }
       
        [AllowAnonymous]
        [HttpPost]
        public ActionResult JsonRegister(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.Email, model.Password, model.Email, passwordQuestion: null, passwordAnswer: null, isApproved: true, providerUserKey: null, status: out createStatus);
                //((FSPMembershipProvider)Membership.Provider).CreateFSPUser(userId, user.Email, "", RoleID, user.FirstName, user.LastName, user.Address, user.City, user.State, user.Zip, user.PhoneNumber, true, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsAuthentication.SetAuthCookie(model.Email, createPersistentCookie: false);
                    return Json(new { success = true });
                }
                else
                {
                    ModelState.AddModelError("", Helpers.Util.ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed
            return Json(new { errors = GetErrorsFromModelState() });
        }
     
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                Guid userId = Guid.NewGuid();

                // Attempt to register the user
                MembershipCreateStatus createStatus;

                ((FSPMembershipProvider)Membership.Provider).CreateFSPUser(userId, model.Email, model.Password, Guid.Empty, false, out createStatus, model.FirstName, model.LastName);

                if (createStatus == MembershipCreateStatus.Success)
                {

                    String subject = "New User Registration";
                    String body = String.Empty;
                    body += "Hello, " + ConfigurationManager.AppSettings["OCTAAdminName"] + ": </br></br>";
                    body += "\"" + model.FirstName + " " + model.LastName + "\" has just registered for the <a href='http://latatrax.com/octafsp/' target='_blank'>OCTA FSP web site</a>. " + "</br></br>";
                    body += "Please, view the <a href='http://latatrax.com/octafsp/AdminArea/Users/' target='_blank'>users page</a> to manage user accounts and access.</br></br></br></br>";
                    body += "-LATA Trax Email System";

                    Util.SendEmail(subject, body);

                    return RedirectToAction("AwaitingApproval", "Account");
                }
                else
                {
                    ModelState.AddModelError("", Helpers.Util.ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

      

        public ActionResult ChangePassword()
        {
            return View();
        }
      
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, userIsOnline: true);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

       
        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        private ActionResult ContextDependentView()
        {
            string actionName = ControllerContext.RouteData.GetRequiredString("action");
            if (Request.QueryString["content"] != null)
            {
                ViewBag.FormAction = "Json" + actionName;
                return PartialView();
            }
            else
            {
                ViewBag.FormAction = actionName;
                return View();
            }
        }

        private IEnumerable<string> GetErrorsFromModelState()
        {
            return ModelState.SelectMany(x => x.Value.Errors.Select(error => error.ErrorMessage));
        }

        public ActionResult AwaitingApproval()
        {
            return View();
        }


    }
}
