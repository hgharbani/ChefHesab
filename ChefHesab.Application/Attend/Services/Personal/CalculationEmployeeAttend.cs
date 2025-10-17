using AutoMapper;
using DNTPersianUtils.Core;
using Ksc.Hr.Domain.Shared;
using Ksc.HR.Appication.Interfaces.Personal;
using Ksc.HR.Appication.Interfaces.WorkShift;
using Ksc.HR.Application.Interfaces;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem;
using Ksc.HR.DTO.Personal.EmployeeEntryExit;
using Ksc.HR.DTO.WorkShift.RollCallConcept;
using Ksc.HR.DTO.WorkShift.RollCallDefinication;
using Ksc.HR.Resources.Messages;
using Ksc.HR.Share.General;
using Ksc.HR.Share.Model.AnalysisType;
using Ksc.HR.Share.Model.RollCallDefinication;
using Ksc.HR.Share.Model.ShiftConcept;
using Ksc.HR.Share.Model.TimeShiftSetting;
using Ksc.HR.Share.Model.WorkCalendar;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Services.Personal
{
    public class CalculationEmployeeAttend
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;
        // private readonly IEmployeeAttendAbsenceItemRepository _EmployeeAttendAbsenceItemRepository;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;
        private readonly IEmployeeEntryExitService _employeeEntryExitService;
        private readonly IRollCallDefinitionService _rollCallDefinitionService;
        private readonly IShiftConceptDetailService _shiftConceptDetailService;
        private readonly IStepper_ProcedureService _procedureService;

        public CalculationEmployeeAttend(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler, IEmployeeEntryExitService employeeEntryExitService, IRollCallDefinitionService rollCallDefinitionService, IShiftConceptDetailService shiftConceptDetailService, IStepper_ProcedureService procedureService)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;
            _mapper = mapper;
            _FilterHandler = FilterHandler;
            _employeeEntryExitService = employeeEntryExitService;
            _rollCallDefinitionService = rollCallDefinitionService;
            _shiftConceptDetailService = shiftConceptDetailService;
            _procedureService = procedureService;
        }

        public async Task<FilterResult<EmployeeAttendAbsenceAnalysisModel>> GetEmployeeAttendAbsenceAnalysis(
    EmployeeAttendAbsenceAnalysisInputModel inputModel)
        {
            var analysisResult = await GetDataEmployeeAttendAbsenceAnalysis(inputModel);
            var attendanceItems = analysisResult.EmployeeAttendAbsenceAnalysisModel;

            if (inputModel.IsOfficialAttend && attendanceItems.Any())
            {
                ApplyOfficialAttendanceRules(attendanceItems.First());
            }

            if (!analysisResult.HasItem)
            {
                //VaccinationCheck(analysisResult);
            }

            ApplyShiftSettings(attendanceItems, analysisResult.TimeSettingDataModel);
            FilterOverTimeItems(attendanceItems, inputModel, analysisResult.TimeSettingDataModel);

            return new FilterResult<EmployeeAttendAbsenceAnalysisModel>
            {
                Data = attendanceItems,
                Total = attendanceItems.Count
            };
        }

        public async Task<AnalysisAttenAbcenseResultModel> GetDataEmployeeAttendAbsenceAnalysis(
    EmployeeAttendAbsenceAnalysisInputModel inputModel)
        {
            var analysisResultModel = InitializeResultModel();
            var date = inputModel.Date.ToGregorianDateTime().Value.Date;
            #region تایید کارکرد شده اس
            var attendAbsenceItems = await GetValidAttendAbsenceItemsAsync(inputModel);
            if (attendAbsenceItems.Any())
            {
                analysisResultModel.EmployeeAttendAbsenceAnalysisModel = attendAbsenceItems;
                analysisResultModel.HasItem = true;
                return analysisResultModel;
            }
            #endregion

            // 2. دریافت اطلاعات پایه
            var employee = await GetEmployeeData(inputModel.EmployeeId);
            if (employee == null) return analysisResultModel;

            // 3. آماده سازی تنظیمات زمانی
            var timeSettings = await PrepareTimeSettings(inputModel, employee, date);
            analysisResultModel.TimeSettingDataModel = timeSettings;

            // 4. دریافت داده های حضور و غیاب
            var attendanceItems = await GetAttendanceItems(inputModel, employee);
            if (!attendanceItems.Any())
            {
                CheckVaccination(analysisResultModel, employee, timeSettings, date);
                return analysisResultModel;
            }

            // 5. تنظیم متغیرهای محاسباتی
            SetCalculationVariables(analysisResultModel, employee, date);



            // 1. بررسی پیوستگی شیفت قبلی
            //if (ShouldCheckShiftContinuity(timeSettingDataModel, employeeEntryExitYesterdayToTomorrow))
            //{
            //    await ProcessContinuousShiftAsync(inputModel, employeeEntryExitYesterdayToTomorrow,
            //        timeSettingDataModel, workCalendars, timeShiftSettingByWorkCityIdModel);
            //}

            //// 2. پردازش رکوردهای امروز
            //var entryExitResult = ProcessTodayRecords(employeeEntryExitYesterdayToTomorrow, timeSettingDataModel);

            //// 3. بررسی پیوستگی به شیفت بعدی
            //var (shouldAddTomorrowExit, exitTime, exitDate) = CheckNextShiftContinuity(
            //    employeeEntryExitYesterdayToTomorrow, timeSettingDataModel, inputModel);

            //if (shouldAddTomorrowExit)
            //{
            //    AddTomorrowExitRecord(ref entryExitResult, employeeEntryExitYesterdayToTomorrow,
            //        timeSettingDataModel, exitTime, exitDate, inputModel);
            //}

            //// 4. پردازش نهایی رکوردها
            //entryExitResult = FinalizeEntryExitRecords(entryExitResult, employeeEntryExitYesterdayToTomorrow,
            //    timeSettingDataModel, employeeEntryExitAttendAbsence);






            return analysisResultModel;

        }

        private void SetCalculationVariables(
    AnalysisAttenAbcenseResultModel result,
    Employee employee,
    DateTime date)
        {
            var conditionalAbsence = _kscHrUnitOfWork.EmployeeConditionalAbsenceRepository
                .GetEmployeeConditionalAbsenceForTimeSheetAnalys(employee.Id, date);

            if (conditionalAbsence != null && !conditionalAbsence.HasForcedOvertime)
            {
                result.InvalidForcedOvertime = true;
            }
        }

        private async Task<Employee> GetEmployeeData(int employeeId)
        {
            return await _kscHrUnitOfWork.EmployeeRepository
                .GetEmployeeByEmployeeId(employeeId);
        }

        private async Task<TimeSettingDataModel> PrepareTimeSettings(
            EmployeeAttendAbsenceAnalysisInputModel input,
            Employee employee,
            DateTime date)
        {
            var timeShiftSettings = _kscHrUnitOfWork.TimeShiftSettingRepository
                .GetTimeShiftSettingByWorkCityId(employee.WorkCityId.Value).ToList();

            var workCalendars = _kscHrUnitOfWork.WorkCalendarRepository
                .GetWorkCalendarForAttendAbcenseAnalysis(date.AddDays(-1), date.AddDays(1), employee.WorkCityId).ToList();

            var settingModel = new TimeShiftDateTimeSettingModel
            {
                employeeId = input.EmployeeId,
                employeeNumber = employee.EmployeeNumber,
                shiftConceptDetailId = input.ShiftConceptDetailId,
                workGroupId = input.WorkGroupId,
                date = date,
                timeShiftSettingByWorkCityIdModel = timeShiftSettings,
                workCalendars = workCalendars,
                FloatTimeSettingId = employee.FloatTimeSettingId,
                WorkCalendarId = input.WorkCalendarId,
                IsOfficialAttendForOverTime = input.IsOfficialAttendForOverTime
            };

            return await GetTimeShiftDateTimeSetting(settingModel);
        }

        private async Task<List<EmployeeAttendAbsenceAnalysisModel>> GetAttendanceItems(
            EmployeeAttendAbsenceAnalysisInputModel input,
            Employee employee)
        {
            var query = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository
                .GetEmployeeAttendAbsenceItemByEmployeeIdWorkCalendarIdAsNoTracking(
                    input.EmployeeId,
                    input.WorkCalendarId)
                .Where(x => !x.InvalidRecord);

            return await query
                .Select(item => new EmployeeAttendAbsenceAnalysisModel
                {
                    EmployeeAttendAbsenceItemId = item.Id,
                    EmployeeId = item.EmployeeId,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    Duration = CalculateDuration(item),
                    RollCallDefinicationInItemModel = new RollCallDefinicationInItemModel
                    {
                        Id = item.RollCallDefinitionId,
                        Code = item.RollCallDefinition.Code,
                        Title = item.RollCallDefinition.Title
                    },
                    RollCallConceptId = item.RollCallDefinition.RollCallConceptId,
                    DeleteIsValid = false,
                    ModifyIsValid = false
                })
                .ToListAsync();
        }

        private void CheckVaccination(
            AnalysisAttenAbcenseResultModel result,
            Employee employee,
            TimeSettingDataModel timeSettings, DateTime date)
        {
            result.IsValidUnVaccine = employee.IsValidUnVaccine;
            result.UnVaccineValidDate = employee.UnVaccineValidDate;
            result.VaccineDosage = employee.VaccineDosage;

            var allRollCall = _rollCallDefinitionService.GetRollCallsForEmployeeAttendAbsence(
                timeSettings, employee.EmploymentTypeId, date);

            result.RollCallDefinitionIdForVaccinationCheck = allRollCall.RollCallToday
                .Where(x => x.VaccinationCheck)
                .Select(x => x.RollCallDefinitionId)
                .ToList();
        }


        private AnalysisAttenAbcenseResultModel InitializeResultModel()
        {
            return new AnalysisAttenAbcenseResultModel
            {
                EmployeeAttendAbsenceAnalysisModel = new List<EmployeeAttendAbsenceAnalysisModel>(),
                RollCallDefinitionIdForVaccinationCheck = new List<int>()
            };
        }
        private async Task<List<EmployeeAttendAbsenceAnalysisModel>> GetValidAttendAbsenceItemsAsync(
    EmployeeAttendAbsenceAnalysisInputModel inputModel)
        {
            var query = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository
                .GetEmployeeAttendAbsenceItemByEmployeeIdWorkCalendarIdAsNoTracking(
                    inputModel.EmployeeId,
                    inputModel.WorkCalendarId
                )
                .Where(x => !x.InvalidRecord);

            return await query
                .Select(item => MapToAnalysisModel(item))
                .ToListAsync();
        }
        private EmployeeAttendAbsenceAnalysisModel MapToAnalysisModel(EmployeeAttendAbsenceItem item)
        {
            return new EmployeeAttendAbsenceAnalysisModel
            {
                EmployeeAttendAbsenceItemId = item.Id,
                EmployeeId = item.EmployeeId,
                StartTime = item.StartTime,
                EndTime = item.EndTime,
                Duration = CalculateDuration(item),
                RollCallDefinicationInItemModel = MapRollCallDefinition(item.RollCallDefinition),
                RollCallConceptId = item.RollCallDefinition?.RollCallConceptId ?? 0,
                DeleteIsValid = false,
                ModifyIsValid = false
            };
        }
        private string CalculateDuration(EmployeeAttendAbsenceItem item)
        {
            if (item.IncreasedTimeDuration == null || item.TimeDurationInMinute == null)
                return item.TimeDuration;

            return Utility.ConvertMinuteIn24ToDuration(
                item.TimeDurationInMinute.Value + item.IncreasedTimeDuration.Value
            );
        }

        private RollCallDefinicationInItemModel? MapRollCallDefinition(RollCallDefinition? definition)
        {
            if (definition == null)
                return null;

            return new RollCallDefinicationInItemModel
            {
                Id = definition.Id,
                Code = definition.Code,
                Title = definition.Title
            };
        }

        /// <summary>
        ///  اعمال قوانین حضور رسمی
        /// </summary>
        /// <param name="item"></param>
        private void ApplyOfficialAttendanceRules(EmployeeAttendAbsenceAnalysisModel item)
        {
            if (item.EmployeeAttendAbsenceItemId == 0 &&
                item.RollCallConceptId == EnumRollCallConcept.Absence.Id)
            {
                item.ModifyIsValid = true;
            }
        }

        /// <summary>
        /// تنظیمات شیفت کاری
        /// </summary>
        /// <param name="items"></param>
        /// <param name="timeSettings"></param>
        private void ApplyShiftSettings(
    List<EmployeeAttendAbsenceAnalysisModel> items,
    TimeSettingDataModel timeSettings)
        {
            if (timeSettings == null) return;

            foreach (var item in items)
            {
                item.IsShiftStart = IsMatchingShiftTime(item.StartTime, item.StartDate,
                                      timeSettings.ShiftStartTimeToTimeSpan, timeSettings.ShiftStartDate);

                item.IsShiftEnd = IsMatchingShiftTime(item.EndTime, item.EndDate,
                                    timeSettings.ShiftEndTimeToTimeSpan, timeSettings.ShiftEndDate);

                item.ForcedOverTime = timeSettings.ForcedOverTime;
                item.TotalWorkHourInWeek = timeSettings.TotalWorkHourInWeek;
                item.YearMonth = timeSettings.YearMonth;
                item.ShiftSettingFromShiftboard = timeSettings.ShiftSettingFromShiftboard;
            }
        }

        private bool IsMatchingShiftTime(string time, DateTime? date, TimeSpan shiftTime, DateTime shiftDate)
        {
            return time.ConvertStringToTimeSpan() == shiftTime && date == shiftDate;
        }

        /// <summary>
        /// فیلتر کردن اضافه‌کارها
        /// </summary>
        /// <param name="items"></param>
        /// <param name="input"></param>
        /// <param name="timeSettings"></param>
        private void FilterOverTimeItems(
    List<EmployeeAttendAbsenceAnalysisModel> items,
    EmployeeAttendAbsenceAnalysisInputModel input,
    TimeSettingDataModel timeSettings)
        {
            if (ShouldExcludeOverTime(input, timeSettings))
            {
                var trainingOverTimeIds = new[] {
            EnumRollCallDefinication.OnlineTrainingExtraWork.Id,
            EnumRollCallDefinication.TrainingOverTime.Id,
            EnumRollCallDefinication.TrainingOverTimeInHoliday.Id
        };

                items.RemoveAll(x =>
                    x.RollCallConceptId == EnumRollCallConcept.OverTime.Id &&
                    !trainingOverTimeIds.Contains(x.RollCallDefinicationInItemModel.Id)
                );
            }
        }

        private bool ShouldExcludeOverTime(
            EmployeeAttendAbsenceAnalysisInputModel input,
            TimeSettingDataModel timeSettings)
        {
            return input.AnalysisType != EnumAnalysisType.Keshik.Id &&
                   !input.IsOfficialAttendForOverTime &&
                   timeSettings?.InvalidOverTime == true;
        }


        public async Task<TimeSettingDataModel> GetTimeShiftDateTimeSetting(TimeShiftDateTimeSettingModel inputModel)
        {
            try
            {
                // 1. دریافت داده‌های پایه
                var (workCalendarToday, workCalendarTomorrow, workGroup, shiftConceptDetail, timeSheetSetting) =
                    await GetBaseDataAsync(inputModel);

                // 2. محاسبه تنظیمات شیفت اصلی
                var mainShiftSettings = await GetMainShiftSettingsAsync(inputModel, workGroup, shiftConceptDetail, workCalendarToday);

                // 3. محاسبه تنظیمات موقت (در صورت وجود)
                var tempShiftSettings = await GetTemporaryShiftSettingsAsync(inputModel, workGroup, shiftConceptDetail,
                    workCalendarToday, workCalendarTomorrow, mainShiftSettings);

                // 4. محاسبه محدوده‌های زمانی
                var timeRanges = CalculateTimeRanges(inputModel, shiftConceptDetail, mainShiftSettings);

                // 5. بررسی شرایط خاص (تعطیلات، اضافه‌کار و ...)
                var specialConditions = await CheckSpecialConditionsAsync(inputModel, workCalendarToday,
                    mainShiftSettings, tempShiftSettings);

                // 6. ساخت مدل نهایی
                return BuildTimeSettingDataModel(inputModel, workCalendarToday, workCalendarTomorrow,
                    mainShiftSettings, tempShiftSettings, timeRanges, specialConditions, timeSheetSetting);
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, "خطا در محاسبه تنظیمات زمانی شیفت");
                throw;
            }
        }
        /// <summary>
        /// دریافت داده های اولیه
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<(
    WorkCalendarForAttendAbcenseAnalysis workCalendarToday,
    WorkCalendarForAttendAbcenseAnalysis workCalendarTomorrow,
    WorkGroup workGroup,
    ShiftConceptDetail shiftConceptDetail,
    TimeSheetSetting timeSheetSetting
)> GetBaseDataAsync(TimeShiftDateTimeSettingModel input)
        {
            var tomorrow = input.date.AddDays(1);

            return (
                workCalendarToday: input.workCalendars.First(x => x.Date == input.date),
                workCalendarTomorrow: input.workCalendars.First(x => x.Date == tomorrow),
                workGroup: await _kscHrUnitOfWork.WorkGroupRepository.GetWorkGroupOutInclud(input.workGroupId),
                shiftConceptDetail: await _kscHrUnitOfWork.ShiftConceptDetailRepository
                    .GetShiftConceptDetailOutIncludedAsNoTracking(input.shiftConceptDetailId),
                timeSheetSetting: await _kscHrUnitOfWork.TimeSheetSettingRepository.GetTimeSheetSettingActiveAsync()
            );
        }

        /// <summary>
        /// محاسبه تنظیمات شیفت اصلی
        /// </summary>
        /// <param name="input"></param>
        /// <param name="workGroup"></param>
        /// <param name="shiftConceptDetail"></param>
        /// <param name="workCalendarToday"></param>
        /// <returns></returns>
        /// <exception cref="HRBusinessException"></exception>
        private async Task<TimeShiftSettingByWorkCityIdModel> GetMainShiftSettingsAsync(
        TimeShiftDateTimeSettingModel input,
        WorkGroup workGroup,
        ShiftConceptDetail shiftConceptDetail,
        WorkCalendarForAttendAbcenseAnalysis workCalendarToday)
        {
            var mainShift = input.timeShiftSettingByWorkCityIdModel
                .FirstOrDefault(x => x.WorktimeId == workGroup.WorkTimeId
                                  && x.ShiftConceptId == shiftConceptDetail.ShiftConceptId
                                  && !x.IsTemporaryTime
                                  && x.ValidityStartDate.Value.Date <= input.date.Date
                                  && x.ValidityEndDate.Value.Date >= input.date.Date);

            return mainShift ?? throw new HRBusinessException(Validations.NotFoundId, "تنظیمات شیفت یافت نشد");
        }

        /// <summary>
        /// 3. محاسبه تنظیمات موقت
        /// </summary>
        /// <param name="input"></param>
        /// <param name="workGroup"></param>
        /// <param name="shiftConceptDetail"></param>
        /// <param name="workCalendarToday"></param>
        /// <param name="workCalendarTomorrow"></param>
        /// <param name="mainShift"></param>
        /// <returns></returns>
        private async Task<TemporaryShiftSettings> GetTemporaryShiftSettingsAsync(
    TimeShiftDateTimeSettingModel input,
    WorkGroup workGroup,
    ShiftConceptDetail shiftConceptDetail,
    WorkCalendarForAttendAbcenseAnalysis workCalendarToday,
    WorkCalendarForAttendAbcenseAnalysis workCalendarTomorrow,
    TimeShiftSettingByWorkCityIdModel mainShift)
        {
            bool shouldCheckTempShift = shiftConceptDetail.ShiftConceptId == EnumShiftConcept.Night.Id ? !workCalendarTomorrow.IsHoliday : !workCalendarToday.IsHoliday;

            if (!shouldCheckTempShift) return null;

            var tempShift = input.timeShiftSettingByWorkCityIdModel
                .FirstOrDefault(x => x.WorktimeId == workGroup.WorkTimeId
                                  && x.ShiftConceptId == shiftConceptDetail.ShiftConceptId
                                  && x.IsTemporaryTime
                                  && x.ValidityStartDate.Value.Date <= input.date.Date
                                  && x.ValidityEndDate.Value.Date >= input.date.Date);

            if (tempShift == null) return null;

            return new TemporaryShiftSettings
            {
                ShiftStartTime = tempShift.ShiftStartTime,
                ShiftEndTime = tempShift.ShiftEndtTime,
                RollCallDefinitionStartShift = tempShift.TemporaryRollCallDefinitionStartShift,
                RollCallDefinitionEndShift = tempShift.TemporaryRollCallDefinitionEndShift,
                BreastfeddingToleranceTime = tempShift.BreastfeddingToleranceTime,
                CheckedEmployeeValidOverTime = tempShift.CheckedEmployeeValidOverTime
            };
        }

        /// <summary>
        /// محاسبه محدوده‌های زمانی
        /// </summary>
        /// <param name="input"></param>
        /// <param name="shiftConceptDetail"></param>
        /// <param name="mainShift"></param>
        /// <returns></returns>
        private TimeRanges CalculateTimeRanges(
    TimeShiftDateTimeSettingModel input,
    ShiftConceptDetail shiftConceptDetail,
    TimeShiftSettingByWorkCityIdModel mainShift)
        {
            var shiftStartTime = mainShift.ShiftStartTime.ConvertStringToTimeSpan();
            var shiftEndTime = mainShift.ShiftEndtTime.ConvertStringToTimeSpan();

            return new TimeRanges
            {
                TimeBeforeShiftStart = Utility.GetTimeBeforeShiftStart(
                    mainShift.ShiftStartTime,
                    shiftConceptDetail.DurationTimeBeforeShiftStartTime),

                TimeAfterShiftEnd = Utility.GetTimeAfterShiftEnd(
                    mainShift.ShiftEndtTime,
                    shiftConceptDetail.DurationTimeAfterShiftEndTime),

                ShiftStartWithTolerance = mainShift.ToleranceShiftStartTime.HasValue
                    ? shiftStartTime.Add(TimeSpan.FromMinutes(mainShift.ToleranceShiftStartTime.Value)).ToString(@"hh\:mm")
                    : null,

                ShiftEndWithTolerance = mainShift.ToleranceShiftEndTime.HasValue
                    ? Utility.GetTimeBeforeShiftStart(mainShift.ShiftEndtTime,
                        Utility.ConvertMinuteToTime(mainShift.ToleranceShiftEndTime.Value))
                : null
            };
        }

        /// <summary>
        /// بررسی شرایط خاص
        /// </summary>
        /// <param name="input"></param>
        /// <param name="workCalendarToday"></param>
        /// <param name="mainShift"></param>
        /// <param name="tempShift"></param>
        /// <returns></returns>
        private async Task<SpecialConditions> CheckSpecialConditionsAsync(
    TimeShiftDateTimeSettingModel input,
    WorkCalendarForAttendAbcenseAnalysis workCalendarToday,
    TimeShiftSettingByWorkCityIdModel mainShift,
    TemporaryShiftSettings tempShift)
        {
            var conditions = new SpecialConditions
            {
                IsRestShift = await _kscHrUnitOfWork.TimeShiftSettingRepository.IsRestShiftAsync(
                    mainShift.ShiftSettingFromShiftboard,
                    mainShift.OfficialUnOfficialHolidayFromWorkCalendar,
                    mainShift.ShiftConceptIsRest,
                    workCalendarToday.IsHoliday,
                    mainShift.WorkCompanySettingId,
                    workCalendarToday.DayNumber),

                IsHoliday = workCalendarToday.IsHoliday,
                IsUnOfficialHoliday = workCalendarToday.IsUnOfficialHoliday
            };

            if (tempShift?.CheckedEmployeeValidOverTime == true && !input.IsOfficialAttendForOverTime)
            {
                conditions.InvalidOverTime = !_kscHrUnitOfWork.EmployeeValidOverTimeRepository
                    .ValidOverTimeByEmployeeId(input.employeeId, input.WorkCalendarId);
            }

            return conditions;
        }


        /// <summary>
        /// 6. ساخت مدل نهایی
        /// </summary>
        /// <param name="input"></param>
        /// <param name="workCalendarToday"></param>
        /// <param name="workCalendarTomorrow"></param>
        /// <param name="mainShift"></param>
        /// <param name="tempShift"></param>
        /// <param name="timeRanges"></param>
        /// <param name="specialConditions"></param>
        /// <param name="timeSheetSetting"></param>
        /// <returns></returns>
        private TimeSettingDataModel BuildTimeSettingDataModel(
    TimeShiftDateTimeSettingModel input,
    WorkCalendarForAttendAbcenseAnalysis workCalendarToday,
    WorkCalendarForAttendAbcenseAnalysis workCalendarTomorrow,
    TimeShiftSettingByWorkCityIdModel mainShift,
    TemporaryShiftSettings tempShift,
    TimeRanges timeRanges,
    SpecialConditions specialConditions,
    TimeSheetSetting timeSheetSetting)
        {
            return new TimeSettingDataModel
            {
                // تنظیمات اصلی
                ShiftStartTime = mainShift.ShiftStartTime,
                ShiftEndTime = mainShift.ShiftEndtTime,
                ShiftStartTimeToTimeSpan = mainShift.ShiftStartTime.ConvertStringToTimeSpan(),
                ShiftEndTimeToTimeSpan = mainShift.ShiftEndtTime.ConvertStringToTimeSpan(),

                // محدوده‌های زمانی
                TimeBeforeShiftStartTime = timeRanges.TimeBeforeShiftStart,
                TimeAfterShiftEndTime = timeRanges.TimeAfterShiftEnd,
                ShiftStartTimeWithTolerance = timeRanges.ShiftStartWithTolerance,
                ShiftEndTimeWithTolerance = timeRanges.ShiftEndWithTolerance,

                // تنظیمات موقت
                IsTemporaryTime = tempShift != null,
                TemporaryShiftStartTime = tempShift?.ShiftStartTime,
                TemporaryShiftEndTime = tempShift?.ShiftEndTime,

                // شرایط خاص
                IsRestShift = specialConditions.IsRestShift,
                IsHoliday = specialConditions.IsHoliday,
                IsUnOfficialHoliday = specialConditions.IsUnOfficialHoliday,
                InvalidOverTime = specialConditions.InvalidOverTime,

                // سایر تنظیمات
                TotalWorkHourInDay = mainShift.TotalWorkHourInDay,
                ForcedOverTime = mainShift.ForcedOverTime,
                YearMonth = workCalendarToday.YearMonth,
                WorkCalendarIdToday = workCalendarToday.WorkCalendarId
            };
        }

        /// <summary>
        /// 1. بررسی پیوستگی شیفت قبلی
        /// </summary>
        /// <param name="timeSettings"></param>
        /// <param name="entryExitData"></param>
        /// <returns></returns>
        private bool ShouldCheckShiftContinuity(
    TimeSettingDataModel timeSettings,
    EmployeeEntryExitYesterdayToTomorrowModel entryExitData)
        {
            return timeSettings.ShiftSettingFromShiftboard
                   && timeSettings.ShiftCondeptId == EnumShiftConcept.Morning.Id
                   && entryExitData.YesterdayList.Any();
        }

        private async Task ProcessContinuousShiftAsync(
            EmployeeAttendAbsenceAnalysisInputModel input,
            EmployeeEntryExitYesterdayToTomorrowModel entryExitData,
            TimeSettingDataModel timeSettings,
            List<WorkCalendar> workCalendars,
            List<TimeShiftSettingByWorkCityIdModel> shiftSettings)
        {
            var lastYesterday = entryExitData.YesterdayList.OrderBy(x => x.EntryTime).Last();
            var firstToday = entryExitData.TodayList.OrderBy(x => x.EntryTime).First();

            if (lastYesterday.ExitTime == null && firstToday.EntryTime == null && firstToday.ExitTime != null)
            {
                //if (firstToday.ExitTimeToTimeSpan > timeSettings.ShiftStartTimeWithToleranceToTimeSpan)
                //{
                //    await HandleContinuousShiftCaseAsync(input, entryExitData, timeSettings,
                //        workCalendars, shiftSettings, lastYesterday, firstToday);
                //}
            }
        }

        private List<EmployeeEntryExitViewModel> ProcessTodayRecords(
    EmployeeEntryExitYesterdayToTomorrowModel entryExitData,
    TimeSettingDataModel timeSettings)
        {
            return entryExitData.TodayList
                .Where(x => x.EntryTime != null && x.ExitTime != null && x.EntryTime != x.ExitTime)
                .OrderBy(x => x.EntryTime)
                .ToList();
        }


        private (bool shouldAdd, string exitTime, DateTime exitDate) CheckNextShiftContinuity(
    EmployeeEntryExitYesterdayToTomorrowModel entryExitData,
    TimeSettingDataModel timeSettings,
    EmployeeAttendAbsenceAnalysisInputModel input)
        {
            var lastToday = entryExitData.TodayList.OrderBy(x => x.EntryTime).Last();

            if (lastToday?.EntryTime != null && lastToday.ExitTime == null && entryExitData.TomorrowList.Any())
            {
                var firstTomorrow = entryExitData.TomorrowList.First();
                return CalculateExitTimeForContinuousShift(lastToday, firstTomorrow, timeSettings, input);
            }

            return (false, null, DateTime.MinValue);
        }

        private (bool, string, DateTime) CalculateExitTimeForContinuousShift(
            EmployeeEntryExitViewModel lastToday,
            EmployeeEntryExitViewModel firstTomorrow,
            TimeSettingDataModel timeSettings,
            EmployeeAttendAbsenceAnalysisInputModel input)
        {
            //var duration = CalculateShiftDuration(lastToday, firstTomorrow);

            //if (duration <= timeSettings.MaximumAttendInMinute || input.AnalysisType == EnumAnalysisType.Keshik.Id)
            //{
            //    return DetermineAppropriateExitTime(lastToday, firstTomorrow, timeSettings, input);
            //}

            return (false, null, DateTime.MinValue);
        }


        private List<EmployeeEntryExitViewModel> FinalizeEntryExitRecords(
    List<EmployeeEntryExitViewModel> entryExitResult,
    EmployeeEntryExitYesterdayToTomorrowModel entryExitData,
    TimeSettingDataModel timeSettings,
    IQueryable<EmployeeEntryExitAttendAbsence> attendAbsenceData)
        {
            // اضافه کردن رکوردهای فردا در صورت نیاز
            if (timeSettings.ShiftEndDate == entryExitData.TomorrowDate && entryExitResult.Any())
            {
                var tomorrowRecords = entryExitData.TomorrowList
                    .Where(x => x.ExitTime != null
                              && x.EntryTime != null
                              && x.EntryTimeToTimeSpan < timeSettings.ShiftEndTimeWithToleranceToTimeSpan)
                    .ToList();

                entryExitResult.AddRange(tomorrowRecords);
            }

            // فیلتر نهایی بر اساس حضور و غیاب
            return entryExitResult
                .Where(x => !attendAbsenceData.Any(a => a.EmployeeEntryExitId == x.EntryId
                                                     || a.EmployeeEntryExitId == x.ExitId))
                .ToList();
        }


    }
}
