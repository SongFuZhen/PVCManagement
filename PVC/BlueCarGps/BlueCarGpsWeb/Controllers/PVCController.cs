using BlueCarGpsLib.Data;
using BlueCarGpsWeb.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlueCarGpsLib.Model.Search;
using BlueCarGpsLib.Service.Implement;
using BlueCarGpsLib.Service.Interface;
using BlueCarGpsWeb.Properties;
using BlueCarGpsLib.Model.Message;
using BlueCarGpsWeb.Helper;
using MvcPaging;
using BlueCarGpsLib.Model.Enum;
using BlueCarGpsLib.Helper;
using BlueCarGpsLib.Model.UserAuth;
using BlueCarGpsLib.Helper.Excel;

namespace BlueCarGpsWeb.Controllers
{
    public class PVCController : Controller
    {
        // GET: PVC      
        public ActionResult Index(int? page)
        {
            int pageIndex = PagingHelper.GetPageIndex(page);

            PVCSearchModel q = new PVCSearchModel();

            IPVCService us = new PVCService(Settings.Default.db, Session["user"] as User);

            IPagedList<PVC> pvcs = us.Search(q).ToPagedList(pageIndex, Settings.Default.pageSize);

            ViewBag.Query = q;

            return View(pvcs);
        }

        public ActionResult Search([Bind(Include = "name, shipDateFrom, shipDateTo, peNum, brand ,batchNo, startTimeFrom, startTimeTo, endTimeFrom, endTimeTo, createdAtFrom, createdAtTo")] PVCSearchModel q)
        {
            int pageIndex = 0;
            int.TryParse(Request.QueryString.Get("page"), out pageIndex);
            pageIndex = PagingHelper.GetPageIndex(pageIndex);

            IPVCService us = new PVCService(Settings.Default.db, Session["user"] as User);

            IPagedList<PVC> pvcs = us.Search(q).ToPagedList(pageIndex, Settings.Default.pageSize);

            ViewBag.Query = q;

            return View("Index", pvcs);
        }

        [HttpGet]
        [UserAuthorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [UserAuthorize]
        public ActionResult Create([Bind(Include = "name, shipDate, shipAmount, unit, batchNo, startTimeStr, endTimeStr, brand")] PVC pvc)
        {
            try
            {
                // TODO: Add insert logic here
                if (string.IsNullOrWhiteSpace(pvc.name))
                {
                    ViewBag.msg = "收货人姓名 不能为空";
                    return View();
                }
                if (string.IsNullOrWhiteSpace(pvc.shipDate.ToString()))
                {
                    ViewBag.msg = "发货日期 不能为空";
                    return View();
                }
                if (string.IsNullOrWhiteSpace(pvc.shipAmount.ToString()))
                {
                    ViewBag.msg = "发货数量 不能为空";
                    return View();
                }

                if (string.IsNullOrWhiteSpace(pvc.batchNo))
                {
                    ViewBag.msg = "线别/批号 不能为空";
                    return View();
                }

                if (string.IsNullOrWhiteSpace(Request.Form["startTimeStr"].ToString()))
                {
                    ViewBag.msg = "开始时间 不能为空";
                    return View();
                }

                if (string.IsNullOrWhiteSpace(Request.Form["endTimeStr"].ToString()))
                {
                    ViewBag.msg = "结束时间 不能为空";
                    return View();
                }

                if (string.IsNullOrWhiteSpace(pvc.brand))
                {
                    ViewBag.msg = "品名 不能为空";
                    return View();
                }

                User user = Session["user"] as User;
                IPVCService service = new PVCService(Settings.Default.db, user);

                pvc.peNum = pvc.batchNo.Trim() + "." + Request.Form["startTimeStr"].Trim().ToString() + "-" + Request.Form["endTimeStr"].Trim().ToString();

                pvc.startTime = DateTime.ParseExact(Request.Form["startTimeStr"].Trim().ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                pvc.endTime = DateTime.ParseExact(Request.Form["endTimeStr"].Trim().ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);

                pvc.createdAt = DateTime.Now;
                pvc.userId = user.id;

                bool result = service.Create(pvc);

                if (result)
                {
                    ViewBag.msg = "新建成功";
                    return RedirectToAction("Index");
                }
                else{
                    ViewBag.msg = "新建失败";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.msg = ex;
                return View();
            }
        }

        [HttpGet]
        [UserAuthorize]
        public ActionResult Edit(int id)
        {
            IPVCService us = new PVCService(Settings.Default.db, Session["user"] as User);
            PVC user = us.FindById(id);

            return View(user);
        }

        // POST: PVC/Edit/5
        [HttpPost]
        [UserAuthorize]
        public ActionResult Edit([Bind(Include = "id, name, shipDate, shipAmount, unit, batchNo, startTimeStr, endTimeStr, brand")] PVC pvc)
        {
            ResultMessage msg = new ResultMessage();
            IPVCService us = new PVCService(Settings.Default.db, Session["user"] as User);
            PVC pvcs = us.FindById(pvc.id);

            try
            {
                if (string.IsNullOrWhiteSpace(pvc.name))
                {
                    ViewBag.msg = "收货人姓名 不能为空";
                    return View();
                }
                if (string.IsNullOrWhiteSpace(pvc.shipDate.ToString()))
                {
                    ViewBag.msg = "发货日期 不能为空";
                    return View();
                }
                if (string.IsNullOrWhiteSpace(pvc.shipAmount.ToString()))
                {
                    ViewBag.msg = "发货数量 不能为空";
                    return View();
                }

                if (string.IsNullOrWhiteSpace(pvc.batchNo))
                {
                    ViewBag.msg = "线别/批号 不能为空";
                    return View();
                }

                if (string.IsNullOrWhiteSpace(Request.Form["startTimeStr"].ToString()))
                {
                    ViewBag.msg = "开始时间 不能为空";
                    return View();
                }

                if (string.IsNullOrWhiteSpace(Request.Form["endTimeStr"].ToString()))
                {
                    ViewBag.msg = "结束时间 不能为空";
                    return View();
                }

                if (string.IsNullOrWhiteSpace(pvc.brand))
                {
                    ViewBag.msg = "品名 不能为空";
                    return View();
                }

                User user = Session["user"] as User;
                IPVCService service = new PVCService(Settings.Default.db, user);

                pvc.peNum = pvc.batchNo.Trim() + "." + Request.Form["startTimeStr"].Trim().ToString() + "-" + Request.Form["endTimeStr"].Trim().ToString();

                pvc.startTime = DateTime.ParseExact(Request.Form["startTimeStr"].Trim().ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                pvc.endTime = DateTime.ParseExact(Request.Form["endTimeStr"].Trim().ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);

                //pvcs.name = pvc.name;
                //pvcs.shipDate = pvc.shipDate;
                //pvcs.shipAmount = pvc.shipAmount;
                //pvcs.unit = pvc.unit;
                //pvcs.peNum = pvc.peNum;
                //pvcs.batchNo = pvc.batchNo;
                //pvcs.startTime = pvc.startTime;
                //pvcs.endTime = pvc.endTime;
                //pvcs.brand = pvc.brand;
                //pvcs.name = pvc.name;

                //可以添加操作日志
                bool isSucceed = us.Update(pvc);

                if (isSucceed)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.msg = "更新失败";
                    return View(pvcs);
                }
            }
            catch (Exception ex)
            {
                ViewBag.msg = ex;
                
                return View(pvcs);
            }
        }

        [UserAuthorize]
        [BasicAuthentication]
        // GET: PVC/Delete/5
        public ActionResult Delete(int id)
        {
            IPVCService us = new PVCService(Settings.Default.db, Session["user"] as User);

            PVC pvc = us.FindById(id);

            return View(pvc);
        }

        [UserAuthorize]
        [BasicAuthentication]
        // POST: PVC/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            IPVCService us = new PVCService(Settings.Default.db, Session["user"] as User);
            PVC pvc = us.FindById(id);
            try
            {
                bool isSucceed = us.DeleteById(id);

                if (isSucceed)
                {
                    ViewBag.msg = "删除成功";

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.msg = "删除失败";
                    
                    return View(pvc);
                }
            }
            catch (Exception)
            {
                ViewBag.msg = "删除异常";
                
                return View(pvc);
            }
        }

        [UserAuthorize]
        [BasicAuthentication]
        public ActionResult Import()
        {
            var ff = Request.Files[0];
            string fileName = Helper.FileHelper.SaveAsTmp(ff);
            PVCExcelHelper helper = new PVCExcelHelper(Settings.Default.db, fileName, Session["user"] as User);
            BlueCarGpsLib.Data.Message.ImportMessage msg = helper.Import();

            //添加"text/html",防止IE 自动下载json 格式返回的数据
            return Json(msg, "text/html");
        }
    }
}