using BlueCarGpsWeb.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlueCarGpsWeb.Controllers
{
    public class HomeController : Controller
    {
        [UserAuthorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult History(string device_nr, int id)
        {
            return View();
        }

        [HttpGet]
        public ActionResult RealTimeTrace(string device_nr, int id)
        {
            ViewBag.from = Request.QueryString["request_from"];

            return View();
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}