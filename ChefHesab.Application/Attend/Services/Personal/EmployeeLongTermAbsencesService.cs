using AutoMapper;
using KSC.Common;
using Ksc.HR.Appication.Interfaces.Personal;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Repositories.Personal;
using Ksc.HR.DTO.Personal.EmployeeLongTermAbsences;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Share.Model.SystemSequenceStatus;
using Ksc.Hr.Domain.Shared;
using Ksc.HR.Resources.Messages;
using Ksc.HR.Appication.Services.WorkShift;
using Ksc.HR.Appication.Interfaces.WorkShift;
using Ksc.HR.Share.Model.RollCallDefinication;
using Ksc.HR.Domain.Entities.Workshift;

namespace Ksc.HR.Appication.Services.Personal
{
    public class EmployeeLongTermAbsencesService : IEmployeeLongTermAbsencesService
    {
        #region field
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;
        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;
        private readonly IShiftConceptDetailService _shiftConceptDetailService;
        private IQueryable<WorkCalendar>? _Calendars { get; set; }
        private IQueryable<EmployeeLongTermAbsence>? EmployeeLongTermAbsences { get; set; }
        private IQueryable<EmployeeWorkGroup>? _EmployeeWorkGroup { get; set; }
        private IQueryable<EmployeeAttendAbsenceItem>? _EmployeeAttendAbsenceItem { get; set; }

        #endregion



        public EmployeeLongTermAbsencesService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler, IShiftConceptDetailService shiftConceptDetailService)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;
            _mapper = mapper;
            _FilterHandler = FilterHandler;
            _shiftConceptDetailService = shiftConceptDetailService;
        }
        public bool Exists(DateTime from, DateTime to, int employeId)
        {
            return _kscHrUnitOfWork.EmployeeLongTermAbsenceRepository.Any(x => employeId == x.EmployeeId
             //((a.AbsenceStartDate >= from && a.AbsenceStartDate <= to) || (a.AbsenceEndDate >= from && a.AbsenceEndDate <= to))

             && ((x.AbsenceStartDate <= from && x.AbsenceEndDate >= from) ||
                (x.AbsenceStartDate <= to && x.AbsenceEndDate >= to) ||
                (x.AbsenceStartDate >= from && x.AbsenceEndDate <= to))

            );
        }

        public bool Exists(int id, DateTime from, DateTime to, int employeId)
        {
            return _kscHrUnitOfWork.EmployeeLongTermAbsenceRepository.Any(x => x.Id != id && employeId == x.EmployeeId
                && ((x.AbsenceStartDate <= from && x.AbsenceEndDate >= from) ||
                (x.AbsenceStartDate <= to && x.AbsenceEndDate >= to) ||
                (x.AbsenceStartDate >= from && x.AbsenceEndDate <= to)));
        }

        public List<EmployeeLongTermAbsencesModel> GetEmployeeLongTermAbsences()
        {
            throw new NotImplementedException();
        }

        public FilterResult<EmployeeLongTermAbsencesModel> GetEmployeeLongTermAbsencesByFilter(EmployeeLongTermAbsencesModel Filter)
        {
            var query = _kscHrUnitOfWork.EmployeeLongTermAbsenceRepository.GetEmployeeLongTermAbsences().AsNoTracking();
            if (Filter.EmployeeId > 0)
            {
                query = query.Where(a => a.EmployeeId == Filter.EmployeeId);
            }
            if (Filter.RollCallCategoryId > 0)
            {
                query = query.Where(a => a.RollCallDefinition.RollCallCategoryId == Filter.RollCallCategoryId);
            }
            var result = _FilterHandler.GetFilterResult<EmployeeLongTermAbsence>(query, Filter, "Id");

            return new FilterResult<EmployeeLongTermAbsencesModel>()
            {
                Data = _mapper.Map<List<EmployeeLongTermAbsencesModel>>(result.Data.ToList()),
                Total = result.Total

            };
        }



        public EditEmployeeLongTermAbsencesModel GetForEdit(int id)
        {
            var result = GetOne(id);
            return _mapper.Map<EditEmployeeLongTermAbsencesModel>(result);
        }

        public EmployeeLongTermAbsence GetOne(int id)
        {
            return _kscHrUnitOfWork.EmployeeLongTermAbsenceRepository.GetAllQueryable()
                .Include(a => a.EmployeeAttendAbsenceItems)
                .ThenInclude(a => a.WorkCalendar)
                .FirstOrDefault(a => a.Id == id);
        }

        public async Task<KscResult> AddEmployeeLongTermAbsences(AddEmployeeLongTermAbsencesModel model)
        {
            var result = model.IsValid();

            if (!result.Success)
                return result;

            try
            {
                var longTermAbsenceCheck = _kscHrUnitOfWork.RollCallDefinitionRepository
                    .GetById(model.RollCallDefinitionId).LongTermAbsenceCheck;
                model.LongTermAbsenceCheck = longTermAbsenceCheck;

                _EmployeeWorkGroup = _kscHrUnitOfWork.EmployeeWorkGroupRepository.WhereQueryable(a => a.EmployeeId == model.EmployeeId)
                    .Include(x=>x.WorkGroup).ThenInclude(x=>x.WorkTime).ThenInclude(x=>x.WorkTimeShiftConcepts)
                    .ThenInclude(x=>x.ShiftConcept).ThenInclude(x=>x.ShiftConceptDetails)
                    .AsNoTracking();

                _Calendars = _kscHrUnitOfWork.WorkCalendarRepository.GetAllAsNoTracking()
                    .Where(a => a.MiladiDateV1 >= model.AbsenceStartDate.Value && a.MiladiDateV1 <= model.AbsenceEndDate.Value).AsQueryable();
               
                _EmployeeAttendAbsenceItem = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository
                        .GetEmployeeAttendAbsenceItemIncludEmployeeEntryExitAttendAbsences();

                result = IsCanAdd(model);
                if (!result.Success)
                {
                    return result;
                }
                model.AbsenceDayCount = Convert.ToInt32(model.AbsenceEndDate.Value.Subtract(model.AbsenceStartDate.Value).TotalDays + 1);

                var employeeLongTermAbsenceItem = new EmployeeLongTermAbsence()
                {
                    EmployeeId = model.EmployeeId,
                    AbsenceEndDate = model.AbsenceEndDate.Value,
                    AbsenceStartDate = model.AbsenceStartDate.Value,
                    AbsenceDayCount = model.AbsenceDayCount,
                    RollCallDefinitionId = model.RollCallDefinitionId,
                    EmployeeAttendAbsenceItems = new List<EmployeeAttendAbsenceItem>(),
                    InsertDate = DateTime.Now,
                    InsertUser = model.CurrentUserName


                };

                var workCalendars = _Calendars.Where(a => a.MiladiDateV1 >= model.AbsenceStartDate.Value && a.MiladiDateV1 <= model.AbsenceEndDate.Value).ToList();
                var employeeWorkGroup = _EmployeeWorkGroup.Where(a => a.EmployeeId == model.EmployeeId).ToList();
                var workGroupIds = employeeWorkGroup.Select(a => a.WorkGroupId).ToList();
                var workcalendarIds = workCalendars.Select(a => a.Id).ToList();
                var shiftConcepts = _kscHrUnitOfWork.ShiftConceptDetailRepository.GetShiftConceptDetailWithWOrkGroupIdDates(workGroupIds, workcalendarIds).GetAwaiter().GetResult();
                var workCity = _kscHrUnitOfWork.EmployeeRepository.GetByIdAsync(employeeLongTermAbsenceItem.EmployeeId).GetAwaiter().GetResult();
                //List<int> codes = new List<int>() { 15, 24, 54, 65 };
                if (model.LongTermAbsenceCheck)
                {
                    var absenceItems = _EmployeeAttendAbsenceItem.Where(x => x.EmployeeId == model.EmployeeId && !x.EmployeeLongTermAbsenceId.HasValue && workcalendarIds.Contains(x.WorkCalendarId) /*&& codes.Contains(x.RollCallDefinitionId)*/);
                    foreach (var item in absenceItems)
                    {

                        if (item.EmployeeEntryExitAttendAbsences.Any())
                        {

                            result.AddError("خطا", "در این بازه کاربر دارای کارکرد می باشد");
                            //throw new HRBusinessException(Validations.RepetitiveId, );
                        }
                        _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.Delete(item);
                    }
                }
                else
                {
                    var absenceItems = _EmployeeAttendAbsenceItem
                        .Where(a =>
                         a.EmployeeId == model.EmployeeId && a.RollCallDefinitionId == EnumRollCallDefinication.AbsenceDaily.Id
                         && !a.EmployeeLongTermAbsenceId.HasValue &&
                         workcalendarIds.Contains(a.WorkCalendarId));
                    foreach (var item in absenceItems)
                    {

                        if (item.EmployeeEntryExitAttendAbsences.Any())
                        {
                            result.AddError("خطا", "در این بازه کاربر دارای کارکرد می باشد");
                            //throw new HRBusinessException(Validations.RepetitiveId, );
                        }
                        _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.Delete(item);
                    }

                }
                var employee = await _kscHrUnitOfWork.EmployeeRepository.GetEmployeeByEmployeeId(model.EmployeeId);
                var employeeWorkGoups = _EmployeeWorkGroup
                     .Where(a => a.EmployeeId == employee.Id).ToList();

                var WorkGroupIds = employeeWorkGoups.Select(a => a.WorkGroupId).ToList();
                var ShiftConceptDetails = _shiftConceptDetailService.GetShiftConceptDetailWithWOrkGroupIdAndWokCalendarIds(WorkGroupIds, workcalendarIds);
                var ShiftConceptDetailsIds = ShiftConceptDetails.Select(a => a.Id).ToList();
                var GetListShiftStartEndTime = await _kscHrUnitOfWork.EmployeeLongTermAbsenceRepository.GetListShiftStartEndTime(ShiftConceptDetailsIds, employee.WorkCityId.Value, WorkGroupIds, workcalendarIds);
                foreach (var calendarId in workcalendarIds)
                {
                    var workCalendar = workCalendars.First(c => c.Id == calendarId);
                    var employeWorkGroup = employeeWorkGoups.Where(a => (workCalendar.MiladiDateV1 >= a.StartDate && workCalendar.MiladiDateV1 <= a.EndDate)
                                   ||
                                   (workCalendar.MiladiDateV1 >= a.StartDate && !a.EndDate.HasValue)
                                  ).FirstOrDefault();
                    var shiftConceptDetailId = 0;
                    if (employeWorkGroup == null)
                    {
                        result.AddError("خطا", "در این بازه شیفت کاری کاربر نیاز به بررسی دارد ");
                        return result;

                    }
                    if (employeWorkGroup.WorkGroup.WorkTime.ShiftSettingFromShiftboard)
                    {
                        shiftConceptDetailId = shiftConcepts.First(x => x.ShiftBoards.Any(a => a.WorkCalendarId == calendarId)).Id;
                    }
                    else
                    {

                        var workTimeShiftConcept = employeWorkGroup.WorkGroup.WorkTime.WorkTimeShiftConcepts.FirstOrDefault(x => x.IsActive == true);
                        if (workTimeShiftConcept != null)
                            shiftConceptDetailId = workTimeShiftConcept.ShiftConcept.ShiftConceptDetails.First(x => x.IsActive == true).Id;
                    }
                    var getstartAndEndTimeShift = GetListShiftStartEndTime.First(x => x.WorkCompanySetting.WorkTimeShiftConcept.ShiftConcept.ShiftConceptDetails.Any(c => c.Id == shiftConceptDetailId) &&
                    x.ValidityStartDate.Value.Date <= workCalendar.MiladiDateV1.Date &&
                    x.ValidityEndDate.Value.Date >= workCalendar.MiladiDateV1.Date);
                    //مدت زمان شیف باید بدست بیاید
                    var addEmployeeAttendAbsenceItem = new EmployeeAttendAbsenceItem()
                    {
                        EmployeeId = employeeLongTermAbsenceItem.EmployeeId,
                        WorkTimeId = employeWorkGroup.WorkGroup.WorkTimeId,
                        WorkCalendarId = calendarId,
                        RollCallDefinitionId = employeeLongTermAbsenceItem.RollCallDefinitionId,
                        ShiftConceptDetailId = shiftConceptDetailId,
                        IsManual = true,
                        StartTime = getstartAndEndTimeShift.ShiftStartTime,
                        EndTime = getstartAndEndTimeShift.ShiftEndtTime,
                        TimeDuration = getstartAndEndTimeShift.TotalWorkHourInDay,
                        InsertDate = DateTime.Now,
                        InsertUser = employeeLongTermAbsenceItem.InsertUser
                    };
                    //مدت زمان شیف باید بدست بیاید
                    employeeLongTermAbsenceItem.EmployeeAttendAbsenceItems.Add(addEmployeeAttendAbsenceItem);
                }

                await _kscHrUnitOfWork.EmployeeLongTermAbsenceRepository.AddAsync(employeeLongTermAbsenceItem);
                await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                return result;
            }


            return result;

        }


        private KscResult IsCanAdd(AddEmployeeLongTermAbsencesModel model)
        {
            var result = new KscResult();

            if (!model.AbsenceStartDate.HasValue)
            {
                result.AddError("رکورد نامعتبر", "لطفا تاریخ شروع را وارد نمایید");
                return result;
            }

            if (!model.AbsenceEndDate.HasValue)
            {
                result.AddError("رکورد نامعتبر", "لطفا تاریخ پایان  را وارد نمایید");
                return result;
            }
            if (model.AbsenceStartDate > model.AbsenceEndDate)
            {
                result.AddError("رکورد نامعتبر", " تاریخ شروع  نمیتواند از تاریخ پایان بزرگتر باشد");
                return result;
            }
            if (model.RollCallDefinitionId <= 0)
            {
                result.AddError("رکورد نامعتبر", " نوع حضور را انتخاب نمایید");
                return result;
            }
            if (model.AbsenceStartDate < model.EmployeeRegisterDate)
            {
                result.AddError("رکورد نامعتبر", " تاریخ شروع  نمیتواند از تاریخ استخدام شخص کوچکتر باشد");
                return result;
            }
            if (model.AbsenceEndDate.Value.Month < DateTime.Now.Month && model.AbsenceEndDate.Value.Year < DateTime.Now.Year)
            {
                result.AddError("رکورد نامعتبر", " تاریخ خاتمه نمی تواند از ماه و سال جاری کوچکتر باشد");
                return result;
            }
            var _timeWorkDay = _kscHrUnitOfWork.SystemControlDateRepository.GetActiveData().AttendAbsenceItemDate;
            if (model.RollCallDefinitionId <= 0)
            {
                result.AddError("رکورد نامعتبر", " نوع حضور را انتخاب نمایید");
                return result;
            }

            if (model.RollCallCategoryId == 2)
            {
                //var employeeeTypecode = _kscHrUnitOfWork.ViewMisEmploymentTypeRepository.GetEmployeTypeCode(employeeeDetail.EmploymentTypeId.Value);
                var finded = _kscHrUnitOfWork.RollCallDefinitionRepository.GetIncludedRollCallEmploymentTypes().AsNoTracking()
                    .Where(a => a.RollCallCategoryId == 2 && a.Id == model.RollCallDefinitionId)
                    .Any(x => x.IsValidForAllEmploymentType
                || x.RollCallEmploymentTypes.Any(a => a.EmploymentTypeCode == model.EmployeeTypeId));
                if (finded == false)
                {
                    result.AddError("رکورد نامعتبر", " کد حضور و غیاب وارد شده نا معتبر است");
                    return result;
                }
            }

            if (model.RollCallCategoryId == 3)
            {
                var finded = _kscHrUnitOfWork.RollCallDefinitionRepository
                     .Any(a => a.RollCallCategoryId == 3 && a.Id == model.RollCallDefinitionId);
                if (finded == false)
                {
                    result.AddError("رکورد نامعتبر", " کد حضور و غیاب وارد شده نا معتبر است");
                    return result;
                }
            }
            var workCalendars = _Calendars
                .Where(a => a.MiladiDateV1 >= model.AbsenceStartDate && a.MiladiDateV1 <= model.AbsenceEndDate).ToList();
            var workcalendarIds = workCalendars.Select(a => a.Id).ToList();
            //List<int> codes = new List<int>() { 15, 24, 54, 65 };
            if (!model.LongTermAbsenceCheck)
            {
                var EmployeeAttendAbsenceItems = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.WhereQueryable(a =>
                a.EmployeeId == model.EmployeeId && a.RollCallDefinitionId != EnumRollCallDefinication.AbsenceDaily.Id &&
                workcalendarIds.Contains(a.WorkCalendarId)).ToList();
                if (EmployeeAttendAbsenceItems.Any(a => a.MissionId != null))
                {
                    result.AddError("رکورد نامعتبر", "در این بازه کاربر دارای کارکرد ماموریت  می باشد");
                    return result;
                }
                else if (EmployeeAttendAbsenceItems.Any(a => a.EmployeeEducationTimeId != null))
                {
                    result.AddError("رکورد نامعتبر", "در این بازه کاربر دارای کارکرد آموزش  می باشد");
                    return result;
                }
                else if (EmployeeAttendAbsenceItems.Any())
                {
                    result.AddError("رکورد نامعتبر", "در این بازه کاربر دارای کارکرد می باشد");
                    return result;
                }
            }
            var minMonthSelected = workCalendars.OrderBy(a => a.YearMonthV1).First();
            //var timeWorkDay = _kscHrUnitOfWork.SystemControlDateRepository.GetActiveData().AttendAbsenceItemDate;
            //if (minMonthSelected.YearMonthV1 < timeWorkDay)
            //{
            //    result.AddError("رکورد نامعتبر", "ماه انتخاب شده مجاز نمی باشد");
            //    return result;
            //}
            if (Exists(model.AbsenceStartDate.Value, model.AbsenceEndDate.Value, model.EmployeeId))
            {
                result.AddError("رکورد نامعتبر", "در این بازه، کاربر دارای عدم حضور یا استعلاجی می باشد");
                return result;
            }
            var WorkGroupcode = _EmployeeWorkGroup.FirstOrDefault(a => model.EmployeeId == a.EmployeeId
            && (model.AbsenceStartDate.Value <= a.StartDate && model.AbsenceEndDate.Value >= a.StartDate
            || model.AbsenceStartDate.Value <= a.EndDate && model.AbsenceEndDate.Value >= a.EndDate));
            if (WorkGroupcode != null && WorkGroupcode.WorkGroup.Code != "R")
            {
                var iSHaveShiftBoard = _kscHrUnitOfWork.ShiftBoardRepository.ISHaveShiftBoard(model.EmployeeId, model.AbsenceStartDate.Value, model.AbsenceEndDate.Value);
                if (iSHaveShiftBoard == false)
                {
                    result.AddError("رکورد نامعتبر", "لوحه شیفت برای این کاربر ثبت نشده است");
                    return result;
                }
            }
            return result;
        }

        public async Task<KscResult> UpdateEmployeeLongTermAbsences(EditEmployeeLongTermAbsencesModel model)
        {

            var result = model.IsValid();
            if (!result.Success)
                return result;
            var oneemployeeLongTermAbsenceg = GetOne(model.Id);
            if (oneemployeeLongTermAbsenceg == null)
            {
                result.AddError("رکورد حذف شده", "رکورد حذف شده است");
                return result;
            }

            _EmployeeWorkGroup = _kscHrUnitOfWork.EmployeeWorkGroupRepository.WhereQueryable(a => a.EmployeeId == model.EmployeeId)
                .Include(x => x.WorkGroup).ThenInclude(x => x.WorkTime).ThenInclude(x => x.WorkTimeShiftConcepts)
                .AsNoTracking();
            _Calendars = _kscHrUnitOfWork.WorkCalendarRepository
                .WhereQueryable(a => a.MiladiDateV1 >= model.AbsenceStartDate && a.MiladiDateV1 <= model.AbsenceEndDate)
                .AsNoTracking();

            _EmployeeAttendAbsenceItem = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository
                    .GetEmployeeAttendAbsenceItemIncludEmployeeEntryExitAttendAbsences();



            var longTermAbsenceCheck = _kscHrUnitOfWork.RollCallDefinitionRepository.GetById(model.RollCallDefinitionId).LongTermAbsenceCheck;
            model.LongTermAbsenceCheck = longTermAbsenceCheck;
            model.AbsenceDayCount = Convert.ToInt32(model.AbsenceEndDate.Value.Subtract(model.AbsenceStartDate.Value).TotalDays + 1);

            result = IsCanEdit(model);
            if (!result.Success)
            {
                return result;
            }
            var allowedworkcalendarIds = _Calendars                
                .ToList()
                .Select(a => a.Id);
            if (model.LongTermAbsenceCheck)
            {
                var absenceItems = _EmployeeAttendAbsenceItem.Where(x => x.EmployeeId == model.EmployeeId && 
                !x.EmployeeLongTermAbsenceId.HasValue 
                && allowedworkcalendarIds.Contains(x.WorkCalendarId) /*&& codes.Contains(x.RollCallDefinitionId)*/);
                foreach (var item in absenceItems)
                {
                    if (item.EmployeeEntryExitAttendAbsences.Any())
                    {
                        throw new HRBusinessException(Validations.RepetitiveId, "در این بازه کاربر دارای کارکرد می باشد");
                    }
                    _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.Delete(item);
                }
            }
            else
            {
               
                var workcalendarIds = _Calendars.Select(a => a.Id).ToList();

                var absenceItems = _EmployeeAttendAbsenceItem
                    .Where(a =>
                     a.EmployeeId == model.EmployeeId && a.RollCallDefinitionId == EnumRollCallDefinication.AbsenceDaily.Id
                     && !a.EmployeeLongTermAbsenceId.HasValue &&
                     workcalendarIds.Contains(a.WorkCalendarId));
                foreach (var item in absenceItems)
                {

                    if (item.EmployeeEntryExitAttendAbsences.Any())
                    {
                        result.AddError("خطا", "در این بازه کاربر دارای کارکرد می باشد");
                        //throw new HRBusinessException(Validations.RepetitiveId, );
                    }
                    _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.Delete(item);
                }

            }

            if (model.AbsenceEndDate.Value.Date != oneemployeeLongTermAbsenceg.AbsenceEndDate.Date
                || model.AbsenceStartDate.Value.Date != oneemployeeLongTermAbsenceg.AbsenceStartDate.Date
                || oneemployeeLongTermAbsenceg.EmployeeAttendAbsenceItems.Count() == 0 || 
                !oneemployeeLongTermAbsenceg.EmployeeAttendAbsenceItems.Any(a => a.RollCallDefinitionId == model.RollCallDefinitionId))
            {
                var employeeLongTermAbsence = _mapper.Map<EditEmployeeLongTermAbsencesModel, EmployeeLongTermAbsence>(model, oneemployeeLongTermAbsenceg);
                employeeLongTermAbsence.UpdateDate = DateTime.Now;
                employeeLongTermAbsence.UpdateUser = model.CurrentUserName;
                var employeeAttendAbsenceItems = employeeLongTermAbsence.EmployeeAttendAbsenceItems.ToList();

                foreach (var item in employeeAttendAbsenceItems)
                {
                    _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.Delete(item);

                }
                //employeeLongTermAbsence = await AddLogTermsAbsenceItemAfterEdit(employeeLongTermAbsence);
                var employeeWorkGoups =  _EmployeeWorkGroup.Include(a=>a.WorkGroup).ThenInclude(a=>a.WorkTime)
                    .ThenInclude(a=>a.WorkTimeShiftConcepts)
                    .ThenInclude(a=>a.ShiftConcept)
                    .ThenInclude(a=>a.ShiftConceptDetails)


                    .ToList();
                var workCalendars = _Calendars.Where(a => a.MiladiDateV1 >= employeeLongTermAbsence.AbsenceStartDate && a.MiladiDateV1 <= employeeLongTermAbsence.AbsenceEndDate)
          
                    
                    .ToListAsync().GetAwaiter().GetResult();
                var workcalendarIds = workCalendars.Select(a => a.Id).ToList();
                var workGroupIds = employeeWorkGoups.Select(a => a.WorkGroupId).ToList();
                var shiftConcepts = _kscHrUnitOfWork.ShiftConceptDetailRepository.GetShiftConceptDetailWithWOrkGroupIdDates(workGroupIds, workcalendarIds).GetAwaiter().GetResult();
                var startDate = employeeLongTermAbsence.AbsenceStartDate;
                var workCity = _kscHrUnitOfWork.EmployeeRepository.GetByIdAsync(employeeLongTermAbsence.EmployeeId).GetAwaiter().GetResult();
                var startTime = DateTime.Now;
                var endTime = DateTime.Now;

                foreach (var calendarId in workcalendarIds)
                {
                    //var systemStatus = _kscHrUnitOfWork.WorkCalendarRepository.GetDailyTimeSheetStatus(calendarId).GetAwaiter().GetResult();
                    //if (systemStatus == EnumSystemSequenceStatusDailyTimeSheet.InActiveForAllUser.Id)
                    //{
                    //    throw new HRBusinessException(Validations.RepetitiveId,
                    //  "کارکرد بسته شده است");
                    //}
                    var workCalendar = workCalendars.First(c => c.Id == calendarId);
                    var employeWorkGroup = employeeWorkGoups.Where(a => (workCalendar.MiladiDateV1 >= a.StartDate && a.EndDate <= workCalendar.MiladiDateV1)
                                   ||
                                   (workCalendar.MiladiDateV1 >= a.StartDate && !a.EndDate.HasValue)
                                  ).FirstOrDefault();
                    if (employeWorkGroup == null)
                    {
                        result.AddError("خطا", "در این بازه شیفت کاری کاربر نیاز به بررسی دارد ");
                        return result;

                    }

                    var shiftConceptDetailId = 0;
                    if (employeWorkGroup.WorkGroup.WorkTime.ShiftSettingFromShiftboard)
                    {
                        shiftConceptDetailId = shiftConcepts.First(x => x.ShiftBoards.Any(a => a.WorkCalendarId == calendarId)).Id;
                    }
                    else
                    {
                        var workTimeShiftConcept = employeWorkGroup.WorkGroup.WorkTime.WorkTimeShiftConcepts.FirstOrDefault(x => x.IsActive == true);
                        if (workTimeShiftConcept != null)
                            shiftConceptDetailId = workTimeShiftConcept.ShiftConcept.ShiftConceptDetails.First(x => x.IsActive == true).Id;
                    }
                    var getstartAndEndTimeShift = _kscHrUnitOfWork.EmployeeLongTermAbsenceRepository
                        .GetShiftStartEndTimeWithWorkCalendarMiladiDate(shiftConceptDetailId,
                        workCity.WorkCityId.Value, 
                        employeWorkGroup.WorkGroupId, workCalendar.MiladiDateV1)

                        .GetAwaiter().GetResult();
                    //مدت زمان شیف باید بدست بیاید
                    var addEmployeeAttendAbsenceItem = new EmployeeAttendAbsenceItem()
                    {
                        EmployeeId = employeeLongTermAbsence.EmployeeId,
                        WorkTimeId = employeWorkGroup.WorkGroup.WorkTimeId,
                        WorkCalendarId = calendarId,
                        RollCallDefinitionId = model.RollCallDefinitionId,
                        ShiftConceptDetailId = shiftConceptDetailId,
                        IsManual = true,
                        StartTime = getstartAndEndTimeShift.Item1,
                        EndTime = getstartAndEndTimeShift.Item2,
                        TimeDuration = getstartAndEndTimeShift.Item3,
                        InsertDate = DateTime.Now,
                        InsertUser = employeeLongTermAbsence.InsertUser
                    };
                    //مدت زمان شیف باید بدست بیاید
                    employeeLongTermAbsence.EmployeeAttendAbsenceItems.Add(addEmployeeAttendAbsenceItem);
                }
                _kscHrUnitOfWork.EmployeeLongTermAbsenceRepository.Update(employeeLongTermAbsence);
            }
            else
            {
                var employeeLongTermAbsence = _mapper.Map<EmployeeLongTermAbsence>(model);
                employeeLongTermAbsence.UpdateDate = DateTime.Now;
                employeeLongTermAbsence.UpdateUser = model.CurrentUserName;
                _kscHrUnitOfWork.EmployeeLongTermAbsenceRepository.Update(employeeLongTermAbsence);
            }

            try
            {
                _kscHrUnitOfWork.SaveAsync().GetAwaiter().GetResult();

            }
            catch (Exception ex)
            {
                result.AddError("", ex.Message);
            }
            return result;
        }

        private KscResult IsCanEdit(EditEmployeeLongTermAbsencesModel model)
        {
            var result = new KscResult();

            if (!model.AbsenceStartDate.HasValue)
            {
                result.AddError("رکورد نامعتبر", "لطفا تاریخ شروع را وارد نمایید");
                return result;
            }
            if (!model.AbsenceEndDate.HasValue)
            {
                result.AddError("رکورد نامعتبر", "لطفا تاریخ پایان  را وارد نمایید");
                return result;
            }
            if (model.AbsenceStartDate > model.AbsenceEndDate)
            {
                result.AddError("رکورد نامعتبر", " تاریخ شروع  نمیتواند از تاریخ پایان بزرگتر باشد");
                return result;
            }
            if (model.AbsenceStartDate < model.EmployeeRegisterDate)
            {
                result.AddError("رکورد نامعتبر", " تاریخ شروع  نمیتواند از تاریخ استخدام شخص کوچکتر باشد");
                return result;
            }

            var rollCallDefinition = _kscHrUnitOfWork.RollCallDefinitionRepository.GetAllQueryable()
                .Include(a=>a.RollCallEmploymentTypes).AsNoTracking();
            var RollCalDefinitionId = rollCallDefinition.FirstOrDefault(a => a.Code.Trim() == "54");
            if (RollCalDefinitionId.Id != model.RollCallDefinitionId && model.AbsenceEndDate.Value.Month < DateTime.Now.Month && model.AbsenceEndDate.Value.Year < DateTime.Now.Year)
            {
                result.AddError("رکورد نامعتبر", " تاریخ خاتمه نمی تواند از ماه و سال جاری کوچکتر باشد");
                return result;
            }
            if (Exists(model.Id, model.AbsenceStartDate.Value, model.AbsenceEndDate.Value, model.EmployeeId))
            {
                result.AddError("رکورد نامعتبر", "این بازه قابل ثبت نیست");
                return result;
            }
            if (model.RollCallDefinitionId == 0)
            {
                result.AddError("رکورد نامعتبر", " نوع حضور را انتخاب نمایید");
                return result;
            }
            if (model.RollCallCategoryId == 2)
            {
                //var employeeeTypecode = _kscHrUnitOfWork.ViewMisEmploymentTypeRepository.GetEmployeTypeCode(employeeeDetail.EmploymentTypeId.Value);
                var finded = rollCallDefinition
                    .Where(a => a.RollCallCategoryId == 2 && a.Id == model.RollCallDefinitionId)
                    .Any(x => x.IsValidForAllEmploymentType
                      || x.RollCallEmploymentTypes.Any(a => a.EmploymentTypeCode == model.EmployeeTypeId));
                if (finded == false)
                {
                    result.AddError("رکورد نامعتبر", " کد حضور و غیاب وارد شده نا معتبر است");
                    return result;
                }
            }

            if (model.RollCallCategoryId == 3)
            {
                var finded = rollCallDefinition
                     .Any(a => a.RollCallCategoryId == 3 && a.Id == model.RollCallDefinitionId);
                if (finded == false)
                {
                    result.AddError("رکورد نامعتبر", " کد حضور و غیاب وارد شده نا معتبر است");
                    return result;
                }
            }
            var workCalendars = _Calendars.Where(a => a.MiladiDateV1 >= model.AbsenceStartDate && a.MiladiDateV1 <= model.AbsenceEndDate).ToList();
            var workcalendarIds = workCalendars.Select(a => a.Id).ToList();
            //List<int> codes = new List<int>() { 15, 24, 54, 65 };
            if (!model.LongTermAbsenceCheck)
            {
                var EmployeeAttendAbsenceItems = _EmployeeAttendAbsenceItem.Where(a =>
                a.EmployeeLongTermAbsenceId != model.Id &&
                a.RollCallDefinitionId != 54 &&
                a.EmployeeId == model.EmployeeId &&
                workcalendarIds.Contains(a.WorkCalendarId));
                if (EmployeeAttendAbsenceItems.Any(a => a.MissionId != null))
                {
                    result.AddError("رکورد نامعتبر", "در این بازه کاربر دارای کارکرد ماموریت  می باشد");
                    return result;
                }
                else if (EmployeeAttendAbsenceItems.Any(a => !a.EmployeeEducationTimeId.HasValue))
                {
                    result.AddError("رکورد نامعتبر", "در این بازه کاربر دارای کارکرد آموزش  می باشد");
                    return result;
                }
                else if (EmployeeAttendAbsenceItems.Any())
                {
                    result.AddError("رکورد نامعتبر", "در این بازه کاربر دارای کارکرد می باشد");
                    return result;
                }
            }

            var minMonthSelected = workCalendars.OrderBy(a => a.YearMonthV1).First();

            if (Exists(model.Id, model.AbsenceStartDate.Value, model.AbsenceEndDate.Value, model.EmployeeId))
            {
                result.AddError("رکورد نامعتبر", "این بازه قابل ثبت نیست");
                return result;
            }
            var WorkGroupcode = _EmployeeWorkGroup.FirstOrDefault(a => model.EmployeeId == a.EmployeeId
           && (model.AbsenceStartDate.Value <= a.StartDate && model.AbsenceEndDate.Value >= a.StartDate
           || model.AbsenceStartDate.Value <= a.EndDate && model.AbsenceEndDate.Value >= a.EndDate));
            if (WorkGroupcode != null && WorkGroupcode.WorkGroup.Code != "R")
            {
                var iSHaveShiftBoard = _kscHrUnitOfWork.ShiftBoardRepository.ISHaveShiftBoard(model.EmployeeId, model.AbsenceStartDate.Value, model.AbsenceEndDate.Value);
                if (iSHaveShiftBoard == true)
                {
                    result.AddError("رکورد نامعتبر", "لوحه شیف برای این کاربر ثبت نشده است");
                    return result;
                }
            }
            return result;
        }


        public async Task<KscResult> RemoveEmployeeLongTermAbsences(EditEmployeeLongTermAbsencesModel model)
        {
            var result = model.IsValid();
            if (!result.Success)
                return result;
            var oneemployeeLongTermAbsenceg = GetOne(model.Id);
            if (oneemployeeLongTermAbsenceg == null)
            {
                result.AddError("رکورد حذف شده", "رکورد حذف شده است");
                return result;
            }
            foreach (var item in oneemployeeLongTermAbsenceg.EmployeeAttendAbsenceItems.ToList())
            {
                //var systemStatus = _kscHrUnitOfWork.WorkCalendarRepository.GetDailyTimeSheetStatus(item.WorkCalendarId).GetAwaiter().GetResult();
                //if (systemStatus == EnumSystemSequenceStatusDailyTimeSheet.InActiveForAllUser.Id)
                //{
                //    result.AddError("پیام", "کارکرد بسته شده است");
                //    //throw new HRBusinessException(Validations.RepetitiveId,
                //    //);
                //}

                _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.Delete(item);
            }
            _kscHrUnitOfWork.EmployeeLongTermAbsenceRepository.Delete(oneemployeeLongTermAbsenceg);

            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }
    }
}
