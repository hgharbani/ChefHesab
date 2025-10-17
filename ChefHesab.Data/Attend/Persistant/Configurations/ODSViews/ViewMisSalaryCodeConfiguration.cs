using Ksc.HR.Domain.Entities.ODSViews;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

#nullable disable

namespace Ksc.HR.Data.Persistant.Configurations
{
    // View_MIS_SalaryCode
    public class ViewMisSalaryCodeConfiguration : IEntityTypeConfiguration<ViewMisSalaryCode>
    {
        public void Configure(EntityTypeBuilder<ViewMisSalaryCode> builder)
        {
            builder.ToView("View_MIS_SalaryCode", "dbo");
            builder.HasNoKey();

            builder.Property(x => x.SalaryAccountCode).HasColumnName(@"SalaryAccountCode").HasColumnType("numeric(4,0)").HasPrecision(4,0).IsRequired();
            builder.Property(x => x.SalaryAccountTitle).HasColumnName(@"SalaryAccountTitle").HasColumnType("nvarchar(60)").IsRequired(false).HasMaxLength(60);
        }
    }

}
// </auto-generated>
