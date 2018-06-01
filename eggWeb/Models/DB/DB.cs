using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eggWeb.Models.viewmodel
{

    public abstract class DB_EGG_INFO_Q_MST : DB_BASE
    {
        public string M_ID { get; set; }
        public string M_DESC { get; set; }
        public string Q_DESC { get; set; }

        public string Q_MEMO { get; set; }

    }

    public abstract class DB_EGG_EVENT_DATA : DB_BASE
    {
        public string EVENT_TITLE { get; set; }

        public string EVENT_URL { get; set; }

        public string EVENT_DESC { get; set; }

        public string EVENT_DATE { get; set; }

        public string EVENT_IMAGE { get; set; }

        public string EVENT_FILE { get; set; }
    }

    public abstract class DB_EGG_TEST_INFO : DB_BASE
    {
        public string USER_ID { get; set; }
 

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

    public abstract class DB_EGG_SYS_PARAMS: DB_BASE
    {
        public string S_MODEL { get; set; }

        public string S_GROUP { get; set; }

        public string S_ID { get; set; }

        public string S_NAME { get; set; }

        public string S_VALUE { get; set; }

        public string S_STATUS { get; set; }

        public string S_MANAGE { get; set; }

    }

    public abstract class DB_BASE
    {
        public string CREATE_ID { get; set; }

        public string CREATE_NAME { get; set; }

        public string CREATE_DATE { get; set; }

        public string MODIFY_ID { get; set; }

        public string MODIFY_NAME { get; set; }

        public string MODIFY_DATE { get; set; }
    }

}