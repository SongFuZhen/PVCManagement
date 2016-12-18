using BlueCarGpsLib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlueCarGpsWeb.CustomAttributes
{
    public class UserAuthorizeAttribute : System.Web.Mvc.FilterAttribute, System.Web.Mvc.IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (System.Web.HttpContext.Current.Session["user"] == null)
            {

                System.Web.HttpContext.Current.Session["user"] = null;
                filterContext.Result =
                 new System.Web.Mvc.RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary {{ "controller", "Account" },
                                              { "action", "Login" },
                                             { "returnUrl",    filterContext.HttpContext.Request.RawUrl } });

                return;
            }
        }
    }
}