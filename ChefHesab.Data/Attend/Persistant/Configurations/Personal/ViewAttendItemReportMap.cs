using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ksc.Hr.Domain.Entities;
namespace Ksc.Hr.Data.Persistant.Configurations
{
    public class ViewAttendItemReportMap : IEntityTypeConfiguration<ViewAttendItemReport>
    {
        public void Configure(EntityTypeBuilder<ViewAttendItemReport> builder)
        {
            // Table & Column Mappings
            builder.ToView("View_AttendItemReport", "dbo");
            builder.HasKey(x => x.Id);
            builder.Property(t => t.Family).HasColumnName("Family").IsRequired().HasMaxLength(500);
            builder.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(500);
            builder.Property(t => t.EmployeeNumber).HasColumnName("EmployeeNumber").IsRequired().HasMaxLength(50);
            builder.Property(t => t.Code).HasColumnName("Code").IsRequired().HasMaxLength(50);
            builder.Property(t => t.Title).HasColumnName("Title").IsRequired().HasMaxLength(500);
            builder.Property(t => t.ShamsiDateV1).HasColumnName("ShamsiDateV1").IsRequired().HasMaxLength(10);
            builder.Property(t => t.MiladiDateV1).HasColumnName("MiladiDateV1");
            builder.Property(t => t.DefinitionTitle).HasColumnName("DefinitionTitle").IsRequired().HasMaxLength(500);
            builder.Property(t => t.DefinitionCode).HasColumnName("DefinitionCode").IsRequired().HasMaxLength(50);
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.EmployeeId).HasColumnName("EmployeeId");
            builder.Property(t => t.WorkCalendarId).HasColumnName("WorkCalendarId");
            builder.Property(t => t.StartTime).HasColumnName("StartTime").IsRequired().HasMaxLength(5);
            builder.Property(t => t.EndTime).HasColumnName("EndTime").IsRequired().HasMaxLength(5);
            builder.Property(t => t.StartDate).HasColumnName("StartDate");
            builder.Property(t => t.EndDate).HasColumnName("EndDate");
            builder.Property(t => t.TimeDuration).HasColumnName("TimeDuration").IsRequired().HasMaxLength(5);
            builder.Property(t => t.EmployeeLongTermAbsenceId).HasColumnName("EmployeeLongTermAbsenceId");
            builder.Property(t => t.RollCallDefinitionId).HasColumnName("RollCallDefinitionId");
            builder.Property(t => t.ShiftConceptDetailId).HasColumnName("ShiftConceptDetailId");
            builder.Property(t => t.ShiftConceptDetailIdInShiftBoard).HasColumnName("ShiftConceptDetailIdInShiftBoard");
            builder.Property(t => t.WorkTimeId).HasColumnName("WorkTimeId");
            builder.Property(t => t.IsManual).HasColumnName("IsManual");
            builder.Property(t => t.IsFloat).HasColumnName("IsFloat");
            builder.Property(t => t.InvalidRecord).HasColumnName("InvalidRecord");
            builder.Property(t => t.InvalidRecordReason).HasColumnName("InvalidRecordReason");
            builder.Property(t => t.InsertDate).HasColumnName("InsertDate");
            builder.Property(t => t.InsertUser).HasColumnName("InsertUser");
            builder.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            builder.Property(t => t.UpdateUser).HasColumnName("UpdateUser");
            builder.Property(t => t.RowVersion).HasColumnName("RowVersion");
            builder.Property(t => t.EvaluationDevelopmentId).HasColumnName("EvaluationDevelopmentId");
            builder.Property(t => t.MissionId).HasColumnName("MissionId");
            builder.Property(t => t.EmployeeEducationTimeId).HasColumnName("EmployeeEducationTimeId");
            builder.Property(t => t.TimeDurationInMinute).HasColumnName("TimeDurationInMinute");
            builder.Property(t => t.OverTimeToken).HasColumnName("OverTimeToken");
            builder.Property(t => t.IncreasedTimeDuration).HasColumnName("IncreasedTimeDuration");
            builder.Property(t => t.RollCategoryCode).HasColumnName("RollCategoryCode").IsRequired().HasMaxLength(50);
            builder.Property(t => t.RollCallCategoryTitle).HasColumnName("RollCallCategoryTitle").IsRequired().HasMaxLength(500);
            builder.Property(t => t.DateKey).HasColumnName("DateKey");









































        }
    }
}

