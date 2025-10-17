using Ksc.HR.Domain.Entities.EmployeeBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Configurations.EmployeeBase
{
    // EmploymentType
    public class EmploymentTypeConfiguration : IEntityTypeConfiguration<EmploymentType>
    {
        public void Configure(EntityTypeBuilder<EmploymentType> builder)
        {
            builder.ToTable("EmploymentType", "EmployeeBase");
            builder.HasKey(x => x.Id).HasName("PK_EmploymentType").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.Title).HasColumnName(@"Title").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.InsertDate).HasColumnName(@"InsertDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.InsertUser).HasColumnName(@"InsertUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.UpdateDate).HasColumnName(@"UpdateDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.UpdateUser).HasColumnName(@"UpdateUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.IsActive).HasColumnName(@"IsActive").HasColumnType("bit").IsRequired(false);
            builder.Property(x => x.IsCreatedManualMonthTimeSheet).HasColumnName(@"IsCreatedManualMonthTimeSheet").HasColumnType("bit").IsRequired();
        }
    }
}
