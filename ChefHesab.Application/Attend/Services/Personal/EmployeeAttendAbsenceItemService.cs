using Antlr.Runtime.Tree;
using AutoMapper;
using DNTPersianUtils.Core;
using Ksc.Hr.Domain.Shared;
using Ksc.HR.Appication.Interfaces.Personal;
using Ksc.HR.Appication.Interfaces.WorkShift;
using Ksc.HR.Application.Interfaces;
using Ksc.HR.Domain.Entities.BusinessTrip;
using Ksc.HR.Domain.Entities.Chart;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Entities.ScheduledLoger;
using Ksc.HR.Domain.Entities.Stepper;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;

using Ksc.HR.DTO.OnCall.OnCall_Request;
using Ksc.HR.DTO.OnCall.OnCall_Type;
using Ksc.HR.DTO.Other;
using Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem;
using Ksc.HR.DTO.Personal.EmployeeEntryExit;
using Ksc.HR.DTO.Personal.EmployeeLongTermAbsences;
using Ksc.HR.DTO.Personal.EmployeeTimeSheet;
using Ksc.HR.DTO.Personal.MonthTimeSheet;
using Ksc.HR.DTO.Stepper;
using Ksc.HR.DTO.WorkFlow.Classes.Enumerations;
using Ksc.HR.DTO.WorkShift.IncludedDefinition;
using Ksc.HR.DTO.WorkShift.RollCallCategory;
using Ksc.HR.DTO.WorkShift.RollCallConcept;
using Ksc.HR.DTO.WorkShift.RollCallDefinication;
using Ksc.HR.DTO.WorkShift.SacrificeOptionSetting;
using Ksc.HR.DTO.WorkShift.ShiftConceptDetail;
using Ksc.HR.DTO.WorkShift.TimeShiftSetting;
using Ksc.HR.Resources.Messages;
using Ksc.HR.Share.Extention;
using Ksc.HR.Share.General;
using Ksc.HR.Share.Model;
using Ksc.HR.Share.Model.AnalysisType;
using Ksc.HR.Share.Model.CompatibleRollCall;
using Ksc.HR.Share.Model.DayNightRollCall;
using Ksc.HR.Share.Model.PaymentStatus;
using Ksc.HR.Share.Model.RollCallDefinication;
using Ksc.HR.Share.Model.ShiftConcept;
using Ksc.HR.Share.Model.SystemSequenceStatus;
using Ksc.HR.Share.Model.TimeShiftSetting;
using Ksc.HR.Share.Model.Vacation;
using Ksc.HR.Share.Model.WorkCalendar;
using KSC.Common;
//using Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItems;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using KSC.MIS.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NetTopologySuite.Index.HPRtree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Services.Personal
{
    public class EmployeeAttendAbsenceItemService : IEmployeeAttendAbsenceItemService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;
        // private readonly IEmployeeAttendAbsenceItemRepository _EmployeeAttendAbsenceItemRepository;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;
        private readonly IEmployeeEntryExitService _employeeEntryExitService;
        private readonly IRollCallDefinitionService _rollCallDefinitionService;
        private readonly IShiftConceptDetailService _shiftConceptDetailService;
        private readonly IStepper_ProcedureService _procedureService;
        private readonly IConfiguration _config;
        public EmployeeAttendAbsenceItemService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler, IEmployeeEntryExitService employeeEntryExitService, IRollCallDefinitionService rollCallDefinitionService, IShiftConceptDetailService shiftConceptDetailService, IStepper_ProcedureService procedureService, IConfiguration config)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;
            _mapper = mapper;
            _FilterHandler = FilterHandler;
            _employeeEntryExitService = employeeEntryExitService;
            _rollCallDefinitionService = rollCallDefinitionService;
            _shiftConceptDetailService = shiftConceptDetailService;
            _procedureService = procedureService;
            _config = config;
        }
        public IEnumerable<EmployeeAttendAbsenceItemForOnCallViewModel> GetEmployeeAttendAbsenceItemForOnCall(DateTime startDate, DateTime endDate)
        {
            var query = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetEmployeeAttendAbsenceItemForOncallByRelatedAsNoTracking();
            // var rollCallDefinitionOnCall = _kscHrUnitOfWork.OnCall_TypeRepository.GetAllOnCallTypeNoTracking().Select(x => x.RollCallDefinitionId);
            var onCallType = _kscHrUnitOfWork.OnCall_TypeRepository.GetAllOnCallTypeNoTracking();
            var changeableRollCall = _kscHrUnitOfWork.CompatibleRollCallRepository.GetCompatibleRollCallByCompatibleRollCallType(EnumCompatibleRollCallType.Interchangeable.Id);
            var RollCallDefinitionValid = changeableRollCall
                .Join(onCallType, x => x.CompatibleRollCallId, y => y.RollCallDefinitionId, (x, y) => new OncallRollCallDefinitionModel()
                { RollCallDefinitionId = x.RollCallDefinitionId, OncallRollCallDefinitionId = y.RollCallDefinitionId, OnCallTypeId = y.Id });
            var startDateCheck = startDate.Date;
            var endDateCheck = endDate.Date;
            //باید کدهای حضور غیاب که قابل تبدیل به فراخوان هستند چک شوند
            query = query.Where(x => x.WorkCalendar.MiladiDateV1 >= startDateCheck && x.WorkCalendar.MiladiDateV1 <= endDateCheck);
            var data = from x in query
                       join r in RollCallDefinitionValid on x.RollCallDefinitionId equals r.RollCallDefinitionId
                       select new EmployeeAttendAbsenceItemForOnCallViewModel()
                       {
                           EmployeeAttendAbsenceItemId = x.Id,
                           EmployeeId = x.EmployeeId,
                           StartDate = x.WorkCalendar.MiladiDateV1,
                           StartTime = x.StartTime,
                           RollCallDefinitionIdForOnCall = r.OncallRollCallDefinitionId,
                           OnCallDuration = x.TimeDuration,
                           DayType = x.WorkCalendar.WorkDayType.Title,
                           OnCallTypeId = r.OnCallTypeId

                       };
            return data;
        }
        public async Task<FilterResult<EmployeeAttendAbsenceAnalysisModel>> GetEmployeeAttendAbsenceAnalysis(EmployeeAttendAbsenceAnalysisInputModel inputModel)
        {
            var analysisResultModel = await GetDataEmployeeAttendAbsenceAnalysis(inputModel);
            //
            var employeeAttendAbsenceAnalysisModel = analysisResultModel.EmployeeAttendAbsenceAnalysisModel.FirstOrDefault();
            if (inputModel.IsOfficialAttend && employeeAttendAbsenceAnalysisModel != null &&
                employeeAttendAbsenceAnalysisModel.EmployeeAttendAbsenceItemId == 0
                && employeeAttendAbsenceAnalysisModel.RollCallConceptId == EnumRollCallConcept.Absence.Id)
            {
                analysisResultModel.EmployeeAttendAbsenceAnalysisModel.First().ModifyIsValid = true;
            }
            //چک کردن واکسیناسیون
            if (analysisResultModel.HasItem == false)
            {
                VaccinationCheck(analysisResultModel);
            }
            var attendAbcenseResultModel = analysisResultModel.EmployeeAttendAbsenceAnalysisModel;
            //
            var result = new FilterResult<EmployeeAttendAbsenceAnalysisModel>();
            //
            foreach (var item in attendAbcenseResultModel)
            {
                item.EmployeeId = inputModel.EmployeeId;
                item.InvalidForcedOvertime = analysisResultModel.InvalidForcedOvertime;

                if (analysisResultModel.TimeSettingDataModel != null)
                {
                    if (item.StartTime.ConvertStringToTimeSpan() == analysisResultModel.TimeSettingDataModel.ShiftStartTimeToTimeSpan && item.StartDate == analysisResultModel.TimeSettingDataModel.ShiftStartDate)
                    {
                        item.IsShiftStart = true;
                    }
                    if (item.EndTime.ConvertStringToTimeSpan() == analysisResultModel.TimeSettingDataModel.ShiftEndTimeToTimeSpan && item.EndDate == analysisResultModel.TimeSettingDataModel.ShiftEndDate)
                    {
                        item.IsShiftEnd = true;
                    }
                    //
                    item.ForcedOverTime = analysisResultModel.TimeSettingDataModel.ForcedOverTime;
                    item.TotalWorkHourInWeek = analysisResultModel.TimeSettingDataModel.TotalWorkHourInWeek;
                    item.YearMonth = analysisResultModel.TimeSettingDataModel.YearMonth;
                    item.ShiftSettingFromShiftboard = analysisResultModel.TimeSettingDataModel.ShiftSettingFromShiftboard;
                    item.WokCalendarId = inputModel.WorkCalendarId;
                    item.AttendTimeInTemprorayTime = analysisResultModel.AttendTimeInTemprorayTime;
                }
            }
            //
            List<int> trainingOverTime = new List<int>() { EnumRollCallDefinication.OnlineTrainingExtraWork.Id ,
            EnumRollCallDefinication.TrainingOverTime.Id,EnumRollCallDefinication.TrainingOverTimeInHoliday.Id
            };
            var compatibleRollCallId = _kscHrUnitOfWork.CompatibleRollCallRepository.GetCompatibleRollCallByCompatibleRollCallType(EnumCompatibleRollCallType.AddNewRowInTimeSheet.Id).Select(x => x.CompatibleRollCallId).ToList();

            if (inputModel.AnalysisType != EnumAnalysisType.Keshik.Id && inputModel.IsOfficialAttendForOverTime == false)
            {
                if (analysisResultModel.TimeSettingDataModel != null && analysisResultModel.TimeSettingDataModel.InvalidOverTime)
                {
                    if (string.IsNullOrEmpty(analysisResultModel.TimeSettingDataModel.ValidOverTimeStartTime))
                    {

                        attendAbcenseResultModel = attendAbcenseResultModel.Where(x =>
                        x.RollCallConceptId != EnumRollCallConcept.OverTime.Id
                         || compatibleRollCallId.Any(y => y == x.RollCallDefinicationInItemModel.Id)
                         || trainingOverTime.Any(t => t == x.RollCallDefinicationInItemModel.Id)
                         || x.RollCallDefinicationInItemModel.Id == EnumRollCallDefinication.janbaziExtraWork.Id
                         || (x.TemprorayOverTimeInStartShift)
                         ).ToList();
                    }
                    else
                    {
                        attendAbcenseResultModel = attendAbcenseResultModel.Where(x =>
                        x.RollCallConceptId != EnumRollCallConcept.OverTime.Id
                        || x.RollCallDefinicationInItemModel.Id == EnumRollCallDefinication.janbaziExtraWork.Id
                         || trainingOverTime.Any(t => t == x.RollCallDefinicationInItemModel.Id)
                          || compatibleRollCallId.Any(y => y == x.RollCallDefinicationInItemModel.Id)
                           || (x.TemprorayOverTimeInStartShift)
                         || (x.RollCallConceptId == EnumRollCallConcept.OverTime.Id &&
                          x.EndDate == analysisResultModel.TimeSettingDataModel.ShiftEndDate.Date &&
                         x.EndTime.ConvertStringToTimeSpan() <= analysisResultModel.TimeSettingDataModel.ValidOverTimeStartTime.ConvertStringToTimeSpan()
                         && ((
                           analysisResultModel.TimeSettingDataModel.ShiftEndDate.Date == analysisResultModel.TimeSettingDataModel.ShiftStartDate.Date &&
                         x.StartTime.ConvertStringToTimeSpan() > analysisResultModel.TimeSettingDataModel.ShiftStartTimeToTimeSpan)
                         || (analysisResultModel.TimeSettingDataModel.ShiftEndDate.Date > analysisResultModel.TimeSettingDataModel.ShiftStartDate.Date)
                         )
                         )).ToList();
                    }
                }
            }
            //
            var modelResult = new FilterResult<EmployeeAttendAbsenceAnalysisModel>
            {
                Data = attendAbcenseResultModel,
                Total = result.Total
            };

            return modelResult;
        }
        public async Task<AnalysisAttenAbcenseResultModel> GetDataEmployeeAttendAbsenceAnalysis(EmployeeAttendAbsenceAnalysisInputModel inputModel)
        {
            // چک کردن وضعیت سیستم در جدول بالایی
            AnalysisAttenAbcenseResultModel analysisResultModel = new AnalysisAttenAbcenseResultModel() { EmployeeAttendAbsenceAnalysisModel = new List<EmployeeAttendAbsenceAnalysisModel>(), RollCallDefinitionIdForVaccinationCheck = new List<int>() };
            List<EmployeeAttendAbsenceAnalysisModel> resultModel = new List<EmployeeAttendAbsenceAnalysisModel>();
            var date = inputModel.Date.ToGregorianDateTime().Value.Date;

            //
            #region تایید کارکرد شده باشد
            // در صورتیکه تایید کارکرد شده باشد
            var attendAbsenceItemByEmployeeIdWorkCalendarId = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetEmployeeAttendAbsenceItemByEmployeeIdWorkCalendarIdAsNoTracking(inputModel.EmployeeId, inputModel.WorkCalendarId);
            attendAbsenceItemByEmployeeIdWorkCalendarId = attendAbsenceItemByEmployeeIdWorkCalendarId.Where(x => x.InvalidRecord == false);
            if (attendAbsenceItemByEmployeeIdWorkCalendarId.Any())
            {
                foreach (var item in attendAbsenceItemByEmployeeIdWorkCalendarId)
                {
                    resultModel.Add(new EmployeeAttendAbsenceAnalysisModel()
                    {
                        EmployeeAttendAbsenceItemId = item.Id,
                        EmployeeId = item.EmployeeId,
                        ShiftConceptDetailId = item.ShiftConceptDetailId,
                        StartTime = item.StartTime,
                        EndTime = item.EndTime,
                        Duration = item.IncreasedTimeDuration != null ? Utility.ConvertMinuteIn24ToDuration(item.TimeDurationInMinute.Value + item.IncreasedTimeDuration.Value) : item.TimeDuration,
                        RollCallDefinicationInItemModel = new RollCallDefinicationInItemModel()
                        {
                            Id = item.RollCallDefinitionId,
                            Code = item.RollCallDefinition.Code,
                            Title = item.RollCallDefinition.Title
                        },
                        RollCallConceptId = item.RollCallDefinition.RollCallConceptId,
                        DeleteIsValid = false,
                        ModifyIsValid = false,

                    });
                }
                analysisResultModel.EmployeeAttendAbsenceAnalysisModel.AddRange(resultModel);
                analysisResultModel.HasItem = true;
                return analysisResultModel;
            }
            #endregion
            //
            //
            var employee = await _kscHrUnitOfWork.EmployeeRepository.GetEmployeeByEmployeeId(inputModel.EmployeeId);
            string sacrificeAttendAbsenceTolerance = null;
            if (employee.SacrificePercentage != null)
                sacrificeAttendAbsenceTolerance = await _kscHrUnitOfWork.SacrificePercentageSettingRepository.GetAttendAbsenceToleranceBySacrificePercentage(employee.SacrificePercentage.Value);
            var timeShiftSettingByWorkCityIdModel = _kscHrUnitOfWork.TimeShiftSettingRepository.GetTimeShiftSettingByWorkCityId(employee.WorkCityId.Value).ToList();
            //
            DateTime yesterday = date.AddDays(-1);
            DateTime tomorrow = date.AddDays(1);
            //
            var workCalendars = _kscHrUnitOfWork.WorkCalendarRepository.GetWorkCalendarForAttendAbcenseAnalysis(yesterday, tomorrow, employee.WorkCityId).ToList();//گرفتن اطلاعات تقویم از دیروز تا فردا تاریخ مورد نظر
            GetworkCalendarsByWorkCityAndDomain(employee.WorkCityId, inputModel.Domain, workCalendars);
            TimeShiftDateTimeSettingModel timeShiftDateTimeSettingModel = new TimeShiftDateTimeSettingModel()
            {
                employeeId = inputModel.EmployeeId,
                employeeNumber = employee.EmployeeNumber,
                shiftConceptDetailId = inputModel.ShiftConceptDetailId,
                workGroupId = inputModel.WorkGroupId,
                date = date,
                timeShiftSettingByWorkCityIdModel = timeShiftSettingByWorkCityIdModel,
                workCalendars = workCalendars,
                FloatTimeSettingId = employee.FloatTimeSettingId,
                WorkCalendarId = inputModel.WorkCalendarId
            };
            //timeShiftDateTimeSettingModel.IsBreastfeddingInStartShift = employee.IsBreastfeddingInStartShift;
            //bool breastfeddingOption = false;
            //if (employee.BreastfeddingStartDate != null && employee.BreastfeddingEndDate != null
            //           && employee.BreastfeddingStartDate.Value.Date <= date && employee.BreastfeddingEndDate.Value.Date >= date)
            //{
            //    breastfeddingOption = true;
            //}
            bool validConditionalAbsence = false;
            bool validConditionalAbsenceInStartShift = false;
            int conditionalAbsenceRollCallId = 0;
            string conditionalAbsenceToleranceTime = null;
            int conditionalAbsenceSubjectId = 0;
            var employeeConditionalAbsenceForTimeSheetAnalys = _kscHrUnitOfWork.EmployeeConditionalAbsenceRepository.GetEmployeeConditionalAbsenceForTimeSheetAnalys(inputModel.EmployeeId, date);
            if (employeeConditionalAbsenceForTimeSheetAnalys != null)
            {
                if (employeeConditionalAbsenceForTimeSheetAnalys.HasForcedOvertime == false)
                    analysisResultModel.InvalidForcedOvertime = true;
                if (employeeConditionalAbsenceForTimeSheetAnalys.IsHourly)
                {
                    timeShiftDateTimeSettingModel.ValidConditionalAbsence = true;
                    timeShiftDateTimeSettingModel.ValidConditionalAbsenceInStartShift = employeeConditionalAbsenceForTimeSheetAnalys.IsInStartShift;
                    timeShiftDateTimeSettingModel.ConditionalAbsenceToleranceTime = employeeConditionalAbsenceForTimeSheetAnalys.HourlyAbsenceToleranceTime;
                    timeShiftDateTimeSettingModel.ValidConditionalAbsenceForBreastfedding = employeeConditionalAbsenceForTimeSheetAnalys.ConditionalAbsenceSubjectId == EnumConditionalAbsenceSubject.BreastFeedTolerance.Id;
                    //validConditionalAbsence = true;
                    //validConditionalAbsenceInStartShift = employeeConditionalAbsenceForTimeSheetAnalys.IsInStartShift;
                    //conditionalAbsenceRollCallId = employeeConditionalAbsenceForTimeSheetAnalys.RollCallDefinitionId;
                    //conditionalAbsenceToleranceTime = employeeConditionalAbsenceForTimeSheetAnalys.HourlyAbsenceToleranceTime;
                    //conditionalAbsenceSubjectId = employeeConditionalAbsenceForTimeSheetAnalys.ConditionalAbsenceSubjectId;
                }
            }
            //timeShiftDateTimeSettingModel.ValidConditionalAbsenceForBreastfedding = conditionalAbsenceSubjectId == EnumConditionalAbsenceSubject.BreastFeedTolerance.Id;
            //timeShiftDateTimeSettingModel.ValidConditionalAbsence = validConditionalAbsence;
            //timeShiftDateTimeSettingModel.ValidConditionalAbsenceInStartShift = validConditionalAbsenceInStartShift;
            //            timeShiftDateTimeSettingModel.ConditionalAbsenceToleranceTime = conditionalAbsenceToleranceTime;
            timeShiftDateTimeSettingModel.IsOfficialAttendForOverTime = inputModel.IsOfficialAttendForOverTime;
            timeShiftDateTimeSettingModel.IsValidHolidayValidOverTime = inputModel.IsValidHolidayValidOverTime;
            TimeSettingDataModel timeSettingDataModel = await GetTimeShiftDateTimeSetting(timeShiftDateTimeSettingModel);
            //
            if (employeeConditionalAbsenceForTimeSheetAnalys != null && employeeConditionalAbsenceForTimeSheetAnalys.IsHourly
                && timeSettingDataModel.ValidConditionalAbsence)
            {
                validConditionalAbsence = true;
                validConditionalAbsenceInStartShift = employeeConditionalAbsenceForTimeSheetAnalys.IsInStartShift;
                conditionalAbsenceRollCallId = employeeConditionalAbsenceForTimeSheetAnalys.RollCallDefinitionId;
                conditionalAbsenceToleranceTime = employeeConditionalAbsenceForTimeSheetAnalys.HourlyAbsenceToleranceTime;
                conditionalAbsenceSubjectId = employeeConditionalAbsenceForTimeSheetAnalys.ConditionalAbsenceSubjectId;
            }
            //

            EmployeeEntryExitYesterdayToTomorrowModel employeeEntryExitYesterdayToTomorrow = _employeeEntryExitService.GetEmployeeEntryExitForTimeSheet(inputModel.EmployeeId, date);

            //var allRollCallDefinition = _rollCallDefinitionService.GetRollCallDefinitionForEmployeeAttendAbsence();
            var allRollCall = _rollCallDefinitionService.GetRollCallsForEmployeeAttendAbsence(timeSettingDataModel, employee.EmploymentTypeId, date);
            var rollCallDefinition = allRollCall.RollCallToday;
            //
            analysisResultModel.IsValidUnVaccine = employee.IsValidUnVaccine;
            analysisResultModel.UnVaccineValidDate = employee.UnVaccineValidDate;
            analysisResultModel.VaccineDosage = employee.VaccineDosage;
            analysisResultModel.TimeSettingDataModel = timeSettingDataModel;
            analysisResultModel.RollCallDefinitionIdForVaccinationCheck = rollCallDefinition.Where(x => x.VaccinationCheck == true).Select(x => x.RollCallDefinitionId).ToList();

            //
            //در صورتیکه در آنالیز عدم حضور را با توجه به حضور کمتر از زمان مجاز انتخاب کرده باشد
            if (inputModel.IsAbsenceRow == "1")
            {
                resultModel = new List<EmployeeAttendAbsenceAnalysisModel>();
                AddDailyAbcenseToAnalysisModel(resultModel, timeSettingDataModel.ShiftStartTime, timeSettingDataModel.ShiftEndTime, timeSettingDataModel.WorkDayDuration, inputModel.ShiftConceptDetailId, timeSettingDataModel.ShiftStartDate, timeSettingDataModel.ShiftEndDate);
                analysisResultModel.EmployeeAttendAbsenceAnalysisModel.AddRange(resultModel);
                return analysisResultModel;
            }
            //
            List<EmployeeEntryExitViewModel> entryExitResult = new List<EmployeeEntryExitViewModel>();
            //
            TimeSettingDataModel timeshiftSettingYesterday = null;
            int shiftConceptDetailIdYesterDay = 0;
            bool continuousShift = false;
            //چک کرد پیوستگی از شیفت قبل تا شیفت امروز که وروز اول را ندارد و خروج دارد
            if (timeSettingDataModel.ShiftSettingFromShiftboard && timeSettingDataModel.ShiftCondeptId == EnumShiftConcept.Morning.Id && employeeEntryExitYesterdayToTomorrow.YesterdayList.Count() != 0)
            {
                var lastEntryExitYesterday = employeeEntryExitYesterdayToTomorrow.YesterdayList.OrderBy(x => x.EntryTime).LastOrDefault();
                if (lastEntryExitYesterday != null && lastEntryExitYesterday.ExitTime == null)
                {
                    var firstEntryExitToday = employeeEntryExitYesterdayToTomorrow.TodayList.OrderBy(x => x.EntryTime).FirstOrDefault();
                    if (firstEntryExitToday != null && firstEntryExitToday.EntryTime == null && firstEntryExitToday.ExitTime != null)
                    {

                        if (firstEntryExitToday.ExitTimeToTimeSpan > timeSettingDataModel.ShiftStartTimeWithToleranceToTimeSpan)
                        {
                            // چک کردن روز قبل
                            if (timeshiftSettingYesterday == null)
                            {
                                var workCalendarYesterday = workCalendars.First(x => x.Date == yesterday);
                                var yesterdayAttendAbsenceItem = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository
                                    .GetEmployeeAttendAbsenceItemByEmployeeIdWorkCalendarIdAsNoTracking(inputModel.EmployeeId, workCalendarYesterday.WorkCalendarId)
                                    .Where(x => !x.InvalidRecord)
                                    .ToList();
                                var employeeAttendAbsenceItemYesterday = yesterdayAttendAbsenceItem.FirstOrDefault();
                                if (employeeAttendAbsenceItemYesterday != null)
                                    shiftConceptDetailIdYesterDay = employeeAttendAbsenceItemYesterday.ShiftConceptDetailId;
                                timeshiftSettingYesterday = await GetShiftTimeSettingByDate(inputModel.EmployeeId, workCalendarYesterday, timeShiftSettingByWorkCityIdModel, null, 0, shiftConceptDetailIdYesterDay);

                                //Tomorrow  = روز قبل 
                                if (timeshiftSettingYesterday != null &&

                                    ((timeshiftSettingYesterday.TomorrowShiftConceptId == EnumShiftConcept.Night.Id && firstEntryExitToday.ExitTimeToTimeSpan >= timeshiftSettingYesterday.TomorrowShiftEndTimeToTimeSpan)
                                     || timeshiftSettingYesterday.TomorrowShiftConceptId != EnumShiftConcept.Night.Id))
                                {
                                    // var duration = Utility.GetDurationStartTimeToEndTime(firstEntryExitToday.ExitTimelastEntryExitYesterday.EntryTime);
                                    var startDateTemp = lastEntryExitYesterday.EntryDate.Date + lastEntryExitYesterday.EntryTimeToTimeSpan;
                                    var endDateTemp = firstEntryExitToday.ExitDate.Date + firstEntryExitToday.ExitTimeToTimeSpan;
                                    var duration = (endDateTemp - startDateTemp).TotalMinutes;
                                    if (duration <= timeSettingDataModel.MaximumAttendInMinute)  //چک کردن مدت حضور از روز قبل تا اولین ورود امروز
                                    {
                                        if ((timeshiftSettingYesterday.TomorrowShiftConceptId == EnumShiftConcept.Night.Id
                                           && yesterdayAttendAbsenceItem.Any(x => //x.WorkCalendarId == inputModel.WorkCalendarId &&
                                           x.StartTime.ConvertStringToTimeSpan() >= timeshiftSettingYesterday.TomorrowShiftEndTimeToTimeSpan
                                           && x.EndTime.ConvertStringToTimeSpan() > timeshiftSettingYesterday.TomorrowShiftEndTimeToTimeSpan
                                           && x.RollCallDefinition.RollCallConceptId == EnumRollCallConcept.OverTime.Id) == false)
                                     || (timeshiftSettingYesterday.TomorrowShiftConceptId != EnumShiftConcept.Night.Id
                                           && yesterdayAttendAbsenceItem.Any(x => x.RollCallDefinition.RollCallConceptId == EnumRollCallConcept.OverTime.Id
                                           && ((x.StartTime.ConvertStringToTimeSpan() >= timeshiftSettingYesterday.TomorrowShiftEndTimeToTimeSpan
                                           && x.EndTime.ConvertStringToTimeSpan() < x.StartTime.ConvertStringToTimeSpan() && x.EndTime.ConvertStringToTimeSpan() > timeSettingDataModel.ShiftStartTimeToTimeSpan)
                                           || (x.StartTime.ConvertStringToTimeSpan() < timeSettingDataModel.ShiftStartTimeToTimeSpan && x.EndTime.ConvertStringToTimeSpan() > timeSettingDataModel.ShiftStartTimeToTimeSpan))) == false))
                                        {
                                            foreach (var item in employeeEntryExitYesterdayToTomorrow.TodayList)
                                            {
                                                if (item.RowGuid == firstEntryExitToday.RowGuid)
                                                {
                                                    item.EntryTime = timeSettingDataModel.ShiftStartTime;
                                                    item.EntryTimeToTimeSpan = timeSettingDataModel.ShiftStartTimeToTimeSpan;
                                                    item.EntryDate = timeSettingDataModel.ShiftStartDate;
                                                    continuousShift = true;
                                                    //

                                                    if (timeshiftSettingYesterday.TomorrowShiftConceptId == EnumShiftConcept.Night.Id
                                                        && yesterdayAttendAbsenceItem.Any(x => x.RollCallDefinition.RollCallConceptId == EnumRollCallConcept.OverTime.Id
                                                        && x.StartTime.ConvertStringToTimeSpan() > timeSettingDataModel.ShiftStartTimeToTimeSpan
                                                        && x.StartTime.ConvertStringToTimeSpan() < timeSettingDataModel.ShiftEndTimeToTimeSpan
                                                        ))
                                                    {
                                                        throw new Exception("اضافه کار روز قبل با شروع ساعت کاری امروز تداخل دارد");
                                                    }
                                                    //
                                                    break;
                                                }
                                                //

                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
            //
            //
            if (employeeEntryExitYesterdayToTomorrow.TodayList.Any(x => x.EntryTime != null && x.ExitTime != null))
            {
                entryExitResult = employeeEntryExitYesterdayToTomorrow.TodayList.Where(x => x.EntryTime != null && x.ExitTime != null && x.EntryTime != x.ExitTime).OrderBy(x => x.EntryTime).ToList();
            }
            int rollcallRestId = Utility.GetRollCallIdRest(timeSettingDataModel.ShiftSettingFromShiftboard, timeSettingDataModel.IsHoliday, timeSettingDataModel.DayNumber, timeSettingDataModel.OfficialUnOfficialHolidayFromWorkCalendar, timeSettingDataModel.ShiftConceptIsRest);
            var rollcallRest = rollCallDefinition.FirstOrDefault(x => x.RollCallDefinitionId == rollcallRestId);
            var employeeEducation = _kscHrUnitOfWork.EmployeeEducationTimeRepository.GetActiveEmployeeEducationTimeByEmployeeIdWorkCalendar(inputModel.EmployeeId, inputModel.WorkCalendarId);
            var employeeEducationExist = employeeEducation.Any();
            //  چک کردن روز بعد
            var lastEntryExitToday = employeeEntryExitYesterdayToTomorrow.TodayList.OrderBy(x => x.EntryTime).LastOrDefault();
            bool addTomorrowExitToList = false;
            //
            string exitTime = string.Empty;
            DateTime exitDate = date;

            if (lastEntryExitToday != null && lastEntryExitToday.EntryTime != null && lastEntryExitToday.ExitTime == null)
            {
                bool invalidExitId = false;

                if (employeeEntryExitYesterdayToTomorrow.TomorrowList.Count() != 0)
                {
                    var firstTomorrowList = employeeEntryExitYesterdayToTomorrow.TomorrowList.First();// اولین رکورد روز بعد
                    if (firstTomorrowList.EntryTime == null && firstTomorrowList.ExitTime != null)
                    {
                        var startDateTemp = lastEntryExitToday.EntryDate.Date + lastEntryExitToday.EntryTimeToTimeSpan;
                        var endDateTemp = firstTomorrowList.ExitDate.Date + firstTomorrowList.ExitTimeToTimeSpan;
                        var duration = (endDateTemp - startDateTemp).TotalMinutes;
                        if (duration <= timeSettingDataModel.MaximumAttendInMinute || inputModel.AnalysisType == EnumAnalysisType.Keshik.Id)  //چک کردن مدت حضور از روز قبل تا اولین ورود امروز
                        {
                            if (timeSettingDataModel.ShiftEndDate == date) // تاریخ مجاز خروج شیفت در همان روز باشد
                            {
                                if (timeSettingDataModel.TomorrowIsRestShift || inputModel.AnalysisType == EnumAnalysisType.Keshik.Id) // روز بعد روز استراحتش باشد
                                {
                                    addTomorrowExitToList = true;
                                    exitTime = firstTomorrowList.ExitTime;
                                }
                                else
                                {
                                    // تاریخ مجاز بعد از شیفت در روز بعد باشد
                                    if (timeSettingDataModel.DateAfterShiftEndTime == employeeEntryExitYesterdayToTomorrow.TomorrowDate)

                                    {
                                        if (firstTomorrowList.ExitTimeToTimeSpan <= timeSettingDataModel.TomorrowShiftStartTimeToTimeSpan)
                                        {
                                            addTomorrowExitToList = true;
                                            exitTime = firstTomorrowList.ExitTime;
                                        }
                                        else // زمان پایان را زمان شروع شیفت بعد در نظر میگیریم
                                        {
                                            addTomorrowExitToList = true;
                                            exitTime = timeSettingDataModel.TomorrowShiftStartTime;//زمان شروع شیفت بعد
                                        }

                                    }
                                }
                            }
                            else// تاریخ مجاز خروج شیفت در روز بعد باشد
                            {

                                if (timeSettingDataModel.DateAfterShiftEndTime == employeeEntryExitYesterdayToTomorrow.TomorrowDate)//
                                {

                                    if (firstTomorrowList.ExitTimeToTimeSpan <= timeSettingDataModel.TimeAfterShiftEndTimeToTimeSpan)
                                    {
                                        addTomorrowExitToList = true;
                                        exitTime = firstTomorrowList.ExitTime;
                                    }
                                }
                                else
                                {
                                    addTomorrowExitToList = true;
                                    exitTime = firstTomorrowList.ExitTime;
                                }
                                if (addTomorrowExitToList)
                                {
                                    if (timeSettingDataModel.TomorrowExistEmployeeAttendAbsenceItem && timeSettingDataModel.TomorrowShiftConceptId == EnumShiftConcept.Morning.Id)
                                    {
                                        exitTime = timeSettingDataModel.ShiftEndTime;
                                        invalidExitId = true;
                                    }
                                }
                            }
                        }
                        if (addTomorrowExitToList)
                        {

                            entryExitResult.Add(new EmployeeEntryExitViewModel()
                            {
                                EntryTime = lastEntryExitToday.EntryTime,
                                EntryTimeToTimeSpan = lastEntryExitToday.EntryTimeToTimeSpan,
                                EntryId = lastEntryExitToday.EntryId,
                                ExitTime = exitTime,
                                ExitTimeToTimeSpan = exitTime.ConvertStringToTimeSpan(),
                                ExitId = invalidExitId ? 0 : firstTomorrowList.ExitId,
                                EntryDate = date.Date,
                                ExitDate = employeeEntryExitYesterdayToTomorrow.TomorrowDate.Date,

                            });
                        }
                    }
                }

            }
            if (employeeEntryExitYesterdayToTomorrow.TomorrowList.Count() != 0)
            {
                if (timeSettingDataModel.ShiftEndDate == employeeEntryExitYesterdayToTomorrow.TomorrowDate && entryExitResult.Count() != 0)
                {
                    var tomorrowList = employeeEntryExitYesterdayToTomorrow.TomorrowList.Where(x => x.ExitTime != null && x.EntryTime != null && x.EntryTimeToTimeSpan < timeSettingDataModel.ShiftEndTimeWithToleranceToTimeSpan);
                    entryExitResult.AddRange(tomorrowList.ToList());
                }
                //employeeEntryExitYesterdayToTomorrow.TomorrowList.Where(x => x.EntryTime != null && x.ExitTimeToTimeSpan <= )
            }
            // فیلتر کردن لیست ورود-خروج قبل از آنالیز براساس ورود و خروج
            List<EmployeeEntryExitViewModel> entryExitFilter = new List<EmployeeEntryExitViewModel>();
            entryExitFilter.AddRange(entryExitResult);
            //
            var employeeEntryExitAttendAbsence = _kscHrUnitOfWork.EmployeeEntryExitAttendAbsenceRepository.GetAllAsnotracking();
            if (timeSettingDataModel.ShiftStartDate != timeSettingDataModel.ShiftEndDate)
            {
                foreach (var item in entryExitFilter)
                {
                    if (employeeEntryExitAttendAbsence.Any(x => x.EmployeeEntryExitId == item.EntryId || x.EmployeeEntryExitId == item.ExitId)
                        || (timeSettingDataModel.TomorrowIsRestShift &&
                        item.EntryDate == timeSettingDataModel.ShiftStartDate && item.EntryTimeToTimeSpan <= timeSettingDataModel.ShiftEndTimeToTimeSpan))
                    {
                        entryExitResult.Remove(item);
                    }
                }
            }

            // در صورتیکه تعطیل غیر رسمی باشد و یا اضافه کار مجوز نداشته باشد لیست ورود-خروج را تخلیه مبکنیم
            if (inputModel.AnalysisType != EnumAnalysisType.Keshik.Id)
            {

                if (inputModel.IsOfficialAttend == false) // اداری نباشد
                {
                    if (
                        (timeSettingDataModel.OfficialUnOfficialHolidayFromWorkCalendar == true && timeSettingDataModel.IsUnOfficialHoliday == true
                        && timeSettingDataModel.IsValidOverTimeInUnOfficialHoliday == false)
                        ||
                        (timeSettingDataModel.ShiftSettingFromShiftboard == false && timeSettingDataModel.IsRestShift && timeSettingDataModel.CheckedEmployeeValidOverTime)
                        )
                    {
                        if (!timeSettingDataModel.ValidOverTimeByEmployeeId)
                        {
                            bool hasOnCall = false;
                            var oncall = _kscHrUnitOfWork.OnCall_RequestRepository.GetRequestByEmployeeIdOncallDate(inputModel.EmployeeId, date, EnumWorkFlowStatus.Cancel.Id);
                            if (oncall.Any(x => x.WF_Request.StatusId == EnumWorkFlowStatus.ReferToOfficialManagement.Id))
                            {
                                hasOnCall = true;
                                timeSettingDataModel.InvalidOverTime = false;
                            }
                            if (hasOnCall == false)
                                entryExitResult = new List<EmployeeEntryExitViewModel>();
                        }
                    }
                }
                else
                {
                    if (timeSettingDataModel.OfficialUnOfficialHolidayFromWorkCalendar == true && timeSettingDataModel.IsUnOfficialHoliday == true)
                        timeSettingDataModel.InvalidOverTime = false;
                }
            }
            //
            AddKarkardToAnalysisViewModel addKarkardToAnalysisViewModel = new AddKarkardToAnalysisViewModel() { };
            //
            #region  وورد-خروج نداشته باشد
            // در صورتیکه وورد-خروج نداشته باشد
            if (entryExitResult.Count() == 0)
            {

                if (timeSettingDataModel.IsRestShift)
                {
                    if (rollcallRest == null)
                    {
                        throw new HRBusinessException(Validations.NotFoundId, String.Format(Validations.NotFound, "کد حضور-غیاب استراحت"));
                    }
                    {
                        //
                        addKarkardToAnalysisViewModel = new AddKarkardToAnalysisViewModel()
                        {
                            resultModel = resultModel,
                            startTime = timeSettingDataModel.ShiftStartTime,
                            endTime = timeSettingDataModel.ShiftEndTime,
                            shiftConceptDetailId = inputModel.ShiftConceptDetailId,
                            karKardCode = rollcallRest,
                            entryId = 0,
                            exitId = 0,
                            timeSettingDataModel = timeSettingDataModel,
                            karKardCodeTomorrow = null,
                            startDate = timeSettingDataModel.ShiftStartDate,
                            endDate = timeSettingDataModel.ShiftEndDate,
                        };
                        //
                        AddKarkardToAnalysisModel(addKarkardToAnalysisViewModel);
                        //
                        //آموزش داشته باشد
                        if (employeeEducationExist)
                        {
                            foreach (var train in employeeEducation)
                            {
                                var rollCallTraining = GetRollCallTraining(timeSettingDataModel, rollCallDefinition, train, false, true);
                                if (rollCallTraining != null)
                                {
                                    AddOverTimeTrainingToAnalysisModel(resultModel, train.StartTime, train.EndTime, inputModel.ShiftConceptDetailId, rollCallTraining, 0, 0, date);
                                }
                            }
                        }
                        //
                        //  return resultModel;
                        analysisResultModel.EmployeeAttendAbsenceAnalysisModel.AddRange(resultModel);
                        return analysisResultModel;
                    }
                }
                else
                {
                    if (!(employeeEntryExitYesterdayToTomorrow.TodayList.Count() != 0 && employeeEntryExitYesterdayToTomorrow.TodayList.OrderBy(x => x.EntryTime).Last().ExitTime == null
                        && employeeEntryExitYesterdayToTomorrow.TomorrowList.Count() != 0 && employeeEntryExitYesterdayToTomorrow.TomorrowList.First().EntryTime == null))
                    {
                        bool addToKarkard = false;
                        //آموزش داشته باشد
                        if (employeeEducationExist)
                        {
                            foreach (var train in employeeEducation)
                            {
                                var rollCallTrainingKarkard = GetRollCallTraining(timeSettingDataModel, rollCallDefinition, train, true, false);
                                //if (rollCallTraining != null)
                                //{
                                // آموزش در محدوده شیفت کاری باشد 
                                if (
                                    train.StartTime.ConvertStringToTimeSpan() < timeSettingDataModel.TrainingEndTimeToTimeSpan
                                    && train.StartTime.ConvertStringToTimeSpan() >= timeSettingDataModel.ShiftStartTimeToTimeSpan
                                    && train.EndTime.ConvertStringToTimeSpan() <= timeSettingDataModel.ShiftEndTimeToTimeSpan)

                                {
                                    if (rollCallTrainingKarkard != null)
                                    {

                                        if (addToKarkard == false)
                                        {
                                            addKarkardToAnalysisViewModel = new AddKarkardToAnalysisViewModel()
                                            {
                                                resultModel = resultModel,
                                                startTime = timeSettingDataModel.ShiftStartTime,
                                                endTime = timeSettingDataModel.ShiftEndTime,
                                                shiftConceptDetailId = inputModel.ShiftConceptDetailId,
                                                karKardCode = rollCallTrainingKarkard,
                                                entryId = 0,
                                                exitId = 0,
                                                timeSettingDataModel = timeSettingDataModel,
                                                karKardCodeTomorrow = null,
                                                startDate = timeSettingDataModel.ShiftStartDate,
                                                endDate = timeSettingDataModel.ShiftEndDate
                                            };
                                            AddKarkardToAnalysisModel(addKarkardToAnalysisViewModel);
                                            addToKarkard = true;
                                        }
                                    }
                                }
                                else   // آموزش در محدوده شیفت کاری نباشد 
                                {
                                    var rollCallTrainingOverTime = GetRollCallTraining(timeSettingDataModel, rollCallDefinition, train, false, true);
                                    if ((train.StartTime.ConvertStringToTimeSpan() >= timeSettingDataModel.ShiftEndTimeToTimeSpan
                                        || train.EndTime.ConvertStringToTimeSpan() > timeSettingDataModel.ShiftEndTimeToTimeSpan)
                                             && rollCallTrainingOverTime != null)
                                    {
                                        string startTime = train.StartTime;
                                        if (train.StartTime.ConvertStringToTimeSpan() < timeSettingDataModel.ShiftEndTimeToTimeSpan) // زمان شروع کلاس قبل از اتمام شیفت باشد
                                        {
                                            startTime = timeSettingDataModel.ShiftEndTime;
                                        }
                                        AddOverTimeTrainingToAnalysisModel(resultModel, startTime, train.EndTime, inputModel.ShiftConceptDetailId, rollCallTrainingOverTime, 0, 0, date);
                                    }
                                }
                                //

                                //}
                            }
                            if (addToKarkard == false)
                            {
                                AddDailyAbcenseToAnalysisModel(resultModel, timeSettingDataModel.ShiftStartTime, timeSettingDataModel.ShiftEndTime, timeSettingDataModel.WorkDayDuration, inputModel.ShiftConceptDetailId, timeSettingDataModel.ShiftStartDate, timeSettingDataModel.ShiftEndDate);
                            }
                        }
                        else
                        {

                            AddDailyAbcenseToAnalysisModel(resultModel, timeSettingDataModel.ShiftStartTime, timeSettingDataModel.ShiftEndTime, timeSettingDataModel.WorkDayDuration, inputModel.ShiftConceptDetailId, timeSettingDataModel.ShiftStartDate, timeSettingDataModel.ShiftEndDate);
                        }
                        //
                        analysisResultModel.EmployeeAttendAbsenceAnalysisModel.AddRange(resultModel);
                        return analysisResultModel;
                        // return resultModel;
                    }
                    else
                    {
                        AddDailyAbcenseToAnalysisModel(resultModel, timeSettingDataModel.ShiftStartTime, timeSettingDataModel.ShiftEndTime, timeSettingDataModel.WorkDayDuration, inputModel.ShiftConceptDetailId, timeSettingDataModel.ShiftStartDate, timeSettingDataModel.ShiftEndDate);
                        analysisResultModel.EmployeeAttendAbsenceAnalysisModel.AddRange(resultModel);
                        return analysisResultModel;
                    }
                }
            }
            #endregion
            //
            //var lastEntryExitToday = employeeEntryExitYesterdayToTomorrow.TodayList.OrderBy(x => x.EntryTime).LastOrDefault();
            //bool addTomorrowExitToList = false;
            //

            #region rollCallDefinition
            //var rollCallDefinition = _rollCallDefinitionService.GetRollCallDefinitionForEmployeeAttendAbsenceFromInputList(allRollCallDefinition, timeSettingDataModel.WorkTimeId, timeSettingDataModel.WorkDayTypeId, employee.EmploymentTypeId, date);
            // var rollCallDefinitionTomorrow = _rollCallDefinitionService.GetRollCallDefinitionForEmployeeAttendAbsenceFromInputList(allRollCallDefinition, timeSettingDataModel.TomorrowWorkTimeId, timeSettingDataModel.TomorrowWorkDayTypeId, employee.EmploymentTypeId, timeSettingDataModel.TomorrowDateTime);
            var rollCallDefinitionTomorrow = allRollCall.RollCallTomorrow;
            var vacationHour = rollCallDefinition.FirstOrDefault(x => x.RollCallConceptId == EnumRollCallConcept.Absence.Id && x.RollCallCategoryId == EnumRollCallCategory.VacationHours.Id);
            if (vacationHour == null)
            {
                // throw new HRBusinessException(Validations.NotFoundId, String.Format(Validations.NotFound, EnumRollCallCategory.VacationHours.Name));
            }
            var hourlyAbsence = rollCallDefinition.FirstOrDefault(x => x.RollCallConceptId == EnumRollCallConcept.Absence.Id && x.RollCallCategoryId == EnumRollCallCategory.HourlyAbsence.Id);
            if (hourlyAbsence == null)
            {
                //throw new HRBusinessException(Validations.NotFoundId, String.Format(Validations.NotFound, EnumRollCallCategory.HourlyAbsence.Name));
            }
            var karKardCode = rollCallDefinition.FirstOrDefault(x => x.RollCallConceptId == EnumRollCallConcept.Attend.Id && x.RollCallCategoryId == EnumRollCallCategory.Karkard.Id);
            if (karKardCode == null)
            {
                if (timeSettingDataModel.IsOfficialHoliday)
                {
                    karKardCode = rollCallDefinition.FirstOrDefault(x => x.RollCallConceptId == EnumRollCallConcept.Attend.Id && x.RollCallCategoryId == EnumRollCallCategory.KarkardInOfficialHoliday.Id);

                }
                else
                     if (timeSettingDataModel.IsUnOfficialHoliday)
                {
                    karKardCode = rollCallDefinition.FirstOrDefault(x => x.RollCallConceptId == EnumRollCallConcept.Attend.Id && x.RollCallCategoryId == EnumRollCallCategory.KarkardInUnOfficialHoliday.Id);
                }
            }

            var karKardCodeTomorrow = rollCallDefinitionTomorrow.FirstOrDefault(x => x.RollCallConceptId == EnumRollCallConcept.Attend.Id && x.RollCallCategoryId == EnumRollCallCategory.Karkard.Id);

            if (karKardCodeTomorrow == null)
            {
                if (timeSettingDataModel.TomorrowIsOfficialHoliday)
                {
                    karKardCodeTomorrow = rollCallDefinitionTomorrow.FirstOrDefault(x => x.RollCallConceptId == EnumRollCallConcept.Attend.Id && x.RollCallCategoryId == EnumRollCallCategory.KarkardInOfficialHoliday.Id);

                }
                else
                     if (timeSettingDataModel.TomorrowIsUnOfficialHoliday)
                {
                    karKardCodeTomorrow = rollCallDefinitionTomorrow.FirstOrDefault(x => x.RollCallConceptId == EnumRollCallConcept.Attend.Id && x.RollCallCategoryId == EnumRollCallCategory.KarkardInUnOfficialHoliday.Id);
                }
            }
            // 
            var compatibleRollCallAddNewRowInTimeSheet = _kscHrUnitOfWork.CompatibleRollCallRepository.GetCompatibleRollCallByCompatibleTypeAsNoTracking(EnumCompatibleRollCallType.AddNewRowInTimeSheet.Id).ToList();
            List<RollCallModelForEmployeeAttendAbsenceModel> karKardToOverTimeToday = new List<RollCallModelForEmployeeAttendAbsenceModel>();
            List<RollCallModelForEmployeeAttendAbsenceModel> karKardToOverTimeTomorrow = new List<RollCallModelForEmployeeAttendAbsenceModel>();
            if (karKardCode != null && timeSettingDataModel.IsRestShift == false)
            {
                karKardToOverTimeToday = GetCompatibleRollCallAddNewRowInTimeSheet(compatibleRollCallAddNewRowInTimeSheet, karKardCode.RollCallDefinitionId, timeSettingDataModel.DayNumber, timeSettingDataModel.WorkDayTypeId, rollCallDefinition);
            }
            if (karKardCodeTomorrow != null)// && timeSettingDataModel.TomorrowIsRestShift == false)
            {

                karKardToOverTimeTomorrow = GetCompatibleRollCallAddNewRowInTimeSheet(compatibleRollCallAddNewRowInTimeSheet, karKardCodeTomorrow.RollCallDefinitionId, timeSettingDataModel.TomorrowDayNumber, timeSettingDataModel.TomorrowWorkDayTypeId, rollCallDefinitionTomorrow);
            }
            //


            //
            var defaultOverTime = rollCallDefinition.FirstOrDefault(x => x.RollCallConceptId == EnumRollCallConcept.OverTime.Id && x.RollCallCategoryId == EnumRollCallCategory.DefaultOverTime.Id);
            if (defaultOverTime == null)
            {
                throw new HRBusinessException(Validations.NotFoundId, String.Format(Validations.NotFound, EnumRollCallCategory.DefaultOverTime.Name));
            }
            bool toleranceOption = false;
            //var breastfeddingTolerance = rollCallDefinition.FirstOrDefault(x => x.RollCallConceptId == EnumRollCallConcept.Absence.Id && x.RollCallCategoryId == EnumRollCallCategory.BreastfeddingTolerance.Id);
            //if (breastfeddingTolerance == null)
            //{
            //    //throw new HRBusinessException(Validations.NotFoundId, String.Format(Validations.NotFound, EnumRollCallCategory.BreastfeddingTolerance.Name));
            //}
            var conditionalAbsenceRollCall = rollCallDefinition.FirstOrDefault(x => x.RollCallDefinitionId == conditionalAbsenceRollCallId);

            var sacrificeDelayEntrance = rollCallDefinition.FirstOrDefault(x => x.RollCallDefinitionId == EnumRollCallDefinication.SacrificeDelayEntrance.Id);
            var sacrificeAccelerationExit = rollCallDefinition.FirstOrDefault(x => x.RollCallDefinitionId == EnumRollCallDefinication.SacrificeAccelerationExit.Id);
            #endregion

            #region تنظیمات  فرجه زمانی
            //

            bool sacrificeOption = false;
            string toleranceTimeInStartShift = null;
            string toleranceTimeInEndShift = null;
            DateTime toleranceDateInStartShift = timeSettingDataModel.ShiftStartDate;
            DateTime toleranceDateInEndShift = timeSettingDataModel.ShiftEndDate;
            bool toleranceInStartShift = false;
            bool toleranceInEndShift = false;
            RollCallModelForEmployeeAttendAbsenceModel rollCallToleranceInStartShift = null;
            RollCallModelForEmployeeAttendAbsenceModel rollCallToleranceInEndShift = null;
            var rollCallTemporaryStartDate = rollCallDefinition.FirstOrDefault(x => x.RollCallDefinitionId == timeSettingDataModel.TemporaryRollCallDefinitionStartShift);
            var rollCallTemporaryEndDate = rollCallDefinition.FirstOrDefault(x => x.RollCallDefinitionId == timeSettingDataModel.TemporaryRollCallDefinitionEndShift);
            var rollCallOverTimeTemporaryStartDate = rollCallDefinition.FirstOrDefault(x => x.RollCallDefinitionId == timeSettingDataModel.TemprorayOverTimeRollCallDefinitionStartShift);
            var rollCallOverTimeTemporaryEndDate = rollCallDefinition.FirstOrDefault(x => x.RollCallDefinitionId == timeSettingDataModel.TemprorayOverTimeRollCallDefinitionEndShift);

            //عدم حضور مجاز داشته باشد یا جانباز باشد و تاخیر در ورود و یا تعجیل در خروج جانبازی داشته باشد


            if (!string.IsNullOrEmpty(sacrificeAttendAbsenceTolerance) && (employee.SacrificeOptionSettingId == EnumSacrificeOptionSetting.SacrificeEntrance.Id || employee.SacrificeOptionSettingId == EnumSacrificeOptionSetting.SacrificeExit.Id
            ))
            {
                sacrificeOption = true;
            }
            // if (breastfeddingOption == true || sacrificeOption == true || timeSettingDataModel.IsTemporaryTime == true)
            if (validConditionalAbsence == true || sacrificeOption == true || timeSettingDataModel.IsTemporaryTime == true)
            {

                toleranceOption = true;
                //if (breastfeddingOption == true) //مرخصی شیر داشته باشد
                if (validConditionalAbsence == true) // عدم حضور مجاز داشته باشد 
                {
                    rollCallToleranceInStartShift = conditionalAbsenceRollCall;
                    rollCallToleranceInEndShift = conditionalAbsenceRollCall;
                    // if (employee.IsBreastfeddingInStartShift)
                    if (validConditionalAbsenceInStartShift)
                    {
                        toleranceInStartShift = true;
                        toleranceTimeInStartShift = timeSettingDataModel.ConditionalAbsenceStartTime;
                        toleranceDateInStartShift = timeSettingDataModel.ConditionalAbsenceStartDate;
                        if (string.IsNullOrEmpty(timeSettingDataModel.TemporaryShiftStartTimeReal))
                        {
                            timeSettingDataModel.TemporaryShiftStartTimeReal = timeSettingDataModel.ConditionalAbsenceStartTime;
                            timeSettingDataModel.TemporaryShiftStartTimeRealToTimeSpan = timeSettingDataModel.ConditionalAbsenceStartTimeToTimeSpan;

                        }
                    }
                    else
                    {

                        toleranceInEndShift = true;
                        toleranceTimeInEndShift = timeSettingDataModel.ConditionalAbsenceEndTime;
                        toleranceDateInEndShift = timeSettingDataModel.ConditionalAbsenceEndDate;
                        if (string.IsNullOrEmpty(timeSettingDataModel.TemporaryShiftEndtTimeReal))
                        {
                            timeSettingDataModel.TemporaryShiftEndtTimeReal = timeSettingDataModel.ConditionalAbsenceEndTime;
                            timeSettingDataModel.TemporaryShiftEndtTimeRealToTimeSpan = timeSettingDataModel.ConditionalAbsenceEndTimeToTimeSpan;

                        }
                    }
                }
                else
                {
                    if (sacrificeOption == true)
                    {
                        var sacrificeStartDate = date;
                        var TotalMinutessacrificeToleranceTime = sacrificeAttendAbsenceTolerance.ConvertStringToTimeSpan().TotalMinutes;
                        if (timeSettingDataModel.ShiftStartTimeToTimeSpan.TotalMinutes + TotalMinutessacrificeToleranceTime > 1440)
                        {
                            sacrificeStartDate = sacrificeStartDate.AddDays(1);
                        }
                        //

                        var sacrificeEndDate = timeSettingDataModel.ShiftEndDate;
                        if (timeSettingDataModel.ShiftEndTimeToTimeSpan.TotalMinutes - TotalMinutessacrificeToleranceTime < 0)
                        {
                            sacrificeEndDate = sacrificeEndDate.AddDays(-1);
                        }

                        if (employee.SacrificeOptionSettingId == EnumSacrificeOptionSetting.SacrificeEntrance.Id)
                        {
                            toleranceInStartShift = true;
                            toleranceTimeInStartShift = Utility.GetTimeAfterShiftEnd(timeSettingDataModel.ShiftStartTime, sacrificeAttendAbsenceTolerance);
                            toleranceDateInStartShift = sacrificeStartDate;
                            rollCallToleranceInStartShift = sacrificeDelayEntrance;
                        }
                        else
                        {
                            toleranceTimeInEndShift = Utility.GetTimeBeforeShiftStart(timeSettingDataModel.ShiftEndTime, sacrificeAttendAbsenceTolerance);
                            toleranceDateInEndShift = sacrificeEndDate;
                            rollCallToleranceInEndShift = sacrificeAccelerationExit;
                            toleranceInEndShift = true;

                        }
                    }
                }
                // در صورتیکه زمان موقت داشته باشد و زمان موقت نسبت به فرجه در محدوده زمانی کمتری باشد آنرا جایگزین زمان فرجه میکنیم
                if (timeSettingDataModel.IsTemporaryTime == true)
                {
                    if (timeSettingDataModel.TemporaryShiftStartTime.ConvertStringToTimeSpan() > timeSettingDataModel.ShiftStartTimeToTimeSpan)
                    {
                        if (string.IsNullOrEmpty(toleranceTimeInStartShift) || timeSettingDataModel.TemporaryShiftStartTime.ConvertStringToTimeSpan() > toleranceTimeInStartShift.ConvertStringToTimeSpan())
                        {
                            toleranceTimeInStartShift = timeSettingDataModel.TemporaryShiftStartTime;
                            toleranceInStartShift = true;
                            rollCallToleranceInStartShift = rollCallTemporaryStartDate;
                        }
                    }
                    if (timeSettingDataModel.TemporaryShiftEndTime.ConvertStringToTimeSpan() < timeSettingDataModel.ShiftEndTimeToTimeSpan)
                    {
                        if (string.IsNullOrEmpty(toleranceTimeInEndShift) || timeSettingDataModel.TemporaryShiftEndTime.ConvertStringToTimeSpan() < toleranceTimeInEndShift.ConvertStringToTimeSpan())
                        {
                            toleranceInEndShift = true;
                            toleranceTimeInEndShift = timeSettingDataModel.TemporaryShiftEndTime;
                            toleranceDateInEndShift = timeSettingDataModel.ShiftEndDate;
                            rollCallToleranceInEndShift = rollCallTemporaryEndDate;// rollCallTemporaryEndDate;

                        }
                    }
                }

            }
            #endregion
            //

            if (entryExitResult.Count() != 0)
            {

                if (timeSettingDataModel.IsRestShift) // استراحت شیفت
                {
                    List<TrainingModel> trainingModel = new List<TrainingModel>();
                    //
                    if (rollcallRest == null)
                    {
                        throw new HRBusinessException(Validations.NotFoundId, String.Format(Validations.NotFound, "کد حضور-غیاب استراحت"));
                    }
                    //
                    addKarkardToAnalysisViewModel = new AddKarkardToAnalysisViewModel()
                    {
                        resultModel = resultModel,
                        startTime = timeSettingDataModel.ShiftStartTime,
                        endTime = timeSettingDataModel.ShiftEndTime,
                        shiftConceptDetailId = inputModel.ShiftConceptDetailId,
                        karKardCode = rollcallRest,
                        entryId = 0,
                        exitId = 0,
                        timeSettingDataModel = timeSettingDataModel,
                        karKardCodeTomorrow = null,
                        startDate = timeSettingDataModel.ShiftStartDate,
                        endDate = timeSettingDataModel.ShiftEndDate,
                        karKardToOverTimeToday = karKardToOverTimeToday,
                        karKardToOverTimeTomorrow = karKardToOverTimeTomorrow
                    };
                    AddKarkardToAnalysisModel(addKarkardToAnalysisViewModel);
                    //bool? isRestShiftInShiftBorad = null;
                    //TimeSettingDataModel timeshiftSettingYesterday = null;
                    //int shiftConceptDetailIdYesterDay = 0;
                    EmployeeAttendAbsenceItem employeeAttendAbsenceItemYesterday = null;
                    IQueryable<EmployeeAttendAbsenceItem> employeeAttendAbsenceItemYesterdayData = null;
                    foreach (var item in entryExitResult)
                    {
                        if (timeSettingDataModel.ShiftSettingFromShiftboard && item.EntryTimeToTimeSpan < timeSettingDataModel.ShiftStartTimeToTimeSpan && item.EntryDate == timeSettingDataModel.ShiftStartDate)
                        {
                            // چک کردن روز قبل
                            if (timeshiftSettingYesterday == null)
                            {
                                var workCalendarYesterday = workCalendars.First(x => x.Date == yesterday);
                                employeeAttendAbsenceItemYesterdayData = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetEmployeeAttendAbsenceItemByEmployeeIdWorkCalendarIdNoIncludedAsNoTracking(inputModel.EmployeeId, workCalendarYesterday.WorkCalendarId);
                                employeeAttendAbsenceItemYesterday = employeeAttendAbsenceItemYesterdayData.FirstOrDefault();
                                if (employeeAttendAbsenceItemYesterday != null)
                                    shiftConceptDetailIdYesterDay = employeeAttendAbsenceItemYesterday.ShiftConceptDetailId;
                                timeshiftSettingYesterday = await GetShiftTimeSettingByDate(inputModel.EmployeeId, workCalendarYesterday, timeShiftSettingByWorkCityIdModel, null, 0, shiftConceptDetailIdYesterDay);
                            }
                            //Tomorrow  = روز قبل 
                            if (timeshiftSettingYesterday != null && timeshiftSettingYesterday.TomorrowIsRestShift == false &&
                                item.EntryTimeToTimeSpan < timeshiftSettingYesterday.TomorrowShiftEndTimeToTimeSpan && item.EntryDate == timeshiftSettingYesterday.ShiftEndDate)
                            {

                                if (employeeAttendAbsenceItemYesterdayData.Count() == 1)
                                {
                                    var rollCallDefinitionYesterday = _kscHrUnitOfWork.RollCallDefinitionRepository
                                        .GetById(employeeAttendAbsenceItemYesterday.RollCallDefinitionId);
                                    if (rollCallDefinitionYesterday.RollCallConceptId != EnumRollCallConcept.Absence.Id)
                                        continue;
                                }
                                else
                                    continue; // درصورتیکه روز قبل روزکاریش باشد و زمان ورودش در امروز قبل از شروع شیفت امروز باشد
                            }
                        }
                        var endTime = item.ExitTime;

                        if (inputModel.AnalysisType != EnumAnalysisType.Keshik.Id && item.ExitDate > timeSettingDataModel.ShiftEndDate && !timeSettingDataModel.TomorrowIsRestShift && item.ExitTime.ConvertStringToTimeSpan() > timeSettingDataModel.TomorrowShiftStartTimeToTimeSpan)
                        {
                            endTime = timeSettingDataModel.TomorrowShiftStartTime;
                        }

                        InputAddOverTimeToAnalysisModel inputAddOverTimeToAnalysisModel = new InputAddOverTimeToAnalysisModel()
                        {
                            resultModel = resultModel,
                            startTime = item.EntryTime,
                            endTime = endTime,
                            startDate = item.EntryDate,
                            endDate = item.ExitDate,
                            shiftConceptDetailId = inputModel.ShiftConceptDetailId,
                            overTime = defaultOverTime,
                            entryId = item.EntryId,
                            exitId = item.ExitId,
                            timeSettingDataModel = timeSettingDataModel,
                            allRollCall = allRollCall,
                            analysisInputModel = inputModel
                        };
                        AddOverTimeToAnalysisModel(inputAddOverTimeToAnalysisModel);
                        //AddOverTimeToAnalysisModel(resultModel, item.EntryTime, endTime, item.EntryDate, item.ExitDate, inputModel.ShiftConceptDetailId, defaultOverTime
                        //    , item.EntryId, item.ExitId, timeSettingDataModel, allRollCall);
                    }
                    //}

                }
                // // استراحت نباشد
                else
                {
                    //
                    if (timeSettingDataModel.IsTemporaryTime && toleranceInEndShift) // زمان موقت در پایان شیفت داشته باشد
                    {
                        EditEntryExitResult(entryExitResult, timeSettingDataModel);
                    }
                    //
                    OverTimeAfterShiftViewModel overTimeAfterShiftViewModel = new OverTimeAfterShiftViewModel();
                    bool addToKarkard = false;
                    if (employeeEducationExist)
                    {

                        var attendanceTraining = employeeEducation.ToList().SingleOrDefault(x => x.TrainingTypeId == EnumTrainingType.AttendanceTraining.Id
                        && x.StartTime.ConvertStringToTimeSpan() >= timeSettingDataModel.ShiftStartTimeToTimeSpan
                                    && x.EndTime.ConvertStringToTimeSpan() <= timeSettingDataModel.ShiftEndTimeToTimeSpan
                        );
                        if (attendanceTraining != null
                            && attendanceTraining.StartTime.ConvertStringToTimeSpan() >= timeSettingDataModel.ShiftStartTimeToTimeSpan
                                    && attendanceTraining.EndTime.ConvertStringToTimeSpan() <= timeSettingDataModel.ShiftEndTimeToTimeSpan
                            ) // آموزش حضوری (کارکرد)داشته باشد 
                        {
                            var rollCallTrainingKarkard = GetRollCallTraining(timeSettingDataModel, rollCallDefinition, attendanceTraining, true, false);
                            if (rollCallTrainingKarkard != null && (timeSettingDataModel.ShiftSettingFromShiftboard == false || timeSettingDataModel.ShiftCondeptId == EnumShiftConcept.Morning.Id)) //   در محدوده شیفت معتبر باشد 
                            {
                                addToKarkard = true;
                                //ورود - خروجهای مربوط به اضافه کاری
                                //ورود - خروجهای مربوط به اضافه کاری قبل از شروع شیفت
                                var entryExitForOverTimeBeforTime = entryExitResult.Where(x =>
                                 x.EntryDate == date && x.EntryTimeToTimeSpan < timeSettingDataModel.MinimumShiftStartTimeInMinuteToTimeSpan
                                && ((x.ExitDate == date && x.ExitTimeToTimeSpan <= timeSettingDataModel.ShiftStartTimeToTimeSpan)
                                || timeSettingDataModel.ShiftSettingFromShiftboard));
                                foreach (var item in entryExitForOverTimeBeforTime)
                                {
                                    if (item.ExitDate == date && item.ExitTimeToTimeSpan <= timeSettingDataModel.ShiftStartTimeToTimeSpan)
                                    {

                                        InputAddOverTimeToAnalysisModel inputAddOverTimeToAnalysisModel = new InputAddOverTimeToAnalysisModel()
                                        {
                                            resultModel = resultModel,
                                            startTime = item.EntryTime,
                                            endTime = item.ExitTime,
                                            startDate = item.EntryDate,
                                            endDate = item.ExitDate,
                                            shiftConceptDetailId = inputModel.ShiftConceptDetailId,
                                            overTime = defaultOverTime,
                                            entryId = item.EntryId,
                                            exitId = item.ExitId,
                                            timeSettingDataModel = timeSettingDataModel,
                                            allRollCall = allRollCall,
                                            analysisInputModel = inputModel
                                        };
                                        AddOverTimeToAnalysisModel(inputAddOverTimeToAnalysisModel);
                                        //AddOverTimeToAnalysisModel(resultModel, item.EntryTime, item.ExitTime, item.EntryDate, item.ExitDate, inputModel.ShiftConceptDetailId, defaultOverTime
                                        //   , item.EntryId, item.ExitId, timeSettingDataModel, allRollCall);
                                    }
                                    else
                                    {
                                        if (timeSettingDataModel.ShiftSettingFromShiftboard)
                                        {

                                            InputAddOverTimeToAnalysisModel inputAddOverTimeToAnalysisModel = new InputAddOverTimeToAnalysisModel()
                                            {
                                                resultModel = resultModel,
                                                startTime = item.EntryTime,
                                                endTime = timeSettingDataModel.ShiftStartTime,
                                                startDate = item.EntryDate,
                                                endDate = timeSettingDataModel.ShiftStartDate,
                                                shiftConceptDetailId = inputModel.ShiftConceptDetailId,
                                                overTime = defaultOverTime,
                                                entryId = item.EntryId,
                                                exitId = 0,
                                                timeSettingDataModel = timeSettingDataModel,
                                                allRollCall = allRollCall,
                                                analysisInputModel = inputModel
                                            };
                                            AddOverTimeToAnalysisModel(inputAddOverTimeToAnalysisModel);
                                            //AddOverTimeToAnalysisModel(resultModel, item.EntryTime, timeSettingDataModel.ShiftStartTime, item.EntryDate, timeSettingDataModel.ShiftStartDate
                                            //    , inputModel.ShiftConceptDetailId, defaultOverTime, item.EntryId, 0, timeSettingDataModel, allRollCall);
                                        }
                                    }
                                }
                                //
                                addKarkardToAnalysisViewModel = new AddKarkardToAnalysisViewModel()
                                {
                                    resultModel = resultModel,
                                    startTime = timeSettingDataModel.ShiftStartTime,
                                    endTime = timeSettingDataModel.ShiftEndTime,
                                    shiftConceptDetailId = inputModel.ShiftConceptDetailId,
                                    karKardCode = rollCallTrainingKarkard,
                                    entryId = 0,
                                    exitId = 0,
                                    timeSettingDataModel = timeSettingDataModel,
                                    karKardCodeTomorrow = null,
                                    startDate = timeSettingDataModel.ShiftStartDate,
                                    endDate = timeSettingDataModel.ShiftEndDate,
                                    karKardToOverTimeToday = karKardToOverTimeToday,
                                    karKardToOverTimeTomorrow = karKardToOverTimeTomorrow
                                };
                                AddKarkardToAnalysisModel(addKarkardToAnalysisViewModel);
                                //
                                var entryExitForOverTimeAfterTime = entryExitResult.Where(x => (x.ExitDate == timeSettingDataModel.ShiftEndDate
                                && x.ExitTimeToTimeSpan > timeSettingDataModel.ShiftEndTimeToTimeSpan) || x.ExitDate > timeSettingDataModel.ShiftEndDate);
                                foreach (var item in entryExitForOverTimeAfterTime)
                                {
                                    string entryTime = item.EntryTime;
                                    string endTime = item.ExitTime;
                                    DateTime endDate = item.ExitDate;
                                    var entryId = item.EntryId;
                                    if (
                                        item.EntryDate == timeSettingDataModel.ShiftStartDate &&
                                        item.EntryTimeToTimeSpan >= timeSettingDataModel.MinimumShiftStartTimeInMinuteToTimeSpan
                                        && ((item.EntryDate == timeSettingDataModel.ShiftEndDate && item.EntryTimeToTimeSpan < timeSettingDataModel.ShiftEndTimeToTimeSpan)
                                        || item.EntryDate < timeSettingDataModel.ShiftEndDate))
                                    {
                                        entryTime = timeSettingDataModel.ShiftEndTime;
                                        entryId = 0;
                                    }
                                    //

                                    //

                                    InputAddOverTimeToAnalysisModel inputAddOverTimeToAnalysisModel = new InputAddOverTimeToAnalysisModel()
                                    {
                                        resultModel = resultModel,
                                        startTime = entryTime,
                                        endTime = endTime,
                                        startDate = item.EntryDate,
                                        endDate = endDate,
                                        shiftConceptDetailId = inputModel.ShiftConceptDetailId,
                                        overTime = defaultOverTime,
                                        entryId = entryId,
                                        exitId = item.ExitId,
                                        timeSettingDataModel = timeSettingDataModel,
                                        allRollCall = allRollCall,
                                        analysisInputModel = inputModel
                                    };
                                    AddOverTimeToAnalysisModel(inputAddOverTimeToAnalysisModel);
                                    //AddOverTimeToAnalysisModel(resultModel, entryTime, endTime, item.EntryDate, endDate, inputModel.ShiftConceptDetailId, defaultOverTime, entryId, item.ExitId
                                    //    , timeSettingDataModel, allRollCall);

                                }
                            }

                        }
                    }
                    // چک کردن شناور بودن زمان
                    else
                    {
                        if (employee.FloatTimeSettingId != null
                            && toleranceTimeInStartShift.ConvertStringToTimeSpan() < timeSettingDataModel.FloatTimeFromShiftStart.ConvertStringToTimeSpan())
                        {
                            FloatTimeSettingInAnalysisItem(entryExitResult, timeSettingDataModel);
                        }
                    }
                    //
                    if (employeeEducationExist == false || addToKarkard == false) //آموزش  یا آموزش حضوری مربوط به تاییدکارکرد نداشته باشد
                    {
                        // چک کردن تداخل زمانی 
                        if (continuousShift == false && timeSettingDataModel.ShiftSettingFromShiftboard && timeSettingDataModel.ShiftCondeptId == EnumShiftConcept.Morning.Id && employeeEntryExitYesterdayToTomorrow.YesterdayList.Count() != 0)
                        {
                            //CheckOverlapppingTime(entryExitResult)
                            // چک کردن روز قبل
                            CheckOverlapppingTimeModel checkOverlapppingTimeModel = new CheckOverlapppingTimeModel()
                            {
                                inputModel = inputModel,
                                date = date,
                                timeShiftSettingByWorkCityIdModel = timeShiftSettingByWorkCityIdModel,
                                yesterday = yesterday,
                                workCalendars = workCalendars,
                                timeSettingDataModel = timeSettingDataModel,
                                entryExitResult = entryExitResult,
                                TodayList = employeeEntryExitYesterdayToTomorrow.TodayList
                            };
                            entryExitResult = await CheckOverlapppingTime(checkOverlapppingTimeModel, timeSettingDataModel);
                        }
                        //
                        for (int i = 0; i < entryExitResult.Count(); i++)
                        {

                            #region زمان ورود بعد از ساعت شروع شیفت با تلورانس و قبل از زمان پایان با تلورانس باشد


                            if (
                                (
                                    timeSettingDataModel.ShiftEndDate == date
                                    && entryExitResult[i].EntryTimeToTimeSpan > timeSettingDataModel.ShiftStartTimeWithToleranceToTimeSpan &&
                                    entryExitResult[i].EntryTimeToTimeSpan < timeSettingDataModel.ShiftEndTimeToTimeSpan
                                )
                              ||
                                (
                                    timeSettingDataModel.ShiftEndDate != date && ((entryExitResult[i].EntryDate != timeSettingDataModel.ShiftEndDate &&
                                    entryExitResult[i].EntryTimeToTimeSpan > timeSettingDataModel.ShiftStartTimeWithToleranceToTimeSpan) ||
                                        (entryExitResult[i].EntryDate == timeSettingDataModel.ShiftEndDate && entryExitResult[i].EntryTimeToTimeSpan < timeSettingDataModel.ShiftEndTimeWithToleranceToTimeSpan))
                                ))
                            {
                                var rollCallDefinitionAbsence = vacationHour;
                                //در صورتیکه مرخصی ساعتی در ابتدای شیفت پذیرفته نباشد،غیبت ساعتی ثبت میشود
                                if (vacationHour.IsValidInShiftStart == false)
                                    rollCallDefinitionAbsence = hourlyAbsence;
                                else
                                {
                                    if (hourlyAbsence != null && hourlyAbsence.IsValidInShiftStart && timeSettingDataModel.ShiftSettingFromShiftboard)
                                    {
                                        rollCallDefinitionAbsence = hourlyAbsence;
                                    }
                                }
                                if (resultModel.Count() == 0 || resultModel.Any(x => x.RollCallConceptId == EnumRollCallConcept.Attend.Id) == false)//(i == 0)
                                {

                                    //
                                    exitTime = entryExitResult[i].EntryTime;
                                    exitDate = entryExitResult[i].EntryDate;
                                    if (toleranceOption && toleranceInStartShift) //آپشن فرجه داشته باشد و در ابتدای شیفت باشد
                                    {
                                        if (entryExitResult[i].EntryTimeToTimeSpan >= toleranceTimeInStartShift.ConvertStringToTimeSpan())
                                        {
                                            exitTime = toleranceTimeInStartShift;
                                            if (entryExitResult[i].EntryTimeToTimeSpan > toleranceTimeInStartShift.ConvertStringToTimeSpan())
                                                exitTime = timeSettingDataModel.TemporaryShiftStartTimeReal;//toleranceTimeInStartShift;
                                                                                                            //
                                            addKarkardToAnalysisViewModel = new AddKarkardToAnalysisViewModel()
                                            {
                                                resultModel = resultModel,
                                                startTime = timeSettingDataModel.ShiftStartTime,
                                                endTime = exitTime,
                                                shiftConceptDetailId = inputModel.ShiftConceptDetailId,
                                                karKardCode = rollCallToleranceInStartShift,
                                                entryId = 0,
                                                exitId = 0,
                                                timeSettingDataModel = timeSettingDataModel,
                                                karKardCodeTomorrow = null,
                                                startDate = timeSettingDataModel.ShiftStartDate,
                                                endDate = exitDate,
                                                karKardToOverTimeToday = karKardToOverTimeToday,
                                                karKardToOverTimeTomorrow = karKardToOverTimeTomorrow
                                            };
                                            AddKarkardToAnalysisModel(addKarkardToAnalysisViewModel);
                                            if (entryExitResult[i].EntryTimeToTimeSpan > toleranceTimeInStartShift.ConvertStringToTimeSpan()) // مرخصی یا غیبت ساعتی ثبت میشود
                                            {
                                                //AddHourlyAbcenseToAnalysisModel(resultModel, toleranceTimeInStartShift, entryExitResult[i].EntryTime, inputModel.ShiftConceptDetailId, rollCallDefinitionAbsence, entryExitResult[i].EntryId, 0, entryExitResult[i].EntryDate, entryExitResult[i].EntryDate);
                                                AddHourlyAbcenseToAnalysisModel(resultModel, exitTime, entryExitResult[i].EntryTime, inputModel.ShiftConceptDetailId, rollCallDefinitionAbsence, entryExitResult[i].EntryId, 0, entryExitResult[i].EntryDate, entryExitResult[i].EntryDate);
                                            }
                                        }
                                        else //ورود قبل از آپشن فرجه در شروع شیفت و بعد از تلورانس شروع شیفت باشد
                                        {
                                            if (entryExitResult[i].EntryTimeToTimeSpan > timeSettingDataModel.ShiftStartTimeWithToleranceToTimeSpan)
                                            {
                                                exitTime = toleranceTimeInStartShift;
                                                //
                                                addKarkardToAnalysisViewModel = new AddKarkardToAnalysisViewModel()
                                                {
                                                    resultModel = resultModel,
                                                    startTime = timeSettingDataModel.ShiftStartTime,
                                                    endTime = entryExitResult[i].EntryTime,
                                                    shiftConceptDetailId = inputModel.ShiftConceptDetailId,
                                                    karKardCode = rollCallToleranceInStartShift,
                                                    entryId = 0,
                                                    exitId = entryExitResult[i].EntryId,
                                                    timeSettingDataModel = timeSettingDataModel,
                                                    karKardCodeTomorrow = null,
                                                    startDate = timeSettingDataModel.ShiftStartDate,
                                                    endDate = entryExitResult[i].EntryDate,
                                                    karKardToOverTimeToday = karKardToOverTimeToday,
                                                    karKardToOverTimeTomorrow = karKardToOverTimeTomorrow
                                                };
                                                AddKarkardToAnalysisModel(addKarkardToAnalysisViewModel);
                                            }
                                        }

                                    }
                                    else
                                    {
                                        AddHourlyAbcenseToAnalysisModel(resultModel, timeSettingDataModel.ShiftStartTime, exitTime, inputModel.ShiftConceptDetailId, rollCallDefinitionAbsence, entryExitResult[i].EntryId, 0, timeSettingDataModel.ShiftStartDate, exitDate);
                                    }
                                }

                                // خروج در بازه صحیح خروج باشد 
                                if (entryExitResult[i].ExitDate == timeSettingDataModel.ShiftEndDate && entryExitResult[i].ExitTimeToTimeSpan >= timeSettingDataModel.ShiftEndTimeWithToleranceToTimeSpan
                                    && entryExitResult[i].ExitTimeToTimeSpan < timeSettingDataModel.MinimumOverTimeAfterShiftToTimeSpan
                                    )
                                {
                                    //
                                    string startTime = entryExitResult[i].EntryTime;
                                    if (i == 0 && toleranceOption && toleranceInStartShift) //فرجه  داشته باشد و فرجه  در ابتدای شیفت باشد
                                    {
                                        if (entryExitResult[i].EntryTimeToTimeSpan == toleranceTimeInStartShift.ConvertStringToTimeSpan())
                                        {
                                            startTime = toleranceTimeInStartShift;
                                        }
                                        else
                                        {
                                            //if (entryExitResult[i].EntryTimeToTimeSpan > toleranceTimeInStartShift.ConvertStringToTimeSpan()) // مرخصی یا غیبت ساعتی ثبت میشود
                                            //{
                                            //    AddHourlyAbcenseToAnalysisModel(resultModel, toleranceTimeInStartShift, entryExitResult[i].EntryTime, inputModel.ShiftConceptDetailId, rollCallDefinitionAbsence, entryExitResult[i].EntryId, 0, entryExitResult[i].EntryDate, entryExitResult[i].EntryDate);
                                            //}
                                        }
                                    }
                                    //

                                    exitTime = timeSettingDataModel.ShiftEndTime;
                                    exitDate = timeSettingDataModel.ShiftEndDate;
                                    //
                                    addKarkardToAnalysisViewModel = new AddKarkardToAnalysisViewModel()
                                    {
                                        resultModel = resultModel,
                                        startTime = startTime,
                                        endTime = exitTime,
                                        shiftConceptDetailId = inputModel.ShiftConceptDetailId,
                                        karKardCode = karKardCode,
                                        entryId = entryExitResult[i].EntryId,
                                        exitId = entryExitResult[i].ExitId,
                                        timeSettingDataModel = timeSettingDataModel,
                                        karKardCodeTomorrow = karKardCodeTomorrow,
                                        startDate = entryExitResult[i].EntryDate,
                                        endDate = exitDate,
                                        karKardToOverTimeToday = karKardToOverTimeToday,
                                        karKardToOverTimeTomorrow = karKardToOverTimeTomorrow
                                    };
                                    AddKarkardToAnalysisModel(addKarkardToAnalysisViewModel);
                                }
                                else
                                {
                                    // خروجش قبل از اتمام شیفت باشد
                                    if ((entryExitResult[i].ExitDate == timeSettingDataModel.ShiftEndDate
                                        && entryExitResult[i].ExitTimeToTimeSpan < timeSettingDataModel.ShiftEndTimeWithToleranceToTimeSpan)
                                        || entryExitResult[i].ExitDate < timeSettingDataModel.ShiftEndDate
                                        )
                                    {
                                        //
                                        string startTime = entryExitResult[i].EntryTime;
                                        var entryId = entryExitResult[i].EntryId;
                                        if (toleranceOption)
                                        {
                                            if (toleranceInStartShift)
                                            {
                                                if (i == 0 && entryExitResult[i].EntryTimeToTimeSpan == toleranceTimeInStartShift.ConvertStringToTimeSpan())
                                                {
                                                    startTime = toleranceTimeInStartShift;
                                                    entryId = 0;
                                                }
                                            }
                                        }
                                        //
                                        addKarkardToAnalysisViewModel = new AddKarkardToAnalysisViewModel()
                                        {
                                            resultModel = resultModel,
                                            startTime = startTime,
                                            endTime = entryExitResult[i].ExitTime,
                                            shiftConceptDetailId = inputModel.ShiftConceptDetailId,
                                            karKardCode = karKardCode,
                                            entryId = entryId,
                                            exitId = entryExitResult[i].ExitId,
                                            timeSettingDataModel = timeSettingDataModel,
                                            karKardCodeTomorrow = karKardCodeTomorrow,
                                            startDate = entryExitResult[i].EntryDate,
                                            endDate = entryExitResult[i].ExitDate,
                                            karKardToOverTimeToday = karKardToOverTimeToday,
                                            karKardToOverTimeTomorrow = karKardToOverTimeTomorrow
                                        };
                                        //
                                        AddKarkardToAnalysisModel(addKarkardToAnalysisViewModel);
                                        if (toleranceOption == false || (toleranceInStartShift && toleranceInEndShift == false)) //  فرجه نداشته باشد و یا در اول شیفت باشد
                                        {
                                            exitTime = timeSettingDataModel.ShiftEndTime;
                                            exitDate = timeSettingDataModel.ShiftEndDate;
                                            long exitId = 0;
                                            if (entryExitResult.Count() > i + 1)
                                            {
                                                // در صورتیکه ورود بعدی قبل از اتمام شیفت فرد باشد
                                                if ((entryExitResult[i + 1].EntryDate == timeSettingDataModel.ShiftEndDate && entryExitResult[i + 1].EntryTimeToTimeSpan < timeSettingDataModel.ShiftEndTimeToTimeSpan)
                                                    || (entryExitResult[i + 1].EntryDate < timeSettingDataModel.ShiftEndDate)
                                                    )
                                                {
                                                    exitTime = entryExitResult[i + 1].EntryTime;
                                                    exitId = entryExitResult[i + 1].EntryId;
                                                    exitDate = entryExitResult[i + 1].EntryDate;
                                                }
                                            }
                                            AddVacationHourToAnalysisModel(resultModel, entryExitResult[i].ExitTime, exitTime, inputModel.ShiftConceptDetailId, vacationHour, entryExitResult[i].ExitId, exitId, entryExitResult[i].ExitDate, exitDate);
                                        }
                                        else
                                        {
                                            if (toleranceInEndShift == true) //فرجه  در پایان شیفت باشد
                                            {
                                                //خروج قبل از فرجه پایان باشد 
                                                if (entryExitResult[i].ExitTimeToTimeSpan < toleranceTimeInEndShift.ConvertStringToTimeSpan())
                                                {

                                                    if (entryExitResult.Count() > i + 1)
                                                    {
                                                        // در صورتیکه ورود بعدی قبل از اتمام شیفت فرد باشد
                                                        if ((entryExitResult[i + 1].EntryDate == timeSettingDataModel.ShiftEndDate && entryExitResult[i + 1].EntryTimeToTimeSpan < timeSettingDataModel.ShiftEndTimeWithToleranceToTimeSpan)
                                                            || (entryExitResult[i + 1].EntryDate < timeSettingDataModel.ShiftEndDate)
                                                            )
                                                        {
                                                            if (entryExitResult[i + 1].EntryTimeToTimeSpan < toleranceTimeInEndShift.ConvertStringToTimeSpan()
                                                                 //&& entryExitResult[i + 1].ExitTimeToTimeSpan < toleranceTimeInEndShift.ConvertStringToTimeSpan()
                                                                 )
                                                            {
                                                                AddVacationHourToAnalysisModel(resultModel, entryExitResult[i].ExitTime, entryExitResult[i + 1].EntryTime, inputModel.ShiftConceptDetailId, vacationHour, entryExitResult[i].ExitId, entryExitResult[i + 1].EntryId, entryExitResult[i].ExitDate, entryExitResult[i + 1].EntryDate);
                                                            }
                                                            else
                                                            {
                                                                if (entryExitResult[i + 1].EntryDate == timeSettingDataModel.ShiftEndDate
                                                                    && ((entryExitResult[i + 1].EntryTimeToTimeSpan > timeSettingDataModel.TemporaryShiftEndtTimeReal.ConvertStringToTimeSpan()
                                                                    && entryExitResult[i + 1].EntryTimeToTimeSpan < timeSettingDataModel.ShiftEndTimeWithToleranceToTimeSpan)
                                                                    || (
                                                                    entryExitResult[i + 1].EntryTimeToTimeSpan >= toleranceTimeInEndShift.ConvertStringToTimeSpan() &&
                                                                    entryExitResult[i + 1].EntryTimeToTimeSpan < timeSettingDataModel.TemporaryShiftEndtTimeReal.ConvertStringToTimeSpan()))
                                                                   )
                                                                {

                                                                    var tempTime = timeSettingDataModel.TemporaryShiftEndtTimeReal;
                                                                    if (entryExitResult[i + 1].EntryTimeToTimeSpan < timeSettingDataModel.TemporaryShiftEndtTimeReal.ConvertStringToTimeSpan())
                                                                    {
                                                                        tempTime = entryExitResult[i + 1].EntryTime;
                                                                    }

                                                                    AddVacationHourToAnalysisModel(resultModel, entryExitResult[i].ExitTime, tempTime, inputModel.ShiftConceptDetailId, vacationHour, entryExitResult[i].ExitId, 0, entryExitResult[i].ExitDate, entryExitResult[i + 1].EntryDate);
                                                                    if (entryExitResult[i + 1].EntryTime != tempTime)
                                                                    {
                                                                        addKarkardToAnalysisViewModel = new AddKarkardToAnalysisViewModel()
                                                                        {
                                                                            resultModel = resultModel,
                                                                            startTime = tempTime,//toleranceTimeInEndShift,
                                                                            endTime = entryExitResult[i + 1].EntryTime,
                                                                            shiftConceptDetailId = inputModel.ShiftConceptDetailId,
                                                                            karKardCode = rollCallToleranceInEndShift,
                                                                            entryId = 0,
                                                                            exitId = entryExitResult[i + 1].EntryId,
                                                                            timeSettingDataModel = timeSettingDataModel,
                                                                            karKardCodeTomorrow = karKardCodeTomorrow,
                                                                            startDate = timeSettingDataModel.ShiftStartDate,
                                                                            endDate = timeSettingDataModel.ShiftEndDate,
                                                                            karKardToOverTimeToday = karKardToOverTimeToday,
                                                                            karKardToOverTimeTomorrow = karKardToOverTimeTomorrow
                                                                        };
                                                                        AddKarkardToAnalysisModel(addKarkardToAnalysisViewModel);
                                                                    }
                                                                }
                                                            }

                                                        }
                                                        else
                                                        {
                                                            if (entryExitResult[i + 1].EntryDate == timeSettingDataModel.ShiftEndDate
                                                                && entryExitResult[i].ExitTimeToTimeSpan < toleranceTimeInEndShift.ConvertStringToTimeSpan()
                                                                && entryExitResult[i + 1].EntryTimeToTimeSpan > timeSettingDataModel.ShiftEndTimeToTimeSpan
                                                                )
                                                            {
                                                                AddVacationHourToAnalysisModel(resultModel, entryExitResult[i].ExitTime, timeSettingDataModel.TemporaryShiftEndtTimeReal, inputModel.ShiftConceptDetailId, vacationHour, entryExitResult[i].ExitId, 0, entryExitResult[i].ExitDate, entryExitResult[i + 1].EntryDate);
                                                                addKarkardToAnalysisViewModel = new AddKarkardToAnalysisViewModel()
                                                                {
                                                                    resultModel = resultModel,
                                                                    startTime = timeSettingDataModel.TemporaryShiftEndtTimeReal,//toleranceTimeInEndShift,
                                                                    endTime = timeSettingDataModel.ShiftEndTime,
                                                                    shiftConceptDetailId = inputModel.ShiftConceptDetailId,
                                                                    karKardCode = rollCallToleranceInEndShift,
                                                                    entryId = 0,
                                                                    exitId = 0,
                                                                    timeSettingDataModel = timeSettingDataModel,
                                                                    karKardCodeTomorrow = karKardCodeTomorrow,
                                                                    startDate = timeSettingDataModel.ShiftStartDate,
                                                                    endDate = timeSettingDataModel.ShiftEndDate,
                                                                    karKardToOverTimeToday = karKardToOverTimeToday,
                                                                    karKardToOverTimeTomorrow = karKardToOverTimeTomorrow
                                                                };
                                                                AddKarkardToAnalysisModel(addKarkardToAnalysisViewModel);
                                                            }
                                                            else
                                                            {
                                                                if (entryExitResult[i + 1].EntryDate == timeSettingDataModel.ShiftEndDate
                                                               && entryExitResult[i].ExitTimeToTimeSpan < toleranceTimeInEndShift.ConvertStringToTimeSpan()
                                                               && entryExitResult[i + 1].EntryTimeToTimeSpan >= timeSettingDataModel.ShiftEndTimeWithToleranceToTimeSpan
                                                               && entryExitResult[i + 1].EntryTimeToTimeSpan <= timeSettingDataModel.ShiftEndTimeToTimeSpan
                                                               )
                                                                {
                                                                    AddVacationHourToAnalysisModel(resultModel, entryExitResult[i].ExitTime, timeSettingDataModel.TemporaryShiftEndtTimeReal, inputModel.ShiftConceptDetailId, vacationHour, entryExitResult[i].ExitId, 0, entryExitResult[i].ExitDate, entryExitResult[i + 1].EntryDate);
                                                                    addKarkardToAnalysisViewModel = new AddKarkardToAnalysisViewModel()
                                                                    {
                                                                        resultModel = resultModel,
                                                                        startTime = timeSettingDataModel.TemporaryShiftEndtTimeReal,//toleranceTimeInEndShift,
                                                                        endTime = entryExitResult[i + 1].EntryTime,
                                                                        shiftConceptDetailId = inputModel.ShiftConceptDetailId,
                                                                        karKardCode = rollCallToleranceInEndShift,
                                                                        entryId = 0,
                                                                        exitId = 0,
                                                                        timeSettingDataModel = timeSettingDataModel,
                                                                        karKardCodeTomorrow = karKardCodeTomorrow,
                                                                        startDate = timeSettingDataModel.ShiftStartDate,
                                                                        endDate = timeSettingDataModel.ShiftEndDate,
                                                                        karKardToOverTimeToday = karKardToOverTimeToday,
                                                                        karKardToOverTimeTomorrow = karKardToOverTimeTomorrow
                                                                    };
                                                                    AddKarkardToAnalysisModel(addKarkardToAnalysisViewModel);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        // افزودن مرخصی ساعتی
                                                        //  AddVacationHourToAnalysisModel(resultModel, entryExitResult[i].ExitTime, toleranceTimeInEndShift, inputModel.ShiftConceptDetailId, vacationHour, entryExitResult[i].ExitId, 0, entryExitResult[i].ExitDate, date);
                                                        AddVacationHourToAnalysisModel(resultModel, entryExitResult[i].ExitTime, timeSettingDataModel.TemporaryShiftEndtTimeReal, inputModel.ShiftConceptDetailId, vacationHour, entryExitResult[i].ExitId, 0, entryExitResult[i].ExitDate, date);
                                                        // افزودن فرجه به کارکرد
                                                        //
                                                        addKarkardToAnalysisViewModel = new AddKarkardToAnalysisViewModel()
                                                        {
                                                            resultModel = resultModel,
                                                            startTime = timeSettingDataModel.TemporaryShiftEndtTimeReal,//toleranceTimeInEndShift,
                                                            endTime = timeSettingDataModel.ShiftEndTime,
                                                            shiftConceptDetailId = inputModel.ShiftConceptDetailId,
                                                            karKardCode = rollCallToleranceInEndShift,
                                                            entryId = 0,
                                                            exitId = 0,
                                                            timeSettingDataModel = timeSettingDataModel,
                                                            karKardCodeTomorrow = karKardCodeTomorrow,
                                                            startDate = timeSettingDataModel.ShiftStartDate,
                                                            endDate = timeSettingDataModel.ShiftEndDate,
                                                            karKardToOverTimeToday = karKardToOverTimeToday,
                                                            karKardToOverTimeTomorrow = karKardToOverTimeTomorrow
                                                        };
                                                        AddKarkardToAnalysisModel(addKarkardToAnalysisViewModel);
                                                    }
                                                }
                                                // خروج بعد از اتمام فرجه و قبل از تلورانس پایان باشد
                                                else
                                                {
                                                    string endTime = timeSettingDataModel.ShiftEndTime;
                                                    if (entryExitResult[i].ExitTimeToTimeSpan < timeSettingDataModel.ShiftEndTimeWithToleranceToTimeSpan)
                                                    {
                                                        //
                                                        if (entryExitResult.Count() > i + 1)
                                                        {
                                                            // در صورتیکه ورود بعدی قبل از اتمام شیفت فرد باشد
                                                            if (entryExitResult[i + 1].EntryDate == timeSettingDataModel.ShiftEndDate
                                                                && entryExitResult[i + 1].EntryTimeToTimeSpan < timeSettingDataModel.ShiftEndTimeWithToleranceToTimeSpan

                                                                )
                                                            {
                                                                endTime = entryExitResult[i + 1].EntryTime;
                                                            }
                                                        }
                                                        //
                                                        //از ورود تا خروج یک کارکرد
                                                        // AddKarkardToAnalysisModel(resultModel, entryExitResult[i].EntryTime, entryExitResult[i].ExitTime, inputModel.ShiftConceptDetailId, karKardCode, entryExitResult[i].EntryId, entryExitResult[i].ExitId, timeSettingDataModel, karKardCodeTomorrow, entryExitResult[i].ExitDate);
                                                        // افزودن فرجه 
                                                        addKarkardToAnalysisViewModel = new AddKarkardToAnalysisViewModel()
                                                        {
                                                            resultModel = resultModel,
                                                            startTime = entryExitResult[i].ExitTime,
                                                            endTime = endTime,//timeSettingDataModel.ShiftEndTime,
                                                            shiftConceptDetailId = inputModel.ShiftConceptDetailId,
                                                            karKardCode = rollCallToleranceInEndShift,
                                                            entryId = entryExitResult[i].ExitId,
                                                            exitId = 0,
                                                            timeSettingDataModel = timeSettingDataModel,
                                                            karKardCodeTomorrow = karKardCodeTomorrow,
                                                            startDate = entryExitResult[i].ExitDate,
                                                            endDate = timeSettingDataModel.ShiftEndDate,
                                                            karKardToOverTimeToday = karKardToOverTimeToday,
                                                            karKardToOverTimeTomorrow = karKardToOverTimeTomorrow
                                                        };
                                                        AddKarkardToAnalysisModel(addKarkardToAnalysisViewModel);
                                                    }

                                                }


                                            }
                                        }
                                    }
                                    else //خروج بعد از اتمام شیفت باشد
                                    {
                                        if (entryExitResult[i].ExitDate > timeSettingDataModel.ShiftEndDate
                                            || (entryExitResult[i].ExitDate == timeSettingDataModel.ShiftEndDate
                                            && entryExitResult[i].ExitTimeToTimeSpan >= timeSettingDataModel.MinimumOverTimeAfterShiftToTimeSpan
                                            )
                                            )
                                        {
                                            EmployeeEntryExitViewModel entryExitCurrentRow = entryExitResult[i];
                                            //
                                            overTimeAfterShiftViewModel = new OverTimeAfterShiftViewModel()
                                            {

                                                resultModel = resultModel,
                                                timeSettingDataModel = timeSettingDataModel,
                                                entryExitResult = entryExitCurrentRow,
                                                ShiftConceptDetailId = inputModel.ShiftConceptDetailId,
                                                karKardCode = karKardCode,
                                                defaultOverTime = defaultOverTime,
                                                allRollCall = allRollCall,
                                                employmentTypeId = employee.EmploymentTypeId,
                                                karKardCodeTomorrow = karKardCodeTomorrow,
                                                karKardToOverTimeToday = karKardToOverTimeToday,
                                                karKardToOverTimeTomorrow = karKardToOverTimeTomorrow,
                                                analysisInputModel = inputModel
                                            };
                                            OverTimeAfterShif(overTimeAfterShiftViewModel);
                                        }
                                    }
                                }
                            }


                            #endregion
                            #region زمان ورود در بازه صحیح  باشد
                            if (entryExitResult[i].EntryTimeToTimeSpan <= timeSettingDataModel.ShiftStartTimeWithToleranceToTimeSpan
                                && entryExitResult[i].EntryTimeToTimeSpan >= timeSettingDataModel.MinimumShiftStartTimeInMinuteToTimeSpan)
                            {
                                // خروجش در بازه صحیح  باشد
                                if (entryExitResult[i].ExitDate == timeSettingDataModel.ShiftEndDate)
                                {
                                    if (entryExitResult[i].ExitTimeToTimeSpan >= timeSettingDataModel.ShiftEndTimeWithToleranceToTimeSpan
                                    && entryExitResult[i].ExitTimeToTimeSpan < timeSettingDataModel.MinimumOverTimeAfterShiftToTimeSpan
                                    )
                                    {
                                        exitTime = timeSettingDataModel.ShiftEndTime;
                                        exitDate = timeSettingDataModel.ShiftEndDate;
                                        //
                                        addKarkardToAnalysisViewModel = new AddKarkardToAnalysisViewModel()
                                        {
                                            resultModel = resultModel,
                                            startTime = timeSettingDataModel.ShiftStartTime,
                                            endTime = exitTime,
                                            shiftConceptDetailId = inputModel.ShiftConceptDetailId,
                                            karKardCode = karKardCode,
                                            entryId = entryExitResult[i].EntryId,
                                            exitId = entryExitResult[i].ExitId,
                                            timeSettingDataModel = timeSettingDataModel,
                                            karKardCodeTomorrow = karKardCodeTomorrow,
                                            startDate = timeSettingDataModel.ShiftStartDate,
                                            endDate = exitDate,
                                            karKardToOverTimeToday = karKardToOverTimeToday,
                                            karKardToOverTimeTomorrow = karKardToOverTimeTomorrow
                                        };
                                        AddKarkardToAnalysisModel(addKarkardToAnalysisViewModel);

                                    }
                                    else
                                    {
                                        // خروجش قبل از اتمام شیفت باشد
                                        if (entryExitResult[i].ExitTimeToTimeSpan < timeSettingDataModel.ShiftEndTimeWithToleranceToTimeSpan)
                                        {
                                            //string startTime = entryExitResult[i].EntryTime;
                                            //DateTime startDate = entryExitResult[i].EntryDate;
                                            //if (i == 0)
                                            //{
                                            string startTime = timeSettingDataModel.ShiftStartTime;
                                            DateTime startDate = timeSettingDataModel.ShiftStartDate;
                                            //}
                                            //
                                            addKarkardToAnalysisViewModel = new AddKarkardToAnalysisViewModel()
                                            {
                                                resultModel = resultModel,
                                                startTime = startTime,
                                                endTime = entryExitResult[i].ExitTime,
                                                shiftConceptDetailId = inputModel.ShiftConceptDetailId,
                                                karKardCode = karKardCode,
                                                entryId = entryExitResult[i].EntryId,
                                                exitId = entryExitResult[i].ExitId,
                                                timeSettingDataModel = timeSettingDataModel,
                                                karKardCodeTomorrow = karKardCodeTomorrow,
                                                startDate = startDate,
                                                endDate = entryExitResult[i].ExitDate,
                                                karKardToOverTimeToday = karKardToOverTimeToday,
                                                karKardToOverTimeTomorrow = karKardToOverTimeTomorrow
                                            };
                                            AddKarkardToAnalysisModel(addKarkardToAnalysisViewModel);
                                            if (toleranceInEndShift == false && (toleranceOption == false || toleranceInStartShift)) //  فرجه نداشته باشد و یا در اول شیفت باشد
                                            {
                                                exitTime = timeSettingDataModel.ShiftEndTime;
                                                exitDate = timeSettingDataModel.ShiftEndDate;
                                                long exitId = 0;
                                                if (entryExitResult.Count() > i + 1)
                                                {
                                                    // در صورتیکه ورود بعدی قبل از اتمام شیفت فرد باشد
                                                    if ((entryExitResult[i + 1].EntryDate == timeSettingDataModel.ShiftEndDate && entryExitResult[i + 1].EntryTimeToTimeSpan < timeSettingDataModel.ShiftEndTimeToTimeSpan)
                                                        || (entryExitResult[i + 1].EntryDate < timeSettingDataModel.ShiftEndDate)
                                                        )
                                                    {
                                                        exitTime = entryExitResult[i + 1].EntryTime;
                                                        exitId = entryExitResult[i + 1].EntryId;
                                                        exitDate = entryExitResult[i + 1].EntryDate;
                                                    }
                                                }
                                                AddVacationHourToAnalysisModel(resultModel, entryExitResult[i].ExitTime, exitTime, inputModel.ShiftConceptDetailId, vacationHour, entryExitResult[i].ExitId, exitId, entryExitResult[i].ExitDate, exitDate);
                                            }
                                            else
                                            {
                                                if (toleranceInEndShift == true) //فرجه  در پایان شیفت باشد
                                                {
                                                    //خروج قبل از فرجه پایان باشد 
                                                    if (entryExitResult[i].ExitTimeToTimeSpan < toleranceTimeInEndShift.ConvertStringToTimeSpan())
                                                    {

                                                        if (entryExitResult.Count() > i + 1)
                                                        {
                                                            // در صورتیکه ورود بعدی قبل از اتمام شیفت فرد باشد
                                                            if ((entryExitResult[i + 1].EntryDate == timeSettingDataModel.ShiftEndDate && entryExitResult[i + 1].EntryTimeToTimeSpan < timeSettingDataModel.ShiftEndTimeWithToleranceToTimeSpan)
                                                                || (entryExitResult[i + 1].EntryDate < timeSettingDataModel.ShiftEndDate)
                                                                )
                                                            {
                                                                if (entryExitResult[i + 1].EntryTimeToTimeSpan < toleranceTimeInEndShift.ConvertStringToTimeSpan()
                                                                     //  && entryExitResult[i + 1].ExitTimeToTimeSpan < toleranceTimeInEndShift.ConvertStringToTimeSpan()
                                                                     )
                                                                {
                                                                    AddVacationHourToAnalysisModel(resultModel, entryExitResult[i].ExitTime, entryExitResult[i + 1].EntryTime, inputModel.ShiftConceptDetailId, vacationHour, entryExitResult[i].ExitId, entryExitResult[i + 1].EntryId, entryExitResult[i].ExitDate, entryExitResult[i + 1].EntryDate);
                                                                }
                                                                else
                                                                {
                                                                    if (entryExitResult[i + 1].EntryDate == timeSettingDataModel.ShiftEndDate
                                                                        && (
                                                                        (entryExitResult[i + 1].EntryTimeToTimeSpan > timeSettingDataModel.TemporaryShiftEndtTimeReal.ConvertStringToTimeSpan()
                                                                       && entryExitResult[i + 1].EntryTimeToTimeSpan < timeSettingDataModel.ShiftEndTimeWithToleranceToTimeSpan)
                                                                       || (
                                                                    entryExitResult[i + 1].EntryTimeToTimeSpan >= toleranceTimeInEndShift.ConvertStringToTimeSpan() &&
                                                                    entryExitResult[i + 1].EntryTimeToTimeSpan < timeSettingDataModel.TemporaryShiftEndtTimeReal.ConvertStringToTimeSpan()))
                                                                       )
                                                                    {
                                                                        var tempTime = timeSettingDataModel.TemporaryShiftEndtTimeReal;
                                                                        if (entryExitResult[i + 1].EntryTimeToTimeSpan < timeSettingDataModel.TemporaryShiftEndtTimeReal.ConvertStringToTimeSpan())
                                                                        {
                                                                            tempTime = entryExitResult[i + 1].EntryTime;
                                                                        }
                                                                        AddVacationHourToAnalysisModel(resultModel, entryExitResult[i].ExitTime, tempTime, inputModel.ShiftConceptDetailId, vacationHour, entryExitResult[i].ExitId, 0, entryExitResult[i].ExitDate, entryExitResult[i + 1].EntryDate);
                                                                        if (entryExitResult[i + 1].EntryTime != tempTime)
                                                                        {
                                                                            addKarkardToAnalysisViewModel = new AddKarkardToAnalysisViewModel()
                                                                            {
                                                                                resultModel = resultModel,
                                                                                startTime = tempTime,//toleranceTimeInEndShift,
                                                                                endTime = entryExitResult[i + 1].EntryTime,
                                                                                shiftConceptDetailId = inputModel.ShiftConceptDetailId,
                                                                                karKardCode = rollCallToleranceInEndShift,
                                                                                entryId = 0,
                                                                                exitId = entryExitResult[i + 1].EntryId,
                                                                                timeSettingDataModel = timeSettingDataModel,
                                                                                karKardCodeTomorrow = karKardCodeTomorrow,
                                                                                startDate = timeSettingDataModel.ShiftStartDate,
                                                                                endDate = timeSettingDataModel.ShiftEndDate,
                                                                                karKardToOverTimeToday = karKardToOverTimeToday,
                                                                                karKardToOverTimeTomorrow = karKardToOverTimeTomorrow
                                                                            };
                                                                            AddKarkardToAnalysisModel(addKarkardToAnalysisViewModel);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if ((entryExitResult[i + 1].EntryDate == timeSettingDataModel.ShiftEndDate
                                                                    && entryExitResult[i + 1].EntryTimeToTimeSpan > timeSettingDataModel.ShiftEndTimeToTimeSpan

                                                                    )
                                                               )
                                                                {
                                                                    // AddVacationHourToAnalysisModel(resultModel, entryExitResult[i].ExitTime, toleranceTimeInEndShift, inputModel.ShiftConceptDetailId, vacationHour, entryExitResult[i].ExitId, 0, entryExitResult[i].ExitDate, date);
                                                                    AddVacationHourToAnalysisModel(resultModel, entryExitResult[i].ExitTime, timeSettingDataModel.TemporaryShiftEndtTimeReal, inputModel.ShiftConceptDetailId, vacationHour, entryExitResult[i].ExitId, 0, entryExitResult[i].ExitDate, date);
                                                                    // افزودن فرجه به کارکرد
                                                                    //
                                                                    addKarkardToAnalysisViewModel = new AddKarkardToAnalysisViewModel()
                                                                    {
                                                                        resultModel = resultModel,
                                                                        startTime = timeSettingDataModel.TemporaryShiftEndtTimeReal,//toleranceTimeInEndShift,
                                                                        endTime = timeSettingDataModel.ShiftEndTime,
                                                                        shiftConceptDetailId = inputModel.ShiftConceptDetailId,
                                                                        karKardCode = rollCallToleranceInEndShift,
                                                                        entryId = 0,
                                                                        exitId = 0,
                                                                        timeSettingDataModel = timeSettingDataModel,
                                                                        karKardCodeTomorrow = karKardCodeTomorrow,
                                                                        startDate = timeSettingDataModel.ShiftStartDate,
                                                                        endDate = timeSettingDataModel.ShiftEndDate,
                                                                        karKardToOverTimeToday = karKardToOverTimeToday,
                                                                        karKardToOverTimeTomorrow = karKardToOverTimeTomorrow
                                                                    };
                                                                    AddKarkardToAnalysisModel(addKarkardToAnalysisViewModel);
                                                                }

                                                            }
                                                        }
                                                        else
                                                        {
                                                            // افزودن مرخصی ساعتی
                                                            // AddVacationHourToAnalysisModel(resultModel, entryExitResult[i].ExitTime, toleranceTimeInEndShift, inputModel.ShiftConceptDetailId, vacationHour, entryExitResult[i].ExitId, 0, entryExitResult[i].ExitDate, date);
                                                            AddVacationHourToAnalysisModel(resultModel, entryExitResult[i].ExitTime, timeSettingDataModel.TemporaryShiftEndtTimeReal, inputModel.ShiftConceptDetailId, vacationHour, entryExitResult[i].ExitId, 0, entryExitResult[i].ExitDate, date);
                                                            // افزودن فرجه به کارکرد
                                                            //
                                                            addKarkardToAnalysisViewModel = new AddKarkardToAnalysisViewModel()
                                                            {
                                                                resultModel = resultModel,
                                                                startTime = timeSettingDataModel.TemporaryShiftEndtTimeReal,//toleranceTimeInEndShift,
                                                                endTime = timeSettingDataModel.ShiftEndTime,
                                                                shiftConceptDetailId = inputModel.ShiftConceptDetailId,
                                                                karKardCode = rollCallToleranceInEndShift,
                                                                entryId = 0,
                                                                exitId = 0,
                                                                timeSettingDataModel = timeSettingDataModel,
                                                                karKardCodeTomorrow = karKardCodeTomorrow,
                                                                startDate = timeSettingDataModel.ShiftStartDate,
                                                                endDate = timeSettingDataModel.ShiftEndDate,
                                                                karKardToOverTimeToday = karKardToOverTimeToday,
                                                                karKardToOverTimeTomorrow = karKardToOverTimeTomorrow
                                                            };
                                                            AddKarkardToAnalysisModel(addKarkardToAnalysisViewModel);
                                                        }
                                                    }
                                                    // خروج بعد از اتمام فرجه و قبل از تلورانس پایان باشد
                                                    else
                                                    {
                                                        if (entryExitResult[i].ExitTimeToTimeSpan < timeSettingDataModel.ShiftEndTimeWithToleranceToTimeSpan)
                                                        {
                                                            //از ورود تا خروج یک کارکرد
                                                            // AddKarkardToAnalysisModel(resultModel, entryExitResult[i].EntryTime, entryExitResult[i].ExitTime, inputModel.ShiftConceptDetailId, karKardCode, entryExitResult[i].EntryId, entryExitResult[i].ExitId, timeSettingDataModel, karKardCodeTomorrow, entryExitResult[i].ExitDate);
                                                            // افزودن فرجه 
                                                            //
                                                            var endtime = timeSettingDataModel.ShiftEndTime;
                                                            if (entryExitResult.Count() > i + 1)
                                                            {
                                                                // در صورتیکه ورود بعدی قبل از اتمام شیفت فرد باشد
                                                                if (entryExitResult[i + 1].EntryDate == timeSettingDataModel.ShiftEndDate
                                                                    && entryExitResult[i + 1].EntryTimeToTimeSpan < timeSettingDataModel.ShiftEndTimeWithToleranceToTimeSpan

                                                                    )
                                                                {
                                                                    endtime = entryExitResult[i + 1].EntryTime;
                                                                }
                                                            }
                                                            //
                                                            addKarkardToAnalysisViewModel = new AddKarkardToAnalysisViewModel()
                                                            {
                                                                resultModel = resultModel,
                                                                startTime = entryExitResult[i].ExitTime,
                                                                endTime = endtime,
                                                                shiftConceptDetailId = inputModel.ShiftConceptDetailId,
                                                                karKardCode = rollCallToleranceInEndShift,
                                                                entryId = entryExitResult[i].ExitId,
                                                                exitId = 0,
                                                                timeSettingDataModel = timeSettingDataModel,
                                                                karKardCodeTomorrow = karKardCodeTomorrow,
                                                                startDate = entryExitResult[i].ExitDate,
                                                                endDate = timeSettingDataModel.ShiftEndDate,
                                                                karKardToOverTimeToday = karKardToOverTimeToday,
                                                                karKardToOverTimeTomorrow = karKardToOverTimeTomorrow
                                                            };
                                                            AddKarkardToAnalysisModel(addKarkardToAnalysisViewModel);
                                                        }

                                                    }


                                                }
                                            }
                                            //long exitId = 0;
                                            //if (entryExitResult.Count() > i + 1)
                                            //{
                                            //    if (entryExitResult[i + 1].EntryDate > timeSettingDataModel.ShiftEndDate
                                            //        || (entryExitResult[i + 1].EntryDate == timeSettingDataModel.ShiftEndDate &&
                                            //        entryExitResult[i + 1].EntryTimeToTimeSpan >= timeSettingDataModel.ShiftEndTimeToTimeSpan))
                                            //    {
                                            //        exitTime = timeSettingDataModel.ShiftEndTime;
                                            //        exitId = 0;
                                            //        exitDate = timeSettingDataModel.ShiftEndDate;
                                            //    }
                                            //    else
                                            //    {
                                            //        exitTime = entryExitResult[i + 1].EntryTime;
                                            //        exitId = entryExitResult[i + 1].EntryId;
                                            //        exitDate = entryExitResult[i + 1].EntryDate;
                                            //    }
                                            //}
                                            //else
                                            //{
                                            //    exitTime = timeSettingDataModel.ShiftEndTime;
                                            //    exitDate = timeSettingDataModel.ShiftEndDate;
                                            //}
                                            //AddVacationHourToAnalysisModel(resultModel, entryExitResult[i].ExitTime, exitTime, inputModel.ShiftConceptDetailId, vacationHour, entryExitResult[i].ExitId, exitId, entryExitResult[i].ExitDate, exitDate);

                                        }

                                        else //خروج بعد از اتمام شیفت باشد
                                        {
                                            if (entryExitResult[i].ExitTimeToTimeSpan >= timeSettingDataModel.MinimumOverTimeAfterShiftToTimeSpan)
                                            {
                                                entryExitResult[i].EntryTime = timeSettingDataModel.ShiftStartTime;
                                                overTimeAfterShiftViewModel = new OverTimeAfterShiftViewModel()
                                                {

                                                    resultModel = resultModel,
                                                    timeSettingDataModel = timeSettingDataModel,
                                                    entryExitResult = entryExitResult[i],
                                                    ShiftConceptDetailId = inputModel.ShiftConceptDetailId,
                                                    karKardCode = karKardCode,
                                                    defaultOverTime = defaultOverTime,
                                                    allRollCall = allRollCall,
                                                    employmentTypeId = employee.EmploymentTypeId,
                                                    karKardCodeTomorrow = karKardCodeTomorrow,
                                                    karKardToOverTimeToday = karKardToOverTimeToday,
                                                    karKardToOverTimeTomorrow = karKardToOverTimeTomorrow,
                                                    analysisInputModel = inputModel
                                                };
                                                OverTimeAfterShif(overTimeAfterShiftViewModel);
                                            }
                                        }

                                    }
                                }
                                else //خروج بعد از اتمام شیفت و در روز بعد باشد
                                {
                                    if (entryExitResult[i].ExitDate > timeSettingDataModel.ShiftEndDate)
                                    {
                                        entryExitResult[i].EntryTime = timeSettingDataModel.ShiftStartTime;
                                        overTimeAfterShiftViewModel = new OverTimeAfterShiftViewModel()
                                        {

                                            resultModel = resultModel,
                                            timeSettingDataModel = timeSettingDataModel,
                                            entryExitResult = entryExitResult[i],
                                            ShiftConceptDetailId = inputModel.ShiftConceptDetailId,
                                            karKardCode = karKardCode,
                                            defaultOverTime = defaultOverTime,
                                            allRollCall = allRollCall,
                                            employmentTypeId = employee.EmploymentTypeId,
                                            karKardCodeTomorrow = karKardCodeTomorrow,
                                            karKardToOverTimeToday = karKardToOverTimeToday,
                                            karKardToOverTimeTomorrow = karKardToOverTimeTomorrow,
                                            analysisInputModel = inputModel
                                        };
                                        OverTimeAfterShif(overTimeAfterShiftViewModel);
                                    }
                                    else
                                    {
                                        if (timeSettingDataModel.ShiftEndDate.Date != timeSettingDataModel.ShiftStartDate.Date
                                            && entryExitResult[i].ExitDate.Date == timeSettingDataModel.ShiftStartDate.Date)
                                        {
                                            if (entryExitResult[i].ExitTimeToTimeSpan > entryExitResult[i].EntryTimeToTimeSpan)
                                            {
                                                var startTime = timeSettingDataModel.ShiftStartTime;
                                                //
                                                addKarkardToAnalysisViewModel = new AddKarkardToAnalysisViewModel()
                                                {
                                                    resultModel = resultModel,
                                                    startTime = startTime,
                                                    endTime = entryExitResult[i].ExitTime,
                                                    shiftConceptDetailId = inputModel.ShiftConceptDetailId,
                                                    karKardCode = karKardCode,
                                                    entryId = 0,
                                                    exitId = entryExitResult[i].ExitId,
                                                    timeSettingDataModel = timeSettingDataModel,
                                                    karKardCodeTomorrow = karKardCodeTomorrow,
                                                    startDate = timeSettingDataModel.ShiftStartDate,
                                                    endDate = entryExitResult[i].ExitDate,
                                                    karKardToOverTimeToday = karKardToOverTimeToday,
                                                    karKardToOverTimeTomorrow = karKardToOverTimeTomorrow
                                                };
                                                AddKarkardToAnalysisModel(addKarkardToAnalysisViewModel);
                                                long exitId = 0;
                                                if (entryExitResult.Count() > i + 1)
                                                {
                                                    exitTime = entryExitResult[i + 1].EntryTime;
                                                    exitId = entryExitResult[i + 1].EntryId;
                                                    exitDate = entryExitResult[i + 1].EntryDate;
                                                }
                                                else
                                                {
                                                    exitTime = timeSettingDataModel.ShiftEndTime;
                                                    exitDate = timeSettingDataModel.ShiftEndDate;
                                                }
                                                AddVacationHourToAnalysisModel(resultModel, entryExitResult[i].ExitTime, exitTime, inputModel.ShiftConceptDetailId, vacationHour, entryExitResult[i].ExitId, exitId, entryExitResult[i].ExitDate, exitDate);

                                            }
                                        }
                                    }
                                }
                            }

                            #endregion
                            #region زمان ورود قبل از شروع شیفت کاربر باشد
                            if (entryExitResult[i].EntryDate == timeSettingDataModel.ShiftStartDate && entryExitResult[i].EntryTimeToTimeSpan < timeSettingDataModel.MinimumShiftStartTimeInMinuteToTimeSpan)
                            {
                                //زمان خروج قبل از شروع شیفت کاربر باشد
                                if (entryExitResult[i].ExitDate == timeSettingDataModel.ShiftStartDate && entryExitResult[i].ExitTimeToTimeSpan <= timeSettingDataModel.ShiftStartTimeToTimeSpan)
                                {
                                    InputAddOverTimeToAnalysisModel inputAddOverTimeToAnalysisModel = new InputAddOverTimeToAnalysisModel()
                                    {
                                        resultModel = resultModel,
                                        startTime = entryExitResult[i].EntryTime,
                                        endTime = entryExitResult[i].ExitTime,
                                        startDate = entryExitResult[i].EntryDate,
                                        endDate = entryExitResult[i].ExitDate,
                                        shiftConceptDetailId = inputModel.ShiftConceptDetailId,
                                        overTime = defaultOverTime,
                                        entryId = entryExitResult[i].EntryId,
                                        exitId = entryExitResult[i].ExitId,
                                        timeSettingDataModel = timeSettingDataModel,
                                        allRollCall = allRollCall,
                                        analysisInputModel = inputModel
                                    };
                                    //چک کردن حداقل اضافه کاری قبل شروع شیفت
                                    var validAddOverTimeBeforeShift = ValidAddOverTimeBeforeShift(inputAddOverTimeToAnalysisModel, timeSettingDataModel);
                                    if (validAddOverTimeBeforeShift)
                                    {
                                        AddOverTimeToAnalysisModel(inputAddOverTimeToAnalysisModel);
                                    }
                                    //
                                    //AddOverTimeToAnalysisModel(resultModel, entryExitResult[i].EntryTime, entryExitResult[i].ExitTime, entryExitResult[i].EntryDate
                                    //    , entryExitResult[i].ExitDate, inputModel.ShiftConceptDetailId, defaultOverTime, entryExitResult[i].EntryId, entryExitResult[i].ExitId, timeSettingDataModel, allRollCall);
                                }
                                else // اضافه کاری پیوسته تا بعد از شروع شیفت که فقط برای شیفتیها قابل قبول است
                                {
                                    if (timeSettingDataModel.ShiftSettingFromShiftboard)
                                    {

                                        //
                                        //افزودن اضافه کار
                                        if (entryExitResult[i].EntryTime.ConvertStringToTimeSpan() <= timeSettingDataModel.MinimumOverTimeBeforeShiftInMinutToTimeSpan)
                                        {
                                            InputAddOverTimeToAnalysisModel inputAddOverTimeToAnalysisModel = new InputAddOverTimeToAnalysisModel()
                                            {
                                                resultModel = resultModel,
                                                startTime = entryExitResult[i].EntryTime,
                                                endTime = timeSettingDataModel.ShiftStartTime,
                                                startDate = entryExitResult[i].EntryDate,
                                                endDate = timeSettingDataModel.ShiftStartDate,
                                                shiftConceptDetailId = inputModel.ShiftConceptDetailId,
                                                overTime = defaultOverTime,
                                                entryId = entryExitResult[i].EntryId,
                                                exitId = 0,
                                                timeSettingDataModel = timeSettingDataModel,
                                                allRollCall = allRollCall,
                                                analysisInputModel = inputModel
                                            };
                                            AddOverTimeToAnalysisModel(inputAddOverTimeToAnalysisModel);
                                        }
                                        //AddOverTimeToAnalysisModel(resultModel, entryExitResult[i].EntryTime, timeSettingDataModel.ShiftStartTime, entryExitResult[i].EntryDate, timeSettingDataModel.ShiftStartDate, inputModel.ShiftConceptDetailId, defaultOverTime, entryExitResult[i].EntryId, 0, timeSettingDataModel, allRollCall);

                                        // خروجش در بازه صحیح  باشد
                                        if (entryExitResult[i].ExitDate == timeSettingDataModel.ShiftEndDate)
                                        {
                                            if (entryExitResult[i].ExitTimeToTimeSpan >= timeSettingDataModel.ShiftEndTimeWithToleranceToTimeSpan
                                            && entryExitResult[i].ExitTimeToTimeSpan < timeSettingDataModel.MinimumOverTimeAfterShiftToTimeSpan
                                            )
                                            {
                                                exitTime = timeSettingDataModel.ShiftEndTime;
                                                exitDate = timeSettingDataModel.ShiftEndDate;
                                                //
                                                addKarkardToAnalysisViewModel = new AddKarkardToAnalysisViewModel()
                                                {
                                                    resultModel = resultModel,
                                                    startTime = timeSettingDataModel.ShiftStartTime,
                                                    endTime = exitTime,
                                                    shiftConceptDetailId = inputModel.ShiftConceptDetailId,
                                                    karKardCode = karKardCode,
                                                    entryId = 0,
                                                    exitId = entryExitResult[i].ExitId,
                                                    timeSettingDataModel = timeSettingDataModel,
                                                    karKardCodeTomorrow = karKardCodeTomorrow,
                                                    startDate = timeSettingDataModel.ShiftStartDate,
                                                    endDate = exitDate,
                                                    karKardToOverTimeToday = karKardToOverTimeToday,
                                                    karKardToOverTimeTomorrow = karKardToOverTimeTomorrow
                                                };
                                                AddKarkardToAnalysisModel(addKarkardToAnalysisViewModel);

                                            }
                                            else
                                            {
                                                // خروجش قبل از اتمام شیفت باشد
                                                if (entryExitResult[i].ExitTimeToTimeSpan < timeSettingDataModel.ShiftEndTimeWithToleranceToTimeSpan)
                                                {
                                                    var startTime = timeSettingDataModel.ShiftStartTime;
                                                    //
                                                    addKarkardToAnalysisViewModel = new AddKarkardToAnalysisViewModel()
                                                    {
                                                        resultModel = resultModel,
                                                        startTime = startTime,
                                                        endTime = entryExitResult[i].ExitTime,
                                                        shiftConceptDetailId = inputModel.ShiftConceptDetailId,
                                                        karKardCode = karKardCode,
                                                        entryId = 0,
                                                        exitId = entryExitResult[i].ExitId,
                                                        timeSettingDataModel = timeSettingDataModel,
                                                        karKardCodeTomorrow = karKardCodeTomorrow,
                                                        startDate = timeSettingDataModel.ShiftStartDate,
                                                        endDate = entryExitResult[i].ExitDate,
                                                        karKardToOverTimeToday = karKardToOverTimeToday,
                                                        karKardToOverTimeTomorrow = karKardToOverTimeTomorrow
                                                    };
                                                    AddKarkardToAnalysisModel(addKarkardToAnalysisViewModel);
                                                    long exitId = 0;
                                                    if (entryExitResult.Count() > i + 1)
                                                    {
                                                        exitTime = entryExitResult[i + 1].EntryTime;
                                                        exitId = entryExitResult[i + 1].EntryId;
                                                        exitDate = entryExitResult[i + 1].EntryDate;
                                                    }
                                                    else
                                                    {
                                                        exitTime = timeSettingDataModel.ShiftEndTime;
                                                        exitDate = timeSettingDataModel.ShiftEndDate;
                                                    }
                                                    AddVacationHourToAnalysisModel(resultModel, entryExitResult[i].ExitTime, exitTime, inputModel.ShiftConceptDetailId, vacationHour, entryExitResult[i].ExitId, exitId, entryExitResult[i].ExitDate, exitDate);

                                                }
                                                else //خروج بعد از اتمام شیفت باشد
                                                {
                                                    if (entryExitResult[i].ExitTimeToTimeSpan >= timeSettingDataModel.MinimumOverTimeAfterShiftToTimeSpan)
                                                    {
                                                        entryExitResult[i].EntryTime = timeSettingDataModel.ShiftStartTime;
                                                        entryExitResult[i].EntryId = 0;
                                                        overTimeAfterShiftViewModel = new OverTimeAfterShiftViewModel()
                                                        {

                                                            resultModel = resultModel,
                                                            timeSettingDataModel = timeSettingDataModel,
                                                            entryExitResult = entryExitResult[i],
                                                            ShiftConceptDetailId = inputModel.ShiftConceptDetailId,
                                                            karKardCode = karKardCode,
                                                            defaultOverTime = defaultOverTime,
                                                            allRollCall = allRollCall,
                                                            employmentTypeId = employee.EmploymentTypeId,
                                                            karKardCodeTomorrow = karKardCodeTomorrow,
                                                            karKardToOverTimeToday = karKardToOverTimeToday,
                                                            karKardToOverTimeTomorrow = karKardToOverTimeTomorrow,
                                                            analysisInputModel = inputModel
                                                        };
                                                        OverTimeAfterShif(overTimeAfterShiftViewModel);
                                                    }
                                                }

                                            }
                                        }
                                        else //خروج بعد از اتمام شیفت و در روز بعد باشد
                                        {
                                            if (entryExitResult[i].ExitDate > timeSettingDataModel.ShiftEndDate)
                                            {
                                                entryExitResult[i].EntryTime = timeSettingDataModel.ShiftStartTime;
                                                entryExitResult[i].EntryId = 0;
                                                overTimeAfterShiftViewModel = new OverTimeAfterShiftViewModel()
                                                {

                                                    resultModel = resultModel,
                                                    timeSettingDataModel = timeSettingDataModel,
                                                    entryExitResult = entryExitResult[i],
                                                    ShiftConceptDetailId = inputModel.ShiftConceptDetailId,
                                                    karKardCode = karKardCode,
                                                    defaultOverTime = defaultOverTime,
                                                    allRollCall = allRollCall,
                                                    employmentTypeId = employee.EmploymentTypeId,
                                                    karKardCodeTomorrow = karKardCodeTomorrow,
                                                    karKardToOverTimeToday = karKardToOverTimeToday,
                                                    karKardToOverTimeTomorrow = karKardToOverTimeTomorrow,
                                                    analysisInputModel = inputModel
                                                };
                                                OverTimeAfterShif(overTimeAfterShiftViewModel);
                                            }
                                            else
                                            {
                                                if (timeSettingDataModel.ShiftEndDate.Date != timeSettingDataModel.ShiftStartDate.Date
                                            && entryExitResult[i].ExitDate.Date == timeSettingDataModel.ShiftStartDate.Date)
                                                {
                                                    if (entryExitResult[i].ExitTimeToTimeSpan > entryExitResult[i].EntryTimeToTimeSpan)
                                                    {
                                                        var startTime = timeSettingDataModel.ShiftStartTime;
                                                        //
                                                        addKarkardToAnalysisViewModel = new AddKarkardToAnalysisViewModel()
                                                        {
                                                            resultModel = resultModel,
                                                            startTime = startTime,
                                                            endTime = entryExitResult[i].ExitTime,
                                                            shiftConceptDetailId = inputModel.ShiftConceptDetailId,
                                                            karKardCode = karKardCode,
                                                            entryId = 0,
                                                            exitId = entryExitResult[i].ExitId,
                                                            timeSettingDataModel = timeSettingDataModel,
                                                            karKardCodeTomorrow = karKardCodeTomorrow,
                                                            startDate = timeSettingDataModel.ShiftStartDate,
                                                            endDate = entryExitResult[i].ExitDate,
                                                            karKardToOverTimeToday = karKardToOverTimeToday,
                                                            karKardToOverTimeTomorrow = karKardToOverTimeTomorrow
                                                        };
                                                        AddKarkardToAnalysisModel(addKarkardToAnalysisViewModel);
                                                        long exitId = 0;
                                                        if (entryExitResult.Count() > i + 1)
                                                        {
                                                            exitTime = entryExitResult[i + 1].EntryTime;
                                                            exitId = entryExitResult[i + 1].EntryId;
                                                            exitDate = entryExitResult[i + 1].EntryDate;
                                                        }
                                                        else
                                                        {
                                                            exitTime = timeSettingDataModel.ShiftEndTime;
                                                            exitDate = timeSettingDataModel.ShiftEndDate;
                                                        }
                                                        AddVacationHourToAnalysisModel(resultModel, entryExitResult[i].ExitTime, exitTime, inputModel.ShiftConceptDetailId, vacationHour, entryExitResult[i].ExitId, exitId, entryExitResult[i].ExitDate, exitDate);

                                                    }
                                                }

                                            }
                                        }
                                    }
                                }
                            }
                            #endregion
                            #region زمان ورود بعد از پایان شیفت باشد
                            // برای شبکارها احتمالا این قسمت اجرا نشود!!!!!!!
                            if (timeSettingDataModel.ShiftStartDate == timeSettingDataModel.ShiftEndDate && entryExitResult[i].EntryTimeToTimeSpan >= timeSettingDataModel.ShiftEndTimeToTimeSpan)
                            {
                                if (inputModel.AnalysisType == EnumAnalysisType.Keshik.Id)
                                {
                                    InputAddOverTimeToAnalysisModel inputAddOverTimeToAnalysisModel = new InputAddOverTimeToAnalysisModel()
                                    {
                                        resultModel = resultModel,
                                        startTime = entryExitResult[i].EntryTime,
                                        endTime = entryExitResult[i].ExitTime,
                                        startDate = entryExitResult[i].EntryDate,
                                        endDate = entryExitResult[i].ExitDate,
                                        shiftConceptDetailId = inputModel.ShiftConceptDetailId,
                                        overTime = defaultOverTime,
                                        entryId = entryExitResult[i].EntryId,
                                        exitId = entryExitResult[i].ExitId,
                                        timeSettingDataModel = timeSettingDataModel,
                                        allRollCall = allRollCall,
                                        analysisInputModel = inputModel
                                    };
                                    AddOverTimeToAnalysisModel(inputAddOverTimeToAnalysisModel);
                                }
                                else
                                {
                                    if ((entryExitResult[i].ExitDate == timeSettingDataModel.ShiftStartDate) || timeSettingDataModel.TomorrowIsRestShift
                                        || (entryExitResult[i].ExitDate == timeSettingDataModel.TomorrowDateTime
                                            && entryExitResult[i].ExitTimeToTimeSpan <= timeSettingDataModel.TomorrowShiftStartTimeToTimeSpan)
                                        ) //خروجش در همان روز باشد یا روز بعد ،روز استراحتش باشد
                                    {

                                        InputAddOverTimeToAnalysisModel inputAddOverTimeToAnalysisModel = new InputAddOverTimeToAnalysisModel()
                                        {
                                            resultModel = resultModel,
                                            startTime = entryExitResult[i].EntryTime,
                                            endTime = entryExitResult[i].ExitTime,
                                            startDate = entryExitResult[i].EntryDate,
                                            endDate = entryExitResult[i].ExitDate,
                                            shiftConceptDetailId = inputModel.ShiftConceptDetailId,
                                            overTime = defaultOverTime,
                                            entryId = entryExitResult[i].EntryId,
                                            exitId = entryExitResult[i].ExitId,
                                            timeSettingDataModel = timeSettingDataModel,
                                            allRollCall = allRollCall,
                                            analysisInputModel = inputModel
                                        };
                                        AddOverTimeToAnalysisModel(inputAddOverTimeToAnalysisModel);
                                        //AddOverTimeToAnalysisModel(resultModel, entryExitResult[i].EntryTime, entryExitResult[i].ExitTime, entryExitResult[i].EntryDate, entryExitResult[i].ExitDate, inputModel.ShiftConceptDetailId, defaultOverTime, entryExitResult[i].EntryId, entryExitResult[i].ExitId, timeSettingDataModel, allRollCall);
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                    //


                }
                // بررسی اضافه کار با آموزش 
                if (employeeEducationExist)
                {
                    List<TrainingModel> trainingModel = new List<TrainingModel>();
                    List<TrainingModel> trainingModelOverTime = new List<TrainingModel>();
                    var compatibleRollCallId = _kscHrUnitOfWork.CompatibleRollCallRepository.GetCompatibleRollCallByCompatibleRollCallType(EnumCompatibleRollCallType.AddNewRowInTimeSheet.Id).Select(x => x.CompatibleRollCallId).ToList();

                    var resultModelOverTime = resultModel.Where(x => x.RollCallConceptId == EnumRollCallConcept.OverTime.Id
                    && ((x.StartDate.Date == date.Date && !karKardToOverTimeToday.Any(c => c.RollCallDefinitionId == x.RollCallDefinicationInItemModel.Id))
                       || (x.StartDate.Date == tomorrow.Date && !karKardToOverTimeTomorrow.Any(c => c.RollCallDefinitionId == x.RollCallDefinicationInItemModel.Id))
                    )).ToList();
                    // resultModel = resultModel.Except(resultModelOverTime).ToList();
                    // اضافه کاریهای قبل از آموزش
                    List<EmployeeAttendAbsenceAnalysisModel> resultBeforeTrain = new List<EmployeeAttendAbsenceAnalysisModel>();
                    List<EmployeeAttendAbsenceAnalysisModel> resultAfterTrain = new List<EmployeeAttendAbsenceAnalysisModel>();
                    foreach (var train in employeeEducation)
                    {
                        if (timeSettingDataModel.IsRestShift
                            || (timeSettingDataModel.ShiftEndDate == date &&
                                (train.StartTime.ConvertStringToTimeSpan() >= timeSettingDataModel.ShiftEndTimeToTimeSpan
                                || train.EndTime.ConvertStringToTimeSpan() <= timeSettingDataModel.ShiftStartTimeWithToleranceToTimeSpan))
                            || train.EndTime.ConvertStringToTimeSpan() <= timeSettingDataModel.ShiftStartTimeToTimeSpan)
                        {
                            //var attendanceTraining = employeeEducation.SingleOrDefault(x => x.TrainingTypeId == train.TrainingTypeId);
                            var attendanceTraining = employeeEducation.SingleOrDefault(x => x.Id == train.Id);
                            var rollCallTrainingOverTime = GetRollCallTraining(timeSettingDataModel, rollCallDefinition, attendanceTraining, false, true);
                            if (rollCallTrainingOverTime != null)
                            {
                                trainingModelOverTime.Add(new TrainingModel() { StartTime = train.StartTime, EndTime = train.EndTime, RollCallTrainingOverTime = rollCallTrainingOverTime });

                            }
                        }
                    }
                    //foreach (var item in trainingModelOverTime)
                    //{
                    //    var before = resultModelOverTime.Where(x => x.EndDate.Date == date && item.StartTimeToTimeSpan >= x.EndTimeToTimeSpan && item.EndTimeToTimeSpan > x.EndTimeToTimeSpan).ToList();
                    //    resultBeforeTrain.AddRange(before);
                    //    var after = resultModelOverTime.Where(x => x.StartDate.Date != date.Date || (x.StartTimeToTimeSpan > item.StartTimeToTimeSpan && x.StartTimeToTimeSpan >= item.EndTimeToTimeSpan)).ToList();
                    //    resultAfterTrain.AddRange(after);
                    //}

                    resultBeforeTrain = resultModelOverTime.Where(x => x.EndDate == date && !trainingModelOverTime.Any(e => e.StartTime.ConvertStringToTimeSpan() < x.EndTime.ConvertStringToTimeSpan() || e.EndTime.ConvertStringToTimeSpan() < x.EndTime.ConvertStringToTimeSpan())).ToList();

                    //
                    resultAfterTrain = resultModelOverTime.Where(x => !trainingModelOverTime.Any(e => x.StartDate == date
                    && (x.StartTime.ConvertStringToTimeSpan() < e.StartTime.ConvertStringToTimeSpan() || x.StartTime.ConvertStringToTimeSpan() < e.EndTime.ConvertStringToTimeSpan()))).ToList();
                    var resultForTrain = resultModelOverTime.Except(resultBeforeTrain).Except(resultAfterTrain);
                    resultForTrain = resultForTrain.Where(x => x.StartDate == date);
                    //
                    if (resultForTrain.Any() == false)
                    {
                        foreach (var train in trainingModelOverTime)
                        {
                            AddOverTimeTrainingToAnalysisModel(resultModel, train.StartTime, train.EndTime, inputModel.ShiftConceptDetailId, train.RollCallTrainingOverTime, 0, 0, date);
                        }
                    }
                    else
                    {

                        resultModel = resultModel.Except(resultModelOverTime).ToList();

                        if (resultBeforeTrain.Count() != 0)
                            resultModel.AddRange(resultBeforeTrain);
                        //
                        //var employeeEducationResult = employeeEducation.ToList().Where(x =>
                        //                   timeSettingDataModel.IsRestShift
                        //                || (timeSettingDataModel.ShiftEndDate == date && x.StartTime.ConvertStringToTimeSpan() >= timeSettingDataModel.ShiftEndTimeToTimeSpan)
                        //                                || (x.EndTime.ConvertStringToTimeSpan() <= timeSettingDataModel.ShiftStartTimeToTimeSpan)).ToList();
                        var employeeEducationResult = employeeEducation.ToList().Where(x =>
                                           timeSettingDataModel.IsRestShift
                                      || (timeSettingDataModel.ShiftEndDate == date &&
                                (x.StartTime.ConvertStringToTimeSpan() >= timeSettingDataModel.ShiftEndTimeToTimeSpan
                                || x.EndTime.ConvertStringToTimeSpan() <= timeSettingDataModel.ShiftStartTimeWithToleranceToTimeSpan))
                                                        || (x.EndTime.ConvertStringToTimeSpan() <= timeSettingDataModel.ShiftStartTimeToTimeSpan)).ToList();

                        foreach (var item in resultForTrain)
                        {
                            foreach (var trainItem in employeeEducationResult)
                            {
                                var rollCallTrainingOverTime = GetRollCallTraining(timeSettingDataModel, rollCallDefinition, trainItem, false, true);
                                if (rollCallTrainingOverTime == null) //   خارج از محدوده شیفت کد نداشته باشد 
                                    continue;
                                //زمان شروع کلاس قبل از شروع اضافه کار باشد
                                if (trainItem.StartTime.ConvertStringToTimeSpan() < item.StartTime.ConvertStringToTimeSpan())
                                {
                                    //زمان پایان کلاس قبل از شروع اضافه کار باشد
                                    if (trainItem.EndTime.ConvertStringToTimeSpan() < item.StartTime.ConvertStringToTimeSpan())
                                    {
                                        if (!trainingModel.Any(x => x.StartTime == trainItem.StartTime))
                                        {
                                            trainingModel.Add(new TrainingModel() { StartTime = trainItem.StartTime, EndTime = trainItem.EndTime });
                                            AddOverTimeTrainingToAnalysisModel(resultModel, trainItem.StartTime, trainItem.EndTime, inputModel.ShiftConceptDetailId, rollCallTrainingOverTime, 0, 0, date);
                                        }
                                    }
                                    else //زمان پایان کلاس بعد از شروع اضافه کار باشد
                                    {
                                        if (!trainingModel.Any(x => x.StartTime == trainItem.StartTime)) // 
                                        {
                                            trainingModel.Add(new TrainingModel() { StartTime = trainItem.StartTime, EndTime = item.StartTime });
                                            AddOverTimeTrainingToAnalysisModel(resultModel, trainItem.StartTime, trainItem.EndTime, inputModel.ShiftConceptDetailId, rollCallTrainingOverTime, item.EntryId, item.ExitId, date);
                                        }
                                        //زمان پایان کلاس قبل از پایان اضافه کار باشد
                                        if (item.EndDate > timeSettingDataModel.ShiftEndDate || trainItem.EndTime.ConvertStringToTimeSpan() < item.EndTime.ConvertStringToTimeSpan())
                                        {
                                            // افزودن اضافه کار عادی
                                            var endTime = item.EndTime;
                                            if (item.EndDate > timeSettingDataModel.ShiftEndDate && !timeSettingDataModel.TomorrowIsRestShift)
                                            {
                                                endTime = timeSettingDataModel.ShiftStartTime;
                                            }

                                            InputAddOverTimeToAnalysisModel inputAddOverTimeToAnalysisModel = new InputAddOverTimeToAnalysisModel()
                                            {
                                                resultModel = resultModel,
                                                startTime = trainItem.EndTime,
                                                endTime = endTime,
                                                startDate = item.StartDate,
                                                endDate = item.EndDate,
                                                shiftConceptDetailId = inputModel.ShiftConceptDetailId,
                                                overTime = defaultOverTime,
                                                entryId = item.EntryId,
                                                exitId = item.ExitId,
                                                timeSettingDataModel = timeSettingDataModel,
                                                allRollCall = allRollCall,
                                                analysisInputModel = inputModel
                                            };
                                            AddOverTimeToAnalysisModel(inputAddOverTimeToAnalysisModel);
                                            //AddOverTimeToAnalysisModel(resultModel, trainItem.EndTime, endTime, item.StartDate, item.EndDate, inputModel.ShiftConceptDetailId, defaultOverTime, item.EntryId, item.ExitId, timeSettingDataModel, allRollCall);
                                        }
                                        //
                                    }
                                }
                                //زمان شروع کلاس  بعد از شروع اضافه کار باشد
                                else
                                {
                                    // زمان شروع کلاس قبل از پایان اضافه کار باشد
                                    if (item.EndDate == date && trainItem.StartTime.ConvertStringToTimeSpan() < item.EndTime.ConvertStringToTimeSpan())
                                    {
                                        //افزودن اضافه کار
                                        if (trainItem.StartTime.ConvertStringToTimeSpan() > item.StartTime.ConvertStringToTimeSpan())
                                        {

                                            InputAddOverTimeToAnalysisModel inputAddOverTimeToAnalysisModel = new InputAddOverTimeToAnalysisModel()
                                            {
                                                resultModel = resultModel,
                                                startTime = item.StartTime,
                                                endTime = trainItem.StartTime,
                                                startDate = item.StartDate,
                                                endDate = item.StartDate,
                                                shiftConceptDetailId = inputModel.ShiftConceptDetailId,
                                                overTime = defaultOverTime,
                                                entryId = item.EntryId,
                                                exitId = 0,
                                                timeSettingDataModel = timeSettingDataModel,
                                                allRollCall = allRollCall,
                                                analysisInputModel = inputModel
                                            };
                                            AddOverTimeToAnalysisModel(inputAddOverTimeToAnalysisModel);
                                            //AddOverTimeToAnalysisModel(resultModel, item.StartTime, trainItem.StartTime, item.StartDate, item.StartDate, inputModel.ShiftConceptDetailId, defaultOverTime, item.EntryId, 0, timeSettingDataModel, allRollCall);
                                        }
                                        if (!trainingModel.Any(x => x.StartTime == trainItem.StartTime))
                                        {
                                            trainingModel.Add(new TrainingModel() { StartTime = trainItem.StartTime, EndTime = trainItem.EndTime });
                                            AddOverTimeTrainingToAnalysisModel(resultModel, trainItem.StartTime, trainItem.EndTime, inputModel.ShiftConceptDetailId, rollCallTrainingOverTime, 0, item.ExitId, date);
                                        }
                                        //زمان پایان کلاس قبل از پایان اضافه کار باشد
                                        if ((item.EndDate == date && trainItem.EndTime.ConvertStringToTimeSpan() < item.EndTime.ConvertStringToTimeSpan())
                                            || item.EndDate > date
                                            )
                                        {

                                            InputAddOverTimeToAnalysisModel inputAddOverTimeToAnalysisModel = new InputAddOverTimeToAnalysisModel()
                                            {
                                                resultModel = resultModel,
                                                startTime = trainItem.EndTime,
                                                endTime = item.EndTime,
                                                startDate = date,
                                                endDate = item.EndDate,
                                                shiftConceptDetailId = inputModel.ShiftConceptDetailId,
                                                overTime = defaultOverTime,
                                                entryId = 0,
                                                exitId = item.ExitId,
                                                timeSettingDataModel = timeSettingDataModel,
                                                allRollCall = allRollCall,
                                                analysisInputModel = inputModel
                                            };
                                            AddOverTimeToAnalysisModel(inputAddOverTimeToAnalysisModel);
                                            // AddOverTimeToAnalysisModel(resultModel, trainItem.EndTime, item.EndTime, date, item.EndDate, inputModel.ShiftConceptDetailId, defaultOverTime, 0, item.ExitId, timeSettingDataModel, allRollCall);
                                        }
                                    }
                                }

                            }
                        }
                        // آموزشهایی که در محدوده اضافه کار نیستند
                        var employeeEducationNotIntrainingModelList = employeeEducationResult.Where(x => !trainingModel.Any(a => a.StartTime == x.StartTime));
                        foreach (var train in employeeEducationNotIntrainingModelList)
                        {
                            var rollCallTrainingOverTime = GetRollCallTraining(timeSettingDataModel, rollCallDefinition, train, false, true);
                            if (rollCallTrainingOverTime == null) //   خارج از محدوده شیفت کد نداشته باشد 
                                continue;
                            AddOverTimeTrainingToAnalysisModel(resultModel, train.StartTime, train.EndTime, inputModel.ShiftConceptDetailId, rollCallTrainingOverTime, 0, 0, date);

                        }

                        //
                        // اضافه کاریهای بعد از آموزش
                        foreach (var item in resultAfterTrain)
                        {
                            var endTime = item.EndTime;

                            if (item.EndDate > timeSettingDataModel.ShiftEndDate && !timeSettingDataModel.TomorrowIsRestShift)
                            {
                                endTime = timeSettingDataModel.ShiftStartTime;
                            }

                            InputAddOverTimeToAnalysisModel inputAddOverTimeToAnalysisModel = new InputAddOverTimeToAnalysisModel()
                            {
                                resultModel = resultModel,
                                startTime = item.StartTime,
                                endTime = item.EndTime,
                                startDate = item.StartDate,
                                endDate = item.EndDate,
                                shiftConceptDetailId = inputModel.ShiftConceptDetailId,
                                overTime = defaultOverTime,
                                entryId = item.EntryId,
                                exitId = item.ExitId,
                                timeSettingDataModel = timeSettingDataModel,
                                allRollCall = allRollCall,
                                analysisInputModel = inputModel
                            };
                            AddOverTimeToAnalysisModel(inputAddOverTimeToAnalysisModel);
                            // AddOverTimeToAnalysisModel(resultModel, item.StartTime, item.EndTime, item.StartDate, item.EndDate, inputModel.ShiftConceptDetailId, defaultOverTime, item.EntryId, item.ExitId, timeSettingDataModel, allRollCall);
                        }
                        //
                    }

                }


                //
                // 
                var attendAbsenceCount = resultModel.Where(x => !string.IsNullOrEmpty(x.Duration) && (x.RollCallConceptId == EnumRollCallConcept.Absence.Id || x.RollCallConceptId == EnumRollCallConcept.Attend.Id))
                    .Sum(x => x.Duration.ConvertStringToTimeSpan().TotalMinutes);
                var totalWorkMinutesInDay = timeSettingDataModel.TotalWorkHourInDayToTimeSpan.TotalMinutes;
                double? attendCount = null;
                if (timeSettingDataModel.IsTemporaryTime && !timeSettingDataModel.IsRestShift && !inputModel.NotCheckMinimumWorkTimeAdmin)
                {
                    var shiftStartTime = timeSettingDataModel.TemporaryShiftStartTimeReal;
                    if (string.IsNullOrEmpty(timeSettingDataModel.TemporaryShiftStartTimeReal))
                        shiftStartTime = timeSettingDataModel.ShiftStartTime;
                    var shiftEndTime = timeSettingDataModel.TemporaryShiftEndtTimeReal;
                    if (string.IsNullOrEmpty(timeSettingDataModel.TemporaryShiftEndtTimeReal))
                        shiftEndTime = timeSettingDataModel.ShiftEndTime;
                    var attendData = resultModel.Where(x => x.RollCallConceptId == EnumRollCallConcept.Attend.Id
             && x.StartTime.ConvertStringToTimeSpan() >= shiftStartTime.ConvertStringToTimeSpan()
             && x.StartTime.ConvertStringToTimeSpan() < shiftEndTime.ConvertStringToTimeSpan());
                    if (attendData.Count() != 0)
                    {
                        attendCount = 0;
                        foreach (var item in attendData)
                        {
                            if (item.EndTime.ConvertStringToTimeSpan() <= shiftEndTime.ConvertStringToTimeSpan())
                                attendCount = attendCount.Value + item.Duration.ConvertStringToTimeSpan().TotalMinutes;
                            else
                            {
                                var duration = GetDuration(item.StartTime, shiftEndTime, item.StartDate, item.EndDate);
                                attendCount += duration;
                            }
                        }
                    }

                }
                analysisResultModel.AttendTimeInTemprorayTime = attendCount;
                if (resultModel.Any(x => x.RollCallConceptId == 0) || attendAbsenceCount != totalWorkMinutesInDay
    //||      (attendCount.HasValue && attendCount.Value < timeSettingDataModel.MinimumWorkHourInDay.ConvertStringToTimeSpan().TotalMinutes)
    )
                {
                    resultModel = new List<EmployeeAttendAbsenceAnalysisModel>();
                    resultModel.Add(new EmployeeAttendAbsenceAnalysisModel()
                    {
                        StartTime = timeSettingDataModel.ShiftStartTime,
                        EndTime = timeSettingDataModel.ShiftEndTime,
                        StartDate = timeSettingDataModel.ShiftStartDate,
                        EndDate = timeSettingDataModel.ShiftEndDate,

                        RollCallDefinicationInItemModel = new RollCallDefinicationInItemModel()
                        {
                            Code = "",
                            Title = "",
                        },
                        RollCallConceptId = 0,
                        HasError = true,
                        ErrorMessage = "کد حضور-غیاب یافت نشد"

                    });
                    //AddDailyAbcenseToAnalysisModel(resultModel, timeSettingDataModel.ShiftStartTime, timeSettingDataModel.ShiftEndTime, timeSettingDataModel.WorkDayDuration, inputModel.ShiftConceptDetailId, timeSettingDataModel.ShiftStartDate, timeSettingDataModel.ShiftEndDate);
                    analysisResultModel.EmployeeAttendAbsenceAnalysisModel.AddRange(resultModel);
                    return analysisResultModel;

                }
            }
            //
            if (timeSettingDataModel.IsRestShift == false)
            {


                TemprorayOverTimeModel temprorayOverTimeModel = new TemprorayOverTimeModel()
                {
                    resultModel = resultModel,
                    timeSettingDataModel = timeSettingDataModel,
                    rollCallTemporaryStartDate = rollCallOverTimeTemporaryStartDate,
                    rollCallTemporaryEndDate = rollCallOverTimeTemporaryEndDate,
                    inputModel = inputModel,
                    allRollCall = allRollCall,
                    IsTemporaryOverTime = timeSettingDataModel.IsTemporaryOverTime,
                    entryExitResult = entryExitResult
                };
                bool education = false;
                //بررسی آموزش
                if (timeSettingDataModel.IsTemporaryOverTime &&
                    employeeEducationExist && !resultModel.Any(x => x.RollCallDefinicationInItemModel.Id == EnumRollCallDefinication.Karkard.Id))
                {
                    {
                        education = true;

                        List<EmployeeAttendAbsenceAnalysisModel> resultModelTemp = new List<EmployeeAttendAbsenceAnalysisModel>();
                        foreach (var item in entryExitResult)
                        {
                            var entryTime = item.EntryTime;
                            if (item.EntryDate.Date == timeSettingDataModel.ShiftStartDate.Date && entryTime.ConvertStringToTimeSpan() < timeSettingDataModel.ShiftStartTimeToTimeSpan)
                            {
                                entryTime = timeSettingDataModel.ShiftStartTime;
                            }
                            resultModelTemp.Add(new EmployeeAttendAbsenceAnalysisModel()
                            {
                                StartTime = entryTime,
                                EndTime = item.ExitTime,
                                StartDate = item.EntryDate,
                                EndDate = item.ExitDate,
                                RollCallDefinicationInItemModel = new RollCallDefinicationInItemModel { Id = EnumRollCallDefinication.TrainingKarkard.Id }
                            });

                        }
                        temprorayOverTimeModel.resultModel = resultModelTemp;
                        AddTemprorayOverTime(temprorayOverTimeModel);
                        resultModelTemp = resultModelTemp.Where(x => x.RollCallConceptId == EnumRollCallConcept.OverTime.Id
                        && (x.TemprorayOverTimeInStartShift || x.TemprorayOverTimeInEndShift)).ToList();
                        resultModel.AddRange(resultModelTemp);
                    }
                }
                //
                if (!education)
                {
                    AddTemprorayOverTime(temprorayOverTimeModel);
                }
            }

            //اضافه کار جانبازی
            if (employee.SacrificeOptionSettingId == EnumSacrificeOptionSetting.SacrificeOverTime.Id && !string.IsNullOrEmpty(sacrificeAttendAbsenceTolerance)
             && timeSettingDataModel.IsRestShift == false && entryExitResult.Count() != 0)
            {
                var lastresultModel = resultModel.OrderBy(x => x.StartDate).ThenBy(x => x.StartTime).LastOrDefault();
                string startTime = lastresultModel.EndTime;
                DateTime startDate = lastresultModel.EndDate;
                var overTimeSacrifice = rollCallDefinition.FirstOrDefault(x => x.RollCallDefinitionId == EnumRollCallDefinication.janbaziExtraWork.Id);
                if (overTimeSacrifice != null)
                    AddSacrificeOverTimeToAnalysisModel(resultModel, timeSettingDataModel, sacrificeAttendAbsenceTolerance, inputModel.ShiftConceptDetailId, overTimeSacrifice);
            }
            //

            //
            resultModel = resultModel.OrderBy(x => x.StartDate).ThenBy(x => x.StartTime).ToList();
            analysisResultModel.EmployeeAttendAbsenceAnalysisModel.AddRange(resultModel);
            return analysisResultModel;
            // return resultModel;
        }
        public void AddTemprorayOverTime(TemprorayOverTimeModel model)
        {
            if (model.timeSettingDataModel.TemprorayOverTimeRollCallDefinitionStartShift != null)
            {
                var kardkardInTemprorayOverTime = model.resultModel.FirstOrDefault(x =>
                //x.RollCallConceptId == EnumRollCallConcept.Attend.Id 
                (x.RollCallConceptId == EnumRollCallConcept.Attend.Id || x.RollCallDefinicationInItemModel.Id == EnumRollCallDefinication.TrainingKarkard.Id)
                &&
x.StartTime.ConvertStringToTimeSpan() >= model.timeSettingDataModel.TemporaryShiftStartOverTimeInStartTime.ConvertStringToTimeSpan()
&& (model.timeSettingDataModel.ShiftStartDate.Date != model.timeSettingDataModel.ShiftEndDate.Date ||
x.StartTime.ConvertStringToTimeSpan() < model.timeSettingDataModel.TemporaryShiftEndOverTimeInStartTime.ConvertStringToTimeSpan()));
                if (kardkardInTemprorayOverTime != null)
                {
                    var temprorayOverTime = model.rollCallTemporaryStartDate;

                    InputAddOverTimeToAnalysisModel inputAddOverTimeToAnalysisModel = new InputAddOverTimeToAnalysisModel()
                    {
                        resultModel = model.resultModel,
                        startTime = kardkardInTemprorayOverTime.StartTime,
                        endTime = model.timeSettingDataModel.TemporaryShiftEndOverTimeInStartTime,
                        startDate = kardkardInTemprorayOverTime.StartDate,
                        endDate = kardkardInTemprorayOverTime.EndDate,
                        shiftConceptDetailId = model.inputModel.ShiftConceptDetailId,
                        overTime = temprorayOverTime,
                        entryId = kardkardInTemprorayOverTime.EntryId,
                        timeSettingDataModel = model.timeSettingDataModel,
                        allRollCall = model.allRollCall,
                        analysisInputModel = model.inputModel,
                        TemprorayOverTimeInStartShift = true
                    };
                    AddOverTimeFixToAnalysisModel(inputAddOverTimeToAnalysisModel);
                }
            }
            if (model.IsTemporaryOverTime)//اضافه کار در زمان موقت برای پایان شیفت چک میشود
            {
                if (model.timeSettingDataModel.TemprorayOverTimeRollCallDefinitionEndShift != null)
                {
                    if (model.timeSettingDataModel.ShiftStartDate.Date == model.timeSettingDataModel.ShiftEndDate.Date)
                    {
                        var kardkardInTemprorayOverTime = model.resultModel.FirstOrDefault(x =>

                        (x.RollCallConceptId == EnumRollCallConcept.Attend.Id || x.RollCallDefinicationInItemModel.Id == EnumRollCallDefinication.TrainingKarkard.Id)
                        && x.StartTime.ConvertStringToTimeSpan() < model.timeSettingDataModel.TemporaryShiftEndOverTimeInEndTime.ConvertStringToTimeSpan()
                       && x.EndTime.ConvertStringToTimeSpan() > model.timeSettingDataModel.TemporaryShiftStartOverTimeInEndTime.ConvertStringToTimeSpan()
                        );

                        if (kardkardInTemprorayOverTime != null)
                        {
                            var startTime = model.timeSettingDataModel.TemporaryShiftStartOverTimeInEndTime;
                            var endTime = model.timeSettingDataModel.TemporaryShiftEndOverTimeInEndTime;
                            if (kardkardInTemprorayOverTime.StartTime.ConvertStringToTimeSpan() > model.timeSettingDataModel.TemporaryShiftStartOverTimeInEndTime.ConvertStringToTimeSpan())
                            {
                                startTime = kardkardInTemprorayOverTime.StartTime;
                            }
                            if (kardkardInTemprorayOverTime.EndTime.ConvertStringToTimeSpan() <= model.timeSettingDataModel.TemporaryShiftEndOverTimeInEndTime.ConvertStringToTimeSpan())
                            {
                                endTime = kardkardInTemprorayOverTime.EndTime;
                                if (model.timeSettingDataModel.ShiftSettingFromShiftboard == false)
                                {
                                    var existData = model.entryExitResult.FirstOrDefault(x => x.ExitId == kardkardInTemprorayOverTime.ExitId);
                                    if (existData != null && existData.ExitTimeToTimeSpan > model.timeSettingDataModel.ShiftEndTimeWithToleranceToTimeSpan
                                        && existData.ExitTimeToTimeSpan < model.timeSettingDataModel.ShiftEndTimeToTimeSpan)
                                    {
                                        endTime = existData.ExitTime;
                                    }
                                }
                            }
                            var temprorayOverTime = model.rollCallTemporaryEndDate;
                            InputAddOverTimeToAnalysisModel inputAddOverTimeToAnalysisModel = new InputAddOverTimeToAnalysisModel()
                            {
                                resultModel = model.resultModel,
                                startTime = startTime,
                                endTime = endTime,
                                startDate = kardkardInTemprorayOverTime.StartDate,
                                endDate = kardkardInTemprorayOverTime.EndDate,
                                shiftConceptDetailId = model.inputModel.ShiftConceptDetailId,
                                overTime = temprorayOverTime,
                                exitId = kardkardInTemprorayOverTime.ExitId,
                                timeSettingDataModel = model.timeSettingDataModel,
                                allRollCall = model.allRollCall,
                                analysisInputModel = model.inputModel,
                                TemprorayOverTimeInEndShift = true
                            };
                            AddOverTimeFixToAnalysisModel(inputAddOverTimeToAnalysisModel);
                        }
                    }
                    else
                    {
                        var kardkardInTemprorayOverTime = model.resultModel.FirstOrDefault(x => x.RollCallConceptId == EnumRollCallConcept.Attend.Id
                       && x.EndDate.Date == model.timeSettingDataModel.ShiftEndDate.Date
                        && x.EndTime.ConvertStringToTimeSpan() > model.timeSettingDataModel.TemporaryShiftStartOverTimeInEndTime.ConvertStringToTimeSpan());
                        if (kardkardInTemprorayOverTime != null)
                        {
                            var startTime = model.timeSettingDataModel.TemporaryShiftStartOverTimeInEndTime;
                            var endTime = model.timeSettingDataModel.TemporaryShiftEndOverTimeInEndTime;
                            if (kardkardInTemprorayOverTime.StartDate.Date == model.timeSettingDataModel.ShiftEndDate.Date
                               && kardkardInTemprorayOverTime.StartTime.ConvertStringToTimeSpan() > model.timeSettingDataModel.TemporaryShiftStartOverTimeInEndTime.ConvertStringToTimeSpan())
                            {
                                startTime = kardkardInTemprorayOverTime.StartTime;
                            }
                            if (
                                kardkardInTemprorayOverTime.EndDate.Date == model.timeSettingDataModel.ShiftEndDate.Date
                              && kardkardInTemprorayOverTime.EndTime.ConvertStringToTimeSpan() <= model.timeSettingDataModel.TemporaryShiftEndOverTimeInEndTime.ConvertStringToTimeSpan())
                            {
                                endTime = kardkardInTemprorayOverTime.EndTime;
                                if (model.timeSettingDataModel.ShiftSettingFromShiftboard == false)
                                {
                                    var existData = model.entryExitResult.FirstOrDefault(x => x.ExitId == kardkardInTemprorayOverTime.ExitId);
                                    if (existData != null && existData.ExitTimeToTimeSpan > model.timeSettingDataModel.ShiftEndTimeWithToleranceToTimeSpan
                                        && existData.ExitTimeToTimeSpan < model.timeSettingDataModel.ShiftEndTimeToTimeSpan)
                                    {
                                        endTime = existData.ExitTime;
                                    }
                                }
                            }
                            var temprorayOverTime = model.rollCallTemporaryEndDate;
                            InputAddOverTimeToAnalysisModel inputAddOverTimeToAnalysisModel = new InputAddOverTimeToAnalysisModel()
                            {
                                resultModel = model.resultModel,
                                startTime = startTime,
                                endTime = endTime,
                                startDate = kardkardInTemprorayOverTime.StartDate,
                                endDate = kardkardInTemprorayOverTime.EndDate,
                                shiftConceptDetailId = model.inputModel.ShiftConceptDetailId,
                                overTime = temprorayOverTime,
                                exitId = kardkardInTemprorayOverTime.ExitId,
                                timeSettingDataModel = model.timeSettingDataModel,
                                allRollCall = model.allRollCall,
                                analysisInputModel = model.inputModel
                            };
                            AddOverTimeFixToAnalysisModel(inputAddOverTimeToAnalysisModel);
                        }
                    }

                }
            }
        }
        /// <summary>
        /// تنظیمات ورود خروج بر اساس زمان شناور کاربر
        /// </summary>
        /// <param name="entryExitResult"></param>
        /// <param name="timeSettingDataModel"></param>
        void FloatTimeSettingInAnalysisItem(List<EmployeeEntryExitViewModel> entryExitResult, TimeSettingDataModel timeSettingDataModel)
        {
            var firstEntryExitResult = entryExitResult.First();

            var lastEntryExitResult = entryExitResult.FirstOrDefault(x => x.EntryTimeToTimeSpan < timeSettingDataModel.ShiftEndTimeToTimeSpan &&
            x.ExitTimeToTimeSpan >= timeSettingDataModel.MinimumOverTimeAfterShiftToTimeSpan
            );
            if (lastEntryExitResult == null)
                return;
            //
            string firstEntryTime = firstEntryExitResult.EntryTime;
            string lastExitTime = lastEntryExitResult.ExitTime;
            var firstEntryTimeToTimeSpan = firstEntryTime.ConvertStringToTimeSpan();
            var lastExitTimeToTimeSpan = lastExitTime.ConvertStringToTimeSpan();
            //ساعت ورود بعد از ساعت شروع شیفت با تلورانس و آخرین ساعت خروج بعد از کمترین  
            if (firstEntryExitResult.EntryDate == timeSettingDataModel.ShiftStartDate
                && firstEntryExitResult.EntryTimeToTimeSpan > timeSettingDataModel.ShiftStartTimeWithToleranceToTimeSpan
                && firstEntryExitResult.EntryTimeToTimeSpan <= timeSettingDataModel.MaximumFloatTimeFromShiftStart.ConvertStringToTimeSpan()
               && lastEntryExitResult.ExitTimeToTimeSpan >= timeSettingDataModel.MinimumOverTimeAfterShiftToTimeSpan)
            {
                firstEntryExitResult.EntryTime = timeSettingDataModel.ShiftStartTime;
                lastEntryExitResult.ExitTime = timeSettingDataModel.ShiftEndTime;
                firstEntryExitResult.EntryTimeToTimeSpan = timeSettingDataModel.ShiftStartTimeToTimeSpan;
                lastEntryExitResult.ExitTimeToTimeSpan = timeSettingDataModel.ShiftEndTimeToTimeSpan;

                //
                foreach (var item in entryExitResult)
                {
                    if (item.RowGuid == firstEntryExitResult.RowGuid)
                    {
                        item.EntryTime = firstEntryExitResult.EntryTime;
                        item.EntryTimeToTimeSpan = firstEntryExitResult.EntryTime.ConvertStringToTimeSpan();
                    }
                    else
                    {
                        if (item.RowGuid == lastEntryExitResult.RowGuid)
                        {
                            item.ExitTime = lastEntryExitResult.ExitTime;
                            item.ExitTimeToTimeSpan = lastEntryExitResult.ExitTime.ConvertStringToTimeSpan();
                        }
                    }
                }
                //
                string lastEntryTime = null;
                //if (firstEntryExitResult.EntryTimeToTimeSpan <= timeSettingDataModel.FloatTimeFromShiftStart.ConvertStringToTimeSpan())
                if (firstEntryTimeToTimeSpan <= timeSettingDataModel.FloatTimeFromShiftStart.ConvertStringToTimeSpan())
                {
                    lastEntryTime = timeSettingDataModel.ShiftEndTime;
                }
                else
                {
                    int exitFromExtraTime = (int)lastExitTimeToTimeSpan.Subtract(timeSettingDataModel.ShiftEndTimeToTimeSpan).TotalMinutes;
                    int entryFromFloatTime = (int)firstEntryTimeToTimeSpan.Subtract(timeSettingDataModel.FloatTimeFromShiftStart.ConvertStringToTimeSpan()).TotalMinutes;
                    if (exitFromExtraTime > entryFromFloatTime)
                    {
                        lastEntryTime = Utility.GetTimeAfterShiftEnd(timeSettingDataModel.ShiftEndTime, Utility.ConvertMinuteIn24ToDuration(entryFromFloatTime));
                    }
                }
                if (string.IsNullOrEmpty(lastEntryTime) == false)
                {
                    entryExitResult.Add(new EmployeeEntryExitViewModel()
                    {
                        EntryTime = lastEntryTime,
                        EntryTimeToTimeSpan = lastEntryTime.ConvertStringToTimeSpan(),
                        EntryId = 0,
                        ExitTime = lastExitTime,
                        ExitTimeToTimeSpan = lastExitTime.ConvertStringToTimeSpan(),
                        ExitId = lastEntryExitResult.ExitId,
                        EntryDate = timeSettingDataModel.ShiftStartDate,
                        ExitDate = lastEntryExitResult.ExitDate,
                    });
                }
            }
        }

        void FloatTimeSettingInAnalysisItem1(List<EmployeeEntryExitViewModel> entryExitResult, TimeSettingDataModel timeSettingDataModel)
        {
            var firstEntryExitResult = entryExitResult.First();

            var lastEntryExitResult = entryExitResult.Last();

            string firstEntryTime = entryExitResult.First().EntryTime;
            string lastExitTime = entryExitResult.Last().ExitTime;
            var firstEntryTimeToTimeSpan = firstEntryTime.ConvertStringToTimeSpan();
            var lastExitTimeToTimeSpan = lastExitTime.ConvertStringToTimeSpan();
            //ساعت ورود بعد از ساعت شروع شیفت با تلورانس و آخرین ساعت خروج بعد از کمترین  
            if (firstEntryExitResult.EntryDate == timeSettingDataModel.ShiftStartDate
                && firstEntryExitResult.EntryTimeToTimeSpan > timeSettingDataModel.ShiftStartTimeWithToleranceToTimeSpan
                && firstEntryExitResult.EntryTimeToTimeSpan <= timeSettingDataModel.MaximumFloatTimeFromShiftStart.ConvertStringToTimeSpan()
               && lastEntryExitResult.ExitTimeToTimeSpan >= timeSettingDataModel.MinimumOverTimeAfterShiftToTimeSpan)
            {
                entryExitResult.First().EntryTime = timeSettingDataModel.ShiftStartTime;
                entryExitResult.First().EntryTimeToTimeSpan = timeSettingDataModel.ShiftStartTimeToTimeSpan;

                entryExitResult.Last().ExitTime = timeSettingDataModel.ShiftEndTime;
                entryExitResult.Last().ExitTimeToTimeSpan = timeSettingDataModel.ShiftEndTimeToTimeSpan;
                string lastEntryTime = null;
                //if (firstEntryExitResult.EntryTimeToTimeSpan <= timeSettingDataModel.FloatTimeFromShiftStart.ConvertStringToTimeSpan())
                if (firstEntryTimeToTimeSpan <= timeSettingDataModel.FloatTimeFromShiftStart.ConvertStringToTimeSpan())
                {
                    lastEntryTime = timeSettingDataModel.ShiftEndTime;
                }
                else
                {
                    int exitFromExtraTime = (int)lastExitTimeToTimeSpan.Subtract(timeSettingDataModel.ShiftEndTimeToTimeSpan).TotalMinutes;
                    int entryFromFloatTime = (int)firstEntryTimeToTimeSpan.Subtract(timeSettingDataModel.FloatTimeFromShiftStart.ConvertStringToTimeSpan()).TotalMinutes;
                    if (exitFromExtraTime > entryFromFloatTime)
                    {
                        lastEntryTime = Utility.GetTimeAfterShiftEnd(timeSettingDataModel.ShiftEndTime, Utility.ConvertMinuteIn24ToDuration(entryFromFloatTime));
                    }
                }
                if (string.IsNullOrEmpty(lastEntryTime) == false)
                {
                    entryExitResult.Add(new EmployeeEntryExitViewModel()
                    {
                        EntryTime = lastEntryTime,
                        EntryTimeToTimeSpan = lastEntryTime.ConvertStringToTimeSpan(),
                        EntryId = 0,
                        ExitTime = lastExitTime,
                        ExitTimeToTimeSpan = lastExitTime.ConvertStringToTimeSpan(),
                        ExitId = lastEntryExitResult.ExitId,
                        EntryDate = timeSettingDataModel.ShiftStartDate,
                        ExitDate = lastEntryExitResult.ExitDate,
                    });
                }
            }
        }

        /// <summary>
        /// این متد برای بررسی تداخل زمانی بین شیفت جاری در صبحکار و شیفت قبل در شبکاری می باشد 
        /// </summary>
        /// <param name="inputModel"></param>
        /// <param name="date"></param>
        /// <param name="timeShiftSettingByWorkCityIdModel"></param>
        /// <param name="yesterday"></param>
        /// <param name="workCalendars"></param>
        /// <param name="timeSettingDataModel"></param>
        /// <param name="entryExitResult"></param>
        /// <returns></returns>
        private async Task<List<EmployeeEntryExitViewModel>> CheckOverlapppingTime(CheckOverlapppingTimeModel model, TimeSettingDataModel timeSettingDataModel)
        {
            int shiftConceptDetailIdYesterDay = 0;
            var workCalendarYesterday = model.workCalendars.First(x => x.Date == model.yesterday);
            var yesterdayAttendAbsenceItem = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository
                .GetEmployeeAttendAbsenceItemByEmployeeIdWorkCalendarIdAsNoTracking(model.inputModel.EmployeeId,
                workCalendarYesterday.WorkCalendarId).Where(x => !x.InvalidRecord).ToList();
            if (yesterdayAttendAbsenceItem.Count() != 0)
            {// تایید کارکرد روز قبل داشته باشد
                var employeeAttendAbsenceItemYesterday = yesterdayAttendAbsenceItem.FirstOrDefault();

                int shiftConceptIdYesterday = _kscHrUnitOfWork.ShiftConceptDetailRepository.GetById(employeeAttendAbsenceItemYesterday.ShiftConceptDetailId).ShiftConceptId;
                if (shiftConceptIdYesterday == EnumShiftConcept.Night.Id) // 
                {
                    shiftConceptDetailIdYesterDay = employeeAttendAbsenceItemYesterday.ShiftConceptDetailId;
                    var timeshiftSettingYesterday = await GetShiftTimeSettingByDate(model.inputModel.EmployeeId, workCalendarYesterday, model.timeShiftSettingByWorkCityIdModel, null, 0, shiftConceptDetailIdYesterDay);

                    if (
                     timeSettingDataModel.ShiftStartTimeToTimeSpan != timeshiftSettingYesterday.TomorrowShiftEndTimeToTimeSpan
                        &&
                        yesterdayAttendAbsenceItem.Any(x => x.StartTime.ConvertStringToTimeSpan() > timeshiftSettingYesterday.TomorrowShiftEndTimeToTimeSpan
                    && x.EndTime.ConvertStringToTimeSpan() > timeshiftSettingYesterday.TomorrowShiftEndTimeToTimeSpan
                     && x.RollCallDefinition.RollCallConceptId == EnumRollCallConcept.OverTime.Id) == false)
                    {
                        List<EmployeeEntryExitViewModel> entryExitResultTemp = new List<EmployeeEntryExitViewModel>();
                        var fisrtentryExit = model.entryExitResult[0];
                        //
                        var exitTimeList = model.TodayList.Where(x => x.ExitTime.ConvertStringToTimeSpan() >= timeshiftSettingYesterday.TomorrowShiftEndTimeToTimeSpan && x.ExitTime.ConvertStringToTimeSpan() < fisrtentryExit.EntryTimeToTimeSpan).Select(x => new { ExitTime = x.ExitTime, ExitTimeToTimeSpan = x.ExitTime.ConvertStringToTimeSpan() });
                        if (exitTimeList.Count() == 0)
                        {
                            exitTimeList = model.TodayList.Where(x => x.ExitTime.ConvertStringToTimeSpan() < timeshiftSettingYesterday.TomorrowShiftEndTimeToTimeSpan && x.ExitTime.ConvertStringToTimeSpan() < fisrtentryExit.EntryTimeToTimeSpan && x.ExitTime.ConvertStringToTimeSpan() > timeSettingDataModel.ShiftStartTimeToTimeSpan).Select(x => new { ExitTime = x.ExitTime, ExitTimeToTimeSpan = x.ExitTime.ConvertStringToTimeSpan() });

                        }
                        if (exitTimeList.Count() != 0)
                        {
                            var exitTimeData = exitTimeList.Where(x => x.ExitTimeToTimeSpan == exitTimeList.Max(x => x.ExitTimeToTimeSpan)).First();
                            //
                            entryExitResultTemp.Add(new EmployeeEntryExitViewModel()
                            {
                                EntryTime = model.timeSettingDataModel.ShiftStartTime,
                                EntryTimeToTimeSpan = model.timeSettingDataModel.ShiftStartTimeToTimeSpan,
                                EntryId = 0,
                                ExitTime = exitTimeData.ExitTime,
                                ExitTimeToTimeSpan = exitTimeData.ExitTimeToTimeSpan,
                                ExitId = 0,
                                EntryDate = model.date.Date,
                                ExitDate = model.date.Date,

                            });

                            entryExitResultTemp.AddRange(model.entryExitResult);
                            model.entryExitResult = entryExitResultTemp;
                        }

                    }
                }
            }

            return model.entryExitResult;
        }

        public List<RollCallModelForEmployeeAttendAbsenceModel> GetCompatibleRollCallAddNewRowInTimeSheet(List<CompatibleRollCallByCompatibleTypeModel> compatibleRollCallAddNewRowInTimeSheet, int RollCallDefinitionId, int dayNumber, int workDayTypeId, List<RollCallModelForEmployeeAttendAbsenceModel> rollCallDefinition)
        {
            List<RollCallModelForEmployeeAttendAbsenceModel> result = new List<RollCallModelForEmployeeAttendAbsenceModel>();
            var rollCallAddNewRowInTimeSheet = compatibleRollCallAddNewRowInTimeSheet.Where(x => x.RollCallDefinitionId == RollCallDefinitionId
               && ((x.WorkDayTypeId == null && x.DayNumber == dayNumber) || (x.DayNumber == null && x.WorkDayTypeId == workDayTypeId)
               || ((x.DayNumber == dayNumber && x.WorkDayTypeId == workDayTypeId)))
               ).ToList();
            if (rollCallAddNewRowInTimeSheet.Count() != 0)
            {
                result = rollCallAddNewRowInTimeSheet.Select(x => new RollCallModelForEmployeeAttendAbsenceModel()
                {
                    RollCallDefinitionId = x.CompatibleRollCallDefinitionId,
                    RollCallDefinitionCode = x.CompatibleRollCallCode,
                    RollCallDefinitionTitle = x.CompatibleRollCallTitle,
                    RollCallConceptId = x.CompatibleRollCallConceptId,
                    IsValidSingleDelete = x.IsValidSingleDelete
                }).ToList();
            }
            return result;
        }

        /// <summary>
        ///  	عدم تایید اضافه کاری برای افرادی که اطلاعات واکسن کرونا آنها ثبت سیستم نشده است  
        ///  	فقط برای پرسنل روزکار  و فقط کدهای حضور و غیاب 11و16و17و 18
        /// </summary>
        /// <param name="analysisResultModel"></param>
        private void VaccinationCheck(AnalysisAttenAbcenseResultModel analysisResultModel)
        {

            if (analysisResultModel.TimeSettingDataModel.ShiftSettingFromShiftboard == false)
            {
                if ((analysisResultModel.VaccineDosage == null || analysisResultModel.VaccineDosage == 0) &&
                    (analysisResultModel.IsValidUnVaccine == false || (analysisResultModel.UnVaccineValidDate.HasValue && analysisResultModel.UnVaccineValidDate.Value.Date < DateTime.Now.Date)))
                {
                    var rollCallDefinitionIdForVaccinationCheck = analysisResultModel.EmployeeAttendAbsenceAnalysisModel.Where(x => analysisResultModel.RollCallDefinitionIdForVaccinationCheck.Any(a => a == x.RollCallDefinicationInItemModel.Id));
                    if (rollCallDefinitionIdForVaccinationCheck.Any())
                    {
                        analysisResultModel.EmployeeAttendAbsenceAnalysisModel = analysisResultModel.EmployeeAttendAbsenceAnalysisModel.Except(rollCallDefinitionIdForVaccinationCheck.ToList()).OrderBy(x => x.StartDate).ThenBy(x => x.StartTime).ToList();
                    }
                }
            }
        }
        private RollCallModelForEmployeeAttendAbsenceModel GetRollCallTraining(TimeSettingDataModel timeSettingDataModel, List<RollCallModelForEmployeeAttendAbsenceModel> rollCallDefinition, EmployeeEducationTime train, bool trainingValidInShiftTime, bool trainingValidOutShiftTime)
        {
            var result = rollCallDefinition.FirstOrDefault(x =>
            x.TrainingTypeId == train.TrainingTypeId &&
            x.TrainingValidInShiftTime == trainingValidInShiftTime &&
            x.TrainingValidOutShiftTime == trainingValidOutShiftTime);
            //&&            (x.IsValidForAllWorkTimeDayType == true || (x.WorkTimeId == timeSettingDataModel.WorkTimeId && x.WorkDayTypeId == timeSettingDataModel.WorkDayTypeId)));
            return result;
        }
        public void AddDailyAbcenseToAnalysisModel(List<EmployeeAttendAbsenceAnalysisModel> resultModel, string startTime, string endTime, string duration, int shiftConceptDetailId, DateTime startDate, DateTime endDate)
        {
            resultModel.Add(new EmployeeAttendAbsenceAnalysisModel()
            {
                StartTime = startTime,
                EndTime = endTime,
                StartDate = startDate,
                EndDate = endDate,
                Duration = duration,
                RollCallDefinicationInItemModel = new RollCallDefinicationInItemModel(),
                RollCallConceptId = EnumRollCallConcept.Absence.Id,
                DeleteIsValid = false,
                ModifyIsValid = true,
                ShiftConceptDetailId = shiftConceptDetailId
            });
        }
        public void AddOverTimeTrainingToAnalysisModel(List<EmployeeAttendAbsenceAnalysisModel> resultModel, string startTime, string endTime, int shiftConceptDetailId, RollCallModelForEmployeeAttendAbsenceModel training, long entryId, long exitId, DateTime endDate)
        {
            AttendAbsenceTempalteModel tempalteModel = new AttendAbsenceTempalteModel()
            {
                StartTime = startTime,
                EndTime = endTime,
                StartDate = endDate,
                EndDate = endDate,
                RollCallDefinication = new RollCallDefinicationInItemModel()
                {
                    Id = training.RollCallDefinitionId,
                    Code = training.RollCallDefinitionCode,
                    Title = training.RollCallDefinitionTitle,
                },
                RollCallConceptId = EnumRollCallConcept.OverTime.Id,
                DeleteIsValid = false,
                ModifyIsValid = false,
                ShiftConceptDetailId = shiftConceptDetailId,
                EntryId = entryId,
                ExitId = exitId
            };
            AddToAnalysisModel(resultModel, tempalteModel);
        }
        public void AddOverTimeFixToAnalysisModel(InputAddOverTimeToAnalysisModel model)
        {
            AttendAbsenceTempalteModel tempalteModel = new AttendAbsenceTempalteModel()
            {
                StartTime = model.startTime,
                EndTime = model.endTime,
                StartDate = model.endDate,
                EndDate = model.endDate,
                RollCallDefinication = new RollCallDefinicationInItemModel()
                {
                    Id = model.overTime.RollCallDefinitionId,
                    Code = model.overTime.RollCallDefinitionCode,
                    Title = model.overTime.RollCallDefinitionTitle,
                },
                RollCallConceptId = EnumRollCallConcept.OverTime.Id,
                DeleteIsValid = model.overTime.IsValidSingleDelete,
                ModifyIsValid = false,
                ShiftConceptDetailId = model.shiftConceptDetailId,
                EntryId = model.entryId,
                ExitId = model.exitId,
                TemprorayOverTimeInStartShift = model.TemprorayOverTimeInStartShift,
                TemprorayOverTimeInEndShift = model.TemprorayOverTimeInEndShift
            };
            AddToAnalysisModel(model.resultModel, tempalteModel);
        }
        public void AddKarkardToAnalysisModel(AddKarkardToAnalysisViewModel model)
        {
            if (model.karKardCodeTomorrow != null && model.startTime.ConvertStringToTimeSpan() > model.endTime.ConvertStringToTimeSpan() && model.timeSettingDataModel.WorkDayTypeId != model.timeSettingDataModel.TomorrowWorkDayTypeId)
            {
                RollCallModelForEmployeeAttendAbsenceModel karKardCodeTomorrow = model.karKardCodeTomorrow;
                if (model.karKardCodeTomorrow.RollCallDefinitionId == model.karKardCode.RollCallDefinitionId)
                {
                    karKardCodeTomorrow = model.karKardCode;
                }
                string tempTime = "00:00";
                AttendAbsenceTempalteModel tempalteModel = new AttendAbsenceTempalteModel()
                {
                    StartTime = model.startTime,
                    EndTime = tempTime,
                    Duration = Utility.GetDurationStartTimeToEndTime(model.startTime, tempTime),
                    StartDate = model.startDate,
                    EndDate = model.endDate.AddDays(-1),
                    RollCallDefinication = new RollCallDefinicationInItemModel()
                    {
                        Id = model.karKardCode.RollCallDefinitionId,
                        Code = model.karKardCode.RollCallDefinitionCode,
                        Title = model.karKardCode.RollCallDefinitionTitle,
                    },
                    RollCallConceptId = EnumRollCallConcept.Attend.Id,
                    DeleteIsValid = false,
                    ModifyIsValid = false,
                    ShiftConceptDetailId = model.shiftConceptDetailId,
                    EntryId = model.entryId,
                    ExitId = model.exitId
                };
                AddToAnalysisModel(model.resultModel, tempalteModel);
                if (model.karKardToOverTimeToday != null && model.karKardToOverTimeToday.Count() != 0)
                {
                    foreach (var item in model.karKardToOverTimeToday)
                    {
                        var rollCallDefinication = new RollCallDefinicationInItemModel()
                        {
                            Id = item.RollCallDefinitionId,
                            Code = item.RollCallDefinitionCode,
                            Title = item.RollCallDefinitionTitle,
                        };
                        tempalteModel.RollCallDefinication = rollCallDefinication;
                        tempalteModel.RollCallConceptId = item.RollCallConceptId;
                        tempalteModel.DeleteIsValid = item.IsValidSingleDelete;
                        AddToAnalysisModel(model.resultModel, tempalteModel);
                    }
                }
                //
                //if(karKardCode.RollCallDefinitionId==EnumRollCallDefinication.)
                //
                tempalteModel = new AttendAbsenceTempalteModel()
                {
                    StartTime = tempTime,
                    EndTime = model.endTime,
                    StartDate = model.endDate,
                    EndDate = model.endDate,
                    Duration = Utility.GetDurationStartTimeToEndTime(tempTime, model.endTime),
                    RollCallDefinication = new RollCallDefinicationInItemModel()
                    {
                        Id = karKardCodeTomorrow.RollCallDefinitionId,
                        Code = karKardCodeTomorrow.RollCallDefinitionCode,
                        Title = karKardCodeTomorrow.RollCallDefinitionTitle,
                    },
                    RollCallConceptId = EnumRollCallConcept.Attend.Id,
                    DeleteIsValid = false,
                    ModifyIsValid = false,
                    ShiftConceptDetailId = model.shiftConceptDetailId,
                    EntryId = model.entryId,
                    ExitId = model.exitId
                };
                AddToAnalysisModel(model.resultModel, tempalteModel);

                if (model.karKardToOverTimeTomorrow != null && model.karKardToOverTimeTomorrow.Count() != 0)
                {
                    foreach (var item in model.karKardToOverTimeTomorrow)
                    {
                        var rollCallDefinication = new RollCallDefinicationInItemModel()
                        {
                            Id = item.RollCallDefinitionId,
                            Code = item.RollCallDefinitionCode,
                            Title = item.RollCallDefinitionTitle,
                        };
                        tempalteModel.RollCallDefinication = rollCallDefinication;
                        tempalteModel.RollCallConceptId = item.RollCallConceptId;
                        tempalteModel.DeleteIsValid = item.IsValidSingleDelete;
                        AddToAnalysisModel(model.resultModel, tempalteModel);
                    }
                }
                // }
                //else
                //{
                //    AttendAbsenceTempalteModel tempalteModel = new AttendAbsenceTempalteModel()
                //    {
                //        StartTime = model.startTime,
                //        EndTime = model.endTime,
                //        StartDate = model.startDate,
                //        EndDate = model.endDate,
                //        RollCallDefinication = new RollCallDefinicationInItemModel()
                //        {
                //            Id = model.karKardCode.RollCallDefinitionId,
                //            Code = model.karKardCode.RollCallDefinitionCode,
                //            Title = model.karKardCode.RollCallDefinitionTitle,
                //        },
                //        RollCallConceptId = EnumRollCallConcept.Attend.Id,
                //        DeleteIsValid = false,
                //        ModifyIsValid = false,
                //        ShiftConceptDetailId = model.shiftConceptDetailId,
                //        EntryId = model.entryId,
                //        ExitId = model.exitId
                //    };
                //    AddToAnalysisModel(model.resultModel, tempalteModel);
                //    if (model.karKardToOverTimeToday != null && model.karKardToOverTimeToday.Count() != 0)
                //    {
                //        foreach (var item in model.karKardToOverTimeToday)
                //        {
                //            var rollCallDefinication = new RollCallDefinicationInItemModel()
                //            {
                //                Id = item.RollCallDefinitionId,
                //                Code = item.RollCallDefinitionCode,
                //                Title = item.RollCallDefinitionTitle,
                //            };
                //            tempalteModel.RollCallDefinication = rollCallDefinication;
                //            tempalteModel.RollCallConceptId = item.RollCallConceptId;
                //            tempalteModel.DeleteIsValid = item.IsValidSingleDelete;
                //            AddToAnalysisModel(model.resultModel, tempalteModel);
                //        }
                //    }
                //}
            }
            else
            {
                AttendAbsenceTempalteModel tempalteModel = new AttendAbsenceTempalteModel()
                {
                    StartTime = model.startTime,
                    EndTime = model.endTime,
                    StartDate = model.startDate,
                    EndDate = model.endDate,
                    RollCallDefinication = new RollCallDefinicationInItemModel()
                    {
                        Id = model.karKardCode.RollCallDefinitionId,
                        Code = model.karKardCode.RollCallDefinitionCode,
                        Title = model.karKardCode.RollCallDefinitionTitle,
                    },
                    RollCallConceptId = model.karKardCode.RollCallConceptId,// EnumRollCallConcept.Attend.Id,
                    DeleteIsValid = false,
                    ModifyIsValid = false,
                    ShiftConceptDetailId = model.shiftConceptDetailId,
                    EntryId = model.entryId,
                    ExitId = model.exitId
                };
                AddToAnalysisModel(model.resultModel, tempalteModel);
                if (model.karKardToOverTimeToday != null && model.karKardToOverTimeToday.Count() != 0)
                {
                    foreach (var item in model.karKardToOverTimeToday)
                    {
                        var rollCallDefinication = new RollCallDefinicationInItemModel()
                        {
                            Id = item.RollCallDefinitionId,
                            Code = item.RollCallDefinitionCode,
                            Title = item.RollCallDefinitionTitle,
                        };
                        tempalteModel.RollCallDefinication = rollCallDefinication;
                        tempalteModel.RollCallConceptId = item.RollCallConceptId;
                        tempalteModel.DeleteIsValid = item.IsValidSingleDelete;
                        AddToAnalysisModel(model.resultModel, tempalteModel);
                    }
                }
            }
        }
        public void AddHourlyAbcenseToAnalysisModel(List<EmployeeAttendAbsenceAnalysisModel> resultModel, string startTime, string endTime, int shiftConceptDetailId, RollCallModelForEmployeeAttendAbsenceModel rollCallDefinittionStartShift, long entryId, long exitId, DateTime startDate, DateTime endDate)
        {

            AttendAbsenceTempalteModel tempalteModel = new AttendAbsenceTempalteModel()
            {
                StartTime = startTime,
                EndTime = endTime,
                StartDate = startDate,
                EndDate = endDate,
                RollCallDefinication = new RollCallDefinicationInItemModel()
                {
                    Id = rollCallDefinittionStartShift.RollCallDefinitionId,
                    Code = rollCallDefinittionStartShift.RollCallDefinitionCode,
                    Title = rollCallDefinittionStartShift.RollCallDefinitionTitle,
                },
                RollCallConceptId = EnumRollCallConcept.Absence.Id,
                DeleteIsValid = false,
                ModifyIsValid = true,
                ShiftConceptDetailId = shiftConceptDetailId,
                EntryId = entryId,
                ExitId = exitId
            };
            AddToAnalysisModel(resultModel, tempalteModel);

        }
        public void AddVacationHourToAnalysisModel(List<EmployeeAttendAbsenceAnalysisModel> resultModel, string startTime, string endTime, int shiftConceptDetailId, RollCallModelForEmployeeAttendAbsenceModel vacationHour, long entryId, long exitId, DateTime startDate, DateTime endDate)
        {

            AttendAbsenceTempalteModel tempalteModel = new AttendAbsenceTempalteModel()
            {
                StartTime = startTime,
                EndTime = endTime,
                StartDate = startDate,
                EndDate = endDate,
                RollCallDefinication = new RollCallDefinicationInItemModel()
                {
                    Id = vacationHour.RollCallDefinitionId,
                    Code = vacationHour.RollCallDefinitionCode,
                    Title = vacationHour.RollCallDefinitionTitle
                },
                RollCallConceptId = EnumRollCallConcept.Absence.Id,
                DeleteIsValid = false,
                ModifyIsValid = true,
                ShiftConceptDetailId = shiftConceptDetailId,
                EntryId = entryId,
                ExitId = exitId
            };
            AddToAnalysisModel(resultModel, tempalteModel);
        }
        /// <summary>
        /// این متد فقط برای افزودن اضافه کاریهای عادی پرسنل است،اضافه کاریهای دیگر مثل آموزش و فراخوان نباید از طریق این متد اضافه شوند
        /// </summary>
        /// <param name="resultModel"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="shiftConceptDetailId"></param>
        /// <param name="overTime"></param>
        /// <param name="entryId"></param>
        /// <param name="exitId"></param>
        public void AddOverTimeToAnalysisModel(InputAddOverTimeToAnalysisModel model)
        {
            if (model.analysisInputModel.AnalysisType == EnumAnalysisType.Keshik.Id)
            {
                var startDateTemp = model.startDate.Date + model.startTime.ConvertStringToTimeSpan();
                var endDateTemp = model.endDate.Date + model.endTime.ConvertStringToTimeSpan();
                var duration = (endDateTemp - startDateTemp).TotalMinutes;
                var rollCallKeshik = model.allRollCall.RollCallToday.FirstOrDefault(x => x.RollCallDefinitionId == EnumRollCallDefinication.KeshikOverTime.Id);
                SetOverTime(model.resultModel, model.startTime, model.endTime, model.startDate, model.endDate, model.shiftConceptDetailId, rollCallKeshik, model.entryId, model.exitId, duration, null);

                return;
            }
            var dayNightRollCallSettingModel = model.timeSettingDataModel.DayNightRollCallSettingModel;
            //var startDateTemp = startDate.Date + startTime.ConvertStringToTimeSpan();
            //var endDateTemp = endDate.Date + endTime.ConvertStringToTimeSpan();
            //var duration = (endDateTemp - startDateTemp).TotalMinutes;
            //
            string overTimeToken = Guid.NewGuid().ToString();
            int dayNumber = 0;
            bool startToday = true;
            if (model.timeSettingDataModel.ShiftStartDate == model.startDate)
            {
                dayNumber = model.timeSettingDataModel.DayNumber;
            }
            else
                if (model.timeSettingDataModel.TomorrowDateTime == model.startDate)
            {
                dayNumber = model.timeSettingDataModel.TomorrowDayNumber;
                startToday = false;
            }
            bool breakOverTime = false;

            var dayNightRollCallPart1 = dayNightRollCallSettingModel.FirstOrDefault(x => x.DayNumber == dayNumber &&
             ((model.startTime.ConvertStringToTimeSpan() >= x.StartTime.ConvertStringToTimeSpan() && model.startTime.ConvertStringToTimeSpan() <= x.EndTime.ConvertStringToTimeSpan())
               || (x.IsDay == false && (model.startTime.ConvertStringToTimeSpan() >= x.StartTime.ConvertStringToTimeSpan() || (
            model.startTime.ConvertStringToTimeSpan() < x.StartTime.ConvertStringToTimeSpan() && model.startTime.ConvertStringToTimeSpan() < x.EndTime.ConvertStringToTimeSpan()
            )))));
            RollCallModelForEmployeeAttendAbsenceModel rollCallPart1 = null;
            AttendAbsenceTempalteModel tempalteModel = new AttendAbsenceTempalteModel();
            if (dayNightRollCallPart1 != null)
            {
                if (startToday)
                {
                    rollCallPart1 = model.allRollCall.RollCallToday.FirstOrDefault(x => x.RollCallDefinitionId == dayNightRollCallPart1.RollCallDefinitionId);
                }
                else
                {
                    rollCallPart1 = model.allRollCall.RollCallTomorrow.FirstOrDefault(x => x.RollCallDefinitionId == dayNightRollCallPart1.RollCallDefinitionId);
                }
                //if (rollCall != null)
                //    breakOverTime = true;
            }
            //

            if (startToday) // شروع اضافه کار در روز تایید کارکرد باشد
            {
                if (model.startDate == model.endDate) // تاریخ شروع و پایان اضافه کار برابر باشد
                {
                    if (model.endTime.ConvertStringToTimeSpan() <= dayNightRollCallPart1.EndTime.ConvertStringToTimeSpan() ||

                       (dayNightRollCallPart1.IsDay == false && model.endTime.ConvertStringToTimeSpan() > dayNightRollCallPart1.EndTime.ConvertStringToTimeSpan()
                       && model.endTime.ConvertStringToTimeSpan() > dayNightRollCallPart1.StartTime.ConvertStringToTimeSpan()
                       && model.startTime.ConvertStringToTimeSpan() >= dayNightRollCallPart1.StartTime.ConvertStringToTimeSpan()
                       )) // در صورتیکه زمان پایان اضافه کاری در بازه پایان تنظیمات روز-شب باشد
                    {
                        if (rollCallPart1 == null)
                        {
                            rollCallPart1 = model.overTime;
                        }
                        var duration = GetDuration(model.startTime, model.endTime, model.startDate, model.endDate);
                        SetOverTime(model.resultModel, model.startTime, model.endTime, model.startDate, model.endDate, model.shiftConceptDetailId, rollCallPart1, model.entryId, model.exitId, duration, overTimeToken);
                    }
                    else
                    {
                        var startTimePart1 = model.startTime;
                        var endTimePart1 = dayNightRollCallPart1.EndTime;
                        var durationPart1 = GetDuration(startTimePart1, endTimePart1, model.startDate, model.endDate);
                        //
                        var startTimePart2 = dayNightRollCallPart1.EndTime;
                        var endTimePart2 = model.endTime;
                        var startTimePart3 = string.Empty;
                        var endTimePart3 = string.Empty;
                        if (dayNightRollCallPart1.IsDay == false && model.endTime.ConvertStringToTimeSpan() > dayNightRollCallPart1.StartTime.ConvertStringToTimeSpan())
                        {
                            endTimePart2 = dayNightRollCallPart1.StartTime;
                            startTimePart3 = dayNightRollCallPart1.StartTime;
                            endTimePart3 = model.endTime;
                        }
                        var dayNightRollCallPart2 = dayNightRollCallSettingModel.FirstOrDefault(x => x.DayNumber == dayNumber && startTimePart2.ConvertStringToTimeSpan() >= x.StartTime.ConvertStringToTimeSpan() && x.IsDay == false);
                        RollCallModelForEmployeeAttendAbsenceModel rollCallPart2 = null;
                        if (dayNightRollCallPart2 != null)
                            rollCallPart2 = model.allRollCall.RollCallToday.FirstOrDefault(x => x.RollCallDefinitionId == dayNightRollCallPart2.RollCallDefinitionId);
                        if (rollCallPart1 == null && rollCallPart2 == null)
                        {
                            var duration = GetDuration(model.startTime, model.endTime, model.startDate, model.endDate);
                            SetOverTime(model.resultModel, model.startTime, model.endTime, model.startDate, model.endDate, model.shiftConceptDetailId, model.overTime, model.entryId, model.exitId, duration, overTimeToken);
                        }
                        else
                        {
                            if (rollCallPart1 == null)
                            {
                                rollCallPart1 = model.overTime;
                            }
                            durationPart1 = GetDuration(startTimePart1, endTimePart1, model.startDate, model.endDate);
                            if (durationPart1 != 0)
                            {
                                SetOverTime(model.resultModel, startTimePart1, endTimePart1, model.startDate, model.endDate, model.shiftConceptDetailId, rollCallPart1, model.entryId, 0, durationPart1, overTimeToken);
                            }
                            if (rollCallPart2 == null)
                            {
                                rollCallPart2 = model.overTime;
                            }
                            var durationPart2 = GetDuration(startTimePart2, endTimePart2, model.startDate, model.endDate);
                            SetOverTime(model.resultModel, startTimePart2, endTimePart2, model.startDate, model.endDate, model.shiftConceptDetailId, rollCallPart2, 0, model.exitId, durationPart2, overTimeToken);
                            if (!string.IsNullOrEmpty(startTimePart3) && !string.IsNullOrEmpty(endTimePart3))
                            {
                                var durationPart3 = GetDuration(startTimePart3, endTimePart3, model.startDate, model.endDate);
                                SetOverTime(model.resultModel, startTimePart3, endTimePart3, model.startDate, model.endDate, model.shiftConceptDetailId, rollCallPart1, 0, model.exitId, durationPart3, overTimeToken);
                            }
                        }
                    }
                }
                else // تاریخ شروع و پایان اضافه کار برابر نباشد
                {
                    if (dayNightRollCallPart1.IsDay == true) // قسمت اول در روز باشد
                    {
                        OverTimePartition(model.resultModel, model.startTime, model.endTime, model.startDate, model.endDate, model.shiftConceptDetailId, model.overTime, model.entryId, model.exitId, model.timeSettingDataModel, model.allRollCall, dayNightRollCallSettingModel, overTimeToken, dayNumber, dayNightRollCallPart1, rollCallPart1, true);
                    } // قسمت اول در روز نباشد و تاریخ شروع-پایان اضافه کار برابر نباشد
                    else
                    {
                        OverTimePartition(model.resultModel, model.startTime, model.endTime, model.startDate, model.endDate, model.shiftConceptDetailId, model.overTime, model.entryId, model.exitId, model.timeSettingDataModel, model.allRollCall, dayNightRollCallSettingModel, overTimeToken, dayNumber, dayNightRollCallPart1, rollCallPart1, false);
                    }
                }
            }
            //
            else// شروع اضافه کار در روز بعد از تایید کارکرد باشد
            {
                if (model.endTime.ConvertStringToTimeSpan() <= dayNightRollCallPart1.EndTime.ConvertStringToTimeSpan()) // در صورتیکه زمان پایان اضافه کاری در بازه پایان تنظیمات روز-شب باشد
                {
                    var duration = GetDuration(model.startTime, model.endTime, model.startDate, model.endDate);
                    if (rollCallPart1 == null)
                    {
                        //rollCallPart1 = model.overTime;
                        rollCallPart1 = model.allRollCall.RollCallTomorrow.FirstOrDefault(x => x.RollCallConceptId == EnumRollCallConcept.OverTime.Id && x.RollCallCategoryId == EnumRollCallCategory.DefaultOverTime.Id);
                    }
                    SetOverTime(model.resultModel, model.startTime, model.endTime, model.startDate, model.endDate, model.shiftConceptDetailId, rollCallPart1, model.entryId, model.exitId, duration, overTimeToken);
                }
                else
                {
                    var startTimePart1 = model.startTime;
                    var endTimePart1 = dayNightRollCallPart1.EndTime;
                    var durationPart1 = GetDuration(startTimePart1, endTimePart1, model.startDate, model.endDate);
                    // SetOverTime(resultModel, startTimePart1, endTimePart1, startDate, endDate, shiftConceptDetailId, rollCall, entryId, 0, duration);
                    //
                    var startTimePart2 = dayNightRollCallPart1.EndTime;
                    var endTimePart2 = model.endTime;
                    var dayNightRollCallePart2 = dayNightRollCallSettingModel.FirstOrDefault(x => x.DayNumber == dayNumber && startTimePart2.ConvertStringToTimeSpan() >= x.StartTime.ConvertStringToTimeSpan() && x.IsDay == false);
                    var rollCallPart2 = model.allRollCall.RollCallToday.FirstOrDefault(x => x.RollCallDefinitionId == dayNightRollCallePart2.RollCallDefinitionId);
                    var durationPart2 = GetDuration(startTimePart2, endTimePart2, model.startDate, model.endDate);
                    if (rollCallPart1 == null && rollCallPart2 == null)
                    {
                        var duration = GetDuration(model.startTime, model.endTime, model.startDate, model.endDate);
                        SetOverTime(model.resultModel, model.startTime, model.endTime, model.startDate, model.endDate, model.shiftConceptDetailId, model.overTime, model.entryId, model.exitId, duration, overTimeToken);
                    }
                    else
                    {
                        if (rollCallPart1 == null)
                        {
                            rollCallPart1 = model.overTime;
                        }
                        SetOverTime(model.resultModel, startTimePart1, endTimePart1, model.startDate, model.endDate, model.shiftConceptDetailId, rollCallPart1, model.entryId, 0, durationPart1, overTimeToken);
                        if (rollCallPart2 == null)
                        {
                            rollCallPart2 = model.overTime;
                        }
                        SetOverTime(model.resultModel, startTimePart2, endTimePart2, model.startDate, model.endDate, model.shiftConceptDetailId, rollCallPart2, 0, model.exitId, durationPart2, overTimeToken);
                    }
                    //SetOverTime(resultModel, startTimePart2, endTimePart2, startDate, endDate, shiftConceptDetailId, rollCallPart2, 0, exitId, durationPart2);
                }

            }


        }
        public void AddOverTimeToAnalysisModel1(List<EmployeeAttendAbsenceAnalysisModel> resultModel, string startTime, string endTime, DateTime startDate, DateTime endDate, int shiftConceptDetailId
            , RollCallModelForEmployeeAttendAbsenceModel overTime, long entryId, long exitId, TimeSettingDataModel timeSettingDataModel, RollCallForEmployeeAttendAbsenceModel allRollCall)
        {
            var dayNightRollCallSettingModel = timeSettingDataModel.DayNightRollCallSettingModel;
            //var startDateTemp = startDate.Date + startTime.ConvertStringToTimeSpan();
            //var endDateTemp = endDate.Date + endTime.ConvertStringToTimeSpan();
            //var duration = (endDateTemp - startDateTemp).TotalMinutes;
            //
            string overTimeToken = Guid.NewGuid().ToString();
            int dayNumber = 0;
            bool startToday = true;
            if (timeSettingDataModel.ShiftStartDate == startDate)
            {
                dayNumber = timeSettingDataModel.DayNumber;
            }
            else
                if (timeSettingDataModel.TomorrowDateTime == startDate)
            {
                dayNumber = timeSettingDataModel.TomorrowDayNumber;
                startToday = false;
            }
            bool breakOverTime = false;
            var dayNightRollCallPart1 = dayNightRollCallSettingModel.FirstOrDefault(x => x.DayNumber == dayNumber &&
           ((startTime.ConvertStringToTimeSpan() >= x.StartTime.ConvertStringToTimeSpan() && startTime.ConvertStringToTimeSpan() <= x.EndTime.ConvertStringToTimeSpan())
            || (x.IsDay == false && (startTime.ConvertStringToTimeSpan() >= x.StartTime.ConvertStringToTimeSpan()) || (
            startTime.ConvertStringToTimeSpan() < x.StartTime.ConvertStringToTimeSpan() && startTime.ConvertStringToTimeSpan() < x.EndTime.ConvertStringToTimeSpan()
            ))));
            RollCallModelForEmployeeAttendAbsenceModel rollCallPart1 = null;
            AttendAbsenceTempalteModel tempalteModel = new AttendAbsenceTempalteModel();
            if (dayNightRollCallPart1 != null)
            {
                if (startToday)
                {
                    rollCallPart1 = allRollCall.RollCallToday.FirstOrDefault(x => x.RollCallDefinitionId == dayNightRollCallPart1.RollCallDefinitionId);
                }
                else
                {
                    rollCallPart1 = allRollCall.RollCallTomorrow.FirstOrDefault(x => x.RollCallDefinitionId == dayNightRollCallPart1.RollCallDefinitionId);
                }
                //if (rollCall != null)
                //    breakOverTime = true;
            }
            //

            if (startToday) // شروع اضافه کار در روز تایید کارکرد باشد
            {
                if (startDate == endDate) // تاریخ شروع و پایان اضافه کار برابر باشد
                {
                    if (endTime.ConvertStringToTimeSpan() <= dayNightRollCallPart1.EndTime.ConvertStringToTimeSpan() || dayNightRollCallPart1.IsDay == false) // در صورتیکه زمان پایان اضافه کاری در بازه پایان تنظیمات روز-شب باشد
                    {
                        if (rollCallPart1 == null)
                        {
                            rollCallPart1 = overTime;
                        }
                        var duration = GetDuration(startTime, endTime, startDate, endDate);
                        SetOverTime(resultModel, startTime, endTime, startDate, endDate, shiftConceptDetailId, rollCallPart1, entryId, exitId, duration, overTimeToken);
                    }
                    else
                    {
                        var startTimePart1 = startTime;
                        var endTimePart1 = dayNightRollCallPart1.EndTime;
                        var durationPart1 = GetDuration(startTimePart1, endTimePart1, startDate, endDate);
                        //
                        var startTimePart2 = dayNightRollCallPart1.EndTime;
                        var endTimePart2 = endTime;
                        var dayNightRollCallPart2 = dayNightRollCallSettingModel.FirstOrDefault(x => x.DayNumber == dayNumber && startTimePart2.ConvertStringToTimeSpan() >= x.StartTime.ConvertStringToTimeSpan() && x.IsDay == false);
                        RollCallModelForEmployeeAttendAbsenceModel rollCallPart2 = null;
                        if (dayNightRollCallPart2 != null)
                            rollCallPart2 = allRollCall.RollCallToday.FirstOrDefault(x => x.RollCallDefinitionId == dayNightRollCallPart2.RollCallDefinitionId);
                        if (rollCallPart1 == null && rollCallPart2 == null)
                        {
                            var duration = GetDuration(startTime, endTime, startDate, endDate);
                            SetOverTime(resultModel, startTime, endTime, startDate, endDate, shiftConceptDetailId, overTime, entryId, exitId, duration, overTimeToken);
                        }
                        else
                        {
                            if (rollCallPart1 == null)
                            {
                                rollCallPart1 = overTime;
                            }
                            durationPart1 = GetDuration(startTimePart1, endTimePart1, startDate, endDate);
                            SetOverTime(resultModel, startTimePart1, endTimePart1, startDate, endDate, shiftConceptDetailId, rollCallPart1, entryId, 0, durationPart1, overTimeToken);
                            if (rollCallPart2 == null)
                            {
                                rollCallPart2 = overTime;
                            }
                            var durationPart2 = GetDuration(startTimePart2, endTimePart2, startDate, endDate);
                            SetOverTime(resultModel, startTimePart2, endTimePart2, startDate, endDate, shiftConceptDetailId, rollCallPart2, 0, exitId, durationPart2, overTimeToken);
                        }
                    }
                }
                else // تاریخ شروع و پایان اضافه کار برابر نباشد
                {
                    if (dayNightRollCallPart1.IsDay == true) // قسمت اول در روز باشد
                    {
                        OverTimePartition(resultModel, startTime, endTime, startDate, endDate, shiftConceptDetailId, overTime, entryId, exitId, timeSettingDataModel, allRollCall, dayNightRollCallSettingModel, overTimeToken, dayNumber, dayNightRollCallPart1, rollCallPart1, true);
                    } // قسمت اول در روز نباشد و تاریخ شروع-پایان اضافه کار برابر نباشد
                    else
                    {
                        OverTimePartition(resultModel, startTime, endTime, startDate, endDate, shiftConceptDetailId, overTime, entryId, exitId, timeSettingDataModel, allRollCall, dayNightRollCallSettingModel, overTimeToken, dayNumber, dayNightRollCallPart1, rollCallPart1, false);
                    }
                }
            }
            //
            else// شروع اضافه کار در روز بعد از تایید کارکرد باشد
            {
                if (endTime.ConvertStringToTimeSpan() <= dayNightRollCallPart1.EndTime.ConvertStringToTimeSpan()) // در صورتیکه زمان پایان اضافه کاری در بازه پایان تنظیمات روز-شب باشد
                {
                    var duration = GetDuration(startTime, endTime, startDate, endDate);
                    if (rollCallPart1 == null)
                        rollCallPart1 = overTime;
                    SetOverTime(resultModel, startTime, endTime, startDate, endDate, shiftConceptDetailId, rollCallPart1, entryId, exitId, duration, overTimeToken);
                }
                else
                {
                    var startTimePart1 = startTime;
                    var endTimePart1 = dayNightRollCallPart1.EndTime;
                    var durationPart1 = GetDuration(startTimePart1, endTimePart1, startDate, endDate);
                    // SetOverTime(resultModel, startTimePart1, endTimePart1, startDate, endDate, shiftConceptDetailId, rollCall, entryId, 0, duration);
                    //
                    var startTimePart2 = dayNightRollCallPart1.EndTime;
                    var endTimePart2 = endTime;
                    var dayNightRollCallePart2 = dayNightRollCallSettingModel.FirstOrDefault(x => x.DayNumber == dayNumber && startTimePart2.ConvertStringToTimeSpan() >= x.StartTime.ConvertStringToTimeSpan() && x.IsDay == false);
                    var rollCallPart2 = allRollCall.RollCallToday.FirstOrDefault(x => x.RollCallDefinitionId == dayNightRollCallePart2.RollCallDefinitionId);
                    var durationPart2 = GetDuration(startTimePart2, endTimePart2, startDate, endDate);
                    if (rollCallPart1 == null && rollCallPart2 == null)
                    {
                        var duration = GetDuration(startTime, endTime, startDate, endDate);
                        SetOverTime(resultModel, startTime, endTime, startDate, endDate, shiftConceptDetailId, overTime, entryId, exitId, duration, overTimeToken);
                    }
                    else
                    {
                        if (rollCallPart1 == null)
                        {
                            rollCallPart1 = overTime;
                        }
                        SetOverTime(resultModel, startTimePart1, endTimePart1, startDate, endDate, shiftConceptDetailId, rollCallPart1, entryId, 0, durationPart1, overTimeToken);
                        if (rollCallPart2 == null)
                        {
                            rollCallPart2 = overTime;
                        }
                        SetOverTime(resultModel, startTimePart2, endTimePart2, startDate, endDate, shiftConceptDetailId, rollCallPart2, 0, exitId, durationPart2, overTimeToken);
                    }
                    //SetOverTime(resultModel, startTimePart2, endTimePart2, startDate, endDate, shiftConceptDetailId, rollCallPart2, 0, exitId, durationPart2);
                }

            }


        }
        private void OverTimePartition(List<EmployeeAttendAbsenceAnalysisModel> resultModel, string startTime, string endTime, DateTime startDate, DateTime endDate, int shiftConceptDetailId, RollCallModelForEmployeeAttendAbsenceModel overTime, long entryId, long exitId, TimeSettingDataModel timeSettingDataModel, RollCallForEmployeeAttendAbsenceModel allRollCall, List<Share.Model.DayNightRollCall.DayNightRollCallSettingModel> dayNightRollCallSettingModel, string overTimeToken, int dayNumber, DayNightRollCallSettingModel? dayNightRollCallPart1, RollCallModelForEmployeeAttendAbsenceModel rollCallPart1, bool startIsDay)
        {
            var startTimePart1 = startTime;
            var endTimePart1 = dayNightRollCallPart1.EndTime;
            var durationePart1 = GetDuration(startTimePart1, endTimePart1, startDate, startDate);
            //بخش دوم اضافه کار در روز بعد باشد
            var startTimePart2 = dayNightRollCallPart1.EndTime;
            var endTimePart2 = endTime;
            var dayNightRollCallPart2 = dayNightRollCallSettingModel.FirstOrDefault(x => x.DayNumber == dayNumber && startTimePart2.ConvertStringToTimeSpan() >= x.StartTime.ConvertStringToTimeSpan() && x.IsDay == false);
            if (startIsDay == false)
            {
                startTimePart2 = startTime;
                endTimePart2 = endTime;
                dayNightRollCallPart2 = dayNightRollCallPart1;
            }
            // بررسی ساعت صفر فردا
            var dayNightRollCallPart0 = dayNightRollCallSettingModel.FirstOrDefault(x => x.DayNumber == timeSettingDataModel.TomorrowDayNumber && startTimePart2.ConvertStringToTimeSpan() >= x.StartTime.ConvertStringToTimeSpan() && x.IsDay == false);
            var rollCallPart0 = allRollCall.RollCallTomorrow.FirstOrDefault(x => x.RollCallDefinitionId == dayNightRollCallPart0.RollCallDefinitionId);
            if (endTime.ConvertStringToTimeSpan() <= dayNightRollCallPart2.EndTime.ConvertStringToTimeSpan())
            {

                var rollCallPart2 = allRollCall.RollCallToday.FirstOrDefault(x => x.RollCallDefinitionId == dayNightRollCallPart2.RollCallDefinitionId);

                if (rollCallPart1 == null && rollCallPart2 == null && rollCallPart0 == null)
                {
                    var duration = GetDuration(startTime, endTime, startDate, endDate);
                    SetOverTime(resultModel, startTime, endTime, startDate, endDate, shiftConceptDetailId, overTime, entryId, exitId, duration, overTimeToken);
                }
                else
                {
                    if (startIsDay == true)
                    {
                        if (rollCallPart1 == null)
                        {
                            rollCallPart1 = overTime;
                        }
                        SetOverTime(resultModel, startTimePart1, endTimePart1, startDate, startDate, shiftConceptDetailId, rollCallPart1, entryId, 0, durationePart1, overTimeToken);
                    }
                    if (rollCallPart2 != null)
                    {
                        if (rollCallPart0 == null || rollCallPart0.RollCallDefinitionId == rollCallPart2.RollCallDefinitionId || rollCallPart0.RollCallDefinitionId == overTime.RollCallDefinitionId)
                        {
                            var durationPart2 = GetDuration(startTimePart2, endTimePart2, startDate, endDate);
                            SetOverTime(resultModel, startTimePart2, endTimePart2, startDate, endDate, shiftConceptDetailId, rollCallPart2, 0, exitId, durationPart2, overTimeToken);
                        }
                        else
                        {


                            var timePartTemp = "00:00";
                            var durationPart2 = GetDuration(startTimePart2, timePartTemp, startDate, endDate);
                            SetOverTime(resultModel, startTimePart2, timePartTemp, startDate, endDate, shiftConceptDetailId, rollCallPart2, 0, 0, durationPart2, overTimeToken);
                            //
                            var durationPart0 = GetDuration(timePartTemp, endTimePart2, endDate, endDate);
                            SetOverTime(resultModel, timePartTemp, endTimePart2, endDate, endDate, shiftConceptDetailId, rollCallPart0, 0, exitId, durationPart0, overTimeToken);
                        }
                    }
                    else
                    {
                        if (rollCallPart0 == null || rollCallPart0.RollCallDefinitionId == overTime.RollCallDefinitionId)
                        {
                            var durationPart2 = GetDuration(startTimePart2, endTimePart2, startDate, endDate);
                            SetOverTime(resultModel, startTimePart2, endTimePart2, startDate, endDate, shiftConceptDetailId, overTime, 0, exitId, durationPart2, overTimeToken);
                        }
                        else
                        {
                            var timePartTemp = "00:00";
                            var durationPart2 = GetDuration(startTimePart2, timePartTemp, startDate, endDate);
                            SetOverTime(resultModel, startTimePart2, timePartTemp, startDate, endDate, shiftConceptDetailId, overTime, 0, 0, durationPart2, overTimeToken);
                            //

                            var durationPart0 = GetDuration(timePartTemp, endTimePart2, endDate, endDate);
                            SetOverTime(resultModel, timePartTemp, endTimePart2, endDate, endDate, shiftConceptDetailId, rollCallPart0, 0, exitId, durationPart0, overTimeToken);
                        }
                    }

                }
            }

            else //قسمتی از اضافه کار پارت 2 خارج از محدوده باشد
            {
                endTimePart2 = dayNightRollCallPart2.EndTime;
                var rollCallPart2 = allRollCall.RollCallToday.FirstOrDefault(x => x.RollCallDefinitionId == dayNightRollCallPart2.RollCallDefinitionId);

                var dayNightRollCallPart3 = dayNightRollCallSettingModel.FirstOrDefault(x => x.DayNumber == timeSettingDataModel.TomorrowDayNumber && startTimePart2.ConvertStringToTimeSpan() >= x.StartTime.ConvertStringToTimeSpan() && x.IsDay == true);
                var rollCallPart3 = allRollCall.RollCallTomorrow.FirstOrDefault(x => x.RollCallDefinitionId == dayNightRollCallPart3.RollCallDefinitionId);
                //
                if (rollCallPart3 == null)
                {
                    rollCallPart3 = allRollCall.RollCallTomorrow.FirstOrDefault(x => x.RollCallConceptId == EnumRollCallConcept.OverTime.Id && x.RollCallCategoryId == EnumRollCallCategory.DefaultOverTime.Id);
                }
                //

                if (rollCallPart1 == null && rollCallPart2 == null && rollCallPart0 == null && rollCallPart3 == null)
                {
                    var duration = GetDuration(startTime, endTime, startDate, endDate);
                    SetOverTime(resultModel, startTime, endTime, startDate, endDate, shiftConceptDetailId, overTime, entryId, exitId, duration, overTimeToken);
                }
                else
                {
                    if (startIsDay == true)
                    {
                        if (rollCallPart1 == null)
                            rollCallPart1 = overTime;
                        startTimePart1 = startTime;
                        endTimePart1 = dayNightRollCallPart1.EndTime;
                        durationePart1 = GetDuration(startTimePart1, endTimePart1, startDate, startDate);
                        if (durationePart1 != 0)
                        {
                            SetOverTime(resultModel, startTimePart1, endTimePart1, startDate, startDate, shiftConceptDetailId, rollCallPart1, entryId, 0, durationePart1, overTimeToken);
                        }
                    }
                    if (rollCallPart2 == null)
                    {


                    }
                    else
                    {
                        if (rollCallPart0 == null || rollCallPart0.RollCallDefinitionId == rollCallPart2.RollCallDefinitionId || rollCallPart0.RollCallDefinitionId == overTime.RollCallDefinitionId)
                        {
                            endTimePart2 = dayNightRollCallPart2.EndTime;
                            if (startIsDay == true)
                            {
                                startTimePart2 = dayNightRollCallPart1.EndTime;
                            }
                            else
                            {
                                startTimePart2 = startTime;
                            }
                            var durationPart2 = GetDuration(startTimePart2, endTimePart2, startDate, endDate);
                            SetOverTime(resultModel, startTimePart2, endTimePart2, startDate, endDate, shiftConceptDetailId, rollCallPart2, 0, 0, durationPart2, overTimeToken);
                            if (rollCallPart3 == null)
                                rollCallPart3 = overTime;
                            var startTimePart3 = dayNightRollCallPart2.EndTime;
                            var endTimePart3 = endTime;
                            var durationPart3 = GetDuration(startTimePart3, endTimePart3, endDate, endDate);
                            SetOverTime(resultModel, startTimePart3, endTimePart3, endDate, endDate, shiftConceptDetailId, rollCallPart3, 0, 0, durationPart3, overTimeToken);
                        }
                        else
                        {
                            var timePartTemp = "00:00";
                            if (startIsDay == true)
                            {
                                startTimePart2 = dayNightRollCallPart1.EndTime;
                            }
                            else
                            {
                                startTimePart2 = startTime;
                            }
                            var durationPart2 = GetDuration(startTimePart2, timePartTemp, startDate, endDate);
                            SetOverTime(resultModel, startTimePart2, timePartTemp, startDate, endDate, shiftConceptDetailId, rollCallPart2, 0, 0, durationPart2, overTimeToken);
                            //
                            endTimePart2 = dayNightRollCallPart2.EndTime;
                            var durationPart0 = GetDuration(timePartTemp, endTimePart2, endDate, endDate);
                            SetOverTime(resultModel, timePartTemp, endTimePart2, endDate, endDate, shiftConceptDetailId, rollCallPart0, 0, 0, durationPart0, overTimeToken);

                            if (rollCallPart3 == null)
                                rollCallPart3 = overTime;
                            var startTimePart3 = dayNightRollCallPart2.EndTime;
                            var endTimePart3 = endTime;
                            var durationPart3 = GetDuration(startTimePart3, endTimePart3, endDate, endDate);
                            SetOverTime(resultModel, startTimePart3, endTimePart3, endDate, endDate, shiftConceptDetailId, rollCallPart3, 0, exitId, durationPart3, overTimeToken);
                        }

                    }
                }


            }

        }
        private double GetDuration(string startTime, string endTime, DateTime startDate, DateTime endDate)
        {
            var startDateTemp = startDate.Date + startTime.ConvertStringToTimeSpan();
            var endDateTemp = endDate.Date + endTime.ConvertStringToTimeSpan();
            var duration = (endDateTemp - startDateTemp).TotalMinutes;
            return duration;
        }
        private void SetOverTime(List<EmployeeAttendAbsenceAnalysisModel> resultModel, string startTime, string endTime, DateTime startDate, DateTime endDate, int shiftConceptDetailId, RollCallModelForEmployeeAttendAbsenceModel overTime, long entryId, long exitId, double duration, string overTimeToken)
        {
            AttendAbsenceTempalteModel tempalteModel = new AttendAbsenceTempalteModel()
            {
                StartTime = startTime,
                EndTime = endTime,
                StartDate = startDate,
                EndDate = endDate,
                Duration = Utility.ConvertMinuteToTime((int)duration),
                RollCallDefinication = new RollCallDefinicationInItemModel()
                {
                    Id = overTime.RollCallDefinitionId,
                    Code = overTime.RollCallDefinitionCode,
                    Title = overTime.RollCallDefinitionTitle
                },
                RollCallConceptId = EnumRollCallConcept.OverTime.Id,
                //DeleteIsValid = overTime.RollCallDefinitionId == EnumRollCallDefinication.WorkerDay.Id ? false : true,
                DeleteIsValid = overTime.IsValidSingleDelete,
                ModifyIsValid = false,
                ShiftConceptDetailId = shiftConceptDetailId,
                EntryId = entryId,
                ExitId = exitId,
                OverTimeToken = overTimeToken
            };

            AddToAnalysisModel(resultModel, tempalteModel);
        }
        public void AddSacrificeOverTimeToAnalysisModel(List<EmployeeAttendAbsenceAnalysisModel> resultModel, TimeSettingDataModel timeSettingDataModel, string duration, int shiftConceptDetailId, RollCallModelForEmployeeAttendAbsenceModel overTime)
        {
            string startTime = timeSettingDataModel.ShiftEndTime;
            DateTime startDate = timeSettingDataModel.ShiftEndDate;
            var endTimeTemp = startTime.ConvertStringToTimeSpan().Add(duration.ConvertStringToTimeSpan()).ToString();
            if (endTimeTemp.Substring(0, 2) == "1.")
            {
                endTimeTemp = endTimeTemp.Replace("1.", "");
            }
            string endTime = endTimeTemp.Substring(0, 5);
            DateTime endDate = startDate;
            if (endTime.ConvertStringToTimeSpan() < startTime.ConvertStringToTimeSpan())
            {
                endDate = endDate.AddDays(1);
            }
            AttendAbsenceTempalteModel tempalteModel = new AttendAbsenceTempalteModel()
            {
                StartTime = startTime,
                EndTime = endTime,
                StartDate = startDate,
                EndDate = endDate,
                Duration = duration,
                RollCallDefinication = new RollCallDefinicationInItemModel()
                {
                    Id = overTime.RollCallDefinitionId,
                    Code = overTime.RollCallDefinitionCode,
                    Title = overTime.RollCallDefinitionTitle
                },
                RollCallConceptId = EnumRollCallConcept.OverTime.Id,
                DeleteIsValid = false,
                ModifyIsValid = false,
                ShiftConceptDetailId = shiftConceptDetailId,
                EntryId = 0,
                ExitId = 0
            };
            AddToAnalysisModel(resultModel, tempalteModel);
        }
        public void OverTimeAfterShif(OverTimeAfterShiftViewModel model)
        {
            var rollCallTomorrow = model.allRollCall.RollCallTomorrow;
            string exitTime = string.Empty;
            //
            var addKarkardToAnalysisViewModel = new AddKarkardToAnalysisViewModel()
            {
                resultModel = model.resultModel,
                startTime = model.entryExitResult.EntryTime,
                endTime = model.timeSettingDataModel.ShiftEndTime,
                shiftConceptDetailId = model.ShiftConceptDetailId,
                karKardCode = model.karKardCode,
                entryId = model.entryExitResult.EntryId,
                exitId = model.entryExitResult.ExitId,
                timeSettingDataModel = model.timeSettingDataModel,
                karKardCodeTomorrow = model.karKardCodeTomorrow,
                startDate = model.entryExitResult.EntryDate,
                endDate = model.timeSettingDataModel.ShiftEndDate,
                karKardToOverTimeToday = model.karKardToOverTimeToday,
                karKardToOverTimeTomorrow = model.karKardToOverTimeTomorrow
            };
            AddKarkardToAnalysisModel(addKarkardToAnalysisViewModel);
            if (model.analysisInputModel.AnalysisType == EnumAnalysisType.Keshik.Id) //کشیک مجتمع باشد
            {
                InputAddOverTimeToAnalysisModel inputAddOverTimeToAnalysisModel = new InputAddOverTimeToAnalysisModel()
                {
                    resultModel = model.resultModel,
                    startTime = model.timeSettingDataModel.ShiftEndTime,
                    endTime = model.entryExitResult.ExitTime,
                    startDate = model.timeSettingDataModel.ShiftEndDate,
                    endDate = model.entryExitResult.ExitDate,
                    shiftConceptDetailId = model.ShiftConceptDetailId,
                    overTime = model.defaultOverTime,
                    entryId = 0,
                    exitId = model.entryExitResult.ExitId,
                    timeSettingDataModel = model.timeSettingDataModel,
                    allRollCall = model.allRollCall,
                    analysisInputModel = model.analysisInputModel
                };
                AddOverTimeToAnalysisModel(inputAddOverTimeToAnalysisModel);
            }
            else
            {
                if (model.entryExitResult.ExitDate.Date == model.timeSettingDataModel.ShiftEndDate.Date) // تاریخ خروج و پایان شیفت برابر باشد
                {

                    InputAddOverTimeToAnalysisModel inputAddOverTimeToAnalysisModel = new InputAddOverTimeToAnalysisModel()
                    {
                        resultModel = model.resultModel,
                        startTime = model.timeSettingDataModel.ShiftEndTime,
                        endTime = model.entryExitResult.ExitTime,
                        startDate = model.timeSettingDataModel.ShiftEndDate,
                        endDate = model.entryExitResult.ExitDate,
                        shiftConceptDetailId = model.ShiftConceptDetailId,
                        overTime = model.defaultOverTime,
                        entryId = 0,
                        exitId = model.entryExitResult.ExitId,
                        timeSettingDataModel = model.timeSettingDataModel,
                        allRollCall = model.allRollCall,
                        analysisInputModel = model.analysisInputModel
                    };
                    AddOverTimeToAnalysisModel(inputAddOverTimeToAnalysisModel);
                    //   AddOverTimeToAnalysisModel(model.resultModel, model.timeSettingDataModel.ShiftEndTime, model.entryExitResult.ExitTime, model.timeSettingDataModel.ShiftEndDate, model.entryExitResult.ExitDate, model.ShiftConceptDetailId, model.defaultOverTime, 0, model.entryExitResult.ExitId, model.timeSettingDataModel, model.allRollCall);

                }
                else
                {
                    if (model.timeSettingDataModel.ShiftStartDate.Date == model.timeSettingDataModel.ShiftEndDate.Date) //شروع و پایان شیفت در یک روز باشد و تاریخ خروج در روز بعد باشد
                    {
                        if (model.timeSettingDataModel.ShiftSettingFromShiftboard) // شیفتی باشد
                        {
                            if (model.timeSettingDataModel.TomorrowIsRestShift) // روز بعد روز استراحتش باشد
                            {

                                InputAddOverTimeToAnalysisModel inputAddOverTimeToAnalysisModel = new InputAddOverTimeToAnalysisModel()
                                {
                                    resultModel = model.resultModel,
                                    startTime = model.timeSettingDataModel.ShiftEndTime,
                                    endTime = model.entryExitResult.ExitTime,
                                    startDate = model.timeSettingDataModel.ShiftEndDate,
                                    endDate = model.entryExitResult.ExitDate,
                                    shiftConceptDetailId = model.ShiftConceptDetailId,
                                    overTime = model.defaultOverTime,
                                    entryId = 0,
                                    exitId = model.entryExitResult.ExitId,
                                    timeSettingDataModel = model.timeSettingDataModel,
                                    allRollCall = model.allRollCall,
                                    analysisInputModel = model.analysisInputModel
                                };
                                AddOverTimeToAnalysisModel(inputAddOverTimeToAnalysisModel);
                                // AddOverTimeToAnalysisModel(model.resultModel, model.timeSettingDataModel.ShiftEndTime, model.entryExitResult.ExitTime, model.timeSettingDataModel.ShiftEndDate, model.entryExitResult.ExitDate, model.ShiftConceptDetailId, model.defaultOverTime, 0, model.entryExitResult.ExitId, model.timeSettingDataModel, model.allRollCall);

                            }
                            else
                            {
                                //
                                exitTime = model.entryExitResult.ExitTime;
                                if (exitTime.ConvertStringToTimeSpan() <= model.timeSettingDataModel.TomorrowShiftStartTime.ConvertStringToTimeSpan())
                                {

                                    InputAddOverTimeToAnalysisModel inputAddOverTimeToAnalysisModel = new InputAddOverTimeToAnalysisModel()
                                    {
                                        resultModel = model.resultModel,
                                        startTime = model.timeSettingDataModel.ShiftEndTime,
                                        endTime = exitTime,
                                        startDate = model.timeSettingDataModel.ShiftEndDate,
                                        endDate = model.timeSettingDataModel.TomorrowDateTime,
                                        shiftConceptDetailId = model.ShiftConceptDetailId,
                                        overTime = model.defaultOverTime,
                                        entryId = model.entryExitResult.ExitId,
                                        exitId = 0,
                                        timeSettingDataModel = model.timeSettingDataModel,
                                        allRollCall = model.allRollCall,
                                        analysisInputModel = model.analysisInputModel
                                    };
                                    AddOverTimeToAnalysisModel(inputAddOverTimeToAnalysisModel);
                                    // AddOverTimeToAnalysisModel(model.resultModel, model.timeSettingDataModel.ShiftEndTime, exitTime, model.timeSettingDataModel.ShiftEndDate, model.timeSettingDataModel.TomorrowDateTime, model.ShiftConceptDetailId, model.defaultOverTime, model.entryExitResult.ExitId, 0, model.timeSettingDataModel, model.allRollCall);
                                }

                            }

                        }
                        else // شیفتی نباشد
                        {

                            InputAddOverTimeToAnalysisModel inputAddOverTimeToAnalysisModel = new InputAddOverTimeToAnalysisModel()
                            {
                                resultModel = model.resultModel,
                                startTime = model.timeSettingDataModel.ShiftEndTime,
                                endTime = model.entryExitResult.ExitTime,
                                startDate = model.timeSettingDataModel.ShiftEndDate,
                                endDate = model.entryExitResult.ExitDate,
                                shiftConceptDetailId = model.ShiftConceptDetailId,
                                overTime = model.defaultOverTime,
                                entryId = 0,
                                exitId = model.entryExitResult.ExitId,
                                timeSettingDataModel = model.timeSettingDataModel,
                                allRollCall = model.allRollCall,
                                analysisInputModel = model.analysisInputModel
                            };
                            AddOverTimeToAnalysisModel(inputAddOverTimeToAnalysisModel);
                            // AddOverTimeToAnalysisModel(model.resultModel, model.timeSettingDataModel.ShiftEndTime, model.entryExitResult.ExitTime, model.timeSettingDataModel.ShiftEndDate, model.entryExitResult.ExitDate, model.ShiftConceptDetailId, model.defaultOverTime, 0, model.entryExitResult.ExitId, model.timeSettingDataModel, model.allRollCall);

                        }

                    }
                    else  // تاریخ ورود و خروج شیفت یکسان نباشد و تاریخ خروج کاربر بعد از تاریخ پایان شیفت باشد
                    {

                    }
                }
            }
        }
        public void AddToAnalysisModel(List<EmployeeAttendAbsenceAnalysisModel> resultModel, AttendAbsenceTempalteModel templateModel)
        {
            string duration = string.IsNullOrEmpty(templateModel.Duration) ? Utility.GetDurationStartTimeToEndTime(templateModel.StartTime, templateModel.EndTime) : templateModel.Duration;
            resultModel.Add(new EmployeeAttendAbsenceAnalysisModel()
            {
                StartTime = templateModel.StartTime,
                EndTime = templateModel.EndTime,
                StartDate = templateModel.StartDate,
                EndDate = templateModel.EndDate.Date,
                Duration = duration,
                RollCallDefinicationInItemModel = new RollCallDefinicationInItemModel()
                {
                    Id = templateModel.RollCallDefinication.Id,
                    Code = templateModel.RollCallDefinication.Code,
                    Title = templateModel.RollCallDefinication.Title,
                },
                RollCallConceptId = templateModel.RollCallConceptId,
                DeleteIsValid = templateModel.DeleteIsValid,
                ModifyIsValid = templateModel.ModifyIsValid,
                ShiftConceptDetailId = templateModel.ShiftConceptDetailId,
                EntryId = templateModel.EntryId,
                ExitId = templateModel.ExitId,
                OverTimeToken = templateModel.OverTimeToken,
                TemprorayOverTimeInStartShift = templateModel.TemprorayOverTimeInStartShift,
                TemprorayOverTimeInEndShift = templateModel.TemprorayOverTimeInEndShift
            });
        }

        //
        public async Task<TimeSettingDataModel> GetTimeShiftDateTimeSetting(TimeShiftDateTimeSettingModel inputModel)
        {
            var tomorrow = inputModel.date.AddDays(1);

            var workCalendarToday = inputModel.workCalendars.First(x => x.Date == inputModel.date);
            var workCalendarTomorrow = inputModel.workCalendars.First(x => x.Date == tomorrow);
            try
            {


                var workGroup = await _kscHrUnitOfWork.WorkGroupRepository.GetWorkGroupOutInclud(inputModel.workGroupId);
                var shiftConceptDetail = await _kscHrUnitOfWork.ShiftConceptDetailRepository.GetShiftConceptDetailOutIncludedAsNoTracking(inputModel.shiftConceptDetailId);
                var timeShiftSetting = inputModel.timeShiftSettingByWorkCityIdModel.FirstOrDefault(x => x.WorktimeId == workGroup.WorkTimeId && x.ShiftConceptId == shiftConceptDetail.ShiftConceptId
                    && x.IsTemporaryTime == false && x.ValidityStartDate.Value.Date <= inputModel.date.Date && x.ValidityEndDate.Value.Date >= inputModel.date.Date);
                if (timeShiftSetting == null)
                {
                    throw new HRBusinessException(Validations.NotFoundId, String.Format(Validations.NotFound, "تنظیمات شیفت"));
                }
                string minimumWorkHourInDay = timeShiftSetting.MinimumWorkHourInDay;
                var ShiftStartTime = timeShiftSetting.ShiftStartTime;
                var ShiftEndTime = timeShiftSetting.ShiftEndtTime;
                TimeShiftSettingByWorkCityIdModel timeShiftSettingTemporary = null;
                bool checktimeShiftSettingTemporary = false;
                if (shiftConceptDetail.ShiftConceptId == EnumShiftConcept.Night.Id)
                {
                    if (!workCalendarTomorrow.IsHoliday)
                        checktimeShiftSettingTemporary = true;
                }
                else if (!workCalendarToday.IsHoliday)
                {
                    checktimeShiftSettingTemporary = true;
                }
                timeShiftSettingTemporary = inputModel.timeShiftSettingByWorkCityIdModel
                     .FirstOrDefault(x => x.WorktimeId == workGroup.WorkTimeId && x.ShiftConceptId == shiftConceptDetail.ShiftConceptId && x.IsTemporaryTime == true
                                       && x.ValidityStartDate.Value.Date <= inputModel.date.Date && x.ValidityEndDate.Value.Date >= inputModel.date.Date);
                //if (checktimeShiftSettingTemporary)
                //{
                //    timeShiftSettingTemporary = null;
                //}
                //

                //
                bool IsTemporaryTime = false;
                string TemporaryShiftStartTime = string.Empty;
                string TemporaryShiftEndtTime = string.Empty;
                string TemporaryShiftEndtTimeReal = string.Empty;
                string TemporaryShiftStartTimeReal = string.Empty;
                string TemporaryShiftEndtTimeWithTolerance = string.Empty;
                int? TemporaryRollCallDefinitionStartShift = null;
                int? TemporaryRollCallDefinitionEndShift = null;
                int? TemprorayOverTimeRollCallDefinitionStartShift = null;
                int? TemprorayOverTimeRollCallDefinitionEndShift = null;
                bool IsTemporaryOverTime = false;
                string TemporaryShiftStartOverTimeInEndTime = string.Empty;
                string TemporaryShiftEndOverTimeInEndTime = string.Empty;
                string TemporaryShiftStartOverTimeInStartTime = string.Empty;
                string TemporaryShiftEndOverTimeInStartTime = string.Empty;
                bool InvalidOverTime = false;
                bool CheckedEmployeeValidOverTime = false;
                bool validConditionalAbsence = false;
                string validOverTimeStartTime = null;
                bool validOverTimeByEmployeeId = false;
                string TemporaryShiftStartTimeWithTolerance = string.Empty;
                if (timeShiftSettingTemporary != null)
                {

                    CheckedEmployeeValidOverTime = timeShiftSettingTemporary.CheckedEmployeeValidOverTime == true ? true : false;
                    int rollCallConceptId = 0;
                    if (timeShiftSettingTemporary.TemporaryRollCallDefinitionStartShift != null)
                        rollCallConceptId = _kscHrUnitOfWork.RollCallDefinitionRepository.GetById(timeShiftSettingTemporary.TemporaryRollCallDefinitionStartShift.Value).RollCallConceptId;
                    else
                        if (timeShiftSettingTemporary.TemporaryRollCallDefinitionEndShift.HasValue)
                    {
                        rollCallConceptId = _kscHrUnitOfWork.RollCallDefinitionRepository.GetById(timeShiftSettingTemporary.TemporaryRollCallDefinitionEndShift.Value).RollCallConceptId;
                    }
                    if (rollCallConceptId != 0)
                    {
                        if (rollCallConceptId == EnumRollCallConcept.OverTime.Id)
                        {
                            IsTemporaryOverTime = true;
                            TemporaryShiftStartOverTimeInEndTime = timeShiftSettingTemporary.ShiftStartTime;
                            TemporaryShiftEndOverTimeInEndTime = timeShiftSettingTemporary.ShiftEndtTime;
                            TemprorayOverTimeRollCallDefinitionStartShift = timeShiftSettingTemporary.TemporaryRollCallDefinitionStartShift;
                            TemprorayOverTimeRollCallDefinitionEndShift = timeShiftSettingTemporary.TemporaryRollCallDefinitionEndShift;
                        }
                        else
                        {
                            IsTemporaryTime = true;
                            if (!string.IsNullOrEmpty(timeShiftSettingTemporary.MinimumWorkHourInDay))
                            {
                                minimumWorkHourInDay = timeShiftSettingTemporary.MinimumWorkHourInDay;
                            }
                            TemporaryRollCallDefinitionStartShift = timeShiftSettingTemporary.TemporaryRollCallDefinitionStartShift;
                            TemporaryRollCallDefinitionEndShift = timeShiftSettingTemporary.TemporaryRollCallDefinitionEndShift;
                            if (timeShiftSettingTemporary.TemporaryRollCallDefinitionStartShift != null)
                            {
                                TemporaryShiftStartTimeReal = timeShiftSettingTemporary.ShiftStartTime;
                                TemporaryShiftStartTime = timeShiftSettingTemporary.ShiftStartTime;
                                TemporaryShiftEndtTime = timeShiftSettingTemporary.ShiftEndtTime;
                                var toleranceShiftstart = timeShiftSetting.ToleranceShiftStartTime;
                                if (timeShiftSettingTemporary.ToleranceShiftStartTime.HasValue)
                                {
                                    toleranceShiftstart = timeShiftSettingTemporary.ToleranceShiftStartTime;

                                    TemporaryShiftStartTimeWithTolerance = timeShiftSettingTemporary.ShiftStartTime.ConvertStringToTimeSpan().Add(Utility.ConvertMinuteToTime(toleranceShiftstart.Value).ConvertStringToTimeSpan()).ToString();
                                    TemporaryShiftStartTime = TemporaryShiftStartTimeWithTolerance.Substring(0, 5);

                                }

                            }
                            //else
                            if (timeShiftSettingTemporary.TemporaryRollCallDefinitionEndShift != null)
                            {
                                if (timeShiftSettingTemporary.TemporaryRollCallDefinitionStartShift == null)
                                {
                                    TemporaryShiftStartTime = timeShiftSettingTemporary.ShiftStartTime;
                                }

                                TemporaryShiftEndtTimeReal = timeShiftSettingTemporary.ShiftEndtTime;
                                var toleranceShiftEndTime = timeShiftSetting.ToleranceShiftEndTime;
                                if (timeShiftSettingTemporary.ToleranceShiftEndTime.HasValue)
                                {
                                    toleranceShiftEndTime = timeShiftSettingTemporary.ToleranceShiftEndTime;
                                    TemporaryShiftEndtTimeWithTolerance = Utility.GetTimeBeforeShiftStart(timeShiftSettingTemporary.ShiftEndtTime, Utility.ConvertMinuteIn24ToDuration(toleranceShiftEndTime.Value));
                                }
                                if (inputModel.ValidConditionalAbsence
                                     && ((inputModel.ValidConditionalAbsenceForBreastfedding && timeShiftSettingTemporary.BreastfeddingToleranceTime != null)

                                     || timeShiftSettingTemporary.ConditionalAbsenceToleranceTime != null))
                                {
                                    validConditionalAbsence = true;
                                }
                                if (validConditionalAbsence && !inputModel.ValidConditionalAbsenceInStartShift)
                                {
                                    validConditionalAbsence = true;
                                    var breastfeddingToleranceInMinute = Utility.ConvertDurationToMinute(timeShiftSettingTemporary.BreastfeddingToleranceTime);
                                    TemporaryShiftEndtTime = Utility.GetTimeBeforeShiftStart(timeShiftSettingTemporary.ShiftEndtTime, Utility.ConvertMinuteIn24ToDuration(breastfeddingToleranceInMinute.Value));
                                    TemporaryShiftEndtTimeReal = TemporaryShiftEndtTime;
                                    TemporaryShiftEndtTimeWithTolerance = TemporaryShiftEndtTime;
                                }
                                else
                                {

                                    if (toleranceShiftEndTime.HasValue)
                                        TemporaryShiftEndtTime = Utility.GetTimeBeforeShiftStart(timeShiftSettingTemporary.ShiftEndtTime, Utility.ConvertMinuteIn24ToDuration(toleranceShiftEndTime.Value));
                                    else
                                    {
                                        TemporaryShiftEndtTime = timeShiftSettingTemporary.ShiftEndtTime;

                                    }
                                }
                            }
                        }
                    }

                    // 
                    if (timeShiftSettingTemporary.TemprorayOverTimeRollCallDefinitionStartShift != null || timeShiftSettingTemporary.TemprorayOverTimeRollCallDefinitionEndShift != null)
                    {
                        IsTemporaryOverTime = true;
                        if (timeShiftSettingTemporary.TemprorayOverTimeRollCallDefinitionStartShift != null)
                        {
                            TemporaryShiftStartOverTimeInStartTime = timeShiftSetting.ShiftStartTime;
                            TemporaryShiftEndOverTimeInStartTime = Utility.GetTimeAfterShiftEnd(ShiftStartTime, timeShiftSettingTemporary.TemprorayOverTimeDurationInStartShift);
                            TemprorayOverTimeRollCallDefinitionStartShift = timeShiftSettingTemporary.TemprorayOverTimeRollCallDefinitionStartShift;
                        }
                        // else
                        if (timeShiftSettingTemporary.TemprorayOverTimeRollCallDefinitionEndShift != null)
                        {
                            TemporaryShiftStartOverTimeInEndTime = Utility.GetTimeBeforeShiftStart(ShiftEndTime, timeShiftSettingTemporary.TemprorayOverTimeDurationInEndShift);
                            TemporaryShiftEndOverTimeInEndTime = timeShiftSetting.ShiftEndtTime;
                            TemprorayOverTimeRollCallDefinitionEndShift = timeShiftSettingTemporary.TemprorayOverTimeRollCallDefinitionEndShift;
                        }
                    }
                    //
                    bool temporaryTime = false;
                    var temporaryShiftEndtTimeForOverTime = timeShiftSetting.ShiftEndtTime.ConvertStringToTimeSpan();
                    if (timeShiftSettingTemporary.TemporaryRollCallDefinitionStartShift != null || timeShiftSettingTemporary.TemporaryRollCallDefinitionEndShift != null)
                    {
                        //تنظیمات زمان شروع و پایان شیفت داشته باشد
                        temporaryShiftEndtTimeForOverTime = timeShiftSettingTemporary.ShiftEndtTime.ConvertStringToTimeSpan();
                        temporaryTime = true;
                    }
                    if (timeShiftSettingTemporary.CheckedEmployeeValidOverTime == true && inputModel.IsOfficialAttendForOverTime == false)
                    {
                        //
                        if (inputModel.IsValidHolidayValidOverTime && timeShiftSetting.ShiftSettingFromShiftboard)
                        {
                            validOverTimeByEmployeeId = true;
                        }
                        else
                        {
                            validOverTimeByEmployeeId = _kscHrUnitOfWork.EmployeeValidOverTimeRepository.ValidOverTimeByEmployeeId(inputModel.employeeId, inputModel.WorkCalendarId);
                        }
                        if (validOverTimeByEmployeeId == false)
                        {
                            if (temporaryTime && timeShiftSettingTemporary.ValidOverTimeStartTime.ConvertStringToTimeSpan() <= temporaryShiftEndtTimeForOverTime)
                            {
                                IsTemporaryOverTime = false;
                            }
                            InvalidOverTime = true;
                            validOverTimeStartTime = timeShiftSettingTemporary.ValidOverTimeStartTime;
                            // چک کردن فراخوان اضطراری
                            var oncall = _kscHrUnitOfWork.OnCall_RequestRepository.GetRequestByEmployeeIdOncallDate(inputModel.employeeId, inputModel.date, EnumWorkFlowStatus.Cancel.Id);
                            if (oncall.Any(x => x.OnCallTypeId == (int)OnCall_TypeEnums.Ezterari
                            && x.WF_Request.StatusId == EnumWorkFlowStatus.ReferToOfficialManagement.Id))
                            {
                                InvalidOverTime = false;
                            }
                            //
                        }
                    }
                    //در صورتیکه شیفت باشد و امکان گرفتن اضافه کار تا آخر تام را نداشته باشد
                    if (timeShiftSetting.ShiftSettingFromShiftboard && !checktimeShiftSettingTemporary)
                    {
                        IsTemporaryOverTime = false;
                    }
                    //
                }
                else
                {
                    validConditionalAbsence = inputModel.ValidConditionalAbsence;
                }
                //

                if (timeShiftSettingTemporary != null && timeShiftSettingTemporary.CheckedEmployeeValidOverTime == true
                    &&
                    !validOverTimeByEmployeeId && (!checktimeShiftSettingTemporary || shiftConceptDetail.ShiftConceptId == EnumShiftConcept.Rest.Id))//&& timeShiftSettingTemporary != null)
                {
                    IsTemporaryOverTime = false;
                    InvalidOverTime = true;
                    validOverTimeStartTime = null;
                }
                //زمان معتبر قبل شروع و بعد پایان شیفت
                var TimeBeforeShiftStartTime = Utility.GetTimeBeforeShiftStart(ShiftStartTime, shiftConceptDetail.DurationTimeBeforeShiftStartTime);
                var TotalMinutesShiftStartTime = ShiftStartTime.ConvertStringToTimeSpan().TotalMinutes;
                var TotalMinutesTimeBeforeShiftStartTime = shiftConceptDetail.DurationTimeBeforeShiftStartTime.ConvertStringToTimeSpan().TotalMinutes;
                var DateBeforeShiftStartTime = inputModel.date;
                if (TotalMinutesShiftStartTime - TotalMinutesTimeBeforeShiftStartTime < 0)
                {
                    DateBeforeShiftStartTime = DateBeforeShiftStartTime.AddDays(-1);
                }
                //
                var ShiftEndDate = inputModel.date;
                if (ShiftStartTime.ConvertStringToTimeSpan() > ShiftEndTime.ConvertStringToTimeSpan())
                {
                    ShiftEndDate = ShiftEndDate.AddDays(1);
                }
                //
                var TimeAfterShiftEndTime = Utility.GetTimeAfterShiftEnd(ShiftEndTime, shiftConceptDetail.DurationTimeAfterShiftEndTime);
                var TotalMinutesShiftEndtTime = ShiftEndTime.ConvertStringToTimeSpan().TotalMinutes;
                var TotalMinutesTimeAfterShiftEndTime = shiftConceptDetail.DurationTimeAfterShiftEndTime.ConvertStringToTimeSpan().TotalMinutes;
                var DateAfterShiftEndTime = ShiftEndDate;
                if (TotalMinutesShiftEndtTime + TotalMinutesTimeAfterShiftEndTime > 1440)
                {
                    DateAfterShiftEndTime = DateAfterShiftEndTime.AddDays(1);
                }
                string ShiftStartTimeWithTolerance = string.Empty;
                if (timeShiftSetting.ToleranceShiftStartTime.HasValue)
                {
                    ShiftStartTimeWithTolerance = ShiftStartTime.ConvertStringToTimeSpan().Add(Utility.ConvertMinuteToTime(timeShiftSetting.ToleranceShiftStartTime.Value).ConvertStringToTimeSpan()).ToString();
                }
                string ShiftEndTimeWithTolerance = string.Empty;
                if (timeShiftSetting.ToleranceShiftEndTime.HasValue)
                {
                    ShiftEndTimeWithTolerance = Utility.GetTimeBeforeShiftStart(ShiftEndTime, Utility.ConvertMinuteToTime(timeShiftSetting.ToleranceShiftEndTime.Value));
                }
                bool isRest = false;
                isRest = await _kscHrUnitOfWork.TimeShiftSettingRepository.IsRestShiftAsync(timeShiftSetting.ShiftSettingFromShiftboard, timeShiftSetting.OfficialUnOfficialHolidayFromWorkCalendar
                    , timeShiftSetting.ShiftConceptIsRest, workCalendarToday.IsHoliday, timeShiftSetting.WorkCompanySettingId, workCalendarToday.DayNumber);
                var timeSheetSettingActive = await _kscHrUnitOfWork.TimeSheetSettingRepository.GetTimeSheetSettingActiveAsync();

                #region conditionalAbsence
                // var breastfeddingToleranceTime = timeSheetSettingActive.BreastfeddingToleranceTime;
                var conditionalAbsenceToleranceTime = inputModel.ConditionalAbsenceToleranceTime;
                if (timeShiftSettingTemporary != null && !string.IsNullOrEmpty(timeShiftSettingTemporary.BreastfeddingToleranceTime)
                    && !string.IsNullOrWhiteSpace(timeShiftSettingTemporary.BreastfeddingToleranceTime))
                {
                    conditionalAbsenceToleranceTime = timeShiftSettingTemporary.BreastfeddingToleranceTime;
                }
                var conditionalAbsenceStartTime = Utility.GetTimeAfterShiftEnd(ShiftStartTime, conditionalAbsenceToleranceTime);
                if (timeShiftSettingTemporary == null && timeShiftSetting.ToleranceShiftStartTime.HasValue)
                {
                    TemporaryShiftStartTimeReal = conditionalAbsenceStartTime;

                    conditionalAbsenceStartTime = conditionalAbsenceStartTime.ConvertStringToTimeSpan().Add(Utility.ConvertMinuteToTime(timeShiftSetting.ToleranceShiftStartTime.Value).ConvertStringToTimeSpan()).ToString().Substring(0, 5);
                }
                //
                var conditionalAbsenceStartDate = inputModel.date;
                var TotalMinutesConditionalAbsenceToleranceTime = conditionalAbsenceToleranceTime.ConvertStringToTimeSpan().TotalMinutes;
                if (TotalMinutesShiftStartTime + TotalMinutesConditionalAbsenceToleranceTime > 1440)
                {
                    conditionalAbsenceStartDate = conditionalAbsenceStartDate.AddDays(1);
                }
                //
                var conditionalAbsenceEndTime = Utility.GetTimeBeforeShiftStart(ShiftEndTime, conditionalAbsenceToleranceTime);
                if (timeShiftSettingTemporary == null && timeShiftSetting.ToleranceShiftStartTime.HasValue)
                {
                    TemporaryShiftEndtTimeReal = conditionalAbsenceEndTime;
                    conditionalAbsenceEndTime = Utility.GetTimeBeforeShiftStart(conditionalAbsenceEndTime, Utility.ConvertMinuteToTime(timeShiftSetting.ToleranceShiftEndTime.Value));
                }
                var conditionalAbsenceEndDate = ShiftEndDate;
                if (TotalMinutesShiftEndtTime - TotalMinutesConditionalAbsenceToleranceTime < 0)
                {
                    conditionalAbsenceEndDate = conditionalAbsenceEndDate.AddDays(-1);
                }
                #endregion
                //
                #region TomorrowData
                //
                var tomorrowAttendAbsenceItem = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetEmployeeAttendAbsenceItemByEmployeeIdWorkCalendarIdAsNoTracking(inputModel.employeeId, workCalendarTomorrow.WorkCalendarId).ToList();
                var employeeAttendAbsenceItemTomorrow = tomorrowAttendAbsenceItem.FirstOrDefault();
                int shiftConceptDetailIdTomorrow = 0;
                bool TomorrowExistEmployeeAttendAbsenceItem = false;
                if (employeeAttendAbsenceItemTomorrow != null)
                {
                    shiftConceptDetailIdTomorrow = employeeAttendAbsenceItemTomorrow.ShiftConceptDetailId;
                    TomorrowExistEmployeeAttendAbsenceItem = true;
                }
                //
                var tomorrowShiftTimeSetting = await GetShiftTimeSettingByDate(inputModel.employeeId, workCalendarTomorrow, inputModel.timeShiftSettingByWorkCityIdModel, timeShiftSetting, inputModel.workGroupId, shiftConceptDetailIdTomorrow);
                #endregion
                //
                var minimumOverTimeAfterShiftInMinut = Utility.ConvertMinuteToTime(timeSheetSettingActive.MinimumOverTimeAfterShiftInMinut);
                var minimumShiftStartTimeInMinute = Utility.ConvertMinuteToTime(timeSheetSettingActive.MinimumShiftStartTimeInMinute);
                //
                //
                bool isValidOverTimeInUnOfficialHoliday = false;
                if (timeShiftSetting.OfficialUnOfficialHolidayFromWorkCalendar == true && workCalendarToday.IsUnOfficialHoliday && isRest)
                {
                    var UnOfficialHolidaySetting = await _kscHrUnitOfWork.WorkCompanyUnOfficialHolidaySettingRepository.GetWorkCompanyUnOfficialHolidaySettingActive(timeShiftSetting.WorkCompanySettingId, workCalendarToday.WorkCalendarId);
                    if (UnOfficialHolidaySetting == null)
                    {
                        //isRest = false;
                        isValidOverTimeInUnOfficialHoliday = false;

                    }
                    else
                    {
                        if (UnOfficialHolidaySetting.IsValidExtraWork)
                        {
                            if (UnOfficialHolidaySetting.IsValidExtraWorkForAllCategoryCode)
                            {
                                isValidOverTimeInUnOfficialHoliday = true;
                            }
                            else
                            {
                                var misEmployee = _kscHrUnitOfWork.ViewMisEmployeeRepository.GetEmployeeByPersonalNumber(inputModel.employeeNumber);
                                isValidOverTimeInUnOfficialHoliday = UnOfficialHolidaySetting.WorkCompanyUnOfficialHolidayJobCategories.Any(x => x.CodeCategoryJobCategory == misEmployee.JobCategoryCode);
                            }
                        }
                    }

                }
                //
                string floatTimeFromShiftStart = null;
                string floatTimeFromShiftEnd = null;
                string maximumFloatTimeFromShiftStart = null;
                if (inputModel.FloatTimeSettingId != null)
                {
                    var floatTimeSetting = _kscHrUnitOfWork.FloatTimeSettingRepository.GetById(inputModel.FloatTimeSettingId.Value);
                    if (floatTimeSetting != null)
                    {
                        floatTimeFromShiftStart = Utility.GetTimeAfterShiftEnd(ShiftStartTime, floatTimeSetting.FloatTimeFromShiftStart);
                        maximumFloatTimeFromShiftStart = Utility.GetTimeAfterShiftEnd(ShiftStartTime, floatTimeSetting.MaximumFloatTimeFromShiftStart);
                        floatTimeFromShiftEnd = Utility.GetTimeAfterShiftEnd(ShiftEndTime, floatTimeSetting.FloatTimeFromShiftStart);
                    }
                }

                //
                var minimumOverTimeBeforeShiftInMinut = timeSheetSettingActive.MinimumOverTimeBeforeShift.ConvertDurationToMinute();
                TimeSettingDataModel model = new TimeSettingDataModel()
                {
                    TimeBeforeShiftStartTime = TimeBeforeShiftStartTime,
                    DateAfterShiftEndTime = DateAfterShiftEndTime.Date,
                    DateBeforeShiftStartTime = DateBeforeShiftStartTime.Date,
                    ShiftStartDate = inputModel.date,
                    ShiftEndTime = ShiftEndTime,
                    ShiftStartTime = ShiftStartTime,
                    ShiftEndTimeToTimeSpan = ShiftEndTime.ConvertStringToTimeSpan(),
                    ShiftStartTimeToTimeSpan = ShiftStartTime.ConvertStringToTimeSpan(),
                    IsTemporaryTime = IsTemporaryTime,
                    TemporaryShiftEndTime = TemporaryShiftEndtTime,
                    TemporaryShiftStartTime = TemporaryShiftStartTime,
                    TemporaryRollCallDefinitionStartShift = TemporaryRollCallDefinitionStartShift,
                    TemporaryRollCallDefinitionEndShift = TemporaryRollCallDefinitionEndShift,
                    TimeAfterShiftEndTime = TimeAfterShiftEndTime,
                    TimeAfterShiftEndTimeToTimeSpan = TimeAfterShiftEndTime.ConvertStringToTimeSpan(),
                    ShiftStartTimeWithTolerance = ShiftStartTimeWithTolerance,
                    ShiftEndTimeWithTolerance = ShiftEndTimeWithTolerance,
                    ShiftStartTimeWithToleranceToTimeSpan = ShiftStartTimeWithTolerance.ConvertStringToTimeSpan(),
                    ShiftEndTimeWithToleranceToTimeSpan = ShiftEndTimeWithTolerance.ConvertStringToTimeSpan(),
                    ShiftEndDate = ShiftEndDate.Date,
                    IsRestShift = isRest,
                    ShiftSettingFromShiftboard = timeShiftSetting.ShiftSettingFromShiftboard,
                    IsOfficialHoliday = workCalendarToday.IsOfficialHoliday,
                    IsUnOfficialHoliday = workCalendarToday.IsUnOfficialHoliday,
                    IsHoliday = workCalendarToday.IsHoliday,
                    WorkDayTypeId = workCalendarToday.WorkDayTypeId,
                    DayNumber = workCalendarToday.DayNumber,
                    WorkTimeId = timeShiftSetting.WorktimeId,
                    ConditionalAbsenceStartTime = conditionalAbsenceStartTime,
                    ConditionalAbsenceEndTime = conditionalAbsenceEndTime,
                    ConditionalAbsenceStartTimeToTimeSpan = conditionalAbsenceStartTime.ConvertStringToTimeSpan(),
                    ConditionalAbsenceEndTimeToTimeSpan = conditionalAbsenceEndTime.ConvertStringToTimeSpan(),
                    ConditionalAbsenceStartDate = conditionalAbsenceStartDate.Date,
                    ConditionalAbsenceEndDate = conditionalAbsenceEndDate.Date,
                    TomorrowIsOfficialHoliday = tomorrowShiftTimeSetting.TomorrowIsOfficialHoliday,
                    TomorrowIsUnOfficialHoliday = tomorrowShiftTimeSetting.TomorrowIsUnOfficialHoliday,
                    TomorrowShiftStartTime = tomorrowShiftTimeSetting.TomorrowShiftStartTime,
                    TomorrowShiftEndTime = tomorrowShiftTimeSetting.TomorrowShiftEndTime,
                    TomorrowShiftStartTimeToTimeSpan = tomorrowShiftTimeSetting.TomorrowShiftStartTime.ConvertStringToTimeSpan(),
                    TomorrowShiftEndTimeToTimeSpan = tomorrowShiftTimeSetting.TomorrowShiftEndTime.ConvertStringToTimeSpan(),
                    TomorrowIsRestShift = tomorrowShiftTimeSetting.TomorrowIsRestShift,
                    TomorrowDayNumber = tomorrowShiftTimeSetting.TomorrowDayNumber,
                    TomorrowWorkDayTypeId = tomorrowShiftTimeSetting.TomorrowWorkDayTypeId,
                    TomorrowWorkTimeId = tomorrowShiftTimeSetting.TomorrowWorkTimeId,
                    TomorrowDateTime = tomorrowShiftTimeSetting.TomorrowDateTime.Date,
                    TomorrowShiftConceptId = tomorrowShiftTimeSetting.TomorrowShiftConceptId,
                    TomorrowExistEmployeeAttendAbsenceItem = TomorrowExistEmployeeAttendAbsenceItem,
                    TotalWorkHourInDay = timeShiftSetting.TotalWorkHourInDay,
                    TotalWorkHourInDayToTimeSpan = timeShiftSetting.TotalWorkHourInDay.ConvertStringToTimeSpan(),
                    MinimumOverTimeAfterShift = (ShiftEndTime.ConvertStringToTimeSpan().Add(minimumOverTimeAfterShiftInMinut.ConvertStringToTimeSpan())).ToString(),
                    MinimumShiftStartTimeInMinute = Utility.GetTimeBeforeShiftStart(ShiftStartTime, minimumShiftStartTimeInMinute),
                    WorkDayDuration = timeSheetSettingActive.WorkDayDuration,
                    MaximumAttendInMinute = timeSheetSettingActive.MaximumAttendInMinute,
                    IsValidOverTimeInUnOfficialHoliday = isValidOverTimeInUnOfficialHoliday,
                    OfficialUnOfficialHolidayFromWorkCalendar = timeShiftSetting.OfficialUnOfficialHolidayFromWorkCalendar,
                    DayNightRollCallSettingModel = _kscHrUnitOfWork.DayNightRollCallRepository.GetDayNightRollCallSetting().ToList(),
                    ForcedOverTime = timeShiftSetting.ForcedOverTime,
                    TotalWorkHourInWeek = timeShiftSetting.TotalWorkHourInWeek != null ? timeShiftSetting.TotalWorkHourInWeek.ConvertDurationToMinute().Value : 0,
                    YearMonth = workCalendarToday.YearMonth,
                    ShiftCondeptId = shiftConceptDetail.ShiftConceptId,
                    WorkCalendarIdToday = workCalendarToday.WorkCalendarId,
                    ShiftConceptIsRest = timeShiftSetting.ShiftConceptIsRest,
                    FloatTimeFromShiftStart = floatTimeFromShiftStart,
                    MaximumFloatTimeFromShiftStart = maximumFloatTimeFromShiftStart,
                    FloatTimeFromShiftEnd = floatTimeFromShiftEnd,
                    TrainingStartTimeToTimeSpan = timeSheetSettingActive.TrainingStartTime.ConvertStringToTimeSpan(),
                    TrainingEndTimeToTimeSpan = timeSheetSettingActive.TrainingEndTime.ConvertStringToTimeSpan(),
                    IsTemporaryOverTime = IsTemporaryOverTime,
                    InvalidOverTime = InvalidOverTime,
                    TemporaryShiftStartOverTimeInEndTime = TemporaryShiftStartOverTimeInEndTime,
                    TemporaryShiftEndOverTimeInEndTime = TemporaryShiftEndOverTimeInEndTime,
                    TemprorayOverTimeRollCallDefinitionStartShift = TemprorayOverTimeRollCallDefinitionStartShift,
                    TemprorayOverTimeRollCallDefinitionEndShift = TemprorayOverTimeRollCallDefinitionEndShift,
                    DateKey = workCalendarToday.DateKey,
                    TemporaryShiftEndtTimeReal = TemporaryShiftEndtTimeReal,
                    TemporaryShiftEndtTimeRealToTimeSpan = TemporaryShiftEndtTimeReal.ConvertStringToTimeSpan(),
                    TemporaryShiftEndtTimeWithToleranceToTimeSpan = TemporaryShiftEndtTimeWithTolerance.ConvertStringToTimeSpan(),
                    CheckedEmployeeValidOverTime = CheckedEmployeeValidOverTime,
                    TemporaryShiftStartTimeReal = TemporaryShiftStartTimeReal,
                    TemporaryShiftStartTimeRealToTimeSpan = TemporaryShiftStartTimeReal.ConvertStringToTimeSpan(),
                    MinimumOverTimeBeforeShiftInMinut = minimumOverTimeBeforeShiftInMinut.HasValue ? minimumOverTimeBeforeShiftInMinut.Value : 0,
                    ValidConditionalAbsence = validConditionalAbsence,
                    ValidOverTimeStartTime = validOverTimeStartTime,
                    ValidOverTimeByEmployeeId = validOverTimeByEmployeeId,
                    MinimumWorkHourInDay = minimumWorkHourInDay,
                    TemporaryShiftStartTimeWithTolerance = TemporaryShiftStartTimeWithTolerance,
                    TemporaryShiftStartOverTimeInStartTime = TemporaryShiftStartOverTimeInStartTime,
                    TemporaryShiftEndOverTimeInStartTime = TemporaryShiftEndOverTimeInStartTime
                };
                model.MinimumOverTimeAfterShiftToTimeSpan = model.MinimumOverTimeAfterShift.ConvertStringToTimeSpan();   //
                model.MinimumShiftStartTimeInMinuteToTimeSpan = model.MinimumShiftStartTimeInMinute.ConvertStringToTimeSpan();
                //
                var minimumOverTimeBeforeShift = Utility.ConvertMinuteToTime(model.MinimumOverTimeBeforeShiftInMinut);
                model.MinimumOverTimeBeforeShiftInMinutToTimeSpan = Utility.GetTimeBeforeShiftStart(ShiftStartTime, minimumOverTimeBeforeShift).ConvertStringToTimeSpan();
                //
                if (model.OfficialUnOfficialHolidayFromWorkCalendar == true && model.IsUnOfficialHoliday == true && model.IsValidOverTimeInUnOfficialHoliday == true)
                {
                    model.InvalidOverTime = false;
                }
                //
                return model;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private async Task<TimeSettingDataModel> GetShiftTimeSettingByDate(int employeeId, WorkCalendarForAttendAbcenseAnalysis workCalendarData, List<TimeShiftSettingByWorkCityIdModel> timeShiftSettingByWorkCityIdModel, TimeShiftSettingByWorkCityIdModel timeShiftSettingCurrent, int workGroupIdCurrent, int? shiftConceptDetailId = null)
        {
            TimeSettingDataModel model = new TimeSettingDataModel();
            var tomorrowDate = workCalendarData.Date;
            //
            model.TomorrowDateTime = tomorrowDate;
            var employeeWorkGroup = await _kscHrUnitOfWork.EmployeeWorkGroupRepository.GetEmployeeWorkGroupByEmployeeIdDateIncludeWorkGroup(employeeId, tomorrowDate);
            if (employeeWorkGroup == null)
            {
                throw new HRBusinessException(Validations.NotFoundId, String.Format(Validations.NotFound, "اطلاعات روز بعد گروه کاری وجود ندارد"));
            }
            model.TomorrowIsOfficialHoliday = workCalendarData.IsOfficialHoliday;
            model.TomorrowIsUnOfficialHoliday = workCalendarData.IsUnOfficialHoliday;
            model.TomorrowDayNumber = workCalendarData.DayNumber;
            model.TomorrowWorkDayTypeId = workCalendarData.WorkDayTypeId;
            //
            SearchShiftConceptDetailModel shiftConceptDetail = null;
            if (shiftConceptDetailId != null && shiftConceptDetailId != 0)
            {
                var shiftConceptDetailTemp = _kscHrUnitOfWork.ShiftConceptDetailRepository.GetById(shiftConceptDetailId.Value);
                shiftConceptDetail = _mapper.Map<SearchShiftConceptDetailModel>(shiftConceptDetailTemp);
            }
            else
            {
                if (employeeWorkGroup.WorkGroupId == workGroupIdCurrent) //در صورتیکه گروه تغییر نکرده باشد
                {
                    if (timeShiftSettingCurrent.ShiftSettingFromShiftboard == false) //   اطلاعات شیفت از تقویم باشد
                    {
                        model.TomorrowShiftStartTime = timeShiftSettingCurrent.ShiftStartTime;
                        model.TomorrowShiftEndTime = timeShiftSettingCurrent.ShiftEndtTime;
                        model.TomorrowIsRestShift = await _kscHrUnitOfWork.TimeShiftSettingRepository.IsRestShiftAsync(timeShiftSettingCurrent.ShiftSettingFromShiftboard,
                      timeShiftSettingCurrent.OfficialUnOfficialHolidayFromWorkCalendar
                          , false, workCalendarData.IsHoliday, timeShiftSettingCurrent.WorkCompanySettingId, workCalendarData.DayNumber);
                    }
                    else
                    {
                        ShiftConceptDetail shiftConceptDetailTemp = null;
                        var ShiftBoard = _kscHrUnitOfWork.ShiftBoardRepository.GetShiftBoardByworkGoupIdWorkCalendarId(employeeWorkGroup.WorkGroupId, workCalendarData.WorkCalendarId);
                        if (ShiftBoard != null)
                            shiftConceptDetailTemp = ShiftBoard.ShiftConceptDetail;
                        if (shiftConceptDetailTemp == null || shiftConceptDetailTemp.Id == 0)
                        {
                            throw new HRBusinessException(Validations.NotFoundId, String.Format(Validations.NotFound, "اطلاعات روز بعد شیفت کاری وجود ندارد"));
                        }
                        shiftConceptDetail = _mapper.Map<SearchShiftConceptDetailModel>(shiftConceptDetailTemp);
                    }
                }
                else
                {
                    shiftConceptDetail = await _shiftConceptDetailService.GetShiftConceptDetailWithWOrkGroupIdDate(employeeWorkGroup.WorkGroupId, workCalendarData.WorkCalendarId);
                    if (shiftConceptDetail == null || shiftConceptDetail.Id == 0)
                    {
                        throw new HRBusinessException(Validations.NotFoundId, String.Format(Validations.NotFound, "اطلاعات روز بعد شیفت کاری وجود ندارد"));
                    }

                }
            }
            //
            if (shiftConceptDetail != null && shiftConceptDetail.Id != 0)
            {
                var timeShiftSetting = timeShiftSettingByWorkCityIdModel.FirstOrDefault(x => x.WorktimeId == employeeWorkGroup.WorkGroup.WorkTimeId && x.ShiftConceptId == shiftConceptDetail.ShiftConceptId
                                                              && x.IsTemporaryTime == false && x.ValidityStartDate.Value.Date <= tomorrowDate.Date && x.ValidityEndDate.Value.Date >= tomorrowDate.Date);
                if (timeShiftSetting == null)
                {
                    throw new HRBusinessException(Validations.NotFoundId, String.Format(Validations.NotFound, "اطلاعات تنظیمات شیفت کاری روز بعدی وجود ندارد"));
                }
                model.TomorrowShiftStartTime = timeShiftSetting.ShiftStartTime;
                model.TomorrowShiftEndTime = timeShiftSetting.ShiftEndtTime;
                model.TomorrowShiftStartTimeToTimeSpan = timeShiftSetting.ShiftStartTime.ConvertStringToTimeSpan();
                model.TomorrowShiftEndTimeToTimeSpan = timeShiftSetting.ShiftEndtTime.ConvertStringToTimeSpan();
                model.TomorrowIsRestShift = await _kscHrUnitOfWork.TimeShiftSettingRepository.IsRestShiftAsync(timeShiftSetting.ShiftSettingFromShiftboard,
                timeShiftSetting.OfficialUnOfficialHolidayFromWorkCalendar
                    , timeShiftSetting.ShiftConceptIsRest, workCalendarData.IsHoliday, timeShiftSetting.WorkCompanySettingId, workCalendarData.DayNumber);
                model.TomorrowShiftConceptId = shiftConceptDetail.ShiftConceptId;
                var ShiftEndDate = tomorrowDate;
                if (model.TomorrowShiftStartTime.ConvertStringToTimeSpan() > model.TomorrowShiftEndTime.ConvertStringToTimeSpan())
                {
                    ShiftEndDate = ShiftEndDate.AddDays(1);
                }
                model.ShiftEndDate = ShiftEndDate;
            }
            //
            model.TomorrowWorkTimeId = employeeWorkGroup.WorkGroup.WorkTimeId;
            return model;
        }
        private void EditEntryExitResult(List<EmployeeEntryExitViewModel> entryExitResult, TimeSettingDataModel timeSettingDataModel)
        {
            foreach (var item in entryExitResult)
            {
                if (
                    item.ExitDate.Date == timeSettingDataModel.ShiftEndDate.Date &&
                    item.ExitTimeToTimeSpan >= timeSettingDataModel.TemporaryShiftEndtTimeWithToleranceToTimeSpan &&
                    item.ExitTimeToTimeSpan < timeSettingDataModel.TemporaryShiftEndtTimeRealToTimeSpan

                    )
                {
                    if (!entryExitResult.Any(x => item.EntryDate.Date == timeSettingDataModel.ShiftStartDate.Date &&
                    item.ExitDate.Date == timeSettingDataModel.ShiftEndDate.Date &&
                    x.EntryTimeToTimeSpan > item.ExitTimeToTimeSpan &&
                    x.EntryTimeToTimeSpan <= timeSettingDataModel.ShiftEndTimeToTimeSpan
                    ))
                    {

                        item.ExitTimeToTimeSpan = timeSettingDataModel.TemporaryShiftEndtTimeRealToTimeSpan;
                        item.ExitTime = timeSettingDataModel.TemporaryShiftEndtTimeReal;
                    }
                    break;
                }
            }
        }
        #region panel left taeed karkard


        #region  // left taeed karkard wirh 2 Actions

        //personals////////////////////////////////  okkkkkk
        public async Task<TeamAndPersonelsFunctionalDetailsModel> GetPersonelsFunctionalDetails(EmployeeEntryExitManagementInputModel inputModel)
        {

            var result = new TeamAndPersonelsFunctionalDetailsModel();
            try
            {
                //مبنای اضافه کار تیم کاری - سقف و میانگین
                var teamWork = _kscHrUnitOfWork.TeamWorkRepository.GetByCode(inputModel.TeamWorkCode);
                if (teamWork != null)
                {
                    result.OvertimeCeiling = teamWork.OverTimeDefinition.MaximumDuration;
                    result.OvertimeCeilingDuration = teamWork.OverTimeDefinition.MaximumDurationMinute;

                    result.EmployeeAverageOvertime = teamWork.OverTimeDefinition.AverageDuration;
                }


                inputModel.EntryExitDate = inputModel.EntryExitDateString.Fa2En().ToGregorianDateTime().Value;
                var miladiStartcurrentMonth = inputModel.EntryExitDate.GetPersianMonthStartAndEndDates().StartDate;
                var miladiEndDaycurrentMonth = inputModel.EntryExitDate.GetPersianMonthStartAndEndDates().EndDate;
                int yearMonth = inputModel.EntryExitDate.GetYearMonthShamsiByMiladiDate();


                var tempEmployeeOvertime = 0;
                var attendAbcenseItem = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.
                    GetEmployeeAttendAbsenceItemActiveByEmployeeIdAsNoTracking(inputModel.EmployeeId)
                    .Select(x => new { x.WorkCalendarId, x.RollCallDefinitionId, x.IncreasedTimeDuration, x.TimeDurationInMinute })
                    ;
                var calendar = _kscHrUnitOfWork.WorkCalendarRepository
                    .GetWorkCalendarByRangeDateAsNotracking(miladiStartcurrentMonth, miladiEndDaycurrentMonth)
                    .Select(x => new { x.Id })
                    ;
                var RollCallDefinition = _kscHrUnitOfWork.RollCallDefinitionRepository.GetRollCallDefinitionAsNoTracking()
                    .Select(x => new { x.Id, x.RollCallCategoryId })
                    ;
                var employeeAttendAbsenceItem = (from item in attendAbcenseItem
                                                 join calnedar in calendar
                                                 on item.WorkCalendarId equals calnedar.Id
                                                 join rollCall in RollCallDefinition on item.RollCallDefinitionId equals rollCall.Id
                                                 group item by new { RollCallDefinitionId = item.RollCallDefinitionId, RollCallCategoryId = rollCall.RollCallCategoryId } into newgroup
                                                 select new EmployeeAttendAbcenseItemGroupModel()
                                                 {
                                                     RollCallDefinitionId = newgroup.Key.RollCallDefinitionId,
                                                     RollCallCategoryId = newgroup.Key.RollCallCategoryId,
                                                     //     TotalDuration =  newgroup.Sum(x => x.TimeDurationInMinute.Value)
                                                     TotalDuration = newgroup.Sum(x => x.IncreasedTimeDuration != null ? x.TimeDurationInMinute.Value + x.IncreasedTimeDuration.Value : x.TimeDurationInMinute.Value)
                                                 }).ToList();
                //item.IncreasedTimeDuration != null ? Utility.ConvertMinuteIn24ToDuration(item.TimeDurationInMinute.Value + item.IncreasedTimeDuration.Value) : item.TimeDuration,

                if (inputModel.EmployeeId > 0)
                {
                    //موقتا کامنت شده
                    var ShiftStartEndTime = await GetShiftStartEndTime(inputModel.ShiftConceptDetailId, inputModel.WorkCityId, inputModel.WorkGroupId, inputModel.WorkCalendarId);
                    result.ShiftStartTime = ShiftStartEndTime.ShiftStartTime;
                    result.ShiftEndTime = ShiftStartEndTime.ShiftEndTime;
                    //

                    var timeSheetSettingActive = await _kscHrUnitOfWork.TimeSheetSettingRepository.GetTimeSheetSettingActiveAsync();
                    //LeaveBalance
                    var vacationEntitlementTimePerMonth = timeSheetSettingActive.VacationEntitlementTimePerMonth;

                    var MeritVacationRemaining = EnumVacation.MeritVacationRemaining.Id.ToString();
                    var remianingVacationInLastMonth =
                        _kscHrUnitOfWork.EmployeeVacationManagementRepository.GetAllQueryable().AsNoTracking().Include(a => a.Vacation)
                        .FirstOrDefault(a => a.EmployeeId == inputModel.EmployeeId && a.Vacation.Code == MeritVacationRemaining).Duration.Value;
                    var sumTimeDuration = employeeAttendAbsenceItem.Where(x => x.RollCallCategoryId == EnumRollCallCategory.VacationDaily.Id
                             || x.RollCallCategoryId == EnumRollCallCategory.VacationHours.Id).Sum(w => w.TotalDuration);
                    int remianingvacation = Convert.ToInt32((vacationEntitlementTimePerMonth.ConvertDurationToMinute() + remianingVacationInLastMonth) - sumTimeDuration);

                    //

                    var LeaveBalanceFinal = remianingvacation != null ? remianingvacation.ConvertMinToDaysHour(timeSheetSettingActive.WorkDayDuration.ConvertStringToTimeSpan().TotalMinutes) : "0";

                    var _sumWorkExtra = Convert.ToInt32(employeeAttendAbsenceItem.Where(x => ConstRollCallCategory.OverTime.Any(o => o == x.RollCallCategoryId)).Sum(w => w.TotalDuration));//اضافه کاری
                    result.LeaveBalance = LeaveBalanceFinal;// مانده مرخصی

                    int SumForcedOverTimeAttendAbsence_int = _kscHrUnitOfWork.MonthTimeSheetDraftRepository.GetForcedOverTimeByEmployeeIdYearMonth(inputModel.EmployeeId, yearMonth);
                    result.ForcedOverTime = SumForcedOverTimeAttendAbsence_int.ConvertMinuteToTime();//اضافه کار قهری
                    int sumJanbaziWorkExtra = Convert.ToInt32(employeeAttendAbsenceItem.Where(x => x.RollCallDefinitionId == EnumRollCallDefinication.janbaziExtraWork.Id).Sum(w => w.TotalDuration));
                    var sumJanbaziWorkExtra_int = sumJanbaziWorkExtra + SumForcedOverTimeAttendAbsence_int;
                    result.TotalVeteranOvertime = sumJanbaziWorkExtra_int.ConvertMinuteToTime();//مجموع جانبازی و قهری
                                                                                                //
                    var rollCallDefinitionCeilingOvertime = _kscHrUnitOfWork.IncludedRollCallRepository.GetAllByRelated()
                   .Where(x => x.IncludedDefinitionId == EnumIncludedDefinition.MaximunOverTime.Id
                   && (x.RollCallDefinition.RollCallCategoryId == EnumRollCallCategory.OverTime.Id
                    || x.RollCallDefinition.RollCallCategoryId == EnumRollCallCategory.DefaultOverTime.Id
                    || x.RollCallDefinition.RollCallCategoryId == EnumRollCallCategory.OverTimeInHoliday.Id
                    || x.RollCallDefinition.RollCallCategoryId == EnumRollCallCategory.OverTimeInUnOfficialHoliday.Id
                    )
                   ).Select(w => w.RollCallDefinitionId).ToList();
                    var sumCeilingOvertime = Convert.ToInt32(employeeAttendAbsenceItem.Where(x => rollCallDefinitionCeilingOvertime.Any(a => a == x.RollCallDefinitionId)).Sum(w => w.TotalDuration));
                    //
                    var sumCeilingOvertime_int = sumCeilingOvertime + SumForcedOverTimeAttendAbsence_int;
                    result.TotalCeilingOvertime = sumCeilingOvertime_int.ConvertMinuteToTime(); //اضافه کاری مشمول سقف و قهری
                    result.TotalCeilingOvertimeDuration = sumCeilingOvertime_int;
                    result.TotalAllOvertime = (
                                     SumForcedOverTimeAttendAbsence_int +
                                         _sumWorkExtra

                                         ).ConvertMinuteToTime();




                }
            }
            catch (Exception ex)
            {

            }


            return result;


        }


        //team//////////////////////////////////////  okkkkkk
        public async Task<TeamAndPersonelsFunctionalDetailsModel> GetTeamFunctionalDetails(EmployeeEntryExitManagementInputModel inputModel)
        {

            var result = new TeamAndPersonelsFunctionalDetailsModel();
            try
            {
                if (inputModel.TeamWorkCode != "0" && !string.IsNullOrWhiteSpace(inputModel.TeamWorkCode))
                { //مبنای اضافه کار تیم کاری - سقف و میانگین
                    var teamWork = _kscHrUnitOfWork.TeamWorkRepository.GetByCode(inputModel.TeamWorkCode);
                    if (teamWork != null)
                    {
                        result.OvertimeCeiling = teamWork.OverTimeDefinition.MaximumDuration;
                        result.OvertimeCeilingDuration = teamWork.OverTimeDefinition.MaximumDurationMinute;

                        result.EmployeeAverageOvertime = teamWork.OverTimeDefinition.AverageDuration;
                    }

                    inputModel.EntryExitDate = inputModel.EntryExitDateString.Fa2En().ToGregorianDateTime().Value;
                    var miladiStartcurrentMonth = inputModel.EntryExitDate.GetPersianMonthStartAndEndDates().StartDate;
                    var miladiEndDaycurrentMonth = inputModel.EntryExitDate.GetPersianMonthStartAndEndDates().EndDate;

                    //میانگین اضافه کار تیم کاری
                    var res = sumCeilingOvertime_AverageOverTimeNew(inputModel, teamWork.Id);
                    result.EmployeeOvertime = res.EmployeeOvertime;
                    result.TeamEmployeeisCount = res.TeamEmployeeisCount;
                    result.TeamAverageOvertime = res.TeamAverageOvertime;
                }

            }
            catch (Exception ex)
            {

            }


            return result;
        }

        #endregion



        // panel left taeed karkard with one Action
        public async Task<TeamAndPersonelsFunctionalDetailsModel> GetTeamAndPersonelsFunctionalDetails(EmployeeEntryExitManagementInputModel inputModel)
        {


            var result = new TeamAndPersonelsFunctionalDetailsModel();
            try
            {
                if (inputModel.EmployeeId > 0)
                {
                    result = await GetPersonelsFunctionalDetails(inputModel);

                }
                //مبنای اضافه کار تیم کاری - سقف و میانگین
                var teamWork = _kscHrUnitOfWork.TeamWorkRepository.GetByCode(inputModel.TeamWorkCode);
                if (teamWork != null)
                {
                    result.OvertimeCeiling = teamWork.OverTimeDefinition.MaximumDuration;
                    result.OvertimeCeilingDuration = teamWork.OverTimeDefinition.MaximumDurationMinute;

                    result.EmployeeAverageOvertime = teamWork.OverTimeDefinition.AverageDuration;
                }

                inputModel.EntryExitDate = inputModel.EntryExitDateString.Fa2En().ToGregorianDateTime().Value;
                var miladiStartcurrentMonth = inputModel.EntryExitDate.GetPersianMonthStartAndEndDates().StartDate;
                var miladiEndDaycurrentMonth = inputModel.EntryExitDate.GetPersianMonthStartAndEndDates().EndDate;
                //میانگین اضافه کار تیم کاری
                var res = sumCeilingOvertime_AverageOverTimeNew(inputModel, teamWork.Id);
                result.EmployeeOvertime = res.EmployeeOvertime;
                result.TeamEmployeeisCount = res.TeamEmployeeisCount;
                result.TeamAverageOvertime = res.TeamAverageOvertime;
            }
            catch (Exception ex)
            {

            }


            return result;
        }

        public async Task<int?> SumForcedOverTimeAttendAbsence(EmployeeEntryExitManagementInputModel inputModel, List<EmployeeAttendAbcenseItemGroupModel> employeeAttendAbcenseItemGroupModel)
        {


            var workGroup = await _kscHrUnitOfWork.WorkGroupRepository.GetWorkGroupsByWorkTimeRelations(inputModel.WorkGroupId);
            var workTime = workGroup.WorkTime;
            #region محاسبه اضافه کار قهری از آیتم
            var WorkCalendar = _kscHrUnitOfWork.WorkCalendarRepository.GetById(inputModel.WorkCalendarId);
            var yearMonth = WorkCalendar.YearMonthV1;
            //var timeShiftSettingForcedOverTimeModel = _timeShiftSettingService.GetDataForcedOverTime(WorkCalendar.MiladiDateV1);
            var timeShiftSettingForcedOverTimeModel = _kscHrUnitOfWork.TimeShiftSettingRepository.GetAllByIncludedAsNotracking(WorkCalendar.MiladiDateV1)
                                                         .Select(x => new ForcedOverTimeModel()
                                                         {
                                                             ShiftConceptId = x.WorkCompanySetting.WorkTimeShiftConcept.ShiftConceptId,
                                                             WorkTimeId = x.WorkCompanySetting.WorkTimeShiftConcept.WorkTimeId,
                                                             WorkCityId = x.WorkCompanySetting.WorkCityId,
                                                             ForcedOverTime = x.ForcedOverTime,
                                                             TotalWorkHourInWeek = x.TotalWorkHourInWeek
                                                         }).ToList();
            var rollCallDefinitionIds = _kscHrUnitOfWork.IncludedRollCallRepository.GetAllQueryable()
                 .Where(x => x.IncludedDefinitionId == EnumIncludedDefinition.ForcedOverTime.Id
                 ).Select(w => w.RollCallDefinitionId).ToList();

            var employeeAttendAbsenceItem = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetEmployeeAttendAbsenceItemIncludedShiftConceptDetail()
                .Where(x => x.InvalidRecord == false)
                .Where(x => x.EmployeeId == inputModel.EmployeeId && x.WorkCalendar.YearMonthV1 == yearMonth
                && rollCallDefinitionIds.Contains(x.RollCallDefinitionId)
                ).ToList();

            var timeDurations = employeeAttendAbcenseItemGroupModel.Where(x => rollCallDefinitionIds.Any(a => a == x.RollCallDefinitionId)).Sum(x => x.TotalDuration);
            #endregion
            int? sumTimeDurationShift = 0;
            int? sumTimeDuration = 0;
            int? x = 0;

            sumTimeDuration = Convert.ToInt32(timeDurations);
            //•	اضافه کار  قهری پرسنل روزکار - 
            var shiftConceptDetail = await _kscHrUnitOfWork.ShiftConceptDetailRepository.GetByIdAsync(inputModel.ShiftConceptDetailId);
            if (workTime.ShiftSettingFromShiftboard == false)
            {
                var timeShiftSettingForcedOverTime = timeShiftSettingForcedOverTimeModel
                                          .First(x => x.ShiftConceptId == shiftConceptDetail.ShiftConceptId
                                              && x.WorkTimeId == workGroup.WorkTimeId && x.WorkCityId == inputModel.WorkCityId);
                //.TotalWorkHourInWeek;
                var TotalWorkHourInWeek = timeShiftSettingForcedOverTime.TotalWorkHourInWeek;
                var TotalWorkHourInWeekInMinute = TotalWorkHourInWeek.ConvertDurationToMinute();  //** 45:00 h =2700 min
                var MaximumForcedOverTime = timeShiftSettingForcedOverTime.ForcedOverTime.ConvertDurationToMinute();
                var MaximumForcedOverTime1 = workTime.MaximumForcedOverTime.ConvertDurationToMinute();
                //**  04:00
                //محاسبه
                // double Div = 0;
                //double mod = 0;
                //var x = Math.DivRem(sumTimeDuration, mod,);
                //var mindiv=x.Quotient.ConvertStringToTimeSpan().TotalMinutes;
                var div = (sumTimeDuration / TotalWorkHourInWeekInMinute) * 60;//خارج قسمت

                var mod = sumTimeDuration % TotalWorkHourInWeekInMinute;//باقیمانده
                if (mod > 1440)
                {
                    x = 60;//60min=1 h

                }

                sumTimeDuration = x + div;

                if (sumTimeDuration < MaximumForcedOverTime)
                {
                    sumTimeDuration = sumTimeDuration;
                }
                else
                {
                    sumTimeDuration = MaximumForcedOverTime;
                }

            }
            else //•	اضافه کار  قهری پرسنل شیفت - 
            {
                var shiftForcedOverTimeModel = employeeAttendAbsenceItem.GroupBy(x => new { x.ShiftConceptDetail_ShiftConceptDetailId.ShiftConceptId }).Select(y => new ShiftForcedOverTimeModel()
                { ShiftConceptId = y.Key.ShiftConceptId, Count = y.Count() }).ToList();
                foreach (var item in shiftForcedOverTimeModel)
                {
                    var forcedOverTime = timeShiftSettingForcedOverTimeModel.First(x => x.ShiftConceptId == item.ShiftConceptId && x.WorkTimeId == workGroup.WorkTimeId && x.WorkCityId == inputModel.WorkCityId).ForcedOverTime;
                    if (forcedOverTime != null)
                    {
                        sumTimeDurationShift += item.Count * forcedOverTime.ConvertDurationToMinute();
                    }
                }
                sumTimeDuration = sumTimeDurationShift;
            }

            return sumTimeDuration;
        }

        //•	اضافه کار  قهری پرسنل  forcedTime Over
        public async Task<int?> SumForcedOverTimeAttendAbsence(EmployeeEntryExitManagementInputModel inputModel)
        {


            var workGroup = await _kscHrUnitOfWork.WorkGroupRepository.GetWorkGroupsByWorkTimeRelations(inputModel.WorkGroupId);
            var workTime = workGroup.WorkTime;
            #region محاسبه اضافه کار قهری از آیتم
            var WorkCalendar = _kscHrUnitOfWork.WorkCalendarRepository.GetById(inputModel.WorkCalendarId);
            var yearMonth = WorkCalendar.YearMonthV1;
            //var timeShiftSettingForcedOverTimeModel = _timeShiftSettingService.GetDataForcedOverTime(WorkCalendar.MiladiDateV1);
            var timeShiftSettingForcedOverTimeModel = _kscHrUnitOfWork.TimeShiftSettingRepository.GetAllByIncludedAsNotracking(WorkCalendar.MiladiDateV1)
                                                         .Select(x => new ForcedOverTimeModel()
                                                         {
                                                             ShiftConceptId = x.WorkCompanySetting.WorkTimeShiftConcept.ShiftConceptId,
                                                             WorkTimeId = x.WorkCompanySetting.WorkTimeShiftConcept.WorkTimeId,
                                                             WorkCityId = x.WorkCompanySetting.WorkCityId,
                                                             ForcedOverTime = x.ForcedOverTime,
                                                             TotalWorkHourInWeek = x.TotalWorkHourInWeek
                                                         }).ToList();
            var rollCallDefinitionIds = _kscHrUnitOfWork.IncludedRollCallRepository.GetAllByRelated()
                 .Where(x => x.IncludedDefinitionId == EnumIncludedDefinition.ForcedOverTime.Id
                 ).Select(w => w.RollCallDefinitionId).ToList();

            var employeeAttendAbsenceItem = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetEmployeeAttendAbsenceItemIncludedShiftConceptDetail()
                .Where(x => x.InvalidRecord == false)
                .Where(x => x.EmployeeId == inputModel.EmployeeId && x.WorkCalendar.YearMonthV1 == yearMonth
                && rollCallDefinitionIds.Contains(x.RollCallDefinitionId)
                ).ToList();

            var timeDurations = employeeAttendAbsenceItem.Sum(x => x.TimeDuration.ConvertStringToTimeSpan().TotalMinutes);
            #endregion
            int? sumTimeDurationShift = 0;
            int? sumTimeDuration = 0;
            int? x = 0;

            sumTimeDuration = Convert.ToInt32(timeDurations);
            //•	اضافه کار  قهری پرسنل روزکار - 
            var shiftConceptDetail = await _kscHrUnitOfWork.ShiftConceptDetailRepository.GetByIdAsync(inputModel.ShiftConceptDetailId);
            if (workTime.ShiftSettingFromShiftboard == false)
            {
                var timeShiftSettingForcedOverTime = timeShiftSettingForcedOverTimeModel
                                          .First(x => x.ShiftConceptId == shiftConceptDetail.ShiftConceptId
                                              && x.WorkTimeId == workGroup.WorkTimeId && x.WorkCityId == inputModel.WorkCityId);
                //  .TotalWorkHourInWeek;
                var TotalWorkHourInWeek = timeShiftSettingForcedOverTime.TotalWorkHourInWeek;
                var TotalWorkHourInWeekInMinute = TotalWorkHourInWeek.ConvertDurationToMinute();  //** 45:00 h =2700 min
                var MaximumForcedOverTime = timeShiftSettingForcedOverTime.ForcedOverTime.ConvertDurationToMinute();
                var MaximumForcedOverTime1 = workTime.MaximumForcedOverTime.ConvertDurationToMinute();
                //**  04:00
                //محاسبه
                // double Div = 0;
                //double mod = 0;
                //var x = Math.DivRem(sumTimeDuration, mod,);
                //var mindiv=x.Quotient.ConvertStringToTimeSpan().TotalMinutes;
                var div = (sumTimeDuration / TotalWorkHourInWeekInMinute) * 60;//خارج قسمت

                //var minDiv = div.ConvertStringToTimeSpan().TotalMinutes;
                var mod = sumTimeDuration % TotalWorkHourInWeekInMinute;//باقیمانده
                if (mod > 1440)
                {
                    x = 60;//60min=1 h

                }

                sumTimeDuration = x + div;

                if (sumTimeDuration < MaximumForcedOverTime)
                {
                    sumTimeDuration = sumTimeDuration;
                }
                else
                {
                    sumTimeDuration = MaximumForcedOverTime;
                }

            }
            else //•	اضافه کار  قهری پرسنل شیفت - 
            {
                var shiftForcedOverTimeModel = employeeAttendAbsenceItem.GroupBy(x => new { x.ShiftConceptDetail_ShiftConceptDetailId.ShiftConceptId }).Select(y => new ShiftForcedOverTimeModel()
                { ShiftConceptId = y.Key.ShiftConceptId, Count = y.Count() }).ToList();
                foreach (var item in shiftForcedOverTimeModel)
                {
                    var forcedOverTime = timeShiftSettingForcedOverTimeModel.First(x => x.ShiftConceptId == item.ShiftConceptId && x.WorkTimeId == workGroup.WorkTimeId && x.WorkCityId == inputModel.WorkCityId).ForcedOverTime;
                    if (forcedOverTime != null)
                    {
                        sumTimeDurationShift += item.Count * forcedOverTime.ConvertDurationToMinute();
                    }
                }
                sumTimeDuration = sumTimeDurationShift;
            }

            return sumTimeDuration;
        }

        //جمع مرخصی های یک فرد در تایید کارکرد TimeDuration
        public int? sumTimeDuration(EmployeeEntryExitManagementInputModel inputModel)
        {

            var miladiStartcurrentMonth = inputModel.EntryExitDate.GetPersianMonthStartAndEndDates().StartDate;
            var miladiEndDaycurrentMonth = inputModel.EntryExitDate.GetPersianMonthStartAndEndDates().EndDate;

            int? sumTimeDuration = 0;
            if (inputModel.EmployeeId > 0)
            {
                //لیست ای دی هایی که به کتگوری مرخص 5 وصل است
                var rollCallDefinitionIds = _kscHrUnitOfWork.RollCallDefinitionRepository.GetAllQueryable()
                    .Where(x => x.RollCallCategoryId == EnumRollCallCategory.VacationDaily.Id
                             || x.RollCallCategoryId == EnumRollCallCategory.VacationHours.Id).Select(w => w.Id).ToList();

                var employeeAttendAbsenceItem = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetEmployeeAttendAbsenceItemByRelated().AsNoTracking()
                    .Where(x => x.InvalidRecord == false)
                    .Where(x => x.EmployeeId == inputModel.EmployeeId && x.WorkCalendar.MiladiDateV1 >= miladiStartcurrentMonth
                    && x.WorkCalendar.MiladiDateV1 <= miladiEndDaycurrentMonth
                    && rollCallDefinitionIds.Contains(x.RollCallDefinitionId)
                    ).ToList();

                var timeDurations = employeeAttendAbsenceItem.Sum(x => x.TimeDuration.ConvertStringToTimeSpan().TotalMinutes);
                sumTimeDuration = Convert.ToInt32(timeDurations);
            }
            return sumTimeDuration;
        }

        //مجموع اضافه کاری   TimeDuration
        public int? sumWorkExtra(EmployeeEntryExitManagementInputModel inputModel, List<EmployeeAttendAbsenceItem> model)
        {
            int? sumWorkExtra = 0;
            if (inputModel.EmployeeId > 0)
            {
                var employeeAttendAbsenceItem = model.Where(x => x.EmployeeId == inputModel.EmployeeId);
                var timeDurations = employeeAttendAbsenceItem.Sum(x => x.TimeDuration.ConvertStringToTimeSpan().TotalMinutes);
                sumWorkExtra = Convert.ToInt32(timeDurations);
            }
            return sumWorkExtra;
        }


        //مجموع اضافه کاری  جانبازی TimeDuration
        public int? sumJanbaziWorkExtra(EmployeeEntryExitManagementInputModel inputModel)
        {
            var miladiStartcurrentMonth = inputModel.EntryExitDate.GetPersianMonthStartAndEndDates().StartDate;
            var miladiEndDaycurrentMonth = inputModel.EntryExitDate.GetPersianMonthStartAndEndDates().EndDate;

            int? sumWorkExtra = 0;
            if (inputModel.EmployeeId > 0)
            {

                var employeeAttendAbsenceItem = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetEmployeeAttendAbsenceItemByRelated().AsNoTracking()
                    .Where(x => x.InvalidRecord == false)
                    .Where(x => x.EmployeeId == inputModel.EmployeeId && x.WorkCalendar.MiladiDateV1 >= miladiStartcurrentMonth
                    && x.WorkCalendar.MiladiDateV1 <= miladiEndDaycurrentMonth
                    && x.RollCallDefinitionId == EnumRollCallDefinication.janbaziExtraWork.Id
                    ).ToList();

                var timeDurations = employeeAttendAbsenceItem.Sum(x => x.TimeDuration.ConvertStringToTimeSpan().TotalMinutes);
                sumWorkExtra = Convert.ToInt32(timeDurations);
            }
            return sumWorkExtra;
        }
        //اضافه کاری مشمول سقف   TimeDuration_AverageOverTime
        public int? sumCeilingOvertime_AverageOverTime(EmployeeEntryExitManagementInputModel inputModel)
        {
            var miladiStartcurrentMonth = inputModel.EntryExitDate.GetPersianMonthStartAndEndDates().StartDate;
            var miladiEndDaycurrentMonth = inputModel.EntryExitDate.GetPersianMonthStartAndEndDates().EndDate;
            int yearMonth = inputModel.EntryExitDate.GetYearMonthShamsiByMiladiDate();
            //var teamWork = _kscHrUnitOfWork.TeamWorkRepository.GetByCode(inputModel.TeamWorkCode);

            var tempEmployeeOvertime = 0;
            var attendAbcenseItem = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetEmployeeAttendAbsenceItemActiveByEmployeeIdAsNoTracking(inputModel.EmployeeId);
            var calendar = _kscHrUnitOfWork.WorkCalendarRepository.GetWorkCalendarByRangeDateAsNotracking(miladiStartcurrentMonth, miladiEndDaycurrentMonth);
            var RollCallDefinition = _kscHrUnitOfWork.RollCallDefinitionRepository.GetRollCallDefinitionAsNoTracking();
            var employeeAttendAbsenceItem = (from item in attendAbcenseItem
                                             join calnedar in calendar
                                             on item.WorkCalendarId equals calnedar.Id
                                             join rollCall in RollCallDefinition on item.RollCallDefinitionId equals rollCall.Id
                                             group item by new { RollCallDefinitionId = item.RollCallDefinitionId, RollCallCategoryId = rollCall.RollCallCategoryId } into newgroup
                                             select new EmployeeAttendAbcenseItemGroupModel()
                                             {
                                                 RollCallDefinitionId = newgroup.Key.RollCallDefinitionId,
                                                 RollCallCategoryId = newgroup.Key.RollCallCategoryId,
                                                 TotalDuration = newgroup.Sum(x => x.TimeDurationInMinute.Value)
                                             }).ToList();


            // int SumForcedOverTimeAttendAbsence_int = _kscHrUnitOfWork.MonthTimeSheetDraftRepository.GetForcedOverTimeByEmployeeIdYearMonth(inputModel.EmployeeId, yearMonth);
            //var rollCallDefinitionCeilingOvertime = _kscHrUnitOfWork.IncludedRollCallRepository.GetAllByRelated()
            //      .Where(x => x.IncludedDefinitionId == EnumIncludedDefinition.AverageOverTime.Id
            //      && (x.RollCallDefinition.RollCallCategoryId == EnumRollCallCategory.OverTime.Id
            //       || x.RollCallDefinition.RollCallCategoryId == EnumRollCallCategory.DefaultOverTime.Id
            //       || x.RollCallDefinition.RollCallCategoryId == EnumRollCallCategory.OverTimeInHoliday.Id
            //       )
            //      ).Select(w => w.RollCallDefinitionId).ToList();
            var sumCeilingOvertime = Convert.ToInt32(employeeAttendAbsenceItem.Where(x => inputModel.RollCallDefinitionCeilingOvertime.Any(a => a == x.RollCallDefinitionId)).Sum(w => w.TotalDuration));
            ////
            var sumCeilingOvertime_int = sumCeilingOvertime;// + SumForcedOverTimeAttendAbsence_int;

            return sumCeilingOvertime_int;
        }

        //اضافه کاری مشمول سقف   TimeDuration
        public int? sumCeilingOvertime(EmployeeEntryExitManagementInputModel inputModel)
        {
            var miladiStartcurrentMonth = inputModel.EntryExitDate.GetPersianMonthStartAndEndDates().StartDate;
            var miladiEndDaycurrentMonth = inputModel.EntryExitDate.GetPersianMonthStartAndEndDates().EndDate;
            int yearMonth = inputModel.EntryExitDate.GetYearMonthShamsiByMiladiDate();
            // var teamWork = _kscHrUnitOfWork.TeamWorkRepository.GetByCode(inputModel.TeamWorkCode);

            var tempEmployeeOvertime = 0;
            var attendAbcenseItem = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetEmployeeAttendAbsenceItemActiveByEmployeeIdAsNoTracking(inputModel.EmployeeId);
            var calendar = _kscHrUnitOfWork.WorkCalendarRepository.GetWorkCalendarByRangeDateAsNotracking(miladiStartcurrentMonth, miladiEndDaycurrentMonth);
            var RollCallDefinition = _kscHrUnitOfWork.RollCallDefinitionRepository.GetRollCallDefinitionAsNoTracking();
            var employeeAttendAbsenceItem = (from item in attendAbcenseItem
                                             join calnedar in calendar
                                             on item.WorkCalendarId equals calnedar.Id
                                             join rollCall in RollCallDefinition on item.RollCallDefinitionId equals rollCall.Id
                                             group item by new { RollCallDefinitionId = item.RollCallDefinitionId, RollCallCategoryId = rollCall.RollCallCategoryId } into newgroup
                                             select new EmployeeAttendAbcenseItemGroupModel()
                                             {
                                                 RollCallDefinitionId = newgroup.Key.RollCallDefinitionId,
                                                 RollCallCategoryId = newgroup.Key.RollCallCategoryId,
                                                 //TotalDuration = newgroup.Sum(x => x.TimeDurationInMinute.Value)
                                                 TotalDuration = newgroup.Sum(x => x.IncreasedTimeDuration != null ? x.TimeDurationInMinute.Value + x.IncreasedTimeDuration.Value : x.TimeDurationInMinute.Value)
                                             }).ToList();


            int SumForcedOverTimeAttendAbsence_int = _kscHrUnitOfWork.MonthTimeSheetDraftRepository.GetForcedOverTimeByEmployeeIdYearMonth(inputModel.EmployeeId, yearMonth);
            var rollCallDefinitionCeilingOvertime = _kscHrUnitOfWork.IncludedRollCallRepository.GetAllByRelated()
                  .Where(x => x.IncludedDefinitionId == EnumIncludedDefinition.MaximunOverTime.Id
                  && (x.RollCallDefinition.RollCallCategoryId == EnumRollCallCategory.OverTime.Id
                   || x.RollCallDefinition.RollCallCategoryId == EnumRollCallCategory.DefaultOverTime.Id
                   || x.RollCallDefinition.RollCallCategoryId == EnumRollCallCategory.OverTimeInHoliday.Id
                   || x.RollCallDefinition.RollCallCategoryId == EnumRollCallCategory.OverTimeInUnOfficialHoliday.Id
                   )
                  ).Select(w => w.RollCallDefinitionId).ToList();
            var sumCeilingOvertime = Convert.ToInt32(employeeAttendAbsenceItem.Where(x => rollCallDefinitionCeilingOvertime.Any(a => a == x.RollCallDefinitionId)).Sum(w => w.TotalDuration));
            ////
            var sumCeilingOvertime_int = sumCeilingOvertime + SumForcedOverTimeAttendAbsence_int;

            return sumCeilingOvertime_int;
        }

        #endregion

        //ثبت تایید کارکرد روزهای تعطیل
        public async Task<MyKscResult> OfficialHolidayForItems(OfficialHolidayForItemsModel model)
        {
            var activeDirectoryLdapKind = _config["AuthenticationOption:ActiveDirectoryLdapKind"];
            var result = new MyKscResult();
            string scheduledName = "AutoLog";
            DateTime yesterday = System.DateTime.Now.AddDays(-1);
            string currentUserName = "AutoSystem";
            if (model.HolidayDate.HasValue)
            {
                yesterday = model.HolidayDate.Value;
                scheduledName = "ManualLog";
                currentUserName = model.CurrentUserName;
            }
            if (currentUserName == "AutoSystem" && activeDirectoryLdapKind != "1")//فقط برای فولادخوزستان بصورت اتواماتیک اجرا میشود
            {
                return result;
            }
            if (yesterday.Date >= System.DateTime.Now.Date)
            {
                result.AddError("", "ثبت اطلاعات برای تاریخ جاری امکان پذیر نمی باشد");
                return result;
            }



            var workCalendar = _kscHrUnitOfWork.WorkCalendarRepository.GetByMiladiDate(yesterday); //workCalendarid
                                                                                                   // var workCalendarid = _kscHrUnitOfWork.WorkCalendarRepository.GetByMiladiDate(yesterday); //workCalendarid
                                                                                                   //var p = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository
                                                                                                   // .GetAll().Where(x => x.WorkCalendarId == workCalendar.Id);//&& x.InsertUser == "AutoSystem");
                                                                                                   //if (p.Any())
                                                                                                   //{
                                                                                                   //    result.AddError("", "تایید کارکرد در این تاریخ انجام شده است");
                                                                                                   //    return result;
                                                                                                   //}
            var activePersonsIds = _kscHrUnitOfWork.EmployeeTeamWorkRepository.GetActivePersonsIdForOfficialHoliday(workCalendar.Id, yesterday);

            var workTimes = _kscHrUnitOfWork.WorkTimeRepository.GetAllQueryable().ToList();
            var employeeWorkGroups = from a in _kscHrUnitOfWork.EmployeeWorkGroupRepository.GetEmployeeWorkGroupByDateWithEmployeeCityId(yesterday)
                                     join b in activePersonsIds on a.employeeWorkGroup.EmployeeId equals b
                                     select a;

            var employeeWorkGroupsList = employeeWorkGroups.ToList();
            var shifHolidayDayNumbers = _kscHrUnitOfWork.ShiftHolidayRepository.GetShiftHolidayActive()
                                            .Select(x => x.DayNumber);

            var WorkGroupIds = employeeWorkGroups.Select(a => a.employeeWorkGroup.WorkGroupId).ToList();

            var ShiftConceptDetails = await _shiftConceptDetailService
                .GetShiftConceptDetailWithWOrkGroupIdDate(WorkGroupIds, workCalendar.Id);

            var getAllShiftStartEndTime = GetAllShiftSetting(yesterday);

            List<EmployeeAttendAbsenceItem> listEmployeeAttendAbsenceItem = new List<EmployeeAttendAbsenceItem>();
            var index = 0;
            var entryExit = _kscHrUnitOfWork.EmployeeEntryExitRepository.GetEmployeeEntryExitActiveByDate(yesterday).ToList();
            //var employeeEntryExit= entryExit.Select(x => x.EmployeeId).Distinct().ToList();
            // جمع آوری اطلاعات برای قهری

            var rollCallIncludedForcedOverTime = GetRollCallIncludedForcedOverTime();
            var employeeAttendAbsenceItems = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetEmployeeAttendAbsenceItemAsNoTracking();
            var workCalendars = _kscHrUnitOfWork.WorkCalendarRepository.GetWorkCalendarByYearMonthAsNoTracking(workCalendar.YearMonthV1);
            var queryEmployeeAttendAbsenceItemByEmployeeId = from item in employeeAttendAbsenceItems
                                                             join cal in workCalendars on item.WorkCalendarId equals cal.Id
                                                             join time in _kscHrUnitOfWork.WorkTimeRepository.GetAllQueryable() on item.WorkTimeId equals time.Id
                                                             where item.InvalidRecord == false && time.ShiftSettingFromShiftboard == false

                                                             select new EmployeeAttendAbsenceItemForcedOverTimeModel()
                                                             {
                                                                 EmployeeId = item.EmployeeId,
                                                                 EmployeeAttendAbsenceItemId = item.Id,
                                                                 RollCallDefinitionId = item.RollCallDefinitionId,
                                                                 TimeDurationInMinute = item.TimeDurationInMinute.Value,
                                                                 WorkTimeId = item.WorkTimeId,
                                                                 WorkCalendarId = item.WorkCalendarId,
                                                                 WorkCalendarDate = cal.MiladiDateV1

                                                             };
            var employeeAttendAbsenceItem = queryEmployeeAttendAbsenceItemByEmployeeId.ToList();
            var monthTimeSheetDraft = _kscHrUnitOfWork.MonthTimeSheetDraftRepository.GetMonthTimeSheetDraftByYearMonth(workCalendar.YearMonthV1);
            //
            List<MonthTimeSheetDraft> monthTimeSheetDraftList = new List<MonthTimeSheetDraft>();
            foreach (var item in activePersonsIds)
            {
                index++;
                try
                {
                    var employeeWorkGroup = employeeWorkGroupsList.FirstOrDefault(x => x.employeeWorkGroup.EmployeeId == item);
                    //
                    if (activeDirectoryLdapKind == "6")// "NIMID" 
                    {
                        if (workCalendar.DateKey == 14040522 && employeeWorkGroup.EmployeeCityId != 4 && employeeWorkGroup.EmployeeCityId != 7)
                        {
                            continue;
                        }
                        if (workCalendar.DateKey == 14040501 && employeeWorkGroup.EmployeeCityId != 1)
                        {
                            continue;
                        }
                    }
                    //
                    if (employeeWorkGroup.WorkGroupCode == null) continue;
                    var ShiftConceptDetail = ShiftConceptDetails.FirstOrDefault(a => a.WorkGroupCode == employeeWorkGroup.WorkGroupCode);
                    var getShiftStartEndTime = getAllShiftStartEndTime
                        .FirstOrDefault(a => a.WorkTimeId == employeeWorkGroup.WorkTimeId &&
                        a.ShiftCondeptId == ShiftConceptDetail.ShiftConceptId &&
                        a.WorkCityId == employeeWorkGroup.EmployeeCityId);

                    if (getShiftStartEndTime is null) continue;

                    if (employeeWorkGroup.ShiftSettingFromShiftboard == false) //شیفتی نباشد=روزکار
                    {
                        if (workCalendar.WorkDayType.IsHoliday == false && !shifHolidayDayNumbers.Contains(workCalendar.DayOfWeek))
                            continue; // هیچ کاری نکند
                        if (entryExit.Any(x => x.EmployeeId == item))
                            continue; // هیچ کاری نکند

                        //
                    }
                    else//شیفتی باشد
                    {
                        if (ShiftConceptDetail.IsRest == false &&
                            (employeeWorkGroup.OfficialUnOfficialHolidayFromWorkCalendar == false || workCalendar.WorkDayType.IsHoliday == false))//خواندن تعطیلات از روی تقویم کاری
                            continue; // هیچ کاری نکند
                                      //
                        if (ShiftConceptDetail.Id == 41) //استراحت شیفت(روزاول)  //6
                        {
                            if (entryExit.Any(x => x.EmployeeId == item && x.EntryExitType == 1)) // 
                                continue; // هیچ کاری نکند
                        }
                        else
                        {
                            if (entryExit.Any(x => x.EmployeeId == item))
                                continue; // هیچ کاری نکند
                        }
                        //
                    }

                    var rollCallDefinition = Utility.GetRollCallIdRest(employeeWorkGroup.ShiftSettingFromShiftboard,
                        workCalendar.WorkDayType.IsHoliday, workCalendar.DayOfWeek, employeeWorkGroup.OfficialUnOfficialHolidayFromWorkCalendar, ShiftConceptDetail.IsRest == true ? true : false);



                    var absenceItem =
                         new AddEmployeeAttendAbsenceItemModel()
                         {
                             WorkCalendarId = workCalendar.Id,
                             ShiftConceptDetailId = ShiftConceptDetail.Id,
                             ShiftConceptDetailIdInShiftBoard = ShiftConceptDetail.Id,
                             EmployeeId = item,
                             StartTime = getShiftStartEndTime.ShiftStartTime,
                             EndTime = getShiftStartEndTime.ShiftEndTime,
                             TimeDuration = getShiftStartEndTime.TotalWorkHourInDay,
                             RollCallDefinitionId = rollCallDefinition,
                             InsertDate = DateTime.Now,
                             CurrentUserName = currentUserName,
                             WorkTimeId = employeeWorkGroup.WorkTimeId.Value,
                             InvalidRecord = false,
                             InvalidRecordReason = "",
                             IsFloat = false,
                             IsManual = false,


                         };

                    listEmployeeAttendAbsenceItem.Add(_mapper.Map<EmployeeAttendAbsenceItem>(absenceItem));
                    // محاسبه اضافه کار قهری
                    if (employeeWorkGroup.ShiftSettingFromShiftboard == false)
                    {
                        var newRow = _mapper.Map<EmployeeAttendAbsenceItemForcedOverTimeModel>(absenceItem);
                        var employeeAttendAbsenceItemByEmployeeId = employeeAttendAbsenceItem.Where(x => x.EmployeeId == item && x.WorkTimeId == employeeWorkGroup.WorkTimeId.Value).ToList();
                        employeeAttendAbsenceItemByEmployeeId.Add(newRow);
                        var workTime = workTimes.First(x => x.Id == employeeWorkGroup.WorkTimeId);
                        var forcedOverTime = CalculateForcedOverTime(employeeAttendAbsenceItemByEmployeeId, rollCallIncludedForcedOverTime, workTime, getShiftStartEndTime.TotalWorkHourInWeek, getShiftStartEndTime.ForcedOverTime);
                        if (forcedOverTime != 0)
                        {
                            var monthTimeSheetDraftByEmployeeId = monthTimeSheetDraft.Where(x => x.EmployeeId == item && x.WorkTimeId == employeeWorkGroup.WorkTimeId.Value).FirstOrDefault();
                            if (monthTimeSheetDraftByEmployeeId != null)
                                monthTimeSheetDraftByEmployeeId.ForcedOverTime = forcedOverTime;
                            else
                            {
                                MonthTimeSheetDraft newMonthTimeSheetDraft = new MonthTimeSheetDraft();
                                newMonthTimeSheetDraft.EmployeeId = item;
                                newMonthTimeSheetDraft.YearMonth = workCalendar.YearMonthV1;
                                newMonthTimeSheetDraft.WorkTimeId = employeeWorkGroup.WorkTimeId.Value;
                                newMonthTimeSheetDraft.ForcedOverTime = forcedOverTime;
                                //_kscHrUnitOfWork.MonthTimeSheetDraftRepository.Add(newMonthTimeSheetDraft);
                                monthTimeSheetDraftList.Add(newMonthTimeSheetDraft);
                            }
                        }

                    }
                    //

                }

                catch (Exception x)
                {

                    throw;
                }
            }

            if (!listEmployeeAttendAbsenceItem.Any())
            {
                return result;

            }


            var tt = listEmployeeAttendAbsenceItem.Select(x => x.WorkTimeId);
            await _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.AddRangeAsync(listEmployeeAttendAbsenceItem);
            if (monthTimeSheetDraftList.Count() != 0)
                await _kscHrUnitOfWork.MonthTimeSheetDraftRepository.AddRangeAsync(monthTimeSheetDraftList);


            try
            {


                // جدید
                var LogData = _kscHrUnitOfWork.ScheduledLogRepository.Any(x => x.ScheduledDate.Value.Date == yesterday.Date);
                //.Where(x => x.ScheduledDate == yesterday);
                // if (LogData != null) continue;//هیچ کاری نکند
                if (LogData == true && scheduledName == "AutoLog")
                    return result;//هیچ کاری نکند
                                  //
                var scheduledLog = new ScheduledLog()
                {
                    ScheduledDate = yesterday,
                    InsertDate = DateTime.Now,
                    InsertUser = currentUserName,
                    ScheduledName = scheduledName

                };
                await _kscHrUnitOfWork.ScheduledLogRepository.AddAsync(scheduledLog);
                await _kscHrUnitOfWork.SaveAsync();



                result.count = listEmployeeAttendAbsenceItem.Count();
                return result;

            }

            catch (Exception ex)
            {

                KscResult kscResult = new KscResult();
                kscResult.AddError("خطا", ex.Message, 0);

                return result;

            }
        }
        public async Task<KscResult> AddScheduled(OfficialHolidayForItemsModel model)
        {
            var result = new KscResult();
            string scheduledName = "AutoLog";
            DateTime yesterday = System.DateTime.Now.AddDays(-1);
            string currentUserName = "AutoSystem";
            if (model.HolidayDate.HasValue)
            {
                yesterday = model.HolidayDate.Value;
                scheduledName = "ManualLog";
                currentUserName = model.CurrentUserName;
            }
            if (yesterday.Date == System.DateTime.Now.Date)
            {
                result.AddError("", "ثبت اطلاعات برای تاریخ جاری امکان پذیر نمی باشد");
                return result;
            }
            try
            {

                var LogData = _kscHrUnitOfWork.ScheduledLogRepository.Any(x => x.ScheduledDate == yesterday);

                if (LogData == true)
                    return result;//هیچ کاری نکند

                var scheduledLog = new ScheduledLog()
                {
                    ScheduledDate = yesterday,
                    InsertDate = DateTime.Now,
                    InsertUser = currentUserName,
                    ScheduledName = scheduledName

                };
                await _kscHrUnitOfWork.ScheduledLogRepository.AddAsync(scheduledLog);
                await _kscHrUnitOfWork.SaveAsync();

                return result;
            }
            catch (Exception ex)
            {

                KscResult kscResult = new KscResult();
                kscResult.AddError("خطا", ex.Message, 0);

                return result;

            }


        }
        public async Task<List<TimeSettingDataModel>> GetAllShiftStartEndTime(List<int> shiftConceptDetailId, List<int> workGroupId, List<int> workCityId, int workCalendarId)
        {
            List<TimeSettingDataModel> model = new List<TimeSettingDataModel>();
            var workCalendar = await _kscHrUnitOfWork.WorkCalendarRepository.GetByIdAsync(workCalendarId);
            var workGroups = await _kscHrUnitOfWork.WorkGroupRepository.GetWorkGroupsByRelations(workGroupId).ToListAsync().ConfigureAwait(false);
            var shiftConceptDetails = await _kscHrUnitOfWork.ShiftConceptDetailRepository.GetAllShiftConceptDetailWithIncluded(shiftConceptDetailId).ToListAsync().ConfigureAwait(false);
            var shiftConceptIds = shiftConceptDetails.SelectMany(a => a.ShiftConcept.WorkTimeShiftConcepts).Select(a => a.Id).ToList();
            var workCompanySettings = await _kscHrUnitOfWork.WorkCompanySettingRepository.GetAllWorkCompanySettingByWorkCityIdWorkTimeShiftConceptId(workCityId, shiftConceptIds).ToListAsync().ConfigureAwait(false);
            DateTime date = workCalendar.MiladiDateV1;
            try
            {
                foreach (var item in workGroups)
                {
                    var workTimeShiftConcept = shiftConceptDetails.SelectMany(a => a.ShiftConcept.WorkTimeShiftConcepts).First(x => x.WorkTimeId == item.WorkTimeId);
                    var workCompanySetting = workCompanySettings.First(a => a.Id == workTimeShiftConcept.Id);
                    var timeShiftSetting = workCompanySetting.TimeShiftSettings.FirstOrDefault(x => x.IsTemporaryTime == false && x.ValidityStartDate.Value.Date <= date.Date && x.ValidityEndDate.Value.Date >= date.Date);
                    if (timeShiftSetting == null)
                    {
                        throw new HRBusinessException(Validations.NotFoundId, String.Format(Validations.NotFound, "تنظیمات شیفت"));
                    }
                    var itemModel = new TimeSettingDataModel();
                    itemModel.ShiftStartTime = timeShiftSetting.ShiftStartTime;
                    itemModel.ShiftEndTime = timeShiftSetting.ShiftEndtTime;
                    itemModel.TotalWorkHourInDay = timeShiftSetting.TotalWorkHourInDay;
                    itemModel.WorkGroupId = item.Id;
                    itemModel.ShiftCondeptId = workTimeShiftConcept.ShiftConceptId;
                    itemModel.WorkCompanySettingId = workCompanySetting.Id;

                    model.Add(itemModel);
                }
            }
            catch
            {
                throw new HRBusinessException(Validations.NotFoundId, String.Format(Validations.NotFound, "تنظیمات شیفت"));

            }
            return model;
        }
        public async Task<TimeSettingDataModel> GetShiftStartEndTime(int shiftConceptDetailId, int workCityId, int workGroupId, int workCalendarId)
        {

            TimeSettingDataModel model = new TimeSettingDataModel();
            var workCalendar = await _kscHrUnitOfWork.WorkCalendarRepository.GetByIdAsync(workCalendarId);
            DateTime date = workCalendar.MiladiDateV1;
            try
            {
                var workGroup = await _kscHrUnitOfWork.WorkGroupRepository.GetByIdAsync(workGroupId);
                var workTime = await _kscHrUnitOfWork.WorkTimeRepository.GetByIdAsync(workGroup.WorkTimeId);
                var shiftConceptDetail = await _kscHrUnitOfWork.ShiftConceptDetailRepository.GetShiftConceptDetailWithIncluded(shiftConceptDetailId);
                var workTimeShiftConcept = shiftConceptDetail.ShiftConcept.WorkTimeShiftConcepts.First(x => x.WorkTimeId == workGroup.WorkTimeId);
                var workCompanySetting = await _kscHrUnitOfWork.WorkCompanySettingRepository.GetWorkCompanySettingByWorkCityIdWorkTimeShiftConceptId(workCityId, workTimeShiftConcept.Id);
                var timeShiftSetting = workCompanySetting.TimeShiftSettings.FirstOrDefault(x => x.IsTemporaryTime == false && x.ValidityStartDate.Value.Date <= date.Date && x.ValidityEndDate.Value.Date >= date.Date);
                if (timeShiftSetting == null)
                {
                    throw new HRBusinessException(Validations.NotFoundId, String.Format(Validations.NotFound, "تنظیمات شیفت"));
                }
                model.ShiftStartTime = timeShiftSetting.ShiftStartTime;
                model.ShiftEndTime = timeShiftSetting.ShiftEndtTime;

            }
            catch
            {

            }
            return model;
        }
        //public int GetRollCallIdRest(bool shiftSettingFromShiftboard, bool isHoliday, int dayNumber)
        //{
        //    int id = 0;
        //    if (shiftSettingFromShiftboard)
        //    {
        //        id = 65;
        //    }
        //    else
        //    {
        //        if (isHoliday)
        //            id = 67;
        //        if (dayNumber == (int)DayNumberType.Friday)
        //        {
        //            id = 65;
        //        }
        //        if (dayNumber == (int)DayNumberType.Thursday)
        //            id = 66;

        //    }
        //    return id;
        //}
        public List<TimeSettingDataModel> GetAllShiftSetting(DateTime date)
        {
            List<TimeSettingDataModel> resultmodel = new List<TimeSettingDataModel>();
            var timeShiftSetting = _kscHrUnitOfWork.TimeShiftSettingRepository.GetAllByIncludedAsNotracking(date);
            resultmodel = timeShiftSetting.Select(x => new TimeSettingDataModel()
            {
                ShiftStartTime = x.ShiftStartTime,
                ShiftEndTime = x.ShiftEndtTime,
                TotalWorkHourInDay = x.TotalWorkHourInDay,
                WorkTimeId = x.WorkCompanySetting.WorkTimeShiftConcept.WorkTimeId,
                ShiftCondeptId = x.WorkCompanySetting.WorkTimeShiftConcept.ShiftConceptId,
                WorkCompanySettingId = x.WorkCompanySettingId,
                WorkCityId = x.WorkCompanySetting.WorkCityId,
                TotalWorkHourInWeek = x.TotalWorkHourInWeek != null ? x.TotalWorkHourInWeek.ConvertDurationToMinute().Value : 0,
                ForcedOverTime = x.ForcedOverTime != null ? x.ForcedOverTime.ConvertDurationToMinute().Value : 0
            }).ToList();
            return resultmodel;
        }


        public async Task<FilterResult<EmployeeDontHaveAttendITemModel>> GetEmployeeDontHaveAttendITemModel(SearchEmployeeEntryExitModel Filter)
        {
            var result = new FilterResult<EmployeeDontHaveAttendITemModel>();
            try
            {

                DateTime? startDate = null;
                DateTime? endDate = null;
                if (Filter.YearMonth != null)
                {
                    var YearMonth = Utility.GetPersianMonth(Filter.YearMonth.ToEnglishNumbers());
                    endDate = YearMonth.EndDate;
                    int persianMonth = System.DateTime.Now.GetPersianMonth();
                    if (YearMonth.StartDate.GetPersianMonth() == persianMonth)
                        endDate = System.DateTime.Now.AddDays(-1).GetEndOfDay();
                    startDate = YearMonth.StartDate;
                }
                else if (Filter.StartDateString != null && Filter.EndDateString != null)
                {
                    startDate = Filter.StartDateString.ToGorgianDate();
                    endDate = Filter.EndDateString.ToGorgianDate();
                }
                string teamManager = Filter.CurrentUser;
                if (Filter.IsOfficialAttend)
                {
                    teamManager = null;
                }
                if (Filter.ToTeamWorkCode == null)
                    Filter.ToTeamWorkCode = Filter.TeamWorkCode;
                var prc = await _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository
                    .GetSpEmployeeTeamNotConfirmedReportAsync(startDate, endDate, teamManager, Filter.TeamWorkCode, Filter.ToTeamWorkCode, Filter.EmployeeId.ToString());
                var allPersonnelNotInAttendItemPrc = prc.Select(a => new EmployeeDontHaveAttendITemModel()
                {
                    EmployeeId = a.EmployeeId,
                    TeamName = a.TeamWorkTitle,
                    TeamWorkCode = a.TeamCode,
                    WorkCalendarDate = a.MiladiDateV1,
                    Date = a.ShamsiDateV1,
                    FullName = a.Name + " " + a.Family,
                    PersonalNumber = a.EmployeeNumber
                }).OrderBy(x => x.PersonalNumber).DistinctBy(x => x.Date + x.EmployeeId + x.ManagerTeam).AsQueryable();
                var finalList = _FilterHandler.GetFilterResult<EmployeeDontHaveAttendITemModel>(allPersonnelNotInAttendItemPrc, Filter, "PersonalNumber");

                result = new FilterResult<EmployeeDontHaveAttendITemModel>()
                {
                    Data = finalList.Data.ToList(),
                    Total = finalList.Total
                };

            }
            catch (Exception ex)
            {

            }

            return result;


        }

        public async Task<FilterResult<EmployeeDontHaveAttendITemModel>> GetEmployeeHaveAttendITemModelWithhRollCalls(SearchEmployeeEntryExitModel Filter)
        {
            var result = new FilterResult<EmployeeDontHaveAttendITemModel>();
            try
            {
                //var query = _kscHrUnitOfWork.ViewAttendItemReportRepository.GetAllQueryable().AsNoTracking();

                DateTime? startDate = null;
                DateTime? endDate = null;
                if (Filter.YearMonth != null)
                {
                    var YearMonth = Utility.GetPersianMonth(Filter.YearMonth.ToEnglishNumbers());
                    endDate = YearMonth.EndDate;
                    int persianMonth = System.DateTime.Now.GetPersianMonth();
                    if (YearMonth.StartDate.GetPersianMonth() == persianMonth)
                        endDate = System.DateTime.Now.AddDays(-1).GetEndOfDay();
                    startDate = YearMonth.StartDate;

                }
                else if (Filter.StartDateString != null && Filter.EndDateString != null)
                {
                    startDate = Filter.StartDateString.ToGorgianDate();
                    endDate = Filter.EndDateString.ToGorgianDate();
                }
                string teamManager = Filter.CurrentUser;
                if (Filter.IsOfficialAttend)
                {
                    teamManager = null;
                }
                if (Filter.ToTeamWorkCode == null)
                    Filter.ToTeamWorkCode = Filter.TeamWorkCode;
                var prc = await _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository
                    .GetSpEmployeeTeamNotConfirmedReportAsync(startDate, endDate, teamManager, Filter.TeamWorkCode, Filter.ToTeamWorkCode, Filter.EmployeeId.ToString());
                var allPersonnelNotInAttendItemPrc = prc.Select(a => new EmployeeDontHaveAttendITemModel()
                {
                    EmployeeId = a.EmployeeId,
                    TeamName = a.TeamWorkTitle,
                    TeamWorkCode = a.TeamCode,
                    WorkCalendarDate = a.MiladiDateV1,
                    Date = a.ShamsiDateV1,

                    FullName = a.Name + " " + a.Family,
                    PersonalNumber = a.EmployeeNumber
                }).OrderBy(x => x.PersonalNumber).DistinctBy(x => x.Date + x.EmployeeId + x.ManagerTeam).AsQueryable();
                var finalList = _FilterHandler.GetFilterResult<EmployeeDontHaveAttendITemModel>(allPersonnelNotInAttendItemPrc, Filter, "PersonalNumber");

                result = new FilterResult<EmployeeDontHaveAttendITemModel>()
                {
                    Data = finalList.Data.ToList(),
                    Total = finalList.Total
                };

            }
            catch (Exception ex)
            {

            }

            return result;


        }






        public async Task<FilterResult<EmployeeDontHaveAttendITemModel>> GetEmployeeDontHaveAttendItemModelReportByTeam1(SearchReportEmployeeEntryExitModel Filter)
        {
            var result = new FilterResult<EmployeeDontHaveAttendITemModel>();
            try
            {
                var YearMonth = Utility.GetPersianMonth(Filter.YearMonth.ToEnglishNumbers());
                var enddate = YearMonth.EndDate;
                int persianMonth = System.DateTime.Now.GetPersianMonth();
                if (YearMonth.StartDate.GetPersianMonth() == persianMonth)
                    enddate = System.DateTime.Now.AddDays(-1).GetEndOfDay();
                string teamManager = Filter.CurrentUserName;
                if (Filter.IsOfficialAttend)
                {
                    teamManager = null;
                }
                int yearMonth = int.Parse(Filter.YearMonth.ToEnglishNumbers());
                var prc = await _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository
                    .GetSpEmployeeTeamNotConfirmedReportAsync(YearMonth.StartDate, enddate, teamManager, Filter.StartTeamCode, Filter.EndTeamCode, Filter.PersonalNumber);
                if (Filter.NotShowEmployeeInMonthSheet) //افرادی که تایم-شیت ماهیانه دارند نمایش داده نشوند
                {
                    var EmployeeIdInMonth = _kscHrUnitOfWork.MonthTimeSheetRepository.GetMonthTimeSheetByYearMonthAsNoTracking(yearMonth).Select(x => x.EmployeeId).ToList();
                    prc = prc.Where(x => !EmployeeIdInMonth.Any(y => y == x.EmployeeId)).ToList();
                }
                var allPersonnelNotInAttendItemPrc = prc
                    .GroupBy(a => new
                    {
                        a.EmployeeId,
                        a.TeamWorkTitle,
                        a.TeamCode,
                        a.MiladiDateV1,
                        a.ShamsiDateV1,
                        a.Name,
                        a.Family,
                        a.EmployeeNumber,
                        a.WorkGroupCode
                    })
                    .Select(a => new EmployeeDontHaveAttendITemModel()
                    {
                        EmployeeId = a.Key.EmployeeId,
                        TeamName = a.Key.TeamWorkTitle,
                        TeamWorkCode = a.Key.TeamCode,
                        WorkCalendarDate = a.Key.MiladiDateV1,
                        Date = a.Key.ShamsiDateV1,
                        FullName = a.Key.Name + " " + a.Key.Family,
                        PersonalNumber = a.Key.EmployeeNumber,
                        ManagerTeam = string.Join(",", a.Select(x => x.ManagerTeam).Distinct()),
                        TeamCode = a.Key.TeamCode,
                        WorkGroupCode = a.Key.WorkGroupCode,
                    }).ToList();
                //var allPersonnelNotInAttendItemPrc = prc.Select(a => new EmployeeDontHaveAttendITemModel()
                //{
                //    EmployeeId = a.EmployeeId,
                //    TeamName = a.TeamWorkTitle,
                //    TeamWorkCode = a.TeamCode,
                //    WorkCalendarDate = a.MiladiDateV1,
                //    Date = a.ShamsiDateV1,
                //    FullName = a.Name + " " + a.Family,
                //    PersonalNumber = a.EmployeeNumber,
                //    ManagerTeam = a.ManagerTeam,
                //    TeamCode = a.TeamCode,
                //    WorkGroupCode = a.WorkGroupCode,
                //}).ToList();
                var finalList = _FilterHandler.GetFilterResult<EmployeeDontHaveAttendITemModel>(allPersonnelNotInAttendItemPrc, Filter, "PersonalNumber");

                result = new FilterResult<EmployeeDontHaveAttendITemModel>()
                {
                    Data = finalList.Data.ToList(),
                    Total = finalList.Total
                };

            }
            catch (Exception ex)
            {

            }

            return result;


        }


        //ماموریت با منطق حذف تایید کارکرد روز تعطیلی ها
        #region 
        //ثبت ماموریت ها از mis در جدول تایید کارکرد
        public async Task<KscResult> RemoveMissionItem(PAR_ASSPY models)
        {


            var result = new KscResult();
            var EmployeeAttendAbsenceItem = await _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository
                 .GetAllQuaryble(new List<long>()).Where(a => a.MissionId == models.ASSPY_ID).ToListAsync();
            foreach (var item in EmployeeAttendAbsenceItem)
            {

                var systemStatus = await _kscHrUnitOfWork.WorkCalendarRepository.GetDailyTimeSheetStatus(item.WorkCalendarId);
                if (systemStatus == EnumSystemSequenceStatusDailyTimeSheet.InActiveForAllUser.Id)
                {
                    throw new HRBusinessException(Validations.RepetitiveId,
                  "کارکرد بسته شده است");
                }

                _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.Delete(item);
            }
            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        public async Task<KscResult> AddMissionAttendAbcenceItem(PAR_ASSPY model)//, CancellationToken token)
        {

            var result = new KscResult();
            try
            {
                var employee = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeByPersonalNum(model.NUM_PRSN_EMPL);
                var employeeWorkGoups = _kscHrUnitOfWork.EmployeeWorkGroupRepository.GetEmployeeWorkGroupInclouded()
                     .Where(a => a.EmployeeId == employee.Id).ToList();

                var workGroupsIds = employeeWorkGoups.Select(a => a.WorkGroupId).ToList();
                var DAT_STR_ASSPYString = int.Parse(model.DAT_STR_ASSPY);
                var DAT_END_ASSPYString = int.Parse(model.DAT_END_ASSPY);
                var workCalendars = _kscHrUnitOfWork.WorkCalendarRepository.GetAllQuerable()
                .Where(a => a.DateKey >= DAT_STR_ASSPYString && a.DateKey <= DAT_END_ASSPYString).ToListAsync().GetAwaiter().GetResult().ToList();

                var workcalendarIds = workCalendars.Select(a => a.Id).ToList();
                //var shiftBoards = _kscHrUnitOfWork.ShiftBoardRepository.GetShiftBoardByworkGoupIdsAndWorkCalendarIds(workGroupsIds, workcalendarIds).ToList();


                var WorkGroupIds = employeeWorkGoups.Select(a => a.WorkGroupId).ToList();
                var ShiftConceptDetails = _shiftConceptDetailService.GetShiftConceptDetailWithWOrkGroupIdAndWokCalendarIds(WorkGroupIds, workcalendarIds);

                //var startDate = model.DAT_STR_ASSPY;//تاریخ شروع ماموریت
                //var workCity = _kscHrUnitOfWork.EmployeeRepository.GetByIdAsync(employee.EmployeeId).GetAwaiter().GetResult();
                var startTime = DateTime.Now;
                var endTime = DateTime.Now;
                var rollCallDefnition = _kscHrUnitOfWork.RollCallDefinitionRepository
                          .FirstOrDefault(a => a.Code == model.FK_ATABT);//کد حضور غیاب
                if (rollCallDefnition == null)
                {
                    result.AddError("", "کد حضور غیاب ندارد");
                    return result;
                }

                var items = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetItemsHolidayByworkcalendarIds(employee.Id, workcalendarIds);
                foreach (var item in items)
                {
                    _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.Delete(item);
                }

                ////بررسی روزهای تعطیل تایید کارکرد شده



                //List<int> rollCallHoliday = new List<int>() {
                //EnumRollCallDefinication.FridayOrShiftRest.Id
                //         ,EnumRollCallDefinication.Thursday.Id
                //          , EnumRollCallDefinication.Holiday.Id
                //};
                // var items = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetItemsHolidayByworkcalendarIds(employee.Id, workcalendarIds);
                //var normalDay = items.Where(x => rollCallHoliday.Any(r => r == x.RollCallDefinitionId) == false).Select(x => x.WorkCalendarId).ToList().Distinct();
                //if (normalDay.Any())
                //{
                //    var dates = workCalendars.Where(x => normalDay.Any(n => n == x.Id)).Select(x => x.ShamsiDateV1).ToList();
                //    var inValidDate = string.Join("-", dates);
                //    result.AddError("", string.Format(" کارکرد تایید شده برای تاریخ {0} وجود دارد", inValidDate));
                //    return result;
                //}
                //foreach (var item in items)
                //{

                //    if (item.RollCallDefinitionId == EnumRollCallDefinication.FridayOrShiftRest.Id
                //     || item.RollCallDefinitionId == EnumRollCallDefinication.Thursday.Id
                //      || item.RollCallDefinitionId == EnumRollCallDefinication.Holiday.Id)
                //    {

                //        _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.Delete(item);

                //    }
                //}


                foreach (var calendar in workCalendars)
                {
                    var calendarId = calendar.Id;
                    var miladiDate = calendar.MiladiDateV1;
                    var workGroup = GetEmployeeWorkGroupByEmployeeIdDate(miladiDate, employeeWorkGoups);


                    var systemStatus = _kscHrUnitOfWork.WorkCalendarRepository.GetDailyTimeSheetStatus(calendarId).GetAwaiter().GetResult();
                    if (systemStatus == EnumSystemSequenceStatusDailyTimeSheet.InActiveForAllUser.Id)
                    {
                        throw new HRBusinessException(Validations.RepetitiveId,
                      "کارکرد بسته شده است");
                    }
                    var workCalendar = workCalendars.First(a => a.Id == calendarId);



                    var shiftConceptDetailId = ShiftConceptDetails.FirstOrDefault(a => a.WorkCalendarId == workCalendar.Id && a.WorkGroupId == workGroup.WorkGroupId);

                    var getstartAndEndTimeShift = _kscHrUnitOfWork.EmployeeLongTermAbsenceRepository
                        .GetShiftStartEndTime(shiftConceptDetailId.Id, employee.WorkCityId.Value
                        , workGroup.WorkGroupId, calendarId).GetAwaiter().GetResult();
                    //مدت زمان شیف باید بدست بیاید
                    var addEmployeeAttendAbsenceItem = new EmployeeAttendAbsenceItem()
                    {
                        MissionId = model.ASSPY_ID,//کد شناسایی ماموریت
                        EmployeeId = employee.Id,
                        WorkTimeId = workGroup.WorkGroup.WorkTimeId,
                        WorkCalendarId = calendarId,
                        RollCallDefinitionId = rollCallDefnition.Id,
                        ShiftConceptDetailId = shiftConceptDetailId.Id,
                        IsManual = false,
                        StartTime = getstartAndEndTimeShift.Item1,
                        EndTime = getstartAndEndTimeShift.Item2,
                        TimeDuration = getstartAndEndTimeShift.Item3,
                        InsertDate = DateTime.Now,
                        InsertUser = model.COD_USR_ATABI,
                        ShiftConceptDetailIdInShiftBoard = shiftConceptDetailId.Id,
                        // TimeDurationInMinute = Utility.ConvertDurationToMinute(getstartAndEndTimeShift.Item3) ?? 00
                    };
                    //مدت زمان شیف باید بدست بیاید
                    _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.Add(addEmployeeAttendAbsenceItem);

                }

                await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new HRBusinessException(Validations.RepetitiveId, "عملیات نا موفق بود");
            }
            return result;


        }
        public EmployeeWorkGroup GetEmployeeWorkGroupByEmployeeIdDate(DateTime date, List<EmployeeWorkGroup> model)
        {
            return
                model.FirstOrDefault(x =>
                    x.StartDate <= date &&
                    (x.EndDate >= date || x.EndDate.HasValue == false));
        }

        public async Task<KscResult> AddMissionAttendAbcenceItemLoaddddd(PAR_ASSPY model)//, CancellationToken token)
        {

            var result = new KscResult();
            try
            {
                var employee = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeByPersonalNum(model.NUM_PRSN_EMPL);


                var wokgroup = _kscHrUnitOfWork.EmployeeWorkGroupRepository.GetEmployeeWorkGroupInclouded()
               .FirstAsync(a => a.Employee.EmployeeNumber == model.NUM_PRSN_EMPL
             && a.EndDate == null

               ).GetAwaiter().GetResult();


                var DAT_STR_ASSPYString = int.Parse(model.DAT_STR_ASSPY);
                var DAT_END_ASSPYString = int.Parse(model.DAT_END_ASSPY);
                var workCalendars = _kscHrUnitOfWork.WorkCalendarRepository.GetAllQuerable()
                .Where(a => a.DateKey >= DAT_STR_ASSPYString && a.DateKey <= DAT_END_ASSPYString).ToListAsync().GetAwaiter().GetResult();

                var workcalendarIds = workCalendars.Select(a => a.Id).ToList();
                var shiftConcepts = _kscHrUnitOfWork.ShiftConceptDetailRepository
                    .GetShiftConceptDetailWithWOrkGroupIdDates(wokgroup.WorkGroupId, workcalendarIds).GetAwaiter().GetResult();

                //var startDate = model.DAT_STR_ASSPY;//تاریخ شروع ماموریت
                var workCity = _kscHrUnitOfWork.EmployeeRepository.GetByIdAsync(wokgroup.EmployeeId).GetAwaiter().GetResult();
                var startTime = DateTime.Now;
                var endTime = DateTime.Now;
                var rollCallDefnition = _kscHrUnitOfWork.RollCallDefinitionRepository
                          .FirstOrDefault(a => a.Code == model.FK_ATABT);//کد حضور غیاب
                if (rollCallDefnition == null)
                {
                    result.AddError("", "کد حضور غیاب ندارد");
                    return result;
                }



                ////بررسی روزهای تعطیل تایید کارکرد شده


                List<int> rollCallHoliday = new List<int>() {
                EnumRollCallDefinication.FridayOrShiftRest.Id
                         ,EnumRollCallDefinication.Thursday.Id
                          , EnumRollCallDefinication.Holiday.Id
                };
                var items = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetItemsHolidayByworkcalendarIds(employee.Id, workcalendarIds);
                var normalDay = items.Where(x => rollCallHoliday.Any(r => r == x.RollCallDefinitionId) == false).Select(x => x.WorkCalendarId).ToList().Distinct();
                if (normalDay.Count() != 0)
                {
                    var dates = workCalendars.Where(x => normalDay.Any(n => n == x.Id)).Select(x => x.ShamsiDateV1).ToList();
                    var inValidDate = string.Join("-", dates);
                    result.AddError("", string.Format(" کارکرد تایید شده برای تاریخ {0} وجود دارد", inValidDate));
                    return result;
                }
                foreach (var item in items)
                {

                    if (item.RollCallDefinitionId == EnumRollCallDefinication.FridayOrShiftRest.Id
                     || item.RollCallDefinitionId == EnumRollCallDefinication.Thursday.Id
                      || item.RollCallDefinitionId == EnumRollCallDefinication.Holiday.Id)
                    {

                        _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.Delete(item);

                    }
                }



                //}//end if




                //........



                var shiftConceptDetailIds = shiftConcepts.Select(a => a.Id).ToList();
                var shiftboards = _kscHrUnitOfWork.ShiftBoardRepository
                    .WhereQueryable(x => shiftConceptDetailIds.Contains(x.ShiftConceptDetailId)
                        && workcalendarIds.Contains(x.WorkCalendarId)).ToList();
                foreach (var calendarId in workcalendarIds)
                {
                    var systemStatus = _kscHrUnitOfWork.WorkCalendarRepository.GetDailyTimeSheetStatus(calendarId).GetAwaiter().GetResult();
                    if (systemStatus == EnumSystemSequenceStatusDailyTimeSheet.InActiveForAllUser.Id)
                    {
                        throw new HRBusinessException(Validations.RepetitiveId,
                      "کارکرد بسته شده است");
                    }
                    var workCalendar = workCalendars.First(a => a.Id == calendarId);


                    var shiftConceptDetailId = 0;
                    if (wokgroup.WorkGroup.WorkTime.ShiftSettingFromShiftboard)
                    {
                        //shiftConceptDetailId = shiftConcepts.First(x => x.ShiftBoards.Any(a => a.WorkCalendarId == calendarId)).id;
                        shiftConceptDetailId = shiftboards.First(a => a.WorkCalendarId == calendarId
                        && wokgroup.WorkGroupId == a.WorkGroupId).ShiftConceptDetailId;
                    }
                    else
                    {
                        var workTimeShiftConcept = wokgroup.WorkGroup.WorkTime.WorkTimeShiftConcepts.FirstOrDefault(x => x.IsActive == true);
                        if (workTimeShiftConcept != null)
                            shiftConceptDetailId = workTimeShiftConcept.ShiftConcept.ShiftConceptDetails.First(x => x.IsActive == true).Id;
                    }
                    var getstartAndEndTimeShift = _kscHrUnitOfWork.EmployeeLongTermAbsenceRepository
                        .GetShiftStartEndTime(shiftConceptDetailId, workCity.WorkCityId.Value
                        , wokgroup.WorkGroupId, calendarId).GetAwaiter().GetResult();
                    //مدت زمان شیف باید بدست بیاید
                    var addEmployeeAttendAbsenceItem = new EmployeeAttendAbsenceItem()
                    {
                        MissionId = model.ASSPY_ID,//کد شناسایی ماموریت
                        EmployeeId = employee.Id,
                        WorkTimeId = wokgroup.WorkGroup.WorkTimeId,
                        WorkCalendarId = calendarId,
                        RollCallDefinitionId = rollCallDefnition.Id,
                        ShiftConceptDetailId = shiftConceptDetailId,
                        IsManual = false,
                        StartTime = getstartAndEndTimeShift.Item1,
                        EndTime = getstartAndEndTimeShift.Item2,
                        TimeDuration = getstartAndEndTimeShift.Item3,
                        InsertDate = DateTime.Now,
                        InsertUser = model.COD_USR_ATABI,
                        ShiftConceptDetailIdInShiftBoard = shiftConceptDetailId,
                        // TimeDurationInMinute = Utility.ConvertDurationToMinute(getstartAndEndTimeShift.Item3) ?? 00
                    };
                    //مدت زمان شیف باید بدست بیاید
                    _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.Add(addEmployeeAttendAbsenceItem);

                }

                await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new HRBusinessException(Validations.RepetitiveId, "عملیات نا موفق بود");
            }
            return result;


        }

        #endregion






        //ماموریت با منطق اول
        #region 
        //public async Task<KscResult> ExistEntryExit(MissionFromMisCommandViewModel models)
        //{


        //    var result = new KscResult();
        //    var miladiDate = workCalendars.Select(a => a.MiladiDateV1).ToList();
        //    var employeeEntryExist = _kscHrUnitOfWork.EmployeeEntryExitRepository.GetAllQueryable()
        //        .Where(a => miladiDate.Any(c => c == a.EntryExitDate)
        //        && a.PersonalNumber == employee.EmployeeNumber).ToList();
        //    if (employeeEntryExist.Any())
        //    {
        //        result.AddError("", "این شماره پرسنلی در این تاریخ حضور و غیاب دارد");
        //        return result;
        //    }

        //    await _kscHrUnitOfWork.SaveAsync();
        //    return result;
        //}

        //ثبت ماموریت ها از mis در جدول تایید کارکرد
        public async Task<KscResult> RemoveMissionItem11(PAR_ASSPY models)
        {


            var result = new KscResult();
            var EmployeeAttendAbsenceItem = await _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository
                 .GetAllQuaryble(new List<long>()).Where(a => a.MissionId == models.ASSPY_ID).ToListAsync();
            foreach (var item in EmployeeAttendAbsenceItem)
            {

                var systemStatus = await _kscHrUnitOfWork.WorkCalendarRepository.GetDailyTimeSheetStatus(item.WorkCalendarId);
                if (systemStatus == EnumSystemSequenceStatusDailyTimeSheet.InActiveForAllUser.Id)
                {
                    throw new HRBusinessException(Validations.RepetitiveId,
                  "کارکرد بسته شده است");
                }

                _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.Delete(item);
            }
            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        public async Task<KscResult> AddMissionAttendAbcenceItem11(PAR_ASSPY model)//, CancellationToken token)
        {

            var result = new KscResult();
            try
            {
                var employee = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeByPersonalNum(model.NUM_PRSN_EMPL);


                var wokgroup = _kscHrUnitOfWork.EmployeeWorkGroupRepository.GetEmployeeWorkGroupInclouded()
                    .FirstAsync(a => a.Employee.EmployeeNumber == model.NUM_PRSN_EMPL).GetAwaiter().GetResult();

                var DAT_STR_ASSPYString = int.Parse(model.DAT_STR_ASSPY);
                var DAT_END_ASSPYString = int.Parse(model.DAT_END_ASSPY);
                var workCalendars = _kscHrUnitOfWork.WorkCalendarRepository.GetAllQuerable()
                .Where(a => a.DateKey >= DAT_STR_ASSPYString && a.DateKey <= DAT_END_ASSPYString).ToListAsync().GetAwaiter().GetResult();

                var workcalendarIds = workCalendars.Select(a => a.Id).ToList();
                var shiftConcepts = _kscHrUnitOfWork.ShiftConceptDetailRepository
                    .GetShiftConceptDetailWithWOrkGroupIdDates(wokgroup.WorkGroupId, workcalendarIds).GetAwaiter().GetResult();

                //var startDate = model.DAT_STR_ASSPY;//تاریخ شروع ماموریت
                var workCity = _kscHrUnitOfWork.EmployeeRepository.GetByIdAsync(wokgroup.EmployeeId).GetAwaiter().GetResult();
                var startTime = DateTime.Now;
                var endTime = DateTime.Now;
                var rollCallDefnition = _kscHrUnitOfWork.RollCallDefinitionRepository
                          .FirstOrDefault(a => a.Code == model.FK_ATABT);//کد حضور غیاب
                if (rollCallDefnition == null)
                {
                    result.AddError("", "کد حضور غیاب ندارد");
                    return result;
                }

                //var miladiDate = workCalendars.Select(a => a.MiladiDateV1).ToList();
                //var employeeEntryExist = _kscHrUnitOfWork.EmployeeEntryExitRepository
                //    .Any(a => miladiDate.Any(c => c == a.EntryExitDate)
                //    && a.PersonalNumber == employee.EmployeeNumber);
                //if (employeeEntryExist)
                //{
                //    //result.Id = "R";
                //    result.AddError("", "این شماره پرسنلی در این بازه دارای  ورود و خروج می باشد");
                //    return result;
                //}

                var isFind = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository
                    .Any(a =>
                    a.EmployeeId == employee.Id &&
                    workcalendarIds.Contains(a.WorkCalendarId));
                if (isFind)
                {
                    result.AddError("", " کارکرد تایید شده  وجود دارد");
                    return result;
                }
                var shiftConceptDetailIds = shiftConcepts.Select(a => a.Id).ToList();
                var shiftboards = _kscHrUnitOfWork.ShiftBoardRepository
                    .WhereQueryable(x => shiftConceptDetailIds.Contains(x.ShiftConceptDetailId)
                        && workcalendarIds.Contains(x.WorkCalendarId)).ToList();
                foreach (var calendarId in workcalendarIds)
                {
                    var systemStatus = _kscHrUnitOfWork.WorkCalendarRepository.GetDailyTimeSheetStatus(calendarId).GetAwaiter().GetResult();
                    if (systemStatus == EnumSystemSequenceStatusDailyTimeSheet.InActiveForAllUser.Id)
                    {
                        throw new HRBusinessException(Validations.RepetitiveId,
                      "کارکرد بسته شده است");
                    }
                    var workCalendar = workCalendars.First(a => a.Id == calendarId);


                    var shiftConceptDetailId = 0;
                    if (wokgroup.WorkGroup.WorkTime.ShiftSettingFromShiftboard)
                    {
                        //shiftConceptDetailId = shiftConcepts.First(x => x.ShiftBoards.Any(a => a.WorkCalendarId == calendarId)).id;
                        shiftConceptDetailId = shiftboards.First(a => a.WorkCalendarId == calendarId && wokgroup.WorkGroupId == a.WorkGroupId).ShiftConceptDetailId;
                    }
                    else
                    {
                        var workTimeShiftConcept = wokgroup.WorkGroup.WorkTime.WorkTimeShiftConcepts.FirstOrDefault(x => x.IsActive == true);
                        if (workTimeShiftConcept != null)
                            shiftConceptDetailId = workTimeShiftConcept.ShiftConcept.ShiftConceptDetails.First(x => x.IsActive == true).Id;
                    }
                    var getstartAndEndTimeShift = _kscHrUnitOfWork.EmployeeLongTermAbsenceRepository
                        .GetShiftStartEndTime(shiftConceptDetailId, workCity.WorkCityId.Value, wokgroup.WorkGroupId, calendarId).GetAwaiter().GetResult();
                    //مدت زمان شیف باید بدست بیاید
                    var addEmployeeAttendAbsenceItem = new EmployeeAttendAbsenceItem()
                    {
                        MissionId = model.ASSPY_ID,//کد شناسایی ماموریت
                        EmployeeId = employee.Id,
                        WorkTimeId = wokgroup.WorkGroup.WorkTimeId,
                        WorkCalendarId = calendarId,
                        RollCallDefinitionId = rollCallDefnition.Id,
                        ShiftConceptDetailId = shiftConceptDetailId,
                        IsManual = false,
                        StartTime = getstartAndEndTimeShift.Item1,
                        EndTime = getstartAndEndTimeShift.Item2,
                        TimeDuration = getstartAndEndTimeShift.Item3,
                        InsertDate = DateTime.Now,
                        InsertUser = model.COD_USR_ATABI,
                        ShiftConceptDetailIdInShiftBoard = shiftConceptDetailId,
                        // TimeDurationInMinute = Utility.ConvertDurationToMinute(getstartAndEndTimeShift.Item3) ?? 00
                    };
                    //مدت زمان شیف باید بدست بیاید
                    _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.Add(addEmployeeAttendAbsenceItem);

                }

                await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new HRBusinessException(Validations.RepetitiveId, "عملیات نا موفق بود");
            }
            return result;


        }

        #endregion



        //حالتیکه برای ماموریت تمام رکوردهای قبلی حذف می شود

        #region 

        public async Task<KscResult> RemoveMissionItem1(PAR_ASSPY models)
        {


            var result = new KscResult();
            var EmployeeAttendAbsenceItem = await _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository
                 .GetAllQuaryble(new List<long>()).Where(a => a.MissionId == models.ASSPY_ID).ToListAsync();
            foreach (var item in EmployeeAttendAbsenceItem)
            {

                var systemStatus = await _kscHrUnitOfWork.WorkCalendarRepository.GetDailyTimeSheetStatus(item.WorkCalendarId);
                if (systemStatus == EnumSystemSequenceStatusDailyTimeSheet.InActiveForAllUser.Id)
                {
                    throw new HRBusinessException(Validations.RepetitiveId,
                  "کارکرد بسته شده است");
                }

                _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.Delete(item);
            }
            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }
        //حالتیکه برای ماموریت تمام رکوردهای قبلی حذف می شود
        public async Task<KscResult> AddMissionAttendAbcenceItem1(PAR_ASSPY model)//, CancellationToken token)
        {

            var result = new KscResult();
            try
            {
                var employee = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeByPersonalNum(model.NUM_PRSN_EMPL);


                var wokgroup = _kscHrUnitOfWork.EmployeeWorkGroupRepository.GetEmployeeWorkGroupInclouded()
                    .FirstAsync(a => a.Employee.EmployeeNumber == model.NUM_PRSN_EMPL).GetAwaiter().GetResult();

                var DAT_STR_ASSPYString = int.Parse(model.DAT_STR_ASSPY);
                var DAT_END_ASSPYString = int.Parse(model.DAT_END_ASSPY);
                var workCalendars = _kscHrUnitOfWork.WorkCalendarRepository.GetAllQueryable()
                .Where(a => a.DateKey >= DAT_STR_ASSPYString && a.DateKey <= DAT_END_ASSPYString).ToListAsync().GetAwaiter().GetResult();

                var workcalendarIds = workCalendars.Select(a => a.Id).ToList();
                var shiftConcepts = _kscHrUnitOfWork.ShiftConceptDetailRepository
                    .GetShiftConceptDetailWithWOrkGroupIdDates(wokgroup.WorkGroupId, workcalendarIds).GetAwaiter().GetResult();

                //var startDate = model.DAT_STR_ASSPY;//تاریخ شروع ماموریت
                var workCity = _kscHrUnitOfWork.EmployeeRepository.GetByIdAsync(wokgroup.EmployeeId).GetAwaiter().GetResult();
                var startTime = DateTime.Now;
                var endTime = DateTime.Now;
                var rollCallDefnition = _kscHrUnitOfWork.RollCallDefinitionRepository
                          .FirstOrDefault(a => a.Code == model.FK_ATABT);//کد حضور غیاب
                if (rollCallDefnition == null)
                {
                    result.AddError("", "کد حضور غیاب ندارد");
                    return result;
                }

                //var miladiDate = workCalendars.Select(a => a.MiladiDateV1).ToList();
                //var employeeEntryExist = _kscHrUnitOfWork.EmployeeEntryExitRepository
                //    .Any(a => miladiDate.Any(c => c == a.EntryExitDate)
                //    && a.PersonalNumber == employee.EmployeeNumber);
                //if (employeeEntryExist)
                //{
                //    //result.Id = "R";
                //    result.AddError("", "این شماره پرسنلی در این بازه دارای  ورود و خروج می باشد");
                //    return result;
                //}


                ////بررسی روزهای تایید کارکرد شده
                var employeeAttendAbsenceItem = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository
                    .Where(a =>
                    a.EmployeeId == employee.Id &&
                    workcalendarIds.Contains(a.WorkCalendarId)).ToList();


                var isFind = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository
                    .Any(a =>
                    a.EmployeeId == employee.Id &&
                    workcalendarIds.Contains(a.WorkCalendarId));
                //if (isFind)
                //{
                //    result.AddError("", " کارکرد تایید شده  وجود دارد");
                //    return result;
                //}



                ////بررسی روزهای تعطیل تایید کارکرد شده

                //var isFind = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository
                //    .Any(a =>
                //    a.EmployeeId == employee.Id &&
                //    workcalendarIds.Contains(a.WorkCalendarId));
                //if (isFind) //اگر تایید کارکرد داشت
                //{

                //    var isholiday = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository
                //   .FirstOrDefault(a =>  a.EmployeeId == employee.Id && workcalendarIds.Contains(a.WorkCalendarId) &&
                //     ( a.RollCallDefinitionId == EnumRollCallDefinication.FridayOrShiftRest.Id
                //     || a.RollCallDefinitionId == EnumRollCallDefinication.Thursday.Id
                //      || a.RollCallDefinitionId == EnumRollCallDefinication.Holiday.Id)

                //      );

                //    if (isholiday!=null)//اگرتعطیل بود
                //    {
                //        _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.Delete(isholiday);

                //    }
                //    else
                //    {
                //        result.AddError("", " کارکرد تایید شده  وجود دارد");
                //         return result;
                //    }

                //}//end if


                //........
                ////بررسی ماه جاری




                //   var isFind = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository
                //       .Any(a =>
                //       a.EmployeeId == employee.Id &&
                //       workcalendarIds.Contains(a.WorkCalendarId));
                //   if (isFind) //اگر تایید کارکرد داشت
                //   {

                //       // if (yesterday.Date == System.DateTime.Now.Date)

                //       var toDay = System.DateTime.Now.Date;
                //       var toDayYYYYMM = int.Parse(toDay.GetPersianYear().ToString("0000") + toDay.GetPersianMonth().ToString("00"));


                //       var isItems = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository
                //.FirstOrDefault(a => a.EmployeeId == employee.Id && workcalendarIds.Contains(a.WorkCalendarId));


                //       if ()//اگر ماه جاری بود
                //       {
                //           _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.Delete();

                //       }
                //       else
                //       {
                //           //result.AddError("", " کارکرد تایید شده  وجود دارد");
                //           //return result;
                //       }

                //   }//end if
                //   //.......


                var shiftConceptDetailIds = shiftConcepts.Select(a => a.Id).ToList();
                var shiftboards = _kscHrUnitOfWork.ShiftBoardRepository
                    .WhereQueryable(x => shiftConceptDetailIds.Contains(x.ShiftConceptDetailId)
                        && workcalendarIds.Contains(x.WorkCalendarId)).ToList();

                foreach (var calendarId in workcalendarIds)
                {
                    var systemStatus = _kscHrUnitOfWork.WorkCalendarRepository.IsValidStatusForMission(calendarId).GetAwaiter().GetResult();
                    if (systemStatus == false)
                    {
                        throw new HRBusinessException(Validations.RepetitiveId,
                      "تاریخ نامعتبر است");
                    }


                    //
                    var employeeAttendAbsenceItemByCalendar = employeeAttendAbsenceItem.Where(x => x.WorkCalendarId == calendarId);


                    if (employeeAttendAbsenceItemByCalendar.Any() == true) //تاریخ های مجاز برای ثبت ماموریت
                    {
                        foreach (var item in employeeAttendAbsenceItemByCalendar)
                        {

                            var iitem = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetById(item.Id);
                            if (iitem != null)
                            {

                                _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.Delete(iitem);
                            }
                        }

                    }
                    //
                    var workCalendar = workCalendars.First(a => a.Id == calendarId);


                    var shiftConceptDetailId = 0;
                    if (wokgroup.WorkGroup.WorkTime.ShiftSettingFromShiftboard)
                    {
                        //shiftConceptDetailId = shiftConcepts.First(x => x.ShiftBoards.Any(a => a.WorkCalendarId == calendarId)).id;
                        shiftConceptDetailId = shiftboards.First(a => a.WorkCalendarId == calendarId && wokgroup.WorkGroupId == a.WorkGroupId).ShiftConceptDetailId;
                    }
                    else
                    {
                        var workTimeShiftConcept = wokgroup.WorkGroup.WorkTime.WorkTimeShiftConcepts.FirstOrDefault(x => x.IsActive == true);
                        if (workTimeShiftConcept != null)
                            shiftConceptDetailId = workTimeShiftConcept.ShiftConcept.ShiftConceptDetails.First(x => x.IsActive == true).Id;
                    }
                    var getstartAndEndTimeShift = _kscHrUnitOfWork.EmployeeLongTermAbsenceRepository
                        .GetShiftStartEndTime(shiftConceptDetailId, workCity.WorkCityId.Value, wokgroup.WorkGroupId, calendarId).GetAwaiter().GetResult();
                    //مدت زمان شیف باید بدست بیاید
                    var addEmployeeAttendAbsenceItem = new EmployeeAttendAbsenceItem()
                    {
                        MissionId = model.ASSPY_ID,//کد شناسایی ماموریت
                        EmployeeId = employee.Id,
                        WorkTimeId = wokgroup.WorkGroup.WorkTimeId,
                        WorkCalendarId = calendarId,
                        RollCallDefinitionId = rollCallDefnition.Id,
                        ShiftConceptDetailId = shiftConceptDetailId,
                        IsManual = false,
                        StartTime = getstartAndEndTimeShift.Item1,
                        EndTime = getstartAndEndTimeShift.Item2,
                        TimeDuration = getstartAndEndTimeShift.Item3,
                        InsertDate = DateTime.Now,
                        InsertUser = model.COD_USR_ATABI,
                        ShiftConceptDetailIdInShiftBoard = shiftConceptDetailId,
                        // TimeDurationInMinute = Utility.ConvertDurationToMinute(getstartAndEndTimeShift.Item3) ?? 00
                    };
                    //مدت زمان شیف باید بدست بیاید
                    _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.Add(addEmployeeAttendAbsenceItem);

                }

                await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new HRBusinessException(Validations.RepetitiveId, "عملیات نا موفق بود");
            }
            return result;


        }

        #endregion

        #region //تایید کارکرد ارزیابی کارکنان

        public async Task<ReturnData<RPC005Evaluation>> SaveEvaluationDevelopment(RPC005Evaluation model)
        {
            var today = DateTime.Now;
            var result = new ReturnData<RPC005Evaluation>();
            result.Data = model;
            result.IsSuccess = true;

            if (model.ARR.Any() == false)
            {
                result.AddError("لیست نبایستی خالی باشد");
                return result;
            }
            // ولیدشن 1 و 2
            var Date_int = int.Parse(model.DATE);
            var workCalendar = _kscHrUnitOfWork.WorkCalendarRepository.Where(x => x.DateKey == Date_int).AsQueryable().Include(x => x.WorkDayType).FirstOrDefault();
            var systemStatus = _kscHrUnitOfWork.WorkCalendarRepository.GetDailyTimeSheetStatus(workCalendar.Id).GetAwaiter().GetResult();
            var employee = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeByRelatedMonthTimeSheet();
            List<EmployeeAttendAbsenceItem> employeeAttendAbsenceItemList = new List<EmployeeAttendAbsenceItem>();
            if (systemStatus == EnumSystemSequenceStatusDailyTimeSheet.InActiveForAllUser.Id)
            {
                result.AddError("کارکرد بسته شده است");
                return result;
            }

            var arrAsQueryable = model.ARR;



            //یافتن ایدی های مورد نیاز در جدول پرسنل

            var evaluationDevelopmentModel =
                                 (from t in arrAsQueryable
                                  join e in employee.AsNoTracking()//.GetEmployee()

                                   on t.NUMP equals e.EmployeeNumber
                                  select new AddEmployeeAttendAbsenceItemModel()
                                  {
                                      EvaluationDevelopmentTypeId = int.Parse(t.COD_TYP),
                                      evaluationDevelopmentId = t.FlgEvaluation,//کد شناسایی 
                                      EmployeeNumber = e.EmployeeNumber,
                                      EmployeeId = e.Id,
                                      WorkTimeId = e.WorkGroup.WorkTimeId,
                                      WorkCalendarId = workCalendar.Id,
                                      StartTime = t.STIME.Substring(0, 2) + ":" + t.STIME.Substring(2, 2),
                                      EndTime = t.ETIME.Substring(0, 2) + ":" + t.ETIME.Substring(2, 2),//t.ETIME,
                                      IsManual = false,
                                      WorkGroupId = e.WorkGroupId.Value,
                                      WorkCityId = e.WorkCityId,
                                      DismissalStatusId = e.DismissalStatusId,
                                      PaymentStatusId = e.PaymentStatusId,
                                      InsertDate = DateTime.Now,
                                      COD_TYP = t.COD_TYP,
                                      FLG_MIS = t.FLG_MIS,
                                      //ClassDate = ClassDate_Miladi.MiladiDateV1,
                                      //StartTime = getstartAndEndTimeShift.Item1,
                                      //EndTime = getstartAndEndTimeShift.Item2,
                                      //TimeDuration = getstartAndEndTimeShift.Item3,

                                  }).ToList();



            //
            var ShiftDetailModelShift = _kscHrUnitOfWork.ShiftBoardRepository.GetShiftBoardByWorkCalendarAsNoTracking(workCalendar.Id)
                .Select(x => new ShiftDetailModel() { ShiftConceptId = x.ShiftConceptDetail.ShiftConceptId, ShiftConceptDetailId = x.ShiftConceptDetailId, WorkGroupId = x.WorkGroupId, WorkTimeId = x.WorkGroup.WorkTimeId }).ToList();

            var ShiftDetailModelFix = _kscHrUnitOfWork.WorkTimeRepository.GetWorkTimesAsNoTracking()
                .Where(x => x.ShiftSettingFromShiftboard == false && x.IsActive == true)
                .Select(x => new ShiftDetailModel()
                {
                    ShiftConceptId = x.WorkTimeShiftConcepts.First().ShiftConceptId
                  ,
                    WorkGroupId = x.WorkGroups.First().Id
                  ,
                    ShiftConceptDetailId = x.WorkTimeShiftConcepts.First().ShiftConcept.ShiftConceptDetails.First().Id
                    ,
                    WorkTimeId = x.Id
                })
                .ToList();

            var shiftDetailModel = ShiftDetailModelShift.Union(ShiftDetailModelFix).ToList();
            var timeShiftSettingList = _kscHrUnitOfWork.TimeShiftSettingRepository.GetTimeShiftSettingAsNoTracking().ToList();
            //// بررسی تک به تک افراد
            foreach (var item in evaluationDevelopmentModel)
            {
                var itemInModel = result.Data.ARR.FirstOrDefault(x => x.NUMP == item.EmployeeNumber.ToString());
                var shiftConceptData = shiftDetailModel.First(x => x.WorkGroupId == item.WorkGroupId);
                var shiftConceptDetailId = shiftConceptData.ShiftConceptDetailId;
                var workTimeId = shiftConceptData.WorkTimeId;
                int? ShiftConceptDetailIdInShiftBoard = shiftConceptData.ShiftConceptDetailId;
                var timeSettingDataModel = timeShiftSettingList.FirstOrDefault(x =>
                     x.WorkCityId == item.WorkCityId &&
                     x.WorktimeId == workTimeId && x.ShiftConceptId == shiftConceptData.ShiftConceptId
                    && x.IsTemporaryTime == false && x.ValidityStartDate.Value.Date <= workCalendar.MiladiDateV1 && x.ValidityEndDate.Value.Date >= workCalendar.MiladiDateV1);
                var timeSettingDataModelTemporary = timeShiftSettingList.FirstOrDefault(x =>
                     x.WorkCityId == item.WorkCityId &&
                     x.WorktimeId == workTimeId && x.ShiftConceptId == shiftConceptData.ShiftConceptId
                    && x.IsTemporaryTime && x.ValidityStartDate.Value.Date <= workCalendar.MiladiDateV1 && x.ValidityEndDate.Value.Date >= workCalendar.MiladiDateV1);
                //
                timeSettingDataModel.ShiftConceptIsRest = await _kscHrUnitOfWork.TimeShiftSettingRepository.IsRestShiftAsync(timeSettingDataModel.ShiftSettingFromShiftboard, timeSettingDataModel.OfficialUnOfficialHolidayFromWorkCalendar
                    , timeSettingDataModel.ShiftConceptIsRest, workCalendar.WorkDayType.IsHoliday, timeSettingDataModel.WorkCompanySettingId, workCalendar.DayOfWeek);
                //
                if (item.PaymentStatusId != 3 && item.DismissalStatusId == null) // اگر کارمند جاری نباشد
                {
                    itemInModel.COD_ERR = "";
                    itemInModel.DES_ERR = $"کارمند جاری نمیباشد";
                    itemInModel.FLG_ERR = "1";
                    result.AddError(itemInModel.DES_ERR);
                    continue;
                }



                if (item.ShiftSettingFromShiftboard == true) // اگر شیفتی باشد
                {
                    itemInModel.COD_ERR = "";
                    itemInModel.DES_ERR = $"نوبت کاري نفر بايداز نوع روزکاري باشد ";
                    itemInModel.FLG_ERR = "1";
                    result.AddError(itemInModel.DES_ERR);
                    continue;
                }
                if (workCalendar.MiladiDateV1.Date >= DateTime.Now.Date) // اگر حضورغیاب بزرگتر از امروز  باشد
                {
                    itemInModel.COD_ERR = "";
                    itemInModel.DES_ERR = $"تاریخ حضور و غیاب از تاریخ جاری بزرگتر می باشد ";
                    itemInModel.FLG_ERR = "1";
                    result.AddError(itemInModel.DES_ERR);
                    continue;
                }

                if (itemInModel.FLG_MIS == "1")
                {// Delete
                    var itemForEmpInDay = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository
                        .GetEmployeeAttendAbsenceItemByRelated()
                           .Where(x => x.InvalidRecord == false
                                    && x.WorkCalendarId == item.WorkCalendarId
                                    && x.EmployeeId == item.EmployeeId
                                    // && x.EvaluationDevelopmentId == models.ASSPY_ID
                                    );
                    if (itemForEmpInDay.Any()) //اگر تایید کارکرد داشت
                    {
                        //برای حذف ::==>> تایید کارکرد ارزیابی توسعه و اضافه کار ارزیابی هر دو را حذف می کند

                        var employeeAttendAbsenceItem = itemForEmpInDay.Where(x => x.EvaluationDevelopmentId != null).ToList();
                        //.Where(x => x.RollCallDefinitionId == EnumRollCallDefinication.EvaluationDevelopmentExtraWork.Id
                        //|| x.RollCallDefinitionId == EnumRollCallDefinication.EvaluationDevelopmentKarkard.Id).ToList();//83,84

                        if (employeeAttendAbsenceItem != null && employeeAttendAbsenceItem.Any())//شاید این داده قبلا توسط سرپرست حذف شده
                        {

                            foreach (var items in employeeAttendAbsenceItem)
                            {
                                _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.Delete(items);

                            }
                        }


                    }

                    else //اگر تایید کارکرد نداشت
                    {

                        ////در جدول کارکرد داده ای ثبت شده و از نوع ارزيابي و توسعه نیست  امکان حذف وجود ندارد
                        // یافتن دیتا و پر کردن  خطا برای مدل خروجی
                        itemInModel.COD_ERR = "";
                        itemInModel.DES_ERR = $"کارکرد ارزيابي و توسعه جهت حذف يافت نشد ";
                        itemInModel.FLG_ERR = "1";
                        result.AddError(itemInModel.DES_ERR);
                        continue;
                    }


                } //delete


                else if (itemInModel.FLG_MIS == "2")//add
                {
                    var tempData = _mapper.Map<EmployeeAttendAbsenceItem>(item);

                    var addTempData = false;
                    var itemForEmpInDay = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository
                       .GetEmployeeAttendAbsenceItemByRelated()
                          .Where(x => x.InvalidRecord == false
                                   && x.WorkCalendarId == item.WorkCalendarId
                                   && x.EmployeeId == item.EmployeeId
                                   // && x.EvaluationDevelopmentId == item.evaluationDevelopmentId
                                   ).ToList();
                    if (itemForEmpInDay.Any(x => x.EvaluationDevelopmentId != null) == true)
                    {
                        itemInModel.COD_ERR = "";
                        itemInModel.DES_ERR = $"یک کارکرد ارزیابی برای این پرسنل وجود دارد تکراری ";
                        itemInModel.FLG_ERR = "1";
                        result.AddError(itemInModel.DES_ERR);
                        continue;
                    }

                    if (itemForEmpInDay.Count() != 0 && timeSettingDataModel.ShiftConceptIsRest != true)//true
                    {//تایید کارکرد شده باشد  و در روز عادی باشد 
                        itemInModel.COD_ERR = "";
                        itemInModel.DES_ERR = $"به دليل اينکه فرد در اين محدوده تاريخ کارکرد دارد ،اجازه انجام عمليات را نداريد  ";
                        itemInModel.FLG_ERR = "1";
                        result.AddError(itemInModel.DES_ERR);
                        continue;
                    }

                    if (timeSettingDataModel.ShiftConceptIsRest == true)//تعطیلی
                    {//85,86,87//ارزیابی کارکنان روز تعطیل 
                     // تمام ساعت اموزش برایش اضافه کار ارزیابی ثبت می شود و چون از قبل رکورد تایید کارکرد داشته بقیه اطلاعات از همان رکورد پر میشود و نیاز به محاسبه نیست
                        var rollCallDefinitionId = Utility.GetRollCallIdRest(timeSettingDataModel.ShiftSettingFromShiftboard,
                          workCalendar.WorkDayType.IsHoliday, workCalendar.DayOfWeek
                          , timeSettingDataModel.OfficialUnOfficialHolidayFromWorkCalendar, timeSettingDataModel.ShiftConceptIsRest);
                        if (itemForEmpInDay.Any() == false) // ردیف روز کارکرد تعطیلی نداشته باشد
                        {
                            // افزودن ردیف روز کارکرد تعطیلی
                            EmployeeAttendAbsenceItem newItem = new EmployeeAttendAbsenceItem()
                            {

                                WorkCalendarId = item.WorkCalendarId,
                                ShiftConceptDetailId = shiftConceptDetailId,
                                ShiftConceptDetailIdInShiftBoard = shiftConceptDetailId,
                                EmployeeId = tempData.EmployeeId,
                                StartTime = timeSettingDataModel.ShiftStartTime,
                                EndTime = timeSettingDataModel.ShiftEndtTime,//t.ETIME,
                                EvaluationDevelopmentId = item.evaluationDevelopmentId,

                                TimeDuration = Utility.GetDurationStartTimeToEndTime(timeSettingDataModel.ShiftStartTime, timeSettingDataModel.ShiftEndtTime),
                                RollCallDefinitionId = rollCallDefinitionId,
                                WorkTimeId = workTimeId,
                                InvalidRecord = false,
                                InvalidRecordReason = "",
                                IsFloat = false,
                                IsManual = false,
                                InsertDate = DateTime.Now,
                                InsertUser = model.WinUser

                            };
                            employeeAttendAbsenceItemList.Add(newItem);
                        }

                        if (itemForEmpInDay.Any()) //اگر تایید کارکرد روز تعطیل داشت ،اضافه کار توسعه میگیرد
                        {

                            var employeeItem = itemForEmpInDay.First();
                            shiftConceptDetailId = employeeItem.ShiftConceptDetailId;
                            workTimeId = employeeItem.WorkTimeId;
                            ShiftConceptDetailIdInShiftBoard = employeeItem.ShiftConceptDetailIdInShiftBoard;
                        }

                        EmployeeAttendAbsenceItem newEmployeeAttendAbsenceItem = new EmployeeAttendAbsenceItem()
                        {

                            WorkCalendarId = workCalendar.Id,
                            ShiftConceptDetailId = shiftConceptDetailId,
                            ShiftConceptDetailIdInShiftBoard = ShiftConceptDetailIdInShiftBoard,
                            EmployeeId = tempData.EmployeeId,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            EvaluationDevelopmentId = item.evaluationDevelopmentId,

                            TimeDuration = Utility.GetDurationStartTimeToEndTime(item.StartTime, item.EndTime),
                            RollCallDefinitionId = EnumRollCallDefinication.EvaluationDevelopmentExtraWork.Id,
                            WorkTimeId = workTimeId,
                            InvalidRecord = false,
                            InvalidRecordReason = "",
                            IsFloat = false,
                            IsManual = false,
                            InsertDate = DateTime.Now,
                            InsertUser = model.WinUser

                        };
                        employeeAttendAbsenceItemList.Add(newEmployeeAttendAbsenceItem);

                    }
                    else
                    { //روز عادی
                        if (item.StartTime.ConvertStringToTimeSpan() != timeSettingDataModel.ShiftStartTime.ConvertStringToTimeSpan())
                        {
                            itemInModel.COD_ERR = "";
                            itemInModel.DES_ERR = $"ساعت شروع کلاس منطبق با ساعت شروع شیفت نمی باشد،اجازه انجام عمليات را نداريد ";
                            itemInModel.FLG_ERR = "1";
                            result.AddError(itemInModel.DES_ERR);
                            continue;
                        }
                        var timeDuration = Utility.GetDurationStartTimeToEndTime(item.StartTime, item.EndTime);
                        var totalWorkHourInDay = timeSettingDataModel.TotalWorkHourInDay;

                        //اگر در حالت زمان موقت بودیم و روزکار ، وارد این شرط نمیشود
                        bool isTemporaryTime = false;
                        if (item.ShiftSettingFromShiftboard ==false && timeSettingDataModelTemporary != null)//روزکار و زمان موقت
                        {
                            isTemporaryTime = true;
                        }

                        if (!isTemporaryTime&& Utility.ConvertDurationToMinute(timeDuration) < Utility.ConvertDurationToMinute(timeSettingDataModel.TotalWorkHourInDay))
                        {
                            itemInModel.COD_ERR = "";
                            itemInModel.DES_ERR = $"مدت زمان کلاس نباید کمتر از مدت زمان کاری فرد باشد،اجازه انجام عمليات را نداريد ";
                            itemInModel.FLG_ERR = "1";
                            result.AddError(itemInModel.DES_ERR);
                            continue;
                        }
                        EmployeeAttendAbsenceItem newEmployeeAttendAbsenceItem = new EmployeeAttendAbsenceItem()
                        {

                            WorkCalendarId = workCalendar.Id,
                            ShiftConceptDetailId = shiftConceptDetailId,
                            ShiftConceptDetailIdInShiftBoard = ShiftConceptDetailIdInShiftBoard,
                            EmployeeId = tempData.EmployeeId,
                            StartTime = timeSettingDataModel.ShiftStartTime,
                            EndTime = timeSettingDataModel.ShiftEndtTime,
                            EvaluationDevelopmentId = item.evaluationDevelopmentId,

                            TimeDuration = Utility.GetDurationStartTimeToEndTime(timeSettingDataModel.ShiftStartTime, timeSettingDataModel.ShiftEndtTime),
                            RollCallDefinitionId = EnumRollCallDefinication.EvaluationDevelopmentKarkard.Id,
                            WorkTimeId = workTimeId,
                            InvalidRecord = false,
                            InvalidRecordReason = "",
                            IsFloat = false,
                            IsManual = false,
                            InsertDate = DateTime.Now,
                            InsertUser = model.WinUser

                        };
                        employeeAttendAbsenceItemList.Add(newEmployeeAttendAbsenceItem);
                        if (item.EndTime.ConvertStringToTimeSpan() > timeSettingDataModel.ShiftEndtTime.ConvertStringToTimeSpan())
                        {
                            EmployeeAttendAbsenceItem newItemsExtraWork = new EmployeeAttendAbsenceItem()
                            {

                                WorkCalendarId = workCalendar.Id,
                                ShiftConceptDetailId = shiftConceptDetailId,
                                ShiftConceptDetailIdInShiftBoard = ShiftConceptDetailIdInShiftBoard,
                                EmployeeId = tempData.EmployeeId,
                                StartTime = timeSettingDataModel.ShiftEndtTime,
                                EndTime = item.EndTime,
                                TimeDuration = Utility.GetDurationStartTimeToEndTime(timeSettingDataModel.ShiftEndtTime, item.EndTime),
                                RollCallDefinitionId = EnumRollCallDefinication.EvaluationDevelopmentExtraWork.Id,
                                WorkTimeId = workTimeId,
                                EvaluationDevelopmentId = item.evaluationDevelopmentId,
                                InvalidRecord = false,
                                InvalidRecordReason = "",
                                IsFloat = false,
                                IsManual = false,
                                InsertDate = DateTime.Now,
                                InsertUser = model.WinUser

                            };
                            employeeAttendAbsenceItemList.Add(newItemsExtraWork);
                        }
                    }

                }

                else
                {
                    itemInModel.COD_ERR = "";
                    itemInModel.DES_ERR = $"اطلاعات نادرست است، با واحد پشتیبانی تماس حاصل فرمایید";
                    itemInModel.FLG_ERR = "1";
                    result.AddError(itemInModel.DES_ERR);
                    continue;
                }
            }
            try
            {
                if (employeeAttendAbsenceItemList.Any())
                    _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.AddRange(employeeAttendAbsenceItemList);
                //اگر حتی یک نفر خطا داشته باشد ذخیره دیتابیس انجام نشود
                if (result.IsSuccess == true && result.Data.ARR.All(x => x.FLG_ERR != "1"))
                    await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
            }

            return result;
        }

        #endregion
        public bool ISemployeeAttendAbsenceItem(int employeeId, DateTime entryExitDate)
        {
            var workCalendar = _kscHrUnitOfWork.WorkCalendarRepository.GetAllQuerable()
                .Where(a => a.MiladiDateV1 == entryExitDate).FirstOrDefaultAsync().GetAwaiter().GetResult();

            var isFind = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository
                                           .Any(a => a.WorkCalendarId == workCalendar.Id && a.EmployeeId == employeeId);
            return isFind;
        }

        public FilterResult<ReportEmployeeAttendAbsenceItemModel> GetReportEmployeeAttendAbsenceItemData(SearchEmployeeEntryExitModel Filter)
        {
            if (!string.IsNullOrWhiteSpace(Filter.YearMonth))
            {
                Filter.YearMonth = Filter.YearMonth.Fa2En();
                var permonth = Utility.GetPersianMonth(Filter.YearMonth);
                Filter.StartDate = permonth.StartDate;
                Filter.EndDate = permonth.EndDate;
            }
            else
            {
                Filter.StartDate = Filter.StartDateString.Fa2En().ToGregorianDateTime();
                Filter.EndDate = Filter.EndDateString.Fa2En().ToGregorianDateTime();
            }
            var WorkCalendarIds = _kscHrUnitOfWork.WorkCalendarRepository.GetWorkCalendarByRangeDate(Filter.StartDate.Value, Filter.EndDate.Value).Select(x => x.Id).ToList();
            var query = (_kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository
                .GetEmployeeAttendAbsenceItemActiveByEmployeeIdAsNoTracking(Filter.EmployeeId)
                .Where(x => WorkCalendarIds.Contains(x.WorkCalendarId))
                .Include(x => x.WorkCalendar)
                .Include(x => x.RollCallDefinition)
                .Include(x => x.ShiftConceptDetail_ShiftConceptDetailId)
                .Select(x => new ReportEmployeeAttendAbsenceItemModel
                {
                    EmployeeId = x.EmployeeId,
                    ShamsiDate = x.WorkCalendar.ShamsiDateV1,
                    WorkCalendarDate = x.WorkCalendar.MiladiDateV1,
                    WeekDay = x.WorkCalendar.DayNameShamsi,
                    ShiftConceptDetailCode = x.ShiftConceptDetail_ShiftConceptDetailId.Code,
                    TimeDurationInMinute = x.TimeDurationInMinute,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    TimeDuration = x.TimeDuration,
                    RollCallDefinitionCode = x.RollCallDefinition.Code,
                    RollCallDefinitionTitle = x.RollCallDefinition.Title,
                    WorkCalendarId = x.WorkCalendarId,
                    InsertUser = x.InsertUser,
                    InsertDate = x.InsertDate,
                    IsManual = x.IsManual

                })).ToList();

            var sumTime = Utility.ConvertMinuteToTime(query.Sum(x => x.TimeDurationInMinute) ?? 0);

            var result = _FilterHandler.GetFilterResult<ReportEmployeeAttendAbsenceItemModel>(query, Filter, "WorkCalendarId");
            var finalData = _mapper.Map<List<ReportEmployeeAttendAbsenceItemModel>>(result.Data.ToList());
            foreach (var item in finalData)
            {
                item.SumTimeDuration = sumTime.ToString();
            }
            return new FilterResult<ReportEmployeeAttendAbsenceItemModel>()
            {
                Data = finalData,
                Total = result.Total

            };
        }

        #region PrivatePortal
        public async Task<ShowMonthTimeSheetViewModel> GetEmployeeTimeSheet(string employeeNumber, string timeSheetMonth)
        {
            var model = new ShowMonthTimeSheetViewModel();
            var employeeData = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeIncludedTeamWorkByEmployeeId(employeeNumber);
            if (employeeData.TeamWork != null)
            {
                model.TeamCode = employeeData.TeamWork.Code;
                model.TeamTitle = employeeData.TeamWork.Title;
            }
            var viewMisEmployeeSecurity = _kscHrUnitOfWork.ViewMisEmployeeSecurityRepository.GetAllQueryable().AsNoTracking().FirstOrDefault(a => a.TeamCode.ToString() == model.TeamCode && a.DisplaySecurity == 1 && a.TeamWorkIsActive);
            if (viewMisEmployeeSecurity != null)
                model.TeamSupervisor = _kscHrUnitOfWork.EmployeeRepository.Where(x => x.EmployeeNumber == viewMisEmployeeSecurity.EmployeeNumber.ToString()).Select(x => x.Name + " " + x.Family).FirstOrDefault();
            model.TimeSheetViewModel = new List<TimeSheetViewModel>();
            var startEndMonth = timeSheetMonth.GetPersianMonth();
            var entries = _kscHrUnitOfWork.EmployeeEntryExitRepository.GetEmployeeEntryExitValidByEmployeeIdRangeDate(employeeData.Id, startEndMonth.StartDate, startEndMonth.EndDate).OrderBy(x => x.EntryExitDate).AsNoTracking().ToList();
            var employeeAttendAbsenceItem = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository
                .GetEmployeeAttendAbsenceItemIncludWorkCalendarAsNoTracking().Where(x => x.EmployeeId == employeeData.Id && x.InvalidRecord == false && x.WorkCalendar.MiladiDateV1 >= startEndMonth.StartDate && x.WorkCalendar.MiladiDateV1 <= startEndMonth.EndDate).OrderBy(x => x.WorkCalendarId)
                .GroupBy(x => new { x.RollCallDefinitionId, x.WorkCalendarId, x.WorkCalendar.MiladiDateV1 })
                .Select(x => new { x.Key.RollCallDefinitionId, x.Key.WorkCalendarId, x.Key.MiladiDateV1, TimeDurationInMinute = x.Sum(x => x.TimeDurationInMinute ?? 0), IncreasedTimeDuration = x.Sum(x => x.IncreasedTimeDuration ?? 0) }).ToList();
            List<TimeSheetViewModel> timeSheetViewModel = new List<TimeSheetViewModel>();
            var overTimeRollcall = _kscHrUnitOfWork.RollCallDefinitionRepository.WhereQueryable(x => ConstRollCallCategory.OverTime.Any(o => o == x.RollCallCategoryId)).AsQueryable().AsNoTracking().Select(x => x.Id).ToList();
            var workCalendars = _kscHrUnitOfWork.WorkCalendarRepository.Where(x => x.MiladiDateV1 >= startEndMonth.StartDate && x.MiladiDateV1 <= startEndMonth.EndDate).OrderBy(x => x.MiladiDateV1).AsQueryable().AsNoTracking().ToList();
            workCalendars.ForEach(x =>
            {
                var queryEntries = entries.Where(e => e.EntryExitDate.Date == x.MiladiDateV1.Date && !e.IsDeleted).OrderBy(x => x.EntryExitDate).AsQueryable().AsNoTracking().ToList();
                var queryemployeeAttend = employeeAttendAbsenceItem.Where(e => x.MiladiDateV1 == e.MiladiDateV1.Date);
                var sumAbsencePerHour = queryemployeeAttend.Where(e => ConstRollCallDefinicationPortal.AbsenceHour.Any(a => a == e.RollCallDefinitionId)).Sum(x => x.TimeDurationInMinute + x.IncreasedTimeDuration);
                var sumOfficeExit = queryemployeeAttend.Where(e => ConstRollCallDefinicationPortal.OfficilaExit.Any(a => a == e.RollCallDefinitionId)).Sum(x => x.TimeDurationInMinute + x.IncreasedTimeDuration);
                var sumOverTime = queryemployeeAttend.Where(e => overTimeRollcall.Any(a => a == e.RollCallDefinitionId)).Sum(x => x.TimeDurationInMinute + x.IncreasedTimeDuration);
                var sumVacationPerHour = queryemployeeAttend.Where(e => ConstRollCallDefinicationPortal.VacationHour.Any(a => a == e.RollCallDefinitionId)).Sum(x => x.TimeDurationInMinute + x.IncreasedTimeDuration);
                var sumTimeDurationInMinute = queryemployeeAttend.Where(e => ConstRollCallDefinicationPortal.Karkard.Any(a => a == e.RollCallDefinitionId)).Sum(e => e.TimeDurationInMinute + e.IncreasedTimeDuration);
                timeSheetViewModel.Add(new TimeSheetViewModel()
                {
                    TimeEntry = queryEntries.Where(x => x.EntryExitType == 1).Select(x => x.EntryExitTime).ToList(),
                    TimeExit = queryEntries.Where(x => x.EntryExitType == 2).Select(x => x.EntryExitTime).ToList(),
                    TimesheetDate = x.MiladiDateV1.ToPersianDate(),
                    TimesheetDay = x.MiladiDateV1.GetPersianWeekDayName(),
                    AbsencePerHour = sumAbsencePerHour > 0 ? Utility.ConvertMinuteToTime(sumAbsencePerHour) : "",
                    OfficeExit = sumOfficeExit > 0 ? Utility.ConvertMinuteToTime(sumOfficeExit) : "",
                    OverTime = sumOverTime > 0 ? Utility.ConvertMinuteToTime(sumOverTime) : "",
                    VacationPerHour = sumVacationPerHour > 0 ? Utility.ConvertMinuteToTime(sumVacationPerHour) : "",
                    TimesheetDuration = sumTimeDurationInMinute > 0 ? Utility.ConvertMinuteToTime(sumTimeDurationInMinute) : "",
                    TimeSheetApprovalDescription = Utility.GetTimeSheetApprovalDescription(queryemployeeAttend.Select(x => x.RollCallDefinitionId).ToList()),
                });
            }
            );

            model.TimeSheetViewModel = timeSheetViewModel;
            int? SumAbsencePerDay = employeeAttendAbsenceItem.Count(e => e.RollCallDefinitionId == EnumRollCallDefinication.AbsenceDaily.Id);
            model.SumAbsencePerDay = SumAbsencePerDay > 0 ? SumAbsencePerDay.ToString() : "";
            var SumAbsencePerHour = employeeAttendAbsenceItem.Where(e => ConstRollCallDefinicationPortal.AbsenceHour.Any(a => a == e.RollCallDefinitionId)).Sum(x => x.TimeDurationInMinute + x.IncreasedTimeDuration);
            model.SumAbsencePerHour = SumAbsencePerHour > 0 ? Utility.ConvertMinuteToTime(SumAbsencePerHour) : "";
            var SumOverTime = employeeAttendAbsenceItem.Where(e => overTimeRollcall.Any(a => a == e.RollCallDefinitionId)).Sum(x => x.TimeDurationInMinute + x.IncreasedTimeDuration);
            //اضافه کار قهری
            int forcedOverTime = _kscHrUnitOfWork.MonthTimeSheetDraftRepository.GetForcedOverTimeByEmployeeIdYearMonth(employeeData.Id, int.Parse(timeSheetMonth));
            SumOverTime = SumOverTime + forcedOverTime;
            //
            model.SumOverTime = SumOverTime > 0 ? Utility.ConvertMinuteToTime(SumOverTime) : "";
            var SumVacationPerHour = employeeAttendAbsenceItem.Where(e => ConstRollCallDefinicationPortal.VacationHour.Any(a => a == e.RollCallDefinitionId)).Sum(x => x.TimeDurationInMinute + x.IncreasedTimeDuration);
            model.SumVacationPerHour = SumVacationPerHour > 0 ? Utility.ConvertMinuteToTime(SumVacationPerHour) : "";
            var maxTimeEntry = timeSheetViewModel.Max(y => y.TimeEntry.Count());
            var maxTimeExit = timeSheetViewModel.Max(y => y.TimeExit.Count());
            model.MaxCountEntryExit = maxTimeEntry > maxTimeExit ? maxTimeEntry : maxTimeExit;
            var SumVacationPerDay = employeeAttendAbsenceItem.Count(e => e.RollCallDefinitionId == EnumRollCallDefinication.DailyVacation.Id);
            model.SumVacationPerDay = SumVacationPerDay > 0 ? SumVacationPerDay.ToString() : "";

            return await Task.FromResult(model);
        }

        #endregion

        public void SumForcedOverTimeAttendAbsence(bool insertData, List<AddEmployeeAttendAbsenceItemModel> model)
        {
            var data = model.FirstOrDefault();
            int workTimeId = data.WorkTimeId;
            int employeeId = data.EmployeeId;
            int workCalendarId = data.WorkCalendarId;
            int forcedOverTime = data.ForcedOverTime;
            int totalWorkHourInWeek = data.TotalWorkHourInWeek;
            int yearMonth = data.YearMonth;
            int sumTimeDuration = 0;
            bool shiftSettingFromShiftboard = data.ShiftSettingFromShiftboard;

            //

            // var workCalendarData = _kscHrUnitOfWork.WorkCalendarRepository.GetYearMonthByWorkCalendarId(workCalendarId);
            var workCalendarData = _kscHrUnitOfWork.WorkCalendarRepository.GetYearMonthWorkDayTypeByWorkCalendarId(workCalendarId);
            yearMonth = workCalendarData.Item1;
            var workCalendarDate = workCalendarData.Item2;
            var workDayTypeId = workCalendarData.Item3;
            //چک کردن نوع روزها که برای آنها محاسبه قهری غیرمعتبر است
            var invalidDayType = _kscHrUnitOfWork.InvalidDayTypeInForcedOvertimeRepository.GetInvalidDayTypeInForcedOvertimeByWorkTime(workTimeId);
            if (invalidDayType.Any(x => x == workDayTypeId))
            {
                return;
            }
            //

            var monthTimeSheetDraftByEmployeeIdYearMonth = _kscHrUnitOfWork.MonthTimeSheetDraftRepository.GetMonthTimeSheetDraftByEmployeeIdYearMonth(employeeId, yearMonth);
            if (insertData)// ایجاد اطلاعات آیتم
            {
                var monthTimeSheetDraftByWorkTimeId = monthTimeSheetDraftByEmployeeIdYearMonth.FirstOrDefault(x => x.WorkTimeId == workTimeId);
                if (shiftSettingFromShiftboard == false) //شیفتی نباشد
                {
                    sumTimeDuration = SetForcedOverTime(model, workTimeId, employeeId, totalWorkHourInWeek, yearMonth, insertData, null, forcedOverTime);

                }
                else //شیفتی باشد
                {

                    var rollCallIncludedForcedOverTime = _kscHrUnitOfWork.IncludedRollCallRepository.GetAllQueryable()
                .Where(x => x.IsActive && x.IncludedDefinitionId == EnumIncludedDefinition.ForcedOverTime.Id
                ).Select(w => w.RollCallDefinitionId).ToList();

                    if (model.Any(x => rollCallIncludedForcedOverTime.Any(y => y == x.RollCallDefinitionId)))
                    {
                        sumTimeDuration = forcedOverTime;
                    }
                    if (monthTimeSheetDraftByWorkTimeId != null)
                    {
                        sumTimeDuration = monthTimeSheetDraftByWorkTimeId.ForcedOverTime + sumTimeDuration;
                    }
                }
                if (monthTimeSheetDraftByWorkTimeId != null)
                    monthTimeSheetDraftByWorkTimeId.ForcedOverTime = sumTimeDuration;
                else
                {
                    MonthTimeSheetDraft newMonthTimeSheetDraft = new MonthTimeSheetDraft();
                    newMonthTimeSheetDraft.EmployeeId = employeeId;
                    newMonthTimeSheetDraft.YearMonth = yearMonth;
                    newMonthTimeSheetDraft.WorkTimeId = workTimeId;
                    newMonthTimeSheetDraft.ForcedOverTime = forcedOverTime;
                    _kscHrUnitOfWork.MonthTimeSheetDraftRepository.Add(newMonthTimeSheetDraft);
                }
            }
            else //حذف اطلاعات آیتم
            {
                WorkTime workTime = _kscHrUnitOfWork.WorkTimeRepository.GetWorkTimeInAttendAbsenceItemAsNoTracking(employeeId, workCalendarId);
                var monthTimeSheetDraftByWorkTimeId = monthTimeSheetDraftByEmployeeIdYearMonth.FirstOrDefault(x => x.WorkTimeId == workTime.Id);
                if (monthTimeSheetDraftByWorkTimeId != null)
                {
                    //
                    var employee = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeByEmployeeIdNoAsync(model.First().EmployeeId);
                    var timeShiftSettingByWorkCityIdModel = _kscHrUnitOfWork.TimeShiftSettingRepository.GetTimeShiftSettingByWorkCityId(employee.WorkCityId.Value).ToList();
                    //
                    var shiftConceptId = _kscHrUnitOfWork.ShiftConceptDetailRepository.GetShiftConceptIdByShiftConceptDetailId(model.First().ShiftConceptDetailId);
                    var timeShiftSetting = timeShiftSettingByWorkCityIdModel.FirstOrDefault(x => x.WorktimeId == workTime.Id && x.ShiftConceptId == shiftConceptId
        && x.IsTemporaryTime == false && x.ValidityStartDate.Value.Date <= workCalendarDate && x.ValidityEndDate.Value.Date >= workCalendarDate);
                    totalWorkHourInWeek = timeShiftSetting.TotalWorkHourInWeek != null ? timeShiftSetting.TotalWorkHourInWeek.ConvertDurationToMinute().Value : 0;
                    forcedOverTime = timeShiftSetting.ForcedOverTime;
                    //
                    if (workTime.ShiftSettingFromShiftboard)
                    {

                        var rollCallIncludedForcedOverTime = _kscHrUnitOfWork.IncludedRollCallRepository.GetAllQueryable()
              .Where(x => x.IsActive && x.IncludedDefinitionId == EnumIncludedDefinition.ForcedOverTime.Id
              ).Select(w => w.RollCallDefinitionId).ToList();

                        if (model.Any(x => rollCallIncludedForcedOverTime.Any(y => y == x.RollCallDefinitionId)))
                        {
                            monthTimeSheetDraftByWorkTimeId.ForcedOverTime = monthTimeSheetDraftByWorkTimeId.ForcedOverTime - forcedOverTime;
                        }
                    }
                    else
                    {
                        monthTimeSheetDraftByWorkTimeId.ForcedOverTime = SetForcedOverTime(model, workTime.Id, employeeId, totalWorkHourInWeek, yearMonth, insertData, workTime, forcedOverTime);
                    }
                }
            }


        }
        private int SetForcedOverTime(List<AddEmployeeAttendAbsenceItemModel> model, int workTimeId, int employeeId, int totalWorkHourInWeek, int yearMonth, bool insertData, WorkTime worktime, int forcedOverTime)
        {
            int sumTimeDuration = 0;
            var employeeAttendAbsenceItems = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetEmployeeAttendAbsenceItemAsNoTracking();
            var rollCallIncludedForcedOverTime = GetRollCallIncludedForcedOverTime();
            var workCalendar = _kscHrUnitOfWork.WorkCalendarRepository.GetWorkCalendarByYearMonthAsNoTracking(yearMonth);
            var queryEmployeeAttendAbsenceItemByEmployeeId = from item in employeeAttendAbsenceItems
                                                             join cal in workCalendar on item.WorkCalendarId equals cal.Id

                                                             where item.EmployeeId == employeeId && item.InvalidRecord == false
                                                             && item.WorkTimeId == workTimeId
                                                             select new EmployeeAttendAbsenceItemForcedOverTimeModel()
                                                             {
                                                                 EmployeeAttendAbsenceItemId = item.Id,
                                                                 RollCallDefinitionId = item.RollCallDefinitionId,
                                                                 TimeDurationInMinute = item.TimeDurationInMinute.Value,
                                                                 WorkTimeId = item.WorkTimeId,
                                                                 WorkCalendarId = item.WorkCalendarId,
                                                                 WorkCalendarDate = cal.MiladiDateV1

                                                             };
            var employeeAttendAbsenceItemByEmployeeId = queryEmployeeAttendAbsenceItemByEmployeeId.ToList();
            if (insertData)
            {

                var selectedItem = model.Select(x => new EmployeeAttendAbsenceItemForcedOverTimeModel()
                {
                    RollCallDefinitionId = x.RollCallDefinitionId,
                    TimeDurationInMinute = x.TimeDuration.ConvertDurationToMinute().Value,
                    WorkTimeId = x.WorkTimeId,
                    WorkCalendarId = x.WorkCalendarId
                });
                employeeAttendAbsenceItemByEmployeeId = employeeAttendAbsenceItemByEmployeeId.Union(selectedItem).ToList();
                worktime = _kscHrUnitOfWork.WorkTimeRepository.GetWorkTimeByIdAsNoTracking(workTimeId);
            }
            else
            {
                employeeAttendAbsenceItemByEmployeeId = employeeAttendAbsenceItemByEmployeeId.Where(x => x.WorkCalendarId != model.First().WorkCalendarId).ToList();
            }

            sumTimeDuration = CalculateForcedOverTime(employeeAttendAbsenceItemByEmployeeId, rollCallIncludedForcedOverTime, worktime, totalWorkHourInWeek, forcedOverTime);
            return sumTimeDuration;
        }

        private int CalculateForcedOverTime(List<EmployeeAttendAbsenceItemForcedOverTimeModel> employeeAttendAbsenceItemByEmployeeId, List<int> rollCallIncludedForcedOverTime, WorkTime worktime, int totalWorkHourInWeek, int forcedOverTime)
        {
            var rollCallForcedOverTime = from item in employeeAttendAbsenceItemByEmployeeId
                                         join rollCallId in rollCallIncludedForcedOverTime on item.RollCallDefinitionId equals rollCallId
                                         select item;
            var sumTimeDuration = rollCallForcedOverTime.Sum(x => x.TimeDurationInMinute);
            var maximumForcedOverTime1 = worktime.MaximumForcedOverTime.ConvertDurationToMinute();
            var maximumForcedOverTime = forcedOverTime;
            var div = (sumTimeDuration / totalWorkHourInWeek) * 60;//خارج قسمت

            var mod = sumTimeDuration % totalWorkHourInWeek;//باقیمانده
            int baseMinute = 0;
            if (mod > 1440)
            {
                baseMinute = 60;//60min=1 h
            }
            sumTimeDuration = baseMinute + div;
            if (sumTimeDuration >= maximumForcedOverTime)
            {
                // sumTimeDuration = maximumForcedOverTime.Value;
                sumTimeDuration = maximumForcedOverTime;
            }

            return sumTimeDuration;
        }
        private List<int> GetRollCallIncludedForcedOverTime()
        {
            return _kscHrUnitOfWork.IncludedRollCallRepository.GetActiveIncludedRollCallByIncludedIdAsNoTracking(EnumIncludedDefinition.ForcedOverTime.Id).Select(x => x.RollCallDefinitionId).ToList();
        }

        public async Task<string> UpdateForcedOverTimeByShamsiYearMonth(int yearMonth)
        {
            try
            {


                var monthTimeSheetDraft = _kscHrUnitOfWork.MonthTimeSheetDraftRepository.GetMonthTimeSheetDraftByYearMonth(yearMonth);

                // قهری غیرشیفتی
                var rollCallIncludedForcedOverTime = GetRollCallIncludedForcedOverTime();
                var employeeAttendAbsenceItems = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetEmployeeAttendAbsenceItemAsNoTracking();
                var workCalendars = _kscHrUnitOfWork.WorkCalendarRepository.GetWorkCalendarByYearMonthAsNoTracking(yearMonth);
                var queryEmployeeAttendAbsenceItemByEmployeeId = from item in employeeAttendAbsenceItems
                                                                 join cal in workCalendars on item.WorkCalendarId equals cal.Id
                                                                 join time in _kscHrUnitOfWork.WorkTimeRepository.GetAllQueryable() on item.WorkTimeId equals time.Id
                                                                 where item.InvalidRecord == false && time.ShiftSettingFromShiftboard == false

                                                                 select new EmployeeAttendAbsenceItemForcedOverTimeModel()
                                                                 {
                                                                     EmployeeId = item.EmployeeId,
                                                                     EmployeeAttendAbsenceItemId = item.Id,
                                                                     RollCallDefinitionId = item.RollCallDefinitionId,
                                                                     TimeDurationInMinute = item.TimeDurationInMinute.Value,
                                                                     WorkTimeId = item.WorkTimeId,
                                                                     WorkCalendarId = item.WorkCalendarId,
                                                                     WorkCalendarDate = cal.MiladiDateV1

                                                                 };
                var employeeAttendAbsenceItem = queryEmployeeAttendAbsenceItemByEmployeeId.ToList();
                var employeeList = employeeAttendAbsenceItem.Select(x => new { x.EmployeeId, x.WorkTimeId }).Distinct().ToList();
                var workTimes = _kscHrUnitOfWork.WorkTimeRepository.GetAllQueryable().ToList();
                foreach (var item in employeeList)
                {
                    var workTime = workTimes.First(x => x.Id == item.WorkTimeId);
                    var employeeAttendAbsenceItemByEmployeeId = employeeAttendAbsenceItem.Where(x => x.EmployeeId == item.EmployeeId && x.WorkTimeId == item.WorkTimeId).ToList();
                    var forcedOverTime = CalculateForcedOverTime(employeeAttendAbsenceItemByEmployeeId, rollCallIncludedForcedOverTime, workTime, 2700, 240);
                    if (forcedOverTime != 0)
                    {
                        var monthTimeSheetDraftByEmployeeId = monthTimeSheetDraft.Where(x => x.EmployeeId == item.EmployeeId && x.WorkTimeId == item.WorkTimeId).FirstOrDefault();
                        if (monthTimeSheetDraftByEmployeeId != null)
                            monthTimeSheetDraftByEmployeeId.ForcedOverTime = forcedOverTime;
                        else
                        {
                            MonthTimeSheetDraft newMonthTimeSheetDraft = new MonthTimeSheetDraft();
                            newMonthTimeSheetDraft.EmployeeId = item.EmployeeId;
                            newMonthTimeSheetDraft.YearMonth = yearMonth;
                            newMonthTimeSheetDraft.WorkTimeId = item.WorkTimeId;
                            newMonthTimeSheetDraft.ForcedOverTime = forcedOverTime;
                            await _kscHrUnitOfWork.MonthTimeSheetDraftRepository.AddAsync(newMonthTimeSheetDraft);
                        }
                    }
                }

                // قهری شیفتی
                var timeShiftSettingForcedOverTimeModel = _kscHrUnitOfWork.TimeShiftSettingRepository.GetAllByIncludedAsNotracking(DateTime.Now)
                                                          .Select(x => new ForcedOverTimeModel()
                                                          {
                                                              ShiftConceptId = x.WorkCompanySetting.WorkTimeShiftConcept.ShiftConceptId,
                                                              WorkTimeId = x.WorkCompanySetting.WorkTimeShiftConcept.WorkTimeId,
                                                              WorkCityId = x.WorkCompanySetting.WorkCityId,
                                                              ForcedOverTime = x.ForcedOverTime,
                                                              TotalWorkHourInWeek = x.TotalWorkHourInWeek
                                                          }).ToList();
                var employeeAttendAbsenceItemShift = from item in employeeAttendAbsenceItems
                                                     join cal in workCalendars on item.WorkCalendarId equals cal.Id
                                                     join time in _kscHrUnitOfWork.WorkTimeRepository.GetAllQueryable() on item.WorkTimeId equals time.Id
                                                     join shift in _kscHrUnitOfWork.ShiftConceptDetailRepository.GetAllQueryable() on item.ShiftConceptDetailId equals shift.Id
                                                     join empl in _kscHrUnitOfWork.EmployeeRepository.GetAllQueryable() on item.EmployeeId equals empl.Id

                                                     where item.InvalidRecord == false && time.ShiftSettingFromShiftboard == true
                                                     select new { item.EmployeeId, item.WorkCalendarId, item.WorkTimeId, shift.ShiftConceptId, empl.WorkCityId };
                var shiftForcedOverTimeModel = employeeAttendAbsenceItemShift.GroupBy(x => new { x.EmployeeId, x.WorkCityId, x.WorkTimeId, x.ShiftConceptId }).Select(y => new
                {
                    EmployeeId = y.Key.EmployeeId,
                    WorkCityId = y.Key.WorkCityId,
                    WorkTimeId = y.Key.WorkTimeId,
                    ShiftConceptId = y.Key.ShiftConceptId,
                    Count = y.Count()
                }).ToList();
                int sumTimeDurationShift = 0;
                foreach (var item in shiftForcedOverTimeModel)
                {
                    sumTimeDurationShift = 0;
                    var forcedOverTimeSetting = timeShiftSettingForcedOverTimeModel.FirstOrDefault(x => x.ShiftConceptId == item.ShiftConceptId && x.WorkTimeId == item.WorkTimeId && x.WorkCityId == item.WorkCityId);
                    if (forcedOverTimeSetting == null)
                    {
                        //  return item.EmployeeId.ToString() + "**" + item.ShiftConceptId.ToString() + "**" + item.WorkTimeId.ToString() + "**" + item.WorkCityId.ToString();
                        continue;
                    }
                    var forcedOverTime = forcedOverTimeSetting.ForcedOverTime;
                    if (forcedOverTime != null)
                    {
                        sumTimeDurationShift = item.Count * forcedOverTime.ConvertDurationToMinute().Value;
                        var monthTimeSheetDraftByEmployeeId = monthTimeSheetDraft.FirstOrDefault(x => x.EmployeeId == item.EmployeeId && x.WorkTimeId == item.WorkTimeId);
                        if (monthTimeSheetDraftByEmployeeId != null)
                        {
                            monthTimeSheetDraftByEmployeeId.ForcedOverTime = monthTimeSheetDraftByEmployeeId.ForcedOverTime + sumTimeDurationShift;
                        }
                        else
                        {
                            MonthTimeSheetDraft newMonthTimeSheetDraft = new MonthTimeSheetDraft();
                            newMonthTimeSheetDraft.EmployeeId = item.EmployeeId;
                            newMonthTimeSheetDraft.YearMonth = yearMonth;
                            newMonthTimeSheetDraft.WorkTimeId = item.WorkTimeId;
                            newMonthTimeSheetDraft.ForcedOverTime = sumTimeDurationShift;
                            await _kscHrUnitOfWork.MonthTimeSheetDraftRepository.AddAsync(newMonthTimeSheetDraft);
                        }
                    }
                    else
                    {
                        // return item.ShiftConceptId.ToString()+"**"+ item.WorkTimeId.ToString() + "**" + item.WorkCityId.ToString();
                    }
                }
                await _kscHrUnitOfWork.SaveAsync();

                return "موفق";
            }
            catch (Exception ex)
            {

                return ex.Message;
            }

        }

        #region employeeAttendAbssenceOperation
        /// <summary>
        /// مدیریت ذخیره تایید کارکرد
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public KscResult AddUpdateEmplyeeAttendAbsenseItems(List<AddEmployeeAttendAbsenceItemModel> models, bool IsOfficialAttend, bool NotCheckMinimumWorkTimeAdmin)
        {
            lock (this)
            {
                var result = new KscResult();
                try
                {
                    if (!models.Any())
                    {
                        result.AddError("", "اطلاعاتی برای  تایید کارکرد وجود ندارد");
                        return result;
                    }
                    //چک کردن درخواست جابجایی
                    result = CheckTransferShiftRequest(models);
                    if (result.Success == false)
                    {
                        return result;
                    }
                    //
                    //چک کردن درخواست فراخوان
                    if (!IsOfficialAttend)
                    {
                        result = CheckOncallRequest(models);
                        if (result.Success == false)
                        {
                            return result;
                        }
                    }
                    //
                    var Ids = models.Where(a => a.Id > 0).Select(a => a.Id).ToList();
                    var employeeId = models.Select(a => a.EmployeeId).First();
                    var workCalendarId = models.Select(a => a.WorkCalendarId).First();

                    var isBossAfairs = models.First().IsBossAfairs;

                    if (isBossAfairs == false)
                    {

                        result = CheckAllRollCallDefinitionCompare(models);
                        if (result.Success == false)
                        {
                            return result;
                        }
                        result = CheckCodeInStartAndEndShiftAnalyse(models);
                        if (result.Success == false)
                        {
                            return result;
                        }

                        result = CheckOverTime(models);
                        if (result.Success == false)
                        {
                            return result;
                        }




                        result = CheckMorakhasi(models);
                        if (result.Success == false)
                        {
                            return result;
                        }
                        if (models.Any(a => a.OverTimeIsOverMax))//اصلاح اضافه کار بالای سقف پرسنل
                        {
                            var RollCallOverTimeLimitAndAverage = _kscHrUnitOfWork.IncludedRollCallRepository.GetAllAsNoTrackingByIncludedDefinition()
                            .Where(a => a.IncludedDefinitionId == EnumIncludedDefinition.AverageOverTime.Id || a.IncludedDefinitionId == EnumIncludedDefinition.MaximunOverTime.Id).ToList();

                            var rollCallDefinitions = RollCallOverTimeLimitAndAverage.Where(a => a.IncludedDefinitionId == EnumIncludedDefinition.MaximunOverTime.Id).Select(a => a.RollCallDefinitionId).ToList();
                            models = models.Where(a => (a.IsDeleted == true
                                                        && rollCallDefinitions.Contains(a.RollCallDefinitionId))
                                                     || (a.IsDeleted == false
                                                            && (a.RollCallConceptId != EnumRollCallConcept.OverTime.Id
                                                                || !rollCallDefinitions.Contains(a.RollCallDefinitionId)))

                             ).ToList();
                        }

                    }

                    //بررسی شرط حداقل زمان کاری
                    //به درخواست آقای حریر فروش تمام کاربران قادر به نقض این شرط نمیباشند
                    if (!NotCheckMinimumWorkTimeAdmin)
                        result = CheckMinWorkTimeEmployee(models);
                    if (result.Success == false)
                    {
                        return result;
                    }


                    if (Ids.Any())
                    {
                        var getAttendAbccense = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetAllQuaryble(new List<long>());
                        var DeletedAttendAbccenceItem = getAttendAbccense.Include(a => a.EmployeeEntryExitAttendAbsences)
                            .Where(a => a.EmployeeId == employeeId && a.WorkCalendarId == workCalendarId && Ids.All(c => c != a.Id)).ToList();
                        foreach (var attenItemForDeleted in DeletedAttendAbccenceItem)
                        {
                            foreach (var employeeEntryExitAttendAbsence in attenItemForDeleted.EmployeeEntryExitAttendAbsences)
                            {
                                _kscHrUnitOfWork.EmployeeEntryExitAttendAbsenceRepository.Delete(employeeEntryExitAttendAbsence);
                            }
                            _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.Delete(attenItemForDeleted);
                        }

                    }
                    var rollCalDefinitionIds = models.Select(a => a.RollCallDefinitionId).ToList();
                    foreach (var model in models)
                    {
                        // اضافه کاریهایی که به شب و روز شکسته میشند ، در صورت حذف یکی ، باقی نیز باید حذف شوند
                        if (model.OverTimeToken != null && models.Any(x => x.OverTimeToken == model.OverTimeToken && x.IsDeleted == true))
                        {
                            model.IsDeleted = true;
                        }
                        //
                        var compatibaleRollCall = _kscHrUnitOfWork.CompatibleRollCallRepository
                        .Where(a => a.RollCallDefinitionId == model.RollCallDefinitionId).Select(a => a.CompatibleRollCallId).ToList();
                        var IsCompatibale = rollCalDefinitionIds.All(a => compatibaleRollCall.Any(c => c == a));
                        if (IsCompatibale == false)
                        {
                            result.AddError("عدم سازگاری", "کدهای انتخاب شده باهم سازگار نیستند");

                            return result;
                        }
                        //
                        if (model.Id > 0)
                        {
                            result.AddError("", "این کاربر تایید کارکرد شده است");
                            return result;
                        }
                        else
                        {
                            result = AddEmplyeeAttendAbsenseItems(model);
                            if (result.Success == false)
                            {
                                return result;
                            }
                        }
                    }
                    #region  محاسبه اضافه کار قهری
                    //
                    if (models.First().InvalidForcedOvertime == false)
                    {
                        SumForcedOverTimeAttendAbsence(true, models);
                    }
                    #endregion
                    //
                    _kscHrUnitOfWork.Save();
                }
                finally
                {
                    Monitor.Enter(this);
                }


                return result;
            }
        }
        private KscResult CheckTransferShiftRequest(List<AddEmployeeAttendAbsenceItemModel> models)
        {
            var result = new KscResult();
            var transferRequest = _kscHrUnitOfWork.Transfer_RequestRepository.GetTransferRequestByEemployeeIdRequestTypeId(models.First().EmployeeId, EnumRequestType.ShiftTransfer.Id);
            if (transferRequest.Any())
            {
                var workCalendar = _kscHrUnitOfWork.WorkCalendarRepository.GetById(models.First().WorkCalendarId);
                if (transferRequest.Any(x => x.TransferChangeDate <= workCalendar.MiladiDateV1 && x.RequestdWorkGroup.WorkTimeId != models.First().WorkTimeId
                 && x.WF_Request.StatusId == EnumWorkFlowStatus.TransferShiftTeamCreate.Id))
                {
                    result.AddError("", "درخواست جا به جایی برای شیفتی دیگر وجود دارد،امکان تایید کارکرد وجود ندارد");
                }
            }
            return result;
        }
        private KscResult CheckOncallRequest(List<AddEmployeeAttendAbsenceItemModel> models)
        {
            var result = new KscResult();
            var model = models.First();
            var workCalendar = _kscHrUnitOfWork.WorkCalendarRepository.GetById(model.WorkCalendarId);
            var oncallNotCanceled = _kscHrUnitOfWork.OnCall_RequestRepository.GetRequestByEmployeeIdOncallDate(model.EmployeeId, workCalendar.MiladiDateV1.Date, EnumWorkFlowStatus.Cancel.Id);
            if (oncallNotCanceled.Any() && oncallNotCanceled.Any(x => x.WF_Request.StatusId != EnumWorkFlowStatus.Finished.Id
           && x.WF_Request.StatusId != EnumWorkFlowStatus.ReferToOfficialManagement.Id
            ))
            {
                result.AddError("", $"کاربر دارای درخواست فراخوان بررسی نشده با شماره درخواست {oncallNotCanceled.First().RequestId}،می باشد امکان تایید کارکرد وجود ندارد");
            }

            return result;
        }
        private KscResult CheckAllRollCallDefinitionCompare(List<AddEmployeeAttendAbsenceItemModel> models)
        {
            var result = new KscResult();
            var rollcallDefinitionIdsmodel = models.Select(a => a.RollCallDefinitionId).ToList();
            var rocallDefinitionsList = _kscHrUnitOfWork.RollCallDefinitionRepository.GetAllIncluded().Where(a => rollcallDefinitionIdsmodel.Contains(a.Id)).ToList();
            foreach (var item in rollcallDefinitionIdsmodel)
            {
                var model = models.Where(c => c.RollCallDefinitionId == item); ;
                var rollCall = rocallDefinitionsList.Where(a => a.Id == item).First();
                var sumduration = model.Sum(a => a.TimeDuration.ConvertDurationToMinute());
                if (sumduration > rollCall.ValidityMaximumTimeMinute)
                {

                    result.AddError("", "حد اکثر زمان مجاز برای کد:" + rollCall.Title + "\n " + rollCall.ValidityMaximumTime + " دقیقه می باشد");
                    return result;
                }
                if (sumduration < rollCall.ValidityMinimumTimeMinute)
                {
                    result.AddError("", "حد اقل زمان مجاز برای کد:" + rollCall.Title + "\n " + rollCall.ValidityMaximumTime + " دقیقه می باشد");
                    return result;
                }
            }
            var firstRecordModel = models.First();
            var conditionalAbsenceRollCall = _kscHrUnitOfWork.ConditionalAbsenceSubjectTypeRepository.GetRollCallDailyAbsence()
                .Select(x => new { ConditionalAbsenceSubjectTypeId = x.Id, RollCallDefinitionId = x.RollCallDefinitionId }).ToList();
            var conditionalAbsenceDailyRollCall = conditionalAbsenceRollCall
                .Join(models, x => x.RollCallDefinitionId, m => m.RollCallDefinitionId, (x, m) => x).FirstOrDefault();
            if (conditionalAbsenceDailyRollCall != null)
            {
                var workCalendar = _kscHrUnitOfWork.WorkCalendarRepository.GetById(firstRecordModel.WorkCalendarId);
                var employeeConditionalAbsence = _kscHrUnitOfWork.EmployeeConditionalAbsenceRepository
                       .GetEmployeeConditionalAbsenceForCreateItemAttendAbsence(firstRecordModel.EmployeeId
                       , workCalendar.MiladiDateV1, conditionalAbsenceDailyRollCall.ConditionalAbsenceSubjectTypeId);
                if (employeeConditionalAbsence == null)
                {
                    result.AddError("خطا", $"امکان استفاده از کد {conditionalAbsenceDailyRollCall.RollCallDefinitionId} وجود ندارد");

                    return result;
                }
                else
                {
                    var rollCall = rocallDefinitionsList.First(x => x.Id == conditionalAbsenceDailyRollCall.RollCallDefinitionId);
                    if (rollCall.TimesAllowedUsePerWeek != 0 && rollCall.TimesAllowedUsePerWeek != null)
                    {
                        var countUsedRollcalInWeek = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetEmployeeAttendAbsenceItemByEmployeeIdForConditionalAbsence(firstRecordModel.EmployeeId, conditionalAbsenceDailyRollCall.RollCallDefinitionId, workCalendar.MiladiDateV1);
                        if (countUsedRollcalInWeek >= rollCall.TimesAllowedUsePerWeek)
                        {
                            result.AddError("", $"تعداد دفعات استفاده از کد {rollCall.Title} در هفته {rollCall.TimesAllowedUsePerWeek} بار می باشد");
                            return result;
                        }
                    }
                }
            }
            return result;
        }
        private KscResult CheckCodeInStartAndEndShiftAnalyse(List<AddEmployeeAttendAbsenceItemModel> models)
        {
            var result = new KscResult();

            var ShiftConceptDetailId = models.First().ShiftConceptDetailId;
            var WorkCityId = models.First().WorkCityId;
            var WorkGroupId = models.First().WorkGroupId;
            var WorkCalendarId = models.First().WorkCalendarId;

            var ShiftStartEndTime = GetShiftStartEndTime(ShiftConceptDetailId, WorkCityId.Value, WorkGroupId.Value, WorkCalendarId).GetAwaiter().GetResult();
            var ShiftStartTime = ShiftStartEndTime.ShiftStartTime;
            var ShiftEndTime = ShiftStartEndTime.ShiftEndTime;
            var firstModels = models.OrderBy(a => a.StartTime.ConvertDurationToMinute()).First(a => a.StartTime == ShiftStartEndTime.ShiftStartTime);
            var lastModels = models.OrderByDescending(a => a.EndTime.ConvertDurationToMinute()).First(a => a.EndTime == ShiftStartEndTime.ShiftEndTime);

            var rollCallDefinitions = _kscHrUnitOfWork.RollCallDefinitionRepository.Where(a => a.Id == firstModels.RollCallDefinitionId || a.Id == lastModels.RollCallDefinitionId).ToList();
            if (!rollCallDefinitions.Any(c => c.Id == firstModels.RollCallDefinitionId && c.IsValidInShiftStart))
            {
                result.AddError("", "کد خروج اداری نمیتواند در شروع شیفت انتخاب شود");
                return result;
            }
            if (!rollCallDefinitions.Any(c => c.Id == lastModels.RollCallDefinitionId && c.IsValidInShiftEnd))
            {
                result.AddError("", "کد خروج اداری نمیتواند در شروع شیفت انتخاب شود");
                return result;
            }
            return result;
        }
        /// <summary>
        /// بررسی مرخصی
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        private KscResult CheckMorakhasi(List<AddEmployeeAttendAbsenceItemModel> models)
        {
            var result = new KscResult();
            var listid = new List<int> { EnumRollCallDefinication.HourlyVacation.Id, //مرخصی ساعتی روز کار
                                         EnumRollCallDefinication.ShiftHourlyVacation.Id,//مرخصی ساعتی شیفت
                                         EnumRollCallDefinication.DailyVacation.Id };//مرخصی روزانه

            var modelsLeaveBalanceSend = models.Where(a => listid.Contains(a.RollCallDefinitionId)).Sum(a => a.TimeDuration.ConvertDurationToMinute());
            if (modelsLeaveBalanceSend == 0)
            {
                return result;
            }
            var Ids = models.Where(a => a.Id > 0).Select(a => a.Id).ToList();
            var employeeId = models.Select(a => a.EmployeeId).First();
            var workCalendarId = models.Select(a => a.WorkCalendarId).First();
            var shiftConceptDetailId = models.Select(a => a.ShiftConceptDetailId).First();
            var workCalendar = _kscHrUnitOfWork.WorkCalendarRepository.GetById(workCalendarId);
            var workCityId = models.Select(a => a.WorkCityId).First();
            var employeeWorkGroups = _kscHrUnitOfWork.EmployeeWorkGroupRepository.GetEmployeeWorkGroupIncloudedWorkTimeWorkGroup()
                    .Where(a => a.EmployeeId == employeeId &&
                    a.StartDate <= workCalendar.MiladiDateV1 &&
                    (a.EndDate >= workCalendar.MiladiDateV1 || a.EndDate.HasValue == false)).FirstOrDefault();

            var minWorkTime = _kscHrUnitOfWork.TimeShiftSettingRepository.GetShiftDateTimeSetting(employeeId, shiftConceptDetailId, workCityId.Value, employeeWorkGroups.WorkGroupId, workCalendarId);


            //LeaveBalance
            var inputModel = new EmployeeEntryExitManagementInputModel()
            {
                EmployeeId = employeeId,
                EntryExitDate = workCalendar.MiladiDateV1
            };
            var vacationEntitlementTimePerMonth = _kscHrUnitOfWork.TimeSheetSettingRepository.GetTimeSheetSettingActive().VacationEntitlementTimePerMonth;

            var MeritVacationRemaining = EnumVacation.MeritVacationRemaining.Id.ToString();
            var remianingVacationInLastMonth = _kscHrUnitOfWork.EmployeeVacationManagementRepository.GetAllQueryable().AsNoTracking()
                .FirstOrDefault(a => a.EmployeeId == inputModel.EmployeeId && a.Vacation.Code == MeritVacationRemaining).Duration.Value;

            //var remianingVacationInLastMonth = _kscHrUnitOfWork.EmployeeRepository.GetAllQueryable().FirstOrDefault(x => x.Id == employeeId).RemianingVacationInMinute;
            var remianingvacation = ((vacationEntitlementTimePerMonth.ConvertDurationToMinute() + remianingVacationInLastMonth)
                                      - sumTimeDuration(inputModel));

            var timeSheetSettingActive = _kscHrUnitOfWork.TimeSheetSettingRepository.GetTimeSheetSettingActive();
            var LeaveBalanceFinal = remianingvacation != null ? remianingvacation : 0.0;
            if (LeaveBalanceFinal > 0)
            {
                return result;
            }
            var remianingvacationToDay = LeaveBalanceFinal.Value.ConvertMinDobleWithPaddLeftToDaysHour(9 * 60);
            var LeaveBalance = LeaveBalanceFinal;// مانده مرخصی


            var LeaveBalanceSend = LeaveBalance - modelsLeaveBalanceSend;
            var dayhourmin = $"{timeSheetSettingActive.MinimumDailyVacation}:{0}:{0}";
            if (LeaveBalanceSend < 0)
            {
                LeaveBalanceSend = LeaveBalanceSend * (-1);
            }
            var minDayLeave = dayhourmin.ConverStringDaysHourToMin(timeSheetSettingActive.WorkDayDuration.ConvertStringToTimeSpan().TotalMinutes);

            if (minDayLeave < LeaveBalanceSend)
            {

                result.AddError("", "مانده مرخصی فرد کمتر از حد مجاز می باشد");
                return result;
            }



            return result;
        }

        /// <summary>
        /// بررسی حداقل کارکرد یک فرد
        /// 
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        private KscResult CheckMinWorkTimeEmployee(List<AddEmployeeAttendAbsenceItemModel> models)
        {
            var result = new KscResult();
            var Ids = models.Where(a => a.Id > 0).Select(a => a.Id).ToList();
            var employeeId = models.Select(a => a.EmployeeId).First();
            var workCalendarId = models.Select(a => a.WorkCalendarId).First();
            var workCityId = models.Select(a => a.WorkCityId).First();
            var shiftConceptDetailId = models.Select(a => a.ShiftConceptDetailId).First();
            var workCalendar = _kscHrUnitOfWork.WorkCalendarRepository.GetById(workCalendarId);
            var employeeWorkGroups = _kscHrUnitOfWork.EmployeeWorkGroupRepository.GetEmployeeWorkGroupIncloudedWorkTimeWorkGroup()
                    .Where(a => a.EmployeeId == employeeId &&
                    a.StartDate <= workCalendar.MiladiDateV1 &&
                    (a.EndDate >= workCalendar.MiladiDateV1 || a.EndDate.HasValue == false)).FirstOrDefault();


            var minWorkTime = _kscHrUnitOfWork.TimeShiftSettingRepository.GetShiftDateTimeSetting(employeeId, shiftConceptDetailId, workCityId.Value, employeeWorkGroups.WorkGroupId, workCalendarId);
            var hourint = string.IsNullOrEmpty(minWorkTime.MinimumWorkHourInDay) ? 0
                : minWorkTime.MinimumWorkHourInDay.ConvertDurationToMinute();

            if (minWorkTime.IsRestShift == false && hourint > 0)
            {
                var rollCallCategory = _kscHrUnitOfWork.RollCallDefinitionRepository.GetRollCallDefinitionAsNoTracking()
                    .Where(a => a.RollCallCategoryId == EnumRollCallCategory.Karkard.Id)
                    .Select(a => a.Id).ToList();
                //
                var SumWorkTime = models
                    .Where(a => rollCallCategory.Contains(a.RollCallDefinitionId)
                    || a.RollCallConceptId == EnumRollCallConcept.Attend.Id)
                    .Sum(a => a.TimeDuration.ConvertDurationToMinute());
                //زمان موقت باشد مجموع کارکرد را از این طریق حساب میکنیم
                if (models.First().AttendTimeInTemprorayTime.HasValue)
                {
                    var rollCallCategoryAttend = models
                   .Where(a => a.RollCallConceptId != EnumRollCallConcept.Attend.Id && rollCallCategory.Contains(a.RollCallDefinitionId))
                   .Sum(a => a.TimeDuration.ConvertDurationToMinute());
                    SumWorkTime = (int)models.First().AttendTimeInTemprorayTime.Value;
                    if (rollCallCategoryAttend.HasValue)
                        SumWorkTime += rollCallCategoryAttend.Value;

                }
                //
                if (hourint > SumWorkTime && SumWorkTime > 0)
                {
                    result.AddError("", "مجموع کارکرد پرسنل کمتر از حد مجاز می باشد");
                    return result;
                }
            }



            return result;
        }

        /// <summary>
        /// بررسی اضافه کار سقف یک کاربر
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        private KscResult CheckOverTime(List<AddEmployeeAttendAbsenceItemModel> models)
        {
            var result = new KscResult();
            var Ids = models.Where(a => a.Id > 0).Select(a => a.Id).ToList();
            var employeeId = models.Select(a => a.EmployeeId).First();
            var workCalendarId = models.Select(a => a.WorkCalendarId).First();
            var ExistDay = _kscHrUnitOfWork.WorkCalendarRepository.GetById(workCalendarId).MiladiDateV1;
            var miladiStartcurrentMonth = ExistDay.GetPersianMonthStartAndEndDates().StartDate;
            var miladiEndDaycurrentMonth = ExistDay.GetPersianMonthStartAndEndDates().EndDate;
            //لیست ای دی هایی که به کتگوری مرخص 8 وصل است
            var RollCallOverTimeLimitAndAverage = _kscHrUnitOfWork.IncludedRollCallRepository.GetAllAsNoTrackingByIncludedDefinition()
            .Where(a => a.IncludedDefinitionId == EnumIncludedDefinition.AverageOverTime.Id || a.IncludedDefinitionId == EnumIncludedDefinition.MaximunOverTime.Id).ToList();

            var rollCallDefinitions = RollCallOverTimeLimitAndAverage.Where(a => a.IncludedDefinitionId == EnumIncludedDefinition.MaximunOverTime.Id).Select(a => a.RollCallDefinitionId).ToList();

            var employeeAttendAbsenceItem = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetEmployeeAttendAbsenceItemByRelated()
                .Where(x => x.InvalidRecord == false)
                .Where(x => employeeId == x.EmployeeId && x.WorkCalendar.MiladiDateV1 >= miladiStartcurrentMonth
                && x.WorkCalendar.MiladiDateV1 <= miladiEndDaycurrentMonth
                && rollCallDefinitions.Contains(x.RollCallDefinitionId)
                ).ToList();


            var iputmodel = new EmployeeEntryExitManagementInputModel()
            {
                EntryExitDate = ExistDay,
                EmployeeId = employeeId
            };
            var teamWorkCode = models.Select(a => a.TeamWorkCode).First();
            var teamWork = _kscHrUnitOfWork.TeamWorkRepository.GetByCode(teamWorkCode);
            if (teamWork == null)
            {
                result.AddError("اضافه کاری", "تیم کاربر مشخص نمی باشد");
                return result;
            }
            var rollCallDefinitionIds = models.Select(a => a.RollCallDefinitionId).ToList();
            var employeeOverTime = sumCeilingOvertime(iputmodel);
            var SumOverTimeSelected = models.Where(a => a.IsDeleted == false && rollCallDefinitions.Contains(a.RollCallDefinitionId)).Sum(a => a.TimeDuration.ConvertDurationToMinute());
            if (SumOverTimeSelected > 0)
            {
                var sumoverTime = employeeOverTime;
                var maxmimunTeamOverTimeMinute = teamWork.OverTimeDefinition.MaximumDuration.ConvertDurationToMinute();
                if (sumoverTime > maxmimunTeamOverTimeMinute)
                {
                    result.Id = "isdeletedTrue";
                    result.AddError("اضافه کاری", "اضافه کار بررسی شود");
                    return result;

                }
            }

            var oldShift = models.Select(a => a.OldShiftConceptDetailId).First();
            var newShift = models.Select(a => a.ShiftConceptDetailId).First();

            if (oldShift != newShift)
            {
                var CountchangeShiftInItem = employeeAttendAbsenceItem.Where(a => a.ShiftConceptDetailId != a.ShiftConceptDetailIdInShiftBoard).GroupBy(a => a.WorkCalendarId).Count();
                var maximumShiftChange = _kscHrUnitOfWork.TimeSheetSettingRepository.GetTimeSheetSettingActive();
                if (CountchangeShiftInItem >= maximumShiftChange.MinimumOverTimeAfterShiftInMinut)
                {
                    result.AddError("اضافه کاری", "تعداد تغییر شیفت بیش از حد مجاز است");
                    return result;
                }
            }
            return result;
        }

        private KscResult IsCanAdd(AddEmployeeAttendAbsenceItemModel model)
        {
            var result = new KscResult();
            // var teamWork = _kscHrUnitOfWork.TeamWorkRepository.GetByCode(model.TeamWorkCode);
            var ExistDay = _kscHrUnitOfWork.WorkCalendarRepository.GetById(model.WorkCalendarId).MiladiDateV1;
            var iputmodel = new EmployeeEntryExitManagementInputModel()
            {
                EntryExitDate = ExistDay,
                EmployeeId = model.EmployeeId
            };

            if (_kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.Any(a => a.EmployeeId == model.EmployeeId && a.WorkCalendarId == model.WorkCalendarId))
            {
                result.AddError("", "این کاربر تایید کارکرد شده است");
                return result;
            }


            var firstDate = DateTime.Now.Date;
            var isHaveEmployeeEntryExitAttendAbsences = _kscHrUnitOfWork.EmployeeEntryExitAttendAbsenceRepository
                .Any(a => (a.EmployeeEntryExitId == model.ExitId ||
                           a.EmployeeEntryExitId == model.EntryId)
                && a.EmployeeEntryExit.EntryExitDate.Date <= firstDate);
            if (isHaveEmployeeEntryExitAttendAbsences)
            {
                return result;
            }

            var rollcall = _kscHrUnitOfWork.RollCallDefinitionRepository.GetById(model.RollCallDefinitionId);
            var workCalendar = _kscHrUnitOfWork.WorkCalendarRepository.GetById(model.WorkCalendarId);
            var attendAbsenceItems = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository
                .GetEmployeeAttendAbsenceItemByEmployeeIdWorkCalendarIdAsNoTracking(model.EmployeeId, 0)
                .Include(a => a.WorkCalendar)
                .Where(a => a.RollCallDefinitionId == model.RollCallDefinitionId);
            var useDay = attendAbsenceItems.Where(a => a.WorkCalendarId == model.WorkCalendarId).Count();
            if (rollcall.TimesAllowedUsePerDay < useDay)
            {
                result.AddError("", "تعداد دفعات استفاده شده در روز" + rollcall.Title + " بیش از حد مجاز است");
                return result;
            }
            var useMonth = attendAbsenceItems.Where(a => a.WorkCalendar.YearMonthV1 == workCalendar.YearMonthV1).Count();
            if (rollcall.TimesAllowedUsePerMonth < useMonth)
            {
                result.AddError("", "تعداد دفعات استفاده شده در ماه" + rollcall.Title + " بیش از حد مجاز است");
                return result;
            }
            var useYear = attendAbsenceItems.Where(a => a.WorkCalendar.YyyyShamsi == workCalendar.YyyyShamsi).Count();
            if (rollcall.ValidityDayNumberInYear < useYear)
            {
                result.AddError("", "تعداد دفعات استفاده شده در سال" + rollcall.Title + " بیش از حد مجاز است");
                return result;
            }
            return result;
        }


        /// <summary>
        /// ورود و خروج
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public FilterResult<ReportEmployeeAttendAbsenceItemModel> GetReportEmployeeEntryExitData(SearchEmployeeEntryExitModel filter)
        {
            filter.FixSortandFilter<EmployeeEntryExit>(new Dictionary<string, string>
                {
                    { "PersonalNumber", "eq" },
                    { "EntryExitType", "eq" },
                    { "IsCreatedManualNum", "eq" },
                    { "IsDeletedNum", "eq" }
                });

            var startDate = DateTime.Now;
            var endDate = DateTime.Now;
            if (!string.IsNullOrEmpty(filter.YearMonth))
            {
                var yearMonth = int.Parse(filter.YearMonth.ToEnglishNumbers());
                var ssDate = Utility.GetPersianMonth(filter.YearMonth.ToEnglishNumbers());

                var sDate = $"{yearMonth}01";
                var eDate = $"{yearMonth}{ssDate.LastDayNumber}";
                startDate = DateTimeExtensions.ToGorgianDate(sDate).Value;
                endDate = DateTimeExtensions.ToGorgianDate(eDate).Value;
            }

            var query = _kscHrUnitOfWork.EmployeeEntryExitRepository.GetEmployeeEntryExitValidByEmployeeIdsRangeDateForReportWithEmployeeeId(filter.EmployeeId, startDate, endDate)
                .Select(a => new ReportEmployeeAttendAbsenceItemModel()
                {
                    PersonalNumber = a.PersonalNumber,
                    EntryExitDate = a.EntryExitDate,
                    EntryExitTime = a.EntryExitTime,
                    FullName = a.Employee.Name + " " + a.Employee.Family,
                    EntryExitType = a.EntryExitType,
                    EntryExitTypeString = a.EntryExitType == 1 ? "ورود" : "خروج",
                    IsCreatedManualNum = (a.IsCreatedManual == true) ? "0" : "1",
                    IsCreatedManualTitle = (a.IsCreatedManual == true) ? "دستی" : "سیستمی",
                    IsDeleted = a.IsDeleted,
                    IsDeletedTitle = (a.IsDeleted == true) ? "حذف شده" : "فعال",
                    IsDeletedNum = (a.IsDeleted == true) ? "0" : "1",
                    CreateUser = a.IsDeleted ? a.DeletedUser : a.CreateUser,
                    CreateDateTime = a.IsDeleted ? a.DeletedDate : a.UpdateDate.HasValue ? a.UpdateDate : a.CreateDateTime,
                }).OrderBy(x => x.EntryExitDate).ThenBy(x => x.EntryExitTime).ToList();
            //var result = _FilterHandler.GetFilterResult<ReportEmployeeAttendAbsenceItemModel>(query, Filter, "WorkCalendarId");

            var finalresult = _FilterHandler.GetFilterResult<ReportEmployeeAttendAbsenceItemModel>(query, filter, "");
            var result = _mapper.Map<List<ReportEmployeeAttendAbsenceItemModel>>(finalresult.Data.ToList()); ;

            var modelResult = new FilterResult<ReportEmployeeAttendAbsenceItemModel>
            {
                Data = result,
                Total = finalresult.Total
            };
            return modelResult;
        }



        /// <summary>
        /// ثبت تایید کارکرد
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private KscResult AddEmplyeeAttendAbsenseItems(AddEmployeeAttendAbsenceItemModel model)
        {
            var result = IsCanAdd(model);
            if (result.Success == false)
            {
                return result;
            }
            var addEmployeeAttendAbsenceItem = new EmployeeAttendAbsenceItem()
            {
                EmployeeId = model.EmployeeId,
                WorkTimeId = model.WorkTimeId,
                WorkCalendarId = model.WorkCalendarId,
                RollCallDefinitionId = model.RollCallDefinitionId,
                ShiftConceptDetailId = model.ShiftConceptDetailId,
                ShiftConceptDetailIdInShiftBoard = model.OldShiftConceptDetailId == 0 ? model.ShiftConceptDetailId : model.OldShiftConceptDetailId,
                IsManual = true,
                InvalidRecord = model.IsDeleted,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                //EndDate=model.EndDate,
                //StartDate=model.StartDate,
                TimeDuration = model.TimeDuration,
                InsertDate = DateTime.Now,
                InsertUser = model.CurrentUserName,
                OverTimeToken = model.OverTimeToken,
                RemoteIpAddress = model.RemoteIpAddress,
                AuthenticateUserName = model.AuthenticateUserName
            };
            if (model.EntryId > 0)
            {

                addEmployeeAttendAbsenceItem.EmployeeEntryExitAttendAbsences.Add(new EmployeeEntryExitAttendAbsence()
                {
                    EmployeeEntryExitId = model.EntryId,
                });

            }
            if (model.ExitId > 0)
            {

                addEmployeeAttendAbsenceItem.EmployeeEntryExitAttendAbsences.Add(new EmployeeEntryExitAttendAbsence()
                {
                    EmployeeEntryExitId = model.ExitId,
                });

            }
            _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.Add(addEmployeeAttendAbsenceItem);
            return result;
        }

        /// <summary>
        /// حذف تایید کارکرد
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public KscResult RemoveAttendAbcenceItem(List<AddEmployeeAttendAbsenceItemModel> models)
        {
            var result = new KscResult();
            if (!models.Any())
            {
                result.AddError("", "اطلاعاتی برای انصراف از تایید کارکرد وجود ندارد");
                return result;
            }
            var model = models.Select(a => new { a.WorkCalendarId, a.CurrentUserName, a.EmployeeId }).First();
            var EmployeeAttendAbsenceItem = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetAllQuaryble(new List<long>())
                .Where(a => a.EmployeeId == model.EmployeeId && a.WorkCalendarId == model.WorkCalendarId)
                .Include(a => a.EmployeeEntryExitAttendAbsences).Include(x => x.RollCallDefinition).ToList();
            if (EmployeeAttendAbsenceItem.Any(x => x.RollCallDefinition.IsValidForDeleteAbsenceItem == false))
            {
                result.AddError("", "در ردیفهای کارکرد آیتم غیر قابل حذف وجود دارد");
                return result;
            }
            foreach (var item in EmployeeAttendAbsenceItem)
            {

                var employeeEntryExist = item.EmployeeEntryExitAttendAbsences.ToList();
                foreach (var employeeEntryExitAttendAbsence in employeeEntryExist)
                {
                    _kscHrUnitOfWork.EmployeeEntryExitAttendAbsenceRepository.Delete(employeeEntryExitAttendAbsence);
                }
                _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.Delete(item);
            }
            SumForcedOverTimeAttendAbsence(false, models);
            _kscHrUnitOfWork.Save();
            return result;
        }


        #endregion
        /// <summary>
        ///  محاسبه اضافه کاری پنج شنبه تعطیل رسمی   
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="HRBusinessException"></exception>
        public async Task<KscResult> OverTimeSpecialHolidayTimeSheet(SearchMonthTimeSheetModel model)
        {
            var result = new KscResult();
            int resultCount = 0;
            try
            {
                var YearMonth = int.Parse(model.DateTimeSheet);
                var systemStatus = await _kscHrUnitOfWork.WorkCalendarRepository.GetMonthTimeSheetStatus(YearMonth);
                if (systemStatus == true)//کاربران اداری سیستم بسته شده باشد
                {
                    if (_kscHrUnitOfWork.MonthTimeSheetRepository.Any(x => x.YearMonth == YearMonth && x.IsCreatedManual == false))
                    {
                        throw new HRBusinessException(Validations.RepetitiveId, "تایم شیت ماه انتخابی ایجاد شده است");
                    }
                    int beforeMonth = YearMonth - 1;
                    if (YearMonth.ToString().Substring(5, 1) == "1")
                    {
                        int year = int.Parse(YearMonth.ToString().Substring(0, 4)) - 1;
                        beforeMonth = int.Parse(year + "12");
                    }
                    //var YearMonthDayShamsi_Prev = _kscHrUnitOfWork.WorkCalendarRepository.GetPrevMonth(YearMonth);
                    //var beforeMonth  = int.Parse(YearMonthDayShamsi_Prev.Substring(0, 7).Replace("/", ""));

                    var monthWorkCalendar = _kscHrUnitOfWork.WorkCalendarRepository.GetRangeMonthWorkCalendar(beforeMonth, YearMonth);
                    var monthWorkCalendarByYearMonth = _kscHrUnitOfWork.WorkCalendarRepository.GetMonthWorkCalendar(YearMonth);
                    var rollCallDefinitionAttend = _kscHrUnitOfWork.RollCallDefinitionRepository.GetRollCallDefinitionByRollCallConceptIdAsNoTracking(EnumRollCallConcept.Attend.Id).Select(x => x.Id).ToList();
                    var timeSheetSettingActive = await _kscHrUnitOfWork.TimeSheetSettingRepository.GetTimeSheetSettingActiveAsync();
                    var shiftConceptDetailValid = _kscHrUnitOfWork.ShiftConceptDetailRepository.WhereQueryable(x => x.IsValidSpecialOverTimeTimeSheet).Select(x => x.Id).ToList();
                    var officialHoliday = monthWorkCalendarByYearMonth.Where(x => x.DayNumber == timeSheetSettingActive.OverTimeSpecialDayNumber && x.IsOfficialHoliday);
                    if (officialHoliday.Any())
                    {
                        List<EmployeeAttendAbsenceItem> resultEmployeeAttendAbsenceItem = new List<EmployeeAttendAbsenceItem>();
                        var includedDefinition = _kscHrUnitOfWork.IncludedRollCallRepository.GetActiveIncludedRollCallByIncludedIdAsNoTracking(EnumIncludedDefinition.ThursdayOfficalHoliadyIncludedOverTime.Id);
                        var overTimeSpecialHolidayTimeSheetSetting = _kscHrUnitOfWork.OverTimeSpecialHolidayTimeSheetSettingRepository.GetAllQueryable().ToList();
                        var employeeAttendAbsenceItem = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetValidItems();
                        var attendAbsenceItemInHoliday = employeeAttendAbsenceItem.Where(x => officialHoliday.Any(y => y.WorkCalendarId == x.WorkCalendarId)).ToList();
                        //
                        var thursdayOfficalHoliadyOverTimeRollCall = EnumRollCallDefinication.ThursdayOfficalHoliadyOverTime.Id;
                        //
                        foreach (var day in officialHoliday)
                        {
                            var endDay = day.Date.AddDays(-1);
                            var startDay = day.Date.AddDays(-5);
                            var WorkCalendarIds = monthWorkCalendar.Where(x => x.Date >= startDay && x.Date <= endDay).Select(x => x.WorkCalendarId).ToList();
                            var attendAbsenceItem = employeeAttendAbsenceItem.Where(x => WorkCalendarIds.Any(y => y == x.WorkCalendarId)
                            && shiftConceptDetailValid.Any(s => s == x.ShiftConceptDetailId));
                            var attendAbsenceItemResult = (from item in attendAbsenceItem
                                                           join includedRollCall in includedDefinition
                                                          on item.RollCallDefinitionId equals includedRollCall.RollCallDefinitionId
                                                           group item by new
                                                           {
                                                               EmployeeId = item.EmployeeId,
                                                           }
                                                    into newgroup
                                                           select new EmployeeItemGroupModel()
                                                           {
                                                               EmployeeId = newgroup.Key.EmployeeId,
                                                               DurationInDay = newgroup.Select(x => x.WorkCalendarId).Distinct().Count()

                                                           });
                            foreach (var item in attendAbsenceItemResult)
                            {
                                var overTimeSpecialHoliday = overTimeSpecialHolidayTimeSheetSetting.FirstOrDefault(x => x.DayCount == item.DurationInDay);
                                if (overTimeSpecialHoliday != null)
                                {
                                    // ;
                                    var employeeAttendAbsenceItemInHoliday = attendAbsenceItemInHoliday.Where(x => x.WorkCalendarId == day.WorkCalendarId && x.EmployeeId == item.EmployeeId);

                                    if (employeeAttendAbsenceItemInHoliday.Count() != 0 && employeeAttendAbsenceItemInHoliday.Any(x => x.RollCallDefinitionId == thursdayOfficalHoliadyOverTimeRollCall) == false) //قبلا ثبت نشده باشد
                                    {
                                        var dataMap = employeeAttendAbsenceItemInHoliday.FirstOrDefault();
                                        if (dataMap != null)
                                        {
                                            EmployeeAttendAbsenceItem newEmployeeAttendAbsenceItem = new EmployeeAttendAbsenceItem()
                                            {
                                                WorkCalendarId = dataMap.WorkCalendarId,
                                                ShiftConceptDetailId = dataMap.ShiftConceptDetailId,
                                                ShiftConceptDetailIdInShiftBoard = dataMap.ShiftConceptDetailIdInShiftBoard,
                                                EmployeeId = dataMap.EmployeeId,
                                                StartTime = "07:00",
                                                EndTime = "16:00",
                                                TimeDuration = overTimeSpecialHoliday.Duration,
                                                RollCallDefinitionId = thursdayOfficalHoliadyOverTimeRollCall,
                                                InsertDate = DateTime.Now,
                                                InsertUser = model.CurrentUser,
                                                WorkTimeId = dataMap.WorkTimeId,
                                                InvalidRecord = false,
                                                InvalidRecordReason = "",
                                                IsFloat = false,
                                                IsManual = false,
                                            };
                                            resultEmployeeAttendAbsenceItem.Add(newEmployeeAttendAbsenceItem);
                                        }
                                    }

                                }
                            }
                        }
                        //
                        resultCount += resultEmployeeAttendAbsenceItem.Count();
                        await _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.AddRangeAsync(resultEmployeeAttendAbsenceItem);
                        //
                    }
                    //
                    _kscHrUnitOfWork.MonthTimeSheetLogRepository.Add(new MonthTimeSheetLog()
                    {
                        InsertDate = DateTime.Now,
                        YearMonth = YearMonth,
                        InsertUser = model.CurrentUser,
                        Result = "محاسبه اضافه کاری پنجشنبه تعطیل کاری با موفقیت انجام شد",
                        MonthTimeShitStepperId = model.Step,
                        ResultCount = resultCount,

                    });
                    //
                    await _kscHrUnitOfWork.SaveAsync();

                }
                else
                {
                    throw new HRBusinessException(Validations.CloseDailyTimeSgeetId, Validations.CloseDailyTimeSgeet);
                }
            }
            catch (Exception ex)
            {

                throw new HRBusinessException(Validations.RepetitiveId, ex.Message);
            }


            return result;
        }
        /// <summary>
        ///  محاسبه اضافه کاری پنج شنبه تعطیل رسمی   
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="HRBusinessException"></exception>
        public async Task<KscResult> OverTimeSpecialHolidayTimeSheetStep(UpdateStatusByYearMonthProcedureModel model)
        {
            var result = new KscResult();
            int resultCount = 0;
            try
            {
                var YearMonth = int.Parse(model.Yearmonth);
                var systemStatus = await _kscHrUnitOfWork.WorkCalendarRepository.GetMonthTimeSheetStatus(YearMonth);
                if (systemStatus == true)//کاربران اداری سیستم بسته شده باشد
                {
                    if (_kscHrUnitOfWork.MonthTimeSheetRepository.Any(x => x.YearMonth == YearMonth && x.IsCreatedManual == false))
                    {
                        throw new HRBusinessException(Validations.RepetitiveId, "تایم شیت ماه انتخابی ایجاد شده است");
                    }
                    int beforeMonth = YearMonth - 1;
                    if (YearMonth.ToString().Substring(5, 1) == "1")
                    {
                        int year = int.Parse(YearMonth.ToString().Substring(0, 4)) - 1;
                        beforeMonth = int.Parse(year + "12");
                    }
                    //var YearMonthDayShamsi_Prev = _kscHrUnitOfWork.WorkCalendarRepository.GetPrevMonth(YearMonth);
                    //var beforeMonth  = int.Parse(YearMonthDayShamsi_Prev.Substring(0, 7).Replace("/", ""));

                    var monthWorkCalendar = _kscHrUnitOfWork.WorkCalendarRepository.GetRangeMonthWorkCalendar(beforeMonth, YearMonth);
                    var monthWorkCalendarByYearMonth = _kscHrUnitOfWork.WorkCalendarRepository.GetMonthWorkCalendar(YearMonth);
                    var rollCallDefinitionAttend = _kscHrUnitOfWork.RollCallDefinitionRepository.GetRollCallDefinitionByRollCallConceptIdAsNoTracking(EnumRollCallConcept.Attend.Id).Select(x => x.Id).ToList();
                    var timeSheetSettingActive = await _kscHrUnitOfWork.TimeSheetSettingRepository.GetTimeSheetSettingActiveAsync();
                    var shiftConceptDetailValid = _kscHrUnitOfWork.ShiftConceptDetailRepository.WhereQueryable(x => x.IsValidSpecialOverTimeTimeSheet).Select(x => x.Id).ToList();
                    var officialHoliday = monthWorkCalendarByYearMonth.Where(x => x.DayNumber == timeSheetSettingActive.OverTimeSpecialDayNumber && x.IsOfficialHoliday);
                    if (officialHoliday.Any())
                    {
                        List<EmployeeAttendAbsenceItem> resultEmployeeAttendAbsenceItem = new List<EmployeeAttendAbsenceItem>();
                        var includedDefinition = _kscHrUnitOfWork.IncludedRollCallRepository.GetActiveIncludedRollCallByIncludedIdAsNoTracking(EnumIncludedDefinition.ThursdayOfficalHoliadyIncludedOverTime.Id);
                        var overTimeSpecialHolidayTimeSheetSetting = _kscHrUnitOfWork.OverTimeSpecialHolidayTimeSheetSettingRepository.GetAllQueryable().ToList();
                        var employeeAttendAbsenceItem = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetValidItems();
                        var attendAbsenceItemInHoliday = employeeAttendAbsenceItem.Where(x => officialHoliday.Any(y => y.WorkCalendarId == x.WorkCalendarId)).ToList();
                        //
                        var thursdayOfficalHoliadyOverTimeRollCall = EnumRollCallDefinication.ThursdayOfficalHoliadyOverTime.Id;
                        //
                        foreach (var day in officialHoliday)
                        {
                            var endDay = day.Date.AddDays(-1);
                            var startDay = day.Date.AddDays(-5);
                            var WorkCalendarIds = monthWorkCalendar.Where(x => x.Date >= startDay && x.Date <= endDay).Select(x => x.WorkCalendarId).ToList();
                            var attendAbsenceItem = employeeAttendAbsenceItem.Where(x => WorkCalendarIds.Any(y => y == x.WorkCalendarId)
                            && shiftConceptDetailValid.Any(s => s == x.ShiftConceptDetailId));
                            var attendAbsenceItemResult = (from item in attendAbsenceItem
                                                           join includedRollCall in includedDefinition
                                                          on item.RollCallDefinitionId equals includedRollCall.RollCallDefinitionId
                                                           group item by new
                                                           {
                                                               EmployeeId = item.EmployeeId,
                                                           }
                                                    into newgroup
                                                           select new EmployeeItemGroupModel()
                                                           {
                                                               EmployeeId = newgroup.Key.EmployeeId,
                                                               DurationInDay = newgroup.Select(x => x.WorkCalendarId).Distinct().Count()

                                                           });
                            foreach (var item in attendAbsenceItemResult)
                            {
                                var overTimeSpecialHoliday = overTimeSpecialHolidayTimeSheetSetting.FirstOrDefault(x => x.DayCount == item.DurationInDay);
                                if (overTimeSpecialHoliday != null)
                                {
                                    // ;
                                    var employeeAttendAbsenceItemInHoliday = attendAbsenceItemInHoliday.Where(x => x.WorkCalendarId == day.WorkCalendarId && x.EmployeeId == item.EmployeeId);

                                    if (employeeAttendAbsenceItemInHoliday.Count() != 0 && employeeAttendAbsenceItemInHoliday.Any(x => x.RollCallDefinitionId == thursdayOfficalHoliadyOverTimeRollCall) == false) //قبلا ثبت نشده باشد
                                    {
                                        var dataMap = employeeAttendAbsenceItemInHoliday.FirstOrDefault();
                                        if (dataMap != null)
                                        {
                                            EmployeeAttendAbsenceItem newEmployeeAttendAbsenceItem = new EmployeeAttendAbsenceItem()
                                            {
                                                WorkCalendarId = dataMap.WorkCalendarId,
                                                ShiftConceptDetailId = dataMap.ShiftConceptDetailId,
                                                ShiftConceptDetailIdInShiftBoard = dataMap.ShiftConceptDetailIdInShiftBoard,
                                                EmployeeId = dataMap.EmployeeId,
                                                StartTime = "07:00",
                                                EndTime = "16:00",
                                                TimeDuration = overTimeSpecialHoliday.Duration,
                                                RollCallDefinitionId = thursdayOfficalHoliadyOverTimeRollCall,
                                                InsertDate = DateTime.Now,
                                                InsertUser = model.CurrentUser,
                                                WorkTimeId = dataMap.WorkTimeId,
                                                InvalidRecord = false,
                                                InvalidRecordReason = "",
                                                IsFloat = false,
                                                IsManual = false,
                                            };
                                            resultEmployeeAttendAbsenceItem.Add(newEmployeeAttendAbsenceItem);
                                        }
                                    }

                                }
                            }
                        }
                        //
                        resultCount += resultEmployeeAttendAbsenceItem.Count();
                        await _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.AddRangeAsync(resultEmployeeAttendAbsenceItem);
                        //
                    }
                    //
                    model.Result = "محاسبه اضافه کاری پنجشنبه تعطیل کاری با موفقیت انجام شد";
                    model.ResultCount = resultCount;
                    result = await _procedureService.InsertStepProcedure(model);
                    if (result.Success)
                        await _kscHrUnitOfWork.SaveAsync();

                }
                else
                {
                    throw new HRBusinessException(Validations.CloseDailyTimeSgeetId, Validations.CloseDailyTimeSgeet);
                }
            }
            catch (Exception ex)
            {

                throw new HRBusinessException(Validations.RepetitiveId, ex.Message);
            }


            return result;
        }

        /// <summary>
        ///  محاسبه اضافه کاری مازاد بر سقف   
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="HRBusinessException"></exception>
        public async Task<KscResult> CeilingOvertimeTimeSheet(SearchMonthTimeSheetModel model)
        {
            var result = new KscResult();
            try
            {
                var YearMonth = int.Parse(model.DateTimeSheet);
                var systemStatus = await _kscHrUnitOfWork.WorkCalendarRepository.GetMonthTimeSheetStatus(YearMonth);
                if (systemStatus == true)//کاربران اداری سیستم بسته شده باشد
                {
                    if (_kscHrUnitOfWork.MonthTimeSheetRepository.Any(x => x.YearMonth == YearMonth && x.IsCreatedManual == false))
                    {
                        throw new HRBusinessException(Validations.RepetitiveId, "تایم شیت ماه انتخابی ایجاد شده است");
                    }

                    //MonthTimeSheetRollCall
                    var attendAbcenseItem = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetMonthTimeSheetCalculateAsNoTracking(YearMonth);
                    var rollCallDefinitionByCeilingOvertime = _kscHrUnitOfWork.IncludedRollCallRepository.GetActiveIncludedRollCallByIncludedIdAsNoTracking(EnumIncludedDefinition.MaximunOverTime.Id)
                        .Select(x => x.RollCallDefinitionId);
                    var overTimePriority = _kscHrUnitOfWork.RollCallDefinitionRepository.WhereQueryable(x => rollCallDefinitionByCeilingOvertime.Any(r => r == x.Id) && x.OverTimePriority == null);
                    if (overTimePriority.Any())
                    {
                        var overTimePriorityIsNull = string.Join(",", overTimePriority.Select(x => x.Id).ToList());
                        throw new HRBusinessException(Validations.RepetitiveId, $"برای کدهای {overTimePriorityIsNull} ترتیب کسر مازاد اضافه کاری مشخص نشده است");

                    }
                    var attendAbcenseItemData = attendAbcenseItem.Where(x => x.InvalidRecord == false).Select(x => new EmployeeItemGroupModel()
                    {
                        EmployeeId = x.EmployeeId,
                        RollCallDefinitionId = x.RollCallDefinitionId,
                        TotalDuration = x.TimeDurationInMinute ?? 0

                    });

                    int forcedOverTime = EnumRollCallDefinication.ForcedOverTime.Id;
                    var forcedOverTimeData = _kscHrUnitOfWork.MonthTimeSheetDraftRepository.GetMonthTimeSheetDraftByYearMonth(YearMonth).Select(x => new EmployeeItemGroupModel()
                    {
                        EmployeeId = x.EmployeeId,
                        RollCallDefinitionId = forcedOverTime,
                        TotalDuration = x.ForcedOverTime

                    });

                    attendAbcenseItemData = attendAbcenseItemData.Concat(forcedOverTimeData);
                    //
                    var query_MonthTimeSheetRollCall = from item in attendAbcenseItemData
                                                       join rollCall in rollCallDefinitionByCeilingOvertime on item.RollCallDefinitionId equals rollCall
                                                       group item by new
                                                       {

                                                           EmployeeId = item.EmployeeId
                                                       } into newgroup
                                                       select new EmployeeItemGroupModel()
                                                       {
                                                           EmployeeId = newgroup.Key.EmployeeId,
                                                           TotalDuration = newgroup.Sum(x => x.TotalDuration)

                                                       };

                    var employee = _kscHrUnitOfWork.EmployeeRepository.GetAllQueryable();
                    var overTimeDefinition = _kscHrUnitOfWork.OverTimeDefinitionRepository.GetAllQueryable();
                    var teamWork = _kscHrUnitOfWork.TeamWorkRepository.GetAllQueryable();
                    var resultData = (from item in query_MonthTimeSheetRollCall
                                      join empl in employee on item.EmployeeId equals empl.Id
                                      join team in teamWork on empl.TeamWorkId equals team.Id
                                      join overTime in overTimeDefinition on team.OverTimeDefinitionId equals overTime.Id
                                      select new EmployeeTimeSheetMonthModel()
                                      {
                                          EmployeeId = item.EmployeeId,
                                          YearMonth = YearMonth,
                                          CeilingOvertime = item.TotalDuration,
                                          ExcessOverTime = item.TotalDuration > overTime.MaximumDurationMinute ? item.TotalDuration - overTime.MaximumDurationMinute : 0
                                      }).ToList();
                    //
                    List<EmployeeTimeSheet> employeeTimeSheetList = new List<EmployeeTimeSheet>();
                    var employeeTimeSheetByYearMonth = _kscHrUnitOfWork.EmployeeTimeSheetRepository.GetEmployeeTimeSheet(YearMonth).ToList();
                    //

                    foreach (var item in resultData)
                    {
                        var employeeTimeSheet = employeeTimeSheetByYearMonth.SingleOrDefault(x => x.EmployeeId == item.EmployeeId);
                        if (employeeTimeSheet != null)
                        {
                            var updateEmployeeTimeSheet = _kscHrUnitOfWork.EmployeeTimeSheetRepository.GetById(employeeTimeSheet.Id);
                            updateEmployeeTimeSheet.CeilingOvertime = (int)item.CeilingOvertime;
                            updateEmployeeTimeSheet.ExcessOverTime = (int)item.ExcessOverTime;
                            updateEmployeeTimeSheet.ExcessOverTimeDuration = Utility.ConvertMinuteToDuration((int)item.ExcessOverTime);
                            updateEmployeeTimeSheet.UpdateDate = DateTime.Now;
                            updateEmployeeTimeSheet.UpdateUser = model.CurrentUser;
                        }
                        else
                        {

                            var newRow = _mapper.Map<EmployeeTimeSheet>(item);
                            newRow.InsertDate = DateTime.Now;
                            newRow.InsertUser = model.CurrentUser;
                            employeeTimeSheetList.Add(newRow);
                        }

                    }
                    if (employeeTimeSheetList.Count() != 0)
                    {
                        await _kscHrUnitOfWork.EmployeeTimeSheetRepository.AddRangeAsync(employeeTimeSheetList);
                    }
                    //
                    _kscHrUnitOfWork.MonthTimeSheetLogRepository.Add(new MonthTimeSheetLog()
                    {
                        InsertDate = DateTime.Now,
                        YearMonth = YearMonth,
                        InsertUser = model.CurrentUser,
                        Result = "محاسبه اضافه کاری مازاد بر سقف با موفقیت انجام شد",
                        MonthTimeShitStepperId = model.Step,
                        ResultCount = employeeTimeSheetList.Count(),

                    });
                    await _kscHrUnitOfWork.SaveAsync();
                }
                else
                {
                    throw new HRBusinessException(Validations.CloseDailyTimeSgeetId, Validations.CloseDailyTimeSgeet);
                }
            }
            catch (Exception ex)
            {

                throw new HRBusinessException(Validations.RepetitiveId, ex.Message);
            }


            return result;
        }
        /// <summary>
        ///  محاسبه اضافه کاری مازاد بر سقف   
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="HRBusinessException"></exception>
        public async Task<KscResult> CeilingOvertimeTimeSheetStep(UpdateStatusByYearMonthProcedureModel model)
        {
            var result = new KscResult();
            try
            {
                var YearMonth = int.Parse(model.Yearmonth);
                var systemStatus = await _kscHrUnitOfWork.WorkCalendarRepository.GetMonthTimeSheetStatus(YearMonth);
                if (systemStatus == true)//کاربران اداری سیستم بسته شده باشد
                {
                    if (_kscHrUnitOfWork.MonthTimeSheetRepository.Any(x => x.YearMonth == YearMonth && x.IsCreatedManual == false))
                    {
                        throw new HRBusinessException(Validations.RepetitiveId, "تایم شیت ماه انتخابی ایجاد شده است");
                    }

                    //MonthTimeSheetRollCall
                    var attendAbcenseItem = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetMonthTimeSheetCalculateAsNoTracking(YearMonth);
                    var rollCallDefinitionByCeilingOvertime = _kscHrUnitOfWork.IncludedRollCallRepository.GetActiveIncludedRollCallByIncludedIdAsNoTracking(EnumIncludedDefinition.MaximunOverTime.Id)
                        .Select(x => x.RollCallDefinitionId);
                    var overTimePriority = _kscHrUnitOfWork.RollCallDefinitionRepository.WhereQueryable(x => rollCallDefinitionByCeilingOvertime.Any(r => r == x.Id) && x.OverTimePriority == null);
                    if (overTimePriority.Any())
                    {
                        var overTimePriorityIsNull = string.Join(",", overTimePriority.Select(x => x.Id).ToList());
                        throw new HRBusinessException(Validations.RepetitiveId, $"برای کدهای {overTimePriorityIsNull} ترتیب کسر مازاد اضافه کاری مشخص نشده است");

                    }
                    var attendAbcenseItemData = attendAbcenseItem.Where(x => x.InvalidRecord == false).Select(x => new EmployeeItemGroupModel()
                    {
                        EmployeeId = x.EmployeeId,
                        RollCallDefinitionId = x.RollCallDefinitionId,
                        TotalDuration = x.TimeDurationInMinute ?? 0 + x.IncreasedTimeDuration ?? 0,
                        IncreasedTimeDuration = x.IncreasedTimeDuration ?? 0

                    });

                    int forcedOverTime = EnumRollCallDefinication.ForcedOverTime.Id;
                    var forcedOverTimeData = _kscHrUnitOfWork.MonthTimeSheetDraftRepository.GetMonthTimeSheetDraftByYearMonth(YearMonth).Select(x => new EmployeeItemGroupModel()
                    {
                        EmployeeId = x.EmployeeId,
                        RollCallDefinitionId = forcedOverTime,
                        TotalDuration = x.ForcedOverTime,
                        IncreasedTimeDuration = 0

                    });

                    attendAbcenseItemData = attendAbcenseItemData.Concat(forcedOverTimeData);
                    //
                    var query_MonthTimeSheetRollCall = from item in attendAbcenseItemData
                                                       join rollCall in rollCallDefinitionByCeilingOvertime on item.RollCallDefinitionId equals rollCall
                                                       group item by new
                                                       {

                                                           EmployeeId = item.EmployeeId
                                                       } into newgroup
                                                       select new EmployeeItemGroupModel()
                                                       {
                                                           EmployeeId = newgroup.Key.EmployeeId,
                                                           TotalDuration = newgroup.Sum(x => x.TotalDuration + (double)x.IncreasedTimeDuration)

                                                       };

                    var employee = _kscHrUnitOfWork.EmployeeRepository.GetAllQueryable();
                    var overTimeDefinition = _kscHrUnitOfWork.OverTimeDefinitionRepository.GetAllQueryable();
                    var teamWork = _kscHrUnitOfWork.TeamWorkRepository.GetAllQueryable();
                    var resultData = (from item in query_MonthTimeSheetRollCall
                                      join empl in employee on item.EmployeeId equals empl.Id
                                      join team in teamWork on empl.TeamWorkId equals team.Id
                                      join overTime in overTimeDefinition on team.OverTimeDefinitionId equals overTime.Id
                                      select new EmployeeTimeSheetMonthModel()
                                      {
                                          EmployeeId = item.EmployeeId,
                                          YearMonth = YearMonth,
                                          CeilingOvertime = item.TotalDuration,
                                          ExcessOverTime = item.TotalDuration > overTime.MaximumDurationMinute ? item.TotalDuration - overTime.MaximumDurationMinute : 0
                                      }).ToList();
                    //
                    List<EmployeeTimeSheet> employeeTimeSheetList = new List<EmployeeTimeSheet>();
                    var employeeTimeSheetByYearMonth = _kscHrUnitOfWork.EmployeeTimeSheetRepository.GetEmployeeTimeSheet(YearMonth).ToList();
                    //

                    foreach (var item in resultData)
                    {
                        var employeeTimeSheet = employeeTimeSheetByYearMonth.SingleOrDefault(x => x.EmployeeId == item.EmployeeId);
                        if (employeeTimeSheet != null)
                        {
                            var updateEmployeeTimeSheet = _kscHrUnitOfWork.EmployeeTimeSheetRepository.GetById(employeeTimeSheet.Id);
                            updateEmployeeTimeSheet.CeilingOvertime = (int)item.CeilingOvertime;
                            updateEmployeeTimeSheet.ExcessOverTime = (int)item.ExcessOverTime;
                            updateEmployeeTimeSheet.ExcessOverTimeDuration = Utility.ConvertMinuteToDuration((int)item.ExcessOverTime);
                            updateEmployeeTimeSheet.UpdateDate = DateTime.Now;
                            updateEmployeeTimeSheet.UpdateUser = model.CurrentUser;
                        }
                        else
                        {

                            var newRow = _mapper.Map<EmployeeTimeSheet>(item);
                            newRow.InsertDate = DateTime.Now;
                            newRow.InsertUser = model.CurrentUser;
                            employeeTimeSheetList.Add(newRow);
                        }

                    }
                    if (employeeTimeSheetList.Count() != 0)
                    {
                        await _kscHrUnitOfWork.EmployeeTimeSheetRepository.AddRangeAsync(employeeTimeSheetList);
                    }
                    //
                    model.Result = "محاسبه اضافه کاری مازاد بر سقف با موفقیت انجام شد";
                    model.ResultCount = employeeTimeSheetList.Count();
                    result = await _procedureService.InsertStepProcedure(model);

                    //_kscHrUnitOfWork.MonthTimeSheetLogRepository.Add(new MonthTimeSheetLog()
                    //{
                    //    InsertDate = DateTime.Now,
                    //    YearMonth = YearMonth,
                    //    InsertUser = model.CurrentUser,
                    //    Result = "محاسبه اضافه کاری مازاد بر سقف با موفقیت انجام شد",
                    //    MonthTimeShitStepperId = model.Step,
                    //    ResultCount = employeeTimeSheetList.Count(),

                    //});
                    if (result.Success)
                        await _kscHrUnitOfWork.SaveAsync();
                }
                else
                {
                    throw new HRBusinessException(Validations.CloseDailyTimeSgeetId, Validations.CloseDailyTimeSgeet);
                }
            }
            catch (Exception ex)
            {

                throw new HRBusinessException(Validations.RepetitiveId, ex.Message);
            }


            return result;
        }

        public async Task<FilterResult<EmployeeDontHaveAttendITemModel>> GetEmployeeDontHaveAttendItemModelReport(SearchReportEmployeeEntryExitModel filter)
        {
            List<EmployeeDontHaveAttendITemModel> allPersonnelNotInAttendItemPrc = new List<EmployeeDontHaveAttendITemModel>();
            var result = new FilterResult<EmployeeDontHaveAttendITemModel>();
            try
            {
                var yearMonth = int.Parse(filter.YearMonth.ToEnglishNumbers());
                var workCalendar = _kscHrUnitOfWork.WorkCalendarRepository.GetWorkCalendarByYearMonthAsNoTracking(yearMonth).Select(x => new { WorkCalendarId = x.Id, ShamsiDate = x.ShamsiDateV1, MiladiDate = x.MiladiDateV1 });
                var employeeData = _kscHrUnitOfWork.EmployeeRepository.GetEmployeesActivAndeHaveTeamAsNotracking();
                var employeeActive = employeeData;
                var employeeSecurity = _kscHrUnitOfWork.ViewMisEmployeeSecurityRepository.GetAllQueryable();
                if (filter.IsOfficialAttend != true)
                {
                    employeeData = from empl in employeeData
                                       //join manager in employeeSecurity on empl.TeamWorkId equals manager.TeamWorkId
                                   select empl;
                }
                var emplyeeModel = employeeData.Include(x => x.TeamWork).Select(x => new EmployeeDontHaveAttendITemModel()
                {
                    EmployeeId = x.Id,
                    PersonalNumber = x.EmployeeNumber,
                    FullName = x.Name + " " + x.Family,
                    TeamWorkId = x.TeamWorkId.Value,
                    TeamCode = x.TeamWork.Code,
                    TeamName = x.TeamWork.Title

                }).ToList();
                var employeeAttendAbsenceItem = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetEmployeeAttendAbsenceItemByYearMonthAsNoTracking(yearMonth).GroupBy(x => new { EmployeeId = x.EmployeeId, WorkCalendarId = x.WorkCalendarId })
                      .Select(x => new { EmployeeId = x.Key.EmployeeId, WorkCalendarId = x.Key.WorkCalendarId }).ToList();
                var temManager = _kscHrUnitOfWork.ViewTeamManagerRepository.WhereQueryable(x => x.DisplaySecurity == 1)
                    .Select(x => new
                    {
                        TeamWorkId = x.TeamWorkId,
                        NameFamily = x.Name + " " + x.Family
                    }).ToList();
                foreach (var cal in workCalendar)
                {
                    var employeeItem = employeeAttendAbsenceItem.Where(x => x.WorkCalendarId == cal.WorkCalendarId);
                    var employeeNotItem = emplyeeModel.Where(x => employeeItem.Any(y => y.EmployeeId == x.EmployeeId) == false).Select(
                        x => new EmployeeDontHaveAttendITemModel()
                        {
                            EmployeeId = x.EmployeeId,
                            PersonalNumber = x.PersonalNumber,
                            FullName = x.FullName,
                            TeamWorkId = x.TeamWorkId,
                            TeamCode = x.TeamCode,
                            TeamName = x.TeamName,
                            Date = cal.ShamsiDate,
                            ManagerTeam = temManager.Any(x => x.TeamWorkId == x.TeamWorkId) ? temManager.First(x => x.TeamWorkId == x.TeamWorkId).NameFamily : "",
                            WorkCalendarDate = cal.MiladiDate,
                            WorkCalendarId = cal.WorkCalendarId

                        }

                        ).ToList();
                    allPersonnelNotInAttendItemPrc.AddRange(employeeNotItem);
                }
                var finalList = _FilterHandler.GetFilterResult<EmployeeDontHaveAttendITemModel>(allPersonnelNotInAttendItemPrc, filter, "PersonalNumber");

                result = new FilterResult<EmployeeDontHaveAttendITemModel>()
                {
                    Data = finalList.Data.ToList(),
                    Total = finalList.Total
                };

            }
            catch (Exception ex)
            {

            }

            return result;


        }

        public async Task<bool> UpdateRollCallDefinitionId()
        {
            var items = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.WhereQueryable(x => x.WorkCalendarId == 37297 && x.RollCallDefinitionId == 2).ToList();
            var items1 = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.WhereQueryable(x => x.WorkCalendarId == 37297 && x.RollCallDefinitionId == 6).Select(x => x.EmployeeId).ToList();
            var code6 = items.Where(x => items1.Any(y => y == x.EmployeeId) == false);
            List<EmployeeAttendAbsenceItem> resultEmployeeAttendAbsenceItem = new List<EmployeeAttendAbsenceItem>();
            foreach (var item in code6)
            {
                var dataMap = item;
                if (dataMap != null)
                {
                    EmployeeAttendAbsenceItem newEmployeeAttendAbsenceItem = new EmployeeAttendAbsenceItem()
                    {
                        WorkCalendarId = dataMap.WorkCalendarId,
                        ShiftConceptDetailId = dataMap.ShiftConceptDetailId,
                        ShiftConceptDetailIdInShiftBoard = dataMap.ShiftConceptDetailIdInShiftBoard,
                        EmployeeId = dataMap.EmployeeId,
                        StartTime = dataMap.StartTime,
                        EndTime = dataMap.EndTime,
                        TimeDuration = dataMap.TimeDuration,
                        RollCallDefinitionId = 6,
                        InsertDate = dataMap.InsertDate,
                        InsertUser = dataMap.InsertUser,
                        WorkTimeId = dataMap.WorkTimeId,
                        InvalidRecord = false,
                        InvalidRecordReason = "",
                        IsFloat = false,
                        IsManual = false,
                    };
                    resultEmployeeAttendAbsenceItem.Add(newEmployeeAttendAbsenceItem);
                }
            }
            //await _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.AddRangeAsync(resultEmployeeAttendAbsenceItem);
            //await _kscHrUnitOfWork.SaveAsync();
            return true;
            //var items= _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.WhereQueryable(x => x.WorkCalendarId == 37297 && x.RollCallDefinitionId == 2);
        }
        /// <summary>
        /// محاسبه اضافه کاری قهری
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// استفاده نمیشود
        public async Task<KscResult> CalculateForcedOverTimeByYearMonth(SearchMonthTimeSheetModel model)

        {
            int yearMonth = int.Parse(model.DateTimeSheet);
            var result = new KscResult();
            try
            {
                var systemStatus = await _kscHrUnitOfWork.WorkCalendarRepository.GetMonthTimeSheetStatus(yearMonth);
                if (systemStatus == true)//کاربران اداری سیستم بسته شده باشد
                {
                    if (_kscHrUnitOfWork.MonthTimeSheetRepository.Any(x => x.YearMonth == yearMonth && x.IsCreatedManual == false))
                    {
                        throw new HRBusinessException(Validations.RepetitiveId, "تایم شیت ماه انتخابی ایجاد شده است");
                    }
                    var date1 = DateTime.Now.Minute + ":" + DateTime.Now.Second;
                    var timeShiftSettingForcedOverTimeModel = _kscHrUnitOfWork.TimeShiftSettingRepository.GetAllByIncludedAsNotracking(DateTime.Now)
                                               .Select(x => new ForcedOverTimeModel()
                                               {
                                                   ShiftConceptId = x.WorkCompanySetting.WorkTimeShiftConcept.ShiftConceptId,
                                                   WorkTimeId = x.WorkCompanySetting.WorkTimeShiftConcept.WorkTimeId,
                                                   WorkCityId = x.WorkCompanySetting.WorkCityId,
                                                   ForcedOverTime = x.ForcedOverTime,
                                                   TotalWorkHourInWeek = x.TotalWorkHourInWeek
                                               }).ToList();
                    var monthTimeSheetDraft = _kscHrUnitOfWork.MonthTimeSheetDraftRepository.GetMonthTimeSheetDraftByYearMonth(yearMonth);
                    var monthTimeSheetDraftList = monthTimeSheetDraft.ToList();
                    var rollCallIncludedForcedOverTime = _kscHrUnitOfWork.IncludedRollCallRepository.GetActiveIncludedRollCallByIncludedIdAsNoTracking(EnumIncludedDefinition.ForcedOverTime.Id);
                    //
                    var employeeAttendAbsenceItems = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetEmployeeAttendAbsenceItemAsNoTracking();
                    var workCalendars = _kscHrUnitOfWork.WorkCalendarRepository.GetWorkCalendarByYearMonthAsNoTracking(yearMonth);
                    var employeeAttendAbsenceItemShift = from item in employeeAttendAbsenceItems
                                                         join cal in workCalendars on item.WorkCalendarId equals cal.Id
                                                         join time in _kscHrUnitOfWork.WorkTimeRepository.GetAllQueryable() on item.WorkTimeId equals time.Id
                                                         join shift in _kscHrUnitOfWork.ShiftConceptDetailRepository.GetAllQueryable() on item.ShiftConceptDetailId equals shift.Id
                                                         join empl in _kscHrUnitOfWork.EmployeeRepository.GetAllQueryable() on item.EmployeeId equals empl.Id
                                                         join includedForced in rollCallIncludedForcedOverTime on item.RollCallDefinitionId equals includedForced.RollCallDefinitionId
                                                         where item.InvalidRecord == false
                                                         select new
                                                         {
                                                             item.EmployeeId,
                                                             item.WorkCalendarId,
                                                             item.WorkTimeId,
                                                             shift.ShiftConceptId,
                                                             empl.WorkCityId,
                                                             time.ShiftSettingFromShiftboard,
                                                             item.TimeDurationInMinute,
                                                             time.MaximumForcedOverTime
                                                         };

                    var shiftForcedOverTimeModel = employeeAttendAbsenceItemShift.GroupBy(x => new
                    {
                        x.EmployeeId,
                        x.WorkCityId,
                        x.WorkTimeId,
                        x.ShiftConceptId,
                        x.WorkCalendarId,
                        x.ShiftSettingFromShiftboard,
                        x.MaximumForcedOverTime
                    }).Select(y => new
                    {
                        EmployeeId = y.Key.EmployeeId,
                        WorkCityId = y.Key.WorkCityId,
                        WorkTimeId = y.Key.WorkTimeId,
                        ShiftConceptId = y.Key.ShiftConceptId,
                        ShiftSettingFromShiftboard = y.Key.ShiftSettingFromShiftboard,
                        MaximumForcedOverTime = y.Key.MaximumForcedOverTime,
                        Count = y.Select(x => x.WorkCalendarId).Distinct().Count(),
                        SumTimeDurationInMinutePerDay = y.Sum(x => x.TimeDurationInMinute)
                    }).ToList();

                    var shiftData = shiftForcedOverTimeModel.Join(timeShiftSettingForcedOverTimeModel, a => new { shiftConceptId = a.ShiftConceptId, workTimeId = a.WorkTimeId, workCityId = a.WorkCityId.Value }
                      , b => new { shiftConceptId = b.ShiftConceptId, workTimeId = b.WorkTimeId, workCityId = b.WorkCityId }
                      , (a, b) => new
                      {
                          EmployeeId = a.EmployeeId,
                          WorkTimeId = a.WorkTimeId,
                          ShiftSettingFromShiftboard = a.ShiftSettingFromShiftboard,
                          MaximumForcedOverTime = a.MaximumForcedOverTime != null ? a.MaximumForcedOverTime.ConvertDurationToMinute().Value : 0,
                          ForcedOverTime = b.ForcedOverTime != null ? b.ForcedOverTime.ConvertDurationToMinute().Value * a.Count : 0,
                          SumTimeDurationInMinutePerDay = a.SumTimeDurationInMinutePerDay,
                          TotalWorkHourInWeek = b.TotalWorkHourInWeek != null ? b.TotalWorkHourInWeek.ConvertDurationToMinute().Value : 0,
                          ShiftConceptId = b.ShiftConceptId,
                      }
                      );

                    var shiftResult = shiftData.GroupBy(x => new { x.EmployeeId, x.WorkTimeId }).Select(x => new
                    {
                        EmployeeId = x.Key.EmployeeId,
                        WorkTimeId = x.Key.WorkTimeId,
                        ForcedOverTime = x.Sum(x => x.ForcedOverTime),
                        SumTimeDurationInMinut = x.Sum(x => x.SumTimeDurationInMinutePerDay),
                        TotalWorkHourInWeek = x.First().TotalWorkHourInWeek,
                        ShiftSettingFromShiftboard = x.First().ShiftSettingFromShiftboard,
                        MaximumForcedOverTime = x.First().MaximumForcedOverTime
                    });

                    int forcedOverTime = 0;

                    foreach (var item in shiftResult)
                    {
                        var monthTimeSheetDraftByEmployeeId = monthTimeSheetDraftList.FirstOrDefault(x => x.EmployeeId == item.EmployeeId && x.WorkTimeId == item.WorkTimeId);
                        //monthTimeSheetDraftByEmployeeId
                        if (item.ShiftSettingFromShiftboard)
                            forcedOverTime = item.ForcedOverTime;
                        else
                        {
                            forcedOverTime = CalculateForcedOverTimeForFixedShift(item.MaximumForcedOverTime, item.TotalWorkHourInWeek, item.SumTimeDurationInMinut.Value);
                        }
                        if (monthTimeSheetDraftByEmployeeId != null)
                        {
                            var monthTimeSheetDraftById = _kscHrUnitOfWork.MonthTimeSheetDraftRepository.GetById(monthTimeSheetDraftByEmployeeId.Id);
                            monthTimeSheetDraftById.LastForcedOverTime = monthTimeSheetDraftByEmployeeId.ForcedOverTime;
                            monthTimeSheetDraftById.ForcedOverTime = forcedOverTime;
                        }
                        else
                        {
                            MonthTimeSheetDraft newMonthTimeSheetDraft = new MonthTimeSheetDraft();
                            newMonthTimeSheetDraft.EmployeeId = item.EmployeeId;
                            newMonthTimeSheetDraft.YearMonth = yearMonth;
                            newMonthTimeSheetDraft.WorkTimeId = item.WorkTimeId;
                            newMonthTimeSheetDraft.ForcedOverTime = forcedOverTime;
                            await _kscHrUnitOfWork.MonthTimeSheetDraftRepository.AddAsync(newMonthTimeSheetDraft);
                        }

                    }
                    List<int> employeeIdList = shiftResult.Select(x => x.EmployeeId).ToList();
                    var monthTimeSheetDraftTemp = monthTimeSheetDraftList.Where(x => employeeIdList.Any(s => s == x.EmployeeId) == false);
                    foreach (var item in monthTimeSheetDraftTemp)
                    {
                        var monthTimeSheetDraftById = monthTimeSheetDraft.First(x => x.Id == item.Id);
                        monthTimeSheetDraftById.LastForcedOverTime = item.ForcedOverTime;
                        var oldForcedOverTime = item.ForcedOverTime;
                        monthTimeSheetDraftById.ForcedOverTime = 0;
                    }
                    //
                    _kscHrUnitOfWork.MonthTimeSheetLogRepository.Add(new MonthTimeSheetLog()
                    {
                        InsertDate = DateTime.Now,
                        YearMonth = yearMonth,
                        InsertUser = model.CurrentUser,
                        Result = "محاسبه اضافه کاری قهری با موفقیت انجام شد",
                        MonthTimeShitStepperId = model.Step,
                        ResultCount = monthTimeSheetDraftTemp.Count() + employeeIdList.Count(),

                    });
                    //
                    await _kscHrUnitOfWork.SaveAsync();
                    var date6 = DateTime.Now.Minute + ":" + DateTime.Now.Second;

                }
            }
            catch (Exception ex)
            {

                result.AddError("", ex.Message);
            }
            return result;

        }
        /// <summary>
        /// محاسبه اضافه کاری قهری
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<KscResult> CalculateForcedOverTimeStep(UpdateStatusByYearMonthProcedureModel model)

        {
            int yearMonth = int.Parse(model.Yearmonth);
            var result = new KscResult();
            try
            {
                var systemStatus = await _kscHrUnitOfWork.WorkCalendarRepository.GetMonthTimeSheetStatus(yearMonth);
                if (systemStatus == true)//کاربران اداری سیستم بسته شده باشد
                {
                    if (_kscHrUnitOfWork.MonthTimeSheetRepository.Any(x => x.YearMonth == yearMonth && x.IsCreatedManual == false))
                    {
                        throw new HRBusinessException(Validations.RepetitiveId, "تایم شیت ماه انتخابی ایجاد شده است");
                    }
                    //
                    //var employeeConditionalAbcenseNotHaveForcedOvertime = _kscHrUnitOfWork.EmployeeConditionalAbsenceRepository
                    //       .GetEmployeeConditionalAbcenseNotHaveForcedOvertime(yearMonth).Select(x => x.EmployeeId).ToList();
                    var employeeConditionalAbcenseNotHaveForcedOvertimeList = _kscHrUnitOfWork.EmployeeConditionalAbsenceRepository
                           .GetEmployeeConditionalAbcenseNotHaveForcedOvertime(yearMonth)
                           .Select(x => new { EmployeeId = x.EmployeeId, StartDate = x.StartDate, EndDate = x.EndDate });

                    //
                    var date1 = DateTime.Now.Minute + ":" + DateTime.Now.Second;
                    var timeShiftSettingForcedOverTimeModel = _kscHrUnitOfWork.TimeShiftSettingRepository.GetAllByIncludedAsNotracking(DateTime.Now)
                                               .Select(x => new ForcedOverTimeModel()
                                               {
                                                   ShiftConceptId = x.WorkCompanySetting.WorkTimeShiftConcept.ShiftConceptId,
                                                   WorkTimeId = x.WorkCompanySetting.WorkTimeShiftConcept.WorkTimeId,
                                                   WorkCityId = x.WorkCompanySetting.WorkCityId,
                                                   ForcedOverTime = x.ForcedOverTime,
                                                   TotalWorkHourInWeek = x.TotalWorkHourInWeek

                                               }).ToList();
                    var monthTimeSheetDraft = _kscHrUnitOfWork.MonthTimeSheetDraftRepository.GetMonthTimeSheetDraftByYearMonth(yearMonth);
                    var monthTimeSheetDraftList = monthTimeSheetDraft.ToList();
                    var rollCallIncludedForcedOverTime = _kscHrUnitOfWork.IncludedRollCallRepository.GetActiveIncludedRollCallByIncludedIdAsNoTracking(EnumIncludedDefinition.ForcedOverTime.Id);
                    //
                    var employeeAttendAbsenceItems = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetEmployeeAttendAbsenceItemAsNoTracking();
                    var workCalendars = _kscHrUnitOfWork.WorkCalendarRepository.GetWorkCalendarByYearMonthAsNoTracking(yearMonth);
                    var employeeAttendAbsenceItemShift = from item in employeeAttendAbsenceItems
                                                         join cal in workCalendars on item.WorkCalendarId equals cal.Id
                                                         join time in _kscHrUnitOfWork.WorkTimeRepository.GetAllQueryable() on item.WorkTimeId equals time.Id
                                                         join shift in _kscHrUnitOfWork.ShiftConceptDetailRepository.GetAllQueryable() on item.ShiftConceptDetailId equals shift.Id
                                                         join empl in _kscHrUnitOfWork.EmployeeRepository.GetAllQueryable() on item.EmployeeId equals empl.Id
                                                         join includedForced in rollCallIncludedForcedOverTime on item.RollCallDefinitionId equals includedForced.RollCallDefinitionId
                                                         where item.InvalidRecord == false
                                                         select new
                                                         {
                                                             item.EmployeeId,
                                                             item.WorkCalendarId,
                                                             cal.MiladiDateV1,
                                                             cal.WorkDayTypeId,
                                                             item.WorkTimeId,
                                                             shift.ShiftConceptId,
                                                             empl.WorkCityId,
                                                             time.ShiftSettingFromShiftboard,
                                                             item.TimeDurationInMinute,
                                                             time.MaximumForcedOverTime
                                                         };
                    //
                    var dataForExclud = from shift in employeeAttendAbsenceItemShift
                                        join condtion in employeeConditionalAbcenseNotHaveForcedOvertimeList on shift.EmployeeId equals condtion.EmployeeId
                                        where shift.MiladiDateV1 >= condtion.StartDate.Date && shift.MiladiDateV1 <= condtion.EndDate.Date
                                        select shift;
                    employeeAttendAbsenceItemShift = employeeAttendAbsenceItemShift.Except(dataForExclud);
                    //
                    var invalidDayTypeInForcedOvertimeList = _kscHrUnitOfWork.InvalidDayTypeInForcedOvertimeRepository.GetAllQueryable().Where(x => x.IsActive).Select(x => new { x.WorkTimeId, x.WorkDayTypeId });
                    if (invalidDayTypeInForcedOvertimeList.Any())
                    {
                        employeeAttendAbsenceItemShift = employeeAttendAbsenceItemShift.Where(x => !invalidDayTypeInForcedOvertimeList.Any(a => a.WorkTimeId == x.WorkTimeId && a.WorkDayTypeId == x.WorkDayTypeId));
                    }
                    //
                    var shiftForcedOverTimeModel = employeeAttendAbsenceItemShift.GroupBy(x => new
                    {
                        x.EmployeeId,
                        x.WorkCityId,
                        x.WorkTimeId,
                        x.ShiftConceptId,
                        x.WorkCalendarId,
                        x.ShiftSettingFromShiftboard,
                        x.MaximumForcedOverTime
                    }).Select(y => new
                    {
                        EmployeeId = y.Key.EmployeeId,
                        WorkCityId = y.Key.WorkCityId,
                        WorkTimeId = y.Key.WorkTimeId,
                        ShiftConceptId = y.Key.ShiftConceptId,
                        ShiftSettingFromShiftboard = y.Key.ShiftSettingFromShiftboard,
                        MaximumForcedOverTime = y.Key.MaximumForcedOverTime,
                        Count = y.Select(x => x.WorkCalendarId).Distinct().Count(),
                        SumTimeDurationInMinutePerDay = y.Sum(x => x.TimeDurationInMinute)
                    }).ToList();
                    var shiftData = shiftForcedOverTimeModel.Join(timeShiftSettingForcedOverTimeModel, a => new { shiftConceptId = a.ShiftConceptId, workTimeId = a.WorkTimeId, workCityId = a.WorkCityId.Value }
                      , b => new { shiftConceptId = b.ShiftConceptId, workTimeId = b.WorkTimeId, workCityId = b.WorkCityId }
                      , (a, b) => new
                      {
                          EmployeeId = a.EmployeeId,
                          WorkTimeId = a.WorkTimeId,
                          ShiftSettingFromShiftboard = a.ShiftSettingFromShiftboard,
                          //MaximumForcedOverTime = a.MaximumForcedOverTime != null ? a.MaximumForcedOverTime.ConvertDurationToMinute().Value : 0,
                          MaximumForcedOverTime = b.ForcedOverTime != null ? b.ForcedOverTime.ConvertDurationToMinute().Value : 0,
                          ForcedOverTime = b.ForcedOverTime != null ? b.ForcedOverTime.ConvertDurationToMinute().Value * a.Count : 0,
                          SumTimeDurationInMinutePerDay = a.SumTimeDurationInMinutePerDay,
                          TotalWorkHourInWeek = b.TotalWorkHourInWeek != null ? b.TotalWorkHourInWeek.ConvertDurationToMinute().Value : 0,
                          ShiftConceptId = b.ShiftConceptId,
                      }
                      );

                    var shiftResult = shiftData.GroupBy(x => new { x.EmployeeId, x.WorkTimeId }).Select(x => new
                    {
                        EmployeeId = x.Key.EmployeeId,
                        WorkTimeId = x.Key.WorkTimeId,
                        ForcedOverTime = x.Sum(x => x.ForcedOverTime),
                        SumTimeDurationInMinut = x.Sum(x => x.SumTimeDurationInMinutePerDay),
                        TotalWorkHourInWeek = x.First().TotalWorkHourInWeek,
                        ShiftSettingFromShiftboard = x.First().ShiftSettingFromShiftboard,
                        MaximumForcedOverTime = x.First().MaximumForcedOverTime
                    });

                    int forcedOverTime = 0;

                    foreach (var item in shiftResult)
                    {
                        var monthTimeSheetDraftByEmployeeId = monthTimeSheetDraftList.FirstOrDefault(x => x.EmployeeId == item.EmployeeId && x.WorkTimeId == item.WorkTimeId);
                        //monthTimeSheetDraftByEmployeeId
                        if (item.ShiftSettingFromShiftboard)
                            forcedOverTime = item.ForcedOverTime;
                        else
                        {
                            forcedOverTime = CalculateForcedOverTimeForFixedShift(item.MaximumForcedOverTime, item.TotalWorkHourInWeek, item.SumTimeDurationInMinut.Value);
                        }
                        if (monthTimeSheetDraftByEmployeeId != null)
                        {
                            var monthTimeSheetDraftById = _kscHrUnitOfWork.MonthTimeSheetDraftRepository.GetById(monthTimeSheetDraftByEmployeeId.Id);
                            monthTimeSheetDraftById.LastForcedOverTime = monthTimeSheetDraftByEmployeeId.ForcedOverTime;
                            monthTimeSheetDraftById.ForcedOverTime = forcedOverTime;
                        }
                        else
                        {
                            MonthTimeSheetDraft newMonthTimeSheetDraft = new MonthTimeSheetDraft();
                            newMonthTimeSheetDraft.EmployeeId = item.EmployeeId;
                            newMonthTimeSheetDraft.YearMonth = yearMonth;
                            newMonthTimeSheetDraft.WorkTimeId = item.WorkTimeId;
                            newMonthTimeSheetDraft.ForcedOverTime = forcedOverTime;
                            await _kscHrUnitOfWork.MonthTimeSheetDraftRepository.AddAsync(newMonthTimeSheetDraft);
                        }

                    }
                    List<int> employeeIdList = shiftResult.Select(x => x.EmployeeId).ToList();
                    var monthTimeSheetDraftTemp = monthTimeSheetDraftList.Where(x => employeeIdList.Any(s => s == x.EmployeeId) == false);
                    foreach (var item in monthTimeSheetDraftTemp)
                    {
                        var monthTimeSheetDraftById = monthTimeSheetDraft.First(x => x.Id == item.Id);
                        monthTimeSheetDraftById.LastForcedOverTime = item.ForcedOverTime;
                        var oldForcedOverTime = item.ForcedOverTime;
                        monthTimeSheetDraftById.ForcedOverTime = 0;
                    }
                    model.Result = "محاسبه اضافه کاری قهری با موفقیت انجام شد";
                    model.ResultCount = monthTimeSheetDraftTemp.Count() + employeeIdList.Count();
                    //
                    //_kscHrUnitOfWork.MonthTimeSheetLogRepository.Add(new MonthTimeSheetLog()
                    //{
                    //    InsertDate = DateTime.Now,
                    //    YearMonth = yearMonth,
                    //    InsertUser = model.CurrentUser,
                    //    Result = "محاسبه اضافه کاری قهری با موفقیت انجام شد",
                    //    MonthTimeShitStepperId = model.Step,
                    //    ResultCount = monthTimeSheetDraftTemp.Count() + employeeIdList.Count(),

                    //});
                    result = await _procedureService.InsertStepProcedure(model);
                    if (result.Success)
                        await _kscHrUnitOfWork.SaveAsync();
                    var date6 = DateTime.Now.Minute + ":" + DateTime.Now.Second;

                }
            }
            catch (Exception ex)
            {

                result.AddError("", ex.Message);
            }
            return result;

        }
        private static int CalculateForcedOverTimeForFixedShift(int maximumForcedOverTime, int totalWorkHourInWeek, int sumTimeDuration)
        {
            var div = (sumTimeDuration / totalWorkHourInWeek) * 60;//خارج قسمت

            var mod = sumTimeDuration % totalWorkHourInWeek;//باقیمانده
            int baseMinute = 0;
            if (mod > 1440)
            {
                baseMinute = 60;//60min=1 h
            }
            sumTimeDuration = baseMinute + div;
            if (sumTimeDuration >= maximumForcedOverTime)
            {
                sumTimeDuration = maximumForcedOverTime;
            }
            int result = sumTimeDuration;
            return result;
        }
        public async Task<string> CalculateForcedOverTimeByYearMonth111(int yearMonth)
        {
            try
            {
                var date1 = DateTime.Now.Minute + ":" + DateTime.Now.Second;
                var timeShiftSettingForcedOverTimeModel = _kscHrUnitOfWork.TimeShiftSettingRepository.GetAllByIncludedAsNotracking(DateTime.Now)
                                           .Select(x => new ForcedOverTimeModel()
                                           {
                                               ShiftConceptId = x.WorkCompanySetting.WorkTimeShiftConcept.ShiftConceptId,
                                               WorkTimeId = x.WorkCompanySetting.WorkTimeShiftConcept.WorkTimeId,
                                               WorkCityId = x.WorkCompanySetting.WorkCityId,
                                               ForcedOverTime = x.ForcedOverTime,
                                               TotalWorkHourInWeek = x.TotalWorkHourInWeek
                                           }).ToList();
                var monthTimeSheetDraft = _kscHrUnitOfWork.MonthTimeSheetDraftRepository.GetMonthTimeSheetDraftByYearMonth(yearMonth);

                // قهری غیرشیفتی
                //var rollCallIncludedForcedOverTime = GetRollCallIncludedForcedOverTime();
                var rollCallIncludedForcedOverTime = _kscHrUnitOfWork.IncludedRollCallRepository.GetActiveIncludedRollCallByIncludedIdAsNoTracking(EnumIncludedDefinition.ForcedOverTime.Id);
                var rollCallIncludedForcedOverTimeSelected = rollCallIncludedForcedOverTime.Select(x => x.RollCallDefinitionId).ToList();
                var employeeAttendAbsenceItems = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetEmployeeAttendAbsenceItemAsNoTracking();
                var workCalendars = _kscHrUnitOfWork.WorkCalendarRepository.GetWorkCalendarByYearMonthAsNoTracking(yearMonth);
                var queryEmployeeAttendAbsenceItemByEmployeeId = from item in employeeAttendAbsenceItems
                                                                 join cal in workCalendars on item.WorkCalendarId equals cal.Id
                                                                 join time in _kscHrUnitOfWork.WorkTimeRepository.GetAllQueryable() on item.WorkTimeId equals time.Id
                                                                 where item.InvalidRecord == false && time.ShiftSettingFromShiftboard == false

                                                                 select new EmployeeAttendAbsenceItemForcedOverTimeModel()
                                                                 {
                                                                     EmployeeId = item.EmployeeId,
                                                                     EmployeeAttendAbsenceItemId = item.Id,
                                                                     RollCallDefinitionId = item.RollCallDefinitionId,
                                                                     TimeDurationInMinute = item.TimeDurationInMinute.Value,
                                                                     WorkTimeId = item.WorkTimeId,
                                                                     WorkCalendarId = item.WorkCalendarId,
                                                                     WorkCalendarDate = cal.MiladiDateV1

                                                                 };
                var employeeAttendAbsenceItem = queryEmployeeAttendAbsenceItemByEmployeeId.ToList();
                var employeeList = employeeAttendAbsenceItem.Select(x => new { x.EmployeeId, x.WorkTimeId }).Distinct().ToList();
                var workTimes = _kscHrUnitOfWork.WorkTimeRepository.GetAllQueryable().ToList();
                var date2 = DateTime.Now.Minute + ":" + DateTime.Now.Second;

                foreach (var item in employeeList)
                {
                    var workTime = workTimes.First(x => x.Id == item.WorkTimeId);
                    var employeeAttendAbsenceItemByEmployeeId = employeeAttendAbsenceItem.Where(x => x.EmployeeId == item.EmployeeId && x.WorkTimeId == item.WorkTimeId).ToList();
                    var timeShiftSettingForcedOverTime = timeShiftSettingForcedOverTimeModel.First(x => x.WorkTimeId == item.WorkTimeId);
                    var totalWorkHourInWeek = timeShiftSettingForcedOverTime.TotalWorkHourInWeek.ConvertDurationToMinute().Value;
                    var forcedOverTimeShiftSetting = timeShiftSettingForcedOverTime.ForcedOverTime.ConvertDurationToMinute().Value;
                    var forcedOverTime = CalculateForcedOverTime(employeeAttendAbsenceItemByEmployeeId, rollCallIncludedForcedOverTimeSelected, workTime, totalWorkHourInWeek, forcedOverTimeShiftSetting);
                    if (forcedOverTime != 0)
                    {
                        var monthTimeSheetDraftByEmployeeId = monthTimeSheetDraft.Where(x => x.EmployeeId == item.EmployeeId && x.WorkTimeId == item.WorkTimeId).FirstOrDefault();
                        if (monthTimeSheetDraftByEmployeeId != null)
                            monthTimeSheetDraftByEmployeeId.ForcedOverTime = forcedOverTime;
                        else
                        {
                            MonthTimeSheetDraft newMonthTimeSheetDraft = new MonthTimeSheetDraft();
                            newMonthTimeSheetDraft.EmployeeId = item.EmployeeId;
                            newMonthTimeSheetDraft.YearMonth = yearMonth;
                            newMonthTimeSheetDraft.WorkTimeId = item.WorkTimeId;
                            newMonthTimeSheetDraft.ForcedOverTime = forcedOverTime;
                            await _kscHrUnitOfWork.MonthTimeSheetDraftRepository.AddAsync(newMonthTimeSheetDraft);
                        }
                    }
                }
                var date3 = DateTime.Now.Minute + ":" + DateTime.Now.Second;

                // قهری شیفتی

                var employeeAttendAbsenceItemShift = from item in employeeAttendAbsenceItems
                                                     join cal in workCalendars on item.WorkCalendarId equals cal.Id
                                                     join time in _kscHrUnitOfWork.WorkTimeRepository.GetAllQueryable() on item.WorkTimeId equals time.Id
                                                     join shift in _kscHrUnitOfWork.ShiftConceptDetailRepository.GetAllQueryable() on item.ShiftConceptDetailId equals shift.Id
                                                     join empl in _kscHrUnitOfWork.EmployeeRepository.GetAllQueryable() on item.EmployeeId equals empl.Id
                                                     join includedForced in rollCallIncludedForcedOverTime on item.RollCallDefinitionId equals includedForced.RollCallDefinitionId
                                                     where item.InvalidRecord == false && time.ShiftSettingFromShiftboard == true
                                                     select new { item.EmployeeId, item.WorkCalendarId, item.WorkTimeId, shift.ShiftConceptId, empl.WorkCityId };
                var shiftForcedOverTimeModel = employeeAttendAbsenceItemShift.Distinct().GroupBy(x => new { x.EmployeeId, x.WorkCityId, x.WorkTimeId, x.ShiftConceptId, x.WorkCalendarId }).Select(y => new
                {
                    EmployeeId = y.Key.EmployeeId,
                    WorkCityId = y.Key.WorkCityId,
                    WorkTimeId = y.Key.WorkTimeId,
                    ShiftConceptId = y.Key.ShiftConceptId,
                    Count = y.Count()
                }).ToList();
                var t1 = shiftForcedOverTimeModel.Where(x => x.EmployeeId == 1736);
                var shiftData = shiftForcedOverTimeModel.Join(timeShiftSettingForcedOverTimeModel, a => new { shiftConceptId = a.ShiftConceptId, workTimeId = a.WorkTimeId, workCityId = a.WorkCityId.Value }
                  , b => new { shiftConceptId = b.ShiftConceptId, workTimeId = b.WorkTimeId, workCityId = b.WorkCityId }
                  , (a, b) => new
                  {
                      EmployeeId = a.EmployeeId,
                      WorkTimeId = a.WorkTimeId,
                      ForcedOverTime = b.ForcedOverTime != null ? b.ForcedOverTime.ConvertDurationToMinute().Value * a.Count : 0,
                      ShiftConceptId = b.ShiftConceptId,
                  }
                  );
                var t3 = shiftData.Where(x => x.EmployeeId == 1736);
                var shiftResult = shiftData.GroupBy(x => new { x.EmployeeId, x.WorkTimeId }).Select(x => new { EmployeeId = x.Key.EmployeeId, WorkTimeId = x.Key.WorkTimeId, ForcedOverTime = x.Sum(x => x.ForcedOverTime) });
                var t2 = shiftResult.Where(x => x.EmployeeId == 1736);
                var date4 = DateTime.Now.Minute + ":" + DateTime.Now.Second;

                foreach (var item in shiftResult)
                {
                    var monthTimeSheetDraftByEmployeeId = monthTimeSheetDraft.FirstOrDefault(x => x.EmployeeId == item.EmployeeId && x.WorkTimeId == item.WorkTimeId);
                    if (monthTimeSheetDraftByEmployeeId != null)
                    {
                        monthTimeSheetDraftByEmployeeId.ForcedOverTime = item.ForcedOverTime;
                    }
                    else
                    {
                        MonthTimeSheetDraft newMonthTimeSheetDraft = new MonthTimeSheetDraft();
                        newMonthTimeSheetDraft.EmployeeId = item.EmployeeId;
                        newMonthTimeSheetDraft.YearMonth = yearMonth;
                        newMonthTimeSheetDraft.WorkTimeId = item.WorkTimeId;
                        newMonthTimeSheetDraft.ForcedOverTime = item.ForcedOverTime;
                        await _kscHrUnitOfWork.MonthTimeSheetDraftRepository.AddAsync(newMonthTimeSheetDraft);
                    }

                }
                var date5 = DateTime.Now.Minute + ":" + DateTime.Now.Second;

                await _kscHrUnitOfWork.SaveAsync();
                var date6 = DateTime.Now.Minute + ":" + DateTime.Now.Second;

                return date1 + "|" + date2 + "|" + date3 + "|" + date4 + "|" + date5 + "|" + date6;
            }
            catch (Exception ex)
            {

                return ex.Message;
            }

        }

        public async Task<KscResult> TestOverTimeSpecialHolidayTimeSheet(SearchMonthTimeSheetModel model)
        {
            var result = new KscResult();
            int resultCount = 0;
            System.IO.StreamWriter newPath = new System.IO.StreamWriter(string.Format((@"d:\\newList.txt")));
            System.IO.StreamWriter deletePath = new System.IO.StreamWriter(string.Format((@"d:\\deleteList.txt")));
            System.IO.StreamWriter updatePath = new System.IO.StreamWriter(string.Format((@"d:\\updateList.txt")));
            //foreach (var item in dataGroupAverage)
            //{
            //    overTimePath.WriteLine(item.EmployeeId + "|" + item.Average);
            //}
            //overTimePath.Close();
            try
            {
                var YearMonth = int.Parse(model.DateTimeSheet);
                var systemStatus = true;// await _kscHrUnitOfWork.WorkCalendarRepository.GetMonthTimeSheetStatus(YearMonth);
                if (systemStatus == true)//کاربران اداری سیستم بسته شده باشد
                {
                    List<overTime> overTimeList = new List<overTime>();
                    //if (_kscHrUnitOfWork.MonthTimeSheetRepository.Any(x => x.YearMonth == YearMonth && x.IsCreatedManual == false))
                    //{
                    //    throw new HRBusinessException(Validations.RepetitiveId, "تایم شیت ماه انتخابی ایجاد شده است");
                    //}
                    int beforeMonth = YearMonth - 1;
                    if (YearMonth.ToString().Substring(5, 1) == "1")
                    {
                        int year = int.Parse(YearMonth.ToString().Substring(0, 4)) - 1;
                        beforeMonth = int.Parse(year + "12");
                    }
                    //var YearMonthDayShamsi_Prev = _kscHrUnitOfWork.WorkCalendarRepository.GetPrevMonth(YearMonth);
                    //var beforeMonth  = int.Parse(YearMonthDayShamsi_Prev.Substring(0, 7).Replace("/", ""));

                    var monthWorkCalendar = _kscHrUnitOfWork.WorkCalendarRepository.GetRangeMonthWorkCalendar(beforeMonth, YearMonth);
                    var monthWorkCalendarByYearMonth = _kscHrUnitOfWork.WorkCalendarRepository.GetMonthWorkCalendar(YearMonth);
                    var rollCallDefinitionAttend = _kscHrUnitOfWork.RollCallDefinitionRepository.GetRollCallDefinitionByRollCallConceptIdAsNoTracking(EnumRollCallConcept.Attend.Id).Select(x => x.Id).ToList();
                    var timeSheetSettingActive = await _kscHrUnitOfWork.TimeSheetSettingRepository.GetTimeSheetSettingActiveAsync();
                    var shiftConceptDetailValid = _kscHrUnitOfWork.ShiftConceptDetailRepository.WhereQueryable(x => x.IsValidSpecialOverTimeTimeSheet).Select(x => x.Id).ToList();
                    var officialHoliday = monthWorkCalendarByYearMonth.Where(x => x.DayNumber == timeSheetSettingActive.OverTimeSpecialDayNumber && x.IsOfficialHoliday);
                    if (officialHoliday.Any())
                    {
                        List<EmployeeAttendAbsenceItem> resultEmployeeAttendAbsenceItem = new List<EmployeeAttendAbsenceItem>();
                        var includedDefinition = _kscHrUnitOfWork.IncludedRollCallRepository.GetActiveIncludedRollCallByIncludedIdAsNoTracking(EnumIncludedDefinition.ThursdayOfficalHoliadyIncludedOverTime.Id);
                        var overTimeSpecialHolidayTimeSheetSetting = _kscHrUnitOfWork.OverTimeSpecialHolidayTimeSheetSettingRepository.GetAllQueryable().ToList();
                        var employeeAttendAbsenceItem = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetValidItems();
                        var attendAbsenceItemInHoliday = employeeAttendAbsenceItem.Where(x => officialHoliday.Any(y => y.WorkCalendarId == x.WorkCalendarId)).ToList();
                        //
                        var thursdayOfficalHoliadyOverTimeRollCall = EnumRollCallDefinication.ThursdayOfficalHoliadyOverTime.Id;
                        //
                        foreach (var day in officialHoliday)
                        {
                            var endDay = day.Date.AddDays(-1);
                            var startDay = day.Date.AddDays(-5);
                            var WorkCalendarIds = monthWorkCalendar.Where(x => x.Date >= startDay && x.Date <= endDay).Select(x => x.WorkCalendarId).ToList();
                            var attendAbsenceItem = employeeAttendAbsenceItem.Where(x => WorkCalendarIds.Any(y => y == x.WorkCalendarId)
                            && shiftConceptDetailValid.Any(s => s == x.ShiftConceptDetailId));
                            var attendAbsenceItemResult = (from item in attendAbsenceItem
                                                           join includedRollCall in includedDefinition
                                                          on item.RollCallDefinitionId equals includedRollCall.RollCallDefinitionId
                                                           group item by new
                                                           {
                                                               EmployeeId = item.EmployeeId,
                                                           }
                                                    into newgroup
                                                           select new EmployeeItemGroupModel()
                                                           {
                                                               EmployeeId = newgroup.Key.EmployeeId,
                                                               DurationInDay = newgroup.Select(x => x.WorkCalendarId).Distinct().Count()

                                                           });
                            var ttttt = attendAbsenceItemResult.Where(x => x.EmployeeId == 8992);
                            foreach (var item in attendAbsenceItemResult)
                            {
                                if (item.EmployeeId == 8992)
                                {
                                    var t = 0;
                                }
                                var overTimeSpecialHoliday = overTimeSpecialHolidayTimeSheetSetting.FirstOrDefault(x => x.DayCount == item.DurationInDay);
                                if (overTimeSpecialHoliday != null)
                                {
                                    overTime obj = new overTime
                                    {
                                        EmployeeId = item.EmployeeId,
                                        Duration = overTimeSpecialHoliday.Duration
                                    };
                                    overTimeList.Add(obj);
                                    //  overTimePath.WriteLine(item.EmployeeId + "|" + overTimeSpecialHoliday.Duration);
                                    // ;
                                    //var employeeAttendAbsenceItemInHoliday = attendAbsenceItemInHoliday.Where(x => x.WorkCalendarId == day.WorkCalendarId && x.EmployeeId == item.EmployeeId);

                                    //if (employeeAttendAbsenceItemInHoliday.Count() != 0 && employeeAttendAbsenceItemInHoliday.Any(x => x.RollCallDefinitionId == thursdayOfficalHoliadyOverTimeRollCall) == false) //قبلا ثبت نشده باشد
                                    //{
                                    //    var dataMap = employeeAttendAbsenceItemInHoliday.FirstOrDefault();
                                    //    if (dataMap != null)
                                    //    {
                                    //        EmployeeAttendAbsenceItem newEmployeeAttendAbsenceItem = new EmployeeAttendAbsenceItem()
                                    //        {
                                    //            WorkCalendarId = dataMap.WorkCalendarId,
                                    //            ShiftConceptDetailId = dataMap.ShiftConceptDetailId,
                                    //            ShiftConceptDetailIdInShiftBoard = dataMap.ShiftConceptDetailIdInShiftBoard,
                                    //            EmployeeId = dataMap.EmployeeId,
                                    //            StartTime = "07:00",
                                    //            EndTime = "16:00",
                                    //            TimeDuration = overTimeSpecialHoliday.Duration,
                                    //            RollCallDefinitionId = thursdayOfficalHoliadyOverTimeRollCall,
                                    //            InsertDate = DateTime.Now,
                                    //            InsertUser = model.CurrentUser,
                                    //            WorkTimeId = dataMap.WorkTimeId,
                                    //            InvalidRecord = false,
                                    //            InvalidRecordReason = "",
                                    //            IsFloat = false,
                                    //            IsManual = false,
                                    //        };
                                    //        resultEmployeeAttendAbsenceItem.Add(newEmployeeAttendAbsenceItem);
                                    //    }
                                    //}

                                }

                            }
                        }
                        //
                        resultCount += resultEmployeeAttendAbsenceItem.Count();
                        // await _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.AddRangeAsync(resultEmployeeAttendAbsenceItem);
                        //
                    }
                    //
                    //_kscHrUnitOfWork.MonthTimeSheetLogRepository.Add(new MonthTimeSheetLog()
                    //{
                    //    InsertDate = DateTime.Now,
                    //    YearMonth = YearMonth,
                    //    InsertUser = model.CurrentUser,
                    //    Result = "محاسبه اضافه کاری پنجشنبه تعطیل کاری با موفقیت انجام شد",
                    //    MonthTimeShitStepperId = model.Step,
                    //    ResultCount = resultCount,

                    //});
                    //
                    // await _kscHrUnitOfWork.SaveAsync();
                    //
                    var insertDateS = new DateTime(2024, 9, 22, 19, 49, 7);
                    var insertDateE = new DateTime(2024, 9, 22, 19, 49, 8);
                    var employeeMonth = _kscHrUnitOfWork.MonthTimeSheetRollCallRepository.GetAllQueryable().Include(x => x.MonthTimeSheet)
                         .Where(x => x.RollCallDefinitionId == 81 && x.MonthTimeSheet.YearMonth == YearMonth).ToList()
                         //.Where(x => x.InsertDate.Value >= insertDateS && x.InsertDate.Value < insertDateE)
                         .Select(x => new { EmployeeId = x.MonthTimeSheet.EmployeeId, Duration = Utility.ConvertMinuteIn24ToDuration(x.DurationInMinut) });
                    var newList = overTimeList.Where(x => !employeeMonth.Any(y => y.EmployeeId == x.EmployeeId));
                    var inValid = employeeMonth.Where(x => !overTimeList.Any(o => o.EmployeeId == x.EmployeeId));
                    //  var updateTime = employeeMonth.Where(x => overTimeList.Any(o => o.EmployeeId == x.EmployeeId && o.Duration != x.Duration));
                    var updateTime = from m in employeeMonth
                                     join o in overTimeList on m.EmployeeId equals o.EmployeeId
                                     where m.Duration != o.Duration
                                     select new
                                     {
                                         EmployeeId = m.EmployeeId,
                                         currentDuration = m.Duration,
                                         newDuration = o.Duration
                                     };
                    //
                    var employee = _kscHrUnitOfWork.EmployeeRepository.GetAllQueryable()
                         .Select(x => new { Id = x.Id, EmployeeNumber = x.EmployeeNumber, Name = x.Name, Family = x.Family }).ToList();
                    var newListGroup = newList.GroupBy(x => x.EmployeeId).Select(x => new { EmployeeId = x.Key, Duration = x.Sum(s => Utility.ConvertDurationToMinute(s.Duration)) });
                    foreach (var item in newListGroup)
                    {
                        var emp = employee.FirstOrDefault(x => x.Id == item.EmployeeId);
                        newPath.WriteLine(emp.Id + "|" + emp.EmployeeNumber + "|" + emp.Name + "|" + emp.Family + "|" + Utility.ConvertMinuteIn24ToDuration(item.Duration.Value));
                    }
                    var inValidGroup = inValid.GroupBy(x => x.EmployeeId).Select(x => new { EmployeeId = x.Key, Duration = x.Sum(s => Utility.ConvertDurationToMinute(s.Duration)) });

                    foreach (var item in inValidGroup)
                    {
                        var emp = employee.FirstOrDefault(x => x.Id == item.EmployeeId);
                        deletePath.WriteLine(emp.Id + "|" + emp.EmployeeNumber + "|" + emp.Name + "|" + emp.Family + "|" + Utility.ConvertMinuteIn24ToDuration(item.Duration.Value));
                    }
                    var updateTimeGroup = updateTime.GroupBy(x => x.EmployeeId).Select(x => new
                    {
                        EmployeeId = x.Key,
                        currentDuration = x.Max(s => Utility.ConvertDurationToMinute(s.currentDuration))
                    ,
                        newDuration = x.Sum(s => Utility.ConvertDurationToMinute(s.newDuration))
                    });
                    updateTimeGroup = updateTimeGroup.Where(x => x.newDuration != x.currentDuration);
                    foreach (var item in updateTimeGroup)
                    {

                        var emp = employee.FirstOrDefault(x => x.Id == item.EmployeeId);
                        updatePath.WriteLine(emp.Id + "|" + emp.EmployeeNumber + "|" + emp.Name + "|" + emp.Family + "|" + Utility.ConvertMinuteIn24ToDuration(item.currentDuration.Value)
                            + "|" + Utility.ConvertMinuteIn24ToDuration(item.newDuration.Value));
                    }

                    newPath.Close();
                    deletePath.Close();
                    updatePath.Close();
                }
                else
                {
                    throw new HRBusinessException(Validations.CloseDailyTimeSgeetId, Validations.CloseDailyTimeSgeet);
                }
            }
            catch (Exception ex)
            {
                newPath.Close();
                deletePath.Close();
                updatePath.Close();
                throw new HRBusinessException(Validations.RepetitiveId, ex.Message);
            }


            return result;
        }
        public async Task<KscResult> TestCeilingOvertimeTimeSheetStep(UpdateStatusByYearMonthProcedureModel model)
        {
            var result = new KscResult();
            System.IO.StreamWriter newPath = new System.IO.StreamWriter(string.Format((@"d:\\CeilingOvertimeList.txt")));
            System.IO.StreamWriter updatePath = new System.IO.StreamWriter(string.Format((@"d:\\CeilingOvertimeListNew.txt")));

            try
            {
                var YearMonth = int.Parse(model.Yearmonth);
                var systemStatus = true;// await _kscHrUnitOfWork.WorkCalendarRepository.GetMonthTimeSheetStatus(YearMonth);
                if (systemStatus == true)//کاربران اداری سیستم بسته شده باشد
                {
                    //if (_kscHrUnitOfWork.MonthTimeSheetRepository.Any(x => x.YearMonth == YearMonth && x.IsCreatedManual == false))
                    //{
                    //    throw new HRBusinessException(Validations.RepetitiveId, "تایم شیت ماه انتخابی ایجاد شده است");
                    //}

                    //MonthTimeSheetRollCall
                    var attendAbcenseItem = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetMonthTimeSheetCalculateAsNoTracking(YearMonth);
                    var rollCallDefinitionByCeilingOvertime = _kscHrUnitOfWork.IncludedRollCallRepository.GetActiveIncludedRollCallByIncludedIdAsNoTracking(EnumIncludedDefinition.MaximunOverTime.Id)
                        .Select(x => x.RollCallDefinitionId);
                    var overTimePriority = _kscHrUnitOfWork.RollCallDefinitionRepository.WhereQueryable(x => rollCallDefinitionByCeilingOvertime.Any(r => r == x.Id) && x.OverTimePriority == null);
                    if (overTimePriority.Any())
                    {
                        var overTimePriorityIsNull = string.Join(",", overTimePriority.Select(x => x.Id).ToList());
                        throw new HRBusinessException(Validations.RepetitiveId, $"برای کدهای {overTimePriorityIsNull} ترتیب کسر مازاد اضافه کاری مشخص نشده است");

                    }
                    var attendAbcenseItemData = attendAbcenseItem.Where(x => x.InvalidRecord == false).Select(x => new EmployeeItemGroupModel()
                    {
                        EmployeeId = x.EmployeeId,
                        RollCallDefinitionId = x.RollCallDefinitionId,
                        TotalDuration = x.TimeDurationInMinute ?? 0,
                        IncreasedTimeDuration = x.IncreasedTimeDuration ?? 0,
                        TotalDuration1 = x.TimeDurationInMinute ?? 0

                    });

                    int forcedOverTime = EnumRollCallDefinication.ForcedOverTime.Id;
                    var forcedOverTimeData = _kscHrUnitOfWork.MonthTimeSheetDraftRepository.GetMonthTimeSheetDraftByYearMonth(YearMonth).Select(x => new EmployeeItemGroupModel()
                    {
                        EmployeeId = x.EmployeeId,
                        RollCallDefinitionId = forcedOverTime,
                        TotalDuration = x.ForcedOverTime,
                        IncreasedTimeDuration = 0,
                        TotalDuration1 = x.ForcedOverTime

                    });

                    attendAbcenseItemData = attendAbcenseItemData.Concat(forcedOverTimeData);
                    //
                    var query_MonthTimeSheetRollCall = from item in attendAbcenseItemData
                                                       join rollCall in rollCallDefinitionByCeilingOvertime on item.RollCallDefinitionId equals rollCall
                                                       group item by new
                                                       {

                                                           EmployeeId = item.EmployeeId
                                                       } into newgroup
                                                       select new EmployeeItemGroupModel()
                                                       {
                                                           EmployeeId = newgroup.Key.EmployeeId,
                                                           TotalDuration = newgroup.Sum(x => x.TotalDuration + (double)x.IncreasedTimeDuration),
                                                           TotalDuration1 = newgroup.Sum(x => x.TotalDuration1),


                                                       };

                    var employee = _kscHrUnitOfWork.EmployeeRepository.GetAllQueryable();
                    var overTimeDefinition = _kscHrUnitOfWork.OverTimeDefinitionRepository.GetAllQueryable();
                    var teamWork = _kscHrUnitOfWork.TeamWorkRepository.GetAllQueryable();
                    var resultData = (from item in query_MonthTimeSheetRollCall
                                      join empl in employee on item.EmployeeId equals empl.Id
                                      join team in teamWork on empl.TeamWorkId equals team.Id
                                      join overTime in overTimeDefinition on team.OverTimeDefinitionId equals overTime.Id
                                      select new EmployeeTimeSheetMonthModel()
                                      {
                                          EmployeeId = item.EmployeeId,
                                          YearMonth = YearMonth,
                                          CeilingOvertime = item.TotalDuration,
                                          ExcessOverTime = item.TotalDuration > overTime.MaximumDurationMinute ? item.TotalDuration - overTime.MaximumDurationMinute : 0,
                                          CeilingOvertime1 = item.TotalDuration1,
                                          ExcessOverTime1 = item.TotalDuration1 > overTime.MaximumDurationMinute ? item.TotalDuration1 - overTime.MaximumDurationMinute : 0
                                      }).ToList();
                    //
                    List<EmployeeTimeSheet> employeeTimeSheetList = new List<EmployeeTimeSheet>();
                    var employeeTimeSheetByYearMonth = _kscHrUnitOfWork.EmployeeTimeSheetRepository.GetEmployeeTimeSheet(YearMonth).ToList();
                    //

                    foreach (var item in resultData)
                    {
                        newPath.WriteLine(item.EmployeeId + "|" + item.CeilingOvertime + "|" + item.ExcessOverTime + "|" + item.CeilingOvertime1 + "|" + item.ExcessOverTime1);
                        if (item.CeilingOvertime != item.CeilingOvertime1)
                            updatePath.WriteLine(item.EmployeeId + "|" + item.CeilingOvertime + "|" + item.ExcessOverTime + "|" + item.CeilingOvertime1 + "|" + item.ExcessOverTime1);
                        //var employeeTimeSheet = employeeTimeSheetByYearMonth.SingleOrDefault(x => x.EmployeeId == item.EmployeeId);
                        //if (employeeTimeSheet != null)
                        //{
                        //    var updateEmployeeTimeSheet = _kscHrUnitOfWork.EmployeeTimeSheetRepository.GetById(employeeTimeSheet.Id);
                        //    updateEmployeeTimeSheet.CeilingOvertime = (int)item.CeilingOvertime;
                        //    updateEmployeeTimeSheet.ExcessOverTime = (int)item.ExcessOverTime;
                        //    updateEmployeeTimeSheet.ExcessOverTimeDuration = Utility.ConvertMinuteToDuration((int)item.ExcessOverTime);
                        //    updateEmployeeTimeSheet.UpdateDate = DateTime.Now;
                        //    updateEmployeeTimeSheet.UpdateUser = model.CurrentUser;
                        //}
                        //else
                        //{

                        //    var newRow = _mapper.Map<EmployeeTimeSheet>(item);
                        //    newRow.InsertDate = DateTime.Now;
                        //    newRow.InsertUser = model.CurrentUser;
                        //    employeeTimeSheetList.Add(newRow);
                        //}

                    }
                    newPath.Close();
                    updatePath.Close();

                }
                else
                {
                    throw new HRBusinessException(Validations.CloseDailyTimeSgeetId, Validations.CloseDailyTimeSgeet);
                }
            }
            catch (Exception ex)
            {
                newPath.Close();
                updatePath.Close();

                throw new HRBusinessException(Validations.RepetitiveId, ex.Message);
            }


            return result;
        }
        public TeamAndPersonelsFunctionalDetailsModel sumCeilingOvertime_AverageOverTimeNew(EmployeeEntryExitManagementInputModel inputModel, int teamWorkId)
        {
            TeamAndPersonelsFunctionalDetailsModel result = new TeamAndPersonelsFunctionalDetailsModel();
            var activeEmployeeTeamWroks = _kscHrUnitOfWork.EmployeeTeamWorkRepository.GetEmployeeTeamWorkByTeamWorkId(teamWorkId, inputModel.EntryExitDate);
            var miladiStartcurrentMonth = inputModel.EntryExitDate.GetPersianMonthStartAndEndDates().StartDate;
            var miladiEndDaycurrentMonth = inputModel.EntryExitDate.GetPersianMonthStartAndEndDates().EndDate;
            int yearMonth = inputModel.EntryExitDate.GetYearMonthShamsiByMiladiDate();
            var attendAbcenseItem = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository
                .GetEmployeeAttendAbsenceItemValidRecordAsNoTracking();
            var calendar = _kscHrUnitOfWork.WorkCalendarRepository.GetWorkCalendarByRangeDateAsNotracking(miladiStartcurrentMonth, miladiEndDaycurrentMonth);
            var rollCallDefinitionCeilingOvertime = GetRollCallDefinitionCeilingOvertime();
            var sumCeilingOvertime = (from item in attendAbcenseItem
                                      join tem in activeEmployeeTeamWroks on item.EmployeeId equals tem.EmployeeId
                                      join calnedar in calendar
                                      on item.WorkCalendarId equals calnedar.Id
                                      join rollCall in rollCallDefinitionCeilingOvertime on item.RollCallDefinitionId equals rollCall
                                      //group item by new { RollCallDefinitionId = item.RollCallDefinitionId } into newgroup
                                      select new
                                      {
                                          TimeDurationInMinute = item.TimeDurationInMinute.Value
                                      }).Sum(x => x.TimeDurationInMinute);
            var CountActiveEmployeeTeamWroks = activeEmployeeTeamWroks.Count();
            result.EmployeeOvertime = sumCeilingOvertime.ConvertMinuteToTime();
            result.TeamEmployeeisCount = CountActiveEmployeeTeamWroks.ToString();
            result.TeamAverageOvertime = (sumCeilingOvertime / CountActiveEmployeeTeamWroks).ConvertMinuteToTime();
            return result;
        }

        private IQueryable<int> GetRollCallDefinitionCeilingOvertime()
        {
            var rollCallDefinitionCeilingOvertime = _kscHrUnitOfWork.IncludedRollCallRepository.GetAllByRelated().AsNoTracking()
                 .Where(x => x.IncludedDefinitionId == EnumIncludedDefinition.AverageOverTime.Id
                 && (x.RollCallDefinition.RollCallCategoryId == EnumRollCallCategory.OverTime.Id
                  || x.RollCallDefinition.RollCallCategoryId == EnumRollCallCategory.DefaultOverTime.Id
                  || x.RollCallDefinition.RollCallCategoryId == EnumRollCallCategory.OverTimeInHoliday.Id
                  || x.RollCallDefinition.RollCallCategoryId == EnumRollCallCategory.OverTimeInUnOfficialHoliday.Id
                  )
                 ).Select(w => w.RollCallDefinitionId);
            return rollCallDefinitionCeilingOvertime;
        }
        private bool ValidAddOverTimeBeforeShift(InputAddOverTimeToAnalysisModel inputAddOverTimeToAnalysisModel, TimeSettingDataModel timeSettingDataModel)
        {
            bool isValid = true;
            var durationOverTime = (int)GetDuration(inputAddOverTimeToAnalysisModel.startTime, inputAddOverTimeToAnalysisModel.endTime, inputAddOverTimeToAnalysisModel.startDate, inputAddOverTimeToAnalysisModel.endDate);
            if (timeSettingDataModel.ShiftSettingFromShiftboard
                && durationOverTime < timeSettingDataModel.MinimumOverTimeBeforeShiftInMinut)
            {
                isValid = false;
            }
            return isValid;
        }
        public async Task<KscResult> AddMissionAttendAbcenceItemFromTextFile(List<PAR_ASSPY> data)
        {

            var result = new KscResult();
            try
            {
                foreach (var model in data)
                {
                    var employee = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeByPersonalNum(model.NUM_PRSN_EMPL);
                    var employeeWorkGoups = _kscHrUnitOfWork.EmployeeWorkGroupRepository.GetEmployeeWorkGroupInclouded()
                         .Where(a => a.EmployeeId == employee.Id).ToList();

                    var workGroupsIds = employeeWorkGoups.Select(a => a.WorkGroupId).ToList();
                    var DAT_STR_ASSPYString = int.Parse(model.DAT_STR_ASSPY);
                    var DAT_END_ASSPYString = int.Parse(model.DAT_END_ASSPY);
                    var workCalendars = _kscHrUnitOfWork.WorkCalendarRepository.GetAllQuerable()
                    .Where(a => a.DateKey >= DAT_STR_ASSPYString && a.DateKey < DAT_END_ASSPYString).ToListAsync().GetAwaiter().GetResult().ToList();

                    var workcalendarIds = workCalendars.Select(a => a.Id).ToList();
                    //var shiftBoards = _kscHrUnitOfWork.ShiftBoardRepository.GetShiftBoardByworkGoupIdsAndWorkCalendarIds(workGroupsIds, workcalendarIds).ToList();


                    var WorkGroupIds = employeeWorkGoups.Select(a => a.WorkGroupId).ToList();
                    var ShiftConceptDetails = _shiftConceptDetailService.GetShiftConceptDetailWithWOrkGroupIdAndWokCalendarIds(WorkGroupIds, workcalendarIds);

                    //var startDate = model.DAT_STR_ASSPY;//تاریخ شروع ماموریت
                    //var workCity = _kscHrUnitOfWork.EmployeeRepository.GetByIdAsync(employee.EmployeeId).GetAwaiter().GetResult();
                    var startTime = DateTime.Now;
                    var endTime = DateTime.Now;
                    var rollCallDefnition = _kscHrUnitOfWork.RollCallDefinitionRepository
                              .FirstOrDefault(a => a.Code == model.FK_ATABT);//کد حضور غیاب
                    if (rollCallDefnition == null)
                    {
                        result.AddError("", "کد حضور غیاب ندارد");
                        return result;
                    }

                    var items = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetItemsHolidayByworkcalendarIds(employee.Id, workcalendarIds);
                    foreach (var item in items)
                    {
                        var employeeEntryExitAttendAbsencesData = _kscHrUnitOfWork.EmployeeEntryExitAttendAbsenceRepository.Where(x => x.EmployeeAttendAbsenceItemId == item.Id);
                        foreach (var employeeEntryExitAttendAbsence in employeeEntryExitAttendAbsencesData)
                        {
                            _kscHrUnitOfWork.EmployeeEntryExitAttendAbsenceRepository.Delete(employeeEntryExitAttendAbsence);
                        }
                        _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.Delete(item);
                    }
                    List<EmployeeAttendAbsenceItem> listData = new List<EmployeeAttendAbsenceItem>();
                    foreach (var calendar in workCalendars)
                    {
                        var calendarId = calendar.Id;
                        var miladiDate = calendar.MiladiDateV1;
                        var workGroup = GetEmployeeWorkGroupByEmployeeIdDate(miladiDate, employeeWorkGoups);


                        var systemStatus = _kscHrUnitOfWork.WorkCalendarRepository.GetDailyTimeSheetStatus(calendarId).GetAwaiter().GetResult();
                        if (systemStatus == EnumSystemSequenceStatusDailyTimeSheet.InActiveForAllUser.Id)
                        {
                            continue;
                            //  throw new HRBusinessException(Validations.RepetitiveId,
                            //"کارکرد بسته شده است");
                        }
                        var workCalendar = workCalendars.First(a => a.Id == calendarId);



                        var shiftConceptDetailId = ShiftConceptDetails.FirstOrDefault(a => a.WorkCalendarId == workCalendar.Id && a.WorkGroupId == workGroup.WorkGroupId);

                        var getstartAndEndTimeShift = _kscHrUnitOfWork.EmployeeLongTermAbsenceRepository
                            .GetShiftStartEndTime(shiftConceptDetailId.Id, employee.WorkCityId.Value
                            , workGroup.WorkGroupId, calendarId).GetAwaiter().GetResult();
                        //مدت زمان شیف باید بدست بیاید
                        var addEmployeeAttendAbsenceItem = new EmployeeAttendAbsenceItem()
                        {
                            MissionId = model.ASSPY_ID,//کد شناسایی ماموریت
                            EmployeeId = employee.Id,
                            WorkTimeId = workGroup.WorkGroup.WorkTimeId,
                            WorkCalendarId = calendarId,
                            RollCallDefinitionId = rollCallDefnition.Id,
                            ShiftConceptDetailId = shiftConceptDetailId.Id,
                            IsManual = false,
                            StartTime = getstartAndEndTimeShift.Item1,
                            EndTime = getstartAndEndTimeShift.Item2,
                            TimeDuration = getstartAndEndTimeShift.Item3,
                            InsertDate = DateTime.Now,
                            InsertUser = model.COD_USR_ATABI,
                            ShiftConceptDetailIdInShiftBoard = shiftConceptDetailId.Id,
                            // TimeDurationInMinute = Utility.ConvertDurationToMinute(getstartAndEndTimeShift.Item3) ?? 00
                        };
                        //مدت زمان شیف باید بدست بیاید
                        listData.Add(addEmployeeAttendAbsenceItem);
                        //_kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.Add(addEmployeeAttendAbsenceItem);

                    }
                    _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.AddRange(listData);

                }
                await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new HRBusinessException(Validations.RepetitiveId, ex.Message);
            }
            return result;


        }
        public void GetworkCalendarsByWorkCityAndDomain(int? workCityId, string domain, List<WorkCalendarForAttendAbcenseAnalysis> workCalendars)
        {

            if (domain == "KSC" && workCalendars.Any(x => x.DateKey == 14040428) && workCityId != 1 && workCityId != 14 && workCityId != 9)
            {
                var item = workCalendars.First(x => x.DateKey == 14040428);
                SetWorkCalendar(item);

            }
            if (domain == "NIMID")
            {
                if (workCalendars.Any(x => x.DateKey == 14040522) && workCityId != 4 && workCityId != 7)
                {
                    var item = workCalendars.First(x => x.DateKey == 14040522);
                    SetWorkCalendar(item);
                }
                if (workCalendars.Any(x => x.DateKey == 14040501) && workCityId != 1)
                {
                    var item = workCalendars.First(x => x.DateKey == 14040501);
                    SetWorkCalendar(item);
                }
            }

        }
        public void SetWorkCalendar(WorkCalendarForAttendAbcenseAnalysis item)
        {
            item.WorkDayTypeId = EnumWorkDayType.NormalDay.Id;
            item.IsOfficialHoliday = false;
            item.IsUnOfficialHoliday = false;
            item.IsHoliday = false;
        }
        public async Task<FilterResult<EmployeeDontHaveAttendITemModel>> GetEmployeeDontHaveAttendItemModelReportByTeam(SearchReportEmployeeEntryExitModel Filter)
        {
            var result = new FilterResult<EmployeeDontHaveAttendITemModel>();
            try
            {
                var YearMonth = Utility.GetPersianMonth(Filter.YearMonth.ToEnglishNumbers());
                var enddate = YearMonth.EndDate;
                int persianMonth = System.DateTime.Now.GetPersianMonth();
                if (YearMonth.StartDate.GetPersianMonth() == persianMonth)
                    enddate = System.DateTime.Now.AddDays(-1).GetEndOfDay();
                string teamManager = Filter.CurrentUserName;
                if (Filter.IsOfficialAttend)
                {
                    teamManager = null;
                }
                int yearMonth = int.Parse(Filter.YearMonth.ToEnglishNumbers());
                var prc = await _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository
                    .GetSpGetEmployeeDontHaveAttendItemModelReportAsync(YearMonth.StartDate, enddate, Filter.StartTeamCode, Filter.EndTeamCode, Filter.PersonalNumber);
                if (Filter.NotShowEmployeeInMonthSheet) //افرادی که تایم-شیت ماهیانه دارند نمایش داده نشوند
                {
                    var EmployeeIdInMonth = _kscHrUnitOfWork.MonthTimeSheetRepository.GetMonthTimeSheetByYearMonthAsNoTracking(yearMonth).Select(x => x.EmployeeId).ToList();
                    prc = prc.Where(x => !EmployeeIdInMonth.Any(y => y == x.EmployeeId)).ToList();
                }
                var teamManagers = _kscHrUnitOfWork.ViewMisEmployeeSecurityRepository.GetTeamByWindowsUser(Filter.CurrentUserName);
                if (Filter.IsOfficialAttend)
                    teamManagers = _kscHrUnitOfWork.ViewMisEmployeeSecurityRepository.GetAllQueryable();
                else
                {
                    if (string.IsNullOrEmpty(Filter.StartTeamCode))
                    {
                        Filter.StartTeamCode = teamManagers.Min(x => x.TeamCode).ToString();
                    }
                    if (string.IsNullOrEmpty(Filter.EndTeamCode))
                    {
                        Filter.EndTeamCode = teamManagers.Max(x => x.TeamCode).ToString();
                    }
                }
                teamManagers = teamManagers.Where(x => x.TeamWorkIsActive && (x.DisplaySecurity == 1 || x.DisplaySecurity == 2));
                var resultTeamManagers = teamManagers.Select(x => new
                {
                    TeamCode = x.TeamCode,
                    FullName = x.FullName,
                    EmployeeNumber = x.EmployeeNumber
                });
                var activeDirectoryLdapKind = _config["AuthenticationOption:ActiveDirectoryLdapKind"];
                if (activeDirectoryLdapKind != "1")
                {
                    var employee = _kscHrUnitOfWork.EmployeeRepository.GetAllQueryable();
                    resultTeamManagers = resultTeamManagers.Join(employee, x => x.EmployeeNumber != null ? x.EmployeeNumber.Value.ToString() : "", y => y.EmployeeNumber, (x, y) => new
                    {
                        TeamCode = x.TeamCode,
                        FullName = y.Name + " " + y.Family,
                        EmployeeNumber = x.EmployeeNumber
                    });
                }
                var listTeamManagers = resultTeamManagers.ToList().GroupBy(x => x.TeamCode).Select(x => new { TeamCode = x.Key.ToString(), ManagerTeam = string.Join(",", x.Select(a => a.FullName).Distinct()), }).ToList();
                List<EmployeeDontHaveAttendITemModel> allPersonnelNotInAttendItemPrc = new List<EmployeeDontHaveAttendITemModel>();
                if (Filter.IsOfficialAttend)
                {
                    allPersonnelNotInAttendItemPrc = (from p in prc
                                                      join t in listTeamManagers on p.TeamCode equals t.TeamCode into r
                                                      from resultJoin in r.DefaultIfEmpty()
                                                      select (new EmployeeDontHaveAttendITemModel()
                                                      {
                                                          EmployeeId = p.EmployeeId,
                                                          TeamName = p.TeamWorkTitle,
                                                          TeamWorkCode = p.TeamCode,
                                                          WorkCalendarDate = p.MiladiDateV1,
                                                          Date = p.ShamsiDateV1,
                                                          FullName = p.Name + " " + p.Family,
                                                          PersonalNumber = p.EmployeeNumber,
                                                          ManagerTeam = resultJoin != null ? resultJoin.ManagerTeam : "",
                                                          TeamCode = p.TeamCode,
                                                          WorkGroupCode = p.WorkGroupCode,
                                                      })).ToList();
                }
                else
                {
                    allPersonnelNotInAttendItemPrc = (from p in prc
                                                      join t in listTeamManagers on p.TeamCode equals t.TeamCode
                                                      select (new EmployeeDontHaveAttendITemModel()
                                                      {
                                                          EmployeeId = p.EmployeeId,
                                                          TeamName = p.TeamWorkTitle,
                                                          TeamWorkCode = p.TeamCode,
                                                          WorkCalendarDate = p.MiladiDateV1,
                                                          Date = p.ShamsiDateV1,
                                                          FullName = p.Name + " " + p.Family,
                                                          PersonalNumber = p.EmployeeNumber,
                                                          ManagerTeam = t.ManagerTeam,
                                                          TeamCode = p.TeamCode,
                                                          WorkGroupCode = p.WorkGroupCode,
                                                      })).ToList();
                }

                var finalList = _FilterHandler.GetFilterResult<EmployeeDontHaveAttendITemModel>(allPersonnelNotInAttendItemPrc, Filter, "PersonalNumber");

                result = new FilterResult<EmployeeDontHaveAttendITemModel>()
                {
                    Data = finalList.Data.ToList(),
                    Total = finalList.Total
                };

            }
            catch (Exception ex)
            {

            }

            return result;


        }

    }
    public class overTime
    {
        public long EmployeeId { get; set; }
        public string Duration { get; set; }
    }
}
