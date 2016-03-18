namespace Silicus.ProjectTracker.Logger
{
    public interface IWindowsEventLogger
    {
        void WriteToEventLog(string strLogName, string strSource, string strErrDetail);
    }
}
