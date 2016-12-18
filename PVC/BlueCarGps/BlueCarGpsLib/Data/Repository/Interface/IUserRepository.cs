using BlueCarGpsLib.Model.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueCarGpsLib.Data.Repository.Interface
{

    public interface IUserRepository
    {
        bool Create(User user);
        List<User> GetAll();
        bool Update(User user);
        IQueryable<User> Search(UserSearchModel searchModel);
        bool DeleteById(int id);
        int GetUserCount();
    }
}
