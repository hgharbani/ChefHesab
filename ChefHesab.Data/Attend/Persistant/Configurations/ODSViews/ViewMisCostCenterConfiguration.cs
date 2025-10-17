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
    public class ViewMisCostCenterConfiguration : IEntityTypeConfiguration<ViewMisCostCenter>
    {
        public void Configure(EntityTypeBuilder<ViewMisCostCenter> builder)
        {
            builder.ToView("View_MIS_CostCenter", "dbo");
            builder.HasNoKey();

            builder.Property(x => x.CostCenterCode).HasColumnName(@"CostCenterCode").HasColumnType("numeric(6,0)").HasPrecision(6,0).IsRequired();
            builder.Property(x => x.CostCenterTitle).HasColumnName(@"CostCenterTitle").HasColumnType("nvarchar(45)").IsRequired(false).HasMaxLength(45);
        }
    }

}
// </auto-generated>
