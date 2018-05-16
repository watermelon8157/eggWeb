using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using acgDBA;
using log4net;
using System.Data.Common;
using System.Data.SQLite;

namespace eggWeb.Models.DB
{
    public class DBLink
    {
        /// <summary>
        /// DB路徑
        /// </summary>
        public string dbPath { get { return AppDomain.CurrentDomain.BaseDirectory + "\\Models\\DB\\EGG_DB.db"; } }
        /// <summary>
        /// 資料庫來源
        /// </summary>
        public string cnStr { get { return string.Concat("data source = " , dbPath); } }
        /// <summary>
        /// 資料庫連線
        /// </summary>
        protected string ConnectionSession { get; set; }

        /// <summary> DB Object, 對 DB 的操作都由此 Object 處理 </summary>
        public DBConnector DBA { get; set; }

        /// <summary>
        /// 建立資料庫連線
        /// <para>MS_SQL :MS_DbConnection</para>
        /// <para>Oracle :Oracle_DbConnection</para>
        /// </summary>
        /// <param name="ConnectionSession">預設:MS_DbConnection</param>
        public DBLink(string pConnectionSession = "SQLite_DbConnection")
        {

            this.ConnectionSession = pConnectionSession;
            DBA = new SQLiteDBConnector(cnStr);
        }
    }

    #region SQLite Provider

    public class SQLiteDBConnector : DBConnector
    {
        public SQLiteDBConnector(string ConnectionString, ILog Logger = null, int? DefaultTimeout = null) : base(new SQLiteProvider(), ConnectionString, Logger, DefaultTimeout)
        {

        }
    }

    public class SQLiteProvider : IDBProvider
    {
        public DbConnection CreateConnectionObject()
        {
            return new SQLiteConnection();
        }

        public DbCommand CreateCommandObject()
        {
            return new SQLiteCommand();
        }

        public DbCommandBuilder CreateCommandBuilder()
        {
            return new SQLiteCommandBuilder();
        }

        public DbDataAdapter CreateDataAdapter()
        {
            return new SQLiteDataAdapter();
        }

        public enmDBAProvider enmDBType { get { return enmDBAProvider.MS; } }

    }


    #endregion
}