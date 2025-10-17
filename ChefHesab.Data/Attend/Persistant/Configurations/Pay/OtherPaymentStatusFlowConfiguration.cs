using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ksc.Hr.Domain.Entities;
using Ksc.HR.Domain.Entities.Pay;
namespace Ksc.Hr.Data.Persistant.Configurations
{
    public class OtherPaymentStatusFlowConfiguration : IEntityTypeConfiguration<OtherPaymentStatusFlow>
    {
        public void Configure(EntityTypeBuilder<OtherPaymentStatusFlow> builder)
        {
            builder.ToTable("OtherPaymentStatusFlow", "Pay");
            builder.HasKey(x => x.Id).HasName("PK_OtherPaymentStatusFlow").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.CurrentStatusId).HasColumnName(@"CurrentStatusId").HasColumnType("int").IsRequired();
            builder.Property(x => x.NextStatusId).HasColumnName(@"NextStatusId").HasColumnType("int").IsRequired();
            builder.Property(x => x.IsActive).HasColumnName(@"IsActive").HasColumnType("bit").IsRequired();
            builder.Property(t => t.UrlName).HasColumnName("UrlName").IsRequired(false).HasMaxLength(500);

            // Foreign keys
            builder.HasOne(a => a.CurrentStatus).WithMany(b => b.CurrentStatusOtherPaymentStatusFlow).HasForeignKey(c => c.CurrentStatusId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_OtherPaymentStatusFlow_OtherPaymentCurrentStatus");
            builder.HasOne(a => a.NextStatus).WithMany(b => b.NextStatusOtherPaymentStatusFlow).HasForeignKey(c => c.NextStatusId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_OtherPaymentStatusFlow_OtherPaymentNextStatus");


        }
    }
}

