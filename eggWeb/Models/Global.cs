using eggWeb.Models.DB;
using eggWeb.Models.viewmodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eggWeb.Models
{
    public class Global
    {
        string csName { get { return "Global"; } }
        /// <summary>
        /// 取得蛋知識
        /// </summary>
        public List<EGG_INFO_Q_MST> egg_info_list { get; set; }

        /// <summary>
        /// 取得蛋事件
        /// </summary>
        public List<EGG_EVENT_DATA> egg_event_list { get; set; }

        public Global()
        {
            string actioName = "Global";

            #region egg_info_list 取得蛋知識

            this.egg_info_list = new List<EGG_INFO_Q_MST>();
            
            try
            {
                DBLink link = new DBLink();
                LogTool.Info(link.dbPath, actioName, this.csName);
                string sql = "SELECT * FROM EGG_INFO_Q_MST";
                this.egg_info_list = link.DBA.getSqlDataTable<EGG_INFO_Q_MST>(sql);
                if (link.DBA.hasLastError)
                {
                    LogTool.Fatal(link.DBA.lastError, actioName, this.csName);
                }
            }
            catch (Exception ex)
            {
                LogTool.Fatal(ex, actioName, this.csName);
            }

            #endregion

            #region egg_event_list 取得蛋事件

            this.egg_event_list = new List<EGG_EVENT_DATA>();
           
            try
            {
                DBLink link = new DBLink();
                LogTool.Info(link.dbPath, actioName, this.csName);
                string sql = "SELECT * FROM EGG_EVENT_DATA WHERE EVENT_IMAGE <> '0'";
                this.egg_event_list = link.DBA.getSqlDataTable<EGG_EVENT_DATA>(sql);
                if (link.DBA.hasLastError)
                {
                    LogTool.Fatal(link.DBA.lastError, actioName, this.csName);
                }
            }
            catch (Exception ex)
            {
                LogTool.Fatal(ex, actioName, this.csName);
            }

            #endregion
        }


    }
}