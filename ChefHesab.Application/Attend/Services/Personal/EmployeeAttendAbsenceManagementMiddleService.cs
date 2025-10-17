using AutoMapper;
using DNTPersianUtils.Core;
using Ksc.Hr.Domain.Shared;
using Ksc.HR.Appication.Interfaces.Personal;
using Ksc.HR.Appication.Interfaces.WorkShift;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Repositories.Personal;
using Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem;
using Ksc.HR.DTO.Personal.EmployeeEntryExit;
using Ksc.HR.DTO.Personal.EmployeeLongTermAbsences;
using Ksc.HR.DTO.Personal.EmployeeWorkGroups;
using Ksc.HR.DTO.Transfer.Transfer_Request;
using Ksc.HR.DTO.WorkShift.IncludedDefinition;
using Ksc.HR.DTO.WorkShift.RollCallCategory;
using Ksc.HR.Resources.Messages;
using Ksc.HR.Share.Extention;
using Ksc.HR.Share.General;
using Ksc.HR.Share.Model.SystemSequenceStatus;
using KSC.Common;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Services.Personal
{
    public class EmployeeAttendAbsenceManagementMiddleService1 : IEmployeeAttendAbsenceManagementMiddleService1
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;
        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;


        public EmployeeAttendAbsenceManagementMiddleService1(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;
            _mapper = mapper;
            _FilterHandler = FilterHandler;


        }

        public KscResult AddUpdateEmplyeeAttendAbsenseItems(List<AddEmployeeLongTermAbsencesModel> models)
        {
            var result = new KscResult();
            var Ids = models.Where(a => a.Id > 0).Select(a => a.Id).ToList();
            
            if (Ids.Any())
            {
                var getAttendAbccense = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetAllQuaryble(new List<long>());
                var DeletedAttendAbccenceItem = getAttendAbccense.Where(a=>Ids.All(c=>c!=a.Id)).ToList();
                foreach (var attenItemForDeleted in DeletedAttendAbccenceItem)
                {
                    _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.Delete(attenItemForDeleted);
                }               

            }
            foreach (var model in models)
            {
                if (model.Id > 0)
                {
                    result = UpdateEmplyeeAttendAbsenseItems(model);
                }
                else
                {
                    result = AddEmplyeeAttendAbsenseItems(model);
                }
            }
            return result;

        }

        private KscResult AddEmplyeeAttendAbsenseItems(AddEmployeeLongTermAbsencesModel model)
        {
            var result = new KscResult();
            var employeeAttendAbsenceItem = _mapper.Map<EmployeeAttendAbsenceItem>(model);
            _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.Add(employeeAttendAbsenceItem);
            _kscHrUnitOfWork.SaveAsync().GetAwaiter().GetResult();
            return result;
        }
        private KscResult UpdateEmplyeeAttendAbsenseItems(AddEmployeeLongTermAbsencesModel model)
        {
            var result = new KscResult();
            var findData = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetById(model.Id);
            if (findData != null)
            {
                throw new HRBusinessException(Validations.NotFound, "رکورد حذف شده است");
            }
            var employeeAttendAbsenceItem = _mapper.Map<EmployeeAttendAbsenceItem>(model);
            _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.Update(employeeAttendAbsenceItem);
            _kscHrUnitOfWork.SaveAsync().GetAwaiter().GetResult();
            return result;
           
        }

        public KscResult RemoveAttendAbcenceItem(AddEmployeeLongTermAbsencesModel model)
        {
            var result = new KscResult();
            var findData = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetById(model.Id);
            if (findData != null)
            {
                throw new HRBusinessException(Validations.NotFound, "رکورد حذف شده است");
            }
            var employeeAttendAbsenceItem = _mapper.Map<EmployeeAttendAbsenceItem>(model);
            _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.Delete(employeeAttendAbsenceItem);
            _kscHrUnitOfWork.SaveAsync().GetAwaiter().GetResult();
            return result;
        }

        public async Task<KscResult> AddLogTermsAbsenceItemForMisApi(AddEmployeeLongTermAbsencesModel item)
        {
            var result = new KscResult();
            try
            {
                var wokgroup = _kscHrUnitOfWork.EmployeeWorkGroupRepository.GetEmployeeWorkGroupInclouded().FirstAsync(a => a.Employee.EmployeeNumber == item.PersonalNumbers).GetAwaiter().GetResult();

                var GeorgianAbsenceStartDate = item.ShamsiAbsenceStartDate.Fa2En().ToGregorianDateTime(); 
                var GeorgianAbsenceEndDate = item.ShamsiAbsenceEndDate.Fa2En().ToGregorianDateTime();
               
                var workCalendars = _kscHrUnitOfWork.WorkCalendarRepository.GetWorkCalendarQuerable().Where(a => a.MiladiDateV1 >= GeorgianAbsenceStartDate && a.MiladiDateV1 <= GeorgianAbsenceEndDate).ToListAsync().GetAwaiter().GetResult();
                var workcalendarIds = workCalendars.Select(a => a.Id).ToList();
                var shiftConcepts = _kscHrUnitOfWork.ShiftConceptDetailRepository.GetShiftConceptDetailWithWOrkGroupIdDates(wokgroup.WorkGroup.WorkTimeId, workcalendarIds).GetAwaiter().GetResult();
                var startDate = item.AbsenceStartDate;
                var workCity = _kscHrUnitOfWork.EmployeeRepository.GetByIdAsync(wokgroup.EmployeeId).GetAwaiter().GetResult();
                var startTime = DateTime.Now;
                var endTime = DateTime.Now;
                foreach (var calendarId in workcalendarIds)
                {
                    var systemStatus = _kscHrUnitOfWork.WorkCalendarRepository.GetDailyTimeSheetStatus(calendarId).GetAwaiter().GetResult();
                    if (systemStatus == EnumSystemSequenceStatusDailyTimeSheet.InActiveForAllUser.Id)
                    {
                        throw new HRBusinessException(Validations.RepetitiveId,
                      "کارکرد بسته شده است");
                    }
                    var workCalendar = workCalendars.First(a => a.Id == calendarId);
                    var isFind = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.Any(a => a.WorkCalendarId == calendarId);
                    if (isFind)
                    {
                        result.AddError("", "تایید کارکرد در روز" + workCalendar.ShamsiDateV1);
                        return result;
                    }
                 
                    var shiftConceptDetailId = 0;
                    if (wokgroup.WorkGroup.WorkTime.ShiftSettingFromShiftboard)
                    {
                        shiftConceptDetailId = shiftConcepts.First(x => x.ShiftBoards.Any(a => a.WorkCalendarId == calendarId)).Id;
                    }
                    else
                    {
                        var workTimeShiftConcept = wokgroup.WorkGroup.WorkTime.WorkTimeShiftConcepts.FirstOrDefault(x => x.IsActive == true);
                        if (workTimeShiftConcept != null)
                            shiftConceptDetailId = workTimeShiftConcept.ShiftConcept.ShiftConceptDetails.First(x => x.IsActive == true).Id;
                    }
                    var getstartAndEndTimeShift = _kscHrUnitOfWork.EmployeeLongTermAbsenceRepository.GetShiftStartEndTime(shiftConceptDetailId, workCity.WorkCityId.Value, wokgroup.WorkGroupId, calendarId).GetAwaiter().GetResult();
                    //مدت زمان شیف باید بدست بیاید
                    var addEmployeeAttendAbsenceItem = new EmployeeAttendAbsenceItem()
                    {
                        EmployeeId = item.EmployeeId,
                        WorkTimeId = wokgroup.WorkGroup.WorkTimeId,
                        WorkCalendarId = calendarId,
                        RollCallDefinitionId = item.RollCallDefinitionId,
                        ShiftConceptDetailId = shiftConceptDetailId,
                        IsManual = true,
                        StartTime = getstartAndEndTimeShift.Item1,
                        EndTime = getstartAndEndTimeShift.Item2,
                        TimeDuration = getstartAndEndTimeShift.Item3,
                        InsertDate = DateTime.Now,
                        InsertUser = item.InsertUser
                    };
                    //مدت زمان شیف باید بدست بیاید
                    _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.Add(addEmployeeAttendAbsenceItem);

                }
               await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new HRBusinessException(Validations.RepetitiveId,"عملیات نا موفق بود");
            }


            return result;
        }
     

    }
}
