using Ksc.HR.Domain.Entities.Salary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Configurations.Salary
{

    public class AccountCodeCompatibleConfiguration : IEntityTypeConfiguration<AccountCodeCompatible>
    {
        public void Configure(EntityTypeBuilder<AccountCodeCompatible> builder)
        {
            builder.ToTable("AccountCodeCompatible", "Salary");
            builder.HasKey(x => x.Id).HasName("PK_AccountCodeCompatible").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.AccountCodeCompatibleTypeId).HasColumnName(@"AccountCodeCompatibleTypeId").HasColumnType("int").IsRequired();
            builder.Property(x => x.AccountCodeId).HasColumnName(@"AccountCodeId").HasColumnType("int").IsRequired();
            builder.Property(x => x.AccountCodeCompatibleId).HasColumnName(@"AccountCodeCompatibleId").HasColumnType("int").IsRequired();
            builder.Property(x => x.InsertDate).HasColumnName(@"InsertDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.InsertUser).HasColumnName(@"InsertUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.UpdateDate).HasColumnName(@"UpdateDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.UpdateUser).HasColumnName(@"UpdateUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.IsActive).HasColumnName(@"IsActive").HasColumnType("bit").IsRequired();

            // Foreign keys
            builder.HasOne(a => a.AccountCode).WithMany(b => b.AccountCodeCompatibles_AccountCodeId).HasForeignKey(c => c.AccountCodeId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_AccountCodeCompatible_AccountCode");
            builder.HasOne(a => a.Salary_AccountCodeCompatible).WithMany(b => b.AccountCodeCompatibles_AccountCodeCompatibleId).HasForeignKey(c => c.AccountCodeCompatibleId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_AccountCodeCompatible_AccountCode1");
            builder.HasOne(a => a.AccountCodeCompatibleType).WithMany(b => b.AccountCodeCompatibles).HasForeignKey(c => c.AccountCodeCompatibleTypeId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_AccountCodeCompatible_AccountCodeCompatibleType");
        }
    }
}
