namespace Silicus.ProjectTracker.Auditing
{
    public interface IAuditManager
    {
        void WriteAudit(string userName, string operationName, AuditInformation auditInformation);
    }
}
