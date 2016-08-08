using Silicus.UtilityContainer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.UtilityContainer.HangFireBackgroundTasks.Interface
{
   public interface IEventProcessor
    {
        void Process(EventType eventType);
      
    }
}
