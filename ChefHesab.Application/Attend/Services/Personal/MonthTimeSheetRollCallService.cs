using AutoMapper;
using DNTPersianUtils.Core;
using Ksc.HR.Appication.Interfaces.Personal;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.Personal.MonthTimeSheet;
using Ksc.HR.DTO.Personal.MonthTimeSheetRollCall;
using Ksc.HR.DTO.WorkFlow.Classes.Enumerations;
using Ksc.HR.DTO.WorkShift;
using Ksc.HR.DTO.WorkShift.IncludedDefinition;
using Ksc.HR.DTO.WorkShift.SacrificeOptionSetting;
using Ksc.HR.Share.General;
using Ksc.HR.Share.Model.RollCallDefinication;
using Ksc.HR.Share.WorkShift;
using KSC.Common;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using NetTopologySuite.Index.HPRtree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Services.Personal
{
    public class MonthTimeSheetRollCallService : IMonthTimeSheetRollCallService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;
        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;

        public MonthTimeSheetRollCallService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler filterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;
            _mapper = mapper;
            _FilterHandler = filterHandler;
        }

        public async Task<FilterResult<SearchTimeSheetRollCallModel>> GetMonthTimeSheetRollCallByKendoFilter(SearchTimeSheetRollCallModel Filter)
        {
            var employeemodel = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeWorkGroupWorkTimeByRelated().FirstOrDefault(x => x.Id == Filter.EmployeeId);
            if (employeemodel == null)
                return new FilterResult<SearchTimeSheetRollCallModel>();
            int yearMonth = Convert.ToInt32(Filter.YearMonth.ToEnglishNumbers());
            var MonthTimeSheetRollCallList = new List<SearchTimeSheetRollCallModel>();
            var misSalaryCode = _kscHrUnitOfWork.ViewMisSalaryCodeRepository.GetAllQueryable();
            //
            var rollCallDefinitionByCeilingOvertime = _kscHrUnitOfWork.IncludedRollCallRepository.GetActiveIncludedRollCallByIncludedIdAsNoTracking(EnumIncludedDefinition.MaximunOverTime.Id)
                       .Select(x => x.RollCallDefinitionId).ToList();
            //
            if (await _kscHrUnitOfWork.MonthTimeSheetRepository.AnyAsync(x => x.EmployeeId == employeemodel.Id && x.YearMonth == yearMonth))
            {
                var monthSheetmodel = _kscHrUnitOfWork.MonthTimeSheetRepository.FirstOrDefault(x => x.EmployeeId == employeemodel.Id && x.YearMonth == yearMonth);
                var queryMonthTimeSheetRollCall = _kscHrUnitOfWork.MonthTimeSheetRollCallRepository.GetMonthTimeSheetRollCall().Where(x => x.IsActive == Filter.IsActive && x.MonthTimeSheetId == monthSheetmodel.Id).ToList();
                queryMonthTimeSheetRollCall.ForEach(x =>
                {
                    var SalaryAccountCode = x.RollCallDefinition.RollCallSalaryCodes.FirstOrDefault(a => a.IsActive && a.EmploymentTypeCode == employeemodel.EmploymentTypeId)?.SalaryAccountCode;
                    var querymisSalaryCode = misSalaryCode.FirstOrDefault(x => x.SalaryAccountCode == SalaryAccountCode);
                    MonthTimeSheetRollCallList.Add(new SearchTimeSheetRollCallModel
                    {
                        RollCallDefinitionId = x.RollCallDefinitionId,
                        Code = x.RollCallDefinition?.Code,
                        Title = x.RollCallDefinition?.Title,
                        DayCountInDailyTimeSheet = x.DayCountInDailyTimeSheet,
                        Duration = x.DurationInMinut.ConvertDurationInHour(),
                        DurationInMinut = x.DurationInMinut,
                        AccountCode = SalaryAccountCode.ToString(),
                        AccountTitle = querymisSalaryCode?.SalaryAccountTitle,
                        UpdateDate = x.UpdateDate,
                        UpdateUser = x.UpdateUser,
                        RollCallDefinitionInCeilingOvertime = rollCallDefinitionByCeilingOvertime.Any(a => a == x.RollCallDefinitionId)
                    });
                });
                FilterResult<SearchTimeSheetRollCallModel> result = _FilterHandler.GetFilterResult<SearchTimeSheetRollCallModel>(MonthTimeSheetRollCallList.AsQueryable(), Filter, "Code");
                var modelResult = new FilterResult<SearchTimeSheetRollCallModel>
                {
                    Data = result.Data,
                    Total = result.Total
                };
                return modelResult;
            }
            else if (Filter.IsActive)
            {
                var workCalendarData = _kscHrUnitOfWork.WorkCalendarRepository.GetIncludedWorkCalendarWorkDayType().Where(x => x.YearMonthV1 == yearMonth).Select(x => new { x.Id, x.WorkDayTypeId, WorkDayTypeName = x.WorkDayType.Title, x.WorkDayType.IsHoliday }).ToList();
                var workDays = workCalendarData.GroupBy(x => new { x.WorkDayTypeId, x.WorkDayTypeName, x.IsHoliday }).Select(x => new { x.Key.WorkDayTypeId, x.Key.WorkDayTypeName, x.Key.IsHoliday, Count = x.Count() });
                if (!employeemodel.WorkGroupId.HasValue)
                    return new FilterResult<SearchTimeSheetRollCallModel>();
                var workTimeId = employeemodel.WorkGroup.WorkTimeId;
                ///برای روزکار ها نمایش داده شود 
                var roozkarCode = ((int)WorkTimeCodeEnums.RoozKar).ToString();
                if (employeemodel.WorkGroup.WorkTimeId.ToString() == roozkarCode)
                {
                    var rollCallWorkTimeDayTypes = _kscHrUnitOfWork.RollCallWorkTimeDayTypeRepository.GetIncludedRollCallWorkTimeDayTypeRollCallDefinitionSalaryCodes().Where(x => x.WorkTimeId == workTimeId && x.RollCallDefinition.RollCallConceptId == 1 && x.IsActive);
                    var enumRollCallDefinication = EnumRollCallDefinication.GetAll<EnumRollCallDefinication>();
                    var timeSheetSettingData = await _kscHrUnitOfWork.TimeSheetSettingRepository.GetTimeSheetSettingActiveAsync();
                    var WorkDayDuration = (int)timeSheetSettingData.WorkDayDuration.ConvertStringToTimeSpan().TotalMinutes;
                    // .ConvertMinToDaysHour();
                    foreach (var w in workDays.Where(x => !x.IsHoliday && x.WorkDayTypeId != EnumWorkDayType.NormalDay.Id))
                    {
                        var queryrollCallWorkTimeDayType = rollCallWorkTimeDayTypes.FirstOrDefault(x => x.WorkDayTypeId == w.WorkDayTypeId);
                        if (queryrollCallWorkTimeDayType != null)
                        {
                            var SalaryAccountCode = queryrollCallWorkTimeDayType.RollCallDefinition.RollCallSalaryCodes.FirstOrDefault(a => a.EmploymentTypeCode == employeemodel.EmploymentTypeId)?.SalaryAccountCode;
                            var querymisSalaryCode = misSalaryCode.FirstOrDefault(x => x.SalaryAccountCode == SalaryAccountCode);
                            var durationInMinut = w.Count * WorkDayDuration;
                            MonthTimeSheetRollCallList.Add(new SearchTimeSheetRollCallModel
                            {
                                RollCallDefinitionId = queryrollCallWorkTimeDayType.RollCallDefinitionId,
                                Code = queryrollCallWorkTimeDayType.RollCallDefinition?.Code,
                                Title = queryrollCallWorkTimeDayType.RollCallDefinition.Title,
                                DayCountInDailyTimeSheet = w.Count,
                                Duration = durationInMinut.ConvertMinuteToDuration(),
                                DurationInMinut = durationInMinut,
                                AccountCode = SalaryAccountCode.ToString(),
                                AccountTitle = querymisSalaryCode?.SalaryAccountTitle,
                            });
                        }
                    }
                    /// روز عادی کارکرد
                    var queryrollCallWorkTimeDayTypeNormalDay = rollCallWorkTimeDayTypes.FirstOrDefault(x => x.WorkDayTypeId == EnumWorkDayType.NormalDay.Id);

                    var SalaryAccountCodeNormalDay = queryrollCallWorkTimeDayTypeNormalDay.RollCallDefinition.RollCallSalaryCodes.FirstOrDefault(a => a.EmploymentTypeCode == employeemodel.EmploymentTypeId)?.SalaryAccountCode;
                    var querymisSalaryCodeNormalDay = misSalaryCode.FirstOrDefault(x => x.SalaryAccountCode == SalaryAccountCodeNormalDay);
                    int countKarkard = workDays.FirstOrDefault(x => !x.IsHoliday && x.WorkDayTypeId == EnumWorkDayType.NormalDay.Id).Count;
                    int durationInMinutKarkard = countKarkard * WorkDayDuration;
                    string attendAbsenceToleranceBySacrificePercentage = await _kscHrUnitOfWork.SacrificePercentageSettingRepository.GetAttendAbsenceToleranceBySacrificePercentage(employeemodel.SacrificePercentage ?? 0);
                    int durationInMinutSacrifice = countKarkard * attendAbsenceToleranceBySacrificePercentage.ConvertDurationToMinute() ?? 0;

                    MonthTimeSheetRollCallList.Add(new SearchTimeSheetRollCallModel
                    {
                        RollCallDefinitionId = EnumRollCallDefinication.Karkard.Id,
                        Code = EnumRollCallDefinication.Karkard.Id.ToString(),
                        Title = EnumRollCallDefinication.Karkard.Name,
                        DayCountInDailyTimeSheet = countKarkard,
                        Duration = durationInMinutKarkard.ConvertMinuteToDuration(),
                        DurationInMinut = durationInMinutKarkard,
                        EmployeeId = Filter.EmployeeId,
                        IsActive = true,
                        AccountCode = SalaryAccountCodeNormalDay.ToString(),
                        AccountTitle = querymisSalaryCodeNormalDay?.SalaryAccountTitle,
                    });
                    //اضافه کار جانبازی
                    if (employeemodel.SacrificeOptionSettingId == EnumSacrificeOptionSetting.SacrificeOverTime.Id)
                    {
                        MonthTimeSheetRollCallList.Add(new SearchTimeSheetRollCallModel
                        {
                            RollCallDefinitionId = EnumRollCallDefinication.janbaziExtraWork.Id,
                            Code = EnumRollCallDefinication.janbaziExtraWork.Id.ToString(),
                            Title = EnumRollCallDefinication.janbaziExtraWork.Name,
                            DayCountInDailyTimeSheet = countKarkard,
                            Duration = durationInMinutSacrifice.ConvertMinuteToDuration(),
                            DurationInMinut = durationInMinutSacrifice,
                            EmployeeId = Filter.EmployeeId,
                            IsActive = true,
                            AccountCode = SalaryAccountCodeNormalDay.ToString(),
                            AccountTitle = querymisSalaryCodeNormalDay?.SalaryAccountTitle,
                        });

                    }
                    //
                    // تعطیل کاری
                    var queryrollCallWorkTimeDayTypeHoliday = rollCallWorkTimeDayTypes.FirstOrDefault(x => x.WorkDayTypeId == EnumWorkDayType.OfficialHoliday.Id);

                    var SalaryAccountCodeHoliday = queryrollCallWorkTimeDayTypeHoliday.RollCallDefinition.RollCallSalaryCodes.FirstOrDefault(a => a.EmploymentTypeCode == employeemodel.EmploymentTypeId)?.SalaryAccountCode;
                    var querymisSalaryCodeHoliday = misSalaryCode.FirstOrDefault(x => x.SalaryAccountCode == SalaryAccountCodeHoliday);
                    var countHoliday = workDays.Where(x => x.IsHoliday).Sum(x => x.Count);
                    var durationInMinutHoliday = countHoliday * WorkDayDuration;

                    MonthTimeSheetRollCallList.Add(new SearchTimeSheetRollCallModel
                    {
                        RollCallDefinitionId = queryrollCallWorkTimeDayTypeHoliday.RollCallDefinitionId,
                        Code = queryrollCallWorkTimeDayTypeHoliday.RollCallDefinition.Code,
                        Title = queryrollCallWorkTimeDayTypeHoliday.RollCallDefinition.Title,
                        DayCountInDailyTimeSheet = countHoliday,
                        Duration = durationInMinutHoliday.ConvertMinuteToDuration(),
                        DurationInMinut = durationInMinutHoliday,
                        EmployeeId = Filter.EmployeeId,
                        IsActive = true,
                        AccountCode = SalaryAccountCodeHoliday.ToString(),
                        AccountTitle = querymisSalaryCodeHoliday?.SalaryAccountTitle,
                    });
                    //اضافه کاری
                    MonthTimeSheetRollCallList.Add(new SearchTimeSheetRollCallModel
                    {
                        RollCallDefinitionId = EnumRollCallDefinication.ForcedOverTime.Id,
                        Code = EnumRollCallDefinication.ForcedOverTime.Id.ToString(),
                        Title = EnumRollCallDefinication.ForcedOverTime.Name,
                        DayCountInDailyTimeSheet = 0,
                        Duration = timeSheetSettingData.ForcedOverTimeBasic,
                        DurationInMinut = timeSheetSettingData.ForcedOverTimeBasic.ConvertDurationToMinute().Value,
                        EmployeeId = Filter.EmployeeId,
                        IsActive = true,
                        AccountCode = SalaryAccountCodeHoliday.ToString(),
                        AccountTitle = querymisSalaryCodeHoliday?.SalaryAccountTitle,
                    });
                    // ماموریت
                    var missons = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetEmployeeAttendAbsenceItemByEmployeeIdWorkCalendarIdAsNoTracking(employeemodel.Id, 0).Where(x => x.MissionId != null && x.WorkCalendar.YearMonthV1 == yearMonth).GroupBy(x => new { x.RollCallDefinition.Id, x.RollCallDefinition.Title, x.TimeDuration }).Select(x => new { x.Key.Id, x.Key.Title, x.Key.TimeDuration, Count = x.Count() }).ToList();
                    foreach (var item in missons)
                    {
                        var SalaryAccountCodeMission = _kscHrUnitOfWork.RollCallSalaryCodeRepository.FirstOrDefault(a => a.RollCallDefinitionId == item.Id && a.EmploymentTypeCode == employeemodel.EmploymentTypeId)?.SalaryAccountCode;
                        var querymisSalaryCodeMission = misSalaryCode.FirstOrDefault(x => x.SalaryAccountCode == SalaryAccountCodeNormalDay);
                        MonthTimeSheetRollCallList.Add(new SearchTimeSheetRollCallModel
                        {
                            RollCallDefinitionId = item.Id,
                            Code = item.Id.ToString(),
                            Title = item.Title,
                            DayCountInDailyTimeSheet = item.Count,
                            Duration = item.TimeDuration,
                            DurationInMinut = item.TimeDuration.ConvertDurationToMinute().Value,
                            EmployeeId = Filter.EmployeeId,
                            IsActive = true,
                            AccountCode = SalaryAccountCodeMission.ToString(),
                            AccountTitle = querymisSalaryCodeMission?.SalaryAccountTitle,
                        });
                    }
                }
                foreach (var item in MonthTimeSheetRollCallList)
                {
                    item.RollCallDefinitionInCeilingOvertime = rollCallDefinitionByCeilingOvertime.Any(a => a == item.RollCallDefinitionId);
                }
                FilterResult<SearchTimeSheetRollCallModel> result = _FilterHandler.GetFilterResult<SearchTimeSheetRollCallModel>(MonthTimeSheetRollCallList.AsQueryable(), Filter, "Code");
                var modelResult = new FilterResult<SearchTimeSheetRollCallModel>
                {
                    Data = result.Data,
                    Total = result.Total
                };
                return modelResult;
            }
            return new FilterResult<SearchTimeSheetRollCallModel>();
        }

    }
}
