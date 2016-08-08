using Microsoft.Win32.SafeHandles;
using Silicus.UtilityContainer.Entities;
using Silicus.UtilityContainer.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace HangFireBackgroundTasks.Services
{

    public class HolidayService : IDisposable
    {
        public IDataContextFactory contextFactory;
        public ICommonDataBaseContext context;
        bool disposed = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        public HolidayService()
        {
            contextFactory = new DataContextFactory();
            context = contextFactory.CreateCommonDBContext();
        }

        public List<int> GetCurrentMonthHolidays()
        {
            return context.Query<Holiday>().Where(p => p.EventDate.Month == DateTime.Now.Month && p.EventDate.Year == DateTime.Now.Year).Select(p=>p.EventDate.Day).ToList();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                // Free any other managed objects here.
                //
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }
    }
}
