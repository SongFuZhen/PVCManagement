using BlueCarGpsWeb.CustomAttributes;
using Microsoft.Owin;
using Newtonsoft.Json;
using Owin;
using System.Web.Http;

[assembly: OwinStartupAttribute(typeof(BlueCarGpsWeb.Startup))]
namespace BlueCarGpsWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {  
            // 设置json，防止loop
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling=
                Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            
            // 取消默认权限
           //ConfigureAuth(app);
        }
    }
}
