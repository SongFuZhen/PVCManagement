using BlueCarGpsLib.Data;
using BlueCarGpsLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueCarGpsLib.Service
{
    public class ServiceBase
    {
        private string dbString;
        private ServiceConfig config;

        private DataContext context;
        private User user;

        private static object syncObj = new object();

        public string DbString
        {
            get
            {
                if (this.config!=null && !string.IsNullOrEmpty(this.config.DbString))
                {
                    return this.config.DbString;
                }
                return dbString;
            }
        }
        public ServiceConfig Config
        {
            get { return config; }
            set { config = value; }
        }


        public User User
        {
            get { return user; }
            set { user = value; }
        }

        public DataContext Context
        {
            get
            {
                if (null == context)
                {
                    lock (syncObj)
                    {
                        if (null == context)
                        {
                            context = new DataContext(this.DbString);
                        }
                    }
                }
                return context;
            }
        }

        public ServiceBase() { }

        public ServiceBase(string dbString)
        {
            this.dbString = dbString;
        }

        public  ServiceBase(string dbString,User user)
        {
            this.dbString = dbString;
            this.config = new ServiceConfig() { DbString = dbString, User = user };
            this.user = user;
        }

        public ServiceBase(ServiceConfig config)
        {
            this.config = config;
        }
    }
}
