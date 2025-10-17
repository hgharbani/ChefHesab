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
    public class BasisSalaryItemPerGroupConfiguration : IEntityTypeConfiguration<BasisSalaryItemPerGroup>
    {
        public void Configure(EntityTypeBuilder<BasisSalaryItemPerGroup> builder)
        {
            // Table & Column Mappings
            builder.ToTable("BasisSalaryItemPerGroup", "Rule");
            builder.HasKey(x => x.Id).HasName("PK_BasisSalaryItemPerGroup").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.BasisSalaryItemId).HasColumnName(@"BasisSalaryItemId").HasColumnType("int").IsRequired();
            builder.Property(x => x.WorkGroupId).HasColumnName(@"WorkGroupId").HasColumnType("int").IsRequired();
            builder.Property(x => x.GroupScore).HasColumnName(@"GroupScore").HasColumnType("int").IsRequired();
            builder.Property(x => x.VariableCoefficient).HasColumnName(@"VariableCoefficient").HasColumnType("float").HasPrecision(53).IsRequired(false);
            builder.Property(x => x.CalculatedVariableCoefficient).HasColumnName(@"CalculatedVariableCoefficient").HasColumnType("float").HasPrecision(53).IsRequired(false);

            builder.Property(x => x.GroupSalaryAmount).HasColumnName(@"GroupSalaryAmount").HasColumnType("bigint").IsRequired();
            builder.Property(x => x.SalaryAccountCodeId).HasColumnName(@"SalaryAccountCodeId").HasColumnType("int").IsRequired();
            builder.Property(x => x.BaseAmountYears).HasColumnName(@"BaseAmountYears").HasColumnType("bigint").IsRequired();
            builder.Property(x => x.IsActive).HasColumnName(@"IsActive").HasColumnType("bit").IsRequired();
            builder.Property(x => x.InsertDate).HasColumnName(@"InsertDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.InsertUser).HasColumnName(@"InsertUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.UpdateDate).HasColumnName(@"UpdateDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.UpdateUser).HasColumnName(@"UpdateUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);

            // Foreign keys
            builder.HasOne(a => a.Chart_JobGroup).WithMany(b => b.BasisSalaryItemPerGroups).HasForeignKey(c => c.WorkGroupId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_BasisSalaryItemPerGroup_JobGroup");
            builder.HasOne(a => a.BasisSalaryItem).WithMany(b => b.BasisSalaryItemPerGroups).HasForeignKey(c => c.BasisSalaryItemId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_BasisSalaryItemPerGroup_BasisSalaryItem");
            builder.HasOne(a => a.Salary_AccountCode).WithMany(b => b.BasisSalaryItemPerGroups).HasForeignKey(c => c.SalaryAccountCodeId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_BasisSalaryItemPerGroup_SalaryAccountCode");

        }
    }
}
