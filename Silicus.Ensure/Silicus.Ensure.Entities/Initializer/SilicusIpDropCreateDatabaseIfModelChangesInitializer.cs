﻿using System.Data.Entity;

namespace Silicus.Ensure.Entities.Initializer
{
    public class SilicusIpDropCreateDatabaseIfModelChangesInitializer : DropCreateDatabaseIfModelChanges<SilicusIpDataContext>
    {
        protected override void Seed(SilicusIpDataContext context)
        {
            BaseDatabaseInitializer.Seed(context);
        }
    }
}