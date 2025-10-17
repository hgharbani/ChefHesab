using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ksc.Hr.Domain.Entities;
using Ksc.HR.Domain.Entities.Pay;
namespace Ksc.Hr.Data.Persistant.Configurations
{
    public class EmployeeOtherPaymentTypeConfiguration : IEntityTypeConfiguration<EmployeeOtherPaymentType>
    {
        public void Configure(EntityTypeBuilder<EmployeeOtherPaymentType> builder)
        {
            // Table & Column Mappings
            builder.ToTable("EmployeeOtherPaymentType", "Pay");
            builder.HasKey(t => t.Id).HasName("PK_EmployeeOtherPaymentType").IsClustered();


            builder.Property(t => t.EmployeeOtherPaymentId).HasColumnName("EmployeeOtherPaymentId").HasColumnType("bigint").IsRequired();
            builder.Property(t => t.OtherPaymentTypeId).HasColumnName("OtherPaymentTypeId").HasColumnType("int").IsRequired();
            builder.Property(t => t.PaymnetAmountUnit).HasColumnName("PaymnetAmountUnit").HasColumnType("bigint").IsRequired();
            builder.Property(t => t.PaymentTypeDate).HasColumnName("PaymentTypeDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(t => t.Year).HasColumnName("Year").HasColumnType("int").IsRequired(false);


            builder.HasOne(t => t.EmployeeOtherPayment)
           .WithMany(t => t.EmployeeOtherPaymentType)
         .HasForeignKey(d => d.EmployeeOtherPaymentId);

            builder.HasOne(t => t.OtherPaymentType)
           .WithMany(t => t.EmployeeOtherPaymentType)
         .HasForeignKey(d => d.OtherPaymentTypeId);


        }
    }
}

