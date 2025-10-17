using KSC.Common;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.WorkCalendar;
using KSC.Common.Filters.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ksc.HR.DTO.Stepper;
using KscHelper.Model;
using System;

namespace Ksc.HR.Appication.Interfaces.WorkShift
{
    public interface IWorkCalendarService
    {
        Task<bool> CheckDailyTimeSheetStatus(string date);
        Task<KscResult> EditWorkCalendar(EditWorkCalendarModel model);
        Task<KscResult> EditWorkCalenderByStatus(EditWorkCalenderStatus model);

        Task<WorkCalendar> GetOne(int id);
        FilterResult<WorkCalendarModel> GetWorkCalendarByFilter(WorkCalendarFilterRequest Filter);
        Task<EditWorkCalendarModel> GetWorkCalendarById(int id);
        FilterResult<WorkCalendarIsHolidayModel> GetWorkCalendarWithIsHoliday(WorkCalendarFilterRequest Filter);
        Task<ReturnData<WorkCalendarIsHolidayModel>> IsHoliday(DateTime date);
        Task<KscResult> SetWorkDayTypeOnWorkCalendar(int year);
        Task<KscResult> UpdateHijriDate(int dateKeyStart, int dateKeyEnd);
        Task<KscResult> UpdateWorkCalenderByYearMonthStatus(UpdateWorkCalenderByYearMonthStatusModel model);
        Task<KscResult> UpdateWorkCalenderByYearMonthStatusStep(UpdateStatusByYearMonthProcedureModel model);
    }
}