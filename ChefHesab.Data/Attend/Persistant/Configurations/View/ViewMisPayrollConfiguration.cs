using Ksc.HR.Domain.Entities.View;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ksc.HR.Data.Persistant.Configurations.View;

public sealed class ViewMisPayrollConfiguration : IEntityTypeConfiguration<ViewMisPayroll>
{
    public void Configure(EntityTypeBuilder<ViewMisPayroll> builder)
    {
        builder.ToView("View_MIS_Payroll", "dbo");

        builder.HasNoKey();

        builder.Property(x => x.EmployeeNumber).HasColumnType(@"EmployeeNumber").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);

        builder.Property(x => x.YearMonth).HasColumnType(@"YearMonth").HasColumnType("numeric(6,0)").IsRequired(false);

        builder.Property(x => x.CompanySaveAmount).HasColumnType(@"CompanySaveAmount").HasColumnType("bigint").IsRequired(false);

        builder.Property(x => x.EmployeeSaveAmount).HasColumnType(@"EmployeeSaveAmount").HasColumnType("bigint").IsRequired(false);
    }
}
