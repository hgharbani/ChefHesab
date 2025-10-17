using Ksc.HR.Domain.Entities.Workshift;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

#nullable disable

namespace Ksc.HR.Data.Persistant.Configurations
{
    // CompatibleRollCall
    public class CompatibleRollCallConfiguration : IEntityTypeConfiguration<CompatibleRollCall>
    {
        public void Configure(EntityTypeBuilder<CompatibleRollCall> builder)
        {
            builder.ToTable("CompatibleRollCall", "dbo");
            builder.HasKey(x => x.Id).HasName("PK_CompatibleRollCall").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.RollCallDefinitionId).HasColumnName(@"RollCallDefinitionId").HasColumnType("int").IsRequired();
            builder.Property(x => x.CompatibleRollCallId).HasColumnName(@"CompatibleRollCallId").HasColumnType("int").IsRequired();
            builder.Property(x => x.CompatibleRollCallType).HasColumnName(@"CompatibleRollCallType").HasColumnType("int").IsRequired();
            builder.Property(x => x.DayNumber).HasColumnName(@"DayNumber").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.WorkDayTypeId).HasColumnName(@"WorkDayTypeId").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.InsertDate).HasColumnName(@"InsertDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.InsertUser).HasColumnName(@"InsertUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.UpdateDate).HasColumnName(@"UpdateDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.UpdateUser).HasColumnName(@"UpdateUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.DomainName).HasColumnName(@"DomainName").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.IsActive).HasColumnName(@"IsActive").HasColumnType("bit").IsRequired();
            builder.Property(x => x.RowVersion).HasColumnName(@"RowVersion").HasColumnType("timestamp(8)").IsRequired().IsFixedLength().HasMaxLength(8).IsRowVersion().IsConcurrencyToken();

            // Foreign keys
            builder.HasOne(a => a.RollCallDefinition_CompatibleRollCallId).WithMany(b => b.CompatibleRollCalls_CompatibleRollCallId).HasForeignKey(c => c.CompatibleRollCallId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CompatibleRollCall_RollCallDefinition1");
            builder.HasOne(a => a.RollCallDefinition_RollCallDefinitionId).WithMany(b => b.CompatibleRollCalls_RollCallDefinitionId).HasForeignKey(c => c.RollCallDefinitionId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CompatibleRollCall_RollCallDefinition");
            builder.HasOne(a => a.WorkDayType).WithMany(b => b.CompatibleRollCalls).HasForeignKey(c => c.WorkDayTypeId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CompatibleRollCall_WorkDayType");
        }
    }

}
// </auto-generated>
