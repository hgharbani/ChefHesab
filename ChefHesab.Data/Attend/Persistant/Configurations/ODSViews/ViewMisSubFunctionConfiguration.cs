using Ksc.HR.Domain.Entities.ODSViews;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

#nullable disable

namespace Ksc.HR.Data.Persistant.Configurations
{
    // View_MIS_CostCenter
    public class ViewMisSubFunctionConfiguration : IEntityTypeConfiguration<ViewMisSubFunction>
    {
        public void Configure(EntityTypeBuilder<ViewMisSubFunction> builder)
        {
            builder.ToView("View_MIS_SubFunction", "dbo");
            builder.HasNoKey();

            builder.Property(x => x.MoavenatCode).HasColumnName(@"MoavenatCode").HasColumnType("numeric(4,0)").HasPrecision(4, 0).IsRequired(false);
            builder.Property(x => x.Moavenat).HasColumnName(@"Moavenat").HasColumnType("nvarchar(60)").IsRequired(false).HasMaxLength(60);
            builder.Property(x => x.ManagmentCode).HasColumnName(@"ManagmentCode").HasColumnType("numeric(4,0)").HasPrecision(4, 0).IsRequired(false);
            builder.Property(x => x.Managment).HasColumnName(@"Managment").HasColumnType("nvarchar(60)").IsRequired(false).HasMaxLength(60);
            builder.Property(x => x.SectionCode).HasColumnName(@"SectionCode").HasColumnType("numeric(4,0)").HasPrecision(4, 0).IsRequired();
            builder.Property(x => x.Section).HasColumnName(@"Section").HasColumnType("nvarchar(60)").IsRequired(false).HasMaxLength(60);
            builder.Property(x => x.LengthSectionCode).HasColumnName(@"LengthSectionCode").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.CostCenterCode).HasColumnName(@"CostCenterCode").HasColumnType("numeric(6,0)").HasPrecision(6, 0).IsRequired();
            builder.Property(x => x.EndDateSection).HasColumnName(@"EndDateSection").HasColumnType("numeric(8,0)").HasPrecision(8, 0).IsRequired(false);
            builder.Property(x => x.IsActiveCostCenter).HasColumnName(@"IsActiveCostCenter").HasColumnType("int").IsRequired(false);
        }
    }

}
// </auto-generated>
