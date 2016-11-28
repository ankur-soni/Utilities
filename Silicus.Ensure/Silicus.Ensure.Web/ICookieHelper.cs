using System;

namespace Silicus.Ensure.Web
{
    public interface ICookieHelper
    {
        void SetCookie(string key, string value, TimeSpan expires);
        string GetCookie(string key);
        void ClearAllCookies();
    }
}