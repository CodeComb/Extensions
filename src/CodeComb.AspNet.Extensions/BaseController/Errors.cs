using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNet.Http;

namespace Microsoft.AspNet.Mvc
{
    public abstract partial class BaseController : Controller
    {
        [NonAction]
        protected IActionResult Error(int Code, string ViewName = "Error", string Title = null, string Message = "", string ReturnUrl = null)
        {
            if (string.IsNullOrEmpty(ReturnUrl))
            {
                ReturnUrl = Request.Headers["Referer"].FirstOrDefault() ?? "/";
            }
            if (string.IsNullOrEmpty(Title))
            {
                switch (Code)
                {
                    case 500:
                        ViewBag.Title = "服务器异常";
                        break;
                    case 414:
                    case 413:
                        ViewBag.Title = "请求内容过大";
                        break;
                    case 411:
                        ViewBag.Title = "需要代理验证";
                        break;
                    case 410:
                        ViewBag.Title = "资源已被删除";
                        break;
                    case 408:
                        ViewBag.Title = "请求超时";
                        break;
                    case 407:
                        ViewBag.Title = "需要代理验证";
                        break;
                    case 406:
                        ViewBag.Title = "内容特性不被接受";
                        break;
                    case 405:
                        ViewBag.Title = "不允许使用该动作";
                        break;
                    case 404:
                        ViewBag.Title = "没有找到相关资源";
                        break;
                    case 403:
                        ViewBag.Title = "禁止访问";
                        break;
                    case 401:
                        ViewBag.Title = "未获得授权";
                        break;
                    case 400:
                        ViewBag.Title = "错误的请求";
                        break;
                    default:
                        ViewBag.Title = "错误";
                        break;
                }
            }
            else
            {
                ViewBag.Title = Title;
            }
            Response.StatusCode = Code;
            ViewData["Message"] = Message;
            ViewData["ReturnUrl"] = ReturnUrl;
            return View(ViewName);
        }

        [NonAction]
        [Obsolete]
        protected IActionResult TemplatedError(int Code, string ViewPath = "~/Views/{template}/Home/Error", string Title = null, string Message = "", string ReturnUrl = null)
        {
            if (string.IsNullOrEmpty(ReturnUrl))
            {
                ReturnUrl = Request.Headers["Referer"].FirstOrDefault() ?? "/";
            }
            if (string.IsNullOrEmpty(Title))
            {
                switch (Code)
                {
                    case 500:
                        ViewBag.Title = "服务器异常";
                        break;
                    case 414:
                    case 413:
                        ViewBag.Title = "请求内容过大";
                        break;
                    case 411:
                        ViewBag.Title = "需要代理验证";
                        break;
                    case 410:
                        ViewBag.Title = "资源已被删除";
                        break;
                    case 408:
                        ViewBag.Title = "请求超时";
                        break;
                    case 407:
                        ViewBag.Title = "需要代理验证";
                        break;
                    case 406:
                        ViewBag.Title = "内容特性不被接受";
                        break;
                    case 405:
                        ViewBag.Title = "不允许使用该动作";
                        break;
                    case 404:
                        ViewBag.Title = "没有找到相关资源";
                        break;
                    case 403:
                        ViewBag.Title = "禁止访问";
                        break;
                    case 401:
                        ViewBag.Title = "未获得授权";
                        break;
                    case 400:
                        ViewBag.Title = "错误的请求";
                        break;
                    default:
                        ViewBag.Title = "错误";
                        break;
                }
            }
            else
            {
                ViewBag.Title = Title;
            }
            Response.StatusCode = Code;
            ViewData["Message"] = Message;
            ViewData["ReturnUrl"] = ReturnUrl;
            ViewPath = ViewPath.Replace("{template}", Cookies["_template"] ?? Template.DefaultTemplate);
            return View(ViewPath);
        }
    }
}
