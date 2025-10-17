using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ksc.Hr.Domain.Entities;
namespace Ksc.Hr.Data.Persistant.Configurations
{
    public class BloodTypeConfiguration : IEntityTypeConfiguration<BloodType>
    {
        public void Configure(EntityTypeBuilder<BloodType> builder)
        {
            // Table & Column Mappings
            builder.ToTable("BloodType", "EmployeeBase");
            builder.HasKey(x => x.Id).HasName("PK_BloodType").IsClustered();
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.Title).HasColumnName("Title").IsRequired(false);
            builder.Property(t => t.InsertDate).HasColumnName("InsertDate").IsRequired(false);
            builder.Property(t => t.InsertUser).HasColumnName("InsertUser").IsRequired(false);
            builder.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired(false);
            builder.Property(t => t.UpdateUser).HasColumnName("UpdateUser").IsRequired(false);
            builder.Property(t => t.IsActive).HasColumnName("IsActive");


        }
    }
}

