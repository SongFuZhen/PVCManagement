using BlueCarGpsWeb.App_Start;
using BlueCarGpsWeb.Controllers;
using BlueCarGpsWeb.Properties;
using Brilliantech.Framwork.Utils.LogUtil;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;

namespace BlueCarGpsWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // 注册权限的缓存及依赖
            //AuthCacheConfig.RegisterCacheAndDependency();
           
            LogUtil.Logger.Info("Application Start.....");
        }

        /// <summary>
        /// 
        /// </summary>
        // 在api的attribute中使用account设置的session
        protected void Application_PostAuthorizeRequest()
        {
            // 运行APIauth获取到web端的session
            if (IsWebApiRequest())
            {
                HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            }
        }

        private bool IsWebApiRequest()
        {
            return HttpContext.Current.Request
                .AppRelativeCurrentExecutionFilePath
                .StartsWith(WebApiConfig.UrlPrefixRelative);
        }

        protected void Application_End()
        {
          //  AuthCacheConfig.UnRegisterCacheAndDependency();
            LogUtil.Logger.Info("Application End.....");
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            Response.Clear();
            Exception exception = Server.GetLastError();
            HttpException httpException = exception as HttpException;
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            if (exception == null)
            {
                routeData.Values.Add("action", "Error");
            }
            else if (httpException == null)
            {
                routeData.Values.Add("action", "Error");
            }
            else
            {
                switch (httpException.GetHttpCode())
                {
                    case 404:
                        routeData.Values.Add("action", "error404");
                        break;
                    case 500:
                        routeData.Values.Add("action", "error500");
                        break;
                    case 401:
                        routeData.Values.Add("action", "error401");
                        break;
                    default:
                        routeData.Values.Add("action", "General");
                        break;
                }
            }
            // Pass exception details to the target error View.  
            //routeData.Values.Add("error", exception.Message);
            // Clear the error on server.  
            Server.ClearError();
            // Call target Controller and pass the routeData.  
            IController errorController = new ErrorController();
            errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
        }
    }
}
