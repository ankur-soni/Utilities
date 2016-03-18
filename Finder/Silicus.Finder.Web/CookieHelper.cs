using System;
using System.Web;

namespace Silicus.Finder.Web
{
    public class CookieHelper : ICookieHelper
    {
        public void SetCookie(string key, string value, TimeSpan expires)
        {
            var encodedCookie = new HttpCookie(key, value);

            encodedCookie.HttpOnly = true;

            if (HttpContext.Current.Request.Cookies[key] != null)
            {
                var cookieOld = HttpContext.Current.Request.Cookies[key];
                cookieOld.Expires = DateTime.Now.Add(expires);
                cookieOld.Value = encodedCookie.Value;
                HttpContext.Current.Response.Cookies.Add(cookieOld);
            }
            else
            {
                encodedCookie.Expires = DateTime.Now.Add(expires);
                HttpContext.Current.Response.Cookies.Add(encodedCookie);
            }
        }

        public string GetCookie(string key)
        {
            string value = string.Empty;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];

            if (cookie != null)
            {
                // For security purpose, we need to encrypt the value.
                HttpCookie decodedCookie = cookie;
                value = decodedCookie.Value;
            }
            return value;
        }

	    public void ClearAllCookies()
	    {
			string[] cookies = HttpContext.Current.Request.Cookies.AllKeys;
	        foreach (string cookie in cookies)
	        {
		        try
		        {
			        HttpContext.Current.Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
		        }
		        catch (Exception ex)
		        {
			        // Trace Error
		        }
	        }
	    }
    }
}