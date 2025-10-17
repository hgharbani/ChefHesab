  using Microsoft.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore.Metadata.Builders;
  using Ksc.HR.Domain.Entities;
using Ksc.HR.Domain.Entities.Personal;

namespace Ksc.HR.Data.Persistant.Configurations
{
   public class EmployeeEfficiencyHistoryConfiguration : IEntityTypeConfiguration<EmployeeEfficiencyHistory>
  {
        public void Configure(EntityTypeBuilder<EmployeeEfficiencyHistory> builder)
        {
            builder.ToTable("EmployeeEfficiencyHistory", "dbo");
            builder.HasKey(x => x.Id).HasName("PK_EmployeeEfficiencyHistory").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.EmployeeId).HasColumnName(@"EmployeeId").HasColumnType("int").IsRequired();
            builder.Property(x => x.YearMonth).HasColumnName(@"YearMonth").HasColumnType("int").IsRequired();
            builder.Property(x => x.Efficiency).HasColumnName(@"Efficiency").HasColumnType("decimal(3,2)").HasPrecision(3, 2).IsRequired();
            builder.Property(x => x.IsLatest).HasColumnName(@"IsLatest").HasColumnType("bit").IsRequired();
            builder.Property(x => x.RemoteIpAddress).HasColumnName(@"RemoteIpAddress").HasColumnType("varchar(50)").IsRequired(false).IsUnicode(false).HasMaxLength(50);

            builder.Property(x => x.InsertDate).HasColumnName(@"InsertDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.InsertUser).HasColumnName(@"InsertUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);

            // Foreign keys
            builder.HasOne(a => a.Employee).WithMany(b => b.EmployeeEfficiencyHistories).HasForeignKey(c => c.EmployeeId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_EmployeeEfficiencyHistory_Employee");
        }
    }
  }

