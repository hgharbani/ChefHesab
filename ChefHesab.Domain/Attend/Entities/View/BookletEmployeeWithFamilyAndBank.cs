using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSC.Domain;
namespace Ksc.Hr.Domain.Entities
{
    public class BookletEmployeeWithFamilyAndBank :IEntityBase
    {
        public int? EmployeeId { get; set; }
        public string FK_BANK_EFML { get; set; }
        public string COD_ACN_EFML { get; set; }
        public decimal? FLG_INHERIT_EFML { get; set; }
        public decimal? PCN_INHERIT_EFML { get; set; }
        public string EMPLOYEE_FAMILY_SP { get; set; }
        public decimal? EmployeeNumber { get; set; }
        public string BookletNumber { get; set; }
        public int? PaymentStatusId { get; set; }
        public string PaymentStatusTitle { get; set; }
        public int? EmployeeFamilyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public int? DependenceTypeId { get; set; }
        public string DependenceTypeTitle { get; set; }
        public bool? IsContinuesDependent { get; set; }
        public DateTime? StartDateDependent { get; set; }
        public DateTime? EndDateDependent { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool? BookletIsActive { get; set; }
        public double? FranchiseAmount { get; set; }
        public int? FranchiseTypeId { get; set; }
        public DateTime? BookletEndValidDate { get; set; }
        public decimal? GenderId { get; set; }
        public string FatherName { get; set; }
        public string GenderTitle { get; set; }
        public string FranchTitle { get; set; }
        public string JobPositionCode { get; set; }
        public string JobPositionTitle { get; set; }
        public string NationalCodeParent { get; set; }
        public string FirstNameParent { get; set; }
        public string LastNameParent { get; set; }
        public int? EmploymentTypeId { get; set; }
        public string EmploymentTypeTitle { get; set; }
        public decimal? BookletIssuingCityId { get; set; }
        public string CityName { get; set; }
        public decimal? CostCenter { get; set; }
    }
}

