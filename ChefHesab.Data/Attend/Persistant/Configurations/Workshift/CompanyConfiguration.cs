using Ksc.HR.Domain.Entities.Workshift;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

#nullable disable

namespace Ksc.HR.Data.Persistant.Configurations
{
    // Company
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Company", "dbo");
            builder.HasKey(x => x.Id).HasName("PK_Company").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.Title).HasColumnName(@"Title").HasColumnType("nvarchar(500)").IsRequired().HasMaxLength(500);
        }
    }

}
// </auto-generated>
