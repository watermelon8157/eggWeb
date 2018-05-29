using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eggWeb.Models.viewmodel
{
    public class VM_EGG
    {
        public List<EGG_INFO_Q_MST> tabList { get; set;}

    }

 
    /// <summary>
    /// 食安知識
    /// </summary>
    public class EGG_INFO_Q_MST: DB_EGG_INFO_Q_MST
    { 
        public bool isShow { get; set; }
    }

    /// <summary>
    /// 食安快訊
    /// </summary>
    public class EGG_EVENT_DATA:DB_EGG_EVENT_DATA
    {

    }

    /// <summary>
    /// 測驗資料
    /// </summary>
    public class EGG_TEST_INFO: DB_EGG_TEST_INFO
    {
         /// <summary>
         /// 目前回答紀錄
         /// </summary>
        public List<EGG_TEST_TEMP> tempList
        {
            get
            {
                List<EGG_TEST_TEMP> _temp = new List<EGG_TEST_TEMP>();
                if (!string.IsNullOrWhiteSpace(this.TEMP)) _temp.Add(new EGG_TEST_TEMP() { q = "0", a = this.TEMP }); //溫度
                if (!string.IsNullOrWhiteSpace(this.DAYS)) _temp.Add(new EGG_TEST_TEMP() { q = "1", a = this.DAYS }); //購買天數
                if (!string.IsNullOrWhiteSpace(this.WASH_EGG)) _temp.Add(new EGG_TEST_TEMP() { q = "2", a = this.WASH_EGG }); //是否為洗選蛋
                if (!string.IsNullOrWhiteSpace(this.STORE)) _temp.Add(new EGG_TEST_TEMP() { q = "3", a = this.STORE }); //儲存環境
                return _temp;
            }
        }
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

    public abstract class DB_EGG_INFO_Q_MST
    {
        public string M_ID { get; set; }
        public string M_DESC { get; set; }
        public string Q_DESC { get; set; }

        public string Q_MEMO { get; set; }

    }

    public abstract class DB_EGG_EVENT_DATA
    {
        public string EVENT_TITLE { get; set; }

        public string EVENT_URL { get; set; }

        public string EVENT_DESC { get; set; }

        public string EVENT_DATE { get; set; }

        public string EVENT_IMAGE { get; set; }

        public string EVENT_FILE { get; set; }
    }

    public abstract class DB_EGG_TEST_INFO
    {
        public string USER_ID { get; set; }

        public string CREATE_DATE { get; set; }

        public string CREATE_USER { get; set; }

        public string DAYS { get; set; }

        public string BUY_PLACE { get; set; }

        public string WASH_EGG { get; set; }

        public string STORE { get; set; }

        public string RESULT { get; set; }

        public string DATASTATUS { get; set; }

        public string CONTENT_JSON { get; set; }

        public string TEMP { get; set; }

        public string BUY_DATE { get; set; }


    }

}