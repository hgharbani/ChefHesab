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
    public class AccountEmploymentTypeConfiguration : IEntityTypeConfiguration<AccountEmploymentType>
    {
        public void Configure(EntityTypeBuilder<AccountEmploymentType> builder)
        {
            builder.ToTable("AccountEmploymentType", "Rule");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.AccountCodeId).HasColumnName("AccountCodeId");
            builder.Property(t => t.EmploymentTypeId).HasColumnName("EmploymentTypeId");
            builder.Property(t => t.IsActive).HasColumnName("IsActive");
            //builder.Property(t => t.IsGroupSalaryAmount).HasColumnName("IsGroupSalaryAmount");
            builder.Property(t => t.InsertDate).HasColumnName("InsertDate");
            builder.Property(t => t.InsertUser).HasColumnName("InsertUser");
            builder.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            builder.Property(t => t.UpdateUser).HasColumnName("UpdateUser");



            builder.HasOne(t => t.AccountCode)
           .WithMany(t => t.AccountEmploymentTypes)
         .HasForeignKey(d => d.AccountCodeId);

            builder.HasOne(t => t.EmploymentType)
           .WithMany(t => t.AccountEmploymentTypes)
         .HasForeignKey(d => d.EmploymentTypeId);






        }
    }
}
