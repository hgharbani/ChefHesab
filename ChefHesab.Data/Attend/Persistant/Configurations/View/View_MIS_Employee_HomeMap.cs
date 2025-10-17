using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ksc.Hr.Domain.Entities;
namespace Ksc.Hr.Data.Persistant.Configurations
{
    public class View_MIS_Employee_HomeMap : IEntityTypeConfiguration<View_MIS_Employee_Home>
    {
        public void Configure(EntityTypeBuilder<View_MIS_Employee_Home> builder)
        {

            // Table & Column Mappings
            builder.ToView("View_MIS_Employee_Home", "dbo");
            builder.HasNoKey();
            builder.Property(t => t.EmployeeNumber).HasColumnName("EmployeeNumber").IsRequired().HasMaxLength(50);
            builder.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(500);
            builder.Property(t => t.Family).HasColumnName("Family").IsRequired().HasMaxLength(500);
            builder.Property(t => t.Category).HasColumnName("Category");
            builder.Property(t => t.StructureCategory).HasColumnName("StructureCategory").IsRequired().HasMaxLength(4);
            builder.Property(t => t.YearMonth).HasColumnName("YearMonth");
            builder.Property(t => t.MaxDate).HasColumnName("MaxDate").IsRequired().HasMaxLength(8);
            builder.Property(t => t.RemainDate).HasColumnName("RemainDate");
            builder.Property(t => t.HouseScore).HasColumnName("HouseScore");
            builder.Property(t => t.InHouseYear).HasColumnName("InHouseYear");
            builder.Property(t => t.InHouseMonth).HasColumnName("InHouseMonth");
            builder.Property(t => t.InHouseDay).HasColumnName("InHouseDay");
            builder.Property(t => t.RemainYear).HasColumnName("RemainYear");
            builder.Property(t => t.RemainMonth).HasColumnName("RemainMonth");
            builder.Property(t => t.RemainDay).HasColumnName("RemainDay");












        }
    }
}

