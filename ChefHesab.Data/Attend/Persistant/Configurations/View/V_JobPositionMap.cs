using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ksc.Hr.Domain.Entities;
namespace Ksc.Hr.Data.Persistant.Configurations
{
    public class V_JobPositionMap : IEntityTypeConfiguration<V_JobPosition>
    {
        public void Configure(EntityTypeBuilder<V_JobPosition> builder)
        {
            // Table & Column Mappings
            builder.ToView("V_JobPosition", "dbo");
            builder.HasNoKey();
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.Code).HasColumnName("Code");
            builder.Property(t => t.Title).HasColumnName("Title");
            builder.Property(t => t.hasChildren).HasColumnName("hasChildren");
            builder.Property(t => t.StructureTitle).HasColumnName("StructureTitle").IsRequired();
            builder.Property(t => t.CostCenter).HasColumnName("CostCenter");
            builder.Property(t => t.CapacityCount).HasColumnName("CapacityCount");
            builder.Property(t => t.CofficientProximityProduct).HasColumnName("CofficientProximityProduct");
            builder.Property(t => t.Description).HasColumnName("Description");
            builder.Property(t => t.EndDate).HasColumnName("EndDate");
            builder.Property(t => t.ExtraCount).HasColumnName("ExtraCount");
            builder.Property(t => t.InsertDate).HasColumnName("InsertDate");
            builder.Property(t => t.InsertUser).HasColumnName("InsertUser");
            builder.Property(t => t.InsuranceJobCode).HasColumnName("InsuranceJobCode");
            builder.Property(t => t.IsOnCall).HasColumnName("IsOnCall");
            builder.Property(t => t.IsStrategic).HasColumnName("IsStrategic");
            builder.Property(t => t.JobIdentityId).HasColumnName("JobIdentityId");
            builder.Property(t => t.JobPoisitionStatusId).HasColumnName("JobPoisitionStatusId");
            builder.Property(t => t.ParentId).HasColumnName("ParentId");
            builder.Property(t => t.PermissionExistCommodityCount).HasColumnName("PermissionExistCommodityCount");
            builder.Property(t => t.RewardSpecificEfficincy).HasColumnName("RewardSpecificEfficincy");
            builder.Property(t => t.SpecificRewardId).HasColumnName("SpecificRewardId");
            builder.Property(t => t.StartDate).HasColumnName("StartDate");
            builder.Property(t => t.StructureEndDate).HasColumnName("StructureEndDate");
            builder.Property(t => t.StructureId).HasColumnName("StructureId");
            builder.Property(t => t.ParentCode).HasColumnName("ParentCode").IsRequired().HasMaxLength(601);
            builder.Property(t => t.SubstituteCount).HasColumnName("SubstituteCount");
            builder.Property(t => t.TemporaryCount).HasColumnName("TemporaryCount");
            builder.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            builder.Property(t => t.UpdateUser).HasColumnName("UpdateUser");
            builder.Property(t => t.WorkingDayCount).HasColumnName("WorkingDayCount");
            builder.Property(t => t.WorkingDayOutsourcingCount).HasColumnName("WorkingDayOutsourcingCount");
            builder.Property(t => t.WorkingShiftCount).HasColumnName("WorkingShiftCount");
            builder.Property(t => t.WorkingShiftOutsourcingCount).HasColumnName("WorkingShiftOutsourcingCount");
            builder.Property(t => t.IsActive).HasColumnName("IsActive");
            builder.Property(t => t.JoinedRelocationCount).HasColumnName("JoinedRelocationCount");
            builder.Property(t => t.TransferRelocationCount).HasColumnName("TransferRelocationCount");
            builder.Property(t => t.ParentCodeChain).HasColumnName("NewCodeRelation");
            builder.Property(t => t.LevelNumber).HasColumnName("LevelNumber");
            builder.Property(t => t.Asistanse).HasColumnName("Asistanse");
            builder.Property(t => t.EmployeeCount).HasColumnName("EmployeeCount");
        }
    }
}

