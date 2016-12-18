using BlueCarGpsLib.Data.Repository.Interface;

using BlueCarGpsLib.Model.UserAuth;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlueCarGpsLib.Model.Search;
using BlueCarGpsLib.Helper;

namespace BlueCarGpsLib.Data.Repository.Implement
{
    public class UserRepository: RepositoryBase<User>, IUserRepository
    {
        private BlurCarGpsDataContext context;
        public UserRepository(IDataContextFactory context):base(context) {
            this.context = this._dataContextFactory.Context as BlurCarGpsDataContext;
        }

        public UserRepository(IDataContextFactory context, User user) : base(context, user) {
            this.context = this._dataContextFactory.Context as BlurCarGpsDataContext;
        }

        public bool Create(User user)
        {
            try
            {
                User us = FindById(user.id);
                this.context.User.InsertOnSubmit(user);
                return true;
            }catch
            {
                return false;
            }
        }
        public User FindById(int id)
        {
            return BaseQuery().FirstOrDefault(u => u.id.Equals(id));
        }
       
        public List<User> GetAll()
        {
            var q = this.context.User;
            return q == null ? new List<User>() : q.ToList();
        }

        private IQueryable<User> BaseQuery()
        {
            if (this.user.roleTypeEnum == RoleType.Admin)
            {
                return this.context.User;
            }
            else
            {
                return this.context.User
                    .Where(c => c.id.Equals(this.user.id));
            }
        }

        public bool DeleteById(int id)
        {
            try
            {
                User user = FindById(id);
                if (user != null)
                {
                    this.context.GetTable<User>().DeleteOnSubmit(user);
                    this.context.SubmitChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return false;
            }
        }

        public int GetUserCount()
        {
            return this.context.User.Count();
        }

        public bool Update(User user)
        {
            throw new NotImplementedException();
        }

        public IQueryable<User> Search(UserSearchModel searchModel)
        {
            IQueryable<User> users = BaseQuery();

            if (!string.IsNullOrWhiteSpace(searchModel.name))
            {
                users = users.Where(c => c.name.Contains(searchModel.name.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(searchModel.email))
            {
                users = users.Where(c => c.email.Contains(searchModel.email.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(searchModel.phone))
            {
                users = users.Where(c => c.phone.Contains(searchModel.phone.Trim()));
            }
            if (searchModel.roleType.HasValue)
            {
                users = users.Where(c => c.roleType.Equals(searchModel.roleType));
            }

            return users.OrderBy(c => c.name);
        }
    }
}
