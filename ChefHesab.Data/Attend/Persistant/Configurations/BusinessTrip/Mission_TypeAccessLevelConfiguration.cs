using Ksc.HR.Domain.Entities.BusinessTrip;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Configurations.BusinessTrip
{
    // TypeAccessLevel
    public class Mission_TypeAccessLevelConfiguration : IEntityTypeConfiguration<Mission_TypeAccessLevel>
    {
        public void Configure(EntityTypeBuilder<Mission_TypeAccessLevel> builder)
        {
            builder.ToTable("TypeAccessLevel", "Mission");
            builder.HasKey(x => x.Id).HasName("PK_TypeAccessLevel").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.MissionTypeId).HasColumnName(@"MissionTypeId").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.AccessLevelId).HasColumnName(@"AccessLevelId").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.ValidMissionCreateDirectly).HasColumnName(@"ValidMissionCreateDirectly").HasColumnType("bit").IsRequired();
            builder.Property(x => x.ValidHasNoMissionReport).HasColumnName(@"ValidHasNoMissionReport").HasColumnType("bit").IsRequired();
            
            // Foreign keys
            builder.HasOne(a => a.AccessLevel).WithMany(b => b.Mission_TypeAccessLevels).HasForeignKey(c => c.AccessLevelId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_TypeAccessLevel_AccessLevel");
            builder.HasOne(a => a.Mission_Type).WithMany(b => b.Mission_TypeAccessLevels).HasForeignKey(c => c.MissionTypeId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_TypeAccessLevel_Type");
        }
    }
}
