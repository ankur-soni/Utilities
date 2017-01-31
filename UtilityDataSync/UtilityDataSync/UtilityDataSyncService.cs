using System;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Timers;
using UtilityDataSyncLibrary;
using UtilityDataSyncLibrary.Mapping;

namespace UtilityDataSync
{
    public partial class UtilityDataSyncService : ServiceBase
    {
        private static IMappingService _mappingService;
        public Timer Timer;
        public UtilityDataSyncService()
        {
            InitializeComponent();
            AutoMapperConfiguration.Configure();
            _mappingService = new MappingService();
            Timer = new Timer(10000);
        }
        
        protected override void OnStart(string[] args)
        {
            Timer.Elapsed += SyncData;
            var ineterval = 1;
            Timer.Interval = (60 * ineterval * 1000);
            Timer.Enabled = true;
            Timer.Start();
            EventLog.WriteEntry("Service Started", EventLogEntryType.Information);
        }

        public void SyncData(object sender, ElapsedEventArgs e)
        {
            Timer.Stop();
            try
            {
                EventLog.WriteEntry("Sync Started", EventLogEntryType.Information);

                var syncFunctions = new SyncFunctions();
                syncFunctions.SyncData();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Error occured Started : " + ex.Message, EventLogEntryType.Error);
            }
            Timer.Start();
        }
        
        protected override void OnStop()
        {

        }
    }
}
