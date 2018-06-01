using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace eggWeb.Models
{
    public static class SecurityTool
    {

        /// <summary> 
        /// DES 加密字串 
        /// </summary> 
        /// <span  name="original" class="mceItemParam"></span>原始字串</param> 
        /// <span  name="key" class="mceItemParam"></span>Key，長度必須為 8 個 ASCII 字元</param> 
        /// <span  name="iv" class="mceItemParam"></span>IV，長度必須為 8 個 ASCII 字元</param> 
        /// <returns></returns> 
        public static string EncodeDES(string original, string key, string iv)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                des.Key = Encoding.ASCII.GetBytes(key);
                des.IV = Encoding.ASCII.GetBytes(iv);
                byte[] s = Encoding.ASCII.GetBytes(original);
                ICryptoTransform desencrypt = des.CreateEncryptor();
                return BitConverter.ToString(desencrypt.TransformFinalBlock(s, 0, s.Length)).Replace("-", string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary> 
        /// DES 解密字串 
        /// </summary> 
        /// <span  name="hexString" class="mceItemParam"></span>加密後 Hex String</param> 
        /// <span  name="key" class="mceItemParam"></span>Key，長度必須為 8 個 ASCII 字元</param> 
        /// <span  name="iv" class="mceItemParam"></span>IV，長度必須為 8 個 ASCII 字元</param> 
        /// <returns></returns> 
        public static string DecodeDES(string hexString, string key, string iv)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                des.Key = Encoding.ASCII.GetBytes(key);
                des.IV = Encoding.ASCII.GetBytes(iv);
                byte[] s = new byte[hexString.Length / 2];
                int j = 0;
                for (int i = 0; i < hexString.Length / 2; i++)
                {
                    s[i] = Byte.Parse(hexString[j].ToString() + hexString[j + 1].ToString(),  NumberStyles.HexNumber);
                    j += 2;
                }
                ICryptoTransform desencrypt = des.CreateDecryptor();
                return Encoding.ASCII.GetString(desencrypt.TransformFinalBlock(s, 0, s.Length));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary> 
        /// 取得 MD5 編碼後的 Hex 字串 
        /// 加密後為 32 Bytes Hex String (16 Byte) 
        /// </summary> 
        /// <span  name="original" class="mceItemParam"></span>原始字串</param> 
        /// <returns></returns> 
        public static string EncodeMD5(string original)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] b = md5.ComputeHash(Encoding.UTF8.GetBytes(original));
            return BitConverter.ToString(b).Replace("-", string.Empty);
        }



    }
}
