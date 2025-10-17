using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ksc.Hr.Domain.Entities;
namespace Ksc.Hr.Data.Persistant.Configurations
{
    public class VacationMap : IEntityTypeConfiguration<Vacation>
    {
        public void Configure(EntityTypeBuilder<Vacation> builder)
        {
            // Table & Column Mappings
            builder.ToTable("Vacation", "dbo");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.Description).HasColumnName("Description");
            builder.Property(t => t.Code).HasColumnName("Code");
            builder.Property(t => t.OrderNo).HasColumnName("OrderNo");
            builder.Property(t => t.ShowInHistory).HasColumnName("ShowInHistory");
            builder.Property(t => t.ShowInManagement).HasColumnName("ShowInManagement");
            builder.Property(t => t.ReadonlyInManagement).HasColumnName("ReadonlyInManagement");

            
        }
    }
}

