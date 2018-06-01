using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eggWeb.Models.viewmodel
{
    public class VM_EGG
    {
        public EGG_TEST_INFO test { get; set; }
        public List<EGG_INFO_Q_MST> tabList { get; set; }

    }


    /// <summary>
    /// 食安知識
    /// </summary>
    public class EGG_INFO_Q_MST : DB_EGG_INFO_Q_MST
    {
        public bool isShow { get; set; }
    }

    /// <summary>
    /// 食安快訊
    /// </summary>
    public class EGG_EVENT_DATA : DB_EGG_EVENT_DATA
    {

    }

    /// <summary>
    /// 測驗資料
    /// </summary>
    public class EGG_TEST_INFO : DB_EGG_TEST_INFO
    {

        /// <summary>
        /// 目前回答紀錄
        /// </summary>
        public List<EGG_TEST_TEMP> tempList
        {
            get
            {
                List<EGG_TEST_TEMP> _temp = new List<EGG_TEST_TEMP>();
                if (!string.IsNullOrWhiteSpace(this.BUY_PLACE)) _temp.Add(new EGG_TEST_TEMP() { q = "0", a = string.Concat(this.BUY_PLACE,"  目前氣溫約 " + this.TEMP)}); //溫度
                if (!string.IsNullOrWhiteSpace(this.DAYS)) _temp.Add(new EGG_TEST_TEMP() { q = "1", a = string.Concat(this.DAYS ,"天") }); //購買天數
                if (!string.IsNullOrWhiteSpace(this.WASH_EGG)) _temp.Add(new EGG_TEST_TEMP() { q = "2", a = this.WASH_EGG }); //是否為洗選蛋
                if (!string.IsNullOrWhiteSpace(this.STORE)) _temp.Add(new EGG_TEST_TEMP() { q = "3", a = this.STORE }); //儲存環境
                return _temp;
            }
        }

        public string RESULT_DESC
        {
            get
            {
                string _tmp = "查無測試結果";
                if (mayaminer.com.library.NumberHelper.isNumber(this.RESULT))
                {
                    Random r = new Random();
                    double score = double.Parse(this.RESULT);
                    if (score >= 80)
                    {
                        
                        List<string> _temp = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(string.Concat("[", IniFile.GetConfig("RESULT_DESC", "good"), "]")) ;
                        return _temp[r.Next(0, _temp.Count())];
                    }
                    if (score < 80 && score >60)
                    {
                        List<string> _temp = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(string.Concat("[", IniFile.GetConfig("RESULT_DESC", "ok"), "]") );
                        return _temp[r.Next(0, _temp.Count())];
                    }
                    if (score <= 60)
                    {
                        List<string> _temp = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(string.Concat("[", IniFile.GetConfig("RESULT_DESC", "bad"), "]") );
                        return _temp[r.Next(0, _temp.Count())];
                    }
                }
                return _tmp;
            }
        }


        #region method

        public void getScore(string score_max, string pInterval)
        {
         
            try
            {
             
                double max_score = int.Parse(score_max) * 1.0 - (int.Parse(this.DAYS)*1.0 * double.Parse(pInterval));
                this.RESULT = max_score.ToString();
                LogTool.Info(this.RESULT, "getScore", "EGG_TEST_INFO");
            }
            catch (Exception ex)
            {
                LogTool.Fatal(ex, "getScore", "EGG_TEST_INFO");
            }
             
        }


        #region SQL  method
        /// <summary>
        /// select 清單
        /// </summary>
        /// <returns></returns>
        public string getSelectList_KEY_USER_ID()
        {
            return "SELECT * FROM EGG_TEST_INFO WHERE USER_ID = @USER_ID AND DATASTATUS in('1','0')";
        }
        /// <summary>
        /// 更新 KEY  USER_ID,CREATE_DATE
        /// </summary>
        /// <returns></returns>
        public string getUpdateSQL_KEY_USER_ID_CREATE_DATE()
        {
            return @"UPDATE EGG_TEST_INFO SET  
 BUY_DATE =@BUY_DATE,DAYS =@DAYS,  BUY_PLACE =@BUY_PLACE, WASH_EGG =@WASH_EGG, STORE =@STORE, TEMP =@TEMP, MODIFY_DATE =@MODIFY_DATE, MODIFY_ID =@MODIFY_ID, MODIFY_NAME =@MODIFY_NAME, DATASTATUS =@DATASTATUS, RESULT =@RESULT
 WHERE CREATE_DATE = @CREATE_DATE AND USER_ID = @USER_ID;";
        }
        #endregion


        #endregion


    }

    /// <summary>
    /// 使用者問答資料
    /// </summary>
    public class EGG_TEST_TEMP
    {
        /// <summary>
        /// 問題
        /// </summary>
        public string q { get; set; }
        /// <summary>
        /// 答案
        /// </summary>
        public string a { get; set; }
    }

    /// <summary>
    /// 溫度資料
    /// </summary>
    public class TEMP
    {

        public string temperature { get; set; }
    }
}