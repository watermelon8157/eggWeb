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
            MvcApplication.global = new Global();
            VM_EGG vm = new VM_EGG();
            vm.tabList = MvcApplication.global.egg_info_list.GroupBy(y => new { y.M_DESC, y.M_ID }).Select(x => new EGG_INFO_Q_MST()
            {
                M_ID = x.Key.M_ID,
                M_DESC = x.Key.M_DESC
            }).Distinct().ToList();
            vm.test = new EGG_TEST_INFO();
            vm.test.BUY_DATE = DateTime.Now.ToString("yyyy-MM-dd");
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