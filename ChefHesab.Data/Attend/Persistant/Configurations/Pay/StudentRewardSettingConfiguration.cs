using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Domain.Entities.Pay;

namespace Ksc.HR.Data.Persistant.Configurations.Pay
{
    public class StudentRewardSettingConfiguration : IEntityTypeConfiguration<StudentRewardSetting>
    {
        public void Configure(EntityTypeBuilder<StudentRewardSetting> builder)
        {
            builder.ToTable("StudentRewardSetting", "Pay");
            builder.HasKey(x => x.Id).HasName("PK_StudentReward").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.Year).HasColumnName(@"Year").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.RewardTypeId).HasColumnName(@"RewardTypeId").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.RewardLevelId).HasColumnName(@"RewardLevelId").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.Title).HasColumnName(@"Title").HasColumnType("nvarchar(500)").IsRequired(false).HasMaxLength(500);
            builder.Property(x => x.MinRank).HasColumnName(@"MinRank").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.MaxRank).HasColumnName(@"MaxRank").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.MinMean).HasColumnName(@"MinMean").HasColumnType("float").HasPrecision(53).IsRequired(false);
            builder.Property(x => x.MaxMean).HasColumnName(@"MaxMean").HasColumnType("float").HasPrecision(53).IsRequired(false);
            builder.Property(x => x.Amount).HasColumnName(@"Amount").HasColumnType("bigint").IsRequired(false);
            builder.Property(x => x.IsActive).HasColumnName(@"IsActive").HasColumnType("bit").IsRequired();
            builder.Property(x => x.InsertUser).HasColumnName(@"InsertUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.InsertDate).HasColumnName(@"InsertDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.UpdateUser).HasColumnName(@"UpdateUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.UpdateDate).HasColumnName(@"UpdateDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.KPercent).HasColumnName(@"KPercent").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.MisCodeDgreeStwrbs).HasColumnName(@"MISCodeDgreeStwrbs").HasColumnType("int").IsRequired(false);

            // Foreign keys
            builder.HasOne(a => a.RewardLevel).WithMany(b => b.StudentRewardSettings).HasForeignKey(c => c.RewardLevelId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_StudentRewardSetting_RewardType");
            builder.HasOne(a => a.RewardType).WithMany(b => b.StudentRewardSettings).HasForeignKey(c => c.RewardTypeId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_StudentRewardSetting_RewardType1");
            builder.HasOne(a => a.KUnitSetting).WithMany(b => b.StudentRewardSettings).HasForeignKey(c => c.RewardTypeId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_StudentRewardSetting_KUnitSetting");
        }
    }
}
