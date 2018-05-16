using eggWeb.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using eggWeb.Models.viewmodel;

namespace eggWeb.Controllers
{
    public class HomeController : Controller
    {
        //Home

        public ActionResult Index()
        {
            VM_EGG vm = new VM_EGG();
            return View(vm);
        }
        public ActionResult Index2()
        {
            return View();
        }
        public ActionResult Index_old()
        {
            return View();
        }

        public JsonResult getInfo()
        {
            List<EGG_INFO_Q_MST> list = new List<EGG_INFO_Q_MST>();
            DBLink link = new DBLink();
            string sql = "SELECT * FROM EGG_INFO_Q_MST";
            list = link.DBA.getSqlDataTable<EGG_INFO_Q_MST>(sql);
 
            return Json(list);
        }

    }
}