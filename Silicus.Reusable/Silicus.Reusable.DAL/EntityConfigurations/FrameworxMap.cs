﻿using Silicus.FrameworxProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.FrameworxProject.DAL.EntityConfigurations
{
    internal class FrameworxMap : EntityTypeConfiguration<Frameworx>
    {
        public FrameworxMap()
        {
            HasKey(o => o.Id);
        }
    }
}
