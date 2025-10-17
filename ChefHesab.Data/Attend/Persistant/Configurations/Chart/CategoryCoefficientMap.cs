using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ksc.Hr.Domain.Entities;
namespace Ksc.Hr.Data.Persistant.Configurations
{
    public class CategoryCoefficientMap : IEntityTypeConfiguration<CategoryCoefficient>
    {
        public void Configure(EntityTypeBuilder<CategoryCoefficient> builder)
        {
            // Table & Column Mappings
            builder.ToTable("CategoryCoefficient", "Chart");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.Score).HasColumnName("Score");
            builder.Property(t => t.Active).HasColumnName("Active");


        }
    }
}

