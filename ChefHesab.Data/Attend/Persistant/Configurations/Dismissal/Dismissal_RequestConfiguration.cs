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
    public class Dismissal_RequestConfiguration : IEntityTypeConfiguration<Dismissal_Request>
    {
        public void Configure(EntityTypeBuilder<Dismissal_Request> builder)
        {
            builder.ToTable("Request", "Dismissal");
            builder.HasKey(x => x.Id).HasName("PK_Request_1").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.WfRequestId).HasColumnName(@"WFRequestId").HasColumnType("int").IsRequired();
            builder.Property(x => x.DismissalStatusId).HasColumnName(@"DismissalStatusId").HasColumnType("int").IsRequired();
            builder.Property(x => x.DismissalDate).HasColumnName(@"DismissalDate").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.HasOrganizationHome).HasColumnName(@"HasOrganizationHome").HasColumnType("bit").IsRequired();
            builder.Property(x => x.InsertDate).HasColumnName(@"InsertDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.InsertUser).HasColumnName(@"InsertUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.UpdateDate).HasColumnName(@"UpdateDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.UpdateUser).HasColumnName(@"UpdateUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.IsActive).HasColumnName(@"IsActive").HasColumnType("bit").IsRequired();

            // Foreign keys
            builder.HasOne(a => a.Dismissal_Status).WithMany(b => b.Dismissal_Requests).HasForeignKey(c => c.DismissalStatusId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Request_Status");
            builder.HasOne(a => a.WF_Request).WithMany(b => b.Dismissal_Requests).HasForeignKey(c => c.WfRequestId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Request_Request");
        }
    }

}
