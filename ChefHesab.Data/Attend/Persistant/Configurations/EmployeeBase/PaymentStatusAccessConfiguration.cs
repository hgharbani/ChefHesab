using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ksc.Hr.Domain.Entities;
namespace Ksc.Hr.Data.Persistant.Configurations
{
    public class PaymentStatusAccessConfiguration : IEntityTypeConfiguration<PaymentStatusAccess>
    {
        public void Configure(EntityTypeBuilder<PaymentStatusAccess> builder)
        {
            builder.ToTable("PaymentStatusAccess", "EmployeeBase");
            builder.HasKey(x => x.Id).HasName("PK_PaymentStatusAccess").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.PaymentStatusId).HasColumnName(@"PaymentStatusId").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.AccessLevelId).HasColumnName(@"AccessLevelId").HasColumnType("int").IsRequired();

            // Foreign keys
            builder.HasOne(a => a.AccessLevel).WithMany(b => b.EmployeeBase_PaymentStatusAccesses).HasForeignKey(c => c.AccessLevelId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_PaymentStatusAccess_AccessLevel");
        }
    }
}

