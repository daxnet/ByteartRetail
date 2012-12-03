using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using ByteartRetail.Domain.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByteartRetail.Domain.Repositories.EntityFramework.ModelConfigurations
{
    public class SalesLineTypeConfiguration : EntityTypeConfiguration<SalesLine>
    {
        public SalesLineTypeConfiguration()
        {
            HasKey<Guid>(s => s.ID);
            Property(p => p.ID)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            HasRequired(p => p.SalesOrder)
                .WithMany(p => p.SalesLines);
            Ignore(p => p.LineAmount);
        }
    }
}
