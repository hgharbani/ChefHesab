using KSC.Domain;
namespace Ksc.Hr.Domain.Entities
{
    public class ViewEmployeeFamilyReport : IEntityBase
    {
        public string EmployeeNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public int? ParentCertificateCityId { get; set; }
        public string CertificateCityTitle { get; set; }
        public DateTime CertificateDate { get; set; }
        public string ShamsiCertificateDate { get; set; }
        public DateTime BirthDate { get; set; }
        public string ShamsiBirthDate { get; set; }
        public string DependenceTypeTitle { get; set; }
        public string ShamsiStartDateDependent { get; set; }
        public string ShamsiEndDateDependent { get; set; }
        public string ParentFirstName { get; set; }
        public string ParentFamily { get; set; }
        public string parentNationalCode { get; set; }
        public string MaritalStatusTitle { get; set; }
        public DateTime? ParentBirthDate { get; set; }
        public string ParentShamsiBirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? InsertDate { get; set; }
        public string ShamsiInsertDate { get; set; }
        public string InsertUser { get; set; }
        public string UpdateUser { get; set; }
        public string ShamsiUpdateDate { get; set; }
        public int? EmploymentTypeId { get; set; }
        public string EmploymentTypetitle { get; set; }
        public bool? BookletIsActive { get; set; }
        public int? DependenceReasonId { get; set; }
        public string DependenceReasonTitle { get; set; }
        public DateTime? EmploymentDate { get; set; }
        public string ShamsiEmploymentDate { get; set; }
        public int? ParentGenderId { get; set; }
        public int GenderId { get; set; }
        public bool IsContinuesDependent { get; set; }
        public int DependenceTypeId { get; set; }
        public int BirthCityId { get; set; }
        public string CertificateNumber { get; set; }
        public int CertificateCityId { get; set; }
        public int NationalityId { get; set; }
        public int? DependenceJobId { get; set; }
        public int? DependentExitDateReasonId { get; set; }
        public DateTime StartDateDependent { get; set; }
        public DateTime? EndDateDependent { get; set; }
        public bool IsCompleteInsurance { get; set; }
        public DateTime? ValidDateInsurance { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsActive { get; set; }
        public int? WorkCityId { get; set; }
        public int? PaymentStatusId { get; set; }
        public int? ParentBirthCityId { get; set; }
        public string ParentCertificateNumber { get; set; }
        public DateTime? ParentCertificateDate { get; set; }
        public DateTime? MarriedDate { get; set; }
        public int? JobPositionId { get; set; }
        public DateTime? JobPositionStartDate { get; set; }
        public string GenderTitle { get; set; }
        public string ParentGenderTitle { get; set; }
        public string PaymentStatusTitle { get; set; }
        public int EmployeeId { get; set; }
        public string FamilyBookletNumber { get; set; }
        public string FamilyStartSerialNumber { get; set; }
        public string FamilyExitSerialNumber { get; set; }
        public int? FamilyBookletIssuingCityId { get; set; }
        public double? FamilyFranchiseAmount { get; set; }
        public int? FamilyFranchiseTypeId { get; set; }
        public bool? FamilyBookletIsActive { get; set; }
        public DateTime? FamilyBookletEndValidDate { get; set; }
        public string ParentBookletNumber { get; set; }
        public string ParentStartSerialNumber { get; set; }
        public string ParentExitSerialNumber { get; set; }
        public int? ParentBookletIssuingCityId { get; set; }
        public double? ParentFranchiseAmount { get; set; }
        public int? ParentFranchiseTypeId { get; set; }
        public bool? ParentBookletIsActive { get; set; }
        public DateTime? ParentBookletEndValidDate { get; set; }
        public int? PersonalTypeId { get; set; }
    }
}

