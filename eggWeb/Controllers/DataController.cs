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
using System.Data.Common;
using System.Net;

namespace eggWeb.Controllers
{
    public class DataController : Controller
    {
        string csName { get { return "DataController"; } }
        List<string> AnserList { get; set; }

        public DataController()
        {
            AnserList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(string.Concat("[", IniFile.GetConfig("Data", "AnserKey"), "]"));
        }
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

            //list = MvcApplication.global.egg_event_list.OrderByDescending(x => DateTime.Parse(x.EVENT_DATE)).Take(2).ToList();

            LogTool.Info(LineID, actionName, this.csName);
            Random r = new Random();
            int rInt = r.Next(0, MvcApplication.global.egg_event_list.Count()); //for ints
            list = MvcApplication.global.egg_event_list.Skip(rInt).Take(1).ToList();

            return Json(list[0], JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// getTest 取得目前測資料
        /// ex.http://localhost:4803/data/getTestData?LineID=testWeb&msg=蛋蛋測試區
        /// </summary>
        /// <param name="LineID"></param>
        /// <returns></returns>
        public JsonResult getTestData(string LineID = "", string msg = "")
        {
            string actionName = "getTestData";
            RESPONSE_MSG rm = new RESPONSE_MSG();
            LogTool.Info( string.Join(",", new List<string>() { LineID, msg }), actionName, this.csName); //紀錄log
            EGG_TEST_INFO test_info = new EGG_TEST_INFO();
            DBLink link = new DBLink();
            bool hasTestData = false;
            bool hasUpdate = false;
            try
            {
                #region 取得測試資料
                string sql = test_info.getSelectList_KEY_USER_ID();
                DynamicParameters dp = new DynamicParameters();
                dp.Add("USER_ID", LineID);
                List<EGG_TEST_INFO> test_List = new List<EGG_TEST_INFO>();
                #endregion
                test_List = link.DBA.getSqlDataTable<EGG_TEST_INFO>(sql, dp);
                if (msg == "getList")
                {
                    if (test_List.Exists(x => x.DATASTATUS == "0"))
                        rm.attachment = test_List.Find(x => x.DATASTATUS == "0").tempList;
                    else
                        rm.attachment = new List<EGG_TEST_TEMP>();
                    return Json(rm, JsonRequestBehavior.AllowGet);
                }
                //是否有使用者資料，且有正在進行中的TEST資料
                if (test_List.Count > 0 && test_List.Exists(x => x.DATASTATUS == "0"))
                {
                    //有資料，且有正在測試的資料
                    hasTestData = true;
                    test_info = test_List.Find(x => x.DATASTATUS == "0");

                    #region 有資料判斷
                    this.updateRowData(ref test_info, ref hasUpdate, ref rm, ref link, LineID, msg);

                    #endregion

                }
                else if (test_List.Count == 0 || !test_List.Exists(x => x.DATASTATUS == "0"))
                {
                    //沒資料，或沒正在測試的資料
                    #region 沒資料判斷
                    if (msg == this.AnserList[0])
                    {
                        //確認是否是開始測驗
                        this.insertRowData(ref test_info,ref rm,ref link,LineID);
                    }
                    else
                    {
                        if (this.AnserList.Contains(msg))
                        {
                            this.insertRowData(ref test_info, ref rm, ref link, LineID);
                            this.updateRowData(ref test_info, ref hasUpdate, ref rm,ref link,LineID, msg);
                            rm.status = RESPONSE_STATUS.WARN;
                            rm.message = "查無測驗資料，先至功能選單點選蛋蛋測試區!";
                        }

                    }//if
                    #endregion
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

        private void insertRowData(ref EGG_TEST_INFO test_info, ref RESPONSE_MSG rm, ref DBLink link, string LineID)
        {
            test_info.USER_ID = LineID;
            test_info.CREATE_DATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            test_info.CREATE_ID = LineID;
            test_info.CREATE_NAME = LineID;
            test_info.DATASTATUS = "0";
            string sql_inser = "INSERT INTO EGG_TEST_INFO ('USER_ID', 'CREATE_DATE', 'CREATE_ID', 'CREATE_NAME', 'DATASTATUS') VALUES (@USER_ID, @CREATE_DATE, @CREATE_ID, @CREATE_NAME, @DATASTATUS );";
            link.DBA.DBExecute<DB_EGG_TEST_INFO>(sql_inser, new List<DB_EGG_TEST_INFO>() { test_info });
            if (link.DBA.hasLastError)
            {
                LogTool.Fatal(link.DBA.lastError, "insertRowData", this.csName);
                rm.status = RESPONSE_STATUS.ERROR;
                rm.message = "程式發生錯誤，請洽資訊人員!";
            }
            else
            {
                rm.attachment = test_info.tempList;
            }
        }

        private void updateRowData(ref EGG_TEST_INFO test_info,ref bool hasUpdate, ref RESPONSE_MSG rm, ref DBLink link, string LineID, string msg)
        {
            #region 更新欄位
            LogTool.Info(Newtonsoft.Json.JsonConvert.SerializeObject(this.AnserList), "updateRowData", "updateRowData");
            //in Key List
            if (this.AnserList.Contains(msg))
            {
                if (msg == this.AnserList[1]) test_info.WASH_EGG = "我買的是洗選蛋"; //"我買的是洗選蛋" 
                if (msg == this.AnserList[2]) test_info.WASH_EGG = "我買的不是洗選蛋";//" "我買的不是洗選蛋"  
                if (msg == this.AnserList[3]) test_info.STORE = "我把雞蛋放在室溫"; // "我把雞蛋放在室溫" 
                if (msg == this.AnserList[4]) test_info.STORE = "我把雞蛋放在冰箱";// "我把雞蛋放在冰箱" 
                if (msg == this.AnserList[5]) test_info.BUY_PLACE = "我在北部"; // "我在北部" 
                if (msg == this.AnserList[6]) test_info.BUY_PLACE = "我在中部";// "我在中部" 
                if (msg == this.AnserList[7]) test_info.BUY_PLACE = "我在南部"; // "我在南部"
                if (this.AnserList.Skip(5).Take(3).Contains(msg))
                {
                    test_info.TEMP = this.getNowTemp(test_info.BUY_PLACE);
                }
                hasUpdate = true;
            }
            //是否是數值
            if (NumberHelper.isInt(msg))
            {
                hasUpdate = true;
                int _days = int.Parse(msg);
                if (Math.Abs(_days) >= 0)
                {
                    test_info.DAYS = msg;
                    rm.attachment = test_info.tempList;
                }
                else
                {
                    rm.status = RESPONSE_STATUS.WARN;
                    rm.message = "請輸入1-30任何一個數值!";
                    LogTool.Error(Newtonsoft.Json.JsonConvert.SerializeObject(test_info), "updateRowData", this.csName);
                }
            }

            #endregion

            if (rm.status == RESPONSE_STATUS.SUCCESS)
            {
                if (hasUpdate)
                {
                    test_info.MODIFY_DATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    test_info.MODIFY_ID = LineID;
                    test_info.MODIFY_NAME = LineID;
                    link.DBA.DBExecute<EGG_TEST_INFO>(test_info.getUpdateSQL_KEY_USER_ID_CREATE_DATE(), new List<EGG_TEST_INFO>() { test_info });
                }

                if (link.DBA.hasLastError)
                {
                    rm.status = RESPONSE_STATUS.ERROR;
                    rm.message = "SQL 發生錯誤，請洽資訊人員!";
                    LogTool.Error(link.DBA.lastError, "updateRowData", this.csName);
                }
                else
                {
                    rm.status = RESPONSE_STATUS.SUCCESS;
                    rm.attachment = test_info.tempList;
                }
            }
        }

        /// <summary>
        /// 取得測試結果
        /// ex.http://localhost:4803/data/getTestResult?LineID=testWeb
        /// </summary>
        /// <param name="LineID"></param>
        /// <returns></returns>
        public JsonResult getTestResult(string LineID = "")
        {
            RESPONSE_MSG rm = new RESPONSE_MSG();
            string actionName = "getTestResult";
            LogTool.Info(LineID, actionName, this.csName); //紀錄log
            EGG_TEST_INFO test_info = new EGG_TEST_INFO();
            DBLink link = new DBLink();
            try
            {
                #region 取得測試資料
                string sql = test_info.getSelectList_KEY_USER_ID();
                DynamicParameters dp = new DynamicParameters();
                dp.Add("USER_ID", LineID);
                List<EGG_TEST_INFO> test_List = new List<EGG_TEST_INFO>();
                #endregion
                test_List = link.DBA.getSqlDataTable<EGG_TEST_INFO>(sql, dp);
                if (test_List.Count > 0 )
                {
                    if (test_List.Exists(x => x.DATASTATUS == "0"))
                    {
                        test_info = test_List.Find(x => x.DATASTATUS == "0");
                        if (test_info.tempList.Count==4)
                        {
                            #region 測試完畢，計算成績
                            rm.message = "您這一次測試結果如下所示";
                            test_info.DATASTATUS = "1";
                            this.getEggScore(ref test_info);
                            test_info.MODIFY_DATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            test_info.MODIFY_ID = LineID;
                            test_info.MODIFY_NAME = LineID;
                            link.DBA.DBExecute<EGG_TEST_INFO>(test_info.getUpdateSQL_KEY_USER_ID_CREATE_DATE(), new List<EGG_TEST_INFO>() { test_info });
                            if (link.DBA.hasLastError)
                            {
                                rm.status = RESPONSE_STATUS.ERROR;
                                rm.message = "SQL 發生錯誤，請洽資訊人員!";
                                LogTool.Error(link.DBA.lastError, actionName, this.csName);
                            }
                            else
                            {
                                rm.status = RESPONSE_STATUS.SUCCESS;
                                rm.attachment = test_info;
                            }
                            #endregion

                        }
                        else
                        {
                            rm.status = RESPONSE_STATUS.ERROR;
                            rm.message = "上有測驗尚未完成，請選擇功能清單點選測試區，繼續完成!";
                        }
                        rm.attachment = test_info;
                    }
                    else
                    {
                        rm.message = "您最後一次測試結果如下所示";
                        test_info = test_List.FindAll(x => x.DATASTATUS == "1").OrderByDescending(x => DateTime.Parse(x.CREATE_DATE)).First();
                        rm.attachment = test_info;
                    }
                }
                else
                {
                    rm.message = "查無測試結果資料，請選擇功能清單點選測試區!";
                }
            }
            catch (Exception ex)
            {
                LogTool.Fatal(ex, actionName, this.csName);
                rm.status = RESPONSE_STATUS.ERROR;
                rm.message = "程式發生錯誤，請洽資訊人員!";
            }
            return Json(rm, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 計算成績
        /// </summary>
        /// <param name="pData"></param>
        private void getEggScore(ref EGG_TEST_INFO pData)
        {
            //"蛋蛋測試區", "我買的是洗選蛋", "我買的不是洗選蛋", "我把雞蛋放在室溫", "我把雞蛋放在冰箱", "我在北部", "我在中部", "我在南部"
            EGG_SYS_PARAMS esp = new EGG_SYS_PARAMS();
            DBLink link = new DBLink();
            string actionName = "getEggScore";
            try
            {
                List<string> pModel = new List<string>() { "formula", "Interval", "eggs", "temp", "test" };//model
                List<string> tempList = new List<string>() { "24", "30", "35" };//溫度
                string sql = esp.getSYS_SQL(pModel);
                DynamicParameters dp = new DynamicParameters();
                dp.Add("S_MODEL", pModel);
                List<EGG_SYS_PARAMS> pList = link.DBA.getSqlDataTable<EGG_SYS_PARAMS>(sql, dp);
                if (!link.DBA.hasLastError)
                {
                    #region 計算分數
                    string MODEL = "", WASH_EGG = "", STORE = "", TEMP="";
                    if (!string.IsNullOrWhiteSpace(pData.WASH_EGG) && pData.WASH_EGG == this.AnserList[1])
                    { WASH_EGG = "洗選"; } else { WASH_EGG = "非洗選"; }//洗選蛋判斷
                    if (!string.IsNullOrWhiteSpace(pData.STORE) && pData.STORE == this.AnserList[4]){
                        MODEL = "formula"; STORE = "冰箱";  
                    }else{
                        if (NumberHelper.isInt(pData.TEMP) && tempList.Contains(int.Parse(pData.TEMP).ToString())){
                            MODEL = "formula"; STORE = pData.TEMP;
                        }else{
                            if (NumberHelper.isNumber(pData.TEMP)){
                                MODEL = "Interval";
                                #region 選擇計算方式
                                if (double.Parse(pData.TEMP) < 24)
                                {
                                    STORE = "24";
                                    TEMP = "10-24";
                                }
                                else if (double.Parse(pData.TEMP) > 24 && double.Parse(pData.TEMP) < 30)
                                {
                                    STORE = "24";
                                    TEMP = "24-30";
                                }
                                else if (double.Parse(pData.TEMP) > 30)
                                {
                                    TEMP = "30";
                                    STORE = "30";
                                }
                                #endregion
                            }
                        }
                    }//保存判斷
                    #region 取得扣新鮮度分數
                    if (MODEL == "formula")
                    {
                        if (pList.Exists(x => x.S_MODEL == MODEL && x.S_GROUP == WASH_EGG && x.S_NAME == STORE)){
                            esp = pList.Find(x => x.S_MODEL == MODEL && x.S_GROUP == WASH_EGG && x.S_NAME == STORE);
                            pData.getScore(pList.Find(x => x.S_MODEL == "test" && x.S_GROUP == "score_max").S_VALUE,
                                            pList.Find(x => x.S_MODEL == MODEL && x.S_GROUP == WASH_EGG && x.S_NAME == STORE).S_VALUE);
                        }
                    }else if (MODEL == "Interval"){
                        esp = pList.Find(x => x.S_MODEL == "formula" && x.S_GROUP == WASH_EGG && x.S_NAME == STORE);
                        double _addTemp = 0;
                        EGG_SYS_PARAMS _esp = new EGG_SYS_PARAMS();
                        double _temp = double.Parse(pData.TEMP) * 1.0;
                        switch (TEMP)
                        {
                            #region 溫差間距
                            case "10-24":
                                //減少
                                _esp = pList.Find(x => x.S_MODEL == MODEL && x.S_GROUP == WASH_EGG && x.S_NAME == "10-24");
                                _addTemp = -((24 * 1.0) - _temp) * double.Parse(_esp.S_VALUE);
                                _addTemp = double.Parse(esp.S_VALUE) + _addTemp;
                                break;
                            case "24-30":
                            case "30":
                                if (_temp > 30)
                                {
                                    //增加
                                    _esp = pList.Find(x => x.S_MODEL == MODEL && x.S_GROUP == WASH_EGG && x.S_NAME == "24-30");
                                    _addTemp = (6 * 1.0) * double.Parse(_esp.S_VALUE);
                                    _addTemp = double.Parse(esp.S_VALUE) + _addTemp;

                                    _esp = pList.Find(x => x.S_MODEL == MODEL && x.S_GROUP == WASH_EGG && x.S_NAME == "30");
                                    _addTemp = _addTemp + ((_temp - 30) * 1.0) * double.Parse(_esp.S_VALUE);
                                }
                                else
                                {
                                    //增加
                                    _esp = pList.Find(x => x.S_MODEL == MODEL && x.S_GROUP == WASH_EGG && x.S_NAME == "24-30");
                                    _addTemp = ((_temp - 24) * 1.0) * double.Parse(_esp.S_VALUE);
                                    _addTemp = double.Parse(esp.S_VALUE) + _addTemp;
                                }
                                break;
                            default:
                                break;
                                #endregion
                        }
                        LogTool.Info(Newtonsoft.Json.JsonConvert.SerializeObject(pData), actionName + "GET", this.csName);
                        LogTool.Info(_addTemp.ToString(), actionName + "GET", this.csName);
                        pData.getScore(pList.Find(x => x.S_MODEL == "test" && x.S_GROUP == "score_max").S_VALUE, (_addTemp).ToString());
                    }
                    #endregion


                    #endregion
                }
                else
                {
                    LogTool.Fatal(link.DBA.lastError, actionName, this.csName);
                }
            }
            catch (Exception ex)
            {
                LogTool.Fatal(ex, actionName, this.csName);
            }
        }

        /// <summary>
        /// 取的現在溫度
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        private string getNowTemp(string store)
        {
            //參考 API https://works.ioa.tw/weather/api/doc/index.html#api-Weather_API
            string _temp = "30";
            if (this.AnserList[5] == store)
            {
                //台北
                _temp = Newtonsoft.Json.JsonConvert.DeserializeObject<TEMP>(new WebClient().DownloadString("https://works.ioa.tw/weather/api/weathers/1.json")).temperature;
                 
            }
            if (this.AnserList[6] == store)
            {
                //台中
                _temp = Newtonsoft.Json.JsonConvert.DeserializeObject<TEMP>(new WebClient().DownloadString("https://works.ioa.tw/weather/api/weathers/10.json")).temperature;
              
            }
            if (this.AnserList[7] == store)
            {
                //台南
                _temp = Newtonsoft.Json.JsonConvert.DeserializeObject<TEMP>(new WebClient().DownloadString("https://works.ioa.tw/weather/api/weathers/16.json")).temperature;
            }

            return _temp;
        }

        /// <summary>
        /// 取得網站成績
        /// </summary>
        /// <param name="pData"></param>
        /// <returns></returns>
        public JsonResult getWebTestResult(EGG_TEST_INFO pData)
        {
            RESPONSE_MSG rm = new RESPONSE_MSG();
            string actionName = "getWebTestResult";
            DBLink link = new DBLink();
            bool hasUpdate = false;
            JsonResult jr = null;
            string LineID = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            try
            {
                if (this.AnserList.Skip(5).Take(3).Contains(pData.BUY_PLACE))
                {
                    pData.TEMP = this.getNowTemp(pData.BUY_PLACE);
                }
                this.insertRowData(ref pData, ref rm, ref link, LineID);
                pData.DAYS = ((DateTime.Now - DateTime.Parse(pData.BUY_DATE)).Days + 1).ToString();
                hasUpdate = true;
                this.updateRowData(ref pData, ref hasUpdate, ref rm, ref link, LineID, "");
                jr = this.getTestResult(LineID);
            }
            catch (Exception ex)
            {
                rm.status = RESPONSE_STATUS.ERROR;
                rm.message = "程式碼發生錯誤";
                LogTool.Fatal(ex, actionName ,this.csName);
            }
            return Json(jr.Data, JsonRequestBehavior.AllowGet);
        }
    }
}