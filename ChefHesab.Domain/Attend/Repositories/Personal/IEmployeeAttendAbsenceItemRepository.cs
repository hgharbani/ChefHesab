using KSC.Domain;
using Ksc.HR.Domain.Entities.Personal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ksc.HR.Domain.Repositories.Personal
{
    public interface IEmployeeAttendAbsenceItemRepository : IRepository<EmployeeAttendAbsenceItem, long>
    {
        IQueryable<EmployeeAttendAbsenceItem> GetAllQuaryble(List<long> ids);
        IQueryable<EmployeeAttendAbsenceItem> GetEmployeeAttendAbsenceItemActiveByEmployeeIdAsNoTracking(int employeeId);
        IQueryable<EmployeeAttendAbsenceItem> GetEmployeeAttendAbsenceItemAsNoTracking();
        IQueryable<EmployeeAttendAbsenceItem> GetEmployeeAttendAbsenceItemByEmployeeIdWorkCalendarIdAsNoTracking(int employeeId, int workCalendarId);
        IQueryable<EmployeeAttendAbsenceItem> GetEmployeeAttendAbsenceItemByRelated();
        IQueryable<EmployeeAttendAbsenceItem> GetEmployeeAttendAbsenceItemForOncallByRelatedAsNoTracking();
        IQueryable<EmployeeAttendAbsenceItem> GetEmployeeAttendAbsenceItemIncludedShiftConceptDetail();
        IQueryable<EmployeeAttendAbsenceItem> GetEmployeeAttendAbsenceItemIncludWorkCalendarAsNoTracking();
        //Task<List<SpEmployeeTeamNotConfirmedReturnModel>> GetSpEmployeeTeamNotConfirmedAsync(DateTime? startDate, DateTime? endDate, string userName);
        Task<List<SpEmployeeTeamNotConfirmedReturnModel>> GetSpEmployeeTeamNotConfirmedReportAsync(DateTime? startDate, DateTime? endDate, string userName, string startTeamCode,string endTeamCode,string personelNumberCode);

        IQueryable<EmployeeAttendAbsenceItem> GetMonthTimeSheetCalculateAsNoTracking(int yearMonth);
        IQueryable<EmployeeAttendAbsenceItem> GetEmployeeAttendAbsenceItemByOverTimeToken(string overTimeToken);
        IQueryable<EmployeeAttendAbsenceItem> GetEmployeeAttendAbsenceItemByEmployeeIdWorkCalendarIdNoIncludedAsNoTracking(int employeeId, int workCalendarId);
        IQueryable<EmployeeAttendAbsenceItem> GetEmployeeAttendAbsenceItemIncludEmployeeEntryExitAttendAbsences();
        IQueryable<EmployeeAttendAbsenceItem> GetEmployeeAttendAbsenceItemByYearMonthAsNoTracking(int yearMonth);
        IQueryable<EmployeeAttendAbsenceItem> GetItemsHolidayByworkcalendarIds(int employeeId, List<int> workcalendarIds);
        IQueryable<EmployeeAttendAbsenceItem> GetEmployeeAttendAbsenceItemValidRecordAsNoTracking();
        IQueryable<EmployeeAttendAbsenceItem> GetValidItems();
       List<SpGetEmployeeLeaveStatusReturnModel> SpGetEmployeeLeaveStatus(int? from, int? to);
        List<EmployeeAttendAbsenceItem> GetEmployeeAttendAbsenceItemByMissionId(int MissionRequestId);
        IQueryable<EmployeeAttendAbsenceItem> GetEmployeeAttendAbsenceItemByRangeDateAndRollCallConcept(DateTime startDate, DateTime endDate, int rollCallConceptId);
        IQueryable<EmployeeAttendAbsenceItem> GetEmployeeEntry_ExitAttendAbsencesByWorkcalendarIds(int employeeId, List<int> workcalendarIds);
        IQueryable<EmployeeAttendAbsenceItem> GetEmployeeAttendAbsenceItemByEmployeeIdWorkCalendarIdAsNoTracking(List<int> employeeId, int workCalendarId);
        IQueryable<EmployeeAttendAbsenceItem> GetMonthTimeSheetEmployeeCalculateAsNoTracking(int yearMonth, long employeeId);
        IQueryable<EmployeeAttendAbsenceItem> GetEmployeeAttendAbsenceItemByMissionRequestId(int MissionRequestId);
        int GetEmployeeAttendAbsenceItemByEmployeeIdForConditionalAbsence(int employeeId, int rollcallDefinitionId, DateTime date);
        IQueryable<EmployeeAttendAbsenceItem> GetEmployeeAttendAbsenceItemValidRecordAndInCludedEmployeeAsNoTracking();
        Task<List<SpEmployeeTeamNotConfirmedReturnModel>> GetSpGetEmployeeDontHaveAttendItemModelReportAsync(DateTime? startDate, DateTime? endDate, string startteamCode, string endteamCode, string personelNumberCode);
    }
}
