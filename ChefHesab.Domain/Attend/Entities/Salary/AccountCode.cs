using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Domain.Entities.Rule;
using KSC.Domain;
namespace Ksc.HR.Domain.Entities.Salary
{
    public class AccountCode : IEntityBase<int>
    {

        public int Id { get; set; } // Id (Primary key)
        public string Title { get; set; } // Title (length: 500)
        public int? InterdictCategoryId { get; set; } // InterdictCategoryId
        public int? AccountCodeCategoryId { get; set; } // AccountCodeCategoryId
        public string UrlOtherPayment { get; set; } // UrlOtherPayment (length: 500)

        /// <summary>
        /// فعال بودن؟
        /// </summary>
        public bool IsActive { get; set; } // IsActive

        /// <summary>
        /// امکان حذف اطلاعات  پرداخت  خارج از فیش پرسنل وجود دارد؟
        /// </summary>
        public bool IsValidDeleteInOtherPaymnet { get; set; } // IsValidDeleteInOtherPaymnet

        /// <summary>
        /// امکان پرداخت مجدد در یک بازه پرداخت خارج از فیش وجود دارد؟
        /// </summary>
        public bool IsValidRepeatableInOtherPaymnetPeriod { get; set; } // IsValidRepeatableInOtherPaymnetPeriod

        /// <summary>
        /// آیا پرداختی است؟
        /// </summary>
        public bool IsAddational { get; set; } // IsAddational

        /// <summary>
        /// کد شرح ذینفع دارد؟
        /// </summary>
        public bool IsBeneficaryCode { get; set; } // IsBeneficaryCode

        /// <summary>
        /// آیا بصورت اتوماتیک ایجاد میشود؟
        /// </summary>
        public bool IsAutomatic { get; set; } // IsAutomatic

        /// <summary>
        /// فقط یکبار استفاده میشود؟
        /// </summary>
        public bool IsOnceUsed { get; set; } // IsOnceUsed

        /// <summary>
        /// قابل استفاده در فیش چاپ شده؟
        /// </summary>
        public bool IsValidInPrintedFish { get; set; } // IsValidInPrintedFish

        /// <summary>
        /// بدهی به شرکت؟
        /// </summary>
        public bool IsDebetToCompany { get; set; } // IsDebetToCompany
        public bool IsInvalidUsedInReport { get; set; } // IsInvalidUsedInReport
        public bool IsValidUseInterdictMaritalSetting { get; set; } // IsValidUseInterdictMaritalSetting
        public bool IsAllMartialSataus { get; set; } // IsAllMartialSataus
        public string InsertUser { get; set; } // InsertUser (length: 50)
        public DateTime? InsertDate { get; set; } // InsertDate
        public string UpdateUser { get; set; } // UpdateUser (length: 50)
        public DateTime? UpdateDate { get; set; } // UpdateDate

        public virtual InterdictCategory InterdictCategory { get; set; } // FK_AccountCode_InterdictCategory
        public virtual ICollection<EmployeeInterdictDetail> EmployeeInterdictDetails { get; set; }
        public virtual ICollection<OtherPaymentSetting> OtherPaymentSettings { get; set; }
        public virtual ICollection<OtherPaymentHeader> OtherPaymentHeaders { get; set; }
        public virtual ICollection<AccountEmploymentType> AccountEmploymentTypes { get; set; }
        public virtual ICollection<BasisSalaryItemPerGroup> BasisSalaryItemPerGroups { get; set; } // BasisSalaryItemPerGroup.FK_BasisSalaryItemPerGroup_BasisSalaryItem
        public virtual ICollection<InterdictMaritalSettingDetail> InterdictMaritalSettingDetails { get; set; } // InterdictMaritalSettingDetail.FK_InterdictMaritalSettingDetail_AccountCode
        /// <summary>
        /// Child Salary_AccountCodeMaritals where [AccountCodeMarital].[AccountCodeId] point to this entity (FK_AccountCodeMarital_AccountCode)
        /// </summary>
        public virtual ICollection<AccountCodeMarital> Salary_AccountCodeMaritals { get; set; } // AccountCodeMarital.FK_AccountCodeMarital_AccountCode

        /// <summary>
        /// Parent AccountCodeCategory pointed by [AccountCode].([AccountCodeCategoryId]) (FK_AccountCode_AccountCodeCategory)
        /// </summary>
        public virtual AccountCodeCategory AccountCodeCategory { get; set; } // FK_AccountCode_AccountCodeCategory


        /// <summary>
        /// Child PaymentAccountCodes where [PaymentAccountCode].[AccountCodeId] point to this entity (FK_PaymentAccountCode_AccountCode)
        /// </summary>
        public virtual ICollection<PaymentAccountCode> PaymentAccountCodes { get; set; } // PaymentAccountCode.FK_PaymentAccountCode_AccountCode

        /// <summary>
        /// Child PropertyAccountSettings where [PropertyAccountSetting].[AccountCodeId] point to this entity (FK_PropertyAccountSetting_AccountCode)
        /// </summary>
        public virtual ICollection<PropertyAccountSetting> PropertyAccountSettings { get; set; } // PropertyAccountSetting.FK_PropertyAccountSetting_AccountCode

        /// <summary>
        /// Child AccountCodeCompatibles where [AccountCodeCompatible].[AccountCodeId] point to this entity (FK_AccountCodeCompatible_AccountCode)
        /// </summary>
        public virtual ICollection<AccountCodeCompatible> AccountCodeCompatibles_AccountCodeId { get; set; } // AccountCodeCompatible.FK_AccountCodeCompatible_AccountCode

        /// <summary>
        /// Child AccountCodeCompatibles where [AccountCodeCompatible].[AccountCodeCompatibleId] point to this entity (FK_AccountCodeCompatible_AccountCode1)
        /// </summary>
        public virtual ICollection<AccountCodeCompatible> AccountCodeCompatibles_AccountCodeCompatibleId { get; set; } // AccountCodeCompatible.FK_AccountCodeCompatible_AccountCode1

        /// <summary>
        /// Child Pay_PaymentDetails where [PaymentDetail].[AccountCodeId] point to this entity (FK_PaymentDetail_AccountCode)
        /// </summary>
        public virtual ICollection<PaymentDetail> PaymentDetails { get; set; } // PaymentDetail.FK_PaymentDetail_AccountCode
        /// <summary>
        /// Child Pay_PaymentAdditionalHeaders where [PaymentAdditionalHeader].[AccountCodeId] point to this entity (FK_PaymentAdditionalHeader_AccountCode)
        /// </summary>
        public virtual ICollection<PaymentAdditionalHeader> PaymentAdditionalHeaders { get; set; } // PaymentAdditionalHeader.FK_PaymentAdditionalHeader_AccountCode
        /// <summary>
        /// Child Salary_AccountCodeDeductionTypes where [AccountCodeDeductionType].[AccountCodeId] point to this entity (FK_AccountCodeDeductionType_AccountCode)
        /// </summary>
        public virtual ICollection<AccountCodeDeductionType> AccountCodeDeductionTypes { get; set; } // AccountCodeDeductionType.FK_AccountCodeDeductionType_AccountCode
        /// <summary>
        /// Child Salary_AccountCodeBeneficiaries where [AccountCodeBeneficiary].[AccountCodeId] point to this entity (FK_AccountCodeBeneficiary_AccountCode)
        /// </summary>
        public virtual ICollection<AccountCodeBeneficiary> AccountCodeBeneficiaries { get; set; } // AccountCodeBeneficiary.FK_AccountCodeBeneficiary_AccountCode

        /// <summary>
        /// Child Pay_EmployeeLoanDeductionHeaders where [EmployeeLoanDeductionHeader].[InstallmentAccountCodeId] point to this entity (FK_EmployeeLoanDeductionHeader_AccountCode1)
        /// </summary>
        public virtual ICollection<EmployeeLoanDeductionHeader> EmployeeLoanDeductionHeaders_InstallmentAccountCodeId { get; set; } // EmployeeLoanDeductionHeader.FK_EmployeeLoanDeductionHeader_AccountCode1

        /// <summary>
        /// Child Pay_EmployeeLoanDeductionHeaders where [EmployeeLoanDeductionHeader].[PaymentAccountCodeId] point to this entity (FK_EmployeeLoanDeductionHeader_AccountCode)
        /// </summary>
        public virtual ICollection<EmployeeLoanDeductionHeader> EmployeeLoanDeductionHeaders_PaymentAccountCodeId { get; set; } // EmployeeLoanDeductionHeader.FK_EmployeeLoanDeductionHeader_AccountCode
        //public virtual ICollection<EmployeeLoanDeductionHeader> EmployeeDeductionHeaders_AccountCode { get; set; } // EmployeeLoanDeductionHeader.FK_EmployeeLoanDeductionHeader_AccountCode1

        public virtual ICollection<EmployeeDeductionHeader> EmployeeDeductionHeaders { get; set; } // EmployeeLoanDeductionHeader.FK_EmployeeLoanDeductionHeader_AccountCode1
        /// <summary>
        /// Child Pay_BudgetRewardTypes where [BudgetRewardType].[AccountCodeId] point to this entity (FK_BudgetRewardType_AccountCode)
        /// </summary>
        public virtual ICollection<BudgetRewardType> BudgetRewardTypes { get; set; } // BudgetRewardType.FK_BudgetRewardType_AccountCode


        public AccountCode()
        {
            BudgetRewardTypes = new List<BudgetRewardType>();
            OtherPaymentSettings = new List<OtherPaymentSetting>();
            OtherPaymentHeaders = new List<OtherPaymentHeader>();
            AccountEmploymentTypes = new List<AccountEmploymentType>();
            InterdictMaritalSettingDetails = new List<InterdictMaritalSettingDetail>();
            PaymentAccountCodes = new List<PaymentAccountCode>();
            PropertyAccountSettings = new List<PropertyAccountSetting>();
            AccountCodeCompatibles_AccountCodeCompatibleId = new List<AccountCodeCompatible>();
            AccountCodeCompatibles_AccountCodeId = new List<AccountCodeCompatible>();
            Salary_AccountCodeMaritals = new List<AccountCodeMarital>();
            EmployeeInterdictDetails=new List<EmployeeInterdictDetail>();
            PaymentDetails = new List<PaymentDetail>();
            PropertyAccountSettings = new List<PropertyAccountSetting>();
            PaymentAdditionalHeaders = new List<PaymentAdditionalHeader>();
            AccountCodeDeductionTypes = new List<AccountCodeDeductionType>();
            AccountCodeBeneficiaries = new List<AccountCodeBeneficiary>();
            EmployeeLoanDeductionHeaders_InstallmentAccountCodeId = new List<EmployeeLoanDeductionHeader>();
            EmployeeLoanDeductionHeaders_PaymentAccountCodeId = new List<EmployeeLoanDeductionHeader>();
            EmployeeDeductionHeaders = new List<EmployeeDeductionHeader>();


        }
    }
}

