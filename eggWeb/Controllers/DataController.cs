using eggWeb.Models;
using eggWeb.Models.DB;
using eggWeb.Models.viewmodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mayaminer.com.library;
using Dapper;

namespace eggWeb.Controllers
{
    public class DataController : Controller
    {
        string csName { get { return "DataController"; } }
        
        /// <summary>
        /// 取得蛋知識
        /// </summary>
        /// <param name="LineID"></param>
        /// <returns></returns>
        public JsonResult getInfo(string LineID = "")
        {
            EGG_INFO_Q_MST item = new EGG_INFO_Q_MST();
            string actionName = "getInfo";
            Random r = new Random();
            int rInt = r.Next(0, MvcApplication.global.egg_info_list.Count()); //for ints
            item = MvcApplication.global.egg_info_list.Skip(rInt).Take(1).SingleOrDefault();
            LogTool.Info(LineID, actionName, this.csName);
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 取得蛋事件
        /// </summary>
        /// <param name="LineID"></param>
        /// <returns></returns>
        public JsonResult getEvent(string LineID = "")
        {
            string actionName = "getEvent";
            List<EGG_EVENT_DATA> list = new List<EGG_EVENT_DATA>();
            list = MvcApplication.global.egg_event_list.OrderByDescending(x => DateTime.Parse(x.EVENT_DATE)).Take(2).ToList();
            LogTool.Info(LineID, actionName, this.csName);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// getTest
        /// </summary>
        /// <param name="LineID"></param>
        /// <returns></returns>
        public JsonResult getTestData(string LineID = "", string msg = "")
        {
            string actionName = "getTestData";
            RESPONSE_MSG rm = new RESPONSE_MSG();
            LogTool.Info( string.Join(",", new List<string>() { LineID, msg }), actionName, this.csName); //紀錄log
            List<string> AnserList = new List<string>() { "蛋蛋測試區", "我買的是洗選蛋", "我買的不是洗選蛋", "我把雞蛋放在室溫", "我把雞蛋放在冰箱", "我在北部", "我在中部", "我在南部" };//keyListㄎ
            EGG_TEST_INFO test_info = new EGG_TEST_INFO();
            DBLink link = new DBLink();
            bool hasTestData = false;
            try
            {
                string sql = "SELECT * FROM EGG_TEST_INFO WHERE USER_ID = @USER_ID";
                DynamicParameters dp = new DynamicParameters();
                dp.Add("USER_ID", LineID);
                List<EGG_TEST_INFO> test_List = new List<EGG_TEST_INFO>();
                test_List = link.DBA.getSqlDataTable<EGG_TEST_INFO>(sql, dp);
                //是否有使用者資料，且有正在進行中的TEST資料
                if (test_List.Count > 0 && test_List.Exists(x => x.DATASTATUS == "0"))
                {
                    //有資料，且有正在測試的資料
                    hasTestData = true;
                    test_info = test_List.Find(x => x.DATASTATUS == "0");
                    //是否是數值
                    if (NumberHelper.isInt(msg))
                    {
                        int _days = int.Parse(msg);
                        if (_days >= 0 && _days <=30)
                        {
                            test_info.TEMP = msg;
                            rm.attachment = new List<EGG_TEST_INFO>() { test_info };
                        }
                        else
                        {
                            rm.status = RESPONSE_STATUS.WARN;
                            rm.message = "請輸入1-30任何一個數值!";
 
                        }
                    }
                    //in Key List
                    if (AnserList.Contains(msg))
                    {
                        //"我買的是洗選蛋" 
                        if (msg == AnserList[1])
                        {
                            test_info.WASH_EGG = "1";
                        }
                        //" "我買的不是洗選蛋"  
                        if (msg == AnserList[2])
                        {
                            test_info.WASH_EGG = "0";
                        }
                        // "我把雞蛋放在室溫" 
                        if (msg == AnserList[3])
                        {
                            test_info.STORE = "0";
                        }
                        // "我把雞蛋放在冰箱" 
                        if (msg == AnserList[4])
                        {
                            test_info.STORE = "1";
                        }
                        // "我在北部" 
                        if (msg == AnserList[5])
                        {
                            test_info.BUY_PLACE = "0";
                        }
                        // "我在中部" 
                        if (msg == AnserList[6])
                        {
                            test_info.BUY_PLACE = "1";
                        }
                        // "我在南部"
                        if (msg == AnserList[7])
                        {
                            test_info.BUY_PLACE = "2";
                        }
                    }

                    return Json(rm, JsonRequestBehavior.AllowGet);
                }
                else if (test_List.Count == 0 || !test_List.Exists(x => x.DATASTATUS == "0"))
                {
                    //沒資料，或沒正在測試的資料
                    if (msg == AnserList[0])
                    {
                        //確認是否是開始測驗
                        test_info.USER_ID = LineID;
                        test_info.CREATE_DATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        test_info.CREATE_USER = LineID;
                        test_info.DATASTATUS = "0";
                        string sql_inser = "INSERT INTO EGG_TEST_INFO ('USER_ID', 'CREATE_DATE', 'CREATE_USER', 'DATASTATUS') VALUES (@USER_ID, @CREATE_DATE, @CREATE_USER, @DATASTATUS );";
                        link.DBA.DBExecute<DB_EGG_TEST_INFO>(sql_inser, new List<DB_EGG_TEST_INFO>() { test_info });
                        if (link.DBA.hasLastError)
                        {
                            LogTool.Fatal(link.DBA.lastError, actionName, this.csName);
                            rm.status = RESPONSE_STATUS.ERROR;
                            rm.message = "程式發生錯誤，請洽資訊人員!";
                        }
                        else
                        {
                            return Json(rm, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        if (AnserList.Contains(msg))
                        {
                            rm.status = RESPONSE_STATUS.WARN;
                            rm.message = "查無測驗資料，先至功能選單點選蛋蛋測試區!";

                            return Json(rm, JsonRequestBehavior.AllowGet);
                        }

                    }//if

                }//if

                 
               
            }
            catch (Exception ex)
            {
                LogTool.Fatal(ex, actionName, this.csName);
                rm.status = RESPONSE_STATUS.ERROR;
                rm.message = "程式發生錯誤，請洽資訊人員!";
            }
  
            return Json(rm, JsonRequestBehavior.AllowGet);
        }//getTestData
    }
}