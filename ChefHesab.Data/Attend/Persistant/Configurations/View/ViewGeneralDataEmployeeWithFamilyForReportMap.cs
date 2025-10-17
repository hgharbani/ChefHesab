using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ksc.Hr.Domain.Entities;
namespace Ksc.Hr.Data.Persistant.Configurations
{
    public class ViewGeneralDataEmployeeWithFamilyForReportMap : IEntityTypeConfiguration<ViewGeneralDataEmployeeWithFamilyForReport>
    {
        public void Configure(EntityTypeBuilder<ViewGeneralDataEmployeeWithFamilyForReport> builder)
        {
            // Table & Column Mappings
            builder.ToView("View_GeneralDataEmployeeWithFamilyForReport", "dbo");
            builder.HasNoKey();
            builder.Property(x => x.EmployeeNumber).HasColumnName(@"EmployeeNumber").HasColumnType("nvarchar(50)").IsRequired().HasMaxLength(50);
            builder.Property(x => x.EmploymentTypeId).HasColumnName(@"EmploymentTypeId").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.PhoneNumber).HasColumnName(@"PhoneNumber").HasColumnType("nvarchar(20)").IsRequired(false).HasMaxLength(20);
            builder.Property(x => x.EmployeeId).HasColumnName(@"EmployeeId").HasColumnType("int").IsRequired();
            builder.Property(x => x.PaymentStatusId).HasColumnName(@"PaymentStatusId").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.EmployeeFamilyId).HasColumnName(@"EmployeeFamilyId").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.FirstName).HasColumnName(@"FirstName").HasColumnType("nvarchar(500)").IsRequired().HasMaxLength(500);
            builder.Property(x => x.LastName).HasColumnName(@"LastName").HasColumnType("nvarchar(500)").IsRequired().HasMaxLength(500);
            builder.Property(x => x.NationalCode).HasColumnName(@"NationalCode").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.DependenceTypeId).HasColumnName(@"DependenceTypeId").HasColumnType("int").IsRequired();
            builder.Property(x => x.DependenceTypeTitle).HasColumnName(@"DependenceTypeTitle").HasColumnType("nvarchar(max)").IsRequired();
            builder.Property(x => x.IsContinuesDependent).HasColumnName(@"IsContinuesDependent").HasColumnType("bit").IsRequired(false);
            builder.Property(x => x.IsCompleteInsurance).HasColumnName(@"IsCompleteInsurance").HasColumnType("bit").IsRequired(false);
            builder.Property(x => x.StartDateDependent).HasColumnName(@"StartDateDependent").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.EndDateDependent).HasColumnName(@"EndDateDependent").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.BirthDate).HasColumnName(@"BirthDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.BookletIsActive).HasColumnName(@"BookletIsActive").HasColumnType("bit").IsRequired(false);
            builder.Property(x => x.BookletNumber).HasColumnName(@"BookletNumber").HasColumnType("nvarchar(max)").IsRequired(false);
            builder.Property(x => x.FranchiseAmount).HasColumnName(@"FranchiseAmount").HasColumnType("float").HasPrecision(53).IsRequired(false);
            builder.Property(x => x.FranchiseTypeId).HasColumnName(@"FranchiseTypeId").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.BookletEndValidDate).HasColumnName(@"BookletEndValidDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.BookletIssuingCityId).HasColumnName(@"BookletIssuingCityId").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.GenderId).HasColumnName(@"GenderId").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.FatherName).HasColumnName(@"FatherName").HasColumnType("nvarchar(500)").IsRequired(false).HasMaxLength(500);
            builder.Property(x => x.SequenceNumber).HasColumnName(@"SequenceNumber").HasColumnType("int").IsRequired();
            builder.Property(x => x.GenderTitle).HasColumnName(@"GenderTitle").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.FranchTitle).HasColumnName(@"FranchTitle").HasColumnType("nvarchar(max)").IsRequired(false);
            builder.Property(x => x.JobPositionCode).HasColumnName(@"JobPositionCode").HasColumnType("nvarchar(13)").IsRequired(false).HasMaxLength(13);
            builder.Property(x => x.JobPositionTitle).HasColumnName(@"JobPositionTitle").HasColumnType("nvarchar(60)").IsRequired(false).HasMaxLength(60);
            builder.Property(x => x.NationalCodeParent).HasColumnName(@"NationalCodeParent").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.FirstNameParent).HasColumnName(@"FirstNameParent").HasColumnType("nvarchar(20)").IsRequired(false).HasMaxLength(20);
            builder.Property(x => x.LastNameParent).HasColumnName(@"LastNameParent").HasColumnType("nvarchar(25)").IsRequired(false).HasMaxLength(25);
            builder.Property(x => x.EmploymentDate).HasColumnName(@"EmploymentDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.MaritalStatusId).HasColumnName(@"MaritalStatusId").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.PersonalTypeId).HasColumnName(@"PersonalTypeId").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.BirthCityId).HasColumnName(@"BirthCityId").HasColumnType("int").IsRequired();
            builder.Property(x => x.DependentExitDateReasonId).HasColumnName(@"DependentExitDateReasonId").HasColumnType("int").IsRequired(false);

            builder.Property(x => x.CertificateDate).HasColumnName(@"CertificateDate").HasColumnType("datetime").IsRequired(false);

        }
    }
}

