using BlueCarGpsLib.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BlueCarGpsLib.Helper.Excel
{
    public class ExcelHelperBase
    {
        public string DbString { get; set; }
        public string FilePath { get; set; }
        public User UserSession { get; set; }

        public ExcelHelperBase() { }

        public ExcelHelperBase(string dbString)
        {
            this.DbString = dbString;
        }

        public ExcelHelperBase(string dbString, string filePath)
        {
            this.DbString = dbString;
            this.FilePath = filePath;
        }
        public ExcelHelperBase(string dbString, string filePath, User user)
        {
            this.DbString = dbString;
            this.FilePath = filePath;
            this.UserSession = user;
        }






    }
}
