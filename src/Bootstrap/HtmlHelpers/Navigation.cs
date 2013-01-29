using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using NavigationRoutes;

namespace BootstrapSupport.HtmlHelpers
{
    public static class Navigation
    {
        public static IHtmlString Breadcrumbs(this HtmlHelper helper)
        {
            var namedRoute = helper.ViewContext.RouteData.Route as NamedRoute;
            if (namedRoute != null)
            {
                var breadcrumbs = new TagBuilder("ul");
                breadcrumbs.AddCssClass("breadcrumb");
                breadcrumbs.InnerHtml = BuildBreadcrumbTrail(namedRoute, helper).ToString();
                return new HtmlString(breadcrumbs.ToString(TagRenderMode.Normal));
            }
            return new HtmlString(String.Empty);
        }

        private static IHtmlString BuildBreadcrumbTrail(NamedRoute namedRoute, HtmlHelper helper)
        {
            var li = new TagBuilder("li");
            var routeLink = helper.RouteLink(namedRoute.DisplayName, namedRoute.Name);
            if (NavigationViewExtensions.CurrentRouteMatchesName(helper, namedRoute.Name))
            {
                li.AddCssClass("active");
                li.InnerHtml = string.Format("{0}", namedRoute.DisplayName);
            }
            else
            {
                li.InnerHtml = string.Format("{0}<span class=\"divider\">/</span>", routeLink);
            }
            var breadcrumbTrailPart = new HtmlString(li.ToString(TagRenderMode.Normal));
            if (namedRoute.Parent == null) return breadcrumbTrailPart;
            return new HtmlString(string.Format("{0}{1}", BuildBreadcrumbTrail(namedRoute.Parent, helper), breadcrumbTrailPart));
        }
    }
}