using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Domain.Entities.Salary;

namespace Ksc.HR.Data.Persistant.Configurations.Salary
{
    public class PropertyAccountSettingConfiguration : IEntityTypeConfiguration<PropertyAccountSetting>
    {
        public void Configure(EntityTypeBuilder<PropertyAccountSetting> builder)
        {
            builder.ToTable("PropertyAccountSetting", "Salary");
            builder.HasKey(x => x.Id).HasName("PK_PropertyAccountSetting").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.PropertyAccountId).HasColumnName(@"PropertyAccountId").HasColumnType("int").IsRequired();
            builder.Property(x => x.AccountCodeId).HasColumnName(@"AccountCodeId").HasColumnType("int").IsRequired();
            builder.Property(x => x.InsertDate).HasColumnName(@"InsertDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.InsertUser).HasColumnName(@"InsertUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.UpdateDate).HasColumnName(@"UpdateDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.UpdateUser).HasColumnName(@"UpdateUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.IsActive).HasColumnName(@"IsActive").HasColumnType("bit").IsRequired();

            // Foreign keys
            builder.HasOne(a => a.AccountCode).WithMany(b => b.PropertyAccountSettings).HasForeignKey(c => c.AccountCodeId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_PropertyAccountSetting_AccountCode");
            builder.HasOne(a => a.PropertyAccount).WithMany(b => b.PropertyAccountSettings).HasForeignKey(c => c.PropertyAccountId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_PropertyAccountSetting_PropertyAccount");
        }
    }
}
