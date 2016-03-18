using System;

namespace Silicus.Finder.Web
{
    public interface ICookieHelper
    {
        void SetCookie(string key, string value, TimeSpan expires);
        string GetCookie(string key);
        void ClearAllCookies();
    }
}