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
using Ksc.HR.Resources.Workshift;
using System.ComponentModel.DataAnnotations;

namespace Ksc.HR.DTO.WorkShift.RollCallDefinication
{
    public class EditRollCallDefinicationModel : FilterRequest
    {
        public EditRollCallDefinicationModel()
        {
            AvilibaleCompatibleRollCall = new List<SearchRollCallDefinicationModel>();
            AvilibaleSearchWorkTimeMode = new List<SearchWorkTimeModel>();
            AvilibaleIncludedRollCalls = new List<SearchIncludedDefinitionModel>();
            AvilibaleRollCallAccessLevels = new List<SearchAccessLevelModel>();
            AvilibaleRollCallJobCategories = new List<SearchViewMisJobCategoryModel>();
            AvilibaleRollCallSalaryCodes = new List<RollCallSalaryCodeModel>();
            AvilibaleRollCallWorkTimeCategories = new List<SearchWorkTimeCategoryModel>();
            AvilibaleRollCallConcept = new List<SearchRollCallConceptModel>();
            AvilibaleRollCallCategory = new List<SearchRollCallCategoryModel>();
            AvilibaleSalaryDeductionIncludedCode = new List<SearchIncludedDefinitionModel>();
            AvilibaleRewardDeductionIncludedCode = new List<SearchIncludedDefinitionModel>();
            AvilibaleSearchWorkDayTypeMode = new List<SearchWorkDayTypeModel>();
            AvilibaleViewMisEmploymentTypeModel = new List<SearchViewMisEmploymentTypeModel>();
            AvilibaleViewMisSalaryCodeModels = new List<SearchViewMisSalaryCodeModel>();
        }
        public int Id { get; set; } // Id (Primary key)

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } // Title (length: 500)

        /// <summary>
        /// کد
        /// </summary>
        public string Code { get; set; } // Code (length: 50)

        /// <summary>
        /// تاریخ شروع اعتبار
        /// </summary>
        public DateTime? ValidityStartDate { get; set; } // ValidityStartDate

        /// <summary>
        /// تاریخ پایان اعتبار
        /// </summary>
        public DateTime? ValidityEndDate { get; set; } // ValidityEndDate

        /// <summary>
        /// حداقل زمان مجاز
        /// </summary>
        public string ValidityMinimumTime { get; set; } // ValidityMinimumTime (length: 6)

        /// <summary>
        /// حداکثر زمان مجاز
        /// </summary>
        public string ValidityMaximumTime { get; set; } // ValidityMaximumTime (length: 6)

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
        public int RollCallConceptId { get; set; } // RollCallConceptId
        public int? RollCallCategoryId { get; set; } // RollCallCategoryId
        public int? TimesAllowedUsePerDay { get; set; } // RollCallCategoryId
        public int? TimesAllowedUsePerWeek { get; set; } // TimesAllowedUsePerWeek
        public int? TimesAllowedUsePerMonth { get; set; } // RollCallCategoryId
        public DateTime? InsertDate { get; set; } // InsertDate
        public string InsertUser { get; set; } // InsertUser (length: 50)
        public DateTime? UpdateDate { get; set; } // UpdateDate
        public string UpdateUser { get; set; } // UpdateUser (length: 50)
        public string DomainName { get; set; } // DomainName (length: 50)
        public bool IsActive { get; set; } // IsActive

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

        // IncludedRollCall.FK_IncludedRollCall_RollCallDefinition
        public List<SearchIncludedDefinitionModel> AvilibaleSalaryDeductionIncludedCode { get; set; }
        public List<SearchIncludedDefinitionModel> AvilibaleRewardDeductionIncludedCode { get; set; }

        public int AvilibaleSalaryDeductionIncludedId { get; set; }
        public int AvilibaleRewardDeductionIncludedId { get; set; }
        public virtual List<SearchRollCallCategoryModel> AvilibaleRollCallCategory { get; set; } // FK_RollCallDefinition_RollCallCategory

        public virtual List<SearchRollCallConceptModel> AvilibaleRollCallConcept { get; set; } // FK_RollCallDefinition_RollCallConcept

        #region روز و زمان
        public List<SearchWorkTimeModel> AvilibaleSearchWorkTimeMode { get; set; }
        public int WorkTimeId { get; set; }

        public List<SearchWorkDayTypeModel> AvilibaleSearchWorkDayTypeMode { get; set; }
        public int WorkDayTypeId { get; set; }

        public List<SearchWorkTimeCategoryModel> AvilibaleRollCallWorkTimeCategories { get; set; } // RollCallWorkTimeCategory.FK_RollCallWorkTimeCategory_RollCallDefinition

        public int AvilibaleRollCallWorkTimeCategoriesId { get; set; }

        #endregion

        #region سطوح دسترسی

        public List<SearchAccessLevelModel> AvilibaleRollCallAccessLevels { get; set; } // RollCallAccessLevel.FK_RollCallAccessLevel_RollCallDefinition
        public int AvilibaleRollCallAccessLevelsId { get; set; }

        public List<SearchViewMisJobCategoryModel> AvilibaleRollCallJobCategories { get; set; } // RollCallJobCategory.FK_RollCallJobCategory_RollCallDefinition
        public int AvilibaleRollCallJobCategoriesId { get; set; }
        #endregion

        #region موارد مشمول
        public List<SearchIncludedDefinitionModel> AvilibaleIncludedRollCalls { get; set; }
        public int AvilibaleIncludedRollCallsId { get; set; }
        #endregion

        #region کدهای سازگار
        public List<SearchRollCallDefinicationModel> AvilibaleCompatibleRollCall { get; set; } // CompatibleRollCall.FK_CompatibleRollCall_RollCallDefinition1
        public int AvilibaleCompatibleRollCallId { get; set; }
        #endregion

        #region محاسبه حقوق
        public ICollection<SearchViewMisSalaryCodeModel> AvilibaleViewMisSalaryCodeModels { get; set; } // RollCallSalaryCode.FK_RollCallSalaryCode_RollCallDefinition
        public int ViewMisSalaryCodesId { get; set; }

        public ICollection<SearchViewMisEmploymentTypeModel> AvilibaleViewMisEmploymentTypeModel { get; set; } // RollCallSalaryCode.FK_RollCallSalaryCode_RollCallDefinition
        public int ViewMisEmploymentTypeId { get; set; }

        public ICollection<RollCallSalaryCodeModel> AvilibaleRollCallSalaryCodes { get; set; } // RollCallSalaryCode.FK_RollCallSalaryCode_RollCallDefinition
        public int AvilibaleRollCallSalaryCodesId { get; set; }
        #endregion
    }
}
