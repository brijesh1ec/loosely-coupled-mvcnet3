using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Platform.Contracts;

namespace WebPlatformMVCNet.Controllers
{
    public class OrderController : BaseController
    {
        public override IUserService UserService { get; set; }

        //
        // GET: /Invoice/

        public ActionResult Index()
        {
            return View();
        }
    }
}
