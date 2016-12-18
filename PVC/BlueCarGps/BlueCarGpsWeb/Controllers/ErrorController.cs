using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlueCarGpsWeb.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Error(string error)
        {
            ViewData["Title"] = "WebSite 网站内部错误";
            ViewData["Description"] = error;
            return View("Error");
        }
        public ActionResult error404(string error)
        {
            ViewData["Title"] = "HTTP 404 - 无法找到文件";
            ViewData["Description"] = error;
            return View("error404");
        }
        public ActionResult error500(string error)
        {
            ViewData["Title"] = "HTTP 500 - 内部服务器错误";
            ViewData["Description"] = error;
            return View("error500");
        }
        public ActionResult error401(string error)
        {
            ViewData["Title"] = "HTTP 401 - 无法找到文件";
            ViewData["Description"] = error;
            return View("error401");
        }
        public ActionResult General(string error)
        {
            ViewData["Title"] = "HTTP 发生错误";
            ViewData["Description"] = error;
            return View("ErrorGeneral");
        }
    }
}