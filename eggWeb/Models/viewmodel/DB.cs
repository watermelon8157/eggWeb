using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eggWeb.Models.viewmodel
{
    public class VM_EGG
    {
        
    }

 
    public class EGG_INFO_Q_MST: DB_EGG_INFO_Q_MST
    { 

    }

    public class EGG_EVENT_DATA:DB_EGG_EVENT_DATA
    {

    }

    public abstract class DB_EGG_INFO_Q_MST
    {
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

}