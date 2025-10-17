using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.DTO.WorkShift.RollCallConcept;
using Ksc.HR.DTO.WorkShift.RollCallCategory;
using Ksc.HR.DTO.WorkShift.IncludedDefinition;
using Ksc.HR.DTO.WorkShift.AccessLevel;
using Ksc.HR.DTO.WorkShift.WorkTimeCategory;
using Ksc.HR.DTO.WorkShift.WorkDayType;
using Ksc.HR.DTO.ODSViews.ViewMisJobCategory;
using Ksc.HR.DTO.ODSViews.ViewMisSalaryCode;
using KSC.Common.Filters.Models;
using Ksc.HR.DTO.WorkShift.RollCallSalaryCode;
using Ksc.HR.DTO.ODSViews.ViewMisEmploymentType;
using System.ComponentModel;
using Ksc.HR.DTO.WorkShift.WorkTime;
using Ksc.HR.DTO.WorkShift.RollCallWorkTimeDayType;
using Ksc.HR.Resources.Workshift;
using System.Security.AccessControl;
using System.ComponentModel.DataAnnotations;
using Ksc.HR.DTO.Salary.AccountCode;
using Ksc.HR.DTO.EmployeeBase;
using Ksc.HR.DTO.WorkShift.City;
using Ksc.HR.DTO.WorkShift.RollCallWorkCity;

namespace Ksc.HR.DTO.WorkShift.RollCallDefinication
{
    public class AddRollCallDefinicationModel : FilterRequest
    {
        public AddRollCallDefinicationModel()
        {
            AvilibaleIncludedRollCallDefinication = new List<SearchRollCallDefinicationModel>();
            AvilibaleSearchWorkTimeMode = new List<SearchWorkTimeModel>();
            AvilibaleIncludedRollCalls = new List<SearchIncludedDefinitionModel>();
            AvilibaleRollCallAccessLevels = new List<SearchAccessLevelModel>();
            AvilibaleRollCallJobCategories = new List<SearchViewMisJobCategoryModel>();
            RollCallSalariesCode = new List<RollCallSalaryCodeModel>();
            RollCallWorkTimeDayTypeModels = new List<RollCallWorkTimeDayTypeModel>();
            AvilibaleRollCallConcept = new List<SearchRollCallConceptModel>();
            AvilibaleRollCallCategory = new List<SearchRollCallCategoryModel>();
            AvilibaleSalaryDeductionIncludedCode = new List<SearchIncludedDefinitionModel>();
            AvilibaleRewardDeductionIncludedCode = new List<SearchIncludedDefinitionModel>();
            AvilibaleSearchWorkDayTypeMode = new List<SearchWorkDayTypeModel>();
            AvilibaleViewMisEmploymentTypeModel = new List<SearchViewMisEmploymentTypeModel>();
            AvilibaleAccountCodeModels = new List<SearchAccountCodeDto>();
            AvilibaleWorkCityModel = new List<SearchCityModel>();
            RollCallWorkCityModels = new List<RollCallWorkCityModel>();
        }
        public int Id { get; set; } // Id (Primary key)

        /// <summary>
        /// عنوان
        /// </summary>
        [Required(ErrorMessageResourceName = "RequiredTitleAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Workshift.RollCallDefinitionResource))]
        [Display(Name = nameof(Title), ResourceType = typeof(RollCallDefinitionResource))]
        public string Title { get; set; } // Title (length: 500)

        /// <summary>
        /// کد
        /// </summary>
        //[Required(ErrorMessageResourceName = "RequiredCodeAttributeErrorMessage", ErrorMessageResourceType = typeof(RollCallDefinitionResource))]
        [Display(Name = nameof(Code), ResourceType = typeof(RollCallDefinitionResource))]

        public string Code { get; set; } // Code (length: 50)

        /// <summary>
        /// تاریخ شروع اعتبار
        /// </summary>
        [Required(ErrorMessageResourceName = "RequiredValidityStartDateAttributeErrorMessage", ErrorMessageResourceType = typeof(RollCallDefinitionResource))]
        [Display(Name = nameof(ValidityStartDate), ResourceType = typeof(RollCallDefinitionResource))]

        public DateTime? ValidityStartDate { get; set; } // ValidityStartDate

        /// <summary>
        /// تاریخ پایان اعتبار
        /// </summary>
        [Required(ErrorMessageResourceName = "RequiredValidityEndDateAttributeErrorMessage", ErrorMessageResourceType = typeof(RollCallDefinitionResource))]
        [Display(Name = nameof(ValidityEndDate), ResourceType = typeof(RollCallDefinitionResource))]

        public DateTime? ValidityEndDate { get; set; } // ValidityEndDate

        /// <summary>
        /// حداقل زمان مجاز
        /// </summary>
        [Required(ErrorMessageResourceName = "RequiredValidityMinimumTimeAttributeErrorMessage", ErrorMessageResourceType = typeof(RollCallDefinitionResource))]
        [Display(Name = nameof(ValidityMinimumTime), ResourceType = typeof(RollCallDefinitionResource))]

        public string ValidityMinimumTime { get; set; } // ValidityMinimumTime (length: 5)

        /// <summary>
        /// حداکثر زمان مجاز
        /// </summary>
        [Required(ErrorMessageResourceName = "RequiredValidityMaximumTimeAttributeErrorMessage", ErrorMessageResourceType = typeof(RollCallDefinitionResource))]
        [Display(Name = nameof(ValidityMaximumTime), ResourceType = typeof(RollCallDefinitionResource))]

        public string ValidityMaximumTime { get; set; } // ValidityMaximumTime (length: 5)

        /// <summary>
        /// حداقل زمان مجاز به دقیقه
        /// </summary>
        public int? ValidityMinimumTimeMinute { get; set; } // ValidityMinimumTimeMinute

        /// <summary>
        /// حداکثر زمان مجاز به دقیقه
        /// </summary>
        public int? ValidityMaximumTimeMinute { get; set; } // ValidityMaximumTimeMinute

        /// <summary>
        /// قابل پذیرش در شروع شیفت
        /// </summary>
        public bool IsValidInShiftStart { get; set; } // IsValidInShiftStart

        /// <summary>
        /// قابل پذیرش در پایان شیفت
        /// </summary>
        public bool IsValidInShiftEnd { get; set; } // IsValidInShiftEnd
        /// <summary>
        /// نوع کد
        /// </summary>
        [Required(ErrorMessageResourceName = "RequiredRollCallConceptIdAttributeErrorMessage", ErrorMessageResourceType = typeof(RollCallDefinitionResource))]
        [Display(Name = nameof(RollCallConceptId), ResourceType = typeof(RollCallDefinitionResource))]
        public int RollCallConceptId { get; set; } // RollCallConceptId
        /// <summary>
        /// دسته بندی کد
        /// </summary>
        //[Required(ErrorMessageResourceName = "RequiredRollCallCategoryIdAttributeErrorMessage", ErrorMessageResourceType = typeof(RollCallDefinitionResource))]
        [Display(Name = nameof(RollCallCategoryId), ResourceType = typeof(RollCallDefinitionResource))]

        public int? RollCallCategoryId { get; set; } // RollCallCategoryId
        /// <summary>
        /// دفعات مجاز استفاده روزانه
        /// </summary>
        [Required(ErrorMessageResourceName = "RequiredTimesAllowedUsePerDayAttributeErrorMessage", ErrorMessageResourceType = typeof(RollCallDefinitionResource))]
        [Display(Name = nameof(TimesAllowedUsePerDay), ResourceType = typeof(RollCallDefinitionResource))]

        public int? TimesAllowedUsePerDay { get; set; } // RollCallCategoryId
        /// <summary>
        /// دفعات مجاز استفاده هفته
        /// </summary>
        [Display(Name = nameof(TimesAllowedUsePerWeek), ResourceType = typeof(RollCallDefinitionResource))]
        public int? TimesAllowedUsePerWeek { get; set; } // TimesAllowedUsePerWeek
        /// <summary>
        /// دفعات مجاز استفاده ماهانه
        /// </summary>
        [Required(ErrorMessageResourceName = "RequiredTimesAllowedUsePerMonthAttributeErrorMessage", ErrorMessageResourceType = typeof(RollCallDefinitionResource))]
        [Display(Name = nameof(TimesAllowedUsePerMonth), ResourceType = typeof(RollCallDefinitionResource))]
        public int? TimesAllowedUsePerMonth { get; set; } // RollCallCategoryId
        /// <summary>
        /// دفعات مجاز استفاده سالانه
        /// </summary>
        //[Required(ErrorMessageResourceName = "RequiredTimesValidityDayNumberInYearAttributeErrorMessage", ErrorMessageResourceType = typeof(RollCallDefinitionResource))]
        //[Display(Name = nameof(ValidityDayNumberInYear), ResourceType = typeof(RollCallDefinitionResource))]

        public int? ValidityDayNumberInYear { get; set; } // RollCallCategoryId
        /// <summary>
        /// ترتیب کسر از مازاد اضافه کاری
        /// </summary>
        public int? OverTimePriority { get; set; }

        public DateTime? InsertDate { get; set; } // InsertDate
        public string InsertUser { get; set; } // InsertUser (length: 50)
        public DateTime? UpdateDate { get; set; } // UpdateDate
        public string UpdateUser { get; set; } // UpdateUser (length: 50)
        public string DomainName { get; set; } // DomainName (length: 50)
        public bool IsActive { get; set; } // IsActive
        /// <summary>
        /// آیا حذف تکی در تایید کارکرد می شود؟
        /// </summary>
        public bool IsValidSingleDelete { get; set; }

        /// <summary>
        /// کد برای تمام نوع استخدام معتبر است؟
        /// </summary>
        public bool IsValidForAllEmploymentType { get; set; } // IsValidForAllEmploymentType

        /// <summary>
        /// ثبت کد توسط تمام رده های شغلی مجاز است؟
        /// </summary>
        public bool IsValidForAllCategoryCode { get; set; } // IsValidForAllCategoryCode
        /// <summary>
        /// برای تمام روزهای کاری در تمام زمان کاری قابل قبول است؟
        /// </summary>
        public bool IsValidForAllWorkTimeDayType { get; set; } // IsValidForAllWorkTimeDayType
        /// <summary>
        /// کد به صورت اتوماتیک در کارکرد ثبت میشود؟
        /// </summary>
        public bool InsertCodeIsAutomatic { get; set; } // InsertCodeIsAutomatic
        /// <summary>
        /// برای زمان موقت در ابتدای شیفت معتبر است؟
        /// </summary>
        public bool IsValidForTemporaryStartDate { get; set; } // IsValidForTemporaryStartDate

        /// <summary>
        /// برای زمان موقت در انتهای شیفت معتبر است؟
        /// </summary>
        public bool IsValidForTemporaryEndDate { get; set; } // IsValidForTemporaryEndDate

        // IncludedRollCall.FK_IncludedRollCall_RollCallDefinition
        public List<SearchIncludedDefinitionModel> AvilibaleSalaryDeductionIncludedCode { get; set; }
        public List<SearchIncludedDefinitionModel> AvilibaleRewardDeductionIncludedCode { get; set; }

        /// <summary>
        /// فهرست دسته بندی کد
        /// </summary>
        public virtual List<SearchRollCallCategoryModel> AvilibaleRollCallCategory { get; set; } // FK_RollCallDefinition_RollCallCategory
        /// <summary>
        /// فهرست نوع کد
        /// </summary>

        public virtual List<SearchRollCallConceptModel> AvilibaleRollCallConcept { get; set; } // FK_RollCallDefinition_RollCallConcept

        #region روز و زمان
        public List<SearchWorkTimeModel> AvilibaleSearchWorkTimeMode { get; set; }
        public int WorkTimeId { get; set; }

        public List<SearchWorkDayTypeModel> AvilibaleSearchWorkDayTypeMode { get; set; }
        /// <summary>
        ///فهرست واسط نوع روز کاری و کدهای حضور غیاب 
        /// </summary>
        public List<RollCallWorkTimeDayTypeModel> RollCallWorkTimeDayTypeModels { set; get; }
        //public int WorkDayTypeId { get; set; }

        //public List<SearchWorkTimeCategoryModel> AvilibaleRollCallWorkTimeCategories { get; set; } // RollCallWorkTimeCategory.FK_RollCallWorkTimeCategory_RollCallDefinition

        //public int AvilibaleRollCallWorkTimeCategoriesId { get; set; }

        #endregion

        #region سطوح دسترسی

        public List<SearchAccessLevelModel> AvilibaleRollCallAccessLevels { get; set; } // RollCallAccessLevel.FK_RollCallAccessLevel_RollCallDefinition
        /// <summary>
        /// سطح دسترسی به کد
        /// </summary>
        [Required(ErrorMessageResourceName = "RequiredAccessLevelIdAttributeErrorMessage", ErrorMessageResourceType = typeof(RollCallDefinitionResource))]
        [Display(Name = nameof(AccessLevelId), ResourceType = typeof(RollCallDefinitionResource))]

        public int? AccessLevelId { get; set; }

        public List<SearchViewMisJobCategoryModel> AvilibaleRollCallJobCategories { get; set; } // RollCallJobCategory.FK_RollCallJobCategory_RollCallDefinition
        /// <summary>
        /// رده های شغلی مجاز استفاده از کد
        /// </summary>
        public string[] RollCallJobCategoriesId { get; set; } = Array.Empty<string>();
        /// <summary>
        /// نوع استخدام دسترسی به کد
        /// </summary>
        public string[] EmploymentTypeCodesId { get; set; } = Array.Empty<string>();

        public ICollection<SearchViewMisEmploymentTypeModel> AvilibaleViewMisEmploymentTypeCodesModel { get; set; } // RollCallSalaryCode.FK_RollCallSalaryCode_RollCallDefinition
        public List<SearchCityModel> AvilibaleWorkCityModel { get; set; }
        public int? GenderTypeId { get; set; }
        public virtual List<SearchGenderDto> AvilibaleGenderType { get; set; }
        /// <summary>
        ///فهرست واسط شهرمحل کار و کدهای حضور غیاب 
        /// </summary>
        public List<RollCallWorkCityModel> RollCallWorkCityModels { set; get; }
        #endregion

        #region موارد مشمول
        public List<SearchIncludedDefinitionModel> AvilibaleIncludedRollCalls { get; set; }
        /// <summary>
        /// موارد مشمول این کد
        /// </summary>
        public string[] IncludedRollCallsId { get; set; } = Array.Empty<string>();
        /// <summary>
        /// مشمول کسر حقوق
        /// </summary>

        public int SalaryDeductionIncludedId { get; set; }
        /// <summary>
        /// مشمول کسر پاداش
        /// </summary>
        public int RewardDeductionIncludedId { get; set; }
        #endregion

        #region کدهای سازگار
        /// <summary>
        ///فهرست کد های حضور و غیاب سازگار 
        /// </summary>
        public List<SearchRollCallDefinicationModel> AvilibaleIncludedRollCallDefinication { get; set; } // CompatibleRollCall.FK_CompatibleRollCall_RollCallDefinition1
        /// <summary>
        /// کد های حضور و غیاب سازگار
        /// </summary>
        public string[] CompatibleRollCallsId { get; set; } = Array.Empty<string>();
        /// <summary>
        /// کد های قابل تبدیل
        /// </summary>
        public string[] InterchangeableRollCallsId { get; set; }

        #endregion
        /// <summary>
        ///  برای کدهای عدم حضور روزانه در تایید کارکرد قابل مشاهده است ؟
        /// </summary>
        public bool IsValidForDailyAbcenseInAnalyz { get; set; } // IsValidForDailyAbcenseInAnalyz
        /// <summary>
        ///امکان حذف کلی کارکرد روزانه را دارد؟
        /// </summary>
        public bool IsValidForDeleteAbsenceItem { get; set; } // IsValidForDeleteAbsenceItem
        #region محاسبه حقوق
        public List<SearchAccountCodeDto> AvilibaleAccountCodeModels { get; set; } // RollCallSalaryCode.FK_RollCallSalaryCode_RollCallDefinition
        public int ViewMisSalaryCodesId { get; set; }
        public List<SearchViewMisEmploymentTypeModel> AvilibaleViewMisEmploymentTypeModel { get; set; }

        /// <summary>
        /// جدول واسط نوع حقوق - نوع استخدام
        /// </summary>
        public ICollection<RollCallSalaryCodeModel> RollCallSalariesCode { get; set; } // RollCallSalaryCode.FK_RollCallSalaryCode_RollCallDefinition
        #endregion
    }
}
