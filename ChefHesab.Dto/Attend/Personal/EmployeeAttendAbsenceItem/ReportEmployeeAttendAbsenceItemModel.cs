using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem
{
    public class ReportEmployeeAttendAbsenceItemModel
    {
        public int Index { get; set; }
        public string FullName { get; set; }
        public string TeamCode { get; set; }
        public string TeamName { get; set; }
        public string PersonnelNumber { get; set; }
        public string InsertUserName { get; set; } // InsertUser (length: 50)
        public string InsertUser { get; set; } // InsertUser (length: 50)
        public DateTime? InsertDate { get; set; } // InsertDate
        public string ShiftConceptDetailCode { get; set; }
        public DateTime WorkCalendarDate { get; set; }
        public int WorkTimeId { get; set; }
        public string WorkTimeTitle { get; set; }
        public int ShiftConceptDetailId { get; set; }
        public string ShiftConceptDetailtitle { get; set; }
        public string ShamsiDate { get; set; }
        public string WeekDay { get; set; }
        public string RollCallDefinitionCode { get; set; }
        public string RollCallDefinitionTitle { get; set; }
        public int? TimeDurationInMinute { get; set; }
        public bool InvalidRecord { get; set; }
        public string SumTimeDuration { get; set; }

        //public long Id { get; set; } // Id (Primary key)

        /// <summary>
        /// شماره پرسنلی
        /// </summary>
        public int EmployeeId { get; set; } // EmployeeId

        /// <summary>
        /// تاریخ روز
        /// </summary>
        public int WorkCalendarId { get; set; } // WorkCalendarId

        //public DateTime? StartDate { get; set; }
        //public DateTime? EndDate { get; set; }
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

        /// <summary>
        /// نشانگر کارکرد دستی
        /// </summary>
        public bool IsManual { get; set; } // IsManual
        public TimeSpan StartTimeToTime { get; set; }
        public TimeSpan EndTimeToTime { get; set; }
        public string InsertDateShamsi { get; set; }



        //===================ورود و خروج================================

        public string PersonalNumber { get; set; } // PersonalNumber (length: 10)
        public DateTime EntryExitDate { get; set; } // EntryExitDate
        public string EntryExitTime { get; set; } // EntryExitTime (length: 5)
        public int EntryExitType { get; set; } // EntryExitType
        public string MachinName { get; set; } // MachinName (length: 2)

        public bool IsCreatedManual { get; set; } // IsCreatedManual
        public string IsCreatedManualNum { get; set; } // IsCreatedManual
        public string IsDeletedNum { get; set; } // IsCreatedManual
        public string IsCreatedManualTitle { get; set; } // IsCreatedManual
        public DateTime? CreateDateTime { get; set; } // CreateDateTime
        public string CreateUser { get; set; } // CreateUser (length: 50)
        public DateTime? UpdateDate { get; set; } // UpdateDate
        public string UpdateUser { get; set; } // UpdateUser (length: 50)
        public DateTime? DeletedDate { get; set; } // DeletedDate
        public string DeletedUser { get; set; } // DeletedUser (length: 50)
        public bool IsDeleted { get; set; } // IsDeleted
        public string IsDeletedTitle { get; set; } // IsDeleted
        public DateTime? EntryDate { get; set; }
        public string EntryDateString { get; set; }
        public DateTime? ExitDate { get; set; } // EntryExitDate
        public string ExitDateString { get; set; } // EntryExitDate
        public string EntryTime { get; set; } // EntryExitTime (length: 5)
        public string ExitTime { get; set; } // EntryExitTime (length: 5)
        public bool Selected { get; set; }
        public string EntryExitTypeString { get; set; }
    }
}
