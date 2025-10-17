using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ksc.Hr.Domain.Entities;
namespace Ksc.Hr.Data.Persistant.Configurations
{
    public class ViewEmployeeFamilyReportMap : IEntityTypeConfiguration<ViewEmployeeFamilyReport>
    {
        public void Configure(EntityTypeBuilder<ViewEmployeeFamilyReport> builder)
        {
            // Table & Column Mappings
            builder.ToView("View_EmployeeFamilyReport", "dbo");
            builder.HasNoKey();
            builder.Property(t => t.EmployeeNumber).HasColumnName("EmployeeNumber").IsRequired().HasMaxLength(50);
            builder.Property(t => t.FirstName).HasColumnName("FirstName").IsRequired().HasMaxLength(500);
            builder.Property(t => t.LastName).HasColumnName("LastName").IsRequired().HasMaxLength(500);
            builder.Property(t => t.NationalCode).HasColumnName("NationalCode").IsRequired().HasMaxLength(20);
            builder.Property(t => t.ParentCertificateCityId).HasColumnName("ParentCertificateCityId");
            builder.Property(t => t.CertificateCityTitle).HasColumnName("CertificateCityTitle").IsRequired().HasMaxLength(300);
            builder.Property(t => t.CertificateDate).HasColumnName("CertificateDate");
            builder.Property(t => t.ShamsiCertificateDate).HasColumnName("ShamsiCertificateDate");
            builder.Property(t => t.BirthDate).HasColumnName("BirthDate");
            builder.Property(t => t.ShamsiBirthDate).HasColumnName("ShamsiBirthDate");
            builder.Property(t => t.DependenceTypeTitle).HasColumnName("DependenceTypeTitle").IsRequired();
            builder.Property(t => t.ShamsiStartDateDependent).HasColumnName("ShamsiStartDateDependent");
            builder.Property(t => t.ShamsiEndDateDependent).HasColumnName("ShamsiEndDateDependent");
            builder.Property(t => t.ParentFirstName).HasColumnName("ParentFirstName").IsRequired().HasMaxLength(500);
            builder.Property(t => t.ParentFamily).HasColumnName("ParentFamily").IsRequired().HasMaxLength(500);
            builder.Property(t => t.parentNationalCode).HasColumnName("parentNationalCode");
            builder.Property(t => t.MaritalStatusTitle).HasColumnName("MaritalStatusTitle").IsRequired().HasMaxLength(50);
            builder.Property(t => t.ParentBirthDate).HasColumnName("ParentBirthDate");
            builder.Property(t => t.ParentShamsiBirthDate).HasColumnName("ParentShamsiBirthDate");
            builder.Property(t => t.PhoneNumber).HasColumnName("PhoneNumber");
            builder.Property(t => t.InsertDate).HasColumnName("InsertDate");
            builder.Property(t => t.ShamsiInsertDate).HasColumnName("ShamsiInsertDate");
            builder.Property(t => t.InsertUser).HasColumnName("InsertUser");
            builder.Property(t => t.UpdateUser).HasColumnName("UpdateUser");
            builder.Property(t => t.ShamsiUpdateDate).HasColumnName("ShamsiUpdateDate");
            builder.Property(t => t.EmploymentTypeId).HasColumnName("EmploymentTypeId");
            builder.Property(t => t.EmploymentTypetitle).HasColumnName("EmploymentTypetitle");
            builder.Property(t => t.BookletIsActive).HasColumnName("BookletIsActive");
            builder.Property(t => t.DependenceReasonId).HasColumnName("DependenceReasonId");
            builder.Property(t => t.DependenceReasonTitle).HasColumnName("DependenceReasonTitle");
            builder.Property(t => t.EmploymentDate).HasColumnName("EmploymentDate");
            builder.Property(t => t.ShamsiEmploymentDate).HasColumnName("ShamsiEmploymentDate");
            builder.Property(t => t.ParentGenderId).HasColumnName("ParentGenderId");
            builder.Property(t => t.GenderId).HasColumnName("GenderId");
            builder.Property(t => t.IsContinuesDependent).HasColumnName("IsContinuesDependent");
            builder.Property(t => t.DependenceTypeId).HasColumnName("DependenceTypeId");
            builder.Property(t => t.BirthCityId).HasColumnName("BirthCityId");
            builder.Property(t => t.CertificateNumber).HasColumnName("CertificateNumber").IsRequired().HasMaxLength(50);
            builder.Property(t => t.CertificateCityId).HasColumnName("CertificateCityId");
            builder.Property(t => t.NationalityId).HasColumnName("NationalityId");
            builder.Property(t => t.DependenceJobId).HasColumnName("DependenceJobId");
            builder.Property(t => t.DependentExitDateReasonId).HasColumnName("DependentExitDateReasonId");
            builder.Property(t => t.StartDateDependent).HasColumnName("StartDateDependent");
            builder.Property(t => t.EndDateDependent).HasColumnName("EndDateDependent");
            builder.Property(t => t.IsCompleteInsurance).HasColumnName("IsCompleteInsurance");
            builder.Property(t => t.ValidDateInsurance).HasColumnName("ValidDateInsurance");
            builder.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            builder.Property(t => t.IsActive).HasColumnName("IsActive");
            builder.Property(t => t.WorkCityId).HasColumnName("WorkCityId");
            builder.Property(t => t.PaymentStatusId).HasColumnName("PaymentStatusId");
            builder.Property(t => t.ParentBirthCityId).HasColumnName("ParentBirthCityId");
            builder.Property(t => t.ParentCertificateNumber).HasColumnName("ParentCertificateNumber");
            builder.Property(t => t.ParentCertificateDate).HasColumnName("ParentCertificateDate");
            builder.Property(t => t.MarriedDate).HasColumnName("MarriedDate");
            builder.Property(t => t.JobPositionId).HasColumnName("JobPositionId");
            builder.Property(t => t.JobPositionStartDate).HasColumnName("JobPositionStartDate");
            builder.Property(t => t.GenderTitle).HasColumnName("GenderTitle");
            builder.Property(t => t.ParentGenderTitle).HasColumnName("ParentGenderTitle");
            builder.Property(t => t.PaymentStatusTitle).HasColumnName("PaymentStatusTitle");
            builder.Property(t => t.EmployeeId).HasColumnName("EmployeeId");
            builder.Property(t => t.FamilyBookletNumber).HasColumnName("FamilyBookletNumber");
            builder.Property(t => t.FamilyStartSerialNumber).HasColumnName("FamilyStartSerialNumber");
            builder.Property(t => t.FamilyExitSerialNumber).HasColumnName("FamilyExitSerialNumber");
            builder.Property(t => t.FamilyBookletIssuingCityId).HasColumnName("FamilyBookletIssuingCityId");
            builder.Property(t => t.FamilyFranchiseAmount).HasColumnName("FamilyFranchiseAmount");
            builder.Property(t => t.FamilyFranchiseTypeId).HasColumnName("FamilyFranchiseTypeId");
            builder.Property(t => t.FamilyBookletIsActive).HasColumnName("FamilyBookletIsActive");
            builder.Property(t => t.FamilyBookletEndValidDate).HasColumnName("FamilyBookletEndValidDate");
            builder.Property(t => t.ParentBookletNumber).HasColumnName("ParentBookletNumber");
            builder.Property(t => t.ParentStartSerialNumber).HasColumnName("ParentStartSerialNumber");
            builder.Property(t => t.ParentExitSerialNumber).HasColumnName("ParentExitSerialNumber");
            builder.Property(t => t.ParentBookletIssuingCityId).HasColumnName("ParentBookletIssuingCityId");
            builder.Property(t => t.ParentFranchiseAmount).HasColumnName("ParentFranchiseAmount");
            builder.Property(t => t.ParentFranchiseTypeId).HasColumnName("ParentFranchiseTypeId");
            builder.Property(t => t.ParentBookletIsActive).HasColumnName("ParentBookletIsActive");
            builder.Property(t => t.ParentBookletEndValidDate).HasColumnName("ParentBookletEndValidDate");
            builder.Property(t => t.PersonalTypeId).HasColumnName("PersonalTypeId");

        }
    }
}

