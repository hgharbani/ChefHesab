using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ksc.Hr.Domain.Entities;
namespace Ksc.Hr.Data.Persistant.Configurations
{
    public class ProductionEfficiencyMap : IEntityTypeConfiguration<ProductionEfficiency>
    {
        public void Configure(EntityTypeBuilder<ProductionEfficiency> builder)
        {
            // Table & Column Mappings
            builder.ToTable("ProductionEfficiency", "Chart");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.Title).HasColumnName("Title");
            builder.Property(t => t.Code).HasColumnName("Code");
            builder.Property(t => t.Active).HasColumnName("Active");
            builder.Property(t => t.CPercent).HasColumnName("CPercent");

            
        }
    }
}

