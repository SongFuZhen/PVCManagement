using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueCarGpsLib.CustomException
{
    public class ValueIsNullException : Exception
    {

        public ValueIsNullException() : base("必须值未初始化") { }
    }
}
