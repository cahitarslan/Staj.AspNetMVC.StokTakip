using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Staj.AspNetMVC.StokTakip.Web.ActiveFolder
{
    public static class ActiveClass
    {
        public static string ActivePage(this HtmlHelper html, string controller, string action)
        {
            string active = "";
            var routeData = html.ViewContext.RouteData;
            string routeController = routeData.Values["controller"].ToString();
            string routeAction = routeData.Values["action"].ToString();

            if (controller == routeController && action == routeAction) active = "active";
            
            return active;
        }
    }
}