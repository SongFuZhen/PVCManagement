using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueCarGpsLib.Model.Message
{
    public class GpsServerMessage
    {
        public string imei { get; set; }
        public byte[] body { get; set; }
    }
}
