using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ksc.Hr.Domain.Entities;
namespace Ksc.Hr.Data.Persistant.Configurations
{
    public class NationalityConfiguration : IEntityTypeConfiguration<Nationality>
    {
        public void Configure(EntityTypeBuilder<Nationality> builder)
        {
            // Table & Column Mappings
            builder.ToTable("Nationality", "EmployeeBase");
            builder.HasKey(x => x.Id).HasName("PK_Nationality").IsClustered();

            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.Title).HasColumnName("Title").IsRequired(false);
            builder.Property(t => t.InsertDate).HasColumnName("InsertDate");
            builder.Property(t => t.InsertUser).HasColumnName("InsertUser").IsRequired(false);
            builder.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired(false);
            builder.Property(t => t.UpdateUser).HasColumnName("UpdateUser").IsRequired(false);
            builder.Property(t => t.IsActive).HasColumnName("IsActive");



        }
    }
}

