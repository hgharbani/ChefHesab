using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ksc.Hr.Domain.Entities;
namespace Ksc.Hr.Data.Persistant.Configurations
{
    public class HrFormulasConfiguration : IEntityTypeConfiguration<HrFormulas>
    {
        public void Configure(EntityTypeBuilder<HrFormulas> builder)
        {
            // Table & Column Mappings
            builder.ToTable("HrFormulas", "dbo");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.HasKey(x => x.Id).HasName("PK_HrFormulas").IsClustered();
            builder.Property(t => t.Code).HasColumnName("Code");
            builder.Property(t => t.Title).HasColumnName("Title").IsRequired();
            builder.Property(t => t.Experssion).HasColumnName("Experssion").IsRequired();
            builder.Property(t => t.ExperssionPersianTitle).HasColumnName("ExperssionPersianTitle");
            builder.Property(t => t.Parameters).HasColumnName("Parameters");
        }
    }
}

