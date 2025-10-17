using Ksc.HR.Domain.Entities.ODSViews;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

#nullable disable

namespace Ksc.HR.Data.Persistant.Configurations
{
    // View_MIS_UserDefinition
    public class ViewMisUserDefinitionConfiguration : IEntityTypeConfiguration<ViewMisUserDefinition>
    {
        public void Configure(EntityTypeBuilder<ViewMisUserDefinition> builder)
        {
            builder.ToView("View_MIS_UserDefinition", "dbo");
            builder.HasNoKey();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("bigint").IsRequired();
            builder.Property(x => x.MisUser).HasColumnName(@"MisUser").HasColumnType("nvarchar(8)").IsRequired(false).HasMaxLength(8);
            builder.Property(x => x.PersonalNumber).HasColumnName(@"PersonalNumber").HasColumnType("numeric(8,0)").HasPrecision(8, 0).IsRequired(false);
            builder.Property(x => x.WinUser).HasColumnName(@"WinUser").HasColumnType("nvarchar(20)").IsRequired().HasMaxLength(20);
            builder.Property(x => x.NationalCode).HasColumnName(@"NationalCode").HasColumnType("nvarchar(15)").IsRequired(false).HasMaxLength(15);
            builder.Property(x => x.FirstName).HasColumnName(@"FirstName").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.LastName).HasColumnName(@"LastName").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
        }
    }

}
// </auto-generated>
