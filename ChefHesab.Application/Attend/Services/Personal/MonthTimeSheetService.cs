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
using Ksc.HR.DTO.Stepper;
using Ksc.HR.Domain.Entities.Stepper;
using Ksc.HR.Application.Interfaces;
using Ksc.HR.Share.Model.PersonalType;
using Ksc.HR.DTO.Personal.EmployeeTimeSheet;
using Microsoft.EntityFrameworkCore;
using KSC.Domain;
using Ksc.HR.DTO.WorkShift.RollCallConcept;
using Ksc.HR.Share.Model;
using Ksc.HR.Share.Model.Pay;
using Ksc.HR.Share.Model.PaymentStatus;
using Ksc.HR.Domain.Entities.Workshift;

namespace Ksc.HR.Appication.Services.Personal
{
    public class MonthTimeSheetService : IMonthTimeSheetService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;

        private readonly IMisUpdateService _misUpdateService;
        private readonly IStepper_ProcedureService _procedureService;
        private readonly IDbTransaction _dbTransaction;
        public MonthTimeSheetService(IKscHrUnitOfWork kscHrUnitOfWork
    , IMapper mapper
    , IFilterHandler FilterHandler
    , IMisUpdateService MisUpdateService
   , IStepper_ProcedureService procedureService
,
IDbTransaction dbTransaction)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;
            _mapper = mapper;
            _FilterHandler = FilterHandler;
            _misUpdateService = MisUpdateService;
            _procedureService = procedureService;
            _dbTransaction = dbTransaction;
        }
        //TestSendFileStreamMIS
        public async Task<KscResult> TestSendFileStreamMIS(SearchMonthTimeSheetModel model)
        {
            var result = new KscResult();

            try
            {
                var PER_MONT_File = new StringBuilder();
                PER_MONT_File.Append("a|vv|ewr" + System.Environment.NewLine);
                PER_MONT_File.AppendLine("a213|v324v|e2342wr");
                PER_MONT_File.AppendLine("a768|v231321|edfgdgfr");
                var contentFile = System.Text.Encoding.UTF8.GetBytes(PER_MONT_File.ToString());


                //"http://wapi.ksc.ir/MisService/api/IT/SendFileStream";
                var uri = Utility.ServerPathSendFileStream;
                //Byte[] bytes = File.ReadAllBytes(path);
                String _file = Convert.ToBase64String(contentFile);

                using (var client = new HttpClient())
                {
                    string env;
#if DEBUG
                    env = "M";
#else
                        env = "L";
#endif

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    SendFile sendFileModel = new SendFile { application = "PER", file = _file, filename = "test_File.TXT", enviroment = env };
                    var postTask = client.PostAsJsonAsync<SendFile>(uri, sendFileModel);
                    postTask.Wait();


                    var resultApi = postTask.Result;
                    if (resultApi.IsSuccessStatusCode)
                    {
                        var returnValue = resultApi.Content.ReadAsStringAsync();

                        var modelObj = JsonConvert.DeserializeObject<ReturnData<string>>(returnValue.Result);
                        if (modelObj.IsSuccess == true)
                        {

                        }
                        else
                        {
                            throw new HRBusinessException(Validations.RepetitiveId, " فایل  ایجاد نشد");
                        }
                    }
                    else
                    {
                        throw new HRBusinessException(Validations.RepetitiveId, "ارتباط با سیستم  برقرار نشد");
                    }
                }
            }
            catch (Exception ex)
            {

                result.AddError("", ex.Message);
            }

            return result;
        }
        public async Task<KscResult> MonthTimeSheetCalculate(SearchMonthTimeSheetModel model)
        {
            var result = new KscResult();
            try
            {
                var YearMonth = int.Parse(model.DateTimeSheet);
                //var workCalendar = _kscHrUnitOfWork.WorkCalendarRepository.GetByMiladiDate(model.DateTimeSheet.Value);
                var systemStatus = await _kscHrUnitOfWork.WorkCalendarRepository.GetMonthTimeSheetStatus(YearMonth);
                if (systemStatus == true)//کاربران اداری سیستم بسته شده باشد
                {
                    if (_kscHrUnitOfWork.MonthTimeSheetRepository.Any(x => x.YearMonth == YearMonth && x.IsCreatedManual == false))
                    {
                        throw new HRBusinessException(Validations.RepetitiveId, "تایم شیت ماه انتخابی ایجاد شده است");
                    }

                    if (_kscHrUnitOfWork.MonthTimeSheetLogRepository.CheckRequiredStepForMonthSheet(YearMonth) == false)
                    {
                        throw new HRBusinessException(Validations.RepetitiveId, "لطفا مراحل کارکرد ماهیانه را کامل انجام دهید");
                    }
                    //
                    #region محاسبه





                    // افرادی که اضافه کار مازاد سقف دارند
                    var employeeTimeSheetExcessOverTime = _kscHrUnitOfWork.EmployeeTimeSheetRepository.GetEmployeeTimeSheetExcessOverTime(YearMonth).ToList();
                    //

                    // افرادی که اضافه کار تعدیل میانگین دارند
                    var employeeTimeSheetAverageBalanceOverTime = employeeTimeSheetExcessOverTime.Where(x => x.AverageBalanceOverTime != null).ToList();
                    // _kscHrUnitOfWork.EmployeeTimeSheetRepository.GetEmployeeTimeSheetExcessOverTime(YearMonth).Where(x=>x.AverageBalanceOverTime != null).ToList();
                    //

                    var attendAbcenseItem = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetMonthTimeSheetCalculateAsNoTracking(YearMonth);
                    var MIS_Persons = attendAbcenseItem.Select(x => x.EmployeeId).Distinct().ToList();
                    //var MIS_Persons = _kscHrUnitOfWork.ViewMisEmployeeRepository.WhereQueryable(x => x.HrMonthTimeSheet == 1)
                    //    .Select(x => x.EmployeeId).ToList();

                    var CreatedManualPersons = _kscHrUnitOfWork.MonthTimeSheetRepository.WhereQueryable(x => x.YearMonth == YearMonth && x.IsCreatedManual == true)
                        .Select(x => x.EmployeeId).ToList();

                    var Persons = MIS_Persons.Except(CreatedManualPersons).ToList(); //یافتن افرادی که بایستی تایم شیت اتوماتیک برایشان ساخته شود

                    var Emp = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeIncludedTeamwork(Persons)
                        //.Where(x=>x.Id == 4500)
                        .Where(x => x.WorkGroupId != null)
                        .Select(x => new
                        {
                            EmployeeId = x.Id,
                            PaymentStatusId = x.PaymentStatusId,
                            WorkGroupId = x.WorkGroupId
                        });

                    //MonthTimeSheetRollCall
                    var query_MonthTimeSheetRollCall = (from item in attendAbcenseItem.Where(x => x.InvalidRecord == false)
                                                        group item by new
                                                        {
                                                            RollCallDefinitionId = item.RollCallDefinitionId,
                                                            EmployeeId = item.EmployeeId
                                                        } into newgroup
                                                        select new EmployeeItemGroupModel()
                                                        {
                                                            RollCallDefinitionId = newgroup.Key.RollCallDefinitionId,
                                                            EmployeeId = newgroup.Key.EmployeeId,
                                                            TotalDuration = newgroup.Sum(x => (x.TimeDurationInMinute ?? 0) + (x.IncreasedTimeDuration ?? 0)),
                                                            DayCountInDailyTimeSheet = newgroup.Select(x => x.WorkCalendarId).Distinct().Count()
                                                        }).ToList();
                    //MonthTimeSheetDraft
                    var query_MonthTimeSheetDraft = (from item in _kscHrUnitOfWork.MonthTimeSheetDraftRepository.GetMonthTimeSheetDraftByYearMonth(YearMonth)
                                                     group item by new
                                                     {
                                                         EmployeeId = item.EmployeeId,
                                                         YearMonth = item.YearMonth
                                                     } into newgroup
                                                     select new EmployeeItemGroupModel()
                                                     {
                                                         RollCallDefinitionId = 7,
                                                         EmployeeId = newgroup.Key.EmployeeId,
                                                         TotalDuration = newgroup.Sum(x => x.ForcedOverTime),
                                                         DayCountInDailyTimeSheet = 0
                                                     }).ToList();

                    query_MonthTimeSheetRollCall.AddRange(query_MonthTimeSheetDraft);
                    // اصلاح مدت زمان اضافه کار
                    EditRollCallOverTimeByOverTimePriority(employeeTimeSheetExcessOverTime, query_MonthTimeSheetRollCall);

                    //MonthTimeSheetWorkTime
                    var query_MonthTimeSheetWorkTime = (from item in attendAbcenseItem.Where(x => x.InvalidRecord == false)
                                                        join includedRollCall in _kscHrUnitOfWork.IncludedRollCallRepository.GetAllAsNoTrackingByIncludedDefinition()
                                                        on item.RollCallDefinitionId equals includedRollCall.RollCallDefinitionId
                                                        where includedRollCall.IncludedDefinition.IsPerWorkTime == true
                                                        group item by new
                                                        {
                                                            WorkTimeId = item.WorkTimeId,
                                                            EmployeeId = item.EmployeeId
                                                        } into newgroup
                                                        select new EmployeeItemGroupModel()
                                                        {
                                                            WorkTimeId = newgroup.Key.WorkTimeId,
                                                            EmployeeId = newgroup.Key.EmployeeId,
                                                            WorkTimeDays = newgroup.Select(x => x.WorkCalendarId).Distinct().Count(),
                                                            ShiftBenefitInMinute = newgroup.Sum(x => x.TimeDurationInMinute.Value)
                                                        }).ToList().OrderBy(x => x.EmployeeId);

                    //MonthTimeSheetIncluded 
                    //dbo.IncludedDefinition.IsSumDuration' is invalid in the select list because it is not contained in either an aggregate function or the GROUP BY clause.
                    var query_MonthTimeSheetIncluded = (from item in attendAbcenseItem.Where(x => x.InvalidRecord == false)
                                                        join includedRollCall in _kscHrUnitOfWork.IncludedRollCallRepository.GetAllAsNoTrackingByIncludedDefinition()
                                                       on item.RollCallDefinitionId equals includedRollCall.RollCallDefinitionId
                                                        where includedRollCall.IncludedDefinition.IsMonthlyTimeSheet == true
                                                        && includedRollCall.IncludedDefinition.IsPerWorkTime == false
                                                        group item by new
                                                        {
                                                            EmployeeId = item.EmployeeId,
                                                            IncludedDefinitionId = includedRollCall.IncludedDefinitionId,
                                                            IsSumDuration = includedRollCall.IncludedDefinition.IsSumDuration
                                                            //DayOrHour = includedRollCall.IncludedDefinition.IsSumDuration == true ? "H" : "D",
                                                        }
                                                     into newgroup
                                                        select new EmployeeItemGroupModel()
                                                        {
                                                            EmployeeId = newgroup.Key.EmployeeId,
                                                            IncludedDefinitionId = newgroup.Key.IncludedDefinitionId,
                                                            DurationInDay = newgroup.Key.IsSumDuration ? null : newgroup.Select(x => x.WorkCalendarId).Distinct().Count(),//روزانه
                                                            DurationInHour = newgroup.Key.IsSumDuration ? newgroup.Sum(x => x.TimeDurationInMinute.Value) : null,//ساعتی

                                                        }).ToList().OrderBy(x => x.EmployeeId);
                    //var tt= query_MonthTimeSheetIncluded.Where(x => x.EmployeeId == 3962);//test
                    // var tt = query_MonthTimeSheetIncluded.Where(x => x.EmployeeId == 4500);
                    List<int?> rollcalTypes = new List<int?> { 8, 9, 13 };
                    var inValidExtraWorks = attendAbcenseItem.Where(x => x.InvalidRecord == true && rollcalTypes.Contains(x.RollCallDefinition.RollCallCategoryId)).
                        GroupBy(x => x.EmployeeId).Select(x => new
                        {
                            EmployeeId = x.Key,
                            SumDuration = x.Sum(c => c.TimeDurationInMinute.Value)
                        })
                        .ToList();

                    List<MonthTimeSheet> MonthTimeSheetDatas = new List<MonthTimeSheet>();
                    var today = DateTime.Now;
                    var CurrentUser = model.CurrentUser;

                    //بدست اوردن مرخصی های استفاده شده کاربران
                    var VacationCurrentMonthUsedRolCallDefinition = _kscHrUnitOfWork.IncludedRollCallRepository.GetRollCallDefinitionByIncludedDefinitionCode(EnumIncludedDefinition.UseedCurrentMonthVacation.Id.ToString());
                    var UsedVacationCurrentMonths = attendAbcenseItem.Where(x => x.InvalidRecord == false
                    && VacationCurrentMonthUsedRolCallDefinition.Contains(x.RollCallDefinitionId)).
                        GroupBy(x => x.EmployeeId).Select(x => new
                        {
                            EmployeeId = x.Key,
                            SumDuration = x.Sum(c => c.TimeDurationInMinute.Value)
                        }).ToList();

                    //بدست اوردن تعداد روزهای بیمه پذیر
                    var InsuranceDaysRollCallDefinition = _kscHrUnitOfWork.IncludedRollCallRepository.GetRollCallDefinitionByIncludedDefinitionCode(EnumIncludedDefinition.CurrentMonthMerit.Id.ToString());
                    var CurrentMonthMeritRollCalls = attendAbcenseItem.Where(x => x.InvalidRecord == false
                    && InsuranceDaysRollCallDefinition.Contains(x.RollCallDefinitionId)).
                        GroupBy(x => new { x.EmployeeId }).Select(x => new
                        {
                            EmployeeId = x.Key.EmployeeId,
                            CurrentMonthMeritCount = x.GroupBy(a => a.WorkCalendar).Select(a => new { a.Key }).Count(),
                        }).ToList();


                    foreach (var item in Emp)
                    {
                        var item_MonthTimeSheetIncluded = query_MonthTimeSheetIncluded.Where(x => x.EmployeeId == item.EmployeeId);
                        List<MonthTimeSheetIncluded> DataMonthTimeSheetIncluded = new List<MonthTimeSheetIncluded>();
                        foreach (var item1 in item_MonthTimeSheetIncluded)
                        {
                            DataMonthTimeSheetIncluded.Add(new MonthTimeSheetIncluded
                            {
                                IncludedDefinitionId = item1.IncludedDefinitionId,
                                DurationInHour = item1.DurationInHour,
                                DurationInDay = item1.DurationInDay,
                                IsActive = true,
                                InsertDate = today,
                                InsertUser = CurrentUser,
                            });
                        }


                        var item_MonthTimeSheetWorkTime = query_MonthTimeSheetWorkTime.Where(x => x.EmployeeId == item.EmployeeId);
                        List<MonthTimeSheetWorkTime> DataMonthTimeSheetWorkTime = new List<MonthTimeSheetWorkTime>();
                        foreach (var item2 in item_MonthTimeSheetWorkTime)
                        {
                            DataMonthTimeSheetWorkTime.Add(new MonthTimeSheetWorkTime
                            {
                                WorkTimeId = item2.WorkTimeId,
                                DayCount = item2.WorkTimeDays,
                                ShiftBenefitInMinute = item2.ShiftBenefitInMinute,
                                IsActive = true,
                                InsertDate = today,
                                InsertUser = CurrentUser,
                            });
                        }


                        var item_MonthTimeSheetRollCall = query_MonthTimeSheetRollCall.Where(x => x.EmployeeId == item.EmployeeId);
                        List<MonthTimeSheetRollCall> DataMonthTimeSheetRollCall = new List<MonthTimeSheetRollCall>();
                        foreach (var item3 in item_MonthTimeSheetRollCall)
                        {
                            var temp = new MonthTimeSheetRollCall();
                            temp.RollCallDefinitionId = item3.RollCallDefinitionId;
                            temp.DurationInMinut = int.Parse(item3.TotalDuration.ToString());
                            temp.DayCountInDailyTimeSheet = item3.DayCountInDailyTimeSheet;
                            temp.IsActive = true;
                            temp.InsertDate = today;
                            temp.InsertUser = CurrentUser;

                            if (temp.DurationInMinut != 0)
                                DataMonthTimeSheetRollCall.Add(temp);

                            //DataMonthTimeSheetRollCall.Add(
                            //    new MonthTimeSheetRollCall
                            //    {
                            //        RollCallDefinitionId = item3.RollCallDefinitionId,
                            //        DurationInMinut = int.Parse(item3.TotalDuration.ToString()),
                            //        DayCountInDailyTimeSheet = item3.DayCountInDailyTimeSheet,
                            //        IsActive = true,
                            //        InsertDate = today,
                            //        InsertUser = CurrentUser,
                            //    });
                        }
                        var excessOverTime = employeeTimeSheetExcessOverTime.FirstOrDefault(x => x.EmployeeId == item.EmployeeId);
                        var averageBalanceOverTime = employeeTimeSheetAverageBalanceOverTime.FirstOrDefault(p => p.EmployeeId == item.EmployeeId);

                        MonthTimeSheet monthTimeSheet = new MonthTimeSheet()
                        {
                            YearMonth = YearMonth,
                            EmployeeId = item.EmployeeId,
                            PaymentStatusId = item.PaymentStatusId.Value,
                            WorkGroupId = item.WorkGroupId.Value,
                            MonthTimeSheetIncludeds = DataMonthTimeSheetIncluded,
                            MonthTimeSheetRollCalls = DataMonthTimeSheetRollCall,
                            MonthTimeSheetWorkTimes = DataMonthTimeSheetWorkTime,
                            SumInvalidOverTimInDailyTimeSheet = inValidExtraWorks.Any(x => x.EmployeeId == item.EmployeeId) ?
                           inValidExtraWorks.First(x => x.EmployeeId == item.EmployeeId).SumDuration.ConvertMinuteToDuration() : "000:00",
                            ExcessOverTime = excessOverTime != null ? excessOverTime.ExcessOverTimeDuration : "000:00",

                            UsedVacationCurrentMonth = UsedVacationCurrentMonths.Any(x => x.EmployeeId == item.EmployeeId) ?
                           UsedVacationCurrentMonths.First(x => x.EmployeeId == item.EmployeeId).SumDuration : 0,


                            CurrentMonthMerit = CurrentMonthMeritRollCalls.Any(x => x.EmployeeId == item.EmployeeId) ?
                           CurrentMonthMeritRollCalls.First(x => x.EmployeeId == item.EmployeeId).CurrentMonthMeritCount : 0,
                            InsertDate = today,
                            InsertUser = CurrentUser,
                            AverageBalanceOverTime = averageBalanceOverTime != null ? averageBalanceOverTime.AverageBalanceOverTimeDuration : "000:00",
                        };

                        MonthTimeSheetDatas.Add(monthTimeSheet);
                    }
                    await _kscHrUnitOfWork.MonthTimeSheetRepository.AddRangeAsync(MonthTimeSheetDatas);
                    //

                    var workCalendar = _kscHrUnitOfWork.WorkCalendarRepository.WhereQueryable(x => x.YearMonthV1 == YearMonth);
                    foreach (var item in workCalendar)
                    {
                        item.SystemSequenceStatusId = EnumSystemSequenceStatusDailyTimeSheet.TimeSheetIsCreated.Id;
                    }
                    //
                    _kscHrUnitOfWork.MonthTimeSheetLogRepository.Add(new MonthTimeSheetLog()
                    {
                        InsertDate = DateTime.Now,
                        YearMonth = YearMonth,
                        InsertUser = model.CurrentUser,
                        Result = "محاسبه کارکرد ماهیانه با موفقیت انجام شد",
                        MonthTimeShitStepperId = model.Step,
                        ResultCount = MonthTimeSheetDatas.Count(),

                    });
                    await _kscHrUnitOfWork.SaveAsync();
                    #endregion

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

        public async Task<KscResult> MonthTimeSheetCalculateStep(UpdateStatusByYearMonthProcedureModel model)
        {
            var result = new KscResult();
            _dbTransaction.BeginTransaction();
            try
            {
                var YearMonth = int.Parse(model.Yearmonth);
                //var workCalendar = _kscHrUnitOfWork.WorkCalendarRepository.GetByMiladiDate(model.DateTimeSheet.Value);
                var systemStatus = await _kscHrUnitOfWork.WorkCalendarRepository.GetMonthTimeSheetStatus(YearMonth);
                if (systemStatus == true)//کاربران اداری سیستم بسته شده باشد
                {
                    if (_kscHrUnitOfWork.MonthTimeSheetRepository.Any(x => x.YearMonth == YearMonth && x.IsCreatedManual == false))
                    {
                        throw new HRBusinessException(Validations.RepetitiveId, "تایم شیت ماه انتخابی ایجاد شده است");
                    }

                    if (_kscHrUnitOfWork.Stepper_ProcedureStatusRepository.CheckStepsDoneByYearMonth(YearMonth, EnumProcedureStep.MonthlyTimeSheet.Id, EnumProcedureStep.MonthlyTimeSheetCalculate.Id) == false)
                    {
                        throw new HRBusinessException(Validations.RepetitiveId, "لطفا مراحل کارکرد ماهیانه را کامل انجام دهید");
                    }
                    //
                    #region محاسبه
                    var employeeTimeSheetByYearMonth = _kscHrUnitOfWork.EmployeeTimeSheetRepository.GetEmployeeTimeSheet(YearMonth).ToList();
                    // افرادی که اضافه کار مازاد سقف دارند
                    var employeeTimeSheetExcessOverTime = employeeTimeSheetByYearMonth.Where(x => x.ExcessOverTime > 0).ToList();
                    //

                    // افرادی که اضافه کار تعدیل میانگین دارند
                    var employeeTimeSheetAverageBalanceOverTime = employeeTimeSheetByYearMonth.Where(x => x.AverageBalanceOverTime != null).ToList();
                    // _kscHrUnitOfWork.EmployeeTimeSheetRepository.GetEmployeeTimeSheetExcessOverTime(YearMonth).Where(x=>x.AverageBalanceOverTime != null).ToList();
                    //


                    //// افرادی که اضافه کار مازاد کلاس آموزش طب دارند
                    //var employeeTimeSheetIndustrialTrainingOverTime = employeeTimeSheetByYearMonth.Where(x => x.TrainingOverTime != null).ToList();
                    ////
                    //// افرادی که اضافه کار جبرانی خدمت دارند
                    //var employeeTimeSheetCompensatoryOverTime = employeeTimeSheetByYearMonth.Where(x => x.CompensatoryOverTime != null).ToList();
                    ////

                    var attendAbcenseItem = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetMonthTimeSheetCalculateAsNoTracking(YearMonth);
                    var MIS_Persons = attendAbcenseItem.Select(x => x.EmployeeId).Distinct().ToList();
                    //var MIS_Persons = _kscHrUnitOfWork.ViewMisEmployeeRepository.WhereQueryable(x => x.HrMonthTimeSheet == 1)
                    //    .Select(x => x.EmployeeId).ToList();

                    var CreatedManualPersons = _kscHrUnitOfWork.MonthTimeSheetRepository.WhereQueryable(x => x.YearMonth == YearMonth && x.IsCreatedManual == true)
                        .Select(x => x.EmployeeId).ToList();

                    var Persons = MIS_Persons.Except(CreatedManualPersons).ToList(); //یافتن افرادی که بایستی تایم شیت اتوماتیک برایشان ساخته شود

                    var Emp = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeIncludedTeamwork(Persons)
                        //.Where(x=>x.Id == 4500)
                        .Where(x => x.WorkGroupId != null)
                        .Select(x => new
                        {
                            EmployeeId = x.Id,
                            PaymentStatusId = x.PaymentStatusId,
                            WorkGroupId = x.WorkGroupId
                        });

                    //MonthTimeSheetRollCall
                    var query_MonthTimeSheetRollCall = (from item in attendAbcenseItem.Where(x => x.InvalidRecord == false)
                                                        group item by new
                                                        {
                                                            RollCallDefinitionId = item.RollCallDefinitionId,
                                                            EmployeeId = item.EmployeeId
                                                        } into newgroup
                                                        select new EmployeeItemGroupModel()
                                                        {
                                                            RollCallDefinitionId = newgroup.Key.RollCallDefinitionId,
                                                            EmployeeId = newgroup.Key.EmployeeId,
                                                            TotalDuration = newgroup.Sum(x => (x.TimeDurationInMinute ?? 0) + (x.IncreasedTimeDuration ?? 0)),
                                                            DayCountInDailyTimeSheet = newgroup.Select(x => x.WorkCalendarId).Distinct().Count(),
                                                            IncreasedTimeDuration = newgroup.Sum(x => x.IncreasedTimeDuration ?? 0)
                                                        }).ToList();
                    //MonthTimeSheetDraft
                    var query_MonthTimeSheetDraft = (from item in _kscHrUnitOfWork.MonthTimeSheetDraftRepository.GetMonthTimeSheetDraftByYearMonth(YearMonth)
                                                     group item by new
                                                     {
                                                         EmployeeId = item.EmployeeId,
                                                         YearMonth = item.YearMonth
                                                     } into newgroup
                                                     select new EmployeeItemGroupModel()
                                                     {
                                                         RollCallDefinitionId = 7,
                                                         EmployeeId = newgroup.Key.EmployeeId,
                                                         TotalDuration = newgroup.Sum(x => x.ForcedOverTime),
                                                         DayCountInDailyTimeSheet = 0
                                                     }).ToList();

                    query_MonthTimeSheetRollCall.AddRange(query_MonthTimeSheetDraft);
                    // افزودن اضافه  کار طب صنعتی
                    var query_MonthTimeSheetIndustrialTraining = employeeTimeSheetByYearMonth.Where(x => x.TrainingOverTime != null)
                              .Select(x =>
                              new EmployeeItemGroupModel()
                              {
                                  RollCallDefinitionId = EnumRollCallDefinication.IndustrialTrainingOverTime.Id,
                                  EmployeeId = x.EmployeeId,
                                  TotalDuration = x.TrainingOverTime.Value,
                                  DayCountInDailyTimeSheet = 0
                              }).ToList();
                    query_MonthTimeSheetRollCall.AddRange(query_MonthTimeSheetIndustrialTraining);
                    // افزودن اضافه کار جبرانی خدمت 
                    var query_MonthTimeSheetCompensatoryOverTime = employeeTimeSheetByYearMonth.Where(x => x.CompensatoryOverTime != null).Select(x =>
                        new EmployeeItemGroupModel()
                        {
                            RollCallDefinitionId = EnumRollCallDefinication.CompensatoryOverTime.Id,
                            EmployeeId = x.EmployeeId,
                            TotalDuration = x.CompensatoryOverTime.Value,
                            DayCountInDailyTimeSheet = 0
                        }).ToList();
                    query_MonthTimeSheetRollCall.AddRange(query_MonthTimeSheetCompensatoryOverTime);
                    //
                    // اصلاح مدت زمان اضافه کار
                    EditRollCallOverTimeByOverTimePriority(employeeTimeSheetExcessOverTime, query_MonthTimeSheetRollCall);

                    //MonthTimeSheetWorkTime
                    var query_MonthTimeSheetWorkTime = (from item in attendAbcenseItem.Where(x => x.InvalidRecord == false)
                                                        join includedRollCall in _kscHrUnitOfWork.IncludedRollCallRepository.GetAllAsNoTrackingByIncludedDefinition()
                                                        on item.RollCallDefinitionId equals includedRollCall.RollCallDefinitionId
                                                        where includedRollCall.IncludedDefinition.IsPerWorkTime == true
                                                        group item by new
                                                        {
                                                            WorkTimeId = item.WorkTimeId,
                                                            EmployeeId = item.EmployeeId
                                                        } into newgroup
                                                        select new EmployeeItemGroupModel()
                                                        {
                                                            WorkTimeId = newgroup.Key.WorkTimeId,
                                                            EmployeeId = newgroup.Key.EmployeeId,
                                                            WorkTimeDays = newgroup.Select(x => x.WorkCalendarId).Distinct().Count(),
                                                            ShiftBenefitInMinute = newgroup.Sum(x => x.TimeDurationInMinute.Value)
                                                        }).OrderBy(x => x.EmployeeId).ToList();

                    //MonthTimeSheetIncluded 
                    //dbo.IncludedDefinition.IsSumDuration' is invalid in the select list because it is not contained in either an aggregate function or the GROUP BY clause.
                    var query_MonthTimeSheetIncluded = (from item in attendAbcenseItem.Where(x => x.InvalidRecord == false)
                                                        join includedRollCall in _kscHrUnitOfWork.IncludedRollCallRepository.GetAllAsNoTrackingByIncludedDefinition()
                                                       on item.RollCallDefinitionId equals includedRollCall.RollCallDefinitionId
                                                        where includedRollCall.IncludedDefinition.IsMonthlyTimeSheet == true
                                                        && includedRollCall.IncludedDefinition.IsPerWorkTime == false
                                                        group item by new
                                                        {
                                                            EmployeeId = item.EmployeeId,
                                                            IncludedDefinitionId = includedRollCall.IncludedDefinitionId,
                                                            IsSumDuration = includedRollCall.IncludedDefinition.IsSumDuration
                                                            //DayOrHour = includedRollCall.IncludedDefinition.IsSumDuration == true ? "H" : "D",
                                                        }
                                                     into newgroup
                                                        select new EmployeeItemGroupModel()
                                                        {
                                                            EmployeeId = newgroup.Key.EmployeeId,
                                                            IncludedDefinitionId = newgroup.Key.IncludedDefinitionId,
                                                            DurationInDay = newgroup.Key.IsSumDuration ? null : newgroup.Select(x => x.WorkCalendarId).Distinct().Count(),//روزانه
                                                            DurationInHour = newgroup.Key.IsSumDuration ? newgroup.Sum(x => x.TimeDurationInMinute.Value) : null,//ساعتی

                                                        }).OrderBy(x => x.EmployeeId).ToList();
                    //var tt= query_MonthTimeSheetIncluded.Where(x => x.EmployeeId == 3962);//test
                    // var tt = query_MonthTimeSheetIncluded.Where(x => x.EmployeeId == 4500);
                    List<int?> rollcalTypes = new List<int?> { 8, 9, 13 };
                    // var test = attendAbcenseItem.Where(x => x.RollCallDefinitionId == null).ToList();
                    var inValidExtraWorks = attendAbcenseItem.Where(x => x.InvalidRecord == true && rollcalTypes.Contains(x.RollCallDefinition.RollCallCategoryId)).
                        GroupBy(x => x.EmployeeId).Select(x => new
                        {
                            EmployeeId = x.Key,
                            SumDuration = x.Sum(c => c.TimeDurationInMinute.Value)
                        })
                        .ToList();

                    ////بدست اوردن مرخصی های استفاده شده کاربران
                    //var VacationCurrentMonthUsedRolCallDefinition = _kscHrUnitOfWork.IncludedRollCallRepository.GetRollCallDefinitionByIncludedDefinitionCode(EnumIncludedDefinition.UseedCurrentMonthVacation.Id.ToString());
                    //var UsedVacationCurrentMonths = attendAbcenseItem.Where(x => x.InvalidRecord == false
                    //&& VacationCurrentMonthUsedRolCallDefinition.Contains(x.RollCallDefinitionId)).
                    //    GroupBy(x => x.EmployeeId).Select(x => new
                    //    {
                    //        EmployeeId = x.Key,
                    //        SumDuration = x.Sum(c => c.TimeDurationInMinute.Value)
                    //    }).ToList();

                    ////بدست اوردن تعداد روزهای مشمول مرخصی استحقاقی
                    //var InsuranceDaysRollCallDefinition = _kscHrUnitOfWork.IncludedRollCallRepository.GetRollCallDefinitionByIncludedDefinitionCode(EnumIncludedDefinition.CurrentMonthMerit.Id.ToString());
                    //var CurrentMonthMeritRollCalls = attendAbcenseItem.Where(x => x.InvalidRecord == false
                    //&& InsuranceDaysRollCallDefinition.Contains(x.RollCallDefinitionId)).
                    //    GroupBy(x => new { x.EmployeeId }).Select(x => new
                    //    {
                    //        EmployeeId = x.Key.EmployeeId,
                    //        CurrentMonthMeritCount = x.GroupBy(a => a.WorkCalendar).Select(a => new { a.Key }).Count(),
                    //    }).ToList();




                    //
                    var rollCallWorkTimeMonthSetting = _kscHrUnitOfWork.RollCallWorkTimeMonthSettingRepository.GetActiveData().ToList();
                    //

                    List<MonthTimeSheet> MonthTimeSheetDatas = new List<MonthTimeSheet>();
                    var today = DateTime.Now;
                    var CurrentUser = model.CurrentUser;
                    var dataResult = Emp.ToList();
                    foreach (var item in dataResult)
                    {
                        var item_MonthTimeSheetIncluded = query_MonthTimeSheetIncluded.Where(x => x.EmployeeId == item.EmployeeId);
                        List<MonthTimeSheetIncluded> DataMonthTimeSheetIncluded = new List<MonthTimeSheetIncluded>();
                        foreach (var item1 in item_MonthTimeSheetIncluded)
                        {
                            DataMonthTimeSheetIncluded.Add(new MonthTimeSheetIncluded
                            {
                                IncludedDefinitionId = item1.IncludedDefinitionId,
                                DurationInHour = item1.DurationInHour,
                                DurationInDay = item1.DurationInDay,
                                IsActive = true,
                                InsertDate = today,
                                InsertUser = CurrentUser,
                            });
                        }


                        var item_MonthTimeSheetWorkTime = query_MonthTimeSheetWorkTime.Where(x => x.EmployeeId == item.EmployeeId);
                        List<MonthTimeSheetWorkTime> DataMonthTimeSheetWorkTime = new List<MonthTimeSheetWorkTime>();
                        foreach (var item2 in item_MonthTimeSheetWorkTime)
                        {
                            DataMonthTimeSheetWorkTime.Add(new MonthTimeSheetWorkTime
                            {
                                WorkTimeId = item2.WorkTimeId,
                                DayCount = item2.WorkTimeDays,
                                ShiftBenefitInMinute = item2.ShiftBenefitInMinute,
                                IsActive = true,
                                InsertDate = today,
                                InsertUser = CurrentUser,
                            });
                        }


                        var item_MonthTimeSheetRollCall = query_MonthTimeSheetRollCall.Where(x => x.EmployeeId == item.EmployeeId);
                        List<MonthTimeSheetRollCall> DataMonthTimeSheetRollCall = new List<MonthTimeSheetRollCall>();
                        foreach (var item3 in item_MonthTimeSheetRollCall)
                        {
                            var temp = new MonthTimeSheetRollCall();
                            temp.RollCallDefinitionId = item3.RollCallDefinitionId;
                            temp.DurationInMinut = int.Parse(item3.TotalDuration.ToString());
                            temp.DayCountInDailyTimeSheet = item3.DayCountInDailyTimeSheet;
                            temp.IsActive = true;
                            temp.InsertDate = today;
                            temp.InsertUser = CurrentUser;
                            temp.IncreasedTimeDuration = item3.IncreasedTimeDuration;
                            if (temp.DurationInMinut != 0)
                                DataMonthTimeSheetRollCall.Add(temp);

                            //DataMonthTimeSheetRollCall.Add(
                            //    new MonthTimeSheetRollCall
                            //    {
                            //        RollCallDefinitionId = item3.RollCallDefinitionId,
                            //        DurationInMinut = int.Parse(item3.TotalDuration.ToString()),
                            //        DayCountInDailyTimeSheet = item3.DayCountInDailyTimeSheet,
                            //        IsActive = true,
                            //        InsertDate = today,
                            //        InsertUser = CurrentUser,
                            //    });
                        }
                        //افزودن حق نوبت کار
                        if (rollCallWorkTimeMonthSetting.Count() != 0)
                        {
                            var workShiftData = GetShiftAddedRollCall(rollCallWorkTimeMonthSetting, DataMonthTimeSheetWorkTime, today, CurrentUser);
                            if (workShiftData.Count() != 0)
                            {
                                DataMonthTimeSheetRollCall.AddRange(workShiftData);
                            }
                        }
                        //
                        var excessOverTime = employeeTimeSheetExcessOverTime.FirstOrDefault(x => x.EmployeeId == item.EmployeeId);
                        var averageBalanceOverTime = employeeTimeSheetAverageBalanceOverTime.FirstOrDefault(p => p.EmployeeId == item.EmployeeId);
                        MonthTimeSheet monthTimeSheet = new MonthTimeSheet()
                        {
                            YearMonth = YearMonth,
                            EmployeeId = item.EmployeeId,
                            PaymentStatusId = item.PaymentStatusId.Value,
                            WorkGroupId = item.WorkGroupId.Value,
                            MonthTimeSheetIncludeds = DataMonthTimeSheetIncluded,
                            MonthTimeSheetRollCalls = DataMonthTimeSheetRollCall,
                            MonthTimeSheetWorkTimes = DataMonthTimeSheetWorkTime,
                            SumInvalidOverTimInDailyTimeSheet = inValidExtraWorks.Any(x => x.EmployeeId == item.EmployeeId) ?
                            inValidExtraWorks.First(x => x.EmployeeId == item.EmployeeId).SumDuration.ConvertMinuteToDuration() : "000:00",
                            ExcessOverTime = excessOverTime != null ? excessOverTime.ExcessOverTimeDuration : "000:00",

                            //UsedVacationCurrentMonth = UsedVacationCurrentMonths.Any(x => x.EmployeeId == item.EmployeeId) ?
                            //UsedVacationCurrentMonths.First(x => x.EmployeeId == item.EmployeeId).SumDuration : 0,


                            //CurrentMonthMerit = CurrentMonthMeritRollCalls.Any(x => x.EmployeeId == item.EmployeeId) ?
                            //CurrentMonthMeritRollCalls.First(x => x.EmployeeId == item.EmployeeId).CurrentMonthMeritCount : 0,


                            InsertDate = today,
                            InsertUser = CurrentUser,
                            AverageBalanceOverTime = averageBalanceOverTime != null ? averageBalanceOverTime.AverageBalanceOverTimeDuration : "000:00",
                        };

                        MonthTimeSheetDatas.Add(monthTimeSheet);
                    }
                    // await _kscHrUnitOfWork.MonthTimeSheetRepository.AddRangeAsync(MonthTimeSheetDatas);
                    //

                    var workCalendar = _kscHrUnitOfWork.WorkCalendarRepository.WhereQueryable(x => x.YearMonthV1 == YearMonth);
                    foreach (var item in workCalendar)
                    {
                        item.SystemSequenceStatusId = EnumSystemSequenceStatusDailyTimeSheet.TimeSheetIsCreated.Id;
                    }
                    //
                    model.Result = "محاسبه کارکرد ماهیانه با موفقیت انجام شد";
                    model.ResultCount = MonthTimeSheetDatas.Count();
                    result = await _procedureService.InsertStepProcedure(model);

                    if (result.Success)
                    {
                        await _kscHrUnitOfWork.SaveAsync(checklog: false);
                        await _kscHrUnitOfWork.MonthTimeSheetRepository.AddBulkAsync(MonthTimeSheetDatas);
                        _dbTransaction.Commit();
                    }
                    #endregion

                }
                else
                {
                    throw new HRBusinessException(Validations.CloseDailyTimeSgeetId, Validations.CloseDailyTimeSgeet);
                }
            }
            catch (Exception ex)
            {
                _dbTransaction.Rollback();
                throw new HRBusinessException(Validations.RepetitiveId, ex.Message);
            }


            return result;
        }

        private List<MonthTimeSheetRollCall> GetShiftAddedRollCall(List<RollCallWorkTimeMonthSetting> rollCallWorkTimeMonthSetting, List<MonthTimeSheetWorkTime> dataMonthTimeSheetWorkTime, DateTime today, string CurrentUser)
        {
            var data = from roll in rollCallWorkTimeMonthSetting
                       join work in dataMonthTimeSheetWorkTime
                       on roll.WorkTimeId equals work.WorkTimeId
                       group new { roll, work } by roll.RollCallDefinitionId
                        into newgroup
                       select new MonthTimeSheetRollCall
                       {
                           RollCallDefinitionId = newgroup.Key,
                           DurationInMinut = newgroup.Sum(x => x.roll.DurationInMinute * x.work.DayCount),
                           DayCountInDailyTimeSheet = newgroup.Sum(x => x.work.DayCount),
                           IsActive = true,
                           InsertDate = today,
                           InsertUser = CurrentUser
                       };
            return data.ToList();
        }

        /// <summary>
        /// اصلاح اضافه کاریها با توجه به مازاد سقف
        /// </summary>
        /// <param name="employeeTimeSheetExcessOverTime"></param>
        /// <param name="query_MonthTimeSheetRollCall"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void EditRollCallOverTimeByOverTimePriority(List<EmployeeTimeSheet> employeeTimeSheetExcessOverTime, List<EmployeeItemGroupModel> query_MonthTimeSheetRollCall)
        {
            var rollCallDefinition = _kscHrUnitOfWork.RollCallDefinitionRepository.GetRollCallDefinicationInCeiling(EnumIncludedDefinition.MaximunOverTime.Id)
                .Select(x => new { RollCallDefinitionId = x.Id, OverTimePriority = x.OverTimePriority }).ToList();
            var data = from query in query_MonthTimeSheetRollCall
                       join rollCall in rollCallDefinition on query.RollCallDefinitionId equals rollCall.RollCallDefinitionId
                       select new
                       {
                           EmployeeId = query.EmployeeId,
                           RollCallDefinitionId = query.RollCallDefinitionId,
                           OverTimePriority = rollCall.OverTimePriority,
                           TotalDuration = query.TotalDuration
                       };
            if (data.Any(x => x.OverTimePriority == null))
            {
                var roll = data.Where(x => x.OverTimePriority == null).Select(x => x.RollCallDefinitionId);
                var overTimePriorityIsNull = string.Join(",", roll.Distinct().ToList());
                throw new HRBusinessException(Validations.RepetitiveId, $"برای کدهای {overTimePriorityIsNull} ترتیب کسر مازاد اضافه کاری مشخص نشده است");
            }
            foreach (var employee in employeeTimeSheetExcessOverTime)
            {

                //var rollCallInOverTimePriority = data.Where(x => x.EmployeeId == employee.EmployeeId && x.OverTimePriority != null).OrderBy(o => o.OverTimePriority);
                var rollCallInOverTimePriority = data.Where(x => x.EmployeeId == employee.EmployeeId).OrderBy(o => o.OverTimePriority);
                int averageBalanceOverTime = employee.AverageBalanceOverTime.HasValue ? employee.AverageBalanceOverTime.Value : 0;
                double remainingDuration = employee.ExcessOverTime + averageBalanceOverTime;
                double excessAverageBalanceOverTime = employee.ExcessOverTime + averageBalanceOverTime;
                double duration = 0;
                foreach (var item in rollCallInOverTimePriority)
                {
                    if (remainingDuration > 0)
                    {
                        if (item.TotalDuration > remainingDuration)
                        {
                            duration = item.TotalDuration - remainingDuration;
                            remainingDuration = 0;
                        }
                        else
                        {
                            if (remainingDuration > item.TotalDuration)
                            {
                                duration = 0;
                            }
                            else
                            {
                                if (remainingDuration - item.TotalDuration < 0)
                                    duration = remainingDuration;
                                else
                                    duration = remainingDuration - item.TotalDuration;
                            }
                            remainingDuration = remainingDuration - item.TotalDuration;
                        }
                        var index = query_MonthTimeSheetRollCall.FindIndex(x => x.EmployeeId == item.EmployeeId && x.RollCallDefinitionId == item.RollCallDefinitionId);
                        // var itemInQuery1 = query_MonthTimeSheetRollCall.First(x => x.EmployeeId == item.EmployeeId && x.RollCallDefinitionId == item.RollCallDefinitionId);
                        var itemInQuery = query_MonthTimeSheetRollCall[index];
                        itemInQuery.TotalDuration = duration;
                        query_MonthTimeSheetRollCall[index] = itemInQuery;
                    }
                }
                if (remainingDuration > 0)
                {
                    throw new Exception("اصلاح اضافه کار با خطا مواجه شد");
                }
            }
        }

        public async Task<KscResult> MonthTimeSheetSendToMIS(SearchMonthTimeSheetModel model)
        {
            var result = new KscResult();

            try
            {
                var YearMonth = int.Parse(model.DateTimeSheet);
                var query_MonthTimeSheet = _kscHrUnitOfWork.MonthTimeSheetRepository
                    .GetMonthTimeSheetAutomaticByYearMonthAsNoTracking(YearMonth)
                    .ToList();
                if (query_MonthTimeSheet.Any() == false)
                {
                    throw new HRBusinessException(Validations.RepetitiveId, "کارکرد ماهانه بایستی ایجاد شود");
                }
                var salaryCode = _kscHrUnitOfWork.RollCallSalaryCodeRepository.WhereQueryable(x => x.IsActive == true).ToList();

                var userWinAndDate = model.CurrentUser;

                #region پر کردن مدل
                var PER_MONT_File = new StringBuilder();
                var MONTHLY_SHIFT = new StringBuilder();
                var MONTHLY_HOURS = new StringBuilder();
                var MONTHLY_ONCALL = new StringBuilder();

                foreach (var item in query_MonthTimeSheet)
                {
                    #region ایجاد فایل PER_MONT_File

                    PER_MONT temp = new PER_MONT();
                    temp.NUM_PRSN_EMPL = item.Employee.EmployeeNumber;

                    var ActiveMonthTimeSheetIncludeds = item.MonthTimeSheetIncludeds.Where(x => x.IsActive == true).ToList();

                    var _NUM_BNS_DAY_MONTP = ActiveMonthTimeSheetIncludeds.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.CalculationInEidDays.Id);
                    temp.NUM_BNS_DAY_MONTP = _NUM_BNS_DAY_MONTP != null ? _NUM_BNS_DAY_MONTP.DurationInDay.ToString() : "0";

                    var _NUM_PNS_DAY_MONTP = ActiveMonthTimeSheetIncludeds.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.InsurancePayment.Id);
                    temp.NUM_PNS_DAY_MONTP = _NUM_PNS_DAY_MONTP != null ? _NUM_PNS_DAY_MONTP.DurationInDay.ToString() : "0";

                    var _NUM_TAX_DAY_MONTP = ActiveMonthTimeSheetIncludeds.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.TaxPayment.Id);
                    temp.NUM_TAX_DAY_MONTP = _NUM_TAX_DAY_MONTP != null ? _NUM_TAX_DAY_MONTP.DurationInDay.ToString() : "0";

                    var _NUM_WRK_DAY_MONTP = ActiveMonthTimeSheetIncludeds.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.Workingdays.Id);
                    temp.NUM_WRK_DAY_MONTP = _NUM_WRK_DAY_MONTP != null ? _NUM_WRK_DAY_MONTP.DurationInDay.ToString() : "0";

                    var _NUM_TRSP_DAY_MONTP = ActiveMonthTimeSheetIncludeds.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.BetweenAway.Id);
                    temp.NUM_TRSP_DAY_MONTP = _NUM_TRSP_DAY_MONTP != null ? _NUM_TRSP_DAY_MONTP.DurationInDay.ToString() : "0";


                    temp.NUM_HOUR_OVT_PAY_EMPL = item.ExcessOverTime.Replace(":", "");

                    var _NUM_DAY_NOPYM_PREM_MONTP = ActiveMonthTimeSheetIncludeds.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.DailyDeductionReward.Id);
                    temp.NUM_DAY_NOPYM_PREM_MONTP = _NUM_DAY_NOPYM_PREM_MONTP != null ? _NUM_DAY_NOPYM_PREM_MONTP.DurationInDay.ToString() : "0";



                    temp.NUM_TIM_HOUR_EOVT_MONTP = item.SumInvalidOverTimInDailyTimeSheet.Replace(":", "");

                    var _TOT_HOUR_NOPYM_MONTP = ActiveMonthTimeSheetIncludeds.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.SalaryAggregateDeduction.Id);
                    temp.TOT_HOUR_NOPYM_MONTP = _TOT_HOUR_NOPYM_MONTP != null ? (_TOT_HOUR_NOPYM_MONTP.DurationInHour ?? 0).ConvertDurationInHour().Replace(":", "") : "0";


                    var _TOT_HOUR_NOPYM_PREM_MONTP = ActiveMonthTimeSheetIncludeds.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.HourlyDeductionReward.Id);
                    temp.TOT_HOUR_NOPYM_PREM_MONTP = _TOT_HOUR_NOPYM_PREM_MONTP != null ? (_TOT_HOUR_NOPYM_PREM_MONTP.DurationInHour ?? 0).ConvertDurationInHour().Replace(":", "") : "0";

                    // temp.NUM_HOUR_OVT_UPMAX_ADMIN = item.AverageBalanceOverTime;

                    ////PER_MONT_File.WriteLine(YearMonth + "|" + temp.NUM_PRSN_EMPL + "|" + temp.NUM_BNS_DAY_MONTP + "|" + temp.NUM_PNS_DAY_MONTP + "|" + temp.NUM_TAX_DAY_MONTP + "|" + temp.NUM_WRK_DAY_MONTP + "|" + temp.NUM_TRSP_DAY_MONTP + "|" + temp.NUM_HOUR_OVT_PAY_EMPL + "|" + temp.NUM_DAY_NOPYM_PREM_MONTP + "|" + temp.NUM_TIM_HOUR_EOVT_MONTP + "|" + temp.TOT_HOUR_NOPYM_MONTP + "|" + temp.TOT_HOUR_NOPYM_PREM_MONTP + "|" + item.AverageBalanceOverTime + "|" + userWinAndDate);
                    //PER_MONT_File.WriteLine(YearMonth + "|" + temp.NUM_PRSN_EMPL + "|" + temp.NUM_BNS_DAY_MONTP + "|" + temp.NUM_PNS_DAY_MONTP + "|" + temp.NUM_TAX_DAY_MONTP + "|" + temp.NUM_WRK_DAY_MONTP + "|" + temp.NUM_TRSP_DAY_MONTP + "|" + temp.NUM_HOUR_OVT_PAY_EMPL + "|" + temp.NUM_DAY_NOPYM_PREM_MONTP + "|" + temp.NUM_TIM_HOUR_EOVT_MONTP + "|" + temp.TOT_HOUR_NOPYM_MONTP + "|" + temp.TOT_HOUR_NOPYM_PREM_MONTP + "|" + userWinAndDate);
                    PER_MONT_File.AppendLine(YearMonth + "|" + temp.NUM_PRSN_EMPL + "|" + temp.NUM_BNS_DAY_MONTP + "|" + temp.NUM_PNS_DAY_MONTP + "|" + temp.NUM_TAX_DAY_MONTP + "|" + temp.NUM_WRK_DAY_MONTP + "|" + temp.NUM_TRSP_DAY_MONTP + "|" + temp.NUM_HOUR_OVT_PAY_EMPL + "|" + temp.NUM_DAY_NOPYM_PREM_MONTP + "|" + temp.NUM_TIM_HOUR_EOVT_MONTP + "|" + temp.TOT_HOUR_NOPYM_MONTP + "|" + temp.TOT_HOUR_NOPYM_PREM_MONTP + "|" + userWinAndDate);
                    #endregion


                    #region ایجاد فایل MONTHLY_SHIFT
                    var ActiveMonthTimeSheetWorkTimes = item.MonthTimeSheetWorkTimes.Where(x => x.IsActive == true).ToList();

                    foreach (var itemWorkTime in ActiveMonthTimeSheetWorkTimes)
                    {
                        temp.MONTHLY_SHIFT.Add(new MONTHLY_SHIFT
                        {
                            COD_TYP_WRK_EMPL = itemWorkTime.WorkTimeId.ToString(),
                            NUM_DAY_SHIFT_MONTP = itemWorkTime.DayCount.ToString()
                        });
                        MONTHLY_SHIFT.AppendLine(YearMonth + "|" + temp.NUM_PRSN_EMPL + "|" + itemWorkTime.WorkTimeId + "|" + itemWorkTime.DayCount);

                    }
                    #endregion

                    #region ایجاد فایل MONTHLY_HOURS
                    var ActiveMonthTimeSheetRollCalls = item.MonthTimeSheetRollCalls.Where(x => x.IsActive == true).ToList();

                    foreach (var itemMonthTimeSheetRollCall in ActiveMonthTimeSheetRollCalls)
                    {
                        var temp_MONTHLY = new MONTHLY_HOURS();
                        temp_MONTHLY.COD_ATT_ATABT = itemMonthTimeSheetRollCall.RollCallDefinitionId.ToString();
                        temp_MONTHLY.NUM_TIM_HOUR_MONTP = itemMonthTimeSheetRollCall.DurationInMinut.ConvertDurationInHour().Replace(":", "");
                        temp_MONTHLY.FK_ACNDF = salaryCode.Where(x =>
                                            x.EmploymentTypeCode == item.Employee.EmploymentTypeId
                                            && x.RollCallDefinitionId == itemMonthTimeSheetRollCall.RollCallDefinitionId
                            ).Select(x => x.SalaryAccountCode.ToString()).FirstOrDefault() ?? "0";

                        temp.MONTHLY_HOURS.Add(temp_MONTHLY);
                        MONTHLY_HOURS.AppendLine(YearMonth + "|" + temp.NUM_PRSN_EMPL + "|" + temp_MONTHLY.COD_ATT_ATABT + "|" + temp_MONTHLY.NUM_TIM_HOUR_MONTP + "|" + temp_MONTHLY.FK_ACNDF);
                    }
                    #endregion

                    #region ایجاد فایل MONTHLY_ONCALL
                    var Farakhan_ActiveMonthTimeSheetRollCalls = ActiveMonthTimeSheetRollCalls.Where(x => x.RollCallDefinitionId == 13 || x.RollCallDefinitionId == 26);
                    if (Farakhan_ActiveMonthTimeSheetRollCalls.Any() == true)
                    {
                        var SUMDayCountInDailyTimeSheet = Farakhan_ActiveMonthTimeSheetRollCalls.Sum(x => x.DayCountInDailyTimeSheet);
                        var COUNTDayCountInDailyTimeSheet = Farakhan_ActiveMonthTimeSheetRollCalls.Count();

                        MONTHLY_ONCALL.AppendLine(YearMonth + "|" + temp.NUM_PRSN_EMPL + "|" + SUMDayCountInDailyTimeSheet);
                    }
                    #endregion

                }
                //نوشتن در فایلها تمام میشود
                var content_PER_MONT_File = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(PER_MONT_File.ToString()));
                var content_MONTHLY_SHIFT = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(MONTHLY_SHIFT.ToString()));
                var content_MONTHLY_HOURS = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(MONTHLY_HOURS.ToString()));
                var content_MONTHLY_ONCALL = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(MONTHLY_ONCALL.ToString()));

                #endregion

                #region Copy File
                var FileforMIS = new List<File>{
                    new File { filename = "PER_MONT_File" , file = content_PER_MONT_File },
                    new File { filename = "MONTHLY_SHIFT" , file = content_MONTHLY_SHIFT },
                    new File { filename = "MONTHLY_HOURS" , file = content_MONTHLY_HOURS },
                    new File { filename = "MONTHLY_ONCALL" , file = content_MONTHLY_ONCALL },

                };

                result = _misUpdateService.SendTextByteFileToMis(Utility.ServerPathSendFileStream, FileforMIS);
                #endregion
            }
            catch (Exception ex)
            {
                result.AddError("", ex.Message);
            }

            return result;
        }
        public async Task<KscResult> MonthTimeSheetSendToMISStep(UpdateStatusByYearMonthProcedureModel model)
        {

            var result = new KscResult();

            try
            {
                var YearMonth = int.Parse(model.Yearmonth);
                var query_MonthTimeSheet = _kscHrUnitOfWork.MonthTimeSheetRepository
                    .GetMonthTimeSheetAutomaticByYearMonthAsNoTracking(YearMonth)
                    .ToList();
                if (query_MonthTimeSheet.Any() == false)
                {
                    throw new HRBusinessException(Validations.RepetitiveId, "کارکرد ماهانه بایستی ایجاد شود");
                }
                var salaryCode = _kscHrUnitOfWork.RollCallSalaryCodeRepository.WhereQueryable(x => x.IsActive == true).ToList();

                var userWinAndDate = model.CurrentUser;

                #region پر کردن مدل
                var PER_MONT_File = new StringBuilder();
                var MONTHLY_SHIFT = new StringBuilder();
                var MONTHLY_HOURS = new StringBuilder();
                var MONTHLY_ONCALL = new StringBuilder();

                foreach (var item in query_MonthTimeSheet)
                {
                    #region ایجاد فایل PER_MONT_File

                    PER_MONT temp = new PER_MONT();
                    temp.NUM_PRSN_EMPL = item.Employee.EmployeeNumber;

                    var ActiveMonthTimeSheetIncludeds = item.MonthTimeSheetIncludeds.Where(x => x.IsActive == true).ToList();

                    var _NUM_BNS_DAY_MONTP = ActiveMonthTimeSheetIncludeds.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.CalculationInEidDays.Id);
                    temp.NUM_BNS_DAY_MONTP = _NUM_BNS_DAY_MONTP != null ? _NUM_BNS_DAY_MONTP.DurationInDay.ToString() : "0";

                    var _NUM_PNS_DAY_MONTP = ActiveMonthTimeSheetIncludeds.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.InsurancePayment.Id);
                    temp.NUM_PNS_DAY_MONTP = _NUM_PNS_DAY_MONTP != null ? _NUM_PNS_DAY_MONTP.DurationInDay.ToString() : "0";

                    var _NUM_TAX_DAY_MONTP = ActiveMonthTimeSheetIncludeds.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.TaxPayment.Id);
                    temp.NUM_TAX_DAY_MONTP = _NUM_TAX_DAY_MONTP != null ? _NUM_TAX_DAY_MONTP.DurationInDay.ToString() : "0";

                    var _NUM_WRK_DAY_MONTP = ActiveMonthTimeSheetIncludeds.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.Workingdays.Id);
                    temp.NUM_WRK_DAY_MONTP = _NUM_WRK_DAY_MONTP != null ? _NUM_WRK_DAY_MONTP.DurationInDay.ToString() : "0";

                    var _NUM_TRSP_DAY_MONTP = ActiveMonthTimeSheetIncludeds.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.BetweenAway.Id);
                    temp.NUM_TRSP_DAY_MONTP = _NUM_TRSP_DAY_MONTP != null ? _NUM_TRSP_DAY_MONTP.DurationInDay.ToString() : "0";


                    temp.NUM_HOUR_OVT_PAY_EMPL = item.ExcessOverTime.Replace(":", "");

                    var _NUM_DAY_NOPYM_PREM_MONTP = ActiveMonthTimeSheetIncludeds.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.DailyDeductionReward.Id);
                    temp.NUM_DAY_NOPYM_PREM_MONTP = _NUM_DAY_NOPYM_PREM_MONTP != null ? _NUM_DAY_NOPYM_PREM_MONTP.DurationInDay.ToString() : "0";



                    temp.NUM_TIM_HOUR_EOVT_MONTP = item.SumInvalidOverTimInDailyTimeSheet.Replace(":", "");

                    var _TOT_HOUR_NOPYM_MONTP = ActiveMonthTimeSheetIncludeds.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.SalaryAggregateDeduction.Id);
                    temp.TOT_HOUR_NOPYM_MONTP = _TOT_HOUR_NOPYM_MONTP != null ? (_TOT_HOUR_NOPYM_MONTP.DurationInHour ?? 0).ConvertDurationInHour().Replace(":", "") : "0";


                    var _TOT_HOUR_NOPYM_PREM_MONTP = ActiveMonthTimeSheetIncludeds.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.HourlyDeductionReward.Id);
                    temp.TOT_HOUR_NOPYM_PREM_MONTP = _TOT_HOUR_NOPYM_PREM_MONTP != null ? (_TOT_HOUR_NOPYM_PREM_MONTP.DurationInHour ?? 0).ConvertDurationInHour().Replace(":", "") : "0";

                    // temp.NUM_HOUR_OVT_UPMAX_ADMIN = item.AverageBalanceOverTime;

                    ////PER_MONT_File.WriteLine(YearMonth + "|" + temp.NUM_PRSN_EMPL + "|" + temp.NUM_BNS_DAY_MONTP + "|" + temp.NUM_PNS_DAY_MONTP + "|" + temp.NUM_TAX_DAY_MONTP + "|" + temp.NUM_WRK_DAY_MONTP + "|" + temp.NUM_TRSP_DAY_MONTP + "|" + temp.NUM_HOUR_OVT_PAY_EMPL + "|" + temp.NUM_DAY_NOPYM_PREM_MONTP + "|" + temp.NUM_TIM_HOUR_EOVT_MONTP + "|" + temp.TOT_HOUR_NOPYM_MONTP + "|" + temp.TOT_HOUR_NOPYM_PREM_MONTP + "|" + item.AverageBalanceOverTime + "|" + userWinAndDate);
                    //PER_MONT_File.WriteLine(YearMonth + "|" + temp.NUM_PRSN_EMPL + "|" + temp.NUM_BNS_DAY_MONTP + "|" + temp.NUM_PNS_DAY_MONTP + "|" + temp.NUM_TAX_DAY_MONTP + "|" + temp.NUM_WRK_DAY_MONTP + "|" + temp.NUM_TRSP_DAY_MONTP + "|" + temp.NUM_HOUR_OVT_PAY_EMPL + "|" + temp.NUM_DAY_NOPYM_PREM_MONTP + "|" + temp.NUM_TIM_HOUR_EOVT_MONTP + "|" + temp.TOT_HOUR_NOPYM_MONTP + "|" + temp.TOT_HOUR_NOPYM_PREM_MONTP + "|" + userWinAndDate);
                    PER_MONT_File.AppendLine(YearMonth + "|" + temp.NUM_PRSN_EMPL + "|" + temp.NUM_BNS_DAY_MONTP + "|" + temp.NUM_PNS_DAY_MONTP + "|" + temp.NUM_TAX_DAY_MONTP + "|" + temp.NUM_WRK_DAY_MONTP + "|" + temp.NUM_TRSP_DAY_MONTP + "|" + temp.NUM_HOUR_OVT_PAY_EMPL + "|" + temp.NUM_DAY_NOPYM_PREM_MONTP + "|" + temp.NUM_TIM_HOUR_EOVT_MONTP + "|" + temp.TOT_HOUR_NOPYM_MONTP + "|" + temp.TOT_HOUR_NOPYM_PREM_MONTP + "|" + userWinAndDate);
                    #endregion


                    #region ایجاد فایل MONTHLY_SHIFT
                    var ActiveMonthTimeSheetWorkTimes = item.MonthTimeSheetWorkTimes.Where(x => x.IsActive == true).ToList();

                    foreach (var itemWorkTime in ActiveMonthTimeSheetWorkTimes)
                    {
                        temp.MONTHLY_SHIFT.Add(new MONTHLY_SHIFT
                        {
                            COD_TYP_WRK_EMPL = itemWorkTime.WorkTimeId.ToString(),
                            NUM_DAY_SHIFT_MONTP = itemWorkTime.DayCount.ToString()
                        });
                        MONTHLY_SHIFT.AppendLine(YearMonth + "|" + temp.NUM_PRSN_EMPL + "|" + itemWorkTime.WorkTimeId + "|" + itemWorkTime.DayCount);

                    }
                    #endregion

                    #region ایجاد فایل MONTHLY_HOURS
                    var ActiveMonthTimeSheetRollCalls = item.MonthTimeSheetRollCalls.Where(x => x.IsActive == true).ToList();

                    foreach (var itemMonthTimeSheetRollCall in ActiveMonthTimeSheetRollCalls)
                    {
                        var temp_MONTHLY = new MONTHLY_HOURS();
                        temp_MONTHLY.COD_ATT_ATABT = itemMonthTimeSheetRollCall.RollCallDefinitionId.ToString();
                        temp_MONTHLY.NUM_TIM_HOUR_MONTP = itemMonthTimeSheetRollCall.DurationInMinut.ConvertDurationInHour().Replace(":", "");
                        temp_MONTHLY.FK_ACNDF = salaryCode.Where(x =>
                                            x.EmploymentTypeCode == item.Employee.EmploymentTypeId
                                            && x.RollCallDefinitionId == itemMonthTimeSheetRollCall.RollCallDefinitionId
                            ).Select(x => x.SalaryAccountCode.ToString()).FirstOrDefault() ?? "0";

                        temp.MONTHLY_HOURS.Add(temp_MONTHLY);
                        MONTHLY_HOURS.AppendLine(YearMonth + "|" + temp.NUM_PRSN_EMPL + "|" + temp_MONTHLY.COD_ATT_ATABT + "|" + temp_MONTHLY.NUM_TIM_HOUR_MONTP + "|" + temp_MONTHLY.FK_ACNDF);
                    }
                    #endregion

                    #region ایجاد فایل MONTHLY_ONCALL
                    var Farakhan_ActiveMonthTimeSheetRollCalls = ActiveMonthTimeSheetRollCalls.Where(x => x.RollCallDefinitionId == 13 || x.RollCallDefinitionId == 26);
                    if (Farakhan_ActiveMonthTimeSheetRollCalls.Any() == true)
                    {
                        var SUMDayCountInDailyTimeSheet = Farakhan_ActiveMonthTimeSheetRollCalls.Sum(x => x.DayCountInDailyTimeSheet);
                        var COUNTDayCountInDailyTimeSheet = Farakhan_ActiveMonthTimeSheetRollCalls.Count();

                        MONTHLY_ONCALL.AppendLine(YearMonth + "|" + temp.NUM_PRSN_EMPL + "|" + SUMDayCountInDailyTimeSheet);
                    }
                    #endregion

                }
                //نوشتن در فایلها تمام میشود
                var content_PER_MONT_File = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(PER_MONT_File.ToString()));
                var content_MONTHLY_SHIFT = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(MONTHLY_SHIFT.ToString()));
                var content_MONTHLY_HOURS = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(MONTHLY_HOURS.ToString()));
                var content_MONTHLY_ONCALL = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(MONTHLY_ONCALL.ToString()));

                #endregion

                #region Copy File
                var FileforMIS = new List<File>{
                    new File { filename = "PER_MONT_File" , file = content_PER_MONT_File },
                    new File { filename = "MONTHLY_SHIFT" , file = content_MONTHLY_SHIFT },
                    new File { filename = "MONTHLY_HOURS" , file = content_MONTHLY_HOURS },
                    new File { filename = "MONTHLY_ONCALL" , file = content_MONTHLY_ONCALL },

                };
                if (model.DomainName != "KSC")
                {
                    //var FileforMISHolding = new List<File>{
                    //new File { filename = "PER_MONT_File" , file = PER_MONT_File.ToString() },
                    //new File { filename = "MONTHLY_SHIFT" , file = MONTHLY_SHIFT.ToString()},
                    //new File { filename = "MONTHLY_HOURS" , file = MONTHLY_HOURS.ToString() },
                    //new File { filename = "MONTHLY_ONCALL" , file = MONTHLY_ONCALL.ToString() }                    };
                    //var serverPath = @"D:\MISTXTHolding\";
                    //foreach (var item in FileforMISHolding)
                    //{
                    //    System.IO.StreamWriter filePath = new System.IO.StreamWriter(serverPath + item.filename + ".TXT"); //d:\\MISTXT\PER_MONT_File.txt
                    //                                                                                                       //PER_MONT_File.WriteLine("YearMonth|NUM_PRSN_EMPL|temp.NUM_BNS_DAY_MONTP|NUM_PNS_DAY_MONTP|NUM_TAX_DAY_MONTP|NUM_WRK_DAY_MONTP|NUM_TRSP_DAY_MONTP|NUM_HOUR_OVT_PAY_EMPL|NUM_DAY_NOPYM_PREM_MONTP|NUM_TIM_HOUR_EOVT_MONTP|TOT_HOUR_NOPYM_MONTP|TOT_HOUR_NOPYM_PREM_MONTP");
                    //    filePath.Write(item.file);
                    //    filePath.Close();
                    //}
                    result = _misUpdateService.SendTextByteFileToMis(Utility.GetServerPathSendFileStream(model.DomainName), FileforMIS, model.DomainName);
                }
                else
                {
                    result = _misUpdateService.SendTextByteFileToMis(Utility.ServerPathSendFileStream, FileforMIS);
                }
                if (result.Success)
                {
                    model.ResultCount = FileforMIS.Count();
                    model.Result = $"انتقال فایل Mis   با موفقیت انجام شد ";
                    result = await _procedureService.InsertStepProcedure(model);
                    //بستن سیستم پرسنلی
                    var nextprocess = model;
                    nextprocess.ProcedureId = EnumProcedureStep.CloseSystemHR.Id;
                    nextprocess.SystemSequenceControlId = Ksc.HR.Share.Model.SystemSequenceControl.EnumSystemSequenceControl.HRSystem.Id;
                    nextprocess.Result = null;
                    result = await _procedureService.ChangeStatusSystemMonth(model);

                    if (result.Success)
                        await _kscHrUnitOfWork.SaveAsync();
                }
                #endregion
            }
            catch (Exception ex)
            {
                result.AddError("", ex.Message);
            }

            return result;
        }

        public async Task<KscResult> MonthTimeSheetSendToMISOld(SearchMonthTimeSheetModel model)
        {
            var result = new KscResult();

            try
            {


                var YearMonth = int.Parse(model.DateTimeSheet);

                var query_MonthTimeSheet = _kscHrUnitOfWork.MonthTimeSheetRepository
                    .GetMonthTimeSheetAutomaticByYearMonthAsNoTracking(YearMonth)
                    .ToList();

                if (query_MonthTimeSheet.Any() == false)
                {
                    throw new HRBusinessException(Validations.RepetitiveId, "کارکرد ماهانه بایستی ایجاد شود");

                    //result.AddError("ذخیره نشده", "کارکرد ماهانه بایستی ایجاد شود");
                    //return result;
                }
                var salaryCode = _kscHrUnitOfWork.RollCallSalaryCodeRepository.WhereQueryable(x => x.IsActive == true).ToList();

                string path = @"\\srv2359\sharefiles\mis\per\";//http://wapi.ksc.ir/MisService/api/IT/SendFileStream
                                                               //Byte[] bytes = File.ReadAllBytes(path);
                                                               //String _file = Convert.ToBase64String(bytes);
                                                               //SendFile sendFileModel = new SendFile { application = "PER", file = _file , enviroment = env };
                                                               /////string path = @"d:\\MISTXT\";

                System.IO.StreamWriter PER_MONT_File = new System.IO.StreamWriter(path + "PER_MONT_File.TXT"); //d:\\MISTXT\PER_MONT_File.txt
                                                                                                               //PER_MONT_File.WriteLine("YearMonth|NUM_PRSN_EMPL|temp.NUM_BNS_DAY_MONTP|NUM_PNS_DAY_MONTP|NUM_TAX_DAY_MONTP|NUM_WRK_DAY_MONTP|NUM_TRSP_DAY_MONTP|NUM_HOUR_OVT_PAY_EMPL|NUM_DAY_NOPYM_PREM_MONTP|NUM_TIM_HOUR_EOVT_MONTP|TOT_HOUR_NOPYM_MONTP|TOT_HOUR_NOPYM_PREM_MONTP");
                System.IO.StreamWriter MONTHLY_SHIFT = new System.IO.StreamWriter(path + "MONTHLY_SHIFT.TXT"); //d:\\MISTXT\MONTHLY_SHIFT.txt
                                                                                                               //MONTHLY_SHIFT.WriteLine("YearMonth|NUM_PRSN_EMPL|WorkTimeId|DayCount");
                System.IO.StreamWriter MONTHLY_HOURS = new System.IO.StreamWriter(path + "MONTHLY_HOURS.TXT"); //d:\\MISTXT\MONTHLY_HOURS.txt
                                                                                                               //MONTHLY_HOURS.WriteLine("YearMonth|NUM_PRSN_EMPL|COD_ATT_ATABT|NUM_TIM_HOUR_MONTP|FK_ACNDF");

                System.IO.StreamWriter MONTHLY_ONCALL = new System.IO.StreamWriter(path + "MONTHLY_ONCALL.TXT"); //d:\\MISTXT\MONTHLY_ONCALL.txt
                                                                                                                 //MONTHLY_ONCALL.WriteLine("YearMonth|NUM_PRSN_EMPL|SUMDayCountInDailyTimeSheet");


                var userWinAndDate = model.CurrentUser;//+ "|" + DateTime.Now.ToShortPersianDateString();

                #region پر کردن مدل
                List<PER_MONT> TempPER_MONT = new List<PER_MONT>();
                foreach (var item in query_MonthTimeSheet)
                {
                    PER_MONT temp = new PER_MONT();
                    temp.NUM_PRSN_EMPL = item.Employee.EmployeeNumber;

                    var ActiveMonthTimeSheetIncludeds = item.MonthTimeSheetIncludeds.Where(x => x.IsActive == true).ToList();

                    var _NUM_BNS_DAY_MONTP = ActiveMonthTimeSheetIncludeds.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.CalculationInEidDays.Id);
                    temp.NUM_BNS_DAY_MONTP = _NUM_BNS_DAY_MONTP != null ? _NUM_BNS_DAY_MONTP.DurationInDay.ToString() : "0";

                    var _NUM_PNS_DAY_MONTP = ActiveMonthTimeSheetIncludeds.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.InsurancePayment.Id);
                    temp.NUM_PNS_DAY_MONTP = _NUM_PNS_DAY_MONTP != null ? _NUM_PNS_DAY_MONTP.DurationInDay.ToString() : "0";

                    var _NUM_TAX_DAY_MONTP = ActiveMonthTimeSheetIncludeds.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.TaxPayment.Id);
                    temp.NUM_TAX_DAY_MONTP = _NUM_TAX_DAY_MONTP != null ? _NUM_TAX_DAY_MONTP.DurationInDay.ToString() : "0";

                    var _NUM_WRK_DAY_MONTP = ActiveMonthTimeSheetIncludeds.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.Workingdays.Id);
                    temp.NUM_WRK_DAY_MONTP = _NUM_WRK_DAY_MONTP != null ? _NUM_WRK_DAY_MONTP.DurationInDay.ToString() : "0";

                    var _NUM_TRSP_DAY_MONTP = ActiveMonthTimeSheetIncludeds.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.BetweenAway.Id);
                    temp.NUM_TRSP_DAY_MONTP = _NUM_TRSP_DAY_MONTP != null ? _NUM_TRSP_DAY_MONTP.DurationInDay.ToString() : "0";


                    temp.NUM_HOUR_OVT_PAY_EMPL = item.ExcessOverTime.Replace(":", "");

                    var _NUM_DAY_NOPYM_PREM_MONTP = ActiveMonthTimeSheetIncludeds.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.DailyDeductionReward.Id);
                    temp.NUM_DAY_NOPYM_PREM_MONTP = _NUM_DAY_NOPYM_PREM_MONTP != null ? _NUM_DAY_NOPYM_PREM_MONTP.DurationInDay.ToString() : "0";

                    //temp.TOT_HOUR_NOPYM_HLDY_MONTP = // حذف

                    temp.NUM_TIM_HOUR_EOVT_MONTP = item.SumInvalidOverTimInDailyTimeSheet.Replace(":", "");

                    var _TOT_HOUR_NOPYM_MONTP = ActiveMonthTimeSheetIncludeds.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.SalaryAggregateDeduction.Id);
                    temp.TOT_HOUR_NOPYM_MONTP = _TOT_HOUR_NOPYM_MONTP != null ? (_TOT_HOUR_NOPYM_MONTP.DurationInHour ?? 0).ConvertDurationInHour().Replace(":", "") : "0";


                    var _TOT_HOUR_NOPYM_PREM_MONTP = ActiveMonthTimeSheetIncludeds.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.HourlyDeductionReward.Id);
                    temp.TOT_HOUR_NOPYM_PREM_MONTP = _TOT_HOUR_NOPYM_PREM_MONTP != null ? (_TOT_HOUR_NOPYM_PREM_MONTP.DurationInHour ?? 0).ConvertDurationInHour().Replace(":", "") : "0";

                    // temp.NUM_HOUR_OVT_UPMAX_ADMIN = item.AverageBalanceOverTime;

                    //PER_MONT_File.WriteLine(YearMonth + "|" + temp.NUM_PRSN_EMPL + "|" + temp.NUM_BNS_DAY_MONTP + "|" + temp.NUM_PNS_DAY_MONTP + "|" + temp.NUM_TAX_DAY_MONTP + "|" + temp.NUM_WRK_DAY_MONTP + "|" + temp.NUM_TRSP_DAY_MONTP + "|" + temp.NUM_HOUR_OVT_PAY_EMPL + "|" + temp.NUM_DAY_NOPYM_PREM_MONTP + "|" + temp.NUM_TIM_HOUR_EOVT_MONTP + "|" + temp.TOT_HOUR_NOPYM_MONTP + "|" + temp.TOT_HOUR_NOPYM_PREM_MONTP + "|" + item.AverageBalanceOverTime + "|" + userWinAndDate);
                    PER_MONT_File.WriteLine(YearMonth + "|" + temp.NUM_PRSN_EMPL + "|" + temp.NUM_BNS_DAY_MONTP + "|" + temp.NUM_PNS_DAY_MONTP + "|" + temp.NUM_TAX_DAY_MONTP + "|" + temp.NUM_WRK_DAY_MONTP + "|" + temp.NUM_TRSP_DAY_MONTP + "|" + temp.NUM_HOUR_OVT_PAY_EMPL + "|" + temp.NUM_DAY_NOPYM_PREM_MONTP + "|" + temp.NUM_TIM_HOUR_EOVT_MONTP + "|" + temp.TOT_HOUR_NOPYM_MONTP + "|" + temp.TOT_HOUR_NOPYM_PREM_MONTP + "|" + userWinAndDate);

                    var ActiveMonthTimeSheetWorkTimes = item.MonthTimeSheetWorkTimes.Where(x => x.IsActive == true).ToList();

                    foreach (var itemWorkTime in ActiveMonthTimeSheetWorkTimes)
                    {
                        temp.MONTHLY_SHIFT.Add(new MONTHLY_SHIFT
                        {
                            COD_TYP_WRK_EMPL = itemWorkTime.WorkTimeId.ToString(),
                            NUM_DAY_SHIFT_MONTP = itemWorkTime.DayCount.ToString()
                        });
                        MONTHLY_SHIFT.WriteLine(YearMonth + "|" + temp.NUM_PRSN_EMPL + "|" + itemWorkTime.WorkTimeId + "|" + itemWorkTime.DayCount);

                    }
                    var ActiveMonthTimeSheetRollCalls = item.MonthTimeSheetRollCalls.Where(x => x.IsActive == true).ToList();

                    foreach (var itemMonthTimeSheetRollCall in ActiveMonthTimeSheetRollCalls)
                    {
                        var temp_MONTHLY = new MONTHLY_HOURS();
                        temp_MONTHLY.COD_ATT_ATABT = itemMonthTimeSheetRollCall.RollCallDefinitionId.ToString();
                        temp_MONTHLY.NUM_TIM_HOUR_MONTP = itemMonthTimeSheetRollCall.DurationInMinut.ConvertDurationInHour().Replace(":", "");
                        temp_MONTHLY.FK_ACNDF = salaryCode.Where(x =>
                                            x.EmploymentTypeCode == item.Employee.EmploymentTypeId
                                            && x.RollCallDefinitionId == itemMonthTimeSheetRollCall.RollCallDefinitionId
                            ).Select(x => x.SalaryAccountCode.ToString()).FirstOrDefault() ?? "0";

                        temp.MONTHLY_HOURS.Add(temp_MONTHLY);
                        MONTHLY_HOURS.WriteLine(YearMonth + "|" + temp.NUM_PRSN_EMPL + "|" + temp_MONTHLY.COD_ATT_ATABT + "|" + temp_MONTHLY.NUM_TIM_HOUR_MONTP + "|" + temp_MONTHLY.FK_ACNDF);
                        //temp.MONTHLY_HOURS.Add(new MONTHLY_HOURS
                        //{
                        //    COD_ATT_ATABT = itemMonthTimeSheetRollCall.RollCallDefinitionId.ToString(),
                        //    NUM_TIM_HOUR_MONTP = itemMonthTimeSheetRollCall.Duration.Replace(":", ""),
                        //    FK_ACNDF = salaryCode.Where(x =>
                        //                    x.EmploymentTypeCode == item.Employee.EmploymentTypeId
                        //                    && x.RollCallDefinitionId == itemMonthTimeSheetRollCall.RollCallDefinitionId
                        //    ).Select(x => x.SalaryAccountCode.ToString()).FirstOrDefault() ?? "0",
                        //});

                    }
                    /////// ایجاد فایل MONTHLY_ONCALL ////////////
                    var Farakhan_ActiveMonthTimeSheetRollCalls = ActiveMonthTimeSheetRollCalls.Where(x => x.RollCallDefinitionId == 13 || x.RollCallDefinitionId == 26);
                    if (Farakhan_ActiveMonthTimeSheetRollCalls.Any() == true)
                    {
                        var SUMDayCountInDailyTimeSheet = Farakhan_ActiveMonthTimeSheetRollCalls.Sum(x => x.DayCountInDailyTimeSheet);
                        var COUNTDayCountInDailyTimeSheet = Farakhan_ActiveMonthTimeSheetRollCalls.Count();
                        //MONTHLY_ONCALL.WriteLine("YearMonth|NUM_PRSN_EMPL|SUMDayCountInDailyTimeSheet");
                        MONTHLY_ONCALL.WriteLine(YearMonth + "|" + temp.NUM_PRSN_EMPL + "|" + SUMDayCountInDailyTimeSheet);
                    }
                    //////////////


                    TempPER_MONT.Add(temp);
                }

                var modelMIS = new PAR_MONT()
                {
                    OPERATION = "I",
                    COUNT_PER = query_MonthTimeSheet.Count().ToString(), // شمارنده (تعداد پرسنل ارسالی)
                    DAT_ATT_MONTP = YearMonth.ToString(), // تاریخ کارکرد (سال و ماه)
                    PER_MONT = TempPER_MONT
                };
                PER_MONT_File.Close();
                MONTHLY_SHIFT.Close();
                MONTHLY_HOURS.Close();
                MONTHLY_ONCALL.Close();


                #endregion

                #region Copy File
                List<string> fileNames = new() { "PER_MONT_File", "MONTHLY_SHIFT", "MONTHLY_HOURS", "MONTHLY_ONCALL" };

                var ServerPath = Utility.ServerPathSendFile; //"https://wapi.ksc.ir/MisService/api/IT/SendFile";
                var uri = $"{ServerPath}";
                foreach (var item in fileNames)
                {
                    using (var client = new HttpClient())
                    {
                        string env;
#if DEBUG
                        env = "M";
#else
                        env = "L";
#endif

                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        SendFile sendFileModel = new SendFile { application = "PER", filename = item + ".txt", enviroment = env };
                        var postTask = client.PostAsJsonAsync<SendFile>(uri, sendFileModel);
                        postTask.Wait();


                        var resultApi = postTask.Result;
                        if (resultApi.IsSuccessStatusCode)
                        {
                            var returnValue = resultApi.Content.ReadAsStringAsync();

                            var modelObj = JsonConvert.DeserializeObject<ReturnData<string>>(returnValue.Result);
                            if (modelObj.IsSuccess == true)
                            {

                            }
                            else
                            {
                                throw new HRBusinessException(Validations.RepetitiveId, "انتقال فایل انجام نشد");
                            }
                        }
                        else
                        {
                            throw new HRBusinessException(Validations.RepetitiveId, "ارتباط با سیستم  برقرار نشد");
                        }
                    }
                }

                #endregion

            }
            catch (Exception ex)
            {

                result.AddError("", ex.Message);
            }

            return result;
        }

        //public async Task<KscResult> MonthTimeSheetSendToMIS1111(SearchMonthTimeSheetModel model)
        //{
        //    var result = new KscResult();
        //    try
        //    {
        //        // var workCalendar = _kscHrUnitOfWork.WorkCalendarRepository.GetByMiladiDate(model.DateTimeSheet.Value);
        //        var YearMonth = int.Parse(model.DateTimeSheet);


        //        var query_MonthTimeSheet = _kscHrUnitOfWork.ViewTimeSheetToMisRepository
        //            .GetVMMonthTimeSheetByYearMonthAsNoTracking(YearMonth)
        //            //.Where(x => x.EmployeeId == 4500)
        //            ;



        //        var EtalaatAfrad = (from item in query_MonthTimeSheet
        //                            group item by new
        //                            {
        //                                EmployeeNumber = item.EmployeeNumber
        //                            } into newgroup
        //                            select new
        //                            {

        //                                EmployeeNumber = newgroup.Key.EmployeeNumber,
        //                                MONTHLY_SHIFT = newgroup.Select(c => new
        //                                {
        //                                    COD_TYP_WRK_EMPL = c.WorkTimeId,
        //                                    NUM_DAY_SHIFT_MONTP = c.DayCount
        //                                }).Distinct().ToList(),
        //                                MONTHLY_HOURS = newgroup.Select(c => new
        //                                {
        //                                    COD_ATT_ATABT = c.RollCallDefinitionId,
        //                                    FK_ACNDF = c.SalaryAccountCode,
        //                                    NUM_TIM_HOUR_MONTP = c.Duration
        //                                }).Distinct().ToList(),
        //                                MonthTimeSheetInclud = newgroup.Select(c => new
        //                                {
        //                                    IncludedDefinitionId = c.IncludedDefinitionId,
        //                                    DurationInHour = c.DurationInHour,
        //                                    DurationInDay = c.DurationInDay,
        //                                    DurationInHourFormatted = c.DurationInHourFormatted,
        //                                }).Distinct().ToList(),

        //                            }).ToList().Select(a => new PER_MONT()
        //                            {
        //                                NUM_PRSN_EMPL = a.EmployeeNumber.ToString(),

        //                                NUM_BNS_DAY_MONTP = a.MonthTimeSheetInclud
        //                                .Where(x => x.IncludedDefinitionId == EnumIncludedDefinition.CalculationInEidDays.Id)
        //                                .Select(x => x.DurationInHour.ToString()).FirstOrDefault()
        //                                ,

        //                                NUM_PNS_DAY_MONTP = a.MonthTimeSheetInclud
        //                                .Where(x => x.IncludedDefinitionId == EnumIncludedDefinition.InsurancePayment.Id)
        //                                .Select(x => x.DurationInHour.ToString()).FirstOrDefault()
        //                                ,


        //                                NUM_TAX_DAY_MONTP = a.MonthTimeSheetInclud
        //                                .Where(x => x.IncludedDefinitionId == EnumIncludedDefinition.TaxPayment.Id)
        //                                .Select(x => x.DurationInHour.ToString()).FirstOrDefault()
        //                                ,

        //                                NUM_WRK_DAY_MONTP = a.MonthTimeSheetInclud
        //                                .Where(x => x.IncludedDefinitionId == EnumIncludedDefinition.Workingdays.Id)
        //                                .Select(x => x.DurationInHour.ToString()).FirstOrDefault()
        //                                ,

        //                                NUM_TRSP_DAY_MONTP = a.MonthTimeSheetInclud
        //                                .Where(x => x.IncludedDefinitionId == EnumIncludedDefinition.BetweenAway.Id)
        //                                .Select(x => x.DurationInHour.ToString()).FirstOrDefault()
        //                                ,

        //                                NUM_HOUR_OVT_PAY_EMPL = a.MonthTimeSheetInclud
        //                                .Where(x => x.IncludedDefinitionId == EnumIncludedDefinition.MaximunOverTime.Id)
        //                                .Select(x => x.DurationInHour.ToString()).FirstOrDefault(),

        //                                MONTHLY_SHIFT = a.MONTHLY_SHIFT.Select(c => new MONTHLY_SHIFT()
        //                                {
        //                                    COD_TYP_WRK_EMPL = c.COD_TYP_WRK_EMPL.ToString(),
        //                                    NUM_DAY_SHIFT_MONTP = c.NUM_DAY_SHIFT_MONTP.ToString(),

        //                                }).ToList(),
        //                                MONTHLY_HOURS = a.MONTHLY_HOURS.Select(c => new MONTHLY_HOURS()
        //                                {
        //                                    COD_ATT_ATABT = c.COD_ATT_ATABT.ToString(),
        //                                    FK_ACNDF = c.FK_ACNDF.ToString(),
        //                                    NUM_TIM_HOUR_MONTP = c.NUM_TIM_HOUR_MONTP.ToString(),
        //                                }).ToList()
        //                            });

        //        List<PER_MONT> TempPER_MONT = new List<PER_MONT>();
        //        //foreach (var item in EtalaatAfrad)
        //        //{
        //        //    PER_MONT temp = new PER_MONT();
        //        //    temp.NUM_PRSN_EMPL = item.EmployeeNumber;


        //        //    TempPER_MONT.Add(temp);
        //        //}

        //        //var modelMIS = new PAR_MONT()
        //        //{
        //        //    COUNT_PER = query_MonthTimeSheet.Select(x => x.EmployeeNumber).Distinct().Count().ToString(), // شمارنده (تعداد پرسنل ارسالی)
        //        //    DAT_ATT_MONTP = workCalendar.YearMonthV1.ToString(), // تاریخ کارکرد (سال و ماه)
        //        //    PER_MONT = TempPER_MONT
        //        //};

        //    }
        //    catch (Exception ec)
        //    {

        //        throw;
        //    }
        //    return result;
        //}
        //        public ReturnData<PAR_MONT> ConnectMISOld(PAR_MONT modelMis)
        //        {
        //            ReturnData<PAR_MONT> MisResult = new ReturnData<PAR_MONT>();

        //            Enviroment env;
        //#if DEBUG
        //            env = Enviroment.Development;
        //#else
        //                        env = Enviroment.Load;
        //#endif


        //            ParamApi<PAR_MONT> miscall = new ParamApi<PAR_MONT>(
        //                 Enviroment: env,
        //                 DomainEnum: KSC.MIS.Service.Domain.KSC,
        //                 LibraryName: LibraryName.PER,
        //                 Subprogram: "S6XML029",
        //                 ParamName: "PAR_MONT",
        //                 Pheader: new PHeader()
        //                 {

        //                 },
        //                     InputModel: modelMis
        //                 );
        //            MisResult = miscall.GetResultDevelop<PAR_MONT>();


        //            return MisResult;

        //        }

        //        public ReturnData<MONTPAY> ConnectMIS(MONTPAY modelMis)
        //        {
        //            ReturnData<MONTPAY> MisResult = new ReturnData<MONTPAY>();

        //            Enviroment env;
        //#if DEBUG
        //            env = Enviroment.Development;
        //#else
        //                env = Enviroment.Load;
        //#endif


        //            ParamApi<MONTPAY> miscall = new ParamApi<MONTPAY>(
        //                 Enviroment: env,
        //                 DomainEnum: KSC.MIS.Service.Domain.KSC,
        //                 LibraryName: LibraryName.PER,
        //                 Subprogram: "S6XML030",
        //                 ParamName: "MONTPAY",
        //                 Pheader: new PHeader()
        //                 {

        //                 },
        //                     InputModel: modelMis
        //                 );
        //            MisResult = miscall.GetResultDevelop<MONTPAY>();


        //            return MisResult;

        //        }


        #region کارکرد ماهانه
        public async Task<EmployeeMonthTimeSheetModel> GetEmployeeTimeSheetModel(int employeeID, int yearMonth)
        {
            var model = new EmployeeMonthTimeSheetModel();
            var employeemodel = await _kscHrUnitOfWork.EmployeeRepository.GetEmployeeByIDMonthTimeSheet(employeeID);
            if (employeemodel == null) return null;
            model.EmployeeNumber = employeemodel.EmployeeNumber;
            model.EmployeeID = employeemodel.Id;
            var monthTimeSheetModel = _kscHrUnitOfWork.MonthTimeSheetRepository.GetMonthTimeSheet(employeemodel.Id, yearMonth);
            model.MonthTimeSheetWorkTimesModel = new List<MonthTimeSheetWorkTimeModel>();
            model.YearMonth = yearMonth.ToString();
            var workCalendarData = _kscHrUnitOfWork.WorkCalendarRepository.GetIncludedWorkCalendarWorkDayType().Where(x => x.YearMonthV1 == yearMonth).Select(x => new { x.Id, x.WorkDayTypeId, WorkDayTypeName = x.WorkDayType.Title, x.WorkDayType.IsHoliday }).ToList();
            var WorkTimeData = _kscHrUnitOfWork.WorkTimeRepository.GetAllActive();

            var systemControl = _kscHrUnitOfWork.SystemControlDateRepository.GetActiveData();

            if (monthTimeSheetModel != null)
            {
                model.MonthTimeSheetID = monthTimeSheetModel.Id;
                model.CanEdit = true;
                model.InsertPersianDate = monthTimeSheetModel.InsertDate.ToShortPersianDateString();
                model.UpdatePersianDate = monthTimeSheetModel.UpdateDate.ToShortPersianDateString();
                model.InsertUser = monthTimeSheetModel.InsertUser;
                model.UpdateUser = monthTimeSheetModel.UpdateUser;
                model.Type = monthTimeSheetModel.IsCreatedManual ? EnumMonthTimeSheet.Manual.Name : EnumMonthTimeSheet.System.Name;
                //8,9,13
                int[] rollcalTypes = new int[] { EnumRollCallCategory.OverTime.Id, EnumRollCallCategory.OverTimeInHoliday.Id, EnumRollCallCategory.DefaultOverTime.Id };
                //var sumDurationInMinut = monthTimeSheetModel.MonthTimeSheetRollCalls.Where(x => x.IsActive && rollcalTypes.Any(r => r == x.RollCallDefinition.RollCallCategoryId)).Sum(x => x.DurationInMinut);
                var sumDurationInMinut = monthTimeSheetModel.MonthTimeSheetRollCalls.Where(x => x.IsActive && x.RollCallDefinition.RollCallConceptId == EnumRollCallConcept.OverTime.Id).Sum(x => x.DurationInMinut);
                model.SumDurationInMinutOverTime = sumDurationInMinut.ConvertMinuteToDuration();
                //  اضافه کار سقف مازاد
                if (monthTimeSheetModel.ExcessOverTime != null)
                {
                    model.ExcessOverTimeHour = int.Parse(monthTimeSheetModel.ExcessOverTime.Substring(0, 3));
                    model.ExcessOverTimeMinute = int.Parse(monthTimeSheetModel.ExcessOverTime.Substring(4, 2));
                }
                // اضافه کار تعدیل میانگین
                if (monthTimeSheetModel.AverageBalanceOverTime != null)
                {
                    model.AverageBalanceOverTimeHour = int.Parse(monthTimeSheetModel.AverageBalanceOverTime.Substring(0, 3));
                    model.AverageBalanceOverTimeMinute = int.Parse(monthTimeSheetModel.AverageBalanceOverTime.Substring(4, 2));
                }
                //اضافه کاری تایید نشده
                if (monthTimeSheetModel.SumInvalidOverTimInDailyTimeSheet != null)
                {
                    model.SumInvalidOverTimInDailyTimeSheetHour = int.Parse(monthTimeSheetModel.SumInvalidOverTimInDailyTimeSheet.Substring(0, 3));
                    model.SumInvalidOverTimInDailyTimeSheetMinute = int.Parse(monthTimeSheetModel.SumInvalidOverTimInDailyTimeSheet.Substring(4, 2));
                }
                model.ContDayForcurrentMonthMerit = monthTimeSheetModel.CurrentMonthMerit;

                // ایام مشمول - قبلی
                var oldIncludedMonthTimesheet = monthTimeSheetModel.MonthTimeSheetIncludeds.Where(x => !x.IsActive).OrderByDescending(x => x.Id);
                var activeMonthTimesheet = monthTimeSheetModel.MonthTimeSheetIncludeds.Where(x => x.IsActive).OrderByDescending(x => x.Id);
                // ایام کارکرد
                model.OldDailyWorkingdays = oldIncludedMonthTimesheet.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.Workingdays.Id)?.DurationInDay;
                model.DailyWorkingdays = activeMonthTimesheet.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.Workingdays.Id)?.DurationInDay;
                //ایام مشمول بیمه
                model.OldDailyInsurancePayment = oldIncludedMonthTimesheet.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.InsurancePayment.Id)?.DurationInDay;
                model.DailyInsurancePayment = activeMonthTimesheet.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.InsurancePayment.Id)?.DurationInDay;
                //ایام مشمول مالیات
                model.OldDailyTaxPayment = oldIncludedMonthTimesheet.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.TaxPayment.Id)?.DurationInDay;
                model.DailyTaxPayment = activeMonthTimesheet.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.TaxPayment.Id)?.DurationInDay;
                //مشمول محاسبه پاداش تولید
                model.OldDailyReward = oldIncludedMonthTimesheet.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.CaculateReward.Id)?.DurationInDay;
                model.DailyReward = activeMonthTimesheet.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.CaculateReward.Id)?.DurationInDay;

                // جمع ساعات عدم پرداخت حقوق
                model.OldDailySalaryAggregateDeduction = (oldIncludedMonthTimesheet.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.SalaryAggregateDeduction.Id)?.DurationInHour ?? 0).ConvertDurationInHour();
                model.DailySalaryAggregateDeduction = (activeMonthTimesheet.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.SalaryAggregateDeduction.Id)?.DurationInHour ?? 0).ConvertDurationInHour();
                // مشمول کسر پاداش ساعتی
                model.OldHourlyDeductionReward = (oldIncludedMonthTimesheet.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.HourlyDeductionReward.Id)?.DurationInHour ?? 0).ConvertDurationInHour();
                model.HourlyDeductionReward = (activeMonthTimesheet.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.HourlyDeductionReward.Id)?.DurationInHour ?? 0).ConvertDurationInHour();
                /// مشمول کسر پاداش روزانه
                model.OldDailyDeductionReward = oldIncludedMonthTimesheet.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.DailyDeductionReward.Id)?.DurationInDay;
                model.DailyDeductionReward = activeMonthTimesheet.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.DailyDeductionReward.Id)?.DurationInDay;
                /// مشمول محاسبه در روزهای عیدی پذیر
                model.OldCalculationInEidDays = oldIncludedMonthTimesheet.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.CalculationInEidDays.Id)?.DurationInDay;
                model.DailyCalculationInEidDays = activeMonthTimesheet.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.CalculationInEidDays.Id)?.DurationInDay;
                ///مشمول حق بین راهی 
                model.OldDailyBetweenAway = oldIncludedMonthTimesheet.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.BetweenAway.Id)?.DurationInDay;
                model.DailyBetweenAway = activeMonthTimesheet.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.BetweenAway.Id)?.DurationInDay;
                // حضور در شیفت
                var monthTimeSheetWorkTimes = monthTimeSheetModel.MonthTimeSheetWorkTimes.OrderByDescending(x => x.Id);
                foreach (var worktimeItem in WorkTimeData)
                {
                    var monthTimeSheetWorkTimemodel = new MonthTimeSheetWorkTimeModel()
                    {
                        WorkTimeId = worktimeItem.Id,
                        Title = worktimeItem.Title,
                        Code = worktimeItem.Code.Trim()
                    };
                    if (monthTimeSheetWorkTimes.Any(x => x.WorkTimeId == worktimeItem.Id))
                    {
                        monthTimeSheetWorkTimemodel.OldDayCount = monthTimeSheetWorkTimes?.FirstOrDefault(x => x.WorkTimeId == monthTimeSheetWorkTimemodel.WorkTimeId && x.IsActive == false)?.DayCount ?? 0;
                        var currentactiveWorkTime = monthTimeSheetWorkTimes?.FirstOrDefault(x => x.WorkTimeId == monthTimeSheetWorkTimemodel.WorkTimeId && x.IsActive);
                        monthTimeSheetWorkTimemodel.DayCount = currentactiveWorkTime?.DayCount;
                    }
                    model.MonthTimeSheetWorkTimesModel.Add(monthTimeSheetWorkTimemodel);
                    ///
                }

            }
            else
            {
                //
                var daycount = workCalendarData.Count();
                var karkardcount = workCalendarData.Count(x => x.WorkDayTypeId == EnumRollCallDefinication.Karkard.Id);
                //model.ContDayForcurrentMonthMerit = daycount;
                model.Type = EnumMonthTimeSheet.Manual.Name;
                // ایام کارکرد
                model.DailyWorkingdays = karkardcount;
                ///مشمول حق بین راهی 
                model.DailyBetweenAway = karkardcount;
                //ایام مشمول بیمه
                model.DailyInsurancePayment = daycount;
                //ایام مشمول مالیات
                model.DailyTaxPayment = daycount;
                //مشمول محاسبه پاداش تولید
                model.DailyReward = daycount;
                /// مشمول محاسبه در روزهای عیدی پذیر
                model.DailyCalculationInEidDays = daycount;
                ///
                var workTimeEmployee = employeemodel.WorkGroup?.WorkTime;
                foreach (var worktimeItem in WorkTimeData)
                {
                    var monthTimeSheetWorkTimemodel = new MonthTimeSheetWorkTimeModel()
                    {
                        Title = worktimeItem.Title,
                        Code = worktimeItem.Code.Trim(),
                        WorkTimeId = worktimeItem.Id
                    };

                    if (workTimeEmployee?.Id == worktimeItem.Id)
                        monthTimeSheetWorkTimemodel.DayCount = daycount;
                    model.MonthTimeSheetWorkTimesModel.Add(monthTimeSheetWorkTimemodel);
                }

            }
            //model.CanEdit = CheckUpdateMonthSheet(model.YearMonth);
            return model;
        }
        /// <summary>
        ///   ایجاد و به روز رسانی تایید کارکرد ماهیانه
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<KscResult> PostMonthTimeSheet(EmployeeMonthTimeSheetModel model)
        {
            var result = model.IsValid();

            var itsForTestProgram = false;

            if (!result.Success)
            {
                return result;
            }
            if (!model.EmployeeID.HasValue)
            {
                throw new HRBusinessException("خطا", MonthTimeSheetResource.RequiredPersonAttributeErrorMessage);
            }
            var employeemodel = await _kscHrUnitOfWork.EmployeeRepository.GetEmployeeByEmployeeId(model.EmployeeID.Value);
            if (employeemodel == null)
            {
                throw new HRBusinessException("خطا", MonthTimeSheetResource.EmptyPersonAttributeErrorMessage);

            }
            if (!employeemodel.WorkGroupId.HasValue || !employeemodel.PaymentStatusId.HasValue)
            {
                throw new HRBusinessException("خطا", MonthTimeSheetResource.EroorPersonAttributeErrorMessage);
            }
            // پرسنل غیرشرکتی امکان ثبت کارکرد وجود ندارد
            if (employeemodel.PersonalTypeId != EnumPersonalType.EmploymentPerson.Id)
            {
                throw new HRBusinessException("خطا", "برای پرسنل غیر شرکتی،امکان ثبت اطلاعات وجود ندارد");
            }
            // پرسنل بازنشسته امکان ثبت کارکرد وجود ندارد
            if (employeemodel.PaymentStatusId == 7)
            {
                throw new HRBusinessException("خطا", MonthTimeSheetResource.PersonPeymentStatusAttributeErrorMessage);
            }
            var today = DateTime.Now;
            int yearMonth = Convert.ToInt32(model.YearMonth.ToEnglishNumbers());
            // در حالت ثبت شرط چک می شود
            if (employeemodel.DismissalDate.HasValue && !model.MonthTimeSheetID.HasValue)
            {
                var dismissalYearMonthDay = employeemodel.DismissalDate.ToPersianYearMonthDay();
                var dismissalYearMonth = $"{dismissalYearMonthDay.Year}{dismissalYearMonthDay.Month.ToString().PadLeft(2, '0')}";
                if (dismissalYearMonth.CompareTo(yearMonth.ToString()) < 0)
                    throw new HRBusinessException("خطا", MonthTimeSheetResource.DismissalDateErrorMessage);
            }
            var CurrentUser = model.InsertUser;
            string excessOverTime = $"{model.ExcessOverTimeHour.ToString().PadLeft(3, '0')}:{model.ExcessOverTimeMinute.ToString().PadLeft(2, '0')}";
            string averageBalanceOverTime = $"{model.AverageBalanceOverTimeHour.ToString().PadLeft(3, '0')}:{model.AverageBalanceOverTimeMinute.ToString().PadLeft(2, '0')}";
            string sumInvalidOverTimInDailyTimeSheet = $"{model.SumInvalidOverTimInDailyTimeSheetHour.ToString().PadLeft(3, '0')}:{model.SumInvalidOverTimInDailyTimeSheetMinute.ToString().PadLeft(2, '0')}";
            string operation = "";
            if (!model.ExcessOverTimeMinute.IsValidateMinute())
            {
                throw new HRBusinessException("خطا", MonthTimeSheetResource.ExcessOverTimeMinuteAttributeErrorMessage);

            }
            var workCalendarData = _kscHrUnitOfWork.WorkCalendarRepository.Where(x => x.YearMonthV1 == yearMonth).Select(x => new { x.Id, x.WorkDayTypeId }).ToList();
            var daycount = workCalendarData.Count();
            if (
                model.DailyBetweenAway > daycount ||
                model.DailyCalculationInEidDays > daycount ||
                model.DailyDeductionReward > daycount ||
                model.DailyInsurancePayment > daycount ||
                model.DailyTaxPayment > daycount ||
                model.DailyWorkingdays > daycount)
            {
                throw new HRBusinessException("خطا", MonthTimeSheetResource.DayAttributeErrorMessage);
            }
            MonthTimeSheet newMonthTimeSheet = null;
            List<SearchTimeSheetRollCallModel> monthTimeSheetRollCallList = JsonConvert.DeserializeObject<List<SearchTimeSheetRollCallModel>>(model.MonthTimeSheetRollCallModels);
            List<MonthTimeSheetWorkTimeModel> monthTimeSheetWorkTimeList = JsonConvert.DeserializeObject<List<MonthTimeSheetWorkTimeModel>>(model.MonthTimeSheetWorkTimesModels);
            //monthTimeSheetWorkTimeList = monthTimeSheetWorkTimeList.Where(x => x.DayCount > 0).ToList();
            var salaryCode = _kscHrUnitOfWork.RollCallSalaryCodeRepository.WhereQueryable(x => x.IsActive == true && x.EmploymentTypeCode == employeemodel.EmploymentTypeId).ToList();
            if (monthTimeSheetRollCallList == null)
            {
                throw new HRBusinessException(Validations.RepetitiveId, MonthTimeSheetResource.EmployeeWorkAttributeErrorMessage);
            }
            if (model.MonthTimeSheetID.HasValue)
            {
                operation = "U";
                newMonthTimeSheet = _kscHrUnitOfWork.MonthTimeSheetRepository.GetIncludedMonthTimeSheet().FirstOrDefault(x => x.Id == model.MonthTimeSheetID.Value);
                if (newMonthTimeSheet == null)
                {
                    throw new HRBusinessException("خطا", MonthTimeSheetResource.MonthTimeSheetAttributeErrorMessage);
                }
                //newMonthTimeSheet.CurrentMonthMerit = model.ContDayForcurrentMonthMerit;
                newMonthTimeSheet.ExcessOverTime = excessOverTime;
                newMonthTimeSheet.UpdateDate = today;
                newMonthTimeSheet.UpdateUser = CurrentUser;
                //newMonthTimeSheet.AverageBalanceOverTime = AverageBalanceOverTime;
                newMonthTimeSheet.SumInvalidOverTimInDailyTimeSheet = sumInvalidOverTimInDailyTimeSheet;
                var activeMonthTimesheet = newMonthTimeSheet.MonthTimeSheetIncludeds.Where(x => x.IsActive);
                // ایام کارکرد
                var monthTimeSheetIncludedWorkingdays = activeMonthTimesheet.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.Workingdays.Id);
                if (monthTimeSheetIncludedWorkingdays != null)
                {
                    if (monthTimeSheetIncludedWorkingdays.DurationInDay != model.DailyWorkingdays)
                    {
                        activeMonthTimesheet.Where(x => x.IncludedDefinitionId == EnumIncludedDefinition.Workingdays.Id && x.DurationInDay != model.DailyWorkingdays).ToList().ForEach(x => x.IsActive = false);
                        newMonthTimeSheet.MonthTimeSheetIncludeds.Add(new MonthTimeSheetIncluded
                        {
                            IncludedDefinitionId = EnumIncludedDefinition.Workingdays.Id,
                            DurationInDay = model.DailyWorkingdays,
                            IsActive = true,
                            InsertDate = today,
                            InsertUser = CurrentUser,
                        });
                    }
                }
                else newMonthTimeSheet.MonthTimeSheetIncludeds.Add(new MonthTimeSheetIncluded
                {
                    IncludedDefinitionId = EnumIncludedDefinition.Workingdays.Id,
                    DurationInDay = model.DailyWorkingdays,
                    IsActive = true,
                    InsertDate = today,
                    InsertUser = CurrentUser,
                });
                // 
                //ایام مشمول بیمه
                var monthTimeSheetIncludedInsurancePayment = activeMonthTimesheet.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.InsurancePayment.Id);
                if (monthTimeSheetIncludedInsurancePayment != null)
                {
                    if (monthTimeSheetIncludedInsurancePayment.DurationInDay != model.DailyInsurancePayment)
                    {
                        activeMonthTimesheet.Where(x => x.IncludedDefinitionId == EnumIncludedDefinition.InsurancePayment.Id && x.DurationInDay != model.DailyInsurancePayment).ToList().ForEach(x => x.IsActive = false);
                        newMonthTimeSheet.MonthTimeSheetIncludeds.Add(new MonthTimeSheetIncluded
                        {
                            IncludedDefinitionId = EnumIncludedDefinition.InsurancePayment.Id,
                            DurationInDay = model.DailyInsurancePayment,
                            IsActive = true,
                            InsertDate = today,
                            InsertUser = CurrentUser,
                        });
                    }
                }
                else newMonthTimeSheet.MonthTimeSheetIncludeds.Add(new MonthTimeSheetIncluded
                {
                    IncludedDefinitionId = EnumIncludedDefinition.InsurancePayment.Id,
                    DurationInDay = model.DailyInsurancePayment,
                    IsActive = true,
                    InsertDate = today,
                    InsertUser = CurrentUser,
                });
                // مشمول پرداخت مالیات
                var monthTimeSheetIncludedTaxPayment = activeMonthTimesheet.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.TaxPayment.Id);
                if (monthTimeSheetIncludedTaxPayment != null)
                {
                    if (monthTimeSheetIncludedTaxPayment.DurationInDay != model.DailyTaxPayment)
                    {
                        activeMonthTimesheet.Where(x => x.IncludedDefinitionId == EnumIncludedDefinition.TaxPayment.Id && x.DurationInDay != model.DailyTaxPayment).ToList().ForEach(x => x.IsActive = false);
                        newMonthTimeSheet.MonthTimeSheetIncludeds.Add(new MonthTimeSheetIncluded
                        {
                            IncludedDefinitionId = EnumIncludedDefinition.TaxPayment.Id,
                            DurationInDay = model.DailyTaxPayment,
                            IsActive = true,
                            InsertDate = today,
                            InsertUser = CurrentUser,
                        });
                    }
                }
                else newMonthTimeSheet.MonthTimeSheetIncludeds.Add(new MonthTimeSheetIncluded
                {
                    IncludedDefinitionId = EnumIncludedDefinition.TaxPayment.Id,
                    DurationInDay = model.DailyTaxPayment,
                    IsActive = true,
                    InsertDate = today,
                    InsertUser = CurrentUser,
                });

                // مشمول محاسبه پاداش تولید             
                var monthTimeSheetIncludedReward = activeMonthTimesheet.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.CaculateReward.Id);
                if (monthTimeSheetIncludedReward != null)
                {
                    if (monthTimeSheetIncludedReward.DurationInDay != model.DailyTaxPayment)
                    {
                        activeMonthTimesheet.Where(x => x.IncludedDefinitionId == EnumIncludedDefinition.CaculateReward.Id && x.DurationInDay != model.DailyTaxPayment).ToList().ForEach(x => x.IsActive = false);
                        newMonthTimeSheet.MonthTimeSheetIncludeds.Add(new MonthTimeSheetIncluded
                        {
                            IncludedDefinitionId = EnumIncludedDefinition.CaculateReward.Id,
                            DurationInDay = model.DailyReward,
                            IsActive = true,
                            InsertDate = today,
                            InsertUser = CurrentUser,
                        });
                    }
                }
                else newMonthTimeSheet.MonthTimeSheetIncludeds.Add(new MonthTimeSheetIncluded
                {
                    IncludedDefinitionId = EnumIncludedDefinition.CaculateReward.Id,
                    DurationInDay = model.DailyReward,
                    IsActive = true,
                    InsertDate = today,
                    InsertUser = CurrentUser,
                });
                // مشمول محاسبه در روزهای عیدی پذیر
                var monthTimeSheetIncludedCalculationInEidDays = activeMonthTimesheet.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.CalculationInEidDays.Id);
                if (monthTimeSheetIncludedCalculationInEidDays != null)
                {
                    if (monthTimeSheetIncludedCalculationInEidDays.DurationInDay != model.DailyCalculationInEidDays)
                    {
                        activeMonthTimesheet.Where(x => x.IncludedDefinitionId == EnumIncludedDefinition.CalculationInEidDays.Id && x.DurationInDay != model.DailyCalculationInEidDays).ToList().ForEach(x => x.IsActive = false);
                        newMonthTimeSheet.MonthTimeSheetIncludeds.Add(new MonthTimeSheetIncluded
                        {
                            IncludedDefinitionId = EnumIncludedDefinition.CalculationInEidDays.Id,
                            DurationInDay = model.DailyCalculationInEidDays,
                            IsActive = true,
                            InsertDate = today,
                            InsertUser = CurrentUser,
                        });
                    }
                }
                else newMonthTimeSheet.MonthTimeSheetIncludeds.Add(new MonthTimeSheetIncluded
                {
                    IncludedDefinitionId = EnumIncludedDefinition.CalculationInEidDays.Id,
                    DurationInDay = model.DailyCalculationInEidDays,
                    IsActive = true,
                    InsertDate = today,
                    InsertUser = CurrentUser,
                });
                ///مشمول حق بین راهی 
                var monthTimeSheetIncludedBetweenAway = activeMonthTimesheet.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.BetweenAway.Id);
                if (monthTimeSheetIncludedBetweenAway != null)
                {
                    if (monthTimeSheetIncludedBetweenAway.DurationInDay != model.DailyBetweenAway)
                    {
                        activeMonthTimesheet.Where(x => x.IncludedDefinitionId == EnumIncludedDefinition.BetweenAway.Id && x.DurationInDay != model.DailyBetweenAway).ToList().ForEach(x => x.IsActive = false);
                        newMonthTimeSheet.MonthTimeSheetIncludeds.Add(new MonthTimeSheetIncluded
                        {
                            IncludedDefinitionId = EnumIncludedDefinition.BetweenAway.Id,
                            DurationInDay = model.DailyBetweenAway,
                            IsActive = true,
                            InsertDate = today,
                            InsertUser = CurrentUser,
                        });
                    }
                }
                else newMonthTimeSheet.MonthTimeSheetIncludeds.Add(new MonthTimeSheetIncluded
                {
                    IncludedDefinitionId = EnumIncludedDefinition.BetweenAway.Id,
                    DurationInDay = model.DailyBetweenAway,
                    IsActive = true,
                    InsertDate = today,
                    InsertUser = CurrentUser,
                });
                /// مشمول کسر حقوق
                var monthTimeSheetIncludedSalaryAggregateDeduction = activeMonthTimesheet.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.SalaryAggregateDeduction.Id);
                if (monthTimeSheetIncludedSalaryAggregateDeduction != null)
                {
                    var dailySalaryAggregateDeduction = model.DailySalaryAggregateDeduction.ConvertDurationToMinute();
                    if ((monthTimeSheetIncludedSalaryAggregateDeduction.DurationInHour ?? 0).ConvertDurationInHour().ConvertDurationToMinute() != dailySalaryAggregateDeduction)
                    {

                        activeMonthTimesheet.Where(x => x.IncludedDefinitionId == EnumIncludedDefinition.SalaryAggregateDeduction.Id && (x.DurationInHour ?? 0).ConvertDurationInHour().ConvertDurationToMinute() != dailySalaryAggregateDeduction).ToList().ForEach(x => x.IsActive = false);
                        newMonthTimeSheet.MonthTimeSheetIncludeds.Add(new MonthTimeSheetIncluded
                        {
                            IncludedDefinitionId = EnumIncludedDefinition.SalaryAggregateDeduction.Id,
                            DurationInHour = model.DailySalaryAggregateDeduction.ConvertDurationToMinute(),
                            IsActive = true,
                            InsertDate = today,
                            InsertUser = CurrentUser,
                        });
                    }
                }
                else newMonthTimeSheet.MonthTimeSheetIncludeds.Add(new MonthTimeSheetIncluded
                {
                    IncludedDefinitionId = EnumIncludedDefinition.SalaryAggregateDeduction.Id,
                    DurationInHour = model.DailySalaryAggregateDeduction.ConvertDurationToMinute(),
                    IsActive = true,
                    InsertDate = today,
                    InsertUser = CurrentUser,
                });
                ///مشمول کسر پاداش ساعتی
                var monthTimeSheetIncludedHourlyDeductionReward = activeMonthTimesheet.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.HourlyDeductionReward.Id);
                if (monthTimeSheetIncludedHourlyDeductionReward != null)
                {
                    var hourlyDeductionReward = model.HourlyDeductionReward.ConvertDurationToMinute();
                    if ((monthTimeSheetIncludedHourlyDeductionReward.DurationInHour ?? 0).ConvertDurationInHour().ConvertDurationToMinute() != hourlyDeductionReward)
                    {
                        activeMonthTimesheet.Where(x => x.IncludedDefinitionId == EnumIncludedDefinition.HourlyDeductionReward.Id && (x.DurationInHour ?? 0).ConvertDurationInHour().ConvertDurationToMinute() != hourlyDeductionReward).ToList().ForEach(x => x.IsActive = false);
                        newMonthTimeSheet.MonthTimeSheetIncludeds.Add(new MonthTimeSheetIncluded
                        {
                            IncludedDefinitionId = EnumIncludedDefinition.HourlyDeductionReward.Id,
                            DurationInHour = model.HourlyDeductionReward.ConvertDurationToMinute(),
                            IsActive = true,
                            InsertDate = today,
                            InsertUser = CurrentUser,
                        });
                    }
                }
                else newMonthTimeSheet.MonthTimeSheetIncludeds.Add(new MonthTimeSheetIncluded
                {
                    IncludedDefinitionId = EnumIncludedDefinition.HourlyDeductionReward.Id,
                    DurationInHour = model.HourlyDeductionReward.ConvertDurationToMinute(),
                    IsActive = true,
                    InsertDate = today,
                    InsertUser = CurrentUser,
                });
                ///مشمول کسر پاداش روزانه
                var monthTimeSheetIncludedDailyDeductionReward = activeMonthTimesheet.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.DailyDeductionReward.Id);
                if (monthTimeSheetIncludedDailyDeductionReward != null)
                {
                    if (monthTimeSheetIncludedDailyDeductionReward.DurationInDay != model.DailyDeductionReward)
                    {
                        activeMonthTimesheet.Where(x => x.IncludedDefinitionId == EnumIncludedDefinition.DailyDeductionReward.Id && x.DurationInDay != model.DailyDeductionReward).ToList().ForEach(x => x.IsActive = false);
                        newMonthTimeSheet.MonthTimeSheetIncludeds.Add(new MonthTimeSheetIncluded
                        {
                            IncludedDefinitionId = EnumIncludedDefinition.DailyDeductionReward.Id,
                            DurationInDay = model.DailyDeductionReward,
                            IsActive = true,
                            InsertDate = today,
                            InsertUser = CurrentUser,
                        });
                    }
                }
                else newMonthTimeSheet.MonthTimeSheetIncludeds.Add(new MonthTimeSheetIncluded
                {
                    IncludedDefinitionId = EnumIncludedDefinition.DailyDeductionReward.Id,
                    DurationInDay = model.DailyDeductionReward,
                    IsActive = true,
                    InsertDate = today,
                    InsertUser = CurrentUser,
                });

                ///
                var VacationCurrentMonthUsedRolCallDefinition = _kscHrUnitOfWork.IncludedRollCallRepository.GetRollCallDefinitionByIncludedDefinitionCode(EnumIncludedDefinition.UseedCurrentMonthVacation.Id.ToString());

                var GetSystemControlDate = _kscHrUnitOfWork.SystemControlDateRepository.GetAllQueryable().AsNoTracking().FirstOrDefault(a => a.IsActive);
                bool isUpdateUsedCurrentYear = CheckIsUpdateUsedCurrentYear(newMonthTimeSheet.YearMonth, GetSystemControlDate.AttendAbsenceItemDate);
                bool isUpdateLastYearVacationRemaining = CheckIsUpdateLastYearVacationRemaining(newMonthTimeSheet.YearMonth, GetSystemControlDate.AttendAbsenceItemDate);

                if (monthTimeSheetRollCallList != null)
                    foreach (var item_rollcall in monthTimeSheetRollCallList)
                    {
                        int? durationInMinut = item_rollcall.Duration.ConvertDurationToMinute();
                        var currentMonthTimeSheetRollCall = newMonthTimeSheet.MonthTimeSheetRollCalls.FirstOrDefault(x => x.RollCallDefinitionId == item_rollcall.RollCallDefinitionId && x.IsActive);
                        bool isEdit = false;
                        if (currentMonthTimeSheetRollCall != null)
                        {
                            isEdit = (currentMonthTimeSheetRollCall.DayCountInDailyTimeSheet != item_rollcall.DayCountInDailyTimeSheet || currentMonthTimeSheetRollCall.DurationInMinut != durationInMinut.Value);
                            if (item_rollcall.IsDeleted || isEdit)
                            {
                                //currentMonthTimeSheetRollCall.DayCountInDailyTimeSheet = item_rollcall.DayCountInDailyTimeSheet;
                                //currentMonthTimeSheetRollCall.DurationInMinut = durationInMinut.Value;
                                currentMonthTimeSheetRollCall.UpdateDate = today;
                                currentMonthTimeSheetRollCall.UpdateUser = CurrentUser;
                                currentMonthTimeSheetRollCall.IsActive = false;
                                // مرخصی
                                if (isEdit && VacationCurrentMonthUsedRolCallDefinition.Contains(item_rollcall.RollCallDefinitionId))
                                {
                                    int newduation = durationInMinut.Value - currentMonthTimeSheetRollCall.DurationInMinut;

                                    if (isUpdateUsedCurrentYear)
                                    {
                                        newduation = newduation * (-1);

                                        var remark = $"تغییرات بعلت تغییرات کارکرد ماهانه در تاریخ {DateTime.Now.ToPersianDate()} به میزان  {newduation.ConvertMinToDaysHour(9 * 60)} می باشد";

                                        _kscHrUnitOfWork.EmployeeVacationManagementRepository.UpdateLastYearVacationRemaining(newMonthTimeSheet.EmployeeId, newduation, remark, CurrentUser);

                                    }
                                    else if (isUpdateLastYearVacationRemaining)
                                    {
                                        var remark = $"تغییرات بعلت تغییرات کارکرد ماهانه در تاریخ {DateTime.Now.ToPersianDate()} به میزان  {newduation.ConvertMinToDaysHour(9 * 60)} می باشد";
                                        _kscHrUnitOfWork.EmployeeVacationManagementRepository.UpdateUsedCurrentYear(newMonthTimeSheet.EmployeeId, newduation, remark, CurrentUser);

                                    }
                                }
                                else if (item_rollcall.IsDeleted && VacationCurrentMonthUsedRolCallDefinition.Contains(item_rollcall.RollCallDefinitionId))
                                {

                                    if (isUpdateUsedCurrentYear)
                                    {
                                        int newduation = (currentMonthTimeSheetRollCall.DurationInMinut);
                                        var remark = $"تغییرات بعلت تغییرات کارکرد ماهانه در تاریخ {DateTime.Now.ToPersianDate()} به میزان  {newduation.ConvertMinToDaysHour(9 * 60)} می باشد";
                                        _kscHrUnitOfWork.EmployeeVacationManagementRepository.UpdateLastYearVacationRemaining(newMonthTimeSheet.EmployeeId, newduation, remark, CurrentUser);
                                    }
                                    else if (isUpdateLastYearVacationRemaining)
                                    {
                                        int newduation = (currentMonthTimeSheetRollCall.DurationInMinut) * (-1);
                                        var remark = $"تغییرات بعلت تغییرات کارکرد ماهانه در تاریخ {DateTime.Now.ToPersianDate()} به میزان  {newduation.ConvertMinToDaysHour(9 * 60)} می باشد";
                                        _kscHrUnitOfWork.EmployeeVacationManagementRepository.UpdateUsedCurrentYear(newMonthTimeSheet.EmployeeId, newduation, remark, CurrentUser);

                                    }
                                }
                            }
                            if (!item_rollcall.IsDeleted && isEdit)
                            {
                                newMonthTimeSheet.MonthTimeSheetRollCalls.Add(
                                       new MonthTimeSheetRollCall
                                       {
                                           RollCallDefinitionId = item_rollcall.RollCallDefinitionId,
                                           DurationInMinut = durationInMinut.Value,
                                           DayCountInDailyTimeSheet = item_rollcall.DayCountInDailyTimeSheet,
                                           IsActive = true,
                                           InsertDate = today,
                                           InsertUser = CurrentUser,
                                       });
                            }
                        }
                        else if (!item_rollcall.IsDeleted)
                        {
                            newMonthTimeSheet.MonthTimeSheetRollCalls.Add(
                                   new MonthTimeSheetRollCall
                                   {
                                       RollCallDefinitionId = item_rollcall.RollCallDefinitionId,
                                       DurationInMinut = durationInMinut.Value,
                                       DayCountInDailyTimeSheet = item_rollcall.DayCountInDailyTimeSheet,
                                       IsActive = true,
                                       InsertDate = today,
                                       InsertUser = CurrentUser,
                                   });
                            if (VacationCurrentMonthUsedRolCallDefinition.Contains(item_rollcall.RollCallDefinitionId))
                            {
                                int newduation = (durationInMinut.Value);
                                if (isUpdateUsedCurrentYear)
                                {
                                    newduation = newduation * (-1);
                                    var remark = $"تغییرات بعلت تغییرات کارکرد ماهانه در تاریخ {DateTime.Now.ToPersianDate()} به میزان  {newduation.ConvertMinToDaysHour(9 * 60)} می باشد";

                                    _kscHrUnitOfWork.EmployeeVacationManagementRepository.UpdateLastYearVacationRemaining(newMonthTimeSheet.EmployeeId, newduation, remark, CurrentUser);

                                }
                                else if (isUpdateLastYearVacationRemaining)
                                {
                                    newduation = newduation;
                                    var remark = $"تغییرات بعلت تغییرات کارکرد ماهانه در تاریخ {DateTime.Now.ToPersianDate()} به میزان  {newduation.ConvertMinToDaysHour(9 * 60)} می باشد";

                                    _kscHrUnitOfWork.EmployeeVacationManagementRepository.UpdateUsedCurrentYear(newMonthTimeSheet.EmployeeId, newduation, remark, CurrentUser);


                                }



                            }
                        }
                    }
                ///
                foreach (var item_worktime in monthTimeSheetWorkTimeList)
                {
                    if (newMonthTimeSheet.MonthTimeSheetWorkTimes.Any(x => x.WorkTimeId == item_worktime.WorkTimeId && x.DayCount != item_worktime.DayCount))
                    {
                        var currentMonthTimeSheetRollCall = newMonthTimeSheet.MonthTimeSheetWorkTimes.First(x => x.WorkTimeId == item_worktime.WorkTimeId);
                        //currentMonthTimeSheetRollCall.DayCount = item_worktime.DayCount ?? 0;
                        currentMonthTimeSheetRollCall.UpdateDate = today;
                        currentMonthTimeSheetRollCall.UpdateUser = CurrentUser;
                        currentMonthTimeSheetRollCall.IsActive = false;
                    }
                    if (!newMonthTimeSheet.MonthTimeSheetWorkTimes.Any(x => x.WorkTimeId == item_worktime.WorkTimeId && x.DayCount == item_worktime.DayCount))
                        newMonthTimeSheet.MonthTimeSheetWorkTimes.Add(new MonthTimeSheetWorkTime
                        {

                            WorkTimeId = item_worktime.WorkTimeId,
                            DayCount = item_worktime.DayCount ?? 0,
                            //ShiftBenefitInMinute = item_worktime.ShiftBenefitInMinute,
                            IsActive = true,
                            InsertDate = today,
                            InsertUser = CurrentUser,
                        });
                }

            }
            else
            {
                operation = "I";
                newMonthTimeSheet = new MonthTimeSheet()
                {
                    YearMonth = yearMonth,
                    EmployeeId = model.EmployeeID.Value,
                    InsertDate = today,
                    InsertUser = CurrentUser,
                    ExcessOverTime = excessOverTime,
                    IsCreatedManual = true,
                    WorkGroupId = employeemodel.WorkGroupId.Value,
                    PaymentStatusId = employeemodel.PaymentStatusId.Value,
                    SumInvalidOverTimInDailyTimeSheet = sumInvalidOverTimInDailyTimeSheet,
                    //CurrentMonthMerit=model.ContDayForcurrentMonthMerit
                };

                // ایام کارکرد
                if (model.DailyWorkingdays.HasValue)
                {
                    newMonthTimeSheet.MonthTimeSheetIncludeds.Add(new MonthTimeSheetIncluded
                    {
                        IncludedDefinitionId = EnumIncludedDefinition.Workingdays.Id,
                        DurationInDay = model.DailyWorkingdays,
                        IsActive = true,
                        InsertDate = today,
                        InsertUser = CurrentUser,
                    });
                }
                //ایام مشمول بیمه
                if (model.DailyInsurancePayment.HasValue)
                {
                    newMonthTimeSheet.MonthTimeSheetIncludeds.Add(new MonthTimeSheetIncluded
                    {
                        IncludedDefinitionId = EnumIncludedDefinition.InsurancePayment.Id,
                        DurationInDay = model.DailyInsurancePayment,
                        IsActive = true,
                        InsertDate = today,
                        InsertUser = CurrentUser,
                    });
                }
                // مشمول پرداخت مالیات
                if (model.DailyTaxPayment.HasValue)
                {
                    newMonthTimeSheet.MonthTimeSheetIncludeds.Add(new MonthTimeSheetIncluded
                    {
                        IncludedDefinitionId = EnumIncludedDefinition.TaxPayment.Id,
                        DurationInDay = model.DailyTaxPayment,
                        IsActive = true,
                        InsertDate = today,
                        InsertUser = CurrentUser,
                    });
                }
                //مشمول محاسبه پاداش تولید
                if (model.DailyReward.HasValue)
                {
                    newMonthTimeSheet.MonthTimeSheetIncludeds.Add(new MonthTimeSheetIncluded
                    {
                        IncludedDefinitionId = EnumIncludedDefinition.CaculateReward.Id,
                        DurationInDay = model.DailyReward,
                        IsActive = true,
                        InsertDate = today,
                        InsertUser = CurrentUser,
                    });
                }
                // مشمول محاسبه در روزهای عیدی پذیر
                if (model.DailyCalculationInEidDays.HasValue)
                {
                    newMonthTimeSheet.MonthTimeSheetIncludeds.Add(new MonthTimeSheetIncluded
                    {
                        IncludedDefinitionId = EnumIncludedDefinition.CalculationInEidDays.Id,
                        DurationInDay = model.DailyCalculationInEidDays,
                        IsActive = true,
                        InsertDate = today,
                        InsertUser = CurrentUser,
                    });
                }
                ///مشمول حق بین راهی 
                if (model.DailyBetweenAway.HasValue)
                {
                    newMonthTimeSheet.MonthTimeSheetIncludeds.Add(new MonthTimeSheetIncluded
                    {
                        IncludedDefinitionId = EnumIncludedDefinition.BetweenAway.Id,
                        DurationInDay = model.DailyBetweenAway,
                        IsActive = true,
                        InsertDate = today,
                        InsertUser = CurrentUser,
                    });
                }
                /// مشمول کسر حقوق
                if (model.DailySalaryAggregateDeduction != null)
                {
                    newMonthTimeSheet.MonthTimeSheetIncludeds.Add(new MonthTimeSheetIncluded
                    {
                        IncludedDefinitionId = EnumIncludedDefinition.SalaryAggregateDeduction.Id,
                        DurationInDay = model.DailySalaryAggregateDeduction.ConvertDurationToMinute(),
                        IsActive = true,
                        InsertDate = today,
                        InsertUser = CurrentUser,
                    });
                }
                ///مشمول کسر پاداش ساعتی
                if (model.HourlyDeductionReward != null)
                {
                    newMonthTimeSheet.MonthTimeSheetIncludeds.Add(new MonthTimeSheetIncluded
                    {
                        IncludedDefinitionId = EnumIncludedDefinition.HourlyDeductionReward.Id,
                        DurationInHour = model.HourlyDeductionReward.ConvertDurationToMinute(),
                        IsActive = true,
                        InsertDate = today,
                        InsertUser = CurrentUser,
                    });
                }
                ///مشمول کسر پاداش روزانه
                if (model.DailyDeductionReward.HasValue)
                {
                    newMonthTimeSheet.MonthTimeSheetIncludeds.Add(new MonthTimeSheetIncluded
                    {
                        IncludedDefinitionId = EnumIncludedDefinition.DailyDeductionReward.Id,
                        DurationInDay = model.DailyDeductionReward,
                        IsActive = true,
                        InsertDate = today,
                        InsertUser = CurrentUser,
                    });
                }
                /////////////
                if (monthTimeSheetRollCallList != null)
                    foreach (var item_rollcall in monthTimeSheetRollCallList.Where(x => x.IsDeleted == false))
                    {
                        int? durationInMinut = item_rollcall.Duration.ConvertDurationToMinute();
                        newMonthTimeSheet.MonthTimeSheetRollCalls.Add(
                                   new MonthTimeSheetRollCall
                                   {
                                       RollCallDefinitionId = item_rollcall.RollCallDefinitionId,
                                       DurationInMinut = durationInMinut.Value,
                                       DayCountInDailyTimeSheet = item_rollcall.DayCountInDailyTimeSheet,
                                       IsActive = !item_rollcall.IsDeleted,
                                       InsertDate = today,
                                       InsertUser = CurrentUser,
                                   });
                    }
                ///
                if (monthTimeSheetWorkTimeList != null)
                    foreach (var item_worktime in monthTimeSheetWorkTimeList)
                    {
                        newMonthTimeSheet.MonthTimeSheetWorkTimes.Add(new MonthTimeSheetWorkTime
                        {

                            WorkTimeId = item_worktime.WorkTimeId,
                            DayCount = item_worktime.DayCount ?? 0,
                            IsActive = true,
                            InsertDate = today,
                            InsertUser = CurrentUser,
                        });
                    }

                await _kscHrUnitOfWork.MonthTimeSheetRepository.AddAsync(newMonthTimeSheet);

            }
            if (itsForTestProgram == true)
            {
                if (await _kscHrUnitOfWork.SaveAsync() > 0)
                    return result;
            }

            var permonth = new PER_MONT()
            {
                NUM_PRSN_EMPL = employeemodel.EmployeeNumber,
                NUM_BNS_DAY_MONTP = (model.DailyCalculationInEidDays ?? 0).ToString(),
                NUM_DAY_NOPYM_PREM_MONTP = model.DailyDeductionReward != null ? model.DailyDeductionReward.ToString() : "0",
                //var _NUM_DAY_NOPYM_PREM_MONTP = ActiveMonthTimeSheetIncludeds.FirstOrDefault(x => x.IncludedDefinitionId == EnumIncludedDefinition.DailyDeductionReward.Id);
                NUM_PNS_DAY_MONTP = (model.DailyInsurancePayment ?? 0).ToString(),
                NUM_TAX_DAY_MONTP = (model.DailyTaxPayment ?? 0).ToString(),
                NUM_TRSP_DAY_MONTP = (model.DailyBetweenAway ?? 0).ToString(),
                NUM_WRK_DAY_MONTP = (model.DailyWorkingdays ?? 0).ToString(),
                //
                NUM_TIM_HOUR_EOVT_MONTP = sumInvalidOverTimInDailyTimeSheet.Replace(":", ""),
                NUM_HOUR_OVT_PAY_EMPL = excessOverTime.Replace(":", ""),
                COUNT_HOURS = monthTimeSheetRollCallList.Where(x => x.IsDeleted == false).Count().ToString(),
                //
                TOT_HOUR_NOPYM_MONTP = model.DailySalaryAggregateDeduction != null ? model.DailySalaryAggregateDeduction.Replace(":", "") : "0",
                // TOT_HOUR_NOPYM_MONTP = "0",
                TOT_HOUR_NOPYM_HLDY_MONTP = "0",
                TOT_HOUR_NOPYM_PREM_MONTP = model.HourlyDeductionReward != null ? model.HourlyDeductionReward.Replace(":", "") : "0",
                //
                MONTHLY_HOURS = monthTimeSheetRollCallList.Where(x => x.IsDeleted == false).Select(x => new MONTHLY_HOURS()
                {
                    COD_ATT_ATABT = x.Code,
                    NUM_TIM_HOUR_MONTP = x.Duration.Replace(":", ""),
                    FK_ACNDF = salaryCode.Where(i => i.RollCallDefinitionId == x.RollCallDefinitionId
                    ).Select(x => x.SalaryAccountCode.ToString()).FirstOrDefault() ?? "0",
                }).ToList(),
                //
                COUNT_SHIFT = monthTimeSheetWorkTimeList.Count(x => x.DayCount > 0).ToString(),
                MONTHLY_SHIFT = monthTimeSheetWorkTimeList.Where(x => x.DayCount > 0).Select(x => new MONTHLY_SHIFT()
                {
                    COD_TYP_WRK_EMPL = x.Code,
                    NUM_DAY_SHIFT_MONTP = (x.DayCount ?? 0).ToString(),
                }).ToList(),
                NAM_USR_DAT_UPD_MONTP = CurrentUser,
            };
            List<PER_MONT> permonthlist = new List<PER_MONT>() { permonth };
            var partmonth = new PAR_MONT()
            {
                COUNT_PER = "1",
                DAT_ATT_MONTP = newMonthTimeSheet.YearMonth.ToString(),
                OPERATION = operation,
                PER_MONT = permonthlist
            };

            ReturnData<PAR_MONT> MISResult = _misUpdateService.ConnectHRToMIS<PAR_MONT>(partmonth, "S6XML029", "PAR_MONT", model.DomainName);
            if (MISResult.IsSuccess == true)
            {
                if (MISResult.Data.OPERATION == "E")
                {
                    throw new HRBusinessException(Validations.RepetitiveId, String.Join(",", MISResult.Messages.ToList()));
                }
                // save
                if (await _kscHrUnitOfWork.SaveAsync() > 0)
                    return result;
                throw new HRBusinessException("رکورد نامعتبر", Errors.InValidRecord);

            }
            else
            {
                throw new HRBusinessException(Validations.RepetitiveId, String.Join(",", MISResult.Messages.ToList()));
            }
        }

        private bool CheckIsUpdateLastYearVacationRemaining(int CurrentyearMonth, int AttendAbbcenseyearMonth)
        {
            var result = false;

            var currentYear = int.Parse(CurrentyearMonth.ToString().Substring(0, 4));
            var currentMonth = int.Parse(CurrentyearMonth.ToString().Substring(4, 2));

            var AttendAbbcenseYear = int.Parse(AttendAbbcenseyearMonth.ToString().Substring(0, 4));
            var AttendAbbcenseMonth = int.Parse(AttendAbbcenseyearMonth.ToString().Substring(4, 2));
            if (currentYear == AttendAbbcenseYear && currentMonth < AttendAbbcenseMonth)
            {
                result = true;
            }
            return result;
        }

        private bool CheckIsUpdateUsedCurrentYear(int CurrentyearMonth, int AttendAbbcenseyearMonth)
        {
            var result = false;

            var currentYear = int.Parse(CurrentyearMonth.ToString().Substring(0, 4));
            var currentMonth = int.Parse(CurrentyearMonth.ToString().Substring(4, 2));

            var AttendAbbcenseYear = int.Parse(AttendAbbcenseyearMonth.ToString().Substring(0, 4));
            var AttendAbbcenseMonth = int.Parse(AttendAbbcenseyearMonth.ToString().Substring(4, 2));
            if (currentYear < AttendAbbcenseYear)
            {
                result = true;
            }
            return result;
        }

        public bool CheckUpdateMonthSheet(string yearmonth)
        {
            ////api new
            ///
            List<PER_MONT> permonthlist = new List<PER_MONT>() { new PER_MONT() };
            var partmonth = new PAR_MONT()
            {
                COUNT_PER = "1",
                DAT_ATT_MONTP = yearmonth,
                OPERATION = "S",
                PER_MONT = permonthlist
            };

            ReturnData<PAR_MONT> MISResult = _misUpdateService.ConnectHRToMIS<PAR_MONT>(partmonth, "S6XML029", "PAR_MONT");
            if (MISResult.IsSuccess == true)
            {
                if (MISResult.Data.OPERATION == "E")
                {
                    throw new HRBusinessException(Validations.RepetitiveId, String.Join(",", MISResult.Messages.ToList()));
                }
                return true;
            }
            return false;

        }
        #endregion
        public async Task<KscResult> TestMonthTimeSheetCalculateStep(UpdateStatusByYearMonthProcedureModel model)
        {
            var result = new KscResult();
            try
            {
                var YearMonth = int.Parse(model.Yearmonth);
                //var workCalendar = _kscHrUnitOfWork.WorkCalendarRepository.GetByMiladiDate(model.DateTimeSheet.Value);
                var systemStatus = true;// await _kscHrUnitOfWork.WorkCalendarRepository.GetMonthTimeSheetStatus(YearMonth);
                if (systemStatus == true)//کاربران اداری سیستم بسته شده باشد
                {

                    #region محاسبه
                    var employeeTimeSheetByYearMonth = _kscHrUnitOfWork.EmployeeTimeSheetRepository.GetEmployeeTimeSheet(YearMonth).ToList();
                    // افرادی که اضافه کار مازاد سقف دارند
                    var employeeTimeSheetExcessOverTime = employeeTimeSheetByYearMonth.Where(x => x.ExcessOverTime > 0).ToList();
                    //

                    // افرادی که اضافه کار تعدیل میانگین دارند
                    var employeeTimeSheetAverageBalanceOverTime = employeeTimeSheetByYearMonth.Where(x => x.AverageBalanceOverTime != null).ToList();
                    // _kscHrUnitOfWork.EmployeeTimeSheetRepository.GetEmployeeTimeSheetExcessOverTime(YearMonth).Where(x=>x.AverageBalanceOverTime != null).ToList();
                    //


                    //// افرادی که اضافه کار مازاد کلاس آموزش طب دارند
                    //var employeeTimeSheetIndustrialTrainingOverTime = employeeTimeSheetByYearMonth.Where(x => x.TrainingOverTime != null).ToList();
                    ////
                    //// افرادی که اضافه کار جبرانی خدمت دارند
                    //var employeeTimeSheetCompensatoryOverTime = employeeTimeSheetByYearMonth.Where(x => x.CompensatoryOverTime != null).ToList();
                    ////

                    var attendAbcenseItem = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetMonthTimeSheetCalculateAsNoTracking(YearMonth);
                    var MIS_Persons = attendAbcenseItem.Select(x => x.EmployeeId).Distinct().ToList();
                    //var MIS_Persons = _kscHrUnitOfWork.ViewMisEmployeeRepository.WhereQueryable(x => x.HrMonthTimeSheet == 1)
                    //    .Select(x => x.EmployeeId).ToList();

                    var CreatedManualPersons = _kscHrUnitOfWork.MonthTimeSheetRepository.WhereQueryable(x => x.YearMonth == YearMonth && x.IsCreatedManual == true)
                        .Select(x => x.EmployeeId).ToList();

                    var Persons = MIS_Persons.Except(CreatedManualPersons).ToList(); //یافتن افرادی که بایستی تایم شیت اتوماتیک برایشان ساخته شود

                    var Emp = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeIncludedTeamwork(Persons)
                        //.Where(x=>x.Id == 4500)
                        .Where(x => x.WorkGroupId != null)
                        .Select(x => new
                        {
                            EmployeeId = x.Id,
                            PaymentStatusId = x.PaymentStatusId,
                            WorkGroupId = x.WorkGroupId
                        });

                    //MonthTimeSheetRollCall
                    var query_MonthTimeSheetRollCall = (from item in attendAbcenseItem.Where(x => x.InvalidRecord == false)
                                                        group item by new
                                                        {
                                                            RollCallDefinitionId = item.RollCallDefinitionId,
                                                            EmployeeId = item.EmployeeId
                                                        } into newgroup
                                                        select new EmployeeItemGroupModel()
                                                        {
                                                            RollCallDefinitionId = newgroup.Key.RollCallDefinitionId,
                                                            EmployeeId = newgroup.Key.EmployeeId,
                                                            TotalDuration = newgroup.Sum(x => (x.TimeDurationInMinute ?? 0) + (x.IncreasedTimeDuration ?? 0)),
                                                            DayCountInDailyTimeSheet = newgroup.Select(x => x.WorkCalendarId).Distinct().Count()
                                                        }).ToList();
                    //MonthTimeSheetDraft
                    var query_MonthTimeSheetDraft = (from item in _kscHrUnitOfWork.MonthTimeSheetDraftRepository.GetMonthTimeSheetDraftByYearMonth(YearMonth)
                                                     group item by new
                                                     {
                                                         EmployeeId = item.EmployeeId,
                                                         YearMonth = item.YearMonth
                                                     } into newgroup
                                                     select new EmployeeItemGroupModel()
                                                     {
                                                         RollCallDefinitionId = 7,
                                                         EmployeeId = newgroup.Key.EmployeeId,
                                                         TotalDuration = newgroup.Sum(x => x.ForcedOverTime),
                                                         DayCountInDailyTimeSheet = 0
                                                     }).ToList();

                    query_MonthTimeSheetRollCall.AddRange(query_MonthTimeSheetDraft);
                    // افزودن اضافه  کار طب صنعتی
                    var query_MonthTimeSheetIndustrialTraining = employeeTimeSheetByYearMonth.Where(x => x.TrainingOverTime != null)
                              .Select(x =>
                              new EmployeeItemGroupModel()
                              {
                                  RollCallDefinitionId = EnumRollCallDefinication.IndustrialTrainingOverTime.Id,
                                  EmployeeId = x.EmployeeId,
                                  TotalDuration = x.TrainingOverTime.Value,
                                  DayCountInDailyTimeSheet = 0
                              }).ToList();
                    query_MonthTimeSheetRollCall.AddRange(query_MonthTimeSheetIndustrialTraining);
                    // افزودن اضافه کار جبرانی خدمت 
                    var query_MonthTimeSheetCompensatoryOverTime = employeeTimeSheetByYearMonth.Where(x => x.CompensatoryOverTime != null).Select(x =>
                        new EmployeeItemGroupModel()
                        {
                            RollCallDefinitionId = EnumRollCallDefinication.CompensatoryOverTime.Id,
                            EmployeeId = x.EmployeeId,
                            TotalDuration = x.CompensatoryOverTime.Value,
                            DayCountInDailyTimeSheet = 0
                        }).ToList();
                    query_MonthTimeSheetRollCall.AddRange(query_MonthTimeSheetCompensatoryOverTime);
                    //
                    var tt1 = query_MonthTimeSheetRollCall.Where(x => x.EmployeeId == 4506);
                    // اصلاح مدت زمان اضافه کار
                    EditRollCallOverTimeByOverTimePriority(employeeTimeSheetExcessOverTime, query_MonthTimeSheetRollCall);
                    var tt2 = query_MonthTimeSheetRollCall.Where(x => x.EmployeeId == 4506);

                    //MonthTimeSheetWorkTime
                    var query_MonthTimeSheetWorkTime = (from item in attendAbcenseItem.Where(x => x.InvalidRecord == false)
                                                        join includedRollCall in _kscHrUnitOfWork.IncludedRollCallRepository.GetAllAsNoTrackingByIncludedDefinition()
                                                        on item.RollCallDefinitionId equals includedRollCall.RollCallDefinitionId
                                                        where includedRollCall.IncludedDefinition.IsPerWorkTime == true
                                                        group item by new
                                                        {
                                                            WorkTimeId = item.WorkTimeId,
                                                            EmployeeId = item.EmployeeId
                                                        } into newgroup
                                                        select new EmployeeItemGroupModel()
                                                        {
                                                            WorkTimeId = newgroup.Key.WorkTimeId,
                                                            EmployeeId = newgroup.Key.EmployeeId,
                                                            WorkTimeDays = newgroup.Select(x => x.WorkCalendarId).Distinct().Count(),
                                                            ShiftBenefitInMinute = newgroup.Sum(x => x.TimeDurationInMinute.Value)
                                                        }).OrderBy(x => x.EmployeeId).ToList();

                    //MonthTimeSheetIncluded 
                    //dbo.IncludedDefinition.IsSumDuration' is invalid in the select list because it is not contained in either an aggregate function or the GROUP BY clause.
                    var query_MonthTimeSheetIncluded = (from item in attendAbcenseItem.Where(x => x.InvalidRecord == false)
                                                        join includedRollCall in _kscHrUnitOfWork.IncludedRollCallRepository.GetAllAsNoTrackingByIncludedDefinition()
                                                       on item.RollCallDefinitionId equals includedRollCall.RollCallDefinitionId
                                                        where includedRollCall.IncludedDefinition.IsMonthlyTimeSheet == true
                                                        && includedRollCall.IncludedDefinition.IsPerWorkTime == false
                                                        group item by new
                                                        {
                                                            EmployeeId = item.EmployeeId,
                                                            IncludedDefinitionId = includedRollCall.IncludedDefinitionId,
                                                            IsSumDuration = includedRollCall.IncludedDefinition.IsSumDuration
                                                            //DayOrHour = includedRollCall.IncludedDefinition.IsSumDuration == true ? "H" : "D",
                                                        }
                                                     into newgroup
                                                        select new EmployeeItemGroupModel()
                                                        {
                                                            EmployeeId = newgroup.Key.EmployeeId,
                                                            IncludedDefinitionId = newgroup.Key.IncludedDefinitionId,
                                                            DurationInDay = newgroup.Key.IsSumDuration ? null : newgroup.Select(x => x.WorkCalendarId).Distinct().Count(),//روزانه
                                                            DurationInHour = newgroup.Key.IsSumDuration ? newgroup.Sum(x => x.TimeDurationInMinute.Value) : null,//ساعتی

                                                        }).OrderBy(x => x.EmployeeId).ToList();
                    //var tt= query_MonthTimeSheetIncluded.Where(x => x.EmployeeId == 3962);//test
                    // var tt = query_MonthTimeSheetIncluded.Where(x => x.EmployeeId == 4500);
                    List<int?> rollcalTypes = new List<int?> { 8, 9, 13 };
                    // var test = attendAbcenseItem.Where(x => x.RollCallDefinitionId == null).ToList();
                    var inValidExtraWorks = attendAbcenseItem.Where(x => x.InvalidRecord == true && rollcalTypes.Contains(x.RollCallDefinition.RollCallCategoryId)).
                        GroupBy(x => x.EmployeeId).Select(x => new
                        {
                            EmployeeId = x.Key,
                            SumDuration = x.Sum(c => c.TimeDurationInMinute.Value)
                        })
                        .ToList();

                    List<MonthTimeSheet> MonthTimeSheetDatas = new List<MonthTimeSheet>();
                    var today = DateTime.Now;
                    var CurrentUser = model.CurrentUser;
                    var dataResult = Emp.ToList();
                    foreach (var item in dataResult)
                    {
                        var item_MonthTimeSheetIncluded = query_MonthTimeSheetIncluded.Where(x => x.EmployeeId == item.EmployeeId);
                        List<MonthTimeSheetIncluded> DataMonthTimeSheetIncluded = new List<MonthTimeSheetIncluded>();
                        foreach (var item1 in item_MonthTimeSheetIncluded)
                        {
                            DataMonthTimeSheetIncluded.Add(new MonthTimeSheetIncluded
                            {
                                IncludedDefinitionId = item1.IncludedDefinitionId,
                                DurationInHour = item1.DurationInHour,
                                DurationInDay = item1.DurationInDay,
                                IsActive = true,
                                InsertDate = today,
                                InsertUser = CurrentUser,
                            });
                        }


                        var item_MonthTimeSheetWorkTime = query_MonthTimeSheetWorkTime.Where(x => x.EmployeeId == item.EmployeeId);
                        List<MonthTimeSheetWorkTime> DataMonthTimeSheetWorkTime = new List<MonthTimeSheetWorkTime>();
                        foreach (var item2 in item_MonthTimeSheetWorkTime)
                        {
                            DataMonthTimeSheetWorkTime.Add(new MonthTimeSheetWorkTime
                            {
                                WorkTimeId = item2.WorkTimeId,
                                DayCount = item2.WorkTimeDays,
                                ShiftBenefitInMinute = item2.ShiftBenefitInMinute,
                                IsActive = true,
                                InsertDate = today,
                                InsertUser = CurrentUser,
                            });
                        }


                        var item_MonthTimeSheetRollCall = query_MonthTimeSheetRollCall.Where(x => x.EmployeeId == item.EmployeeId);
                        List<MonthTimeSheetRollCall> DataMonthTimeSheetRollCall = new List<MonthTimeSheetRollCall>();
                        foreach (var item3 in item_MonthTimeSheetRollCall)
                        {
                            var temp = new MonthTimeSheetRollCall();
                            temp.RollCallDefinitionId = item3.RollCallDefinitionId;
                            temp.DurationInMinut = int.Parse(item3.TotalDuration.ToString());
                            temp.DayCountInDailyTimeSheet = item3.DayCountInDailyTimeSheet;
                            temp.IsActive = true;
                            temp.InsertDate = today;
                            temp.InsertUser = CurrentUser;

                            if (temp.DurationInMinut != 0)
                                DataMonthTimeSheetRollCall.Add(temp);

                            //DataMonthTimeSheetRollCall.Add(
                            //    new MonthTimeSheetRollCall
                            //    {
                            //        RollCallDefinitionId = item3.RollCallDefinitionId,
                            //        DurationInMinut = int.Parse(item3.TotalDuration.ToString()),
                            //        DayCountInDailyTimeSheet = item3.DayCountInDailyTimeSheet,
                            //        IsActive = true,
                            //        InsertDate = today,
                            //        InsertUser = CurrentUser,
                            //    });
                        }
                        var excessOverTime = employeeTimeSheetExcessOverTime.FirstOrDefault(x => x.EmployeeId == item.EmployeeId);
                        var averageBalanceOverTime = employeeTimeSheetAverageBalanceOverTime.FirstOrDefault(p => p.EmployeeId == item.EmployeeId);

                        MonthTimeSheet monthTimeSheet = new MonthTimeSheet()
                        {
                            YearMonth = YearMonth,
                            EmployeeId = item.EmployeeId,
                            PaymentStatusId = item.PaymentStatusId.Value,
                            WorkGroupId = item.WorkGroupId.Value,
                            MonthTimeSheetIncludeds = DataMonthTimeSheetIncluded,
                            MonthTimeSheetRollCalls = DataMonthTimeSheetRollCall,
                            MonthTimeSheetWorkTimes = DataMonthTimeSheetWorkTime,
                            SumInvalidOverTimInDailyTimeSheet = inValidExtraWorks.Any(x => x.EmployeeId == item.EmployeeId) ?
                            inValidExtraWorks.First(x => x.EmployeeId == item.EmployeeId).SumDuration.ConvertMinuteToDuration() : "000:00",
                            ExcessOverTime = excessOverTime != null ? excessOverTime.ExcessOverTimeDuration : "000:00",
                            InsertDate = today,
                            InsertUser = CurrentUser,
                            AverageBalanceOverTime = averageBalanceOverTime != null ? averageBalanceOverTime.AverageBalanceOverTimeDuration : "000:00",
                        };

                        MonthTimeSheetDatas.Add(monthTimeSheet);
                    }
                    var ttt3 = MonthTimeSheetDatas.Where(x => x.EmployeeId == 4506);
                    string path = @"d:\\MISTXT\";

                    System.IO.StreamWriter PER_MONT_File = new System.IO.StreamWriter(path + "monthRollCall.TXT"); //d:\\MISTXT\PER_MONT_File.txt
                    foreach (var month in MonthTimeSheetDatas)
                    {
                        foreach (var item in month.MonthTimeSheetRollCalls)
                        {
                            PER_MONT_File.WriteLine(month.EmployeeId + "|" + item.RollCallDefinitionId + "|" + item.DurationInMinut);
                        }
                    }
                    PER_MONT_File.Close();
                    //

                    var workCalendar = _kscHrUnitOfWork.WorkCalendarRepository.WhereQueryable(x => x.YearMonthV1 == YearMonth);
                    foreach (var item in workCalendar)
                    {
                        item.SystemSequenceStatusId = EnumSystemSequenceStatusDailyTimeSheet.TimeSheetIsCreated.Id;
                    }




                    #endregion

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
        public async Task<KscResult> TestCeilingOvertimeTimeSheetStep(UpdateStatusByYearMonthProcedureModel model)
        {
            var result = new KscResult();
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

                    var employee = _kscHrUnitOfWork.EmployeeRepository.WhereQueryable(x => x.Id == 53);
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
                    var tt1 = resultData.Where(x => x.EmployeeId == 53);
                    var timesheet = new StringBuilder();
                    foreach (var item in resultData)
                    {
                        timesheet.AppendLine(item.EmployeeId + "|" + item.CeilingOvertime + "|" + item.ExcessOverTime);

                    }
                    var content_timesheet = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(timesheet.ToString()));

                    var FileforMIS = new List<File>{
                    new File { filename = "TestCeilingOvertimeTimeSheetStep" , file = content_timesheet },

                };

                    result = _misUpdateService.SendTextByteFileToMis(Utility.ServerPathSendFileStream, FileforMIS);
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
        public async Task<KscResult> MonthTimeSheetDeleteStep(UpdateStatusByYearMonthProcedureModel model)
        {
            var result = new KscResult();
            _dbTransaction.BeginTransaction();
            try
            {
                var yearMonth = int.Parse(model.Yearmonth);
                var systemStatus = await _kscHrUnitOfWork.WorkCalendarRepository.IsValidSystemControlDate(yearMonth, EnumSystemSequenceStatusDailyTimeSheet.TimeSheetIsCreated.Id);
                if (systemStatus == true)//کاربران اداری سیستم بسته شده باشد
                {
                    if (_kscHrUnitOfWork.Stepper_ProcedureStatusRepository.GetAllQueryable().Any(x => x.YearMonth == yearMonth && x.ProcedureId == EnumProcedureStep.SendToMIS.Id))
                    {
                        throw new Exception("انتقال به MIS انجام شده است");
                    }
                    //
                    var workCalendar = _kscHrUnitOfWork.WorkCalendarRepository.WhereQueryable(x => x.YearMonthV1 == yearMonth);
                    foreach (var item in workCalendar)
                    {
                        item.SystemSequenceStatusId = EnumSystemSequenceStatusDailyTimeSheet.InActiveForAllUser.Id;
                    }
                    //
                    model.Result = "حذف کارکرد ماهیانه با موفقیت انجام شد";
                    //model.ResultCount = MonthTimeSheetDatas.Count();
                    result = await _procedureService.InsertStepProcedure(model);
                    await _kscHrUnitOfWork.SaveAsync(checklog: false);
                    //
                    _kscHrUnitOfWork.MonthTimeSheetRepository.DeleteMonthTimeSheet(yearMonth);
                    _dbTransaction.Commit();
                    //await _kscHrUnitOfWork.SaveAsync(checklog: false);
                }
                else
                {
                    throw new Exception("با توجه به وضعیت سیستم،امکان حذف وجود ندارد");

                }
            }
            catch (Exception ex)
            {
                _dbTransaction.Rollback();

                result.AddError("", ex.Message);
            }
            return result;
        }
        public List<ReportMonthTimeSheet> ReportMonthTimeSheet(SearchPivoteReport search)
        {
            var query = _kscHrUnitOfWork.MonthTimeSheetRepository.GetAllQueryable()
                .Include(a => a.Employee).Include(a => a.MonthTimeSheetRollCalls).AsNoTracking();
            if (search.StartYearMonth > 0)
            {
                query = query.Where(a => a.YearMonth >= search.StartYearMonth);
            }
            if (search.EndYearMonth > 0)
            {
                query = query.Where(a => a.YearMonth >= search.EndYearMonth);
            }
            var queryList = query.Select(a => new
            {
                EmployeeNumber = a.Employee.EmployeeNumber,
                FullName = a.Employee.Name + " " + a.Employee.Family,
                NationalCode = a.Employee.NationalCode,
                FatherName = a.Employee.FatherName,
                MonthTimeSheetRollCalls = a.MonthTimeSheetRollCalls.Where(x => x.IsActive).Select(c => new
                {
                    c.DurationInMinut,
                    c.RollCallDefinitionId
                }).ToList()
            }).OrderBy(a => a.EmployeeNumber).ToList();
            var result = queryList.Select(a => new ReportMonthTimeSheet()
            {
                EmployeeNumber = a.EmployeeNumber,
                FullFamily = a.FullName,
                NationalCode = a.NationalCode,
                FatherName = a.FatherName,
                Code1 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 1) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 1).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code2 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 2) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 2).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code3 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 3) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 3).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code4 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 4) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 4).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code5 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 5) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 5).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code6 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 6) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 6).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code7 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 7) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 7).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code8 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 8) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 8).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code9 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 9) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 9).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code10 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 10) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 10).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code11 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 11) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 11).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code12 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 12) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 12).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code13 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 13) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 13).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code14 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 14) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 14).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code15 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 15) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 15).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code16 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 16) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 16).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code17 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 17) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 17).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code18 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 18) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 18).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code19 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 19) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 19).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code20 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 20) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 20).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code21 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 21) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 21).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code22 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 22) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 22).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code23 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 23) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 23).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code24 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 24) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 24).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code25 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 25) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 25).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code26 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 26) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 26).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code27 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 27) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 27).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code28 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 28) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 28).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code29 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 29) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 29).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code30 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 30) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 30).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code31 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 31) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 31).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code32 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 32) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 32).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code33 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 33) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 33).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code34 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 34) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 34).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code35 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 35) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 35).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code36 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 36) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 36).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code37 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 37) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 37).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code38 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 38) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 38).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code39 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 39) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 39).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code40 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 40) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 40).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code41 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 41) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 41).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code42 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 42) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 42).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code43 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 43) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 43).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code44 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 44) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 44).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code45 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 45) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 45).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code46 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 46) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 46).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code47 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 47) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 47).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code48 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 48) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 48).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code49 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 49) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 49).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code50 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 50) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 50).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code51 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 51) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 51).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code52 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 52) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 52).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code53 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 53) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 53).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code54 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 54) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 54).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code55 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 55) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 55).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code56 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 56) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 56).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code57 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 57) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 57).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code58 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 58) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 58).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code59 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 59) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 59).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code60 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 60) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 60).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code61 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 61) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 61).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code62 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 62) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 62).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code63 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 63) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 63).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code64 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 64) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 64).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code65 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 65) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 65).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code66 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 69) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 66).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code67 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 67) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 67).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code68 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 68) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 68).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code69 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 69) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 69).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code70 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 70) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 70).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code71 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 71) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 71).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code72 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 72) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 72).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code73 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 73) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 73).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code74 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 74) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 74).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code75 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 75) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 75).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code76 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 76) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 76).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code77 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 77) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 77).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code78 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 78) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 78).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code79 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 79) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 79).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code80 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 80) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 80).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code81 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 81) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 81).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code82 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 82) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 82).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code83 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 83) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 83).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code84 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 84) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 84).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code85 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 85) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 85).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code86 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 86) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 86).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code87 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 87) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 87).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code88 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 88) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 88).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code89 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 89) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 89).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code90 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 90) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 90).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code91 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 91) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 91).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code92 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 92) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 92).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code93 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 93) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 93).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code94 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 94) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 94).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code95 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 95) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 95).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code96 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 96) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 96).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code97 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 97) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 97).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code98 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 98) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 98).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code99 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 99) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 99).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code100 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 100) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 100).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code101 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 101) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 101).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code102 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 102) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 102).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code103 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 103) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 103).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : "",
                Code104 = a.MonthTimeSheetRollCalls.Any(a => a.RollCallDefinitionId == 104) ? a.MonthTimeSheetRollCalls.Where(a => a.RollCallDefinitionId == 104).Sum(a => a.DurationInMinut).ConvertMinuteToDuration() : ""


            }).ToList();

            return result;


        }


        public List<List<PivoteMonthTimesheet>> GetPivoteMonthTimeSheet(SearchPivoteReport model)
        {
            var result = _kscHrUnitOfWork.MonthTimeSheetRepository.GetPivoteMonthTimeSheet(model.StartYearMonth.ToString(), model.EndYearMonth.ToString(), model.RollCallId);
            return result;
        }

    }
}
