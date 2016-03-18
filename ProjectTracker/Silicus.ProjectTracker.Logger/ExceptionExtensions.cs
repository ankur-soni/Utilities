using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Silicus.ProjectTracker.Logger
{
    /// <summary>
    /// Extension methods for the <see cref="Exception"/> type.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ExceptionExtensions
    {
        private static readonly int MAX_LENGHT = 500;

        /// <summary>
        /// Convert Exception object into a string representation for Logging purpose.
        /// </summary>
        /// <param name="exception">Input Exception object.</param>
        /// <returns>A string representation of Exception object.</returns>
        public static string ToLoggableString(this Exception exception)
        {
            var sb = new StringBuilder();

            while (exception != null)
            {
                string message = exception.Message;
                if (string.IsNullOrEmpty(message))
                {
                    sb.Append(exception.GetType());
                }
                else
                {
                    sb.Append(exception.GetType());
                    sb.Append(": ");
                    sb.Append(message);
                }

                if (exception.StackTrace != null)
                {
                    sb.AppendLine();

                    sb.Append(exception.StackTrace.Length >= MAX_LENGHT ? exception.StackTrace.Substring(0, MAX_LENGHT) : exception.StackTrace);
                }

                sb.AppendLine();

                exception = exception.InnerException;
            }

            return sb.Replace(Environment.NewLine, " ").ToString();
        }
    }
}
