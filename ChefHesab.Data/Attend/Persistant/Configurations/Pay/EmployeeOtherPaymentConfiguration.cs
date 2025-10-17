using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ksc.Hr.Domain.Entities;
using Ksc.HR.Domain.Entities.Pay;
namespace Ksc.Hr.Data.Persistant.Configurations
{
    public class EmployeeOtherPaymentConfiguration : IEntityTypeConfiguration<EmployeeOtherPayment>
    {
        public void Configure(EntityTypeBuilder<EmployeeOtherPayment> builder)
        {
            // Table & Column Mappings
            builder.ToTable("EmployeeOtherPayment", "Pay");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.EmployeeId).HasColumnName("EmployeeId");
            builder.Property(t => t.OtherPaymentHeaderId).HasColumnName("OtherPaymentHeaderId");
            builder.Property(t => t.CostCenterCode).HasColumnName("CostCenterCode").IsRequired().HasMaxLength(50);
            builder.Property(t => t.YearMonthStartReport).HasColumnName("YearMonthStartReport");
            builder.Property(t => t.YearMonthEndReport).HasColumnName("YearMonthEndReport");
            builder.Property(t => t.PaymentAmount).HasColumnName("PaymentAmount");
            builder.Property(t => t.PaymentPersonCount).HasColumnName("PaymentPersonCount");
            builder.Property(t => t.IsBlacklist).HasColumnName("IsBlacklist");
            builder.Property(t => t.InvalidShowInPortal).HasColumnName("InvalidShowInPortal");
            builder.Property(t => t.InvalidPayment).HasColumnName("InvalidPayment");
            builder.Property(t => t.AccountNumber).HasColumnName("AccountNumber");
            builder.Property(t => t.DefaultPaymentPersonCount).HasColumnName("DefaultPaymentPersonCount");
            builder.Property(t => t.DefaultPaymentAmount).HasColumnName("DefaultPaymentAmount");
            builder.Property(t => t.InsertUser).HasColumnName("InsertUser");
            builder.Property(t => t.InsertDate).HasColumnName("InsertDate"); 
            builder.Property(t => t.UpdateUser).HasColumnName("UpdateUser");
            builder.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            builder.Property(t => t.RemoteIpAddress).HasColumnName("RemoteIpAddress");
            builder.Property(t => t.AuthenticateUserName).HasColumnName("AuthenticateUserName");



            builder.HasOne(t => t.Employee)
           .WithMany(t => t.EmployeeOtherPayment)
         .HasForeignKey(d => d.EmployeeId);

            builder.HasOne(t => t.OtherPaymentHeader)
           .WithMany(t => t.EmployeeOtherPayment)
         .HasForeignKey(d => d.OtherPaymentHeaderId);











        





        }
    }
}

