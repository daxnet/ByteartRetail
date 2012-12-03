using ByteartRetail.Domain.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ByteartRetail.Domain.Repositories.EntityFramework.ModelConfigurations
{
    public class ShoppingCartTypeConfiguration : EntityTypeConfiguration<ShoppingCart>
    {
        public ShoppingCartTypeConfiguration()
        {
            HasKey(c => c.ID);
            Property(c => c.ID)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
