using Ksc.HR.Domain.Entities.ODSViews;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

#nullable disable

namespace Ksc.HR.Data.Persistant.Configurations
{
    // View_MIS_JobCategory
    public class ViewMisJobCategoryConfiguration : IEntityTypeConfiguration<ViewMisJobCategory>
    {
        public void Configure(EntityTypeBuilder<ViewMisJobCategory> builder)
        {
            builder.ToView("View_MIS_JobCategory", "dbo");
            builder.HasNoKey();

            builder.Property(x => x.JobCategoryCode).HasColumnName(@"JobCategoryCode").HasColumnType("numeric(2,0)").HasPrecision(2,0).IsRequired();
            builder.Property(x => x.JobCategoryTitle).HasColumnName(@"JobCategoryTitle").HasColumnType("nvarchar(30)").IsRequired(false).HasMaxLength(30);
            builder.Property(x => x.CodeCategoryJobCategory).HasColumnName(@"CodeCategoryJobCategory").HasColumnType("nvarchar(2)").IsRequired(false).HasMaxLength(2);
        }
    }

}
// </auto-generated>
