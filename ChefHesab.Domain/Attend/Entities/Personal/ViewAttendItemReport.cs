using KSC.Domain;
namespace Ksc.Hr.Domain.Entities
{
    public class ViewAttendItemReport : IEntityBase<long>
    {
        public long Id { get; set; }
   
        public string Family { get; set; }
        public string Name { get; set; }
        public string EmployeeNumber { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string ShamsiDateV1 { get; set; }
        public DateTime MiladiDateV1 { get; set; }
        public string DefinitionTitle { get; set; }
        public string DefinitionCode { get; set; }

        public int EmployeeId { get; set; }
        public int WorkCalendarId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string TimeDuration { get; set; }
        public int? EmployeeLongTermAbsenceId { get; set; }
        public int RollCallDefinitionId { get; set; }
        public int ShiftConceptDetailId { get; set; }
        public int? ShiftConceptDetailIdInShiftBoard { get; set; }
        public int WorkTimeId { get; set; }
        public bool IsManual { get; set; }
        public bool IsFloat { get; set; }
        public bool InvalidRecord { get; set; }
        public string InvalidRecordReason { get; set; }
        public DateTime? InsertDate { get; set; }
        public string InsertUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public System.Byte[] RowVersion { get; set; }
        public string EvaluationDevelopmentId { get; set; }
        public string MissionId { get; set; }
        public int? EmployeeEducationTimeId { get; set; }
        public int? TimeDurationInMinute { get; set; }
        public string OverTimeToken { get; set; }
        public int? IncreasedTimeDuration { get; set; }
        public string RollCategoryCode { get; set; }
        public int DateKey { get; set; }
        public string RollCallCategoryTitle { get; set; }
    }
}

