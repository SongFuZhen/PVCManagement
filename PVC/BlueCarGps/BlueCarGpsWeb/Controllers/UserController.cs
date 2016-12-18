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

namespace BlueCarGpsWeb.Controllers
{
    [UserAuthorize]
    public class UserController : Controller
    {
        // GET: User      
        public ActionResult Index(int? page)
        {
            int pageIndex = PagingHelper.GetPageIndex(page);

            UserSearchModel q = new UserSearchModel();

            IUserService us = new UserService(Settings.Default.db, Session["user"] as User);

            IPagedList<User> users = us.Search(q).ToPagedList(pageIndex, Settings.Default.pageSize);

            ViewBag.Query = q;

            SetRoleTypeList(q.roleType);

            return View(users);
        }

        public ActionResult Search([Bind(Include = "name, email, phone, roleType")] UserSearchModel q)
        {
            int pageIndex = 0;
            int.TryParse(Request.QueryString.Get("page"), out pageIndex);
            pageIndex = PagingHelper.GetPageIndex(pageIndex);

            IUserService us = new UserService(Settings.Default.db, Session["user"] as User);

            IPagedList<User> users = us.Search(q).ToPagedList(pageIndex, Settings.Default.pageSize);

            ViewBag.Query = q;

            SetRoleTypeList(q.roleType);

            return View("Index", users);
        }

        [HttpGet]
        [UserAuthorize]
        public JsonResult StatisticalQty()
        {
            Dictionary<string, List<Dictionary<string, string>>> StatisticalQty = new Dictionary<string, List<Dictionary<string, string>>>();

            List<Dictionary<string, string>> adminInfo = new List<Dictionary<string, string>>();
            List<Dictionary<string, string>> bankInfo = new List<Dictionary<string, string>>();
            List<Dictionary<string, string>> shopInfo = new List<Dictionary<string, string>>();
            List<Dictionary<string, string>> superInfo = new List<Dictionary<string, string>>();
            List<Dictionary<string, string>> unknowInfo = new List<Dictionary<string, string>>();
            List<Dictionary<string, string>> otherInfo = new List<Dictionary<string, string>>();

            IUserService us = new UserService(Settings.Default.db, Session["user"] as User);

            List<User> users = us.All();

            foreach(var user in users)
            {
                if(user.roleType == 100)
                {
                    Dictionary<string, string> usif = new Dictionary<string, string>();
                    usif.Add("id",user.id.ToString());
                    usif.Add("name",user.name);
                    usif.Add("email",user.email);
                    usif.Add("phone", user.phone);
                    usif.Add("roleType",user.roleTypeEnum.ToString());
                    adminInfo.Add(usif);
                }
                else if(user.roleType == 200)
                {
                    //银行
                    Dictionary<string, string> usif = new Dictionary<string, string>();
                    usif.Add("id", user.id.ToString());
                    usif.Add("name", user.name);
                    usif.Add("email", user.email);
                    usif.Add("phone", user.phone);
                    usif.Add("roleType", user.roleTypeEnum.ToString());
                    bankInfo.Add(usif);
                }
                else if (user.roleType == 300)
                {
                    //4S店
                    Dictionary<string, string> usif = new Dictionary<string, string>();
                    usif.Add("id", user.id.ToString());
                    usif.Add("name", user.name);
                    usif.Add("email", user.email);
                    usif.Add("phone", user.phone);
                    usif.Add("roleType", user.roleTypeEnum.ToString());
                    shopInfo.Add(usif);
                }
                else if (user.roleType == 400)
                {
                    Dictionary<string, string> usif = new Dictionary<string, string>();
                    usif.Add("id", user.id.ToString());
                    usif.Add("name", user.name);
                    usif.Add("email", user.email);
                    usif.Add("phone", user.phone);
                    usif.Add("roleType", user.roleTypeEnum.ToString());
                    superInfo.Add(usif);
                }
                else if (user.roleType == 500)
                {
                    Dictionary<string, string> usif = new Dictionary<string, string>();
                    usif.Add("id", user.id.ToString());
                    usif.Add("name", user.name);
                    usif.Add("email", user.email);
                    usif.Add("phone", user.phone);
                    usif.Add("roleType", user.roleTypeEnum.ToString());
                    unknowInfo.Add(usif);
                }
                else
                {
                    Dictionary<string, string> usif = new Dictionary<string, string>();
                    usif.Add("id", user.id.ToString());
                    usif.Add("name", user.name);
                    usif.Add("email", user.email);
                    usif.Add("phone", user.phone);
                    usif.Add("roleType", user.roleTypeEnum.ToString());
                    otherInfo.Add(usif);
                }
            }

            StatisticalQty.Add("超级管理员", adminInfo);
            StatisticalQty.Add("银行人员", bankInfo);
            StatisticalQty.Add("4S人员", shopInfo);
            StatisticalQty.Add("监管员", superInfo);
            StatisticalQty.Add("未设置角色", unknowInfo);
            StatisticalQty.Add("其他", otherInfo);

            return Json(StatisticalQty, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [UserAuthorize]
        public ActionResult Create()
        {
            //IUserService us = new UserService(Settings.Default.db, Session["user"] as User);
            //User user;
            SetRoleTypeList(null);
            return View();
        }

        [HttpPost]
        [UserAuthorize]
        public ActionResult Create([Bind(Include = "name,email,phone,pwd,roleType")] User user)
        {
            try
            {
                // TODO: Add insert logic here
                if (string.IsNullOrWhiteSpace(user.name))
                {
                    ViewBag.msg = "用户简称 不能为空";
                    SetRoleTypeList(null);
                    return View();
                }
                if (string.IsNullOrWhiteSpace(user.email))
                {
                    ViewBag.msg = "用户邮箱 不能为空";
                    SetRoleTypeList(null);
                    return View();
                }
                if (string.IsNullOrWhiteSpace(user.pwd))
                {
                    ViewBag.msg = "用户密码 不能为空";
                    SetRoleTypeList(null);
                    return View();
                }
                if (string.IsNullOrWhiteSpace(Convert.ToString(user.roleType)))
                {
                    ViewBag.msg = "用户角色 不能为空";
                    SetRoleTypeList(null);
                    return View();
                }
                IUserService service = new UserService(Settings.Default.db, Session["user"] as User);
                bool result= service.Create(user);

                if (result)
                {
                    ViewBag.msg = "新建成功";
                    return RedirectToAction("Index");
                }
                else{
                    SetRoleTypeList(null);
                    ViewBag.msg = "新建失败";
                    return View();
                }
            }
            catch (Exception ex)
            {
                SetRoleTypeList(null);
                ViewBag.msg = ex;
                return View();
            }
        }

        [HttpGet]
        [UserAuthorize]
        public ActionResult Edit(int id)
        {
            IUserService us = new UserService(Settings.Default.db, Session["user"] as User);
            User user = us.FindById(id);

            SetRoleTypeList(user.roleType);

            return View(user);
        }

        // POST: User/Edit/5
        [HttpPost]
        [UserAuthorize]
        public ActionResult Edit([Bind(Include = "id, name, email,phone, pwd, roleType")] User user)
        {
            ResultMessage msg = new ResultMessage();
            IUserService us = new UserService(Settings.Default.db, Session["user"] as User);
            User users = us.FindById(user.id);
            try
            {
                if (string.IsNullOrWhiteSpace(user.name))
                {
                    ViewBag.msg = "用户简称 不能为空";
                    SetRoleTypeList(users.roleType);
                    return View(users);
                }
                if (string.IsNullOrWhiteSpace(user.email))
                {
                    ViewBag.msg = "用户邮箱 不能为空";
                    SetRoleTypeList(users.roleType);
                    return View(users);
                }
                if (string.IsNullOrWhiteSpace(user.pwd))
                {
                    ViewBag.msg = "用户密码 不能为空";
                    SetRoleTypeList(users.roleType);
                    return View(users);
                }
                if (!user.roleType.HasValue)
                {
                    ViewBag.msg = "用户角色 不能为空";
                    SetRoleTypeList(users.roleType);
                    return View(users);
                }

                bool isSucceed = us.Update(user);
                if (isSucceed)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.msg = "更新失败";
                    SetRoleTypeList(users.roleType);
                    return View(users);
                }
            }
            catch (Exception ex)
            {
                ViewBag.msg = ex;
                SetRoleTypeList(users.roleType);
                return View(users);
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            IUserService us = new UserService(Settings.Default.db, Session["user"] as User);

            User user = us.FindById(id);

            SetRoleTypeList(user.roleType);

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            IUserService us = new UserService(Settings.Default.db, Session["user"] as User);
            User user = us.FindById(id);
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
                    ViewBag.msg = "请删除用户权限再删除用户";
                    SetRoleTypeList(user.roleType);
                    return View(user);
                }
            }
            catch (Exception)
            {
                ViewBag.msg = "请删除用户权限再删除用户";
                SetRoleTypeList(user.roleType);
                return View(user);
            }
        }

        [HttpGet]
        public ActionResult SetAuth(int id)
        {
            IUserService us = new UserService(Settings.Default.db, Session["user"] as User);
            User user = us.FindById(id);
            SetRoleTypeList(user.roleType);
            if (user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult BindWX()
        {
            ViewBag.userId = (Session["user"] as User).id;
            return View();
        }

        private void SetRoleTypeList (int? type, bool allowBlank = true)
        {
            List<EnumItem> item = EnumHelper.GetList(typeof(RoleType));

            List<SelectListItem> select = new List<SelectListItem>();

            if (allowBlank)
            {
                select.Add(new SelectListItem { Text = "", Value = "" });
            }

            foreach (var it in item)
            {
                if (type.HasValue && type.ToString().Equals(it.Value))
                {
                    select.Add(new SelectListItem { Text = it.Text, Value = it.Value.ToString(), Selected = true });
                }
                else
                {
                    select.Add(new SelectListItem { Text = it.Text, Value = it.Value.ToString(), Selected = false });
                }
            }
            ViewData["roleTypeList"] = select;
        }

        //private void SetUserList(int? type, bool allowBlank = true)
        //{
        //    IUserService bs = new UserService(Settings.Default.db, Session["user"] as User);
        //    List<User> item = bs.All();

        //    List<SelectListItem> select = new List<SelectListItem>();

        //    if (allowBlank)
        //    {
        //        select.Add(new SelectListItem { Text = "", Value = "" });
        //    }

        //    foreach (var it in item)
        //    {
        //        select.Add(new SelectListItem { Text = it.name.ToString(), Value = it.id.ToString(), Selected = false });

        //    }
        //    ViewData["UserList"] = select;
        //}
    }
}