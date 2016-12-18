using BlueCarGpsLib.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueCarGpsLib.Model.Search
{
    public class UserSearchModel
    {
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public int? roleType { get; set; }
    }
}
