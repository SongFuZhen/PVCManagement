using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BlueCarGpsLib.Model.UserAuth
{

    public enum RoleType
    {
        [Description("超级管理员")]
        Admin = 100,

        [Description("普通用户")]
        User = 200,
    }
}