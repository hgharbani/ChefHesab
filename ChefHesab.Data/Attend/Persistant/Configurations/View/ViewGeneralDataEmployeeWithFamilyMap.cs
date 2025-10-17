using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ksc.Hr.Domain.Entities;
namespace Ksc.Hr.Data.Persistant.Configurations
{
    public class View_GeneralDataEmployeeWithFamilyMap : IEntityTypeConfiguration<ViewGeneralDataEmployeeWithFamily>
    {
        public void Configure(EntityTypeBuilder<ViewGeneralDataEmployeeWithFamily> builder)
        {
            // Table & Column Mappings
            builder.ToView("View_GeneralDataEmployeeWithFamily", "dbo");
            builder.HasNoKey();
            builder.Property(t => t.EmployeeNumber).HasColumnName("EmployeeNumber");
            builder.Property(t => t.EmploymentTypeId).HasColumnName("EmploymentTypeId");
            builder.Property(t => t.PhoneNumber).HasColumnName("PhoneNumber");
            builder.Property(t => t.EmployeeId).HasColumnName("EmployeeId");
            builder.Property(t => t.PaymentStatusId).HasColumnName("PaymentStatusId");
            builder.Property(t => t.EmployeeFamilyId).HasColumnName("EmployeeFamilyId");
            builder.Property(t => t.FirstName).HasColumnName("FirstName");
            builder.Property(t => t.LastName).HasColumnName("LastName");
            builder.Property(t => t.NationalCode).HasColumnName("NationalCode");
            builder.Property(t => t.DependenceTypeId).HasColumnName("DependenceTypeId");
            builder.Property(t => t.DependenceTypeTitle).HasColumnName("DependenceTypeTitle");
            builder.Property(t => t.IsContinuesDependent).HasColumnName("IsContinuesDependent");
            builder.Property(t => t.IsCompleteInsurance).HasColumnName("IsCompleteInsurance");
            builder.Property(t => t.StartDateDependent).HasColumnName("StartDateDependent");
            builder.Property(t => t.EndDateDependent).HasColumnName("EndDateDependent");
            builder.Property(t => t.BirthDate).HasColumnName("BirthDate");
            builder.Property(t => t.BookletIsActive).HasColumnName("BookletIsActive");
            builder.Property(t => t.BookletNumber).HasColumnName("BookletNumber");
            builder.Property(t => t.FranchiseAmount).HasColumnName("FranchiseAmount");
            builder.Property(t => t.FranchiseTypeId).HasColumnName("FranchiseTypeId");
            builder.Property(t => t.BookletEndValidDate).HasColumnName("BookletEndValidDate");
            builder.Property(t => t.BookletIssuingCityId).HasColumnName("BookletIssuingCityId");
            builder.Property(t => t.GenderId).HasColumnName("GenderId");
            builder.Property(t => t.FatherName).HasColumnName("FatherName");
            builder.Property(t => t.SequenceNumber).HasColumnName("SequenceNumber");
            builder.Property(t => t.GenderTitle).HasColumnName("GenderTitle");
            builder.Property(t => t.FranchTitle).HasColumnName("FranchTitle");
            builder.Property(t => t.JobPositionCode).HasColumnName("JobPositionCode");
            builder.Property(t => t.JobPositionTitle).HasColumnName("JobPositionTitle");
            builder.Property(t => t.NationalCodeParent).HasColumnName("NationalCodeParent");
            builder.Property(t => t.FirstNameParent).HasColumnName("FirstNameParent");
            builder.Property(t => t.LastNameParent).HasColumnName("LastNameParent");
        }
    }
}

