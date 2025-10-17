using Ksc.HR.Domain.Entities.Pay;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Configurations.Pay
{
    public class PaymentAdditionalSettingJobPositionConfiguration : IEntityTypeConfiguration<PaymentAdditionalSettingJobPosition>
    {
        public void Configure(EntityTypeBuilder<PaymentAdditionalSettingJobPosition> builder)
        {
            // Table & Column Mappings
            builder.ToTable("PaymentAdditionalSettingJobPosition", "Pay");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.PaymentAdditionalSettingId).HasColumnName("PaymentAdditionalSettingId");
            builder.Property(t => t.JobpositionId).HasColumnName("JobpositionId");
            builder.Property(t => t.ValidityStartYearMonth).HasColumnName("ValidityStartYearMonth");
            builder.Property(t => t.ValidityEndYearMonth).HasColumnName("ValidityEndYearMonth");
            builder.Property(t => t.BaseAmount).HasColumnName("BaseAmount");
            builder.Property(t => t.IsActive).HasColumnName("IsActive");
            builder.Property(t => t.InsertUser).HasColumnName("InsertUser").IsRequired().HasMaxLength(50);
            builder.Property(t => t.InsertDate).HasColumnName("InsertDate");
            builder.Property(t => t.UpdateUser).HasColumnName("UpdateUser");
            builder.Property(t => t.UpdateDate).HasColumnName("UpdateDate");


            builder.HasOne(t => t.PaymentAdditionalSetting)
           .WithMany(t => t.PaymentAdditionalSettingJobPositions)
         .HasForeignKey(d => d.PaymentAdditionalSettingId);



        }

    }
}
