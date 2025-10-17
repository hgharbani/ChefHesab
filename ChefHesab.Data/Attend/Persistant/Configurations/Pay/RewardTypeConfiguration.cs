using Ksc.HR.Domain.Entities.Pay;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ksc.HR.Data.Persistant.Configurations.Pay
{
    public class RewardTypeConfiguration : IEntityTypeConfiguration<RewardType>
    {
        public void Configure(EntityTypeBuilder<RewardType> builder)
        {
            builder.ToTable("RewardType", "Pay");
            builder.HasKey(x => x.Id).HasName("PK_RewardType").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.Title).HasColumnName(@"Title").HasColumnType("nvarchar(500)").IsRequired(false).HasMaxLength(500);
            builder.Property(x => x.IsActive).HasColumnName(@"IsActive").HasColumnType("bit").IsRequired();
            builder.Property(x => x.InsertUser).HasColumnName(@"InsertUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.InsertDate).HasColumnName(@"InsertDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.UpdateUser).HasColumnName(@"UpdateUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.UpdateDate).HasColumnName(@"UpdateDate").HasColumnType("datetime").IsRequired(false);
        }
    }
}
