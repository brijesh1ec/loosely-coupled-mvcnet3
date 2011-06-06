using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO.Compression;

namespace WebPlatformMVCNet.Utils
{
    public class CompressionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            HttpRequestBase request = filterContext.HttpContext.Request;
            string acceptEncoding = request.Headers["Accept-Encoding"].ToLowerInvariant();
            if (string.IsNullOrEmpty(acceptEncoding))
                return;

            HttpResponseBase response = filterContext.HttpContext.Response;
            if (acceptEncoding.Contains("gzip"))
            {
                response.AppendHeader("Content-Encoding", "gzip");
                response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
            }
            else if (acceptEncoding.Contains("deflate"))
            {
                response.AppendHeader("Content-Encoding", "deflate");
                response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
            }
        }
    }
}