using Ksc.HR.Domain.Entities.Workshift;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

#nullable disable

namespace Ksc.HR.Data.Persistant.Configurations
{
    // WorkCity
    public class WorkCityConfiguration : IEntityTypeConfiguration<WorkCity>
    {
        public void Configure(EntityTypeBuilder<WorkCity> builder)
        {
            builder.ToTable("WorkCity", "dbo");
            builder.HasKey(x => x.Id).HasName("PK_WorkCity").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.CityId).HasColumnName(@"CityId").HasColumnType("int").IsRequired();
            builder.Property(x => x.CompanyId).HasColumnName(@"CompanyId").HasColumnType("int").IsRequired();
            builder.Property(x => x.IsCompanyCentralCity).HasColumnName(@"IsCompanyCentralCity").HasColumnType("bit").IsRequired();

            // Foreign keys
            builder.HasOne(a => a.City).WithMany(b => b.WorkCities).HasForeignKey(c => c.CityId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_WorkCity_City");
            builder.HasOne(a => a.Company).WithMany(b => b.WorkCities).HasForeignKey(c => c.CompanyId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_WorkCity_Company");
        }
    }

}
// </auto-generated>
