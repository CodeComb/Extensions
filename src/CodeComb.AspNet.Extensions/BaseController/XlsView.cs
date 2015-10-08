using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;

namespace Microsoft.AspNet.Mvc
{
    public abstract partial class BaseController : Controller
    {
        [NonAction]
        protected IActionResult XlsView(object Model = null, string FileName = null, string ViewPath = null)
        {
            if (string.IsNullOrEmpty(FileName))
                FileName = DateTime.Now.ToTimeStamp().ToString() + ".xls";
            Response.Headers.Add("content-disposition", new string[] { "attachment;filename=\"" + FileName + "\"" });
            Response.ContentType = "application/x-xls";
            if (string.IsNullOrEmpty(ViewPath))
            {
                if (Model == null)
                    return View();
                else
                    return View(Model);
            }
            else
            {
                if (Model == null)
                    return View(ViewPath);
                else
                    return View(ViewPath, Model);
            }
        }
    }
}
