using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Newtonsoft.Json;

namespace eggWeb.Models
{
    public class LogTool
    {
        /// <summary>
        /// 預設log檔案位置
        /// </summary>
        private static string default_log_save_path = AppDomain.CurrentDomain.BaseDirectory.ToString();


        private static string DefaultPath
        {
            get
            {
                return default_log_save_path;
            }
        }
        /// <summary>
        /// 檢驗路徑的資料夾是否存在，若否則新增
        /// </summary>
        /// <param name="file_path"></param>
        /// <returns></returns>
        private static void CheckExistsAndCreateFolder(string file_path)
        {
            if (Directory.Exists(file_path) == false)
            {
                Directory.CreateDirectory(file_path);
            }
        }


        /// <summary>
        /// 記錄log
        /// </summary>
        /// <param name="message">文字訊息</param>
        /// <param name="level">紀錄層級</param>
        /// <param name="log_type">記錄分類</param>
        /// <param name="save_path">記錄路徑</param>
        private static void SaveLogMessage(string message, string level, string actionName, string csName)
        {
            try
            {
                actionName = string.IsNullOrWhiteSpace(actionName) ? "SaveLogMessage" : actionName;
                csName = string.IsNullOrWhiteSpace(csName) ? "LogTool" : csName;
                string current_log_path = LogTool.DefaultPath;
                // log file name
                string dic_path = current_log_path + "log\\" + (string.IsNullOrEmpty(csName) ? "" : actionName + "\\");
                string file_name = actionName + "_" + DateTime.Now.ToString("yyyy_MM") + ".log";
                string log_file_name = dic_path + file_name;
                LogTool.CheckExistsAndCreateFolder(dic_path);

                string log_message = string.Concat("[",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), " ",
                    Environment.GetEnvironmentVariable("COMPUTERNAME"), "]",
                    " ", level, " ", message, "\n");

                using (StreamWriter sw = File.AppendText(log_file_name))
                {
                    sw.WriteLine(log_message);
                    sw.Close();
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// 記錄 Error log
        /// </summary>
        /// <param name="ex">例外物件</param>
        /// <param name="log_type">記錄分類</param>
        /// <param name="save_path">記錄路徑</param>
        public static void Error(Exception ex, string actionName = "", string csName = "")
        {
            string save_message = ex.Message.ToString() + "\n" + ex.StackTrace.ToString();
            LogTool.SaveLogMessage(save_message, "Error", actionName, csName);
        }

        /// <summary>
        /// 記錄 Error log
        /// </summary>
        /// <param name="ex">例外物件</param>
        /// <param name="log_type">記錄分類</param>
        /// <param name="save_path">記錄路徑</param>
        public static void Error(string msg, string actionName = "", string csName = "")
        {
            LogTool.SaveLogMessage(msg, "Error", actionName, csName);
        }

        /// <summary>
        /// 記錄 Warn log
        /// </summary>
        /// <param name="msg">例外物件</param>
        /// <param name="log_type">記錄分類</param>
        /// <param name="save_path">記錄路徑</param>
        public static void Warn(string msg, string actionName = "", string csName = "")
        {
            LogTool.SaveLogMessage(msg, "Warn", actionName, csName);
        }

        /// <summary>
        /// 記錄 Fatal log
        /// </summary>
        /// <param name="msg">例外物件</param>
        /// <param name="log_type">記錄分類</param>
        /// <param name="save_path">記錄路徑</param>
        public static void Fatal(string msg, string actionName = "", string csName = "")
        {
            LogTool.SaveLogMessage(msg, "Fatal", actionName, csName);
        }

        /// <summary>
        /// 記錄log
        /// </summary>
        /// <param name="msg">例外物件</param>
        /// <param name="log_type">記錄分類</param>
        /// <param name="save_path">記錄路徑</param>
        public static void Fatal(Exception ex, string actionName = "", string csName = "")
        {
            string save_message = ex.Message.ToString() + "\n" + ex.StackTrace.ToString();
            LogTool.SaveLogMessage(save_message, "Fatal", actionName, csName);
        }

        /// <summary>
        /// 記錄 Debug log
        /// </summary>
        /// <param name="msg">例外物件</param>
        /// <param name="log_type">記錄分類</param>
        /// <param name="save_path">記錄路徑</param>
        public static void Debug(string msg, string actionName = "", string csName = "")
        {
            LogTool.SaveLogMessage(msg, "Debug", actionName, csName);
        }

        /// <summary>
        /// 記錄 Info log
        /// </summary>
        /// <param name="msg">例外物件</param>
        /// <param name="log_type">記錄分類</param>
        /// <param name="save_path">記錄路徑</param>
        public static void Info(string msg, string actionName = "", string csName = "")
        {
            LogTool.SaveLogMessage(msg, "Info", actionName, csName);
        }
    }

 
    #region 回傳訊息

    public enum RESPONSE_STATUS
    {
        SUCCESS = 0,
        ERROR = 1,
        EXCEPTION = 2,
        DUPLICATE = 3,
        /// <summary>
        /// 提醒 , 注意
        /// </summary>
        WARN = 4
    }

    public enum RESPONSE_ACTION
    {
        INSERT = 0,
        UPDATE = 1
    }

    /// <summary> 回傳值 </summary>
    public class RESPONSE_MSG
    {

        /// <summary> 處理狀態 </summary>
        public RESPONSE_STATUS status { set; get; }

        /// <summary> 傳回訊息或內容 </summary>
        public string message { set; get; }

        /// <summary> 附帶物件 </summary>
        public object attachment { set; get; }

        /// <summary> 取得序列化結果 </summary>
        public string get_json()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void setErrorMsg(string msgStr)
        {
            this.status = RESPONSE_STATUS.ERROR;
            this.message = msgStr;
        }

        public void setRESPONSE_MSG(RESPONSE_MSG rm)
        {
            this.status = rm.status;
            this.message = rm.message;
            this.attachment = rm.attachment;
        }
    }

    #endregion
}