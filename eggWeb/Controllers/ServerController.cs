using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eggWeb.Models;
using eggWeb.Models.viewmodel;

namespace eggWeb.Controllers
{
    public class ServerController : Controller
    {
        // GET: Server
        string csName { get { return "ServerController"; } }
        /// <summary>
        /// 新增食安事件
        /// </summary>
        /// <param name="pData"></param>
        /// <returns></returns>
        public JsonResult inserEvent(EGG_EVENT_DATA pData)
        {
            RESPONSE_MSG rm = new RESPONSE_MSG();
            string actionName = "inserEvent";
            try
            {

            }
            catch (Exception ex)
            {

                throw;
            }
            return Json(rm, JsonRequestBehavior.AllowGet);
        }

       /// <summary>
       /// 檢查是否有此新聞資料
       /// </summary>
       /// <param name="URL"></param>
       /// <returns></returns>
        public JsonResult getEventURL(string URL)
        {
            RESPONSE_MSG rm = new RESPONSE_MSG();
            string actionName = "getEventURL";
            try
            {

            }
            catch (Exception ex)
            {

                throw;
            }
            return Json(rm, JsonRequestBehavior.AllowGet);
        }
    }
}