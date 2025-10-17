using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ksc.Hr.Domain.Entities;
using Ksc.HR.Domain.Entities;

namespace Ksc.Hr.Data.Persistant.Configurations
{
    public class TableDefinitionConfiguration : IEntityTypeConfiguration<TableDefinition>
    {
        public void Configure(EntityTypeBuilder<TableDefinition> builder)
        {
            builder.ToTable("TableDefinition", "dbo");
            builder.HasKey(x => x.Id).HasName("PK_TableDefinition").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.Title).HasColumnName(@"Title").HasColumnType("nvarchar(150)").IsRequired().HasMaxLength(150);
            builder.Property(x => x.UrlName).HasColumnName(@"UrlName").HasColumnType("nvarchar(500)").IsRequired().HasMaxLength(500);
            builder.Property(x => x.TableName).HasColumnName(@"TableName").HasColumnType("nvarchar(150)").IsRequired().HasMaxLength(150);
            builder.Property(x => x.IsActive).HasColumnName(@"IsActive").HasColumnType("bit").IsRequired();
        }
    }
}

