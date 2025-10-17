using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.DTO.WorkShift.RollCallConcept;
using Ksc.HR.DTO.WorkShift.RollCallCategory;
namespace Ksc.HR.DTO.WorkShift.RollCallDefinication
{
    public class RollCallDefinicationModel
    {
        public RollCallDefinicationModel()
        {
            //AvilibaleCompatibleRollCall = new List<CompatibleRollCall>();

            //AvilibaleIncludedRollCalls = new List<IncludedRollCall>();
            //AvilibaleRollCallJobCategories = new List<RollCallJobCategory>();
            AvilibaleRollCallConcept = new List<SearchRollCallConceptModel>();
            AvilibaleRollCallCategory = new List<SearchRollCallCategoryModel>();
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

        //public List<CompatibleRollCall> AvilibaleCompatibleRollCall { get; set; } // CompatibleRollCall.FK_CompatibleRollCall_RollCallDefinition1
        public int AvilibaleCompatibleRollCallId { get; set; }


        //public List<IncludedRollCall> AvilibaleIncludedRollCalls { get; set; } // IncludedRollCall.FK_IncludedRollCall_RollCallDefinition

        public int AvilibaleIncludedRollCallsId { get; set; }
        /// <summary>
        /// Child RollCallAccessLevels where [RollCallAccessLevel].[RollCallDefinitionId] point to this entity (FK_RollCallAccessLevel_RollCallDefinition)
        /// </summary>
        public int AvilibaleRollCallAccessLevelsId { get; set; }

        //public List<RollCallJobCategory> AvilibaleRollCallJobCategories { get; set; } // RollCallJobCategory.FK_RollCallJobCategory_RollCallDefinition
        public int AvilibaleRollCallJobCategoriesId { get; set; }

        public int AvilibaleRollCallSalaryCodesId { get; set; }

        public int AvilibaleRollCallWorkTimeCategoriesId { get; set; }

        public int AvilibaleRollCallWorkDayTypesId { get; set; }
        public string RollCallConceptTitle { get; set; }
        public string RollCallCategoryTitle { get; set; }
        public virtual List<SearchRollCallCategoryModel> AvilibaleRollCallCategory { get; set; } // FK_RollCallDefinition_RollCallCategory


        public virtual List<SearchRollCallConceptModel> AvilibaleRollCallConcept { get; set; } // FK_RollCallDefinition_RollCallConcept
        public bool IsValidForAllWorkTimeDayType { get; set; }
        public bool IsIncluded { get; set; }
        public bool LongTermAbsenceCheck { get; set; }
        /// <summary>
        ///  اولویت اضافه کاری
        /// </summary>
        public int? OverTimePriority { get; set; } // OverTimePriority
    }
}
