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
    public class PaymentAccountCodeConfiguration : IEntityTypeConfiguration<PaymentAccountCode>
    {
        public void Configure(EntityTypeBuilder<PaymentAccountCode> builder)
        {
            builder.ToTable("PaymentAccountCode", "Salary");
            builder.HasKey(x => x.Id).HasName("PK_PaymentAccountCode").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.AccountCodeId).HasColumnName(@"AccountCodeId").HasColumnType("int").IsRequired();
            builder.Property(x => x.PaymentTypeId).HasColumnName(@"PaymentTypeId").HasColumnType("int").IsRequired();
            builder.Property(x => x.InsertDate).HasColumnName(@"InsertDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.InsertUser).HasColumnName(@"InsertUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.UpdateDate).HasColumnName(@"UpdateDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.UpdateUser).HasColumnName(@"UpdateUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.IsActive).HasColumnName(@"IsActive").HasColumnType("bit").IsRequired();

            // Foreign keys
            builder.HasOne(a => a.AccountCode).WithMany(b => b.PaymentAccountCodes).HasForeignKey(c => c.AccountCodeId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_PaymentAccountCode_AccountCode");
            builder.HasOne(a => a.PaymentType).WithMany(b => b.PaymentAccountCodes).HasForeignKey(c => c.PaymentTypeId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_PaymentAccountCode_PaymentType");
        }
    }
}
