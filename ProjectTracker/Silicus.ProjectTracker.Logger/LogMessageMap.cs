using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Eda.RDBI.Logger
{
    public class LogMessageMap : EntityTypeConfiguration<LogMessage>
    {
        public LogMessageMap()
        {
            HasKey(o => o.LogMessageId);

            Property(p => p.LogMessageId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}