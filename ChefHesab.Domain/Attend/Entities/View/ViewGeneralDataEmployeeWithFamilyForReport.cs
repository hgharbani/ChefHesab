using KSC.Domain;
using System.ComponentModel.DataAnnotations;
namespace Ksc.Hr.Domain.Entities
{
    public class ViewGeneralDataEmployeeWithFamilyForReport : IEntityBase
    {

        public DateTime? CertificateDate { get; set; }
        public int? BirthCityId { get; set; }
        public int? DependentExitDateReasonId { get; set; }

        public decimal? EmployeeNumber { get; set; }
        public int? EmploymentTypeId { get; set; }
        public string PhoneNumber { get; set; }
        public int EmployeeId { get; set; }
        public int? PaymentStatusId { get; set; }
        public int? EmployeeFamilyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public int? DependenceTypeId { get; set; }
        public string DependenceTypeTitle { get; set; }
        public bool? IsContinuesDependent { get; set; }
        public bool? IsCompleteInsurance { get; set; }
        public DateTime? StartDateDependent { get; set; }
        public DateTime? EndDateDependent { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool? BookletIsActive { get; set; }
        public string BookletNumber { get; set; }
        public double? FranchiseAmount { get; set; }
        public int? FranchiseTypeId { get; set; }
        public DateTime? BookletEndValidDate { get; set; }
        public decimal? BookletIssuingCityId { get; set; }
        public decimal? GenderId { get; set; }
        public string FatherName { get; set; }
        public int SequenceNumber { get; set; }
        public DateTime? EmploymentDate { get; set; } // EmploymentDate
        public int? MaritalStatusId { get; set; } // MaritalStatusId
        public int? PersonalTypeId { get; set; } // PersonalTypeId
        public string GenderTitle { get; set; }
        public string FranchTitle { get; set; }
        public string JobPositionCode { get; set; }
        public string JobPositionTitle { get; set; }
        public string NationalCodeParent { get; set; }
        public string FirstNameParent { get; set; }
        public string LastNameParent { get; set; }

    }
}

