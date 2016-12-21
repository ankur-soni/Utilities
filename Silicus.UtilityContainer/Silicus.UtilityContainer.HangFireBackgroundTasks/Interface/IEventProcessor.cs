using HangFireBackgroundTasks.Enums;
using Silicus.UtilityContainer.Models;
using Silicus.UtilityContainer.Models.Enumerations;

namespace Silicus.UtilityContainer.HangFireBackgroundTasks.Interface
{
    public interface IEventProcessor
    {
        void Process(EventType eventType,EventProcess eventProcess, string  awardName);
      
    }
}
