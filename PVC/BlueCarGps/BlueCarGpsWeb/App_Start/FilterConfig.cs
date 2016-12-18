using BlueCarGpsWeb.CustomAttributes;
using System.Web;
using System.Web.Mvc;

namespace BlueCarGpsWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
           
        }
    }
}
