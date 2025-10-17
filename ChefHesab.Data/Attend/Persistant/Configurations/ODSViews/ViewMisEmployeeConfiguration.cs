using Ksc.HR.Domain.Entities.ODSViews;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Configurations.ODSViews
{
    public class ViewMisEmployeeConfiguration : IEntityTypeConfiguration<ViewMisEmployee>
    {
        public void Configure(EntityTypeBuilder<ViewMisEmployee> builder)
        {
            builder.ToView("View_MIS_Employee", "dbo");
            builder.HasNoKey();
            builder.Property(x => x.EmployeeNumber).HasColumnName(@"EmployeeNumber").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.FirstName).HasColumnName(@"FirstName").HasColumnType("nvarchar(20)").IsRequired(false).HasMaxLength(20);
            builder.Property(x => x.LastName).HasColumnName(@"LastName").HasColumnType("nvarchar(25)").IsRequired(false).HasMaxLength(25);
            builder.Property(x => x.JobPositionCode).HasColumnName(@"JobPositionCode").HasColumnType("nvarchar(13)").IsRequired(false).HasMaxLength(13);
            builder.Property(x => x.JobPositionTitle).HasColumnName(@"JobPositionTitle").HasColumnType("nvarchar(60)").IsRequired(false).HasMaxLength(60);
            builder.Property(x => x.JobCategoryCode).HasColumnName(@"JobCategoryCode").HasColumnType("nvarchar(2)").IsRequired(false).HasMaxLength(2);
            builder.Property(x => x.JobCategoryTitle).HasColumnName(@"JobCategoryTitle").HasColumnType("nvarchar(30)").IsRequired(false).HasMaxLength(30);
            builder.Property(x => x.JobStatusCode).HasColumnName(@"JobStatusCode").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.PaymentStatusCode).HasColumnName(@"PaymentStatusCode").HasColumnType("numeric(2,0)").HasPrecision(2, 0).IsRequired(false);
            builder.Property(x => x.MisUser).HasColumnName(@"MisUser").HasColumnType("nvarchar(8)").IsRequired(false).HasMaxLength(8);
            builder.Property(x => x.WinUser).HasColumnName(@"WinUser").HasColumnType("nvarchar(20)").IsRequired(false).HasMaxLength(20);

            builder.Property(x => x.EmployeeId).HasColumnName(@"EmployeeId").HasColumnType("int").IsRequired();
            builder.Property(x => x.HrMonthTimeSheet).HasColumnName(@"HrMonthTimeSheet").HasColumnType("numeric(1,0)").IsRequired(false);
            builder.Property(x => x.JobStatusDescription).HasColumnName(@"JobStatusDescription").HasColumnType("nvarchar(13)").IsRequired(false).HasMaxLength(13);
            builder.Property(x => x.CategoryId).HasColumnName(@"CategoryId").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.CategoryTitle).HasColumnName(@"CategoryTitle").HasColumnType("varchar(5)").IsRequired(false).IsUnicode(false).HasMaxLength(5);
            builder.Property(x => x.TeamCode).HasColumnName(@"TeamCode").HasColumnType("nvarchar(50)").IsRequired(false);
            builder.Property(x => x.TeamTitle).HasColumnName(@"TeamTitle").HasColumnType("nvarchar(50)").IsRequired(false);
            builder.Property(x => x.JobLevelCode).HasColumnName(@"JobLevelCode").HasColumnType("decimal(2,0)").HasPrecision(2, 0).IsRequired(false);

            builder.Property(x => x.CostCenterCode).HasColumnName(@"CostCenterCode").HasColumnType("nvarchar(30)").IsRequired(false);
            builder.Property(x => x.AccountNumber).HasColumnName(@"AccountNumber").HasColumnType("nvarchar(30)").IsRequired(false);
            builder.Property(x => x.NationalCode).HasColumnName(@"NationalCode").HasColumnType("nvarchar(50)").IsRequired(false);

        }
    }
}
