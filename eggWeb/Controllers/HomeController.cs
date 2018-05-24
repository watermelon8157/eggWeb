using eggWeb.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using eggWeb.Models.viewmodel;
using eggWeb.Models;
namespace eggWeb.Controllers
{
    public class HomeController : Controller
    {
        //Home
        string csName { get { return "HomeController";} }
        public ActionResult Index()
        {
            VM_EGG vm = new VM_EGG();
            return View(vm);
        }

        public JsonResult getInfo()
        {
            return Json(MvcApplication.global.egg_info_list);
        }

        public JsonResult getEvent()
        {
            return Json(MvcApplication.global.egg_event_list);
        }

        #region TEST
        public ActionResult Index2()
        {
            return View();
        }
        public ActionResult Index_old()
        {
            return View();
        }
        #endregion

    }
}