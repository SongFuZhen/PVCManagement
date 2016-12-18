using BlueCarGpsLib.Data;
using BlueCarGpsLib.Service.Interface;
using System;
using System.Linq;
using System.Collections.Generic;
using BlueCarGpsLib.Model.Message;
using BlueCarGpsLib.Data.Repository.Interface;
using BlueCarGpsLib.Data.Repository.Implement;
using BlueCarGpsLib.Model.Search;
using BlueCarGpsLib.Helper;

namespace BlueCarGpsLib.Service.Implement
{
    public class UserService : ServiceBase, IUserService
    {

        private IUserRepository rep;
        public UserService(string dbString) : base(dbString) {
            rep = new UserRepository(this.Context, this.User);
        }
        public UserService(string dbString, User user) : base(dbString, user) {
            rep = new UserRepository(this.Context, this.User);
        }

        public List<User> All()
        {
            return rep.GetAll();
        }
        public bool Create(User user)
        {
            //ICarTypeRepository rep = new CarTypeRepository(this.Context, this.User);
            user.pwdSalt = user.GenSalt();
            user.pwd = MD5Helper.Encryt(string.Format("{0}{1}", user.pwd,  user.pwdSalt ));
            rep.Create(user);

            this.Context.SaveAll();
            return true;
            //throw new NotImplementedException();
        }
        public bool Update(User user)
        {
            //string salt=string.IsNullOrEmpty( user.pwdSalt) ? user.GenSalt() : user.pwdSalt;
            //user.pwd = MD5Helper.Encryt(string.Format("{0}{1}", user.pwd, user.pwdSalt));
            return rep.Update(user);
        }

        public User FindByLoginField(string loginField)
        {
            DataContext dc = new DataContext(this.DbString);
            return dc.Context.GetTable<User>().FirstOrDefault(s => s.email==loginField || s.phone==loginField);
        }

        public User Auth(string loginField, string pwd)
        {
            User u = FindByLoginField(loginField);
            if (u != null && u.pwd == MD5Helper.Encryt(string.Format("{0}{1}", pwd, u.pwdSalt)))
            {
                return u;
            }
            return null;
        }

        //public List<User> GetUserDetail(int id)
        //{
        //    DataContext dc = new DataContext(this.DbString);
        //    List<User> list= new List<User>();
        //    list.Add(dc.Context
        //        .GetTable<User>()
        //        .FirstOrDefault(s => s.id.Equals(id)));
        //    return list == null ? list : null;
        //}

        public User FindById(int id)
        {
            DataContext dc = new DataContext(this.DbString);

            return dc.Context
                .GetTable<User>()
                .FirstOrDefault(s => s.id.Equals(id));
        }
        public IQueryable<User> Search(UserSearchModel searchModel)
        {
            return rep.Search(searchModel);
        }

        public bool DeleteById(int id)
        {
            return rep.DeleteById(id);
        }

        public int GetUserCount()
        {
            return rep.GetUserCount();
        }
    }
}
