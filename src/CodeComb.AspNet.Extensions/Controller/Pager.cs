﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Dnx.Runtime;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Framework.DependencyInjection;

namespace Microsoft.AspNet.Mvc
{
    public class PagerInfo
    {
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public long Count { get; set; }
    }

    public static class Pager
    {
        public static PagerInfo GetPagerInfo<T>(ref IEnumerable<T> src, int PageSize = 50, int Page = 1)
        {
            var ret = new PagerInfo();
            ret.Count = src.LongCount();
            ret.PageCount = Convert.ToInt32((src.LongCount() + PageSize - 1) / PageSize);
            ret.PageSize = PageSize;
            ret.Start = (Page - PageSize) < 1 ? 1 : (Page - PageSize);
            ret.End = (ret.Start + 10) > ret.PageCount ? ret.PageCount : (ret.Start + 10);
            if (ret.End < ret.Start) ret.End = ret.Start;
            if (ret.PageCount == 0) ret.PageCount = 1;
            return ret;
        }

        public static void PlainDivide<T>(ref IEnumerable<T> src, int PageSize = 50, int Page = 1)
        {
            src = src.Skip((Page - 1) * PageSize).Take(PageSize).ToList();
        }

        public static PagerInfo Divide<T>(ref IEnumerable<T> src, int PageSize = 50, int Page = 1)
        {
            var ret = new PagerInfo();
            ret.Count = src.LongCount();
            ret.PageCount = Convert.ToInt32((src.LongCount() + PageSize - 1) / PageSize);
            ret.PageSize = PageSize;
            ret.Start = (Page - PageSize) < 1 ? 1 : (Page - PageSize);
            ret.End = (ret.Start + 10) > ret.PageCount ? ret.PageCount : (ret.Start + 10);
            if (ret.End < ret.Start) ret.End = ret.Start;
            if (ret.PageCount == 0) ret.PageCount = 1;
            src = src.Skip((Page - 1) * PageSize).Take(PageSize).ToList();
            return ret;
        }
    }
}

namespace Microsoft.AspNet.Mvc.Rendering
{
    public static class PagerHtmlHelper
    {
        public static HtmlString MakePager(this IHtmlHelper self, string PlainClass = "pager-item", string ActiveClass = "active", string OuterClass = "pager-outer", string PageNumberFormat = null, string PagerInfo = "PagerInfo")
        {
            StringBuilder ret = new StringBuilder();
            if (self.ViewContext.ViewData["__Performance"] != null && Convert.ToInt32(self.ViewContext.ViewData["__Performance"]) == 1)
            {
                ret.AppendLine("<ul id=\"" + self.ViewContext.ViewData["__PagerDomId"] + "\" class=\"" + OuterClass + "\" data-plain-class=\"" + PlainClass + "\" data-active-class=\"" + ActiveClass + "\">");
                ret.AppendLine("</ul>");
                ret.AppendLine("<script>");
                ret.AppendLine("    var __contentSelector = '" + self.ViewContext.ViewData["__ContentSelector"] + "';");
                ret.AppendLine("    var __performance = '" + self.ViewContext.ViewData["__Performance"] + "';");
                ret.AppendLine("    var __pagerSelector = '#" + self.ViewContext.ViewData["__PagerDomId"] + "';");
                ret.AppendLine("    var __formSelector = '" + self.ViewContext.ViewData["__FormSelector"] + "';");
                ret.AppendLine("    var __url = '" + self.ViewContext.HttpContext.Request.Path.Value + "';");
                ret.AppendLine("    __CodeCombAjaxEvents[__url] = {};");
                ret.AppendLine("</script>");
            }
            else if (self.ViewContext.ViewData["__Performance"] != null && Convert.ToInt32(self.ViewContext.ViewData["__Performance"]) == 0)
            {
                ret.AppendLine("<script>");
                ret.AppendLine("    var __contentSelector = '" + self.ViewContext.ViewData["__ContentSelector"] + "';");
                ret.AppendLine("    var __performance = '" + self.ViewContext.ViewData["__Performance"] + "';");
                ret.AppendLine("    var __pagerSelector = '#" + self.ViewContext.ViewData["__PagerDomId"] + "';");
                ret.AppendLine("    var __formSelector = '" + self.ViewContext.ViewData["__FormSelector"] + "';");
                ret.AppendLine("    var __url = '" + self.ViewContext.HttpContext.Request.Path.Value + "';");
                ret.AppendLine("    __CodeCombAjaxEvents[__url] = {};");
                ret.AppendLine("</script>");
            }
            else
            {
                var httpContextAccessor = self.ViewContext.HttpContext.RequestServices.GetService<IHttpContextAccessor>();
                IDictionary<string, object> RouteValueTemplate = new Dictionary<string, object>();
                foreach (var q in httpContextAccessor.HttpContext.Request.Query)
                {
                    var str = "";
                    foreach (var s in q.Value)
                        str += s + ", ";
                    RouteValueTemplate[q.Key] = str.TrimEnd(' ').TrimEnd(',');
                }
                var CurrentPage = httpContextAccessor.HttpContext.Request.Query["p"] != null ? int.Parse(httpContextAccessor.HttpContext.Request.Query["p"].ToString()) : 1;
                var tmp = (PagerInfo)self.ViewData[PagerInfo];
                ret.AppendLine("<ul class=\"" + OuterClass + "\">");
                RouteValueTemplate["p"] = "1";
                ret.AppendLine("<li class=\"" + PlainClass + "\">" + (self.ActionLink("«", self.ViewContext.RouteData.Values["action"].ToString(), self.ViewContext.RouteData.Values["controller"].ToString(), RouteValueTemplate, null) as TagBuilder).ToHtmlString() + "</li>");
                for (var i = tmp.Start; i <= tmp.End; i++)
                {
                    RouteValueTemplate["p"] = i.ToString();
                    if (CurrentPage == i)
                    {
                        ret.AppendLine("<li class=\"" + PlainClass + " " + ActiveClass + "\">" + (self.ActionLink(
                            PageNumberFormat == null ? i.ToString() : i.ToString(PageNumberFormat),
                            self.ViewContext.RouteData.Values["action"].ToString(),
                            self.ViewContext.RouteData.Values["controller"].ToString(),
                            RouteValueTemplate,
                            null) as TagBuilder).ToHtmlString() + "</li>");
                    }
                    else
                    {
                        ret.AppendLine("<li class=\"" + PlainClass + "\">" + (self.ActionLink(
                            PageNumberFormat == null ? i.ToString() : i.ToString(PageNumberFormat),
                            self.ViewContext.RouteData.Values["action"].ToString(),
                            self.ViewContext.RouteData.Values["controller"].ToString(),
                            RouteValueTemplate,
                            null) as TagBuilder).ToHtmlString() + "</li>");
                    }
                }
                RouteValueTemplate["p"] = tmp.PageCount.ToString();
                ret.AppendLine("<li class=\"" + PlainClass + "\">" + (self.ActionLink(
                     "»",
                     self.ViewContext.RouteData.Values["action"].ToString(),
                     self.ViewContext.RouteData.Values["controller"].ToString(),
                     RouteValueTemplate,
                     null) as TagBuilder).ToHtmlString() + "</li>");
                ret.AppendLine("</ul>");
            }
            return new HtmlString(ret.ToString());
        }
    }
}