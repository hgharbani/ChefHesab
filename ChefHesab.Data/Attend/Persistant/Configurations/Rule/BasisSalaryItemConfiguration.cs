using Ksc.HR.Domain.Entities.Rule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Configurations.Rule
{
    public class BasisSalaryItemConfiguration : IEntityTypeConfiguration<BasisSalaryItem>
    {
        public void Configure(EntityTypeBuilder<BasisSalaryItem> builder)
        {
            builder.ToTable("BasisSalaryItem", "Rule");
            builder.HasKey(x => x.Id).HasName("PK_BasisSalaryItem").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.EmploymentTypeId).HasColumnName(@"EmploymentTypeId").HasColumnType("int").IsRequired();
            builder.Property(x => x.StartDate).HasColumnName(@"StartDate").HasColumnType("int").IsRequired();
            builder.Property(x => x.EndDate).HasColumnName(@"EndDate").HasColumnType("int").IsRequired();
            builder.Property(x => x.MinDailySalaryAmount).HasColumnName(@"MinDailySalaryAmount").HasColumnType("bigint").IsRequired();
            builder.Property(x => x.TableCoefficient).HasColumnName(@"TableCoefficient").HasColumnType("float").HasPrecision(53).IsRequired();
            builder.Property(x => x.IsActive).HasColumnName(@"IsActive").HasColumnType("bit").IsRequired();
            builder.Property(x => x.IsConfirmed).HasColumnName(@"IsConfirmed").HasColumnType("bit").IsRequired();
            builder.Property(x => x.InsertDate).HasColumnName(@"InsertDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.InsertUser).HasColumnName(@"InsertUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.UpdateDate).HasColumnName(@"UpdateDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.UpdateUser).HasColumnName(@"UpdateUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);

            // Foreign keys
            builder.HasOne(a => a.EmploymentType).WithMany(b => b.BasisSalaryItems).HasForeignKey(c => c.EmploymentTypeId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_BasisSalaryItem_EmploymentType");

        }
    }
}
