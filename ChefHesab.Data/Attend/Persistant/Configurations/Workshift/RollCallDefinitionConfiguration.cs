using Ksc.HR.Domain.Entities.Workshift;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

#nullable disable

namespace Ksc.HR.Data.Persistant.Configurations
{
    // RollCallDefinition
    public class RollCallDefinitionConfiguration : IEntityTypeConfiguration<RollCallDefinition>
    {
        public void Configure(EntityTypeBuilder<RollCallDefinition> builder)
        {
            builder.ToTable("RollCallDefinition", "dbo");
            builder.HasKey(x => x.Id).HasName("PK_RollCallDefinition_1").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.Title).HasColumnName(@"Title").HasColumnType("nvarchar(500)").IsRequired().HasMaxLength(500);
            builder.Property(x => x.Code).HasColumnName(@"Code").HasColumnType("nvarchar(50)").IsRequired().HasMaxLength(50);
            builder.Property(x => x.ValidityStartDate).HasColumnName(@"ValidityStartDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.ValidityEndDate).HasColumnName(@"ValidityEndDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.ValidityMinimumTime).HasColumnName(@"ValidityMinimumTime").HasColumnType("char(5)").IsRequired(false).IsFixedLength().IsUnicode(false).HasMaxLength(5);
            builder.Property(x => x.ValidityMaximumTime).HasColumnName(@"ValidityMaximumTime").HasColumnType("char(5)").IsRequired(false).IsFixedLength().IsUnicode(false).HasMaxLength(5);
            builder.Property(x => x.ValidityMinimumTimeMinute).HasColumnName(@"ValidityMinimumTimeMinute").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.ValidityMaximumTimeMinute).HasColumnName(@"ValidityMaximumTimeMinute").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.IsValidInShiftStart).HasColumnName(@"IsValidInShiftStart").HasColumnType("bit").IsRequired();
            builder.Property(x => x.IsValidInShiftEnd).HasColumnName(@"IsValidInShiftEnd").HasColumnType("bit").IsRequired();
            builder.Property(x => x.RollCallConceptId).HasColumnName(@"RollCallConceptId").HasColumnType("int").IsRequired();
            builder.Property(x => x.RollCallCategoryId).HasColumnName(@"RollCallCategoryId").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.TimesAllowedUsePerDay).HasColumnName(@"TimesAllowedUsePerDay").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.TimesAllowedUsePerWeek).HasColumnName(@"TimesAllowedUsePerWeek").HasColumnType("int").IsRequired(false);

            builder.Property(x => x.TimesAllowedUsePerMonth).HasColumnName(@"TimesAllowedUsePerMonth").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.ValidityDayNumberInYear).HasColumnName(@"ValidityDayNumberInYear").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.IsValidForAllWorkTimeDayType).HasColumnName(@"IsValidForAllWorkTimeDayType").HasColumnType("bit").IsRequired();
            builder.Property(x => x.InsertCodeIsAutomatic).HasColumnName(@"InsertCodeIsAutomatic").HasColumnType("bit").IsRequired();
            builder.Property(x => x.IsValidForAllEmploymentType).HasColumnName(@"IsValidForAllEmploymentType").HasColumnType("bit").IsRequired();
            builder.Property(x => x.IsValidForAllCategoryCode).HasColumnName(@"IsValidForAllCategoryCode").HasColumnType("bit").IsRequired();
            builder.Property(x => x.AccessLevelId).HasColumnName(@"AccessLevelId").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.IsValidSingleDelete).HasColumnName(@"IsValidSingleDelete").HasColumnType("bit").IsRequired();
            builder.Property(x => x.IsValidForTemporaryStartDate).HasColumnName(@"IsValidForTemporaryStartDate").HasColumnType("bit").IsRequired();
            builder.Property(x => x.IsValidForTemporaryEndDate).HasColumnName(@"IsValidForTemporaryEndDate").HasColumnType("bit").IsRequired();
            builder.Property(x => x.TrainingTypeId).HasColumnName(@"TrainingTypeId").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.TrainingValidInShiftTime).HasColumnName(@"TrainingValidInShiftTime").HasColumnType("bit").IsRequired();
            builder.Property(x => x.TrainingValidOutShiftTime).HasColumnName(@"TrainingValidOutShiftTime").HasColumnType("bit").IsRequired();
            builder.Property(x => x.IsValidForDailyAbcenseInAnalyz).HasColumnName(@"IsValidForDailyAbcenseInAnalyz").HasColumnType("bit").IsRequired();
            builder.Property(x => x.VaccinationCheck).HasColumnName(@"VaccinationCheck").HasColumnType("bit").IsRequired();
            builder.Property(x => x.OverTimePriority).HasColumnName(@"OverTimePriority").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.InsertDate).HasColumnName(@"InsertDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.InsertUser).HasColumnName(@"InsertUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.UpdateDate).HasColumnName(@"UpdateDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.UpdateUser).HasColumnName(@"UpdateUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.DomainName).HasColumnName(@"DomainName").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.IsActive).HasColumnName(@"IsActive").HasColumnType("bit").IsRequired();
            builder.Property(x => x.RowVersion).HasColumnName(@"RowVersion").HasColumnType("timestamp(8)").IsRequired().IsFixedLength().HasMaxLength(8).IsRowVersion().IsConcurrencyToken();
            builder.Property(x => x.IdTemp).HasColumnName(@"IdTemp").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.TimesAllowedUsePerWeek).HasColumnName(@"TimesAllowedUsePerWeek").HasColumnType("int").IsRequired(false);

            builder.Property(x => x.IsValidForDeleteAbsenceItem).HasColumnName(@"IsValidForDeleteAbsenceItem").HasColumnType("bit").IsRequired();
            builder.Property(x => x.GenderTypeId).HasColumnName(@"GenderTypeId").HasColumnType("int").IsRequired(false);
            // Foreign keys
            builder.HasOne(a => a.AccessLevel).WithMany(b => b.RollCallDefinitions).HasForeignKey(c => c.AccessLevelId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_RollCallDefinition_AccessLevel");
            builder.HasOne(a => a.RollCallCategory).WithMany(b => b.RollCallDefinitions).HasForeignKey(c => c.RollCallCategoryId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_RollCallDefinition_RollCallCategory");
            builder.HasOne(a => a.RollCallConcept).WithMany(b => b.RollCallDefinitions).HasForeignKey(c => c.RollCallConceptId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_RollCallDefinition_RollCallConcept");
            builder.HasOne(a => a.TrainingType).WithMany(b => b.RollCallDefinitions).HasForeignKey(c => c.TrainingTypeId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_RollCallDefinition_TrainingType");
            builder.HasOne(a => a.GenderType).WithMany(b => b.RollCallDefinitions).HasForeignKey(c => c.GenderTypeId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_RollCallDefinition_GenderType");
        }
    }

}
// </auto-generated>
