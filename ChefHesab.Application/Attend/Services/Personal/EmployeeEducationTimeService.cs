using AutoMapper;
using Ksc.Hr.Domain.Shared;
using Ksc.HR.Appication.Interfaces.Personal;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Repositories.Personal;
using Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem;
using Ksc.HR.DTO.Personal.EmployeeEducationTime;
using Ksc.HR.DTO.Personal.EmployeeWorkGroups;
using Ksc.HR.DTO.Transfer.Transfer_Request;
using Ksc.HR.DTO.WorkShift.RollCallCategory;
using Ksc.HR.DTO.WorkShift.RollCallConcept;
using Ksc.HR.Resources.Messages;
using Ksc.HR.Share.General;
using Ksc.HR.Share.Model.City;
using Ksc.HR.Share.Model.RollCallDefinication;
using Ksc.HR.Share.Model.SystemSequenceStatus;
using KSC.Common;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using KSC.MIS.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Services.Personal
{
    public class EmployeeEducationTimeService : IEmployeeEducationTimeService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;
        // private readonly IEmployeeWorkGroupRepository _employeeWorkGroupRepository;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;

        public EmployeeEducationTimeService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
        }


        public async Task<ReturnData<RPC005>> SaveEducationTime(RPC005 model)
        {
            var today = DateTime.Now;
            var result = new ReturnData<RPC005>();
            result.Data = model;
            result.IsSuccess = true;

            if (model.ARR.Any() == false)
            {
                result.AddError("لیست نبایستی خالی باشد");
                return result;
            }
            // ولیدشن 1 و 2
            var Date_int = int.Parse(model.DATE);
            var currentUser = model.WinUser;
            var ClassDate_Miladi = _kscHrUnitOfWork.WorkCalendarRepository.Where(x => x.DateKey == Date_int).FirstOrDefault();
            var systemStatus = _kscHrUnitOfWork.WorkCalendarRepository.GetDailyTimeSheetStatus(ClassDate_Miladi.Id).GetAwaiter().GetResult();
            if (systemStatus == EnumSystemSequenceStatusDailyTimeSheet.InActiveForAllUser.Id)
            {
                result.AddError("کارکرد بسته شده است");
                return result;
            }

            var arrAsQueryable = model.ARR;

            //یافتن ایدی های مورد نیاز در جدول پرسنل
            var educationTimeModel =
                                 (from t in arrAsQueryable
                                  join e in _kscHrUnitOfWork.EmployeeRepository.GetEmployeeByRelatedMonthTimeSheet()//.GetEmployee()
                                                                                                                    //.Where(x=> x.DismissalStatusId == null && x.PaymentStatusId == 3) //افراد جاری
                                  on t.NUMP equals e.EmployeeNumber
                                  select new AddEmployeeEducationTimeModel()
                                  {
                                      EmployeeId = e.Id,
                                      EmployeeNumber = e.EmployeeNumber,
                                      TrainingTypeId = int.Parse(t.COD_TYP),
                                      StartTime = t.STIME.Substring(0, 2) + ":" + t.STIME.Substring(2, 2),
                                      EndTime = t.ETIME.Substring(0, 2) + ":" + t.ETIME.Substring(2, 2),//t.ETIME,
                                      ClassDate = ClassDate_Miladi.MiladiDateV1,
                                      WorkCalendarId = ClassDate_Miladi.Id,
                                      COD_TYP = t.COD_TYP,
                                      FLG_MIS = t.FLG_MIS,
                                      WorkTimeId = e.WorkGroup.WorkTimeId,
                                      WorkGroupId = e.WorkGroupId,
                                      WorkCityId = e.WorkCityId,
                                      DismissalStatusId = e.DismissalStatusId,
                                      PaymentStatusId = e.PaymentStatusId,
                                      CurrentUserName = currentUser,
                                  }).ToList();


            List<EmployeeEducationTime> EmployeeEducationTimes = new List<EmployeeEducationTime>();

            // بررسی تک به تک افراد
            foreach (var item in educationTimeModel)
            {
                var itemInModel = result.Data.ARR.FirstOrDefault(x => x.NUMP == item.EmployeeNumber);

                if ((item.PaymentStatusId != 3 && item.PaymentStatusId != 2) && item.DismissalStatusId == null) // اگر کارمند جاری نباشد
                {
                    itemInModel.COD_ERR = "";
                    itemInModel.DES_ERR = itemInModel.DES_ERR + " " + $"کارمند جاری نمیباشد";
                    itemInModel.FLG_ERR = "1";
                    continue;
                }

                var educationTime = _kscHrUnitOfWork.EmployeeEducationTimeRepository.FirstOrDefault(x => x.EmployeeId == item.EmployeeId
                                                                                   && x.WorkCalendarId == item.WorkCalendarId
                                                                                   && x.TrainingTypeId == item.TrainingTypeId
                                                                                   && x.StartTime == item.StartTime
                                                                                   && x.IsDeleted == false
                                                                                   );
                var itemForEmpInDay = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetEmployeeAttendAbsenceItemByRelated()
                            .Where(x => x.InvalidRecord == false
                                     && x.WorkCalendarId == item.WorkCalendarId
                                     && x.EmployeeId == item.EmployeeId);




                //بررسی تکراری بودن
                if (educationTime != null)//اگر در جدول اموزش داده وجود داشت یا پیام تکراری بودن دیتا میدهد یا میخواهد حذف کند
                {
                    if (item.FLG_MIS == "1")
                    { // Delete
                        if (itemForEmpInDay.Any(x => x.EmployeeEducationTimeId == null) == true)
                        {
                            ////در جدول کارکرد داده ای ثبت شده و از نوع آموزش نیست  امکان حذف وجود ندارد
                            // یافتن دیتا و پر کردن  خطا برای مدل خروجی
                            itemInModel.COD_ERR = "";
                            itemInModel.DES_ERR = itemInModel.DES_ERR + " " + $"کارکرد  فرد در این روز توسط سرپرست تایید شده است";
                            itemInModel.FLG_ERR = "1";
                            continue;

                        }
                        else
                        {
                            if (item.TrainingTypeId == EnumTrainingType.AttendanceTraining.Id)//حضوری
                            {
                                //در جدول کارکرد داده ای ثبت شده و از نوع آموزش نباشد امکان حذف وجود دارد
                                educationTime.IsDeleted = true;
                                educationTime.DeletedDate = today;
                                educationTime.DeletedUser = currentUser;

                                _kscHrUnitOfWork.EmployeeEducationTimeRepository.Update(educationTime);
                            }

                            if (item.TrainingTypeId == EnumTrainingType.OnlineTraining.Id)//انلاین
                            {
                                //در جدول کارکرد داده ای ثبت شده و از نوع آموزش نباشد امکان حذف وجود دارد
                                educationTime.IsDeleted = true;
                                educationTime.DeletedDate = today;

                                _kscHrUnitOfWork.EmployeeEducationTimeRepository.Update(educationTime);

                                var employeeAttendAbsenceItem = itemForEmpInDay
                                                                .FirstOrDefault(x => x.EmployeeEducationTimeId == educationTime.Id);
                                if (employeeAttendAbsenceItem != null)//شاید این داده قبلا توسط سرپرست حذف شده
                                {
                                    // حذف داده کارکرد آموزش
                                    _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.Delete(employeeAttendAbsenceItem);
                                }
                            }

                        }

                    }
                    else
                    {
                        // یافتن دیتا و پر کردن  خطا برای مدل خروجی
                        itemInModel.COD_ERR = "";
                        itemInModel.DES_ERR = itemInModel.DES_ERR + " " + $"ساعت آموزش در این روز ساعت {educationTime.StartTime} الی {educationTime.EndTime} قبلا ثبت شده است.";
                        itemInModel.FLG_ERR = "1";
                        continue;
                    }

                }
                else
                {
                    if (item.FLG_MIS == "1")
                    {
                        // یافتن دیتا و پر کردن  خطا برای مدل خروجی
                        itemInModel.COD_ERR = "";
                        itemInModel.DES_ERR = itemInModel.DES_ERR + " " + $"کارکرد آموزشی فرد در پرسنلی حذف شده است";
                        itemInModel.FLG_ERR = "1";
                        continue;
                    }
                    else
                    {
                        //add
                        if (item.TrainingTypeId == EnumTrainingType.AttendanceTraining.Id)//حضوری
                        {
                            //// بررسی شهر اهواز برای کلاسهای حضوری
                            //if(item.WorkCityId != EnumCity.Ahvaz.Id)
                            //{
                            //    // یافتن دیتا و پر کردن  خطا برای مدل خروجی
                            //    itemInModel.COD_ERR = "";
                            //    itemInModel.DES_ERR = itemInModel.DES_ERR + " " + $"شهر محل خدمت اهواز نمیباشد،بایستی ماموریت آموزشی ثبت شود.";
                            //    itemInModel.FLG_ERR = "1";
                            //    continue;
                            //}


                            // ولیدشن 6 آموزش
                            //6-	برای آموزش حضوری  کنترل می شود فرد در آن روز رکوردی در کارکرد روزانه داشته باشد ثبت انجام نمیشود
                            //  بعنوان مثال : ماموریت ،استعلاجی،مرخصی و ...

                            var itemForEmpInDayHasMission = itemForEmpInDay.Any(x => x.RollCallDefinition.RollCallCategoryId == EnumRollCallCategory.Mission.Id);
                            if (itemForEmpInDayHasMission == true) // اگر ماموریت داشته باشد امکان ثبت آموزش وجود ندارد
                            {
                                // اعلام داشتن ماموریت
                                // یافتن دیتا و پر کردن  خطا برای مدل خروجی
                                itemInModel.COD_ERR = "";
                                itemInModel.DES_ERR = itemInModel.DES_ERR + " " + $"در این تاریخ ماموریت ثبت شده دارد.";
                                itemInModel.FLG_ERR = "1";
                                continue;
                            }
                            var itemForEmpInDayHasLongTermAbsence = itemForEmpInDay.Any(x => x.EmployeeLongTermAbsenceId != null);
                            if (itemForEmpInDayHasLongTermAbsence == true) // اگر مرخصی طولانی مدت داشته باشد امکان ثبت آموزش وجود ندارد
                            {
                                // اعلام داشتن مرخصی طولانی مدت
                                // یافتن دیتا و پر کردن  خطا برای مدل خروجی
                                itemInModel.COD_ERR = "";
                                itemInModel.DES_ERR = itemInModel.DES_ERR + " " + $"در این تاریخ مرخصی طولانی مدت ثبت شده دارد.";
                                itemInModel.FLG_ERR = "1";
                                continue;
                            }

                            // کلاسهای حضوری
                            var tempData = _mapper.Map<EmployeeEducationTime>(item);
                            EmployeeEducationTimes.Add(tempData);


                        }
                        else
                        { // کلاسهای بجز حضوری
                            var shiftConceptDetailIdEachPerson = await _kscHrUnitOfWork.ShiftConceptDetailRepository.GetShiftConceptDetailWithWOrkGroupIdDate(item.WorkGroupId.Value, item.WorkCalendarId.Value);

                            var timeSettingDataModel = await _kscHrUnitOfWork.TimeShiftSettingRepository
                                .GetShiftDateTimeSettingAsync(item.EmployeeId.Value, shiftConceptDetailIdEachPerson.Id, item.WorkCityId.Value, item.WorkGroupId.Value, item.WorkCalendarId.Value);
                            // کلاسی که قبل از شروع شیفت و یا بعد از پایان شیفت نباشد
                            //خطا دهد
                            if (timeSettingDataModel.IsRestShift == false &&
                               !((timeSettingDataModel.ShiftEndDate.Date == item.ClassDate.Value.Date && item.StartTime.ConvertStringToTimeSpan() >= timeSettingDataModel.ShiftEndTime.ConvertStringToTimeSpan())
                                || item.EndTime.ConvertStringToTimeSpan() <= timeSettingDataModel.ShiftStartTime.ConvertStringToTimeSpan()))
                            {

                                itemInModel.COD_ERR = "";
                                itemInModel.DES_ERR = itemInModel.DES_ERR + " " + $"کلاس در بازه شیفت کاری می باشد.";
                                itemInModel.FLG_ERR = "1";
                                continue;
                            }
                            //EmployeeEducationTime tempData = new EmployeeEducationTime()
                            //{
                            //    EmployeeId = item.Id,
                            //    EmployeeNumber = item.EmployeeNumber,
                            //    TrainingTypeId = int.Parse(item.COD_TYP),
                            //    StartTime = item.StartTime,
                            //    EndTime = item.EndTime,
                            //    ClassDate = item.ClassDate,
                            //    WorkCalendarId = item.WorkCalendarId,
                            //};
                            var tempData = _mapper.Map<EmployeeEducationTime>(item);

                            var addTempData = false;
                            if (item.TrainingTypeId == EnumTrainingType.OnlineTraining.Id)//انلاین
                            {

                                var itemForEmpInDayHasMission = itemForEmpInDay.Any(x => x.RollCallDefinition.RollCallCategoryId == EnumRollCallCategory.Mission.Id);
                                if (itemForEmpInDay.Any())  // داده داشته باشد کلاسهای انلاین را در جدول کارکرد مستقیما ثبت میکنیم
                                {
                                    if (itemForEmpInDayHasMission == true) // اگر ماموریت داشته باشد امکان ثبت آموزش انلاین وجود ندارد
                                    {
                                        // اعلام داشتن ماموریت
                                        // یافتن دیتا و پر کردن  خطا برای مدل خروجی
                                        itemInModel.COD_ERR = "";
                                        itemInModel.DES_ERR = itemInModel.DES_ERR + " " + $"در این تاریخ ماموریت ثبت شده دارد.";
                                        itemInModel.FLG_ERR = "1";
                                        continue;
                                    }

                                    var resultModel = itemForEmpInDay.ToList();
                                    List<TrainingModel> trainingModel = new List<TrainingModel>();
                                    List<TrainingModel> trainingModelOverTime = new List<TrainingModel>();
                                    var resultModelOverTime = resultModel.Where(x => x.RollCallDefinition.RollCallConceptId == EnumRollCallConcept.OverTime.Id).ToList();
                                    if (resultModelOverTime.Count() != 0) // اضافه کار داشته باشد
                                    {
                                        resultModel = resultModel.Except(resultModelOverTime).ToList();
                                        // اضافه کاریهای قبل از آموزش
                                        List<EmployeeAttendAbsenceItem> resultBeforeTrain = new List<EmployeeAttendAbsenceItem>();
                                        List<EmployeeAttendAbsenceItem> resultAfterTrain = new List<EmployeeAttendAbsenceItem>();
                                        trainingModelOverTime.Add(new TrainingModel() { StartTime = item.StartTime, EndTime = item.EndTime });
                                        resultBeforeTrain = resultModelOverTime.Where(x => x.StartTime.ConvertStringToTimeSpan() > x.EndTime.ConvertStringToTimeSpan() && !trainingModelOverTime.Any(e => e.StartTime.ConvertStringToTimeSpan() < x.EndTime.ConvertStringToTimeSpan() || e.EndTime.ConvertStringToTimeSpan() < x.EndTime.ConvertStringToTimeSpan())).ToList();
                                        resultAfterTrain = resultModelOverTime.Where(x => !trainingModelOverTime.Any(e => x.WorkCalendarId == item.WorkCalendarId
                                       && (x.StartTime.ConvertStringToTimeSpan() < e.StartTime.ConvertStringToTimeSpan() || x.StartTime.ConvertStringToTimeSpan() < e.EndTime.ConvertStringToTimeSpan()))).ToList();
                                        // 
                                        var resultForTrain = resultModelOverTime.Except(resultBeforeTrain).Except(resultAfterTrain);

                                        foreach (var overTime in resultForTrain)
                                        {

                                            //زمان شروع کلاس قبل از شروع اضافه کار باشد
                                            if (item.StartTime.ConvertStringToTimeSpan() < overTime.StartTime.ConvertStringToTimeSpan())
                                            {
                                                //زمان پایان کلاس قبل از شروع اضافه کار باشد
                                                if (item.EndTime.ConvertStringToTimeSpan() < overTime.StartTime.ConvertStringToTimeSpan())
                                                {
                                                    if (!trainingModel.Any(x => x.StartTime == item.StartTime))
                                                    {
                                                        trainingModel.Add(new TrainingModel() { StartTime = item.StartTime, EndTime = item.EndTime });
                                                        addTempData = true;
                                                        break;

                                                    }
                                                }
                                                else //زمان پایان کلاس بعد از شروع اضافه کار باشد
                                                {
                                                    if (!trainingModel.Any(x => x.StartTime == item.StartTime)) // 
                                                    {
                                                        trainingModel.Add(new TrainingModel() { StartTime = item.StartTime, EndTime = overTime.StartTime });
                                                        // پایان کلاس بعد از پایان اضافه کار باشد
                                                        if (overTime.StartTime.ConvertStringToTimeSpan() < overTime.EndTime.ConvertStringToTimeSpan())
                                                        {
                                                            if (item.EndTime.ConvertStringToTimeSpan() >= overTime.EndTime.ConvertStringToTimeSpan())
                                                            {
                                                                var updateEmployeeAttendAbsenceItem = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetById(overTime.Id);
                                                                updateEmployeeAttendAbsenceItem.StartTime = item.StartTime;
                                                                updateEmployeeAttendAbsenceItem.EndTime = item.EndTime;
                                                                updateEmployeeAttendAbsenceItem.TimeDuration = Utility.GetDurationStartTimeToEndTime(item.StartTime, item.EndTime);
                                                                updateEmployeeAttendAbsenceItem.RollCallDefinitionId = EnumRollCallDefinication.OnlineTrainingExtraWork.Id;
                                                                updateEmployeeAttendAbsenceItem.InsertUser = currentUser;
                                                                tempData.EmployeeAttendAbsenceItems.Add(updateEmployeeAttendAbsenceItem);
                                                                break;
                                                            }
                                                            else // پایان کلاس قبل از پایان اضافه کار باشد
                                                            {
                                                                var updateEmployeeAttendAbsenceItem = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetById(overTime.Id);
                                                                var overTimeRollCallDefinitionId = updateEmployeeAttendAbsenceItem.RollCallDefinitionId;
                                                                var overTimeEndTime = updateEmployeeAttendAbsenceItem.EndTime;
                                                                updateEmployeeAttendAbsenceItem.StartTime = item.StartTime;
                                                                updateEmployeeAttendAbsenceItem.EndTime = item.EndTime;
                                                                updateEmployeeAttendAbsenceItem.TimeDuration = Utility.GetDurationStartTimeToEndTime(item.StartTime, item.EndTime);
                                                                updateEmployeeAttendAbsenceItem.RollCallDefinitionId = EnumRollCallDefinication.OnlineTrainingExtraWork.Id;
                                                                updateEmployeeAttendAbsenceItem.InsertUser = currentUser;
                                                                tempData.EmployeeAttendAbsenceItems.Add(updateEmployeeAttendAbsenceItem);

                                                                //
                                                                EmployeeAttendAbsenceItem newEmployeeAttendAbsenceItem = new EmployeeAttendAbsenceItem()
                                                                {
                                                                    EmployeeId = tempData.EmployeeId.Value,
                                                                    WorkTimeId = item.WorkTimeId.Value,
                                                                    WorkCalendarId = tempData.WorkCalendarId.Value,
                                                                    RollCallDefinitionId = overTimeRollCallDefinitionId,
                                                                    ShiftConceptDetailId = updateEmployeeAttendAbsenceItem.ShiftConceptDetailId,
                                                                    IsManual = false,
                                                                    StartTime = item.EndTime,
                                                                    EndTime = overTimeEndTime,
                                                                    TimeDuration = Utility.GetDurationStartTimeToEndTime(item.EndTime, overTimeEndTime),
                                                                    InsertDate = DateTime.Now,
                                                                    InsertUser = currentUser,
                                                                };
                                                                _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.Add(newEmployeeAttendAbsenceItem);
                                                                //
                                                                break;
                                                            }
                                                        }
                                                    }

                                                }
                                            }
                                            //زمان شروع کلاس  بعد از شروع اضافه کار باشد
                                            else
                                            {
                                                // زمان شروع کلاس قبل از پایان اضافه کار باشد

                                                if (overTime.StartTime.ConvertStringToTimeSpan() < overTime.EndTime.ConvertStringToTimeSpan()
                                                    && item.StartTime.ConvertStringToTimeSpan() < overTime.EndTime.ConvertStringToTimeSpan())
                                                {
                                                    var updateEmployeeAttendAbsenceItem = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetById(overTime.Id);
                                                    var overTimeRollCallDefinitionId = updateEmployeeAttendAbsenceItem.RollCallDefinitionId;
                                                    var overTimeEndTime = updateEmployeeAttendAbsenceItem.EndTime;
                                                    if (!trainingModel.Any(x => x.StartTime == item.StartTime))
                                                    {
                                                        trainingModel.Add(new TrainingModel() { StartTime = item.StartTime, EndTime = item.EndTime });
                                                        //اصلاح اضافه کار
                                                        if (item.StartTime.ConvertStringToTimeSpan() > overTime.StartTime.ConvertStringToTimeSpan())
                                                        {

                                                            updateEmployeeAttendAbsenceItem.EndTime = item.StartTime;
                                                            updateEmployeeAttendAbsenceItem.TimeDuration = Utility.GetDurationStartTimeToEndTime(updateEmployeeAttendAbsenceItem.StartTime, item.StartTime);
                                                            updateEmployeeAttendAbsenceItem.UpdateUser = currentUser;
                                                            updateEmployeeAttendAbsenceItem.UpdateDate = today;
                                                        }

                                                        // پایان کلاس قبل از پایان اضافه کاری باشد
                                                        if (item.EndTime.ConvertStringToTimeSpan() < overTime.EndTime.ConvertStringToTimeSpan())
                                                        {
                                                            //
                                                            EmployeeAttendAbsenceItem newEmployeeAttendAbsenceItem = new EmployeeAttendAbsenceItem()
                                                            {
                                                                EmployeeId = tempData.EmployeeId.Value,
                                                                WorkTimeId = item.WorkTimeId.Value,
                                                                WorkCalendarId = tempData.WorkCalendarId.Value,
                                                                RollCallDefinitionId = EnumRollCallDefinication.OnlineTrainingExtraWork.Id,
                                                                ShiftConceptDetailId = shiftConceptDetailIdEachPerson.Id,
                                                                IsManual = false,
                                                                StartTime = item.StartTime,
                                                                EndTime = item.EndTime,
                                                                TimeDuration = Utility.GetDurationStartTimeToEndTime(item.StartTime, item.EndTime),
                                                                InsertDate = DateTime.Now,
                                                                InsertUser = currentUser,
                                                            };
                                                            tempData.EmployeeAttendAbsenceItems.Add(newEmployeeAttendAbsenceItem);
                                                            //
                                                            //
                                                            var overEmployeeAttendAbsenceItem = new EmployeeAttendAbsenceItem()
                                                            {
                                                                EmployeeId = tempData.EmployeeId.Value,
                                                                WorkTimeId = item.WorkTimeId.Value,
                                                                WorkCalendarId = tempData.WorkCalendarId.Value,
                                                                RollCallDefinitionId = overTimeRollCallDefinitionId,
                                                                ShiftConceptDetailId = updateEmployeeAttendAbsenceItem.ShiftConceptDetailId,
                                                                IsManual = false,
                                                                StartTime = item.EndTime,
                                                                EndTime = overTimeEndTime,
                                                                TimeDuration = Utility.GetDurationStartTimeToEndTime(item.EndTime, overTimeEndTime),
                                                                InsertDate = DateTime.Now,
                                                                InsertUser = currentUser,
                                                            };
                                                            _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.Add(overEmployeeAttendAbsenceItem);
                                                            //
                                                        }
                                                        else // پایان کلاس  بعد از پایان اضافه کاری باشد
                                                        {
                                                            EmployeeAttendAbsenceItem newEmployeeAttendAbsenceItem = new EmployeeAttendAbsenceItem()
                                                            {
                                                                EmployeeId = tempData.EmployeeId.Value,
                                                                WorkTimeId = item.WorkTimeId.Value,
                                                                WorkCalendarId = tempData.WorkCalendarId.Value,
                                                                RollCallDefinitionId = EnumRollCallDefinication.OnlineTrainingExtraWork.Id,
                                                                ShiftConceptDetailId = shiftConceptDetailIdEachPerson.Id,
                                                                IsManual = false,
                                                                StartTime = item.StartTime,
                                                                EndTime = item.EndTime,
                                                                TimeDuration = Utility.GetDurationStartTimeToEndTime(item.StartTime, item.EndTime),
                                                                InsertDate = DateTime.Now,
                                                                InsertUser = currentUser,
                                                            };
                                                            tempData.EmployeeAttendAbsenceItems.Add(newEmployeeAttendAbsenceItem);
                                                        }
                                                        break;
                                                    }
                                                }
                                            }

                                        }


                                    }

                                    if (trainingModel.Count() == 0)
                                    {
                                        var addEmployeeAttendAbsenceItem = new EmployeeAttendAbsenceItem()
                                        {
                                            EmployeeId = tempData.EmployeeId.Value,
                                            WorkTimeId = item.WorkTimeId.Value,
                                            WorkCalendarId = tempData.WorkCalendarId.Value,
                                            RollCallDefinitionId = EnumRollCallDefinication.OnlineTrainingExtraWork.Id,
                                            ShiftConceptDetailId = shiftConceptDetailIdEachPerson.Id,
                                            IsManual = false,
                                            StartTime = tempData.StartTime,
                                            EndTime = tempData.EndTime,
                                            TimeDuration = Utility.GetDurationStartTimeToEndTime(tempData.StartTime, tempData.EndTime),
                                            InsertDate = DateTime.Now,
                                            InsertUser = currentUser,
                                        };
                                        tempData.EmployeeAttendAbsenceItems.Add(addEmployeeAttendAbsenceItem);
                                    }
                                }


                            }
                            EmployeeEducationTimes.Add(tempData);
                        }

                    }
                }
            }

            try
            {
                if (EmployeeEducationTimes.Any())
                    _kscHrUnitOfWork.EmployeeEducationTimeRepository.AddRange(EmployeeEducationTimes);


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

        public FilterResult<EmployeeEducationTimeModel> GetEmployeeAducationDontHaveItem(SearchEmployeeEducationModel filterDate)
        {
            var employees = GetEmployees(filterDate);
            var getAttenItemYearMonth = _kscHrUnitOfWork.SystemControlDateRepository.GetActiveData().AttendAbsenceItemDate;
            var workCalendar= _kscHrUnitOfWork.WorkCalendarRepository.Where(a => a.YearMonthV1 == getAttenItemYearMonth);
            var workCalendarIds=workCalendar.Select(a => a.Id).ToList();
            var ducationNotInAttendItem=_kscHrUnitOfWork.ViewNotConfirmedEducationRepository.GetViewNotConfirmedEducationAsNoTracking();
            var finalList = (from education in ducationNotInAttendItem
                             where employees.Contains(education.EmployeeId.Value) && workCalendarIds.Contains(education.WorkCalendarId.Value)
                             select new EmployeeEducationTimeModel()
                             {
                                 WorkCalendarId = education.WorkCalendarId,
                                 EmployeeNumber = education.EmployeeNumber,
                                 EmployeeId = education.EmployeeId,
                             }
                           ).ToList();
            var resultFinalList = _FilterHandler.GetFilterResult<EmployeeEducationTimeModel>(finalList, filterDate, "WorkCalendarDate");
            var finalListWorkCalendar = resultFinalList.Data.Select(a => a.WorkCalendarId);
            var finalworkCalendar = workCalendar.Where(a => finalListWorkCalendar.Contains(a.Id)).ToList();
            var finalEmployeeIDs = resultFinalList.Data.Where(a=>a.EmployeeId.HasValue).Select(a => a.EmployeeId.Value).ToList();
            var finalEmployee = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeIncludedTeamwork(finalEmployeeIDs);
            foreach (var item in resultFinalList.Data)
            {
                var employee = finalEmployee.First(a => a.Id == item.EmployeeId);
                var workcalendar = finalworkCalendar.First(a => a.Id == item.WorkCalendarId);
                item.FullName = employee.Name + " " + employee.Family;
                item.WorkCalendarDate = workcalendar.MiladiDateV1;
            }
            return new FilterResult<EmployeeEducationTimeModel>()
            {
                Data = resultFinalList.Data.ToList(),
                Total = resultFinalList.Total

            };


        }

        private List<int> GetEmployees(SearchEmployeeEducationModel Filter)
        {
            var query_ViewMisEmployeeSecurity = _kscHrUnitOfWork.ViewEmployeeTeamUserActiveRepository.GetAllAsNoTracking();

            if (Filter.FromTeam > 0)
                query_ViewMisEmployeeSecurity = query_ViewMisEmployeeSecurity.Where(x => x.Code.CompareTo(Filter.FromTeam.ToString()) >= 0);
            if (Filter.ToTeam > 0)
                query_ViewMisEmployeeSecurity = query_ViewMisEmployeeSecurity.Where(x => x.Code.CompareTo(Filter.ToTeam.ToString()) <= 0);




            if (!Filter.IsOfficialAttendAbcense)
            {
                query_ViewMisEmployeeSecurity = query_ViewMisEmployeeSecurity.Where(x => (x.DisplaySecurity == 1 || x.DisplaySecurity == 2) && x.WindowsUser.ToLower() == Filter.CurrentUserName);
            }
            return query_ViewMisEmployeeSecurity.Select(a => a.Id).ToList();

        }

    }
}
