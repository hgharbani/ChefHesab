using Ksc.HR.Domain.Entities.Chart;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Configurations.Chart
{
    public class ViewJobPositionConfiguration : IEntityTypeConfiguration<ViewJobPosition>
    {
        public void Configure(EntityTypeBuilder<ViewJobPosition> builder)
        {
            builder.ToView("View_JobPosition", "dbo");
            builder.HasKey(a=>a.Id);

            builder.Property(x => x.Code).HasColumnName(@"Code").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.Title).HasColumnName(@"Title").HasColumnType("nvarchar(500)").IsRequired(false).HasMaxLength(500);
            builder.Property(x => x.StatusTitle).HasColumnName(@"StatusTitle").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.ColorChart).HasColumnName(@"ColorChart").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.StructureCode).HasColumnName(@"structureCode").HasColumnType("nvarchar(max)").IsRequired(false);
            builder.Property(x => x.StructureTitle).HasColumnName(@"structureTitle").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.IsActive).HasColumnName(@"IsActive").HasColumnType("bit").IsRequired();
            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired();
            //builder.Property(x => x.WorkingDayCount).HasColumnName(@"WorkingDayCount").HasColumnType("int").IsRequired(false);
            //builder.Property(x => x.WorkingShiftCount).HasColumnName(@"WorkingShiftCount").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.ParentId).HasColumnName(@"ParentId").HasColumnType("int").IsRequired(false);
            builder.HasOne(a => a.Parent).WithMany(b => b.Childrens).HasForeignKey(c => c.ParentId).OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
