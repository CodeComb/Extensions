﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;

namespace Microsoft.AspNet.Builder
{
    public static class AutoAjaxExtensions
    {
        public static IApplicationBuilder UseAutoAjax(this IApplicationBuilder self, string Path = "/scripts/jquery.autoajax.js")
        {
            return self.Map(Path, app =>
            {
                app.Run(async context => {
                    await context.Response.WriteAsync(@"// jquery.autoajax.js
$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};

var __page = 1;
var __lock = false;
var __pageCount = 1;
var __paramsCache = {};
var __CodeCombAjaxEvents = {};
__CodeCombAjaxEvents['All'] = {};
// __CodeCombAjaxEvents['Home/Index'].onLoading = function () {};
// __CodeCombAjaxEvents['Home/Index'].onLoaded = function () {};
// __CodeCombAjaxEvents['Home/Index'].onNoMore = function () {};

function __movePage(page) {
    if (__page > __pageCount || __page < 1) return;
    if (__lock) return;
    __page = page;
    __Load('__movePage');
}

function __Load(sender) {
    if (__lock) return;
    __lock = true;
    try { __CodeCombAjaxEvents['All'].onLoading(); } catch (e) { };
    try { __CodeCombAjaxEvents['Home/Index'].onLoading(); } catch (e) { };
    var __pagerInfoParams;
    if (typeof (sender) == 'undefined') {
        __paramsCache = $(__formSelector).serializeObject();
        __paramsCache.raw = 'true';
    }
    __paramsCache.p = __page;
    __paramsCache.raw = 'info';
    $.getJSON(__url, __paramsCache, function (result) {
        __pageCount = result.PageCount;
        if (__performance == 1) {
            var __plainClass = $(__pagerSelector).attr('data-plain-class');
            var __activeClass = $(__pagerSelector).attr('data-active-class');
            $(__pagerSelector).html('');
            $(__pagerSelector).append('<li class=' + __plainClass + '><a href=\'javascript: __movePage(1);\'>«</a></li>');
            for (var i = result.Start; i <= result.End; i++) {
                if (i != __page)
                    $(__pagerSelector).append('<li class=' + __plainClass + '><a href=\'javascript: __movePage(' + i + ');\'>' + i + '</a></li>');
                else
                    $(__pagerSelector).append('<li class=' + __plainClass + ' ' + __activeClass + '><a href=\'javascript: __movePage(' + i + ');\'>' + i + '</a></li>');
            }
            $(__pagerSelector).append('<li class=' + __plainClass + '><a href=\'javascript: __movePage(' + (__pageCount == 0 ? 1 : __pageCount) + ');\'>»</a></li>');
        }
        __paramsCache.raw = 'true';
        if (__performance == 1)
            $.get(__url, __paramsCache, function (html) {
                $(__contentSelector).html(html);
                try { __CodeCombAjaxEvents['All'].onLoaded(); } catch (e) { };
                try { __CodeCombAjaxEvents['Home/Index'].onLoaded(); } catch (e) { };
                __lock = false;
            });
        else
            $.get(__url, __paramsCache, function (html) {
                $(__contentSelector).append(html);
                try { __CodeCombAjaxEvents['All'].onLoaded(); } catch (e) { };
                try { __CodeCombAjaxEvents['Home/Index'].onLoaded(); } catch (e) { };
                __lock = false;
                __page++;
                if (__page > __pageCount) {
                    __lock = true;
                    try { __CodeCombAjaxEvents['All'].onNoMore(); } catch (e) { };
                    try { __CodeCombAjaxEvents['Home/Index'].onNoMore(); } catch (e) { };
                }
            });
    });
}

$(document).ready(function () {
    if (typeof (__contentSelector) != 'undefined' && $(__contentSelector).length > 0) {
        if (__performance == 0) {
            $(window).scroll(function () {
                totalheight = parseFloat($(window).height()) + parseFloat($(window).scrollTop());
                if ($(document).height() <= totalheight) {
                    __Load();
                }
            });
        }
        __Load();
    }
});");
                });
            });
        }
    }
}
