using Ksc.HR.Domain.Entities.Workshift;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

#nullable disable

namespace Ksc.HR.Data.Persistant.Configurations
{
    // TeamWorkCategory
    public class TeamWorkCategoryConfiguration : IEntityTypeConfiguration<TeamWorkCategory>
    {
        public void Configure(EntityTypeBuilder<TeamWorkCategory> builder)
        {
            builder.ToTable("TeamWorkCategory", "dbo");
            builder.HasKey(x => x.Id).HasName("PK_TeamWorkCategory").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.Title).HasColumnName(@"Title").HasColumnType("nvarchar(500)").IsRequired().HasMaxLength(500);
            builder.Property(x => x.Code).HasColumnName(@"Code").HasColumnType("nvarchar(50)").IsRequired().HasMaxLength(50);
            builder.Property(x => x.CostCenter).HasColumnName(@"CostCenter").HasColumnType("numeric(6,0)").HasPrecision(6, 0).IsRequired();
            builder.Property(x => x.TeamWorkCategoryTypeId).HasColumnName(@"TeamWorkCategoryTypeId").HasColumnType("int").IsRequired();
            builder.Property(x => x.TeamWorkMangementCodeId).HasColumnName(@"TeamWorkMangementCodeId").HasColumnType("int").IsRequired();
            builder.Property(x => x.InsertDate).HasColumnName(@"InsertDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.InsertUser).HasColumnName(@"InsertUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.UpdateDate).HasColumnName(@"UpdateDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.UpdateUser).HasColumnName(@"UpdateUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.DomainName).HasColumnName(@"DomainName").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.IsActive).HasColumnName(@"IsActive").HasColumnType("bit").IsRequired();
            builder.Property(x => x.RowVersion).HasColumnName(@"RowVersion").HasColumnType("timestamp(8)").IsRequired().IsFixedLength().HasMaxLength(8).IsRowVersion().IsConcurrencyToken();

            // Foreign keys
            builder.HasOne(a => a.TeamWorkCategoryType_TeamWorkCategoryTypeId).WithMany(b => b.TeamWorkCategories).HasForeignKey(c => c.TeamWorkCategoryTypeId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_TeamWorkCategory_TeamWorkCategoryType");
            builder.HasOne(a => a.TeamWorkMangementCode_TeamWorkMangementCodeId).WithMany(b => b.TeamWorkCategories).HasForeignKey(c => c.TeamWorkMangementCodeId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_TeamWorkCategory_TeamWorkMangementCode");
        }
    }

}
// </auto-generated>
