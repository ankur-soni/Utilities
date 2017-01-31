using System;
using UtilityDataSyncLibrary;

namespace TestService
{
    class Program
    {
        public Program()
        {
            
        }

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
