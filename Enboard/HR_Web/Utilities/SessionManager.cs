using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace HR_Web.Utilities
{
    public static class SessionManager
    {
        public static int? RoleId
        {
            get
            {
                if (null != HttpContext.Current.Session["RoleId"])
                    return Convert.ToInt32(HttpContext.Current.Session["RoleId"]);
                else
                    return -1;
            }
            set
            {
                HttpContext.Current.Session["RoleId"] = value;
            }
        }

        public static long UserId
        {
            get
            {
                if (null != HttpContext.Current.Session["UserId"])
                    return Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                else
                    return -1;
            }
            set
            {
                HttpContext.Current.Session["UserId"] = value;
            }
        }
        public static string IsActive(this HtmlHelper html,
                                  string control,
                                  string action)
        {
            var routeData = html.ViewContext.RouteData;

            var routeAction = (string)routeData.Values["action"];
            var routeControl = (string)routeData.Values["controller"];

            // both must match
            var returnActive = control == routeControl &&
                               action == routeAction;

            return returnActive ? "active" : "";
        }
        public static bool IsOnBoarded { get; set; }

        public static string EncryptData(this string ActualPwd)
        {

            string strmsg = string.Empty;
            byte[] encode = new byte[ActualPwd.Length];
            encode = Encoding.UTF8.GetBytes(ActualPwd);
            strmsg = Convert.ToBase64String(encode);
            return strmsg;
         
            //HashAlgorithm hash = new SHA256Managed();
            //byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(ActualPwd);
            //byte[] hashBytes = hash.ComputeHash(plainTextBytes);

            ////in this string you got the encrypted password
            //string hashValue = Convert.ToBase64String(hashBytes);
            //return hashValue;
        }

        public static string DecryptData(this string password)
        {

            string decryptpwd = string.Empty;
            UTF8Encoding encodepwd = new UTF8Encoding();
            Decoder Decode = encodepwd.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(password);
            int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            decryptpwd = new String(decoded_char);
            return decryptpwd;
            //System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            //SHA256Managed sha256hasher = new SHA256Managed();
            //byte[] hashedDataBytes = sha256hasher.ComputeHash(encoder.GetBytes(password));
            //return Convert.ToBase64String(hashedDataBytes);
        }
        public static DateTime ?LastLogin { get; set; }
    }

}