using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ksc.Hr.Domain.Entities;
using Ksc.HR.Domain.Entities.Pay;
namespace Ksc.Hr.Data.Persistant.Configurations
{
    public class OtherPaymentHeaderConfiguration : IEntityTypeConfiguration<OtherPaymentHeader>
    {
        public void Configure(EntityTypeBuilder<OtherPaymentHeader> builder)
        {
            // Table & Column Mappings
            builder.ToTable("OtherPaymentHeader", "Pay");
            builder.HasKey(t => t.Id).HasName("PK_OtherPaymentHeader").IsClustered();


            builder.Property(t => t.PaymentYearMonth).HasColumnName("PaymentYearMonth").HasColumnType("int").IsRequired();
            builder.Property(t => t.AccountCodeId).HasColumnName("AccountCodeId").HasColumnType("int").IsRequired();
            builder.Property(t => t.OtherPaymentStatusId).HasColumnName("OtherPaymentStatusId").HasColumnType("int").IsRequired();

            builder.Property(t => t.AccountingDocumentNumber).HasColumnName("AccountingDocumentNumber").HasColumnType("int").IsRequired(false);
            builder.Property(t => t.AccountingDocumentDate).HasColumnName("AccountingDocumentDate").HasColumnType("date").IsRequired(false);
            builder.Property(t => t.BankId).HasColumnName("BankId").HasColumnType("int").IsRequired(false);
            builder.Property(t => t.AccountBankTypeId).HasColumnName("AccountBankTypeId").HasColumnType("int").IsRequired(false);
            builder.Property(t => t.ShowInPortal).HasColumnName("ShowInPortal");
            builder.Property(t => t.YearMonthStartReport).HasColumnName("YearMonthStartReport");
            builder.Property(t => t.YearMonthEndReport).HasColumnName("YearMonthEndReport");
            builder.Property(x => x.DescriptionForPortal).HasColumnName(@"DescriptionForPortal").HasColumnType("nvarchar(max)").IsRequired(false);
            builder.Property(x => x.PaymentDateForPortal).HasColumnName(@"PaymentDateForPortal").HasColumnType("datetime").IsRequired(false);
            builder.HasOne(t => t.AccountCode)
           .WithMany(t => t.OtherPaymentHeaders)
         .HasForeignKey(d => d.AccountCodeId);

            builder.HasOne(t => t.OtherPaymentStatus)
           .WithMany(t => t.OtherPaymentHeader)
         .HasForeignKey(d => d.OtherPaymentStatusId);


            builder.HasOne(t => t.AccountBankType)
           .WithMany(t => t.OtherPaymentHeaders)
         .HasForeignKey(d => d.AccountBankTypeId);

        }
    }
}

