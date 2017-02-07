using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityDataSyncLibrary;

namespace UtilityDataSyncService
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var syncFunctions = new SyncFunctions();
                syncFunctions.SyncData();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
