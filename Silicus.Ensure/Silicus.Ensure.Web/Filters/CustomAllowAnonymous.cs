using System;

namespace Silicus.Ensure.Web.Filters
{
    /// <summary>
    /// Specifies that actions and controllers are skipped by <see cref="T:System.Web.Http.AuthorizeAttribute"/> during authorization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class CandidateAttribute : Attribute
    {
    }
}