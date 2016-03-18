using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Silicus.Finder.Models.DataObjects;


namespace Silicus.Finder.Entities.EntityConfigurations
{
    internal class ContactMap : EntityTypeConfiguration<Contact>
    {
        public ContactMap()
        {
            HasKey(o => o.ContactId);

            Property(p => p.ContactId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable(TableSettings.Contacts, TableSettings.DefaultSchema);


        }
    }
}