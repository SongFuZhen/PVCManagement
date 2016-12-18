using BlueCarGpsLib.Data;
using BlueCarGpsLib.Model.Message;
using BlueCarGpsLib.Service.Implement;
using BlueCarGpsLib.Service.Interface;
using BlueCarGpsWeb.CustomAttributes;
using BlueCarGpsWeb.Models;
using BlueCarGpsWeb.Properties;
using Brilliantech.Framwork.Utils.LogUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace BlueCarGpsWeb.Controllers.API
{
    public class UsersController : ApiController
    {
        /// <summary>
        /// 用户登陆
        /// </summary>
        /// <param name="email"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpPost]
        public ResultMessage Login(string email, string pwd)
        {
            LogUtil.Logger.Info(Request);

            ResultMessage msg = new ResultMessage();
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pwd))
            {
                msg.Content = "账号和密码不可空";
            }
            else
            {
                IUserService s = new UserService(Settings.Default.db);
                User user = s.Auth(email, pwd);
                if (user != null)
                {
                    msg.Success = true;
                    msg.Content = "登陆成功";
                }
                else
                {
                    msg.Content = "账号或密码错误";
                }
            }
            return msg;
        }

        [BasicAuthentication]
        [HttpGet]
        public int GetUserCount()
        {
            return new UserService(Settings.Default.db,
                ((ApiIdentity)HttpContext.Current.User.Identity).User).GetUserCount();
        }

        [HttpGet]
        [BasicAuthentication]
        public List<UserViewModel> GetUserDetail()
        {
            IUserService us = new UserService(Settings.Default.db);
            User u = us.FindById(((ApiIdentity)HttpContext.Current.User.Identity).User.id);
            List<User> ls = new List<User>();
            ls.Add(u);
            return UserViewModel.Converts(ls);
        }
    }
}
