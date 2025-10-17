using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ksc.Hr.Domain.Entities;
using Ksc.HR.Domain.Entities.Salary;
using Ksc.HR.Domain.Entities.Rule;
namespace Ksc.Hr.Data.Persistant.Configurations
{
    public class AccountCodeConfiguration : IEntityTypeConfiguration<AccountCode>
    {
        public void Configure(EntityTypeBuilder<AccountCode> builder)
        {
            // Table & Column Mappings
            builder.ToTable("AccountCode", "Salary");
            builder.HasKey(t => t.Id).HasName("PK_AccountCode").IsClustered();

            builder.Property(t => t.Title).HasColumnName("Title").IsRequired().HasMaxLength(500);
            builder.Property(t => t.UrlOtherPayment).HasColumnName("UrlOtherPayment").IsRequired(false).HasMaxLength(500);
            builder.Property(t => t.InterdictCategoryId).HasColumnName("InterdictCategoryId").IsRequired(false);
            builder.Property(t => t.AccountCodeCategoryId).HasColumnName("AccountCodeCategoryId").IsRequired(false);
            builder.Property(t => t.IsAddational).HasColumnName("IsAddational");
            builder.Property(t => t.IsBeneficaryCode).HasColumnName("IsBeneficaryCode");
            builder.Property(t => t.IsAutomatic).HasColumnName("IsAutomatic");
            builder.Property(t => t.IsAllMartialSataus).HasColumnName("IsAllMartialSataus");
            builder.Property(t => t.IsOnceUsed).HasColumnName("IsOnceUsed");
            builder.Property(t => t.IsValidInPrintedFish).HasColumnName("IsValidInPrintedFish");
            builder.Property(t => t.IsDebetToCompany).HasColumnName("IsDebetToCompany");
            builder.Property(t => t.IsInvalidUsedInReport).HasColumnName("IsInvalidUsedInReport");
            builder.Property(t => t.IsValidUseInterdictMaritalSetting).HasColumnName("IsValidUseInterdictMaritalSetting");

            builder.Property(t => t.IsValidDeleteInOtherPaymnet).HasColumnName("IsValidDeleteInOtherPaymnet").HasColumnType("bit").IsRequired();
            builder.Property(t => t.IsValidRepeatableInOtherPaymnetPeriod).HasColumnName("IsValidRepeatableInOtherPaymnetPeriod").HasColumnType("bit").IsRequired();
            builder.Property(t => t.IsActive).HasColumnName("IsActive").HasColumnType("bit").IsRequired();
            builder.Property(t => t.InsertUser).HasColumnName("InsertUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(t => t.InsertDate).HasColumnName("InsertDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(t => t.UpdateUser).HasColumnName("UpdateUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(t => t.UpdateDate).HasColumnName("UpdateDate").HasColumnType("datetime").IsRequired(false);


            builder.HasOne(a => a.AccountCodeCategory).WithMany(b => b.AccountCodes).HasForeignKey(c => c.AccountCodeCategoryId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_AccountCode_AccountCodeCategory");
            builder.HasOne(a => a.InterdictCategory).WithMany(b => b.AccountCodes).HasForeignKey(c => c.InterdictCategoryId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_AccountCode_InterdictCategory");

        }
    }
}

