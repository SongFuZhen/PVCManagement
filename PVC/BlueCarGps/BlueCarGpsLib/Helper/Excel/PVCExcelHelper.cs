using BlueCarGpsLib.Data;
using BlueCarGpsLib.Data.Message;
using BlueCarGpsLib.Service.Implement;
using BlueCarGpsLib.Service.Interface;
using BlueCarGpsLib.Data.Model.Excel;
using Brilliantech.Framwork.Utils.LogUtil;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BlueCarGpsLib.Helper.Excel
{
    public class PVCExcelHelper : ExcelHelperBase
    {
        public PVCExcelHelper() { }
        public PVCExcelHelper(string dbString) : base(dbString)
        {
            this.DbString = dbString;
        }

        public PVCExcelHelper(string dbString, string filePath) : base(dbString, filePath)
        {
        }
        public PVCExcelHelper(string dbString, string filePath, User user) : base(dbString, filePath, user)
        {
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <returns></returns>
        public ImportMessage Import()
        {
            ImportMessage msg = new ImportMessage() { Success = true };
            try
            {
                FileInfo fileInfo = new FileInfo(this.FilePath);
                List<PVCExcelModel> records = new List<PVCExcelModel>();
                string sheetName = "Tmp";
                /// 读取excel文件
                using (ExcelPackage ep = new ExcelPackage(fileInfo))
                {
                    if (ep.Workbook.Worksheets.Count > 0)
                    {
                        ExcelWorksheet ws = ep.Workbook.Worksheets.First();
                        sheetName = ws.Name;

                        for (int i = 2; i <= ws.Dimension.End.Row; i++)
                        {
                            records.Add(new PVCExcelModel()
                            {
                                name = ws.Cells[i, 1].Value == null ? string.Empty : ws.Cells[i, 1].Value.ToString().Trim(),
                                shipDate = ws.Cells[i, 2].Value == null ? string.Empty : ws.Cells[i, 2].Value.ToString().Trim(),
                                shipAmount = Convert.ToInt32(ws.Cells[i, 3].Value == null ? string.Empty : ws.Cells[i, 3].Value),
                                unit = ws.Cells[i, 4].Value == null ? string.Empty : ws.Cells[i, 4].Value.ToString().Trim(),
                                batchNo = ws.Cells[i, 5].Value == null ? string.Empty : ws.Cells[i, 5].Value.ToString().Trim(),
                                startTime = ws.Cells[i, 6].Value == null ? string.Empty : ws.Cells[i, 6].Value.ToString().Trim(),
                                endTime = ws.Cells[i, 7].Value == null ? string.Empty : ws.Cells[i, 7].Value.ToString().Trim(),
                                brand = ws.Cells[i, 8].Value == null ? string.Empty : ws.Cells[i, 8].Value.ToString().Trim()
                            });
                        }
                    }
                    else
                    {
                        msg.Success = false;
                        msg.Content = "文件不包含数据表，请检查";
                    }
                }

                if (msg.Success)
                {
                    /// 验证数据
                    if (records.Count > 0)
                    {
                        Validates(records);
                        if (records.Where(s => s.ValidateMessage.Success == false).Count() > 0)
                        {
                            /// 创建错误文件
                            msg.Success = false;
                            /// 写入文件夹，然后返回
                            string tmpFile = FileHelper.CreateFullTmpFilePath(Path.GetFileName(this.FilePath), true);
                            msg.Content = FileHelper.GetDownloadTmpFilePath(tmpFile);
                            msg.ErrorFileFeed = true;

                            FileInfo tmpFileInfo = new FileInfo(tmpFile);
                            using (ExcelPackage ep = new ExcelPackage(tmpFileInfo))
                            {
                                ExcelWorksheet sheet = ep.Workbook.Worksheets.Add(sheetName);
                                ///写入Header
                                for (int i = 0; i < PVCExcelModel.Headers.Count(); i++)
                                {
                                    sheet.Cells[1, i + 1].Value = PVCExcelModel.Headers[i];
                                }
                                ///写入错误数据
                                for (int i = 0; i < records.Count(); i++)
                                {
                                    sheet.Cells[i + 2, 1].Value = records[i].name;
                                    sheet.Cells[i + 2, 2].Value = records[i].shipDate;
                                    sheet.Cells[i + 2, 3].Value = records[i].shipAmount;
                                    sheet.Cells[i + 2, 4].Value = records[i].unit;
                                    sheet.Cells[i + 2, 5].Value = records[i].batchNo;
                                    sheet.Cells[i + 2, 6].Value = records[i].startTime;
                                    sheet.Cells[i + 2, 7].Value = records[i].endTime;
                                    sheet.Cells[i + 2, 8].Value = records[i].brand;

                                    foreach(var content in records[i].ValidateMessage.Contents)
                                    {
                                        sheet.Cells[i + 2, 9].Value += " [" + content + "] ";
                                    }
                                }

                                /// 保存
                                ep.Save();
                            }
                        }
                        else
                        {
                            for (var i = 0; i < records.Count; i++)
                            {
                                PVC detail = new PVC();
                                detail.name = records[i].name;
                                detail.shipDate = records[i].shipDateTime.Value;
                                detail.shipAmount = records[i].shipAmount.Value;
                                detail.unit = records[i].unit;
                                detail.batchNo = records[i].batchNo;
                                detail.startTime = records[i].startDateTime.Value;
                                detail.endTime = records[i].endDateTime.Value;
                                detail.brand = records[i].brand;

                                //新增车辆
                                IPVCService pvcs = new PVCService(this.DbString, UserSession);

                                //TODO：判断PENum 是否可以重复
                                //PVC temp_pvc = pvcs.FindByPeNum(detail.vin);
                                detail.peNum = detail.batchNo + "." + records[i].startTime + "-" + records[i].endTime;
                                detail.createdAt = DateTime.Now;
                                detail.userId = UserSession.id;

                                var CarResult = pvcs.Create(detail);
                                if (CarResult)
                                {
                                    msg.Content = "导入成功";
                                }
                                else
                                {
                                    msg.Content = "数据有问题";
                                }
                            }
                        }
                    }
                    else
                    {
                        msg.Success = false;
                        msg.Content = "文件不包含数据，请检查";
                    }
                }
            }
            catch (Exception e)
            {
                msg.Success = false;
                msg.Content = "导入失败：" + e.Message + "，请联系系统管理员";
                LogUtil.Logger.Error(e.Message);
                LogUtil.Logger.Error(e.StackTrace);
            }
            return msg;
        }

        public List<PVCExcelModel> Validates(List<PVCExcelModel> models)
        {
            User user = UserSession;
            foreach (var m in models)
            {
                m.Validate(this.DbString,user);
            }
            return models;
        }
    }
}
