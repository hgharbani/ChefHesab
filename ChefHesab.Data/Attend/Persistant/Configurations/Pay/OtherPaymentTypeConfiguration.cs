using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ksc.Hr.Domain.Entities;
using Ksc.HR.Domain.Entities.Pay;
namespace Ksc.Hr.Data.Persistant.Configurations
{
    public class OtherPaymentTypeConfiguration : IEntityTypeConfiguration<OtherPaymentType>
    {
        public void Configure(EntityTypeBuilder<OtherPaymentType> builder)
        {
            // Table & Column Mappings
            builder.ToTable("OtherPaymentType", "Pay");
            builder.HasKey(t => t.Id).HasName("PK_OtherPaymentType").IsClustered();


            builder.Property(t => t.Title).HasColumnName("Title").IsRequired().HasMaxLength(500);
            //builder.Property(t => t.IsValidPaymnetDelete).HasColumnName("IsValidPaymnetDelete").HasColumnType("bit").IsRequired();
            //builder.Property(t => t.IsValidRepeatableInPaymnetPeriod).HasColumnName("IsValidRepeatableInPaymnetPeriod").HasColumnType("bit").IsRequired();
            builder.Property(t => t.IsActive).HasColumnName("IsActive").HasColumnType("bit").IsRequired();
            builder.Property(t => t.InsertUser).HasColumnName("InsertUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(t => t.InsertDate).HasColumnName("InsertDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(t => t.UpdateUser).HasColumnName("UpdateUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(t => t.UpdateDate).HasColumnName("UpdateDate").HasColumnType("datetime").IsRequired(false);


        }
    }
}

