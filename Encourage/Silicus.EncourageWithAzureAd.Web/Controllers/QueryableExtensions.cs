namespace Silicus.EncourageWithAzureAd.Web.Controllers
{
    using Silicus.Encourage.DAL.Interfaces;
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// This class provides an extension method to IQureyable interface.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class QueryableExtensions
    {
        private static IIncluder _includer = new NullIncluder();

        internal static IIncluder Includer
        {
            set { _includer = value; }
        }

        public static IQueryable<T> Include<T, TProperty>(this IQueryable<T> source, Expression<Func<T, TProperty>> path)
             where T : class
        {
           return _includer.Include(source, path);
        }

        internal class NullIncluder : IIncluder
        {
            public IQueryable<T> Include<T, TProperty>(IQueryable<T> source, Expression<Func<T, TProperty>> path)
                 where T : class
            {
               return source;
            }
        }
    }
}
