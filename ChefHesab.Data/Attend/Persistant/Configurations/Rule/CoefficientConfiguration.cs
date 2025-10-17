using Ksc.HR.Domain.Entities.Rule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Configurations.Rule
{
    public class CoefficientConfiguration : IEntityTypeConfiguration<Coefficient>
    {
        public void Configure(EntityTypeBuilder<Coefficient> builder)
        {
            // Table & Column Mappings
            builder.ToTable("Coefficient", "Rule");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.Title).HasColumnName("Title");

            builder.Property(t => t.IsActive).HasColumnName("IsActive");
            builder.Property(t => t.InsertDate).HasColumnName("InsertDate");
            builder.Property(t => t.InsertUser).HasColumnName("InsertUser");
            builder.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            builder.Property(t => t.UpdateUser).HasColumnName("UpdateUser");

        }
    }
}
