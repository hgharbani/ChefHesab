using Ksc.HR.Domain.Entities.Dismissal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Configurations.Dismissal
{
    public class Dismissal_StatusConfiguration : IEntityTypeConfiguration<Dismissal_Status>
    {
        public void Configure(EntityTypeBuilder<Dismissal_Status> builder)
        {
            builder.ToTable("Status", "Dismissal");
            builder.HasKey(x => x.Id).HasName("PK_DismissalStatus").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.Title).HasColumnName(@"Title").HasColumnType("nvarchar(200)").IsRequired().HasMaxLength(200);
            builder.Property(x => x.Code).HasColumnName(@"Code").HasColumnType("nvarchar(50)").IsRequired().HasMaxLength(50);
            builder.Property(x => x.InsertDate).HasColumnName(@"InsertDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.InsertUser).HasColumnName(@"InsertUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.UpdateDate).HasColumnName(@"UpdateDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.UpdateUser).HasColumnName(@"UpdateUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.DomainName).HasColumnName(@"DomainName").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.IsActive).HasColumnName(@"IsActive").HasColumnType("bit").IsRequired();
        }
    }
}
