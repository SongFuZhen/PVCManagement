using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlueCarGpsLib.Data;
using BlueCarGpsLib.Service.Implement;
using BlueCarGpsWeb.Properties;

namespace BlueCarGpsWeb.Helper
{
    public class ViewPermitHelper
    {
        public static bool Permit(string code)
        {
            User user = System.Web.HttpContext.Current.Session["user"] as User;
            if (user == null || !user.roleType.HasValue)
            {
                return false;
            }

            return true;

            //return new SysRoleTypeAuthService(Settings.Default.db)
            //    .PermitView(user, code);
        }
    }
}