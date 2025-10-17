//using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Entities.Workshift;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

#nullable disable

namespace Ksc.HR.Data.Persistant.Configurations
{
    // OverTimeDefinition
    public class OverTimeDefinitionConfiguration : IEntityTypeConfiguration<OverTimeDefinition>
    {
        public void Configure(EntityTypeBuilder<OverTimeDefinition> builder)
        {
            builder.ToTable("OverTimeDefinition", "dbo");
            builder.HasKey(x => x.Id).HasName("PK_OverTimeDefinition").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.Title).HasColumnName(@"Title").HasColumnType("nvarchar(500)").IsRequired().HasMaxLength(500);
            builder.Property(x => x.Code).HasColumnName(@"Code").HasColumnType("nvarchar(50)").IsRequired().HasMaxLength(50);
            builder.Property(x => x.AverageDuration).HasColumnName(@"AverageDuration").HasColumnType("char(6)").IsRequired().IsFixedLength().IsUnicode(false).HasMaxLength(6);
            builder.Property(x => x.MaximumDuration).HasColumnName(@"MaximumDuration").HasColumnType("char(6)").IsRequired().IsFixedLength().IsUnicode(false).HasMaxLength(6);
            builder.Property(x => x.AverageDurationMinute).HasColumnName(@"AverageDurationMinute").HasColumnType("int").IsRequired();
            builder.Property(x => x.MaximumDurationMinute).HasColumnName(@"MaximumDurationMinute").HasColumnType("int").IsRequired();
            builder.Property(x => x.InsertDate).HasColumnName(@"InsertDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.InsertUser).HasColumnName(@"InsertUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.UpdateDate).HasColumnName(@"UpdateDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.UpdateUser).HasColumnName(@"UpdateUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.DomainName).HasColumnName(@"DomainName").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.IsActive).HasColumnName(@"IsActive").HasColumnType("bit").IsRequired();
            builder.Property(x => x.RowVersion).HasColumnName(@"RowVersion").HasColumnType("timestamp(8)").IsRequired().IsFixedLength().HasMaxLength(8).IsRowVersion().IsConcurrencyToken();
        }
    }

}
// </auto-generated>
