using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Platform.Contracts;
using WebPlatform.Core;
using WebPlatformMVCNet.Utils;
using WebPlatformMVCNet.GridUtils;

namespace WebPlatformMVCNet.Controllers
{
    public class ProductController : BaseController
    {
        public override IUserService UserService { get; set; }
        public IProductService ProductService { get; set; }
        //
       
        [ActionAuthorize(Module = Modules.Product, Permission = Functions.Access)]
        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetData(GridSettings grid)
        {
            int totalCount = 0;
            List<IProduct> resultList = ProductService.GetAllByFilter(grid, out totalCount).ToList();
            var jsonData = new
            {
                Total = (int)Math.Ceiling((double)totalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = totalCount,
                Rows =
                (from product in resultList
                 select new
                 {
                     ID = product.Id,
                     ProductName = product.ProductName,
                     ProductDescription = product.ProductDescription,
                     UnitPrice = product.UnitPrice
                 }).ToArray()
              
            };

            return Json(jsonData, JsonRequestBehavior.DenyGet);
        }
    }
}
