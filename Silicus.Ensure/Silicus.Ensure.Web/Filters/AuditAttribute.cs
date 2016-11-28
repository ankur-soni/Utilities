using System;
using System.Web.Mvc;
using Silicus.FrameWorx.Auditing;
using Silicus.FrameWorx.Utility;

namespace Silicus.Ensure.Web.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class AuditAttribute : ActionFilterAttribute
    {
        private readonly IAuditManager _auditManager;

        public AuditAttribute()
        {
            _auditManager = new AuditManager("name=SilicusAuditingDataContext");
        }

        internal AuditAttribute(IAuditManager auditManager)
        {
            _auditManager = auditManager;
        }

        public int AuditingLevel { get; set; }
        public string ActionName { get; set; }

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            try
            {
                var request = actionContext.HttpContext.Request;

                var userName = (request.IsAuthenticated)
                    ? actionContext.HttpContext.User.Identity.Name
                    : request.Params["userName"];

                if (string.IsNullOrEmpty(userName))
                {
                    userName = actionContext.ActionParameters.ContainsKey("userName") 
                        ? actionContext.ActionParameters["userName"].ToString() 
                        : "AnonymousUser";
                }

                var operationName = ActionName ?? actionContext.ActionDescriptor.ActionName;

                var data = string.Empty;

                if (actionContext.ActionParameters.ContainsKey("data"))
                {
                    data = actionContext.ActionParameters["data"].ToString();
                }

                var auditInformation = new AuditInformation();

                if (!string.IsNullOrEmpty(data))
                {
                    var decryptedData = RijndaelEncryptionHelper.DecryptString(data);
                    auditInformation.Data = decryptedData;
                }
                else
                {
                    auditInformation.Data = string.Format("RRID: {0}, DownloadCode: {1}, PartnerKey: {2}", request.Params["rrid"], request.Params["downloadCode"], request.Params["partnerKey"]);
                };

                _auditManager.WriteAudit(userName, operationName, auditInformation);
            }
            catch (Exception ex)
            {
                // Something went wrong, very unlikely case. Not appropriate to
                // throw from here.  Logging at this point is not appropriate
                // either.
                System.Diagnostics.Trace.WriteLine("Error occured while making an audit entry.");
                System.Diagnostics.Trace.WriteLine(ex.Message);
            }
        }
    }
}