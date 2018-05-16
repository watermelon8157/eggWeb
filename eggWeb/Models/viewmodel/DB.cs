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

    public abstract class DB_EGG_INFO_Q_MST
    {
        public string Q_DESC { get; set; }

        public string Q_MEMO { get; set; }

    }
}