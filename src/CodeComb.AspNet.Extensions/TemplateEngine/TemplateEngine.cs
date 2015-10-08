using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewFeatures;
using Microsoft.AspNet.Mvc.Routing;
using Microsoft.AspNet.Mvc.Razor;
using Microsoft.AspNet.Mvc.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.OptionsModel;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.ViewEngines;

namespace CodeComb.AspNet.TemplateEngine
{
    public class CodeCombRazorViewEngine : IRazorViewEngine
    {
        private const string ViewExtension = ".cshtml";
        internal const string ControllerKey = "controller";
        internal const string AreaKey = "area";

        private static readonly IEnumerable<string> _viewLocationFormats = new[]
        {
            "/Views/{1}/{0}" + ViewExtension,
            "/Views/Shared/{0}" + ViewExtension,
            "/Views/{template}/{1}/{0}" + ViewExtension,
            "/Views/{template}/Shared/{0}" + ViewExtension,
            "/Templates/{1}/{0}" + ViewExtension,
            "/Templates/Shared/{0}" + ViewExtension,
            "/Templates/{template}/{1}/{0}" + ViewExtension,
            "/Templates/{template}/Shared/{0}" + ViewExtension,
        };

        private static readonly IEnumerable<string> _areaViewLocationFormats = new[]
        {
            "/Areas/{2}/Views/{1}/{0}" + ViewExtension,
            "/Areas/{2}/Views/Shared/{0}" + ViewExtension,
            "/Views/Shared/{0}" + ViewExtension,
            "/Areas/{2}/Views/{template}/{1}/{0}" + ViewExtension,
            "/Areas/{2}/Views/{template}/Shared/{0}" + ViewExtension,
            "/Views/{template}/Shared/{0}" + ViewExtension,
            "/Areas/{2}/Templates/{1}/{0}" + ViewExtension,
            "/Areas/{2}/Templates/Shared/{0}" + ViewExtension,
            "/Templates/Shared/{0}" + ViewExtension,
            "/Areas/{2}/Templates/{template}/{1}/{0}" + ViewExtension,
            "/Areas/{2}/Templates/{template}/Shared/{0}" + ViewExtension,
            "/Templates/{template}/Shared/{0}" + ViewExtension,
        };

        private readonly IRazorPageFactory _pageFactory;
        private readonly IRazorViewFactory _viewFactory;
        private readonly IList<IViewLocationExpander> _viewLocationExpanders;
        private readonly IViewLocationCache _viewLocationCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="RazorViewEngine" /> class.
        /// </summary>
        /// <param name="pageFactory">The page factory used for creating <see cref="IRazorPage"/> instances.</param>
        public CodeCombRazorViewEngine(IRazorPageFactory pageFactory,
                               IRazorViewFactory viewFactory,
                               IOptions<RazorViewEngineOptions> optionsAccessor,
                               IViewLocationCache viewLocationCache)
        {
            _pageFactory = pageFactory;
            _viewFactory = viewFactory;
            _viewLocationExpanders = optionsAccessor.Value.ViewLocationExpanders;
            _viewLocationCache = viewLocationCache;
        }

        public virtual IEnumerable<string> GenerateViewLocationFormats(string template)
        {
            var ret = new List<string>();
            foreach (var x in ViewLocationFormats)
                ret.Add(x.Replace("{template}", template));
            return ret;
        }

        public virtual IEnumerable<string> GenerateAreaViewLocationFormats(string template)
        {
            var ret = new List<string>();
            foreach (var x in AreaViewLocationFormats)
                ret.Add(x.Replace("{template}", template));
            return ret;
        }

        public virtual IEnumerable<string> ViewLocationFormats
        {
            get { return _viewLocationFormats; }
        }


        public virtual IEnumerable<string> AreaViewLocationFormats
        {
            get { return _areaViewLocationFormats; }
        }

        /// <inheritdoc />
        public ViewEngineResult FindView(
            ActionContext context,
            string viewName)
        {
            var templateService = context.HttpContext.ApplicationServices.GetService<Template>();
            var template = "Default";
            if (templateService != null)
            {
                template = context.HttpContext.Request.Cookies["_template"];
                if (template == null)
                    template = templateService.DefaultTemplate;
            }
            viewName = viewName.Replace("{template}", template);
            if (string.IsNullOrEmpty(viewName))
            {
                throw new ArgumentException();
            }

            var pageResult = GetRazorPageResult(context, viewName, isPartial: false);
            return CreateViewEngineResult(pageResult, _viewFactory, isPartial: false);
        }

        /// <inheritdoc />
        public ViewEngineResult FindPartialView(ActionContext context,
                                                string partialViewName)
        {
            var templateService = context.HttpContext.ApplicationServices.GetService<Template>();
            var template = "Default";
            if (templateService != null)
            {
                template = context.HttpContext.Request.Cookies["_template"];
                if (template == null)
                    template = templateService.DefaultTemplate;
            }
            partialViewName = partialViewName.Replace("{template}", template);
            if (string.IsNullOrEmpty(partialViewName))
            {
                throw new ArgumentException();
            }

            var pageResult = GetRazorPageResult(context, partialViewName, isPartial: true);
            return CreateViewEngineResult(pageResult, _viewFactory, isPartial: true);
        }

        /// <inheritdoc />
        public RazorPageResult FindPage(ActionContext context,
                                        string pageName)
        {
            var templateService = context.HttpContext.ApplicationServices.GetService<Template>();
            var template = "Default";
            if (templateService != null)
            {
                template = context.HttpContext.Request.Cookies["_template"];
                if (template == null)
                    template = templateService.DefaultTemplate;
            }
            pageName = pageName.Replace("{template}", template);
            if (string.IsNullOrEmpty(pageName))
            {
                throw new ArgumentException();
            }

            return GetRazorPageResult(context, pageName, isPartial: true);
        }

        internal static string GetNormalizedRouteValue(ActionContext context, string key)
        {
            object routeValue;
            if (!context.RouteData.Values.TryGetValue(key, out routeValue))
            {
                return null;
            }

            var actionDescriptor = context.ActionDescriptor;
            string normalizedValue = null;
            if (actionDescriptor.AttributeRouteInfo != null)
            {
                object match;
                if (actionDescriptor.RouteValueDefaults.TryGetValue(key, out match))
                {
                    normalizedValue = match?.ToString();
                }
            }
            else
            {
                // For traditional routes, lookup the key in RouteConstraints if the key is RequireKey.
                var match = actionDescriptor.RouteConstraints.FirstOrDefault(
                    constraint => string.Equals(constraint.RouteKey, key, StringComparison.OrdinalIgnoreCase));
                if (match != null && match.KeyHandling != RouteKeyHandling.CatchAll)
                {
                    if (match.KeyHandling == RouteKeyHandling.DenyKey)
                    {
                        return null;
                    }

                    normalizedValue = match.RouteValue;
                }
            }

            var stringRouteValue = routeValue?.ToString();
            if (string.Equals(normalizedValue, stringRouteValue, StringComparison.OrdinalIgnoreCase))
            {
                return normalizedValue;
            }

            return stringRouteValue;
        }

        private RazorPageResult GetRazorPageResult(ActionContext context,
                                                   string pageName,
                                                   bool isPartial)
        {
            if (IsApplicationRelativePath(pageName))
            {
                var applicationRelativePath = pageName;
                if (!pageName.EndsWith(ViewExtension, StringComparison.OrdinalIgnoreCase))
                {
                    applicationRelativePath += ViewExtension;
                }

                var page = _pageFactory.CreateInstance(applicationRelativePath);
                if (page != null)
                {
                    return new RazorPageResult(pageName, page);
                }

                return new RazorPageResult(pageName, new[] { pageName });
            }
            else
            {
                return LocatePageFromViewLocations(context, pageName, isPartial);
            }
        }

        private RazorPageResult LocatePageFromViewLocations(ActionContext context,
                                                            string pageName,
                                                            bool isPartial)
        {
            var templateService = context.HttpContext.ApplicationServices.GetService<Template>();
            var template = "Default";
            if (templateService != null)
            {
                template = context.HttpContext.Request.Cookies["_template"];
                if (template == null)
                    template = templateService.DefaultTemplate;
            }

            // Initialize the dictionary for the typical case of having controller and action tokens.
            var areaName = GetNormalizedRouteValue(context, AreaKey);

            // Only use the area view location formats if we have an area token.
            var viewLocations = !string.IsNullOrEmpty(areaName) ? GenerateAreaViewLocationFormats(template) :
                                                                  GenerateViewLocationFormats(template);

            var expanderContext = new ViewLocationExpanderContext(context, pageName, isPartial);
            if (_viewLocationExpanders.Count > 0)
            {
                expanderContext.Values = new Dictionary<string, string>(StringComparer.Ordinal);

                // 1. Populate values from viewLocationExpanders.
                foreach (var expander in _viewLocationExpanders)
                {
                    expander.PopulateValues(expanderContext);
                }
            }

            // 2. With the values that we've accumumlated so far, check if we have a cached result.
            var pageLocation = _viewLocationCache.Get(expanderContext);
            if (!string.IsNullOrEmpty(pageLocation.ViewLocation))
            {
                var page = _pageFactory.CreateInstance(pageLocation.ViewLocation);

                if (page != null)
                {
                    // 由于多模板的机制，在这里不能使用2a的缓存机制
                    // 2a. We found a IRazorPage at the cached location.
                    //return new RazorPageResult(pageName, page);
                }
            }

            // 2b. We did not find a cached location or did not find a IRazorPage at the cached location.
            // The cached value has expired and we need to look up the page.
            foreach (var expander in _viewLocationExpanders)
            {
                viewLocations = expander.ExpandViewLocations(expanderContext, viewLocations);
            }

            // 3. Use the expanded locations to look up a page.
            var controllerName = GetNormalizedRouteValue(context, ControllerKey);
            var searchedLocations = new List<string>();
            foreach (var path in viewLocations)
            {
                var transformedPath = string.Format(CultureInfo.InvariantCulture,
                                                    path,
                                                    pageName,
                                                    controllerName,
                                                    areaName);
                var page = _pageFactory.CreateInstance(transformedPath);
                if (page != null)
                {
                    // 3a. We found a page. Cache the set of values that produced it and return a found result.
                    _viewLocationCache.Set(expanderContext, new ViewLocationCacheResult(path, searchedLocations));
                    return new RazorPageResult(pageName, page);
                }

                searchedLocations.Add(transformedPath);
            }

            // 3b. We did not find a page for any of the paths.
            return new RazorPageResult(pageName, searchedLocations);
        }

        private ViewEngineResult CreateViewEngineResult(RazorPageResult result,
                                                        IRazorViewFactory razorViewFactory,
                                                        bool isPartial)
        {
            if (result.SearchedLocations != null)
            {
                return ViewEngineResult.NotFound(result.Name, result.SearchedLocations);
            }

            var view = razorViewFactory.GetView(this, result.Page, isPartial);
            return ViewEngineResult.Found(result.Name, view);
        }

        private static bool IsApplicationRelativePath(string name)
        {
            Debug.Assert(!string.IsNullOrEmpty(name));
            return name[0] == '~' || name[0] == '/';
        }
    }
}
