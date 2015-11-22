﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;

namespace Microsoft.AspNet.Mvc
{
    public enum AjaxPerformanceType
    {
        WaterFallFlow,
        Tradition
    }

    public abstract partial class BaseController : Controller
    {
        [NonAction]
        public IActionResult AjaxPagedView<TModel>(
            IEnumerable<TModel> Source,
            string ContentSelector,
            int PageSize = 50,
            AjaxPerformanceType AjaxPerformance = AjaxPerformanceType.WaterFallFlow,
            string PagerDomId = "pager-outer",
            string FormSelector = "form",
            string ViewPath = null)
        {
            if (Request.Query["raw"] == "info")
            {
                var info = Pager.GetPagerInfo(ref Source, PageSize, string.IsNullOrEmpty(Request.Query["p"]) ? 1 : Convert.ToInt32(Request.Query["p"]));
                return Json(info);
            }
            else if (Request.Query["raw"] == "true")
            {
                Pager.PlainDivide(ref Source, PageSize, string.IsNullOrEmpty(Request.Query["p"]) ? 1 : Convert.ToInt32(Request.Query["p"]));
                if (string.IsNullOrEmpty(ViewPath))
                    return View("_" + ControllerContext.ActionDescriptor.Name, Source);
                else
                {
                    var last = ViewPath.LastIndexOf('/');
                    var tmp = ViewPath.Substring(0, last);
                    var tmp2 = ViewPath.Substring(last + 1, ViewPath.Length - 1 - last);
                    return View(tmp + "/_" + tmp2, Source);
                }
            }
            else
            {
                ViewData["__Performance"] = (int)AjaxPerformance;
                ViewData["__PagerDomId"] = PagerDomId;
                ViewData["__ContentSelector"] = ContentSelector;
                ViewData["__FormSelector"] = FormSelector;
                if (string.IsNullOrEmpty(ViewPath))
                    return View();
                else
                    return View(ViewPath);
            }
        }
    }
}
