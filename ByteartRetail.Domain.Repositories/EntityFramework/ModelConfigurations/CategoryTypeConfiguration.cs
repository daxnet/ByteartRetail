using ByteartRetail.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace ByteartRetail.Domain.Repositories.EntityFramework.ModelConfigurations
{
    public class CategoryTypeConfiguration : EntityTypeConfiguration<Category>
    {
        public CategoryTypeConfiguration()
        {
            HasKey<Guid>(c => c.ID);
            Property(c => c.ID)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(25);
        }
    }
}
