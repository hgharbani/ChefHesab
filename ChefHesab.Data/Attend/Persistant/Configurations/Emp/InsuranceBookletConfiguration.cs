using Ksc.HR.Domain.Entities.Emp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Configurations.Emp
{
    public class InsuranceBookletConfiguration : IEntityTypeConfiguration<InsuranceBooklet>
    {
        public void Configure(EntityTypeBuilder<InsuranceBooklet> builder)
        {
            builder.ToTable("InsuranceBooklet", "Emp");
            builder.HasKey(x => x.Id).HasName("PK_Booklet").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("bigint").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.BookletNumber).HasColumnName(@"BookletNumber").HasColumnType("nvarchar(max)").IsRequired(false);
            builder.Property(x => x.IsDuplicate).HasColumnName(@"IsDuplicate").HasColumnType("bit").IsRequired();
            builder.Property(x => x.DuplicateNumber).HasColumnName(@"DuplicateNumber").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.DuplicateDate).HasColumnName(@"DuplicateDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.IssueDate).HasColumnName(@"IssueDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.StartSerialNumber).HasColumnName(@"StartSerialNumber").HasColumnType("nvarchar(max)").IsRequired(false);
            builder.Property(x => x.ExitSerialNumber).HasColumnName(@"ExitSerialNumber").HasColumnType("nvarchar(max)").IsRequired(false);
            builder.Property(x => x.PrintYear).HasColumnName(@"PrintYear").HasColumnType("nvarchar(max)").IsRequired(false);
            builder.Property(x => x.StartValidDate).HasColumnName(@"StartValidDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.EndValidDate).HasColumnName(@"EndValidDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.IsActive).HasColumnName(@"IsActive").HasColumnType("bit").IsRequired();
            builder.Property(x => x.BookletIssuingCityId).HasColumnName(@"BookletIssuingCityId").HasColumnType("int").IsRequired();
            builder.Property(x => x.FranchiseTypeId).HasColumnName(@"FranchiseTypeId").HasColumnType("int").IsRequired();
            builder.Property(x => x.FranchiseAmount).HasColumnName(@"FranchiseAmount").HasColumnType("float").HasPrecision(53).IsRequired();
            builder.Property(x => x.EmployeeId).HasColumnName(@"EmployeeId").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.FamilyId).HasColumnName(@"FamilyId").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.PensionerId).HasColumnName(@"PensionerId").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.ParentId).HasColumnName(@"ParentId").HasColumnType("bigint").IsRequired(false);
            builder.Property(x => x.Description).HasColumnName(@"Description").HasColumnType("nvarchar(max)").IsRequired(false);
            builder.Property(x => x.InsertDate).HasColumnName(@"InsertDate").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.UpdateDate).HasColumnName(@"UpdateDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.InsertUser).HasColumnName(@"InsertUser").HasColumnType("nvarchar(max)").IsRequired();
            builder.Property(x => x.UpdateUser).HasColumnName(@"UpdateUser").HasColumnType("nvarchar(max)").IsRequired(false);

            // Foreign keys
            builder.HasOne(a => a.Family).WithMany(b => b.InsuranceBooklets).HasForeignKey(c => c.FamilyId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_InsuranceBooklet_Family");
            builder.HasOne(a => a.Employee).WithMany(b => b.InsuranceBooklets).HasForeignKey(c => c.EmployeeId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_InsuranceBooklet_Employee");
            builder.HasOne(a => a.FranchiseType).WithMany(b => b.InsuranceBooklets).HasForeignKey(c => c.FranchiseTypeId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_InsuranceBooklet_FranchiseType");
            builder.HasOne(a => a.Parent).WithMany(b => b.InsuranceBooklets).HasForeignKey(c => c.ParentId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_InsuranceBooklet_InsuranceBooklet");
        }


    }
}
