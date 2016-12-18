using BlueCarGpsLib.Data.Enum;
using BlueCarGpsLib.Data.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueCarGpsLib.Data.Model.Excel
{
    public class BaseExcelModel
    {
        public ValidateMessage ValidateMessage { get; set; }
        public static string SOURCE_TYPE = AttendanceRecordSourceType.FileImport.ToString();
    }
}
