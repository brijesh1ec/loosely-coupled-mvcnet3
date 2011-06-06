using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Platform.Contracts;
using WebPlatform.Core;

namespace WebPlatformMVCNet.Controllers
{
    public abstract class BaseController : Controller
    {
        //[Dependency]
        public abstract IUserService UserService { get; set; }

        private const string cookieName = "AccessibleMenus";

        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.HttpContext.IsCustomErrorEnabled)
            {
                //just sample logging
                log4net.ILog log = log4net.LogManager.GetLogger(GetType());
                log.Error(filterContext.Exception.Message, filterContext.Exception);
                filterContext.ExceptionHandled = true;
                ViewData["ErrorMessage"] = filterContext.Exception.Message;

                string errorView = ((System.Web.Mvc.ViewResult)(filterContext.Result)).ViewName ?? "Error";
                this.View(errorView).ExecuteResult(this.ControllerContext);
            }
        }
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            HttpCookie cookie = requestContext.HttpContext.Request.Cookies[cookieName];
            if (cookie != null)
            {
                ViewData[cookieName] = Array.ConvertAll(cookie.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), m => Convert.ToByte(m)).ToList();
                return;
            }

            AuthenticationWebPlatformPrincipal webPlatformPrincipal = requestContext.HttpContext.User as AuthenticationWebPlatformPrincipal;
            if (webPlatformPrincipal == null)
                return;

            IList<byte> modules = this.GetAccessableModuleNames(webPlatformPrincipal.UserData.UserID);
            ViewData[cookieName] = modules;

            cookie = new HttpCookie(cookieName, string.Join(",", modules.ToArray()));
            cookie.Expires = DateTime.Now.AddMinutes(20);
            requestContext.HttpContext.Response.Cookies.Add(cookie);

        }


        [NonAction()]
        private IList<byte> GetAccessableModuleNames(int userId)
        {
            try
            {
                IList<byte> modNames = new List<byte>();
                foreach (var module in Enum.GetValues(typeof(Modules)))
                {
                    if (this.HasUserAccessToModule(userId, Convert.ToByte(module)))
                        modNames.Add(Convert.ToByte(module));
                }

                return modNames;
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        [NonAction()]
        private bool HasUserAccessToModule(int userId, byte moduleId)
        {

            return UserService.HasUserAccessToModule(userId, moduleId);
        }
    }

}
