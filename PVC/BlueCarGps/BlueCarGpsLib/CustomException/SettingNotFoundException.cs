using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueCarGpsLib.CustomException
{
    public class SettingNotFoundException:Exception
    {

        public SettingNotFoundException() : base("配置未找到，请联系管理员") { }
    }
}
