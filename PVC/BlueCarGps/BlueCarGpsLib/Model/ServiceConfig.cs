using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Caching;
using BlueCarGpsLib.Data;

namespace BlueCarGpsLib.Model
{
    public class ServiceConfig
    {
        private string dbString;

        public string DbString
        {
            get
            {
                return dbString;
            }
            set
            {
                dbString = value;
            }
        }

        public User User { get; set; }

        private string createQueuePath;
        public string CreateQueuePath
        {
            get
            {
                return createQueuePath;
            }
            set
            {
                createQueuePath = value;
            }
        }

        private string messageQueuePath;
        public string MessageQueuePath
        {
            get
            {
                return messageQueuePath;
            }
            set
            {
                messageQueuePath = value;
            }
        }
    }
}
