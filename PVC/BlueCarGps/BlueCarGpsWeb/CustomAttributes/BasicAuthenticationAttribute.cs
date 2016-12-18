using BlueCarGpsLib.Data;
using BlueCarGpsLib.Model.Message;
using BlueCarGpsLib.Service.Implement;
using BlueCarGpsLib.Service.Interface;
using BlueCarGpsWeb.Models;
using BlueCarGpsWeb.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;

namespace BlueCarGpsWeb.CustomAttributes
{

    public class BasicAuthenticationAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            try
            {
                // account 设置的session如果存在，则可以调用api
                var s = System.Web.HttpContext.Current.Session["user"];
                if (s != null)
                {
                    HttpContext.Current.User = new GenericPrincipal(new ApiIdentity(s as User), new string[] { });
                    base.OnActionExecuting(actionContext);
                    return;
                }
                // 从其他API入口进入的需使用basic auth
                if (actionContext.Request.Headers.Authorization == null)
                {
                     UnauthorizedRes(actionContext);
                   // actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, new ResultMessage() { Content = "401 Unauthorized" }, actionContext.ControllerContext.Configuration.Formatters.JsonFormatter);
                }
                else
                {
                    string token = actionContext.Request.Headers.Authorization.Parameter;
                    string decodeToken = Encoding.UTF8.GetString(Convert.FromBase64String(token));
                    string loginField = decodeToken.Substring(0, decodeToken.IndexOf(":"));
                    string pwd = decodeToken.Substring(decodeToken.IndexOf(":") + 1);

                    IUserService us = new UserService(Settings.Default.db);
                    User user = us.Auth(loginField, pwd);

                    if (user != null)
                    {
                        HttpContext.Current.User = new GenericPrincipal(new ApiIdentity(user), new string[] { });
                        base.OnActionExecuting(actionContext);
                    }
                    else
                    {
                        UnauthorizedRes(actionContext);
                    }
                }
            }
            catch(Exception ex)
            {
               UnauthorizedRes(actionContext);
            }
        }

        private void UnauthorizedRes(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized,
               new ResultMessage() { Content = "401 Unauthorized" },
               actionContext.ControllerContext.Configuration.Formatters.JsonFormatter);
        }
    }
}