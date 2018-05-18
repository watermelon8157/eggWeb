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
            string actioName = "getInfo";
            try
            {
                DBLink link = new DBLink();
                LogTool.SaveLogMessage(link.dbPath, actioName, this.csName);
                string sql = "SELECT * FROM EGG_INFO_Q_MST";
                list = link.DBA.getSqlDataTable<EGG_INFO_Q_MST>(sql);
                if (link.DBA.hasLastError)
                {
                    LogTool.SaveLogMessage(link.DBA.lastError, actioName, this.csName);
                }
            }
            catch (Exception ex)
            {
                LogTool.SaveLogMessage(ex, actioName,this.csName);
            }
            
            return Json(list);
        }

    }
}