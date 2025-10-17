using AutoMapper;
using DNTPersianUtils.Core;
using KSC.Common;
using Ksc.HR.Appication.Interfaces.WorkShift;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.WorkFlow.Classes.Enumerations;
using Ksc.HR.DTO.WorkShift.WorkCalendar;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Share.General;
using Ksc.HR.Share.Model.SystemSequenceStatus;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.Hr.Domain.Shared;
using Ksc.HR.Resources.Messages;
using Ksc.HR.DTO.Stepper;
using Ksc.HR.Domain.Entities.Stepper;
using Ksc.HR.Application.Interfaces;
using System.Globalization;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using KscHelper.Model;

namespace Ksc.HR.Appication.Services.WorkShift
{
    public class WorkCalendarService : IWorkCalendarService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;
        private readonly IStepper_ProcedureService _procedureService;

        public WorkCalendarService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler, IStepper_ProcedureService procedureService)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;
            _mapper = mapper;
            _FilterHandler = FilterHandler;
            _procedureService = procedureService;
        }
        public async Task<KscResult> SetWorkDayTypeOnWorkCalendar(int year)
        {
            var result = new KscResult();
            try
            {


                var workCalendarByShamsiYear = _kscHrUnitOfWork.WorkCalendarRepository.GetWorkCalendarByShamsiYear(year);
                if (!workCalendarByShamsiYear.Any())
                {
                    throw new Exception("برای این عدد سال اطلاعات وجود ندارد");
                }
                var calendarEvent = _kscHrUnitOfWork.CalendarEventRepository.WhereQueryable(x => x.IsHoliday); // مناسبت ها
                foreach (var day in workCalendarByShamsiYear)
                {
                    //  تنظیمات نوع روزهای تعطیل
                    if (calendarEvent.Any(x => (x.CalendarType == EnumCalendarType.Shamsi.Id && x.Mmdd == day.MmddShamsi) ||
                    (x.CalendarType == EnumCalendarType.Miladi.Id && x.Mmdd == day.MmddMiladi) ||
                    (x.CalendarType == EnumCalendarType.Ghamari.Id && x.Mmdd == day.MmddHijri)
                    ))
                    {
                        //if (day.MmddShamsi == "0211") // روز جهانی کارگر
                        if (day.MmddMiladi == "0501") // روز جهانی کارگر
                            day.WorkDayTypeId = EnumWorkDayType.WorkerDayHoliday.Id;
                        else
                        {
                            if (day.DayOfWeek != (int)DayNumberType.Thursday && day.DayOfWeek != (int)DayNumberType.Friday)
                                day.WorkDayTypeId = EnumWorkDayType.OfficialHoliday.Id;
                            else
                            {
                                if (day.DayOfWeek == (int)DayNumberType.Thursday)
                                    day.WorkDayTypeId = EnumWorkDayType.OfficialHolidayInThursday.Id;
                                else
                                {
                                    day.WorkDayTypeId = EnumWorkDayType.OfficialHolidayInFriday.Id;
                                }
                            }
                        }
                    }
                    else
                    {
                        //if (day.MmddShamsi == "0211") // روز جهانی کارگر
                        if (day.MmddMiladi == "0501")// روز جهانی کارگر
                            day.WorkDayTypeId = EnumWorkDayType.WorkerDayHoliday.Id;
                        else
                        {
                            if (day.DayOfWeek != (int)DayNumberType.Thursday && day.DayOfWeek != (int)DayNumberType.Friday)
                                day.WorkDayTypeId = EnumWorkDayType.NormalDay.Id;
                            else
                            {
                                if (day.DayOfWeek == (int)DayNumberType.Thursday)
                                    day.WorkDayTypeId = EnumWorkDayType.Thursday.Id;
                                else
                                {
                                    day.WorkDayTypeId = EnumWorkDayType.Friday.Id;
                                }
                            }
                        }
                    }
                }

                await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                result.AddError("", ex.Message);
            }
            return result;
        }
        public FilterResult<WorkCalendarModel> GetWorkCalendarByFilter(WorkCalendarFilterRequest Filter)
        {

            var query = _kscHrUnitOfWork.WorkCalendarRepository.GetWorkCalendarIncluded();
            if (!string.IsNullOrEmpty(Filter.CalendarMonthPickerId))
            {
                Filter.CalendarMonthPickerId = Filter.CalendarMonthPickerId.ToEnglishNumbers();
                query = query.Where(x => x.YearMonthV2 == Filter.CalendarMonthPickerId);
            }
            var data = query.Select(a => new WorkCalendarModel()
            {
                Id = a.Id,
                DayOfYear = a.DayOfYear,
                MiladiDateV1 = a.MiladiDateV1,
                ShamsiDateV2 = a.ShamsiDateV2,
                SystemSequenceStatusCode = a.SystemSequenceStatus != null ? a.SystemSequenceStatus.Code : "",
                SystemSequenceStatusTitle = a.SystemSequenceStatus != null ? a.SystemSequenceStatus.Title : "",
                WeekOfYear = a.WeekOfYear,
                WorkDayTypeCode = a.WorkDayType != null ? a.WorkDayType.Code : "",
                WorkDayTypeTitle = a.WorkDayType != null ? a.WorkDayType.Title : "",
                HijriDateV1 = a.HijriDateV1,
                HijriDateV2 = a.HijriDateV2,
                MonthNameHijriV1 = a.MonthNameHijriV1

            });
            var result = _FilterHandler.GetFilterResult<WorkCalendarModel>(data, Filter, "Id");
            return new FilterResult<WorkCalendarModel>()
            {
                Data = _mapper.Map<List<WorkCalendarModel>>(result.Data.ToList()),
                Total = result.Total

            };
        }

        public async Task<WorkCalendar> GetOne(int id)
        {
            return await _kscHrUnitOfWork.WorkCalendarRepository.GetByIdAsync(id);
        }
        public async Task<EditWorkCalendarModel> GetWorkCalendarById(int id)
        {
            var model = await GetOne(id);
            return _mapper.Map<EditWorkCalendarModel>(model);
        }
        public async Task<KscResult> EditWorkCalendar(EditWorkCalendarModel model)
        {
            var result = model.IsValid();
            if (!result.Success)
                return result;
            var workCalenderData = await GetOne(model.Id);
            workCalenderData.WorkDayTypeId = model.WorkDayTypeId;
            workCalenderData.SystemSequenceStatusId = model.SystemSequenceStatusId;
            //
            if (workCalenderData.WorkDayTypeId == EnumWorkDayType.OfficialHoliday.Id)
            {
                //  if (workCalenderData.MmddShamsi == "0211") // روز جهانی کارگر
                if (workCalenderData.MmddMiladi == "0501") // روز جهانی کارگر
                    workCalenderData.WorkDayTypeId = EnumWorkDayType.WorkerDayHoliday.Id;
                else
                {
                    if (workCalenderData.DayOfWeek != (int)DayNumberType.Thursday && workCalenderData.DayOfWeek != (int)DayNumberType.Friday)
                        workCalenderData.WorkDayTypeId = EnumWorkDayType.OfficialHoliday.Id;
                    else
                    {
                        if (workCalenderData.DayOfWeek == (int)DayNumberType.Thursday)
                            workCalenderData.WorkDayTypeId = EnumWorkDayType.OfficialHolidayInThursday.Id;
                        else
                        {
                            workCalenderData.WorkDayTypeId = EnumWorkDayType.OfficialHolidayInFriday.Id;
                        }
                    }
                }
            }
            //
            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        public async Task<KscResult> EditWorkCalenderByStatus(EditWorkCalenderStatus model)
        {
            var result = model.IsValid();
            if (!result.Success)
                return result;
            foreach (var id in model.Ids.Split(",").ToArray())
            {
                var workCalenderData = await GetOne(Int32.Parse(id));
                workCalenderData.SystemSequenceStatusId = model.SystemSequenceStatusId;
            }
            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }
        public async Task<KscResult> UpdateWorkCalenderByYearMonthStatus(UpdateWorkCalenderByYearMonthStatusModel model)
        {
            var result = model.IsValid();
            if (model.Yearmonth == null)
            {
                result.AddError("", "لطفا ماه را وارد نمایید");
                return result;
            }
            if (model.SystemSequenceStatusId == null)
            {
                result.AddError("", "لطفا وضعیت را وارد نمایید");
                return result;
            }
            //if (model.Yearmonth == null)
            //{
            //    result.AddError("", "لطفا ماه را وارد نمایید");
            //    return result;
            //}
            if (model.Step == null)
            {
                result.AddError("", "لطفا مرحله کارکرد ماهانه را وارد نمایید");
                return result;
            }
            try
            {
                int yearmonth = Convert.ToInt32(model.Yearmonth);
                int step = Convert.ToInt32(model.Step);
                int systemSequenceStatusId = Convert.ToInt32(model.SystemSequenceStatusId);
                if (!result.Success)
                    return result;
                var queryWorkCalendar = _kscHrUnitOfWork.WorkCalendarRepository.WhereQueryable(a => a.YearMonthV1 == yearmonth);
                var systemSequenceStatus = _kscHrUnitOfWork.SystemSequenceStatusRepository.GetById(systemSequenceStatusId).Title;
                foreach (var workCalenderData in queryWorkCalendar)
                {
                    workCalenderData.SystemSequenceStatusId = systemSequenceStatusId;
                }
                _kscHrUnitOfWork.MonthTimeSheetLogRepository.Add(new MonthTimeSheetLog()
                {
                    InsertDate = DateTime.Now,
                    YearMonth = yearmonth,
                    InsertUser = model.CurrentUser,
                    Result = systemSequenceStatus,
                    //Step = step,///
                    MonthTimeShitStepperId = step,
                    ResultCount = queryWorkCalendar.Count(),

                });
                await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new HRBusinessException(Validations.RepetitiveId, "عملیات نا موفق بود" + ex.Message);
            }
            return result;
        }

        public async Task<KscResult> UpdateWorkCalenderByYearMonthStatusStep(UpdateStatusByYearMonthProcedureModel model)
        {
            var result = model.IsValid();
            if (model.Yearmonth == null)
            {
                result.AddError("", "لطفا ماه را وارد نمایید");
                return result;
            }
            var StepperProcedureData = _kscHrUnitOfWork.Stepper_ProcedureRepository.GetById(model.ProcedureId);
            if (StepperProcedureData.SystemSequenceStatusId == null)
            {
                result.AddError("", "لطفا وضعیت را وارد نمایید");
                return result;
            }
            if (StepperProcedureData == null)
            {
                result.AddError("", "لطفا مرحله کارکرد ماهانه را وارد نمایید");
                return result;
            }
            try
            {
                int yearmonth = Convert.ToInt32(model.Yearmonth);
                //int step = Convert.ToInt32(model.Step);
                int systemSequenceStatusId;
                if (model.IsBackStep)
                    systemSequenceStatusId = StepperProcedureData.SystemSequenceStatusIdBack.Value;
                else
                    systemSequenceStatusId = StepperProcedureData.SystemSequenceStatusId.Value;
                if (!result.Success)
                    return result;
                var queryWorkCalendar = _kscHrUnitOfWork.WorkCalendarRepository.GetAllQuerable().Where(a => a.YearMonthV1 == yearmonth);
                var systemSequenceStatus = _kscHrUnitOfWork.SystemSequenceStatusRepository.GetById(systemSequenceStatusId).Title;
                foreach (var workCalenderData in queryWorkCalendar)
                {
                    workCalenderData.SystemSequenceStatusId = systemSequenceStatusId;
                }
                model.Result = systemSequenceStatus;
                model.ResultCount = queryWorkCalendar.Count();
                result = await _procedureService.InsertStepProcedure(model);
                //if (!_kscHrUnitOfWork.Stepper_ProcedureStatusRepository.Any(x => x.YearMonth == yearmonth && x.ProcedureId == model.ProcedureId))
                //    _kscHrUnitOfWork.Stepper_ProcedureStatusRepository.Add(new Stepper_ProcedureStatus()
                //    {
                //        InsertDate = DateTime.Now,
                //        InsertUser = model.CurrentUser,
                //        ProcedureId = model.ProcedureId,
                //        IsActive = false,
                //        IsDone = true,
                //        Result = systemSequenceStatus,
                //        ResultCount = queryWorkCalendar.Count(),
                //        YearMonth = yearmonth
                //    });
                //_kscHrUnitOfWork.Stepper_ProcedureLogRepository.Add(new Stepper_ProcedureLog
                //{
                //    InsertDate = DateTime.Now,
                //    YearMonth = yearmonth,
                //    InsertUser = model.CurrentUser,
                //    ProcedureId = model.ProcedureId,
                //    IsActive = true,
                //    //Result = systemSequenceStatus,
                //    //Step = step,///
                //    //MonthTimeShitStepperId = step,
                //    //ResultCount = queryWorkCalendar.Count(),

                //});
                if (result.Success)
                    await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new HRBusinessException(Validations.RepetitiveId, "عملیات نا موفق بود" + ex.Message);
            }
            return result;
        }
        public async Task<bool> CheckDailyTimeSheetStatus(string date)
        {
            var result = new KscResult();
            var data = _kscHrUnitOfWork.WorkCalendarRepository.FirstOrDefault(x => x.YearMonthV2 == date.ToEnglishNumbers());
            var systemStatus = await _kscHrUnitOfWork.WorkCalendarRepository.GetDailyTimeSheetStatus(data.Id);
            var enumSystemSequenceStatusDaily = EnumSystemSequenceStatusDailyTimeSheet.GetAll<EnumSystemSequenceStatusDailyTimeSheet>().Where(x =>
                                   ConstSystemSequenceStatusDailyTimeSheet.DailyTimeSheetIsActive.Contains(x.Id));
            // result.Id = (bool)(systemStatus != EnumSystemSequenceStatusDailyTimeSheet.InActiveForAllUser.Id);
            //

            var systemControlDate = _kscHrUnitOfWork.SystemControlDateRepository.GetAllQueryable().FirstOrDefault();
            var yearMonth = date.ToEnglishNumbers();
            yearMonth = yearMonth.Replace("/", "");
            if (systemControlDate.AttendAbsenceItemDate > int.Parse(yearMonth))
            {
                return false;
            }
            else
            {
                if (enumSystemSequenceStatusDaily.Any(x => x.Id == systemStatus))
                    return true;
            }
            //
            return false;
        }
        public async Task<KscResult> UpdateHijriDate(int dateKeyStart, int dateKeyEnd)
        {
            var workCalendars = _kscHrUnitOfWork.WorkCalendarRepository.Where(x => x.DateKey >= dateKeyStart && x.DateKey <= dateKeyEnd)
                .Select(x => new
                {

                    HijriDateV1 = x.HijriDateV1,
                    HijriDateV2 = x.HijriDateV2,
                    DayNameHijri = x.DayNameHijri,
                    MonthNameHijriV1 = x.MonthNameHijriV1,
                    DayOfMonthHijri = x.DayOfMonthHijri,
                    MmddHijri = x.MmddHijri,
                    YyyymmHijri = x.YyyymmHijri,
                    YyyyHijri = x.YyyyHijri,
                    Id = x.Id
                })
                .ToList();
            var calendarEvent = _kscHrUnitOfWork.CalendarEventRepository.WhereQueryable(x => x.IsHoliday); // مناسبت ها
            for (int i = 0; i < workCalendars.Count(); i++)
            {
                var currentWorkCalendar = workCalendars[i];
                var HijriDateV1 = currentWorkCalendar.HijriDateV1;
                var HijriDateV2 = currentWorkCalendar.HijriDateV2;
                var DayNameHijri = currentWorkCalendar.DayNameHijri;
                var MonthNameHijriV1 = currentWorkCalendar.MonthNameHijriV1;
                var DayOfMonthHijri = currentWorkCalendar.DayOfMonthHijri;
                var MmddHijri = currentWorkCalendar.MmddHijri;
                var YyyymmHijri = currentWorkCalendar.YyyymmHijri;
                var YyyyHijri = currentWorkCalendar.YyyyHijri;
                var nextWorkCalendar = _kscHrUnitOfWork.WorkCalendarRepository.GetById(workCalendars[i + 1].Id);

                nextWorkCalendar.HijriDateV1 = HijriDateV1;
                nextWorkCalendar.HijriDateV2 = HijriDateV2;
                nextWorkCalendar.DayNameHijri = DayNameHijri;
                nextWorkCalendar.MonthNameHijriV1 = MonthNameHijriV1;
                nextWorkCalendar.DayOfMonthHijri = DayOfMonthHijri;
                nextWorkCalendar.MmddHijri = MmddHijri;
                nextWorkCalendar.YyyymmHijri = YyyymmHijri;
                nextWorkCalendar.YyyyHijri = YyyyHijri;
                //
                //  تنظیمات نوع روزهای تعطیل
                if (calendarEvent.Any(x => (x.CalendarType == EnumCalendarType.Shamsi.Id && x.Mmdd == nextWorkCalendar.MmddShamsi) ||
                (x.CalendarType == EnumCalendarType.Miladi.Id && x.Mmdd == nextWorkCalendar.MmddMiladi) ||
                (x.CalendarType == EnumCalendarType.Ghamari.Id && x.Mmdd == nextWorkCalendar.MmddHijri)
                ))
                {
                    // if (nextWorkCalendar.MmddShamsi == "0211") // روز جهانی کارگر
                    if (nextWorkCalendar.MmddMiladi == "0501") // روز جهانی کارگر
                        nextWorkCalendar.WorkDayTypeId = EnumWorkDayType.WorkerDayHoliday.Id;
                    else
                    {
                        if (nextWorkCalendar.DayOfWeek != (int)DayNumberType.Thursday && nextWorkCalendar.DayOfWeek != (int)DayNumberType.Friday)
                            nextWorkCalendar.WorkDayTypeId = EnumWorkDayType.OfficialHoliday.Id;
                        else
                        {
                            if (nextWorkCalendar.DayOfWeek == (int)DayNumberType.Thursday)
                                nextWorkCalendar.WorkDayTypeId = EnumWorkDayType.OfficialHolidayInThursday.Id;
                            else
                            {
                                nextWorkCalendar.WorkDayTypeId = EnumWorkDayType.OfficialHolidayInFriday.Id;
                            }
                        }
                    }
                }
                else
                {
                    //if (nextWorkCalendar.MmddShamsi == "0211") // روز جهانی کارگر
                    if (nextWorkCalendar.MmddMiladi == "0501")// روز جهانی کارگر
                        nextWorkCalendar.WorkDayTypeId = EnumWorkDayType.WorkerDayHoliday.Id;
                    else
                    {
                        if (nextWorkCalendar.DayOfWeek != (int)DayNumberType.Thursday && nextWorkCalendar.DayOfWeek != (int)DayNumberType.Friday)
                            nextWorkCalendar.WorkDayTypeId = EnumWorkDayType.NormalDay.Id;
                        else
                        {
                            if (nextWorkCalendar.DayOfWeek == (int)DayNumberType.Thursday)
                                nextWorkCalendar.WorkDayTypeId = EnumWorkDayType.Thursday.Id;
                            else
                            {
                                nextWorkCalendar.WorkDayTypeId = EnumWorkDayType.Friday.Id;
                            }
                        }
                    }
                }
                //
                if (nextWorkCalendar.DateKey == dateKeyEnd)
                    break;
            }
            await _kscHrUnitOfWork.SaveAsync();
            var result = new KscResult();
            return result;
        }

        /// <summary>
        /// این متد برای روزکار است ( پنج شنبه و جمعه را تعطیلا اعلام میکند)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<ReturnData<WorkCalendarIsHolidayModel>> IsHoliday(DateTime date)
        {
            var result = new ReturnData<WorkCalendarIsHolidayModel>();
            try
            {
                var isHoliday = _kscHrUnitOfWork.WorkCalendarRepository.IsHoliday(date)
                    .Select(x => new WorkCalendarIsHolidayModel
                    {
                        DayOfWeek = x.DayOfWeek,
                        IsHoliday = x.WorkDayType.IsHoliday || (x.DayOfWeek >= 6),
                        MiladiDateV1 = x.MiladiDateV1,
                        ShamsiDateV1 = x.ShamsiDateV1,
                        ShamsiDateV2 = x.ShamsiDateV2,
                        WorkDayTypeTitle = x.WorkDayType.Title,
                    }).First();
                result.AddData(isHoliday);
            }
            catch (Exception ex)
            {
                result.AddError($"Message:{ex.Message} |InnerExceptionMessage:{ex.InnerException?.Message}");
            }
            return result;
        }

        /// <summary>
        /// تقویم روزکاری فولاد
        /// </summary>
        /// <param name="Filter"></param>
        /// <returns></returns>
        public FilterResult<WorkCalendarIsHolidayModel> GetWorkCalendarWithIsHoliday(WorkCalendarFilterRequest Filter)
        {

            var query = _kscHrUnitOfWork.WorkCalendarRepository.GetWorkCalendarIncluded();
            if (!string.IsNullOrEmpty(Filter.CalendarMonthPickerId))
            {
                Filter.CalendarMonthPickerId = Filter.CalendarMonthPickerId.ToEnglishNumbers();
                var yearMonthV1 = int.Parse(Filter.CalendarMonthPickerId.Replace("/", ""));
                query = query.Where(x => x.YearMonthV1 == yearMonthV1);
            }
            if (Filter.FromYearmonth>0)
            {
                query = query.Where(x => x.YearMonthV1>= Filter.FromYearmonth);
            }
            if (Filter.ToYearMonth > 0)
            {
                
                query = query.Where(x => x.YearMonthV1 <= Filter.ToYearMonth);
            }
            var data = query.Select(x => new WorkCalendarIsHolidayModel()
            {
                //Id = x.Id,
              
                //MiladiDateV1 = x.MiladiDateV1,
                //ShamsiDateV2 = x.ShamsiDateV2,
                //SystemSequenceStatusCode = x.SystemSequenceStatus != null ? x.SystemSequenceStatus.Code : "",
                //SystemSequenceStatusTitle = x.SystemSequenceStatus != null ? x.SystemSequenceStatus.Title : "",
                //WeekOfYear = x.WeekOfYear,
                //WorkDayTypeCode = x.WorkDayType != null ? x.WorkDayType.Code : "",
                //WorkDayTypeTitle = x.WorkDayType != null ? x.WorkDayType.Title : "",
                //HijriDateV2 = x.HijriDateV2,
                //MonthNameHijriV1 = x.MonthNameHijriV1
                HijriDateV1 = x.HijriDateV1,
                DayOfYear = x.DayOfYear,
                DayOfWeek = x.DayOfWeek,
                IsHoliday = x.WorkDayType.IsHoliday || (x.DayOfWeek >= 6),
                MiladiDateV1 = x.MiladiDateV1,
                ShamsiDateV1 = x.ShamsiDateV1,
                ShamsiDateV2 = x.ShamsiDateV2,
                WorkDayTypeTitle = x.WorkDayType.Title,

            }).ToList();
            var result = _FilterHandler.GetFilterResult<WorkCalendarIsHolidayModel>(data, Filter, "MiladiDateV1");
            return new FilterResult<WorkCalendarIsHolidayModel>()
            {
                Data = _mapper.Map<List<WorkCalendarIsHolidayModel>>(result.Data.ToList()),
                Total = result.Total

            };
        }



        public async Task<KscResult> UpdateHijriDate2(int dateKeyStart, int dateKeyEnd)
        {
            if (dateKeyStart < 14030101)
                throw new Exception("تاریخ شروع باید بزرگتر یا مساوی از 14030101 باشد");
            var workCalendars = _kscHrUnitOfWork.WorkCalendarRepository.Where(x => x.DateKey >= dateKeyStart && x.DateKey <= dateKeyEnd);

            var calendarEvent = _kscHrUnitOfWork.CalendarEventRepository.WhereQueryable(x => x.IsHoliday); // مناسبت ها
            //for (int i = 0; i < workCalendars.Count(); i++)
            UmAlQuraCalendar um = new UmAlQuraCalendar();
            foreach (var item in workCalendars)
            {

                var HijriDateV1 = item.MiladiDateV1.ToIslamicDay();
                var year = um.GetYear(item.MiladiDateV1);
                var month = um.GetMonth(item.MiladiDateV1);
                var day = um.GetDayOfMonth(item.MiladiDateV1);
                //item.HijriDateV1 = HijriDateV1;
                //item.HijriDateV2 = HijriDateV2;
                //item.DayNameHijri = DayNameHijri;
                //item.MonthNameHijriV1 = MonthNameHijriV1;
                //item.DayOfMonthHijri = DayOfMonthHijri;
                //item.MmddHijri = MmddHijri;
                //item.YyyymmHijri = YyyymmHijri;
                //item.YyyyHijri = YyyyHijri;
                //
                //  تنظیمات نوع روزهای تعطیل
                if (calendarEvent.Any(x => (x.CalendarType == EnumCalendarType.Shamsi.Id && x.Mmdd == item.MmddShamsi) ||
                (x.CalendarType == EnumCalendarType.Miladi.Id && x.Mmdd == item.MmddMiladi) ||
                (x.CalendarType == EnumCalendarType.Ghamari.Id && x.Mmdd == item.MmddHijri)
                ))
                {
                    // if (item.MmddShamsi == "0211") // روز جهانی کارگر
                    if (item.MmddMiladi == "0501") // روز جهانی کارگر
                        item.WorkDayTypeId = EnumWorkDayType.WorkerDayHoliday.Id;
                    else
                    {
                        if (item.DayOfWeek != (int)DayNumberType.Thursday && item.DayOfWeek != (int)DayNumberType.Friday)
                            item.WorkDayTypeId = EnumWorkDayType.OfficialHoliday.Id;
                        else
                        {
                            if (item.DayOfWeek == (int)DayNumberType.Thursday)
                                item.WorkDayTypeId = EnumWorkDayType.OfficialHolidayInThursday.Id;
                            else
                            {
                                item.WorkDayTypeId = EnumWorkDayType.OfficialHolidayInFriday.Id;
                            }
                        }
                    }
                }
                else
                {
                    // if (item.MmddShamsi == "0211") // روز جهانی کارگر
                    if (item.MmddMiladi == "0501") // روز جهانی کارگر
                        item.WorkDayTypeId = EnumWorkDayType.WorkerDayHoliday.Id;
                    else
                    {
                        if (item.DayOfWeek != (int)DayNumberType.Thursday && item.DayOfWeek != (int)DayNumberType.Friday)
                            item.WorkDayTypeId = EnumWorkDayType.NormalDay.Id;
                        else
                        {
                            if (item.DayOfWeek == (int)DayNumberType.Thursday)
                                item.WorkDayTypeId = EnumWorkDayType.Thursday.Id;
                            else
                            {
                                item.WorkDayTypeId = EnumWorkDayType.Friday.Id;
                            }
                        }
                    }
                }
                //
                if (item.DateKey == dateKeyEnd)
                    break;
            }
            //  await _kscHrUnitOfWork.SaveAsync();
            var result = new KscResult();
            return result;
        }
    }
}
