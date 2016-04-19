﻿using Silicus.UtilityContainer.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.UtilityContainerr.Entities.EntityConfigurations
{
    internal class ResourceHistoryMap : EntityTypeConfiguration<ResourceHistory>
    {
        public ResourceHistoryMap()
        {
            HasKey(o => o.ID);

            Property(p => p.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable(TableSettings.EngagementHistory, TableSettings.DefaultSchema);
        }
    }
}