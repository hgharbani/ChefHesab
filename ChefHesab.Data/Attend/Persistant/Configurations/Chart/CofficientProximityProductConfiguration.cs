using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ksc.Hr.Domain.Entities;
namespace Ksc.Hr.Data.Persistant.Configurations
{
    public class CofficientProximityProductConfiguration : IEntityTypeConfiguration<CofficientProximityProduct>
    {
        public void Configure(EntityTypeBuilder<CofficientProximityProduct> builder)
        {
            // Table & Column Mappings
            builder.ToTable("CofficientProximityProduct", "Chart");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.Score).HasColumnName("Score");
            builder.Property(t => t.Active).HasColumnName("Active");


        }
    }
}

