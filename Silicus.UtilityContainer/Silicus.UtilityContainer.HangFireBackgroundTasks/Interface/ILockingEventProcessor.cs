using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangFireBackgroundTasks.Interface
{
    public interface ILockingEventProcessor
    {
        void setLockForNomination();
    }
}
