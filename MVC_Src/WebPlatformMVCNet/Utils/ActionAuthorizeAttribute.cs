using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPlatform.Core;
using WebPlatformMVCNet.Controllers;

namespace WebPlatformMVCNet.Utils
{
    public class ActionAuthorizeAttribute : AuthorizeAttribute
    {
        public Modules Module { get; set; }
        public Functions Permission { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            AuthenticationWebPlatformPrincipal webPlatformPrincipal = filterContext.HttpContext.User as AuthenticationWebPlatformPrincipal;
            if (webPlatformPrincipal == null)
            {
                CloseConnection(filterContext);
                return;
            }

            List<byte> functionIds = new List<byte>();
            foreach (Functions function in Enum.GetValues(typeof(Functions)))
            {
                if ((Permission & function) == function)
                    functionIds.Add((byte)function);
            }

            BaseController controller = filterContext.Controller as BaseController;
            if (controller == null || controller.UserService == null)
            {
                CloseConnection(filterContext);
                return;
            }

            byte moduleId = (byte)Module;

            if (!controller.UserService.IsUserAuthorized(webPlatformPrincipal.UserData.UserID, moduleId, functionIds))
                CloseConnection(filterContext);
        }

        /// <summary>
        /// Closes the connection and sets the status code to Unauthorized
        /// </summary>
        /// <param name="filterContext">The AuthorizationContext</param>
        private void CloseConnection(AuthorizationContext filterContext)
        {
            HttpResponseBase response = filterContext.HttpContext.Response;

            response.StatusCode = 401;//Unauthorized status code
            response.Write("You are not authorized to access this resource.");
            response.Flush();
            response.Close();
            response.End();

            //set the Result in order to prevent any further Filter attributes from executing
            filterContext.Result = new EmptyResult();
        }
    }
}