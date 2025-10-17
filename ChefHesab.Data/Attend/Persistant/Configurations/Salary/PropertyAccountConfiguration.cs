using Ksc.HR.Domain.Entities.Salary;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Configurations.Salary
{
    public class PropertyAccountConfiguration : IEntityTypeConfiguration<PropertyAccount>
    {
        public void Configure(EntityTypeBuilder<PropertyAccount> builder)
        {
            // Table & Column Mappings
            builder.ToTable("PropertyAccount", "Salary");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.Title).HasColumnName("Title");
            builder.Property(t => t.PropertyAccountTypeId).HasColumnName("PropertyAccountTypeId");
            builder.Property(t => t.IsCheckedValidDate).HasColumnName("IsCheckedValidDate");
            builder.Property(t => t.StartValidDate).HasColumnName("StartValidDate");
            builder.Property(t => t.EndValidDate).HasColumnName("EndValidDate");
            builder.Property(t => t.InsertDate).HasColumnName("InsertDate");
            builder.Property(t => t.InsertUser).HasColumnName("InsertUser");
            builder.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            builder.Property(t => t.UpdateUser).HasColumnName("UpdateUser");
            builder.Property(t => t.IsActive).HasColumnName("IsActive");


            builder.HasOne(t => t.PropertyAccountType)
           .WithMany(t => t.PropertyAccounts)
         .HasForeignKey(d => d.PropertyAccountTypeId);


        }
    }
}
