using Silicus.FrameworxProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Threading.Tasks;

namespace Silicus.FrameworxProject.DAL.EntityConfigurations
{
    internal class ExtensionSolutionMap : EntityTypeConfiguration<ExtensionSolution>
    {
        public ExtensionSolutionMap()
        {
            HasKey(o => o.Id);
        }
    }
}
