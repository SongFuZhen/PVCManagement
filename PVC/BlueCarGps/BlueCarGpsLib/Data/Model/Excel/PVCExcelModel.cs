using BlueCarGpsLib.Data.Message;
using BlueCarGpsLib.Data.Model.Excel;
using BlueCarGpsLib.Properties;
using BlueCarGpsLib.Service.Implement;
using BlueCarGpsLib.Service.Interface;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BlueCarGpsLib.Data.Model.Excel
{
    public class PVCExcelModel : BaseExcelModel
    {

        public static List<string> Headers = new List<string>() { "名称", "发货日期", "发货数量" ,"单位", "线别/批号", "开始时间", "结束时间", "错误信息" };

        public string name { get; set; }
        public string shipDate { get; set; }
        public int? shipAmount { get; set; }
        public string unit { get; set; }
        public string batchNo { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string brand { get; set; }
        public DateTime? shipDateTime
        {
            get
            {
                try
                {
                    return DateTime.Parse(this.shipDate);
                }
                catch
                {
                    return null;
                }
            }
        }
        public DateTime? startDateTime
        {
            get
            {
                try
                {
                    return DateTime.ParseExact(startTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture); ;
                }
                catch
                {
                    return null;
                }
            }
        }
        public DateTime? endDateTime
        {
            get
            {
                try
                {
                    return DateTime.ParseExact(endTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture); ;
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="model"></param>
        /// <param name="dbString"></param>
        public void Validate(string dbString, User user)
        {
            ValidateMessage msg = new ValidateMessage();

            if (string.IsNullOrEmpty(this.name))
            {
                msg.Contents.Add("收货人姓名不可空");
            }

            if (!this.shipDateTime.HasValue)
            {
                msg.Contents.Add("发货日期不可空");
            }

            if (!this.shipAmount.HasValue)
            {
                msg.Contents.Add("发货数量不可空");
            }

            if (string.IsNullOrWhiteSpace(unit))
            {
                msg.Contents.Add("单位不可空");
            }

            if (!this.startDateTime.HasValue)
            {
                msg.Contents.Add("开始时间错误");
            }

            if (!this.endDateTime.HasValue)
            {
                msg.Contents.Add("结束时间错误");
            }

            if (string.IsNullOrWhiteSpace(brand))
            {
                msg.Contents.Add("品名不可为空");
            }
            
            msg.Success = msg.Contents.Count == 0;

            this.ValidateMessage = msg;
        }
    }
}
