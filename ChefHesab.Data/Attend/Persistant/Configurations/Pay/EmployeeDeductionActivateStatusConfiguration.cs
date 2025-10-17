using Ksc.HR.Domain.Entities.Pay;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ksc.HR.Data.Persistant.Configurations.Pay;

public class EmployeeDeductionActivateStatusConfiguration : IEntityTypeConfiguration<EmployeeDeductionActivateStatus>
{
    public void Configure(EntityTypeBuilder<EmployeeDeductionActivateStatus> builder)
    {
        builder.ToTable("EmployeeDeductionActivateStatus", "Pay");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
        builder.Property(x => x.YearMonth).HasColumnName(@"YearMonth").HasColumnType("int").IsRequired();
        builder.Property(x => x.IsActive).HasColumnName(@"IsActive").HasColumnType("bit").IsRequired();
        builder.Property(x => x.InsertDate).HasColumnName(@"InsertDate").HasColumnType("datetime").IsRequired();
        builder.Property(x => x.InsertUser).HasColumnName(@"InsertUser").HasColumnType("varchar(50)").IsRequired().HasMaxLength(50);
        builder.Property(x => x.UpdateDate).HasColumnName(@"UpdateDate").HasColumnType("datetime").IsRequired(false);
        builder.Property(x => x.UpdateUser).HasColumnName(@"UpdateUser").HasColumnType("varchar(50)").IsRequired(false).HasMaxLength(50);
    }
}
