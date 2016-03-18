using System;
using Newtonsoft.Json;

namespace Silicus.ProjectTracker.Auditing
{
    public class AuditManager : IAuditManager
    {
        private readonly string _connectionString;
        private static readonly Func<DateTime> DefaultDateGetter = () => DateTime.UtcNow;
        private Func<DateTime> _dateGetter = DefaultDateGetter;
        private bool _disposed;
        private IDataContext _dbContext;

        public Func<DateTime> DateGetter
        {
            set
            {
                _dateGetter = value;
            }
        }

        public AuditManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void WriteAudit(string userName, string operationName, AuditInformation auditInformation)
        {
            if (auditInformation == null)
            {
                return;
            }

            var jsonString = JsonConvert.SerializeObject(auditInformation, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            var auditMessage = new AuditMessage
            {
                Timestamp = _dateGetter(),
                UserName = userName,
                OperationName = operationName,
                Data = jsonString
            };

            using (var auditingContext = new AuditingContext(_connectionString))
            {
                auditingContext.Add(auditMessage);
            }
        }
    }
}