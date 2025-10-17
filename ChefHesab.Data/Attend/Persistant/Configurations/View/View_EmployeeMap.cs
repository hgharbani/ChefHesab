using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ksc.Hr.Domain.Entities;
using Ksc.HR.Domain.Entities.View;
namespace Ksc.Hr.Data.Persistant.Configurations
{
    public class View_EmployeeMap : IEntityTypeConfiguration<View_Employee>
    {
        public void Configure(EntityTypeBuilder<View_Employee> builder)
        {
            // Table & Column Mappings
            builder.ToView("View_Employee", "dbo");
            builder.HasNoKey();
            builder.Property(t => t.EmployeeId).HasColumnName("EmployeeId").IsRequired();
            builder.Property(t => t.WindowsUser).HasColumnName("WindowsUser");
            builder.Property(t => t.EmployeeNumber).HasColumnName("EmployeeNumber").IsRequired().HasMaxLength(50);
            builder.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(500);
            builder.Property(t => t.Family).HasColumnName("Family").IsRequired().HasMaxLength(500);
            builder.Property(t => t.PhoneNumber).HasColumnName("PhoneNumber");
            builder.Property(t => t.TeamWorkId).HasColumnName("TeamWorkId");
            builder.Property(t => t.TeamCode).HasColumnName("TeamCode");
            builder.Property(t => t.TeamTitle).HasColumnName("TeamTitle");
            builder.Property(t => t.MisJobPositionCode).HasColumnName("MisJobPositionCode");
            builder.Property(t => t.JobPositionTitle).HasColumnName("JobPositionTitle");
            builder.Property(t => t.PaymentStatusId).HasColumnName("PaymentStatusId");
            builder.Property(t => t.PaymentStatusTitle).HasColumnName("PaymentStatusTitle");
            builder.Property(t => t.EmploymentTypeId).HasColumnName("EmploymentTypeId");
            builder.Property(t => t.EmploymentTypeTitle).HasColumnName("EmploymentTypeTitle");
            builder.Property(t => t.JobCategoryCode).HasColumnName("JobCategoryCode");
            builder.Property(t => t.JobCategoryTitle).HasColumnName("JobCategoryTitle");
            builder.Property(t => t.CostCenter).HasColumnName("CostCenter").IsRequired(false);
            builder.Property(t => t.JobStatusCode).HasColumnName("JobStatusCode");
            builder.Property(t => t.JobStatusDescription).HasColumnName("JobStatusDescription");

        }
    }
}

