using Ksc.HR.DTO.Personal.EmployeeEntryExit;
using Ksc.HR.Share.Model.TimeShiftSetting;
using Ksc.HR.Share.Model.WorkCalendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem
{
    public class CheckOverlapppingTimeModel
    {
        public EmployeeAttendAbsenceAnalysisInputModel inputModel { get; set; }
        public DateTime date { get; set; }
        public List<TimeShiftSettingByWorkCityIdModel> timeShiftSettingByWorkCityIdModel { get; set; }
        public DateTime yesterday { get; set; }
        public List<WorkCalendarForAttendAbcenseAnalysis> workCalendars { get; set; }
        public TimeSettingDataModel timeSettingDataModel { get; set; }
        public List<EmployeeEntryExitViewModel> entryExitResult { get; set; }
        public List<EmployeeEntryExitViewModel> TodayList { get; set; }
    }
}
