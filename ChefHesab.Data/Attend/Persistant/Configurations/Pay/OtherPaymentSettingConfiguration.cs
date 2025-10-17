using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ksc.Hr.Domain.Entities;
using Ksc.HR.Domain.Entities.Pay;
namespace Ksc.Hr.Data.Persistant.Configurations
{
    public class OtherPaymentSettingConfiguration : IEntityTypeConfiguration<OtherPaymentSetting>
    {
        public void Configure(EntityTypeBuilder<OtherPaymentSetting> builder)
        {
            // Table & Column Mappings
            builder.ToTable("OtherPaymentSetting", "Pay");
            builder.HasKey(t => t.Id).HasName("PK_OtherPaymentSetting").IsClustered();

            builder.Property(t => t.OtherPaymentTypeId).HasColumnName("OtherPaymentTypeId").HasColumnType("int").IsRequired();
            builder.Property(t => t.AccountCodeId).HasColumnName("AccountCodeId").HasColumnType("int").IsRequired();
            builder.Property(t => t.ValidityStartYearMonth).HasColumnName("ValidityStartYearMonth").HasColumnType("int").IsRequired();
            builder.Property(t => t.ValidityEndYearMonth).HasColumnName("ValidityEndYearMonth").HasColumnType("int").IsRequired(false);
            builder.Property(t => t.KPercent).HasColumnName("KPercent").HasColumnType("int").IsRequired(false);
            //builder.Property(t => t.KUnit).HasColumnName("KUnit").HasColumnType("int").IsRequired(false);
            //builder.Property(t => t.CountValidDaysForPayment).HasColumnName("CountValidDaysForPayment").HasColumnType("int").IsRequired(false);
            //builder.Property(t => t.MinimumPersonForPayment).HasColumnName("MinimumPersonForPayment").HasColumnType("int").IsRequired(false);
            //builder.Property(t => t.MaximumPersonForPayment).HasColumnName("MaximumPersonForPayment").HasColumnType("int").IsRequired(false);
            builder.Property(t => t.IsActive).HasColumnName("IsActive").HasColumnType("bit").IsRequired();
            builder.Property(t => t.InsertUser).HasColumnName("InsertUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(t => t.InsertDate).HasColumnName("InsertDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(t => t.UpdateUser).HasColumnName("UpdateUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(t => t.UpdateDate).HasColumnName("UpdateDate").HasColumnType("datetime").IsRequired(false);



            builder.HasOne(t => t.OtherPaymentType)
           .WithMany(t => t.OtherPaymentSetting)
         .HasForeignKey(d => d.OtherPaymentTypeId);

            builder.HasOne(t => t.AccountCode)
           .WithMany(t => t.OtherPaymentSettings)
         .HasForeignKey(d => d.AccountCodeId);

        }
    }
}

