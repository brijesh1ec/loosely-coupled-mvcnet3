using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Web;
using System.Web.Routing;
using MvcContrib.TestHelper;

namespace WebPlatformUnitTests
{
    [TestFixture]
    public class RoutingTests
    {

        [Test]
        [Description("Verifies that on requesting Scripts folder the ignore route will be returned using MvcContrib.TestHelper")]
        public void IgnoreRouteWithTestHelper()
        {

            RouteTable.Routes.Clear();
            WebPlatformMVCNet.MvcApplication.RegisterRoutes(RouteTable.Routes);

            "~/Scripts/somejsFile.js".ShouldBeIgnored();
        }

        [Test]
        [Description("Verifies that empty url root matches Authenticate/Index (Authenticate controller, Index() method) using MvcContrib.TestHelper")]
        public void DefaultRouteWithTestHelper()
        {
            RouteTable.Routes.Clear();
            WebPlatformMVCNet.MvcApplication.RegisterRoutes(RouteTable.Routes);

            "~/".ShouldMapTo<WebPlatformMVCNet.Controllers.AuthenticateController>(c => c.Index());
        }
    }
}
