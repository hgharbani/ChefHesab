using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ksc.HR.Domain.Entities;
namespace Ksc.HR.Data.Persistant.Configurations
{
    public class EducationCategoryMap : IEntityTypeConfiguration<EducationCategory>
    {
        public void Configure(EntityTypeBuilder<EducationCategory> builder)
        {
            // Table & Column Mappings
            builder.ToTable("EducationCategory", "dbo");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.Title).HasColumnName("Title");
            builder.Property(t => t.IsActive).HasColumnName("IsActive");
            builder.Property(t => t.Score).HasColumnName("Score");
            builder.Property(t => t.LevelNumber).HasColumnName("LevelNumber");





        }
    }
}

