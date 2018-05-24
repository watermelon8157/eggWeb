using eggWeb.Models;
using eggWeb.Models.DB;
using eggWeb.Models.viewmodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eggWeb.Controllers
{
    public class DataController : Controller
    {
        string csName { get { return "DataController"; } }
        // GET: DataController
        public JsonResult HelloWorld()
        {
            return Json("HelloWorld",JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 取得蛋知識
        /// </summary>
        /// <param name="LineID"></param>
        /// <returns></returns>
        public JsonResult getInfo(string LineID = "")
        {
            EGG_INFO_Q_MST item = new EGG_INFO_Q_MST();
            Random r = new Random();
            int rInt = r.Next(0, MvcApplication.global.egg_info_list.Count()); //for ints
            item = MvcApplication.global.egg_info_list.Skip(rInt).Take(1).SingleOrDefault();
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 取得蛋事件
        /// </summary>
        /// <param name="LineID"></param>
        /// <returns></returns>
        public JsonResult getEvent(string LineID = "")
        {
            List<EGG_EVENT_DATA> list = new List<EGG_EVENT_DATA>();
            list = MvcApplication.global.egg_event_list.OrderByDescending(x => DateTime.Parse(x.EVENT_DATE)).Take(2).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}