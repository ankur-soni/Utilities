using System;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Silicus.ProjectTracker.Logger;
using WebGrease.Css.Extensions;

namespace Silicus.ProjectTracker.Web.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class LogAttribute : ActionFilterAttribute
    {
        private readonly ILogger _logger;

        public LogAttribute()
        {
            _logger = new DatabaseLogger("name=RdbiLoggerDataContext", null, (Func<DateTime>)(() => DateTime.UtcNow));
        }

        internal LogAttribute(ILogger logger)
        {
            _logger = logger;
        }

        internal string ActionName { get; set; }

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            var parameterList = new StringBuilder();
            try
            {
                if (actionContext.ActionParameters.Count > 0)
                {
                    actionContext.ActionParameters.ForEach(a => parameterList.Append(a.Key + " = " + a.Value.ToString() + ", "));
                }
            }
            catch
            {
                // Not important step so ignore an exception
            }

            LogRequest(actionContext, "OnActionExecuting", actionContext.ActionDescriptor.ActionName, parameterList.ToString());
        }

        public override void OnActionExecuted(ActionExecutedContext actionContext)
        {
            LogRequest(actionContext, "OnActionExecuted", actionContext.ActionDescriptor.ActionName);
        }

        private void LogRequest(ControllerContext actionContext, string context, string actName = "", string parameterList = "")
        {
            var request = actionContext.HttpContext.Request;
            var actionName = request.RawUrl ?? actName;

            try
            {
                string userName = (request.IsAuthenticated)
                    ? actionContext.HttpContext.User.Identity.Name
                    : request.Params["userName"];

                if(string.IsNullOrEmpty(actionName))
                 actionName = ActionName;

                var sessionId = request.Params["ASP.NET_SessionId"] + "-" + userName;

                Task.Run(
                    () =>
                        _logger.Log(
                            string.Format("{0} by UserName: {1}, ActionName: {2}, Params: {3}", context, userName, actionName, parameterList),
                            LogCategory.Verbose, sessionId));
            }
            catch
            {
            }
        }
    }
}