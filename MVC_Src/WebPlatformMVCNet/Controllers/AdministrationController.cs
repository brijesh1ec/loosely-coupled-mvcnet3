using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Platform.Contracts;
using WebPlatformMVCNet.Utils;
using WebPlatform.Core;
using WebPlatformMVCNet.Models;

namespace WebPlatformMVCNet.Controllers
{
    public class AdministrationController : BaseController
    {
        public override IUserService UserService { get; set; }

        //
        // GET: /Administration/
        [ActionAuthorize(Module = Modules.Administration, Permission = Functions.Access)]
        public ActionResult Index()
        {
            return View();
        }

        [ActionName("Create")]
        [HttpPost]
        [ActionAuthorize(Module = Modules.Administration, Permission = Functions.Access | Functions.ManageUsers)]
        public ActionResult CreateUser(UserModel user)
        {
            if (!ModelState.IsValid)
                return View("Create", user);

            try
            {
                UserService.CreateUser(user.Username, user.Password, user.FirstName, user.LastName, user.Email);
                return View("Create");
            }
            catch (ArgumentException aEx)
            {
                ModelState.AddModelError("Username", aEx.Message);
                return View("Create", user);
            }
        }

        [ActionName("Create")]
        [HttpGet]
        [ActionAuthorize(Module = Modules.Administration, Permission = Functions.Access)]
        public ActionResult LoadCreateUser()
        {
            return View("Create");
        }

        [ActionName("Email")]
        [HttpGet]
        [ActionAuthorize(Module = Modules.Administration, Permission = Functions.Access)]
        public ActionResult LoadChangeEmail()
        {
            AuthenticationWebPlatformPrincipal principal = HttpContext.User as AuthenticationWebPlatformPrincipal;
            var user = UserService.GetById(principal.UserData.UserID);

            return View("Email", new UserModel{ Email = user.Email });
        }

        [ActionName("Email")]
        [HttpPost]
        [ActionAuthorize(Module = Modules.Administration, Permission = Functions.Access | Functions.ManageUsers)]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeEmail(UserModel user)
        {
            if (!ModelState.IsValidField("Email"))
                return View("Email", user);

            AuthenticationWebPlatformPrincipal principal = HttpContext.User as AuthenticationWebPlatformPrincipal;
            
            var usr = UserService.GetById(principal.UserData.UserID);
            usr.Email = user.Email;

            UserService.SaveChanges();

            return View("Index");
        }
    }
}
