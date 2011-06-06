using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Security;
using System.Configuration;
using Platform.Contracts;
using WebPlatform.Core;
using log4net;
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace WebPlatformMVCNet.Controllers
{
    public class AuthenticateController : BaseController
    {
        private static readonly ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public override IUserService UserService { get; set; }

        public ActionResult Index()
        {
            return View();
        }


        [AcceptVerbs(HttpVerbs.Post)]
        [HandleError(View = "AuthenticateError")]
        public ActionResult Index(FormCollection collection)
        {
            if (String.IsNullOrEmpty(collection["username"]) || String.IsNullOrEmpty(collection["password"]))
            {
                ViewData["ErrorDetails"] = "The username or password are incorrect.";

                return View();
            }

            var userName = collection["username"];
            var password = collection["password"];

            if (!Authenticate(userName,password))
                return View();

            var logonUser = GetUserByUserName(userName);

            DoLogin(logonUser);

            _log.Info(String.Format("successful login for user with username {0}, at: {1}", userName, DateTime.Now));
            return RedirectToAction("Index", "Administration");

        }

        public ActionResult LogOff()
        {
            this.ExpireCookie(".ASPXAUTH");
            this.ExpireCookie("AccessibleMenus");
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
            _log.Info(String.Format("logout for user with username {0}, at: {1}", (this.ControllerContext.HttpContext.User as AuthenticationWebPlatformPrincipal).UserData.UserName, DateTime.Now));
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Authenticate");
        }

        public ActionResult PasswordRecovery()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PasswordRecovery(string email)
        {
            if (string.IsNullOrEmpty(email) || Regex.Match(email, "^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$") == null)
            {
                ViewData["Error"] = "Please enter a correctly formatted email address.";
                return View();
            }

            try
            {
                UserService.RecoverPasswordByEmail(email);
            }
            catch (ArgumentException aEx)
            {
                ViewData["Error"] = aEx.Message;
            }

            return View();
        }


        #region Helper methods
        [NonAction()]
        private bool Authenticate(string username, string password)
        {
            ViewData["ErrorDetails"] = string.Empty;
            var fieldAuthenticated = true;
            if (String.IsNullOrEmpty(username))
            {
                fieldAuthenticated = false;

            }
            if (String.IsNullOrEmpty(password))
            {
                fieldAuthenticated = false;

            }

            if (!Login(username, password))
            {
                fieldAuthenticated = false;

            }

            if (!fieldAuthenticated)
            {
                ViewData["ErrorDetails"] = "The username or password provided is incorrect or user is logged in.";

            }

            return fieldAuthenticated;
        }

        [NonAction()]
        private IUser GetUserByUserName(string username)
        {
            return UserService.GetByUsername(username);
        }

        [NonAction()]
        private bool Login(string username, string pass)
        {
            if (username == null)
                throw new ArgumentNullException("userName");

            if (username == string.Empty)
                throw new ArgumentException("userName");

            if (pass == null)
                throw new ArgumentNullException("password");

            if (pass == string.Empty)
                throw new ArgumentException("password");


            IUser logonUser = UserService.GetByUsername(username);
            var strDBHash = string.Empty;
            var strDBSalt = string.Empty;
            
            if (logonUser == null) return false;

            strDBSalt = logonUser.Salt;
            WebSecurity.HashWithSalt(pass, ref strDBSalt, out strDBHash);

            if (strDBHash != logonUser.Hash) return false;

            return true;
           
        }

        [NonAction()]
        private void DoLogin(IUser user)
        {

            var timeOut = 400;
            UserData ud = new UserData(user.ID, user.UserName);
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, user.UserName, DateTime.Now, DateTime.Now.AddMinutes(timeOut), false, ud.ToXml());
            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            Response.Cookies.Add(authCookie);
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));//
            InvalidateModulesCookie();
        }
        [NonAction()]
        private void InvalidateModulesCookie()
        {
            HttpCookie cookie = HttpContext.Request.Cookies["AccessibleMenus"];
            if (cookie != null)
            {
                cookie.Expires = new DateTime(1970, 1, 1);
                HttpContext.Response.Cookies.Add(cookie);
            }
        }

        [NonAction()]
        private void ExpireCookie(string cookieName)
        {
            if (Request.Cookies[cookieName] != null)
            {
                HttpCookie cookie = new HttpCookie(cookieName);
                cookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(cookie);
            }
        }
        #endregion       
    }
}
