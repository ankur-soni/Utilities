﻿using Silicus.UtilityContainer.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.UtilityContainer.Entities.EntityConfigurations
{
    internal class EngagementUserPermissionsMap : EntityTypeConfiguration<EngagementUserPermission>
    {
        public EngagementUserPermissionsMap()
        {
            HasKey(o => o.ID);

            Property(p => p.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable(TableSettings.EngagementUserPermissions, TableSettings.DefaultSchema);
        }
    } 
}
