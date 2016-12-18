using BlueCarGpsLib.Data;
using BlueCarGpsLib.Model.Message;
using BlueCarGpsLib.Model.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueCarGpsLib.Service.Interface
{
   public interface IUserService
    {
        User FindById(int id);
          
        /// <summary>
        /// 根据Email或Phone找到用户
        /// </summary>
        /// <param name="loginField"></param>
        /// <returns></returns>
        User FindByLoginField(string loginField);
        
        List<User> All();
        User Auth(string loginField, string pwd);
        bool Create(User user);
        bool Update(User user);

        IQueryable<User> Search(UserSearchModel searchModel);

        bool DeleteById(int id);

        int GetUserCount();
    }
}
