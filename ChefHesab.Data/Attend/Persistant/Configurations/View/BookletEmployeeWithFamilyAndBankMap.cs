using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ksc.Hr.Domain.Entities;
namespace Ksc.Hr.Data.Persistant.Configurations
{
    public class BookletEmployeeWithFamilyAndBankMap : IEntityTypeConfiguration<BookletEmployeeWithFamilyAndBank>
    {
        public void Configure(EntityTypeBuilder<BookletEmployeeWithFamilyAndBank> builder)
        {
            // Table & Column Mappings
            builder.ToView("BookletEmployeeWithFamilyAndBank", "dbo");
            builder.HasNoKey();
            builder.Property(t => t.FK_BANK_EFML).HasColumnName("FK_BANK_EFML");
            builder.Property(t => t.COD_ACN_EFML).HasColumnName("COD_ACN_EFML");
            builder.Property(t => t.FLG_INHERIT_EFML).HasColumnName("FLG_INHERIT_EFML");
            builder.Property(t => t.PCN_INHERIT_EFML).HasColumnName("PCN_INHERIT_EFML");
            builder.Property(t => t.EMPLOYEE_FAMILY_SP).HasColumnName("EMPLOYEE_FAMILY_SP");
            builder.Property(t => t.EmployeeNumber).HasColumnName("EmployeeNumber");
            builder.Property(t => t.EmployeeId).HasColumnName("EmployeeId");
            builder.Property(t => t.BookletNumber).HasColumnName("BookletNumber");
            builder.Property(t => t.PaymentStatusId).HasColumnName("PaymentStatusId");
            builder.Property(t => t.PaymentStatusTitle).HasColumnName("PaymentStatusTitle");
            builder.Property(t => t.EmployeeFamilyId).HasColumnName("EmployeeFamilyId");
            builder.Property(t => t.FirstName).HasColumnName("FirstName");
            builder.Property(t => t.LastName).HasColumnName("LastName");
            builder.Property(t => t.NationalCode).HasColumnName("NationalCode");
            builder.Property(t => t.DependenceTypeId).HasColumnName("DependenceTypeId");
            builder.Property(t => t.DependenceTypeTitle).HasColumnName("DependenceTypeTitle");
            builder.Property(t => t.IsContinuesDependent).HasColumnName("IsContinuesDependent");
            builder.Property(t => t.StartDateDependent).HasColumnName("StartDateDependent");
            builder.Property(t => t.EndDateDependent).HasColumnName("EndDateDependent");
            builder.Property(t => t.BirthDate).HasColumnName("BirthDate");
            builder.Property(t => t.BookletIsActive).HasColumnName("BookletIsActive");
            builder.Property(t => t.FranchiseAmount).HasColumnName("FranchiseAmount");
            builder.Property(t => t.FranchiseTypeId).HasColumnName("FranchiseTypeId");
            builder.Property(t => t.BookletEndValidDate).HasColumnName("BookletEndValidDate");
            builder.Property(t => t.GenderId).HasColumnName("GenderId");
            builder.Property(t => t.FatherName).HasColumnName("FatherName");
            builder.Property(t => t.GenderTitle).HasColumnName("GenderTitle");
            builder.Property(t => t.FranchTitle).HasColumnName("FranchTitle");
            builder.Property(t => t.JobPositionCode).HasColumnName("JobPositionCode");
            builder.Property(t => t.JobPositionTitle).HasColumnName("JobPositionTitle");
            builder.Property(t => t.NationalCodeParent).HasColumnName("NationalCodeParent");
            builder.Property(t => t.FirstNameParent).HasColumnName("FirstNameParent");
            builder.Property(t => t.LastNameParent).HasColumnName("LastNameParent");
            builder.Property(t => t.EmploymentTypeId).HasColumnName("EmploymentTypeId");
            builder.Property(t => t.EmploymentTypeTitle).HasColumnName("EmploymentTypeTitle");
            builder.Property(t => t.BookletIssuingCityId).HasColumnName("BookletIssuingCityId");
            builder.Property(t => t.CityName).HasColumnName("CityName");
            builder.Property(t => t.CostCenter).HasColumnName("CostCenter");
        }
    }
}

