using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ksc.Hr.Domain.Entities;
using Ksc.HR.Domain.Entities.Pay;
namespace Ksc.Hr.Data.Persistant.Configurations
{
    public class EmployeeOtherPaymentHistoryConfiguration : IEntityTypeConfiguration<EmployeeOtherPaymentHistory>
    {
        public void Configure(EntityTypeBuilder<EmployeeOtherPaymentHistory> builder)
        {
            // Table & Column Mappings
            builder.ToTable("EmployeeOtherPaymentHistory", "Pay");
            builder.HasKey(t => t.Id).HasName("PK_EmployeeOtherPaymentHistory").IsClustered();

            builder.Property(t => t.EmployeeOtherPaymentId).HasColumnName("EmployeeOtherPaymentId").HasColumnType("bigint").IsRequired();

            builder.Property(t => t.InsertUser).HasColumnName("InsertUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(t => t.InsertDate).HasColumnName("InsertDate").HasColumnType("datetime").IsRequired(false);

            builder.Property(t => t.RemoteIpAddress).HasColumnName("RemoteIpAddress").HasColumnType("varchar(50)").IsRequired(false).HasMaxLength(50); ;
            builder.Property(t => t.AuthenticateUserName).HasColumnName("AuthenticateUserName").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50); ;
            builder.Property(t => t.HistoryDescription).HasColumnName("HistoryDescription").HasColumnType("nvarchar(500)").IsRequired(false).HasMaxLength(500); ;



            builder.HasOne(t => t.EmployeeOtherPayment)
           .WithMany(t => t.EmployeeOtherPaymentHistory)
         .HasForeignKey(d => d.EmployeeOtherPaymentId);


        }
    }
}

