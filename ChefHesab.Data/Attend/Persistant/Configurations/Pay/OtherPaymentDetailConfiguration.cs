using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ksc.Hr.Domain.Entities;
using Ksc.HR.Domain.Entities.Pay;
namespace Ksc.Hr.Data.Persistant.Configurations
{
    public class OtherPaymentDetailConfiguration : IEntityTypeConfiguration<OtherPaymentDetail>
    {
        public void Configure(EntityTypeBuilder<OtherPaymentDetail> builder)
        {
            // Table & Column Mappings
            builder.ToTable("OtherPaymentDetail", "Pay");
            builder.HasKey(t => t.Id).HasName("PK_OtherPaymentDetail").IsClustered();


            builder.Property(t => t.OtherPaymentHeaderId).HasColumnName("OtherPaymentHeaderId").HasColumnType("int").IsRequired();
            builder.Property(t => t.OtherPaymentStatusId).HasColumnName("OtherPaymentStatusId").HasColumnType("int").IsRequired();
            builder.Property(t => t.InsertUser).HasColumnName("InsertUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(t => t.InsertDate).HasColumnName("InsertDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(t => t.RemoteIpAddress).HasColumnName("RemoteIpAddress").HasColumnType("varchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(t => t.AuthenticateUserName).HasColumnName("AuthenticateUserName").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);


            builder.HasOne(t => t.OtherPaymentHeader)
           .WithMany(t => t.OtherPaymentDetail)
         .HasForeignKey(d => d.OtherPaymentHeaderId);

            builder.HasOne(t => t.OtherPaymentStatus)
           .WithMany(t => t.OtherPaymentDetail)
         .HasForeignKey(d => d.OtherPaymentStatusId);


        }
    }
}

