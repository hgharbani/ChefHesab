using Ksc.HR.Domain.Entities.Rule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ksc.HR.Data.Persistant.Configurations.Rule
{
    public class EmployeePromotionInterdictsConfiguration : IEntityTypeConfiguration<EmployeePromotionInterdicts>
    {
        public void Configure(EntityTypeBuilder<EmployeePromotionInterdicts> builder)
        {
            // Table & Column Mappings
            builder.ToTable("EmployeePromotionInterdicts", "Rule");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.EmployeeInterdictId).HasColumnName("EmployeeInterdictId");
            builder.Property(t => t.EmployeePromotionId).HasColumnName("EmployeePromotionId");

        }
    }
}
