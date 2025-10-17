using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ksc.Hr.Domain.Entities;
namespace Ksc.Hr.Data.Persistant.Configurations
{
    public class EmployeeHaveTravelConfiguration : IEntityTypeConfiguration<EmployeeHaveTravel>
    {
        public void Configure(EntityTypeBuilder<EmployeeHaveTravel> builder)
        {
            // Table & Column Mappings
            builder.ToTable("EmployeeHaveTravel", "dbo");


            builder.Property(t => t.Id).HasColumnName("Id");
            builder.HasKey(x => x.Id).HasName("PK_EmployeeHaveTravel").IsClustered();
            builder.Property(t => t.EmployeeId).HasColumnName("EmployeeId");
            builder.Property(t => t.CheckDate).HasColumnName("CheckDate");
        }
    }
}

