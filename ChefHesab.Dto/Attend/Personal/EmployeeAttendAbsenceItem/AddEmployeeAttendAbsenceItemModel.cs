using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem
{
    public class AddEmployeeAttendAbsenceItemModel
    {
        public int? EvaluationDevelopmentTypeId { get; set; } // TrainingTypeId
        public int? DismissalStatusId { get; set; }
        public string evaluationDevelopmentId { get; set; } //کد شناسایی ارزیابی توسعه

        public bool OverTimeIsOverMax { get; set; }
        public int? PaymentStatusId { get; set; }
        public long Id { get; set; } // Id (Primary key)
        /// <summary>
        /// شماره پرسنلی
        /// </summary>
        public string EmployeeNumber { get; set; }

        /// <summary>
        /// شناسه پرسنلی
        /// </summary>
        public int EmployeeId { get; set; } // EmployeeId

        public bool IsBossAfairs { get; set; }
        /// <summary>
        /// تاریخ روز
        /// </summary>
        public int WorkCalendarId { get; set; } // WorkCalendarId

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// ساعت شروع
        /// </summary>
        public string StartTime { get; set; } // StartTime (length: 5)

        /// <summary>
        /// ساعت پایان
        /// </summary>
        public string EndTime { get; set; } // EndTime (length: 5)

        /// <summary>
        /// مدت زمان
        /// </summary>
        public string TimeDuration { get; set; } // TimeDuration (length: 6)
        public bool IsDeleted { get; set; }
        public int? EmployeeLongTermAbsenceId { get; set; } // EmployeeLongTermAbsenceId

        /// <summary>
        /// کد حضور غیاب
        /// </summary>
        public int RollCallDefinitionId { get; set; } // RollCallDefinitionId

        public int RollCallConceptId { get; set; }
        /// <summary>
        /// کد شیفت کاری
        /// </summary>
        public int ShiftConceptDetailId { get; set; } // ShiftConceptDetailId
        public int ShiftConceptDetailIdInShiftBoard { get; set; } //ShiftConceptDetailIdInShiftBoard
        public int OldShiftConceptDetailId { get; set; } // ShiftConceptDetailId

        /// <summary>
        /// زمان کاری
        /// </summary>
        public int WorkTimeId { get; set; } // WorkTimeId

        /// <summary>
        /// نشانگر کارکرد دستی
        /// </summary>
        public bool IsManual { get; set; } // IsManual

        /// <summary>
        /// نشانگر کارکرد شناور
        /// </summary>
        public bool IsFloat { get; set; } // IsFloat

        /// <summary>
        /// رکورد که نباید مشاهده شود
        /// </summary>
        public bool InvalidRecord { get; set; } // InvalidRecord

        /// <summary>
        /// علت عدم مشاهده رکورد
        /// </summary>
        public string InvalidRecordReason { get; set; } // InvalidRecordReason
        public DateTime? InsertDate { get; set; } // InsertDate
        public string InsertUser { get; set; } // InsertUser (length: 50)
        public DateTime? UpdateDate { get; set; } // UpdateDate
        public string UpdateUser { get; set; } // UpdateUser (length: 50)
        public string CurrentUserName { get; set; }
        public string TeamWorkCode { get; set; }
        public long EntryId { get; set; }
        public long ExitId { get; set; }
        public string OverTimeToken { get; set; }
        public int ForcedOverTime { get; set; }
        public int TotalWorkHourInWeek { get; set; }
        public int YearMonth { get; set; }
        public bool ShiftSettingFromShiftboard { get; set; }

        public int? WorkGroupId { get; set; }
        public int? WorkCityId { get; set; }

        public string COD_TYP { get; set; }
        public string NUMP { get; set; }
        public string STIME { get; set; }
        public string ETIME { get; set; }
        public string FLG_ERR { get; set; }
        public string FLG_MIS { get; set; }
        public string COD_ERR { get; set; }
        public string DES_ERR { get; set; }
        public string RemoteIpAddress { get; set; }
        public string AuthenticateUserName { get; set; }
        public bool InvalidForcedOvertime { get; set; }
        public double? AttendTimeInTemprorayTime { get; set; }
    }
}
