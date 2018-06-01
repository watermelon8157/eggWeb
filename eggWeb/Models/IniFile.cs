using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eggWeb.Models
{
    public static class IniFile
    {
        /// <summary>
        /// DES encode A key
        /// </summary>
        public readonly static string akey = "aaaaaaaa";

        /// <summary>
        /// DES encode B key
        /// </summary>
        public readonly static string bkey = "bbbbbbbb";

        /// <summary>
        /// ini file path
        /// </summary>
        public readonly static string ini_path = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\EGGConfig.ini";

        /// <summary>
        /// 取得連線字串
        /// </summary>
        /// <returns>連線字串</returns>
        public static string GetConnStr(string ConnectionSession)
        {
            string connstr = GetConfig(ConnectionSession, "DBConnStr").Trim();
            string password = SecurityTool.DecodeDES(GetConfig(ConnectionSession, "Password"), akey, bkey).Trim();
            return string.Format(connstr, password);
        }

        /// <summary>
        /// 取得設定檔的值
        /// </summary>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfig(string session, string key)
        {
            LogTool.Info(ini_path, "GetConfig", "GetConfig");
            string line = "", head = "", result = "";
            try
            {
                StreamReader sr = new StreamReader(ini_path, Encoding.Default);

                //String[] value;
                char[] spChr = { '=' };

                while ((line = sr.ReadLine()) != null)
                {
                    if (line.IndexOf("[") != -1 && line.IndexOf("]") != -1)
                    {
                        head = line.Replace("[", "").Replace("]", "");
                        continue;
                    }
                    else
                    {
                        if (head == session)
                        {
                            if (line.StartsWith(key))
                            {
                                result = line.Substring(line.IndexOf("=") + 1);
                                break;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                sr.Close();
                line = null;
                head = null;
                spChr = null;
                sr = null;
            }
            catch (Exception ex)
            {
                LogTool.Fatal(ini_path, "GetConfig", "GetConfig");
            }
            LogTool.Info(result, "GetConfig", "GetConfig");
            return result;
        }

    }
}
