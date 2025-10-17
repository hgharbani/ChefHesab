using Ksc.HR.Domain.Entities.Workshift;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

#nullable disable

namespace Ksc.HR.Data.Persistant.Configurations
{
    // TeamWork
    public class TeamWorkConfiguration : IEntityTypeConfiguration<TeamWork>
    {
        public void Configure(EntityTypeBuilder<TeamWork> builder)
        {
            builder.ToTable("TeamWork", "dbo");
            builder.HasKey(x => x.Id).HasName("PK_TeamWork").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.Title).HasColumnName(@"Title").HasColumnType("nvarchar(500)").IsRequired().HasMaxLength(500);
            builder.Property(x => x.Code).HasColumnName(@"Code").HasColumnType("nvarchar(50)").IsRequired().HasMaxLength(50);
            builder.Property(x => x.TeamWorkCategoryId).HasColumnName(@"TeamWorkCategoryId").HasColumnType("int").IsRequired();
            builder.Property(x => x.OverTimeDefinitionId).HasColumnName(@"OverTimeDefinitionId").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.ValidityStartDate).HasColumnName(@"ValidityStartDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.ValidityEndDate).HasColumnName(@"ValidityEndDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.HasCommode).HasColumnName(@"HasCommode").HasColumnType("bit").IsRequired(false);
            builder.Property(x => x.InsertDate).HasColumnName(@"InsertDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.InsertUser).HasColumnName(@"InsertUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.UpdateDate).HasColumnName(@"UpdateDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.UpdateUser).HasColumnName(@"UpdateUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.DomainName).HasColumnName(@"DomainName").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.IsActive).HasColumnName(@"IsActive").HasColumnType("bit").IsRequired();
            builder.Property(x => x.RowVersion).HasColumnName(@"RowVersion").HasColumnType("timestamp(8)").IsRequired().IsFixedLength().HasMaxLength(8).IsRowVersion().IsConcurrencyToken();

            // Foreign keys
            builder.HasOne(a => a.OverTimeDefinition).WithMany(b => b.TeamWorks).HasForeignKey(c => c.OverTimeDefinitionId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_TeamWork_OverTimeDefinition");
            builder.HasOne(a => a.TeamWorkCategory).WithMany(b => b.TeamWorks).HasForeignKey(c => c.TeamWorkCategoryId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_TeamWork_TeamWorkCategory");
        }
    }

}
// </auto-generated>
