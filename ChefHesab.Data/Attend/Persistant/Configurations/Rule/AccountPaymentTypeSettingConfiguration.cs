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
    public class AccountPaymentTypeSettingConfiguration : IEntityTypeConfiguration<AccountPaymentTypeSetting>
    {
        public void Configure(EntityTypeBuilder<AccountPaymentTypeSetting> builder)
        {
            
            builder.ToTable("AccountPaymentTypeSetting", "Rule");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.AccountPaymentTypeId).HasColumnName("AccountPaymentTypeId");
            builder.Property(t => t.StartDate).HasColumnName("StartDate");
            builder.Property(t => t.EndDate).HasColumnName("EndDate");
            builder.Property(t => t.MaritalStatusId).HasColumnName("MaritalStatusId");
            builder.Property(t => t.Amount).HasColumnName("Amount");
            builder.Property(t => t.JobGroupId).HasColumnName("JobGroupId");
            builder.Property(t => t.IsActive).HasColumnName("IsActive");
            builder.Property(t => t.InsertDate).HasColumnName("InsertDate");
            builder.Property(t => t.InsertUser).HasColumnName("InsertUser");
            builder.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            builder.Property(t => t.UpdateUser).HasColumnName("UpdateUser");



            builder.HasOne(t => t.AccountEmploymentType)
           .WithMany(t => t.AccountPaymentTypeSettings)
         .HasForeignKey(d => d.AccountPaymentTypeId);



            builder.HasOne(t => t.MaritalStatus)
           .WithMany(t => t.AccountPaymentTypeSettings)
         .HasForeignKey(d => d.MaritalStatusId);


            builder.HasOne(t => t.JobGroup)
           .WithMany(t => t.AccountPaymentTypeSettings)
         .HasForeignKey(d => d.JobGroupId);






        }
    }
}
