using Ksc.Hr.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Domain.Entities.EmployeeBase;

namespace Ksc.HR.Data.Persistant.Configurations.EmployeeBase
{
    public class IsarStatusConfiguration : IEntityTypeConfiguration<IsarStatus>
    {
        public void Configure(EntityTypeBuilder<IsarStatus> builder)
        {
            // Table & Column Mappings
            builder.ToTable("IsarStatus", "dbo");
            builder.HasKey(x => x.Id).HasName("PK_IsarStatus").IsClustered();

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
