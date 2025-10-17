//using Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItems;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem;
using Ksc.HR.DTO.Personal.EmployeeEntryExit;
using Ksc.HR.DTO.Personal.EmployeeLongTermAbsences;
using Ksc.HR.DTO.Personal.MonthTimeSheet;
using Ksc.HR.DTO.Stepper;
using Ksc.HR.Share.Model;
using KSC.Common;
using KSC.Common.Filters.Models;
using KSC.MIS.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Interfaces.Personal
{
    public interface IEmployeeAttendAbsenceItemService
    {
        FilterResult<ReportEmployeeAttendAbsenceItemModel> GetReportEmployeeEntryExitData(SearchEmployeeEntryExitModel filter);
        Task<ReturnData<RPC005Evaluation>> SaveEvaluationDevelopment(RPC005Evaluation model); //تایید کارکرد ارزیابی کارکنان
        Task<MyKscResult> OfficialHolidayForItems(OfficialHolidayForItemsModel model);

        Task<TeamAndPersonelsFunctionalDetailsModel> GetTeamAndPersonelsFunctionalDetails(EmployeeEntryExitManagementInputModel inputModel);
        Task<TeamAndPersonelsFunctionalDetailsModel> GetPersonelsFunctionalDetails(EmployeeEntryExitManagementInputModel inputModel);
        Task<TeamAndPersonelsFunctionalDetailsModel> GetTeamFunctionalDetails(EmployeeEntryExitManagementInputModel inputModel);

        Task<FilterResult<EmployeeAttendAbsenceAnalysisModel>> GetEmployeeAttendAbsenceAnalysis(EmployeeAttendAbsenceAnalysisInputModel inputModel);
        IEnumerable<EmployeeAttendAbsenceItemForOnCallViewModel> GetEmployeeAttendAbsenceItemForOnCall(DateTime startDate, DateTime endDate);
        //Task<TimeSettingDataModel> GetShiftDateTimeSetting(int employeeId ,int shiftConceptDetailId, int workCityId, int workGroupId, int workCalendarId);
        Task<List<TimeSettingDataModel>> GetAllShiftStartEndTime(List<int> shiftConceptDetailId, List<int> workGroupId, List<int> workCityId, int workCalendarId);
        KscResult AddUpdateEmplyeeAttendAbsenseItems(List<AddEmployeeAttendAbsenceItemModel> models,bool IsOfficialAttend,bool NotCheckMinimumWorkTimeAdmin);
        KscResult RemoveAttendAbcenceItem(List<AddEmployeeAttendAbsenceItemModel> model);
        Task<FilterResult<EmployeeDontHaveAttendITemModel>> GetEmployeeDontHaveAttendITemModel(SearchEmployeeEntryExitModel Filter);
        Task<FilterResult<EmployeeDontHaveAttendITemModel>> GetEmployeeDontHaveAttendItemModelReportByTeam(SearchReportEmployeeEntryExitModel Filter);
        Task<KscResult> AddMissionAttendAbcenceItem(PAR_ASSPY model);
        Task<KscResult> RemoveMissionItem(PAR_ASSPY models);
        bool ISemployeeAttendAbsenceItem(int employeeId, DateTime entryExitDate);
        Task<ShowMonthTimeSheetViewModel> GetEmployeeTimeSheet(string employeeNumber, string timeSheetMonth);
        // Task<int?> SumForcedOverTimeAttendAbsence(EmployeeAttendAbsenceAnalysisInputModel inputModel);

        FilterResult<ReportEmployeeAttendAbsenceItemModel> GetReportEmployeeAttendAbsenceItemData(SearchEmployeeEntryExitModel model);
        Task<string> UpdateForcedOverTimeByShamsiYearMonth(int yearMonth);
        Task<KscResult> OverTimeSpecialHolidayTimeSheet(SearchMonthTimeSheetModel model);
        Task<KscResult> CeilingOvertimeTimeSheet(SearchMonthTimeSheetModel model);
        Task<FilterResult<EmployeeDontHaveAttendITemModel>> GetEmployeeDontHaveAttendItemModelReport(SearchReportEmployeeEntryExitModel filter);
        Task<bool> UpdateRollCallDefinitionId();
        EmployeeWorkGroup GetEmployeeWorkGroupByEmployeeIdDate(DateTime date, List<EmployeeWorkGroup> model);
        Task<KscResult> CalculateForcedOverTimeByYearMonth(SearchMonthTimeSheetModel model);
        Task<KscResult> OverTimeSpecialHolidayTimeSheetStep(UpdateStatusByYearMonthProcedureModel model);
        Task<KscResult> CalculateForcedOverTimeStep(UpdateStatusByYearMonthProcedureModel model);
        Task<KscResult> CeilingOvertimeTimeSheetStep(UpdateStatusByYearMonthProcedureModel model);
        Task<KscResult> TestOverTimeSpecialHolidayTimeSheet(SearchMonthTimeSheetModel model);
        Task<KscResult> TestCeilingOvertimeTimeSheetStep(UpdateStatusByYearMonthProcedureModel model);
        Task<KscResult> AddMissionAttendAbcenceItemFromTextFile(List<PAR_ASSPY> data);
    }
}
