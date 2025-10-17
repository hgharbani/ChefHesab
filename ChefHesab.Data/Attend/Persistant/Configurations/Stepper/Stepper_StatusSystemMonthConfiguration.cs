using Ksc.HR.Domain.Entities.Stepper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Configurations.Stepper
{
    // StatusSystemMonth
    public class Stepper_StatusSystemMonthConfiguration : IEntityTypeConfiguration<Stepper_StatusSystemMonth>
    {
        public void Configure(EntityTypeBuilder<Stepper_StatusSystemMonth> builder)
        {
            builder.ToTable("StatusSystemMonth", "Stepper");
            builder.HasKey(x => x.Id).HasName("PK_EmployeeSystemStatusMonth").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.SystemSequenceStatusId).HasColumnName(@"SystemSequenceStatusId").HasColumnType("int").IsRequired();
            builder.Property(x => x.YearMonth).HasColumnName(@"YearMonth").HasColumnType("int").IsRequired();
            builder.Property(x => x.InsertDate).HasColumnName(@"InsertDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.InsertUser).HasColumnName(@"InsertUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.InsertAuthenticateUserName).HasColumnName(@"InsertAuthenticateUserName").HasColumnType("nvarchar(100)").IsRequired(false).HasMaxLength(100);
            builder.Property(x => x.UpdateDate).HasColumnName(@"UpdateDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.UpdateUser).HasColumnName(@"UpdateUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.UpdateAuthenticateUserName).HasColumnName(@"UpdateAuthenticateUserName").HasColumnType("nvarchar(100)").IsRequired(false).HasMaxLength(100);
            builder.Property(x => x.IsActive).HasColumnName(@"IsActive").HasColumnType("bit").IsRequired();

            // Foreign keys
            builder.HasOne(a => a.SystemSequenceStatus).WithMany(b => b.Stepper_StatusSystemMonths).HasForeignKey(c => c.SystemSequenceStatusId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_EmployeeSystemStatusMonth_SystemSequenceStatus");
        }
    }
}
