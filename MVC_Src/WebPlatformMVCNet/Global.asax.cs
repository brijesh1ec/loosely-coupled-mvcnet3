using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebPlatformMVCNet.Utils;
using Platform.Contracts;

using System.Configuration;

using System.Reflection;
using Spring.Core.IO;
using Spring.Objects.Factory;
using Spring.Objects.Factory.Xml;
using MvcContrib.Spring;
using MvcContrib.ControllerFactories;
using System.Web.Security;
using WebPlatform.Core;
using Spring.Context.Support;

namespace WebPlatformMVCNet
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("Scripts/{*pathInfo}");
            routes.IgnoreRoute("Content/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Authenticate", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }


        public override void Init()
        {
            base.Init();
            //WebApplicationContext webApplicationContext = ContextRegistry.GetContext() as WebApplicationContext;
            //var dr1 = new  WebPlatformMVCNet.Utils.SpringDependencyResolver(webApplicationContext.ObjectFactory);
            //DependencyResolver.SetResolver(dr1);

        }
        //private static IObjectFactory controllersFactory;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            GlobalFilters.Filters.Add(new CompressionFilterAttribute());

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            log4net.Config.XmlConfigurator.Configure();


            IResource daoInput = new FileSystemResource(Server.MapPath("~/Config/DAO.xml"));
            IObjectFactory daoFactory = new XmlObjectFactory(daoInput);

            IResource servicesInput = new FileSystemResource(Server.MapPath("~/Config/Services.xml"));
            IObjectFactory servicesFactory = new XmlObjectFactory(servicesInput, daoFactory);

            IResource controllersInput = new FileSystemResource(Server.MapPath("~/Config/spring-config.xml"));


            DependencyResolver.SetResolver(new WebPlatformMVCNet.Utils.SpringDependencyResolver(new XmlObjectFactory(controllersInput, servicesFactory)));

        }


        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            string cookie = FormsAuthentication.FormsCookieName;
            HttpCookie httpCookie = Context.Request.Cookies[cookie];

            if (httpCookie == null) return;

            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(httpCookie.Value);
            if (ticket == null || ticket.Expired) return;

            FormsIdentity identity = new FormsIdentity(ticket);
            UserData udata = UserData.CreateUserData(ticket.UserData);
            AuthenticationWebPlatformPrincipal principal = new AuthenticationWebPlatformPrincipal(identity, udata);
            Context.User = principal;
        }

    }
}