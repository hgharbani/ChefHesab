using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ksc.Hr.Domain.Entities;
namespace Ksc.Hr.Data.Persistant.Configurations
{
    public class ConfirmInterdictStatusMap : IEntityTypeConfiguration<ConfirmInterdictStatus>
    {
        public void Configure(EntityTypeBuilder<ConfirmInterdictStatus> builder)
        {
            // Table & Column Mappings
            builder.ToTable("ConfirmInterdictStatus", "Rule");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.Title).HasColumnName("Title").IsRequired().HasMaxLength(50);




        }
    }
}

