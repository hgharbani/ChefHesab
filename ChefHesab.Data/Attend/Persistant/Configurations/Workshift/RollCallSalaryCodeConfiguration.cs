using Ksc.HR.Domain.Entities.Workshift;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

#nullable disable

namespace Ksc.HR.Data.Persistant.Configurations
{
    // RollCallSalaryCode
    public class RollCallSalaryCodeConfiguration : IEntityTypeConfiguration<RollCallSalaryCode>
    {
        public void Configure(EntityTypeBuilder<RollCallSalaryCode> builder)
        {
            builder.ToTable("RollCallSalaryCode", "dbo");
            builder.HasKey(x => x.Id).HasName("PK_RollCallSalaryCode").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.RollCallDefinitionId).HasColumnName(@"RollCallDefinitionId").HasColumnType("int").IsRequired();
            builder.Property(x => x.SalaryAccountCode).HasColumnName(@"SalaryAccountCode").HasColumnType("numeric(4,0)").HasPrecision(4, 0).IsRequired();
            builder.Property(x => x.EmploymentTypeCode).HasColumnName(@"EmploymentTypeCode").HasColumnType("numeric(2,0)").HasPrecision(2, 0).IsRequired();
            builder.Property(x => x.InsertDate).HasColumnName(@"InsertDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.InsertUser).HasColumnName(@"InsertUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.UpdateDate).HasColumnName(@"UpdateDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.UpdateUser).HasColumnName(@"UpdateUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.DomainName).HasColumnName(@"DomainName").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.IsActive).HasColumnName(@"IsActive").HasColumnType("bit").IsRequired();
            builder.Property(x => x.RowVersion).HasColumnName(@"RowVersion").HasColumnType("timestamp(8)").IsRequired().IsFixedLength().HasMaxLength(8).IsRowVersion().IsConcurrencyToken();

            // Foreign keys
            builder.HasOne(a => a.RollCallDefinition).WithMany(b => b.RollCallSalaryCodes).HasForeignKey(c => c.RollCallDefinitionId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_RollCallSalaryCode_RollCallDefinition");
        }
    }

}
// </auto-generated>
