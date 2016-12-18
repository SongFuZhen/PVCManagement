using BlueCarGpsLib.Data;
using BlueCarGpsLib.Helper;
using BlueCarGpsLib.Model.UserAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlueCarGpsWeb.Models
{
    public class UserViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public int? roleType { get; set; }
        public string roleTypeDisplay { get; set; }
        //public string roleTypeName
        //{
        //    get
        //    {
        //        try
        //        {
        //            return EnumHelper.GetDescription((RoleType) roleType);


        //            //if (roleType == 100)
        //            //{
        //            //    return "超级管理员";
        //            //}
        //            //if (roleType == 200)
        //            //{
        //            //    return "银行管理员";
        //            //}
        //            //if (roleType == 300)
        //            //{
        //            //    return "4S店管理员";
        //            //}
        //            //if (roleType == 999)
        //            //{
        //            //    return "未知角色";
        //            //}
        //            //else return null;
        //        }
        //        catch
        //        {
        //            return null;
        //        }
        //    }
        //}

        public static List<UserViewModel> Converts(List<User> users)
        {
            List<UserViewModel> l = new List<UserViewModel>();
            foreach (var i in users)
            {
                l.Add(UserViewModel.Convert(i));
            }
            return l;
        }

        public static UserViewModel Convert(User user)
        {
            return new UserViewModel()
            {
                id = user.id,
                name = user.name,
                email = user.email,
                roleType = user.roleType,
                roleTypeDisplay = user.roleTypeDisplay
            };
        }
    }
}