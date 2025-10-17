using AutoMapper;
using Ksc.Hr.Domain.Shared;
using Ksc.HR.Appication.Interfaces.Personal;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.Personal.MonthTimeSheet;
using Ksc.HR.DTO.WorkShift.IncludedDefinition;
using Ksc.HR.Resources.Messages;
using Ksc.HR.Share.Model.Employee;
using Ksc.HR.Share.Model.SystemSequenceStatus;
using KSC.Common;
using KSC.Common.Filters.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Share.General;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.DTO.MIS;
using Ksc.HR.Share.Model.RollCallDefinication;
using KSC.MIS.Service;
using Ksc.HR.DTO.Personal.MonthTimeSheetRollCall;
using DNTPersianUtils.Core;
using Ksc.HR.DTO.WorkShift.RollCallCategory;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Newtonsoft.Json;
using Ksc.HR.Appication.Interfaces.MIS;
using Ksc.HR.Share.Model.MonthTimeSheet;
using Ksc.HR.Resources.Personal;
using Ksc.HR.DTO.Personal.MonthTimeSheetDraft;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem;

namespace Ksc.HR.Appication.Services.Personal
{
    public class MonthTimeSheetDraftService : IMonthTimeSheetDraftService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;

        public MonthTimeSheetDraftService(IKscHrUnitOfWork kscHrUnitOfWork
             , IMapper mapper
             , IFilterHandler FilterHandler

             )
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;
            _mapper = mapper;
            _FilterHandler = FilterHandler;


        }
        //public void SumForcedOverTimeAttendAbsence1(ForcedOverTimeAttendAbsenceModel model)
        //{
        // // if(model.InsertData
        //}
        //public void SumForcedOverTimeAttendAbsence(MonthTimeSheetDraftModel model)
        //{
        //    var rollCallIncludedForcedOverTime = _kscHrUnitOfWork.IncludedRollCallRepository.GetActiveIncludedRollCallByIncludedIdAsNoTracking(EnumIncludedDefinition.ForcedOverTime.Id).Select(w => w.RollCallDefinitionId).ToList();
        //    List<MonthTimeSheetDraftModel> data = new List<MonthTimeSheetDraftModel>();
        //    if (model.FromDailyItem == false)
        //    {
        //        var rollCallForcedOverTime = from item in model.EmployeeAttendAbsenceItemForcedOverTimeModel
        //                                     join rollCallId in rollCallIncludedForcedOverTime on item.RollCallDefinitionId equals rollCallId
        //                                     select item;
        //        var itemData = rollCallForcedOverTime.GroupBy(x => x.WorkCalendarId).Select(x => new MonthTimeSheetDraftModel
        //        {
        //            WorkCalendarId = x.Key,
        //            EmployeeId = model.EmployeeId,
        //            ForcedOverTime =
        //            EmployeeAttendAbsenceItemForcedOverTimeModel = x.Where(i => i.WorkCalendarId == x.Key).ToList()
        //        }).ToList();
        //        data = _mapper.Map<List<List<EmployeeAttendAbsenceItemForcedOverTimeModel>>>(itemData);
        //    }
        //    else
        //    {
        //        data.Add(model);
        //    }
        //    var workCalendarData = _kscHrUnitOfWork.WorkCalendarRepository.GetYearMonthByWorkCalendarId(model.WorkCalendarId);
        //    int yearMonth = workCalendarData.Item1;
        //    model.YearMonth = yearMonth;
        //    var workCalendarDate = workCalendarData.Item2;
        //    model.RollCallIncludedForcedOverTime = rollCallIncludedForcedOverTime;
        //    SumForcedOverTimeAttendAbsenceCalculate(model);

        //}
        //public void SumForcedOverTimeAttendAbsenceCalculate(MonthTimeSheetDraftModel model)
        //{
        //    int sumTimeDuration = 0;
        //    //



        //    var monthTimeSheetDraftByEmployeeIdYearMonth = _kscHrUnitOfWork.MonthTimeSheetDraftRepository.GetMonthTimeSheetDraftByEmployeeIdYearMonth(model.EmployeeId, model.YearMonth);
        //    if (model.InsertData)// ایجاد اطلاعات آیتم
        //    {
        //        var monthTimeSheetDraftByWorkTimeId = monthTimeSheetDraftByEmployeeIdYearMonth.FirstOrDefault(x => x.WorkTimeId == model.WorkTimeId);
        //        if (model.ShiftSettingFromShiftboard == false) //شیفتی نباشد
        //        {
        //            sumTimeDuration = SetForcedOverTime(model, null);

        //        }
        //        else //شیفتی باشد
        //        {
        //            if (model.EmployeeAttendAbsenceItemForcedOverTimeModel.Any(x => model.RollCallIncludedForcedOverTime.Any(y => y == x.RollCallDefinitionId)))
        //            {
        //                sumTimeDuration = model.ForcedOverTime;
        //            }
        //            if (monthTimeSheetDraftByWorkTimeId != null)
        //            {
        //                sumTimeDuration = monthTimeSheetDraftByWorkTimeId.ForcedOverTime + sumTimeDuration;
        //            }
        //        }
        //        //
        //        ForcedOverTimeItem forcedOverTimeItem = new ForcedOverTimeItem()
        //        {
        //            WorkCalendarId = model.WorkCalendarId,
        //            ForcedOverTime = model.ForcedOverTime,
        //            TotalWorkHourInWeekPerMinute = model.TotalWorkHourInWeek,
        //            HasIncludedRollCallForcedOverTime = model.EmployeeAttendAbsenceItemForcedOverTimeModel.Any(x => model.RollCallIncludedForcedOverTime.Any(y => y == x.RollCallDefinitionId))
        //        };
        //        if (monthTimeSheetDraftByWorkTimeId != null)
        //        {
        //            monthTimeSheetDraftByWorkTimeId.ForcedOverTime = sumTimeDuration;
        //            monthTimeSheetDraftByWorkTimeId.ForcedOverTimeItems.Add(forcedOverTimeItem);
        //        }
        //        else
        //        {
        //            MonthTimeSheetDraft newMonthTimeSheetDraft = new MonthTimeSheetDraft();
        //            newMonthTimeSheetDraft.EmployeeId = model.EmployeeId;
        //            newMonthTimeSheetDraft.YearMonth = model.YearMonth;
        //            newMonthTimeSheetDraft.WorkTimeId = model.WorkTimeId;
        //            newMonthTimeSheetDraft.ForcedOverTime = model.ForcedOverTime;
        //            newMonthTimeSheetDraft.ForcedOverTimeItems.Add(forcedOverTimeItem);
        //            _kscHrUnitOfWork.MonthTimeSheetDraftRepository.Add(newMonthTimeSheetDraft);
        //        }
        //    }
        //    else //حذف اطلاعات آیتم
        //    {
        //        WorkTime workTime = _kscHrUnitOfWork.WorkTimeRepository.GetWorkTimeInAttendAbsenceItemAsNoTracking(model.EmployeeId, model.WorkCalendarId);
        //        var monthTimeSheetDraftByWorkTimeId = monthTimeSheetDraftByEmployeeIdYearMonth.FirstOrDefault(x => x.WorkTimeId == workTime.Id);
        //        if (monthTimeSheetDraftByWorkTimeId != null)
        //        {
        //            //
        //            var forcedOverTimeItem = _kscHrUnitOfWork.ForcedOverTimeItemRepository.GetForcedOverTimeItemByMonthDraftIdWorkCalendarId(model.WorkCalendarId, monthTimeSheetDraftByWorkTimeId.Id);
        //            if (forcedOverTimeItem != null)
        //            {
        //                int totalWorkHourInWeek = forcedOverTimeItem.TotalWorkHourInWeekPerMinute ?? 0;
        //                int forcedOverTime = forcedOverTimeItem.ForcedOverTime ?? 0;
        //                //
        //                if (workTime.ShiftSettingFromShiftboard)
        //                {
        //                    if (model.EmployeeAttendAbsenceItemForcedOverTimeModel.Any(x => model.RollCallIncludedForcedOverTime.Any(y => y == x.RollCallDefinitionId)))
        //                    {
        //                        monthTimeSheetDraftByWorkTimeId.ForcedOverTime = monthTimeSheetDraftByWorkTimeId.ForcedOverTime - forcedOverTime;
        //                    }
        //                }
        //                else
        //                {
        //                    model.TotalWorkHourInWeek = forcedOverTimeItem.TotalWorkHourInWeekPerMinute ?? 0;
        //                    monthTimeSheetDraftByWorkTimeId.ForcedOverTime = SetForcedOverTime(model, workTime);
        //                }
        //                _kscHrUnitOfWork.ForcedOverTimeItemRepository.Delete(forcedOverTimeItem);
        //            }
        //        }
        //    }


        //}
        //private int SetForcedOverTime(MonthTimeSheetDraftModel model, WorkTime worktime)
        //{
        //    int sumTimeDuration = 0;
        //    var employeeAttendAbsenceItems = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetEmployeeAttendAbsenceItemAsNoTracking();
        //    var rollCallIncludedForcedOverTime = model.RollCallIncludedForcedOverTime;
        //    var workCalendar = _kscHrUnitOfWork.WorkCalendarRepository.GetWorkCalendarByYearMonthAsNoTracking(model.YearMonth);
        //    var queryEmployeeAttendAbsenceItemByEmployeeId = from item in employeeAttendAbsenceItems
        //                                                     join cal in workCalendar on item.WorkCalendarId equals cal.Id

        //                                                     where item.EmployeeId == model.EmployeeId && item.InvalidRecord == false
        //                                                     && item.WorkTimeId == model.WorkTimeId
        //                                                     select new EmployeeAttendAbsenceItemForcedOverTimeModel()
        //                                                     {
        //                                                         EmployeeAttendAbsenceItemId = item.Id,
        //                                                         RollCallDefinitionId = item.RollCallDefinitionId,
        //                                                         TimeDurationInMinute = item.TimeDurationInMinute.Value,
        //                                                         WorkTimeId = item.WorkTimeId,
        //                                                         WorkCalendarId = item.WorkCalendarId,
        //                                                         WorkCalendarDate = cal.MiladiDateV1

        //                                                     };
        //    var employeeAttendAbsenceItemByEmployeeId = queryEmployeeAttendAbsenceItemByEmployeeId.ToList();
        //    if (model.InsertData)
        //    {

        //        var selectedItem = model.EmployeeAttendAbsenceItemForcedOverTimeModel;
        //        employeeAttendAbsenceItemByEmployeeId = employeeAttendAbsenceItemByEmployeeId.Union(selectedItem).ToList();
        //        worktime = _kscHrUnitOfWork.WorkTimeRepository.GetWorkTimeByIdAsNoTracking(model.WorkTimeId);
        //    }
        //    else
        //    {
        //        employeeAttendAbsenceItemByEmployeeId = employeeAttendAbsenceItemByEmployeeId.Where(x => x.WorkCalendarId != model.WorkCalendarId).ToList();
        //    }

        //    sumTimeDuration = CalculateForcedOverTime(employeeAttendAbsenceItemByEmployeeId, rollCallIncludedForcedOverTime, worktime.MaximumForcedOverTime, model.TotalWorkHourInWeek);
        //    return sumTimeDuration;
        //}

        //private int CalculateForcedOverTime(List<EmployeeAttendAbsenceItemForcedOverTimeModel> employeeAttendAbsenceItemByEmployeeId, List<int> rollCallIncludedForcedOverTime, string maximumForcedOverTime, int totalWorkHourInWeek)
        //{
        //    var rollCallForcedOverTime = from item in employeeAttendAbsenceItemByEmployeeId
        //                                 join rollCallId in rollCallIncludedForcedOverTime on item.RollCallDefinitionId equals rollCallId
        //                                 select item;
        //    var sumTimeDuration = rollCallForcedOverTime.Sum(x => x.TimeDurationInMinute);
        //    var maximumForcedOverTimeToMinute = maximumForcedOverTime.ConvertDurationToMinute();
        //    var div = (sumTimeDuration / totalWorkHourInWeek) * 60;//خارج قسمت

        //    var mod = sumTimeDuration % totalWorkHourInWeek;//باقیمانده
        //    int baseMinute = 0;
        //    if (mod > 1440)
        //    {
        //        baseMinute = 60;//60min=1 h
        //    }
        //    sumTimeDuration = baseMinute + div;
        //    if (sumTimeDuration >= maximumForcedOverTimeToMinute)
        //    {
        //        sumTimeDuration = maximumForcedOverTimeToMinute.Value;
        //    }

        //    return sumTimeDuration;
        //}

    }
}
