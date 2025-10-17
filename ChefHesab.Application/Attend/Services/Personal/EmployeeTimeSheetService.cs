using AutoMapper;
using Ksc.Hr.Domain.Shared;
using Ksc.HR.Appication.Interfaces.Personal;
using Ksc.HR.Application;
using Ksc.HR.Application.Interfaces;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.Personal.EmployeeTimeSheet;
using Ksc.HR.DTO.Report;
using Ksc.HR.DTO.Stepper;
using Ksc.HR.DTO.WorkShift;
using Ksc.HR.DTO.WorkShift.IncludedDefinition;
using Ksc.HR.Resources.Messages;
using Ksc.HR.Share.Extention;
using Ksc.HR.Share.General;
using Ksc.HR.Share.Model.PaymentStatus;
using Ksc.HR.Share.Model.SystemSequenceStatus;
using Ksc.HR.Share.WorkShift;
using KSC.Common;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using KSC.Identity.Http;
using KSC.MIS.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
namespace Ksc.HR.Appication.Services.Personal
{
    public class EmployeeTimeSheetService : IEmployeeTimeSheetService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;
        private readonly IStepper_ProcedureService _procedureService;
        // private readonly IEmployeeWorkGroupRepository _employeeWorkGroupRepository;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;

        private readonly IHttpClientServiceEx _client;
        public EmployeeTimeSheetService(
            IKscHrUnitOfWork kscHrUnitOfWork
            , IMapper mapper
            , IFilterHandler FilterHandler
            ,IHttpClientServiceEx client
            , IStepper_ProcedureService procedureService
            )
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
            _client = client;
            _procedureService = procedureService;

        }

        public FilterResult<EmployeeTimeSheetMonthReportModel> GetEmployeeTimeSheetByRelated(EmployeeTimeSheetMonthReportSearchModel Filter)
        {
            var query = _kscHrUnitOfWork.EmployeeTimeSheetRepository.GetEmployeeTimeSheetByRelated(Filter.YearMonth)
                .Select(x => new EmployeeTimeSheetMonthReportModel
                {
                    PersonalNumber = x.Employee.EmployeeNumber,
                    MaximumDuration = x.Employee.TeamWork.OverTimeDefinition.MaximumDuration,//--سقف اضافه کار
                    TeamCode = x.Employee.TeamWork.Code,
                    TeamTitle = x.Employee.TeamWork.Title,
                    FullName = x.Employee.Name + " " + x.Employee.Family,
                    CeilingOvertime = x.CeilingOvertime,//جمع اضافه کار
                    ExcessOverTime = x.ExcessOverTime,//اضافه کار کسر شده
                    //ExcessOverTimeDuration = Utility.ConvertMinuteToDuration((int)src.ExcessOverTime);

                })
                ;




            var result = _FilterHandler.GetFilterResult<EmployeeTimeSheetMonthReportModel>(query, Filter, "PersonalNumber");
            var finalData = result.Data.ToList();
            foreach (var item in finalData)
            {
                item.ExcessOverTimeDuration = Utility.ConvertMinuteToDuration((int)item.ExcessOverTime);
                item.CeilingOvertime_Format = Utility.ConvertMinuteToDuration(item.CeilingOvertime);
            }

            return new FilterResult<EmployeeTimeSheetMonthReportModel>()
            {
                Data = finalData,
                Total = result.Total

            };
        }

        public FilterResult<EmployeeTimeSheetGridManageModel> GetEmployeeOverTimeDetailForBalanceAverage(ReportSearchModel Filter)
        {
            var query_ViewMisEmployeeSecurity = _kscHrUnitOfWork.ViewEmployeeTeamUserActiveRepository.GetAllAsNoTracking();

            if (Filter.FromTeam > 0)
                query_ViewMisEmployeeSecurity = query_ViewMisEmployeeSecurity.Where(x => x.Code.CompareTo(Filter.FromTeam.ToString()) >= 0);
            if (Filter.ToTeam > 0)
                query_ViewMisEmployeeSecurity = query_ViewMisEmployeeSecurity.Where(x => x.Code.CompareTo(Filter.ToTeam.ToString()) <= 0);

            if (Filter.EmployeeIds.Count() > 0)
            {

                query_ViewMisEmployeeSecurity = query_ViewMisEmployeeSecurity.Where(x => Filter.EmployeeIds.Contains(x.Id));
            }

            if (Filter.FromTeam == 0) return new FilterResult<EmployeeTimeSheetGridManageModel>()
            {
                Data = new List<EmployeeTimeSheetGridManageModel>(),
                Total = 0

            }; ;

            if (!Filter.IsOfficialAttendAbcense)
            {
                query_ViewMisEmployeeSecurity = query_ViewMisEmployeeSecurity.Where(x => (x.DisplaySecurity == 1 || x.DisplaySecurity == 2) && x.WindowsUser.ToLower() == Filter.CurrentUserName);
            }
            var employeeIds = query_ViewMisEmployeeSecurity.Select(a => a.Id).ToList();

            Filter.YearMonth = int.Parse(Filter.YearMonthString.Fa2En());
            var sartDate = int.Parse(Filter.YearMonth.ToString() + "01");
            var endDate = int.Parse(Filter.YearMonth.ToString() + "31");
            var query = _kscHrUnitOfWork.ViewAttendItemReportRepository.GetAllQueryableWithEmployeeIds(employeeIds)
                .Where(x => x.InvalidRecord == false && x.DateKey >= sartDate && x.DateKey <= endDate).AsNoTracking();

            //filtered OverTime MAx And Average

            var RollCallOverTimeLimitAndAverage = _kscHrUnitOfWork.IncludedRollCallRepository.GetAllAsNoTrackingByIncludedDefinition()
                .Where(a => a.IncludedDefinitionId == EnumIncludedDefinition.AverageOverTime.Id || a.IncludedDefinitionId == EnumIncludedDefinition.MaximunOverTime.Id).ToList();
            var AllRollCallDefinition = RollCallOverTimeLimitAndAverage.Select(a => a.RollCallDefinitionId).ToList();

            var RollCallOverTimeLimit = RollCallOverTimeLimitAndAverage.Where(a => a.IncludedDefinitionId == EnumIncludedDefinition.MaximunOverTime.Id).Select(a => a.RollCallDefinitionId).ToList();

            var RollCallOverTimeAverage = RollCallOverTimeLimitAndAverage.Where(a => a.IncludedDefinitionId == EnumIncludedDefinition.AverageOverTime.Id).Select(a => a.RollCallDefinitionId).ToList();

            //query = query.Where(a => AllRollCallDefinition.Contains(a.RollCallDefinitionId));


            var EmployeeTimeSetting = _kscHrUnitOfWork.SystemControlDateRepository.GetActiveData();
            var isActiveMonth = EmployeeTimeSetting.AttendAbsenceItemDate == Filter.YearMonth;



            var QueryGrouped = (from item in query
                                group item by new { item.RollCallDefinitionId, item.EmployeeId, item.EmployeeNumber, item.Name, item.Family, item.Code, item.Title } into final

                                select new
                                {
                                    final.Key.EmployeeId,
                                    list = final.Select(a => a.TimeDurationInMinute).ToList(),
                                    TeamName = final.Key.Title,
                                    TeamCode = final.Key.Code,
                                    FullName = final.Key.Name + " " + final.Key.Family,
                                    EmployeeNumber = final.Key.EmployeeNumber,
                                    RollCallDefinition = final.Key.RollCallDefinitionId
                                }).ToList();

            var bindtomodel = QueryGrouped.GroupBy(x => x.EmployeeNumber).Select(x => new EmployeeTimeSheetGridManageModel()
            {
                FullName = x.First().FullName,
                EmployeeId = x.First().EmployeeId,
                EmployeeNumber = x.Key,
                TeamCode = x.First().TeamCode + "-" + x.First().TeamName,
                SumOverTimeLimit = x.Where(c => RollCallOverTimeLimit.Contains(c.RollCallDefinition)).Select(c => c.list.Sum()).Sum().Value,
                SumOverTimeAverage = x.Where(c => RollCallOverTimeAverage.Contains(c.RollCallDefinition)).Select(c => c.list.Sum()).Sum().Value,
                SumOverTime = x.Where(c => AllRollCallDefinition.Contains(c.RollCallDefinition)).Select(c => c.list.Sum()).Sum().Value,
                AverageBalanceOverTime = 0,
                AverageBalanceOverTimeDuration = "000:00",
                IsActiveMonth = isActiveMonth,
                YearMonth = Filter.YearMonth.ToString(),
                CurrentUser = Filter.CurrentUserName,
                SumForeOverTime = 0
            }).ToList();
            var employeeIdsFindedList = bindtomodel.Select(x => x.EmployeeId).ToList();
            var MonthTimeSheetDrafts = _kscHrUnitOfWork.MonthTimeSheetDraftRepository.GetMonthTimeSheetDraftByEmployeeIdYearMonth_Team(employeeIdsFindedList, Filter.YearMonth).ToList();
            var employeeSheet = _kscHrUnitOfWork.EmployeeTimeSheetRepository.GetEmployeeTimeSheetByYearMonthAndEmployeeId(EmployeeTimeSetting.AttendAbsenceItemDate, employeeIdsFindedList).ToList();

            foreach (var item in bindtomodel)
            {
                var sumForceOverTimeEmployee = MonthTimeSheetDrafts.Where(a => a.EmployeeId == item.EmployeeId).Sum(a => a.ForcedOverTime);
                item.SumOverTimeLimit = item.SumOverTimeLimit + sumForceOverTimeEmployee;
                item.SumOverTime = item.SumOverTime + sumForceOverTimeEmployee;
                var find = employeeSheet.FirstOrDefault(x => x.EmployeeId == item.EmployeeId);
                if (find != null)
                {
                    item.AverageBalanceOverTimeDuration = find.AverageBalanceOverTimeDuration;
                    item.AverageBalanceOverTime = find.AverageBalanceOverTime;
                    item.Id = find.Id;

                }
            }


            var resultFinalList = _FilterHandler.GetFilterResult<EmployeeTimeSheetGridManageModel>(bindtomodel, Filter, "SumOverTime");

            return new FilterResult<EmployeeTimeSheetGridManageModel>()
            {
                Data = resultFinalList.Data.ToList(),
                Total = resultFinalList.Total

            };

        }

        public KscResult AddOrUpdateEmployeetimeSheet(List<EmployeeTimeSheetMonthModel> models, string currentUser)
        {
            var result = new KscResult();
            var Ids = models.Where(a => a.Id > 0).Select(a => a.Id).ToList();
            var employeeTImeSheet = _kscHrUnitOfWork.EmployeeTimeSheetRepository.WhereQueryable(a => Ids.Contains(a.Id)).ToList();
            var item = models.First();
            var workCalendar = _kscHrUnitOfWork.WorkCalendarRepository.FirstOrDefault(c => c.YearMonthV1 == item.YearMonth);
            var systemStatus = _kscHrUnitOfWork.WorkCalendarRepository.GetDailyTimeSheetStatus(workCalendar.Id).GetAwaiter().GetResult();
            if (systemStatus == EnumSystemSequenceStatusDailyTimeSheet.InActiveForAllUser.Id
                || systemStatus == EnumSystemSequenceStatusDailyTimeSheet.InActiveSystem.Id)
            {
                result.AddError("خطا", "کارکرد بسته شده است");
                return result;
            }

            foreach (var model in models)
            {
                if (model.Id > 0)
                {
                    result = UpdateEmployeetimeSheet(model, employeeTImeSheet);

                }
                else
                {
                    result = AddEmployeetimeSheet(model);
                }

                if (!result.Success)
                {
                    return result;
                }
            }
            var status = _kscHrUnitOfWork.Save();
            if (status > 0)
            {
                return result;
            }
            result.AddError("", "عملیات نا موفق بود");
            return result;
        }

        private KscResult AddEmployeetimeSheet(EmployeeTimeSheetMonthModel model)
        {
            var result = new KscResult();
            try
            {
                if (_kscHrUnitOfWork.EmployeeTimeSheetRepository.Any(a => a.EmployeeId == model.EmployeeId && model.YearMonth == a.YearMonth))
                {

                    return result;
                }
                if (model.AverageBalanceOverTime == 0) return result;
                var item = new EmployeeTimeSheet()
                {
                    EmployeeId = Convert.ToInt32(model.EmployeeId),
                    YearMonth = model.YearMonth,
                    AverageBalanceOverTime = model.AverageBalanceOverTime,
                    AverageBalanceOverTimeDuration = model.AverageBalanceOverTimeDuration,
                    CeilingOvertime = 0,
                    ExcessOverTime = 0,
                    ExcessOverTimeDuration = "000:00",
                    InsertDate = DateTime.Now,
                    InsertUser = model.CurrentUser
                };
                _kscHrUnitOfWork.EmployeeTimeSheetRepository.Add(item);
                return result;
            }
            catch
            {
                result.AddError("", "عملیات نا موفق بود");
                return result;
            }


        }

        private KscResult UpdateEmployeetimeSheet(EmployeeTimeSheetMonthModel model, List<EmployeeTimeSheet> entity)
        {
            var result = new KscResult();
            try
            {
                if (_kscHrUnitOfWork.EmployeeTimeSheetRepository.Any(a => a.Id != model.Id.Value && a.EmployeeId == model.EmployeeId && model.YearMonth == a.YearMonth))
                {

                    return result;
                }
                var findEntity = entity.FirstOrDefault(a => a.Id == model.Id.Value);
                if (findEntity == null)
                {
                    result.AddError("", "اطلاعات حذف شده است ");
                    return result;
                }

                if (model.AverageBalanceOverTime == 0)
                {
                    _kscHrUnitOfWork.EmployeeTimeSheetRepository.Delete(findEntity);
                    return result;
                }

                findEntity.AverageBalanceOverTime = model.AverageBalanceOverTime;
                findEntity.AverageBalanceOverTimeDuration = model.AverageBalanceOverTimeDuration;
                findEntity.UpdateDate = DateTime.Now;
                findEntity.UpdateUser = model.CurrentUser;
                _kscHrUnitOfWork.EmployeeTimeSheetRepository.Update(findEntity);
                return result;
            }
            catch
            {
                result.AddError("", "عملیات نا موفق بود");
                return result;
            }

        }


        public async Task<KscResult> AddTrainingOverTime(UpdateStatusByYearMonthProcedureModel model)
        {
            var result = new KscResult();
            var today = DateTime.Now;
            List<EmployeeTimeSheet> employeeTimeSheetList = new List<EmployeeTimeSheet>();

            var YearMonth_int = int.Parse(model.Yearmonth);

            if (_kscHrUnitOfWork.MonthTimeSheetRepository.Any(x => x.YearMonth == YearMonth_int && x.IsCreatedManual == false))
            {
                result.AddError(Validations.RepetitiveId, "تایم شیت ماه انتخابی ایجاد شده است");
                return result;
                //throw new HRBusinessException(Validations.RepetitiveId, "تایم شیت ماه انتخابی ایجاد شده است");
            }

            var startAndEndMiladi = _kscHrUnitOfWork.WorkCalendarRepository.GetStartDateAndEndDateWithYearMonth(YearMonth_int);

            //ارتباط با سیستم آموزش
            var trainingData = GetIndustrialMedicineClassMembersByAttendingDate(start: startAndEndMiladi.Item1,
                                                                                end: startAndEndMiladi.Item2);
            if (trainingData.Count > 0)
            {

                //واکشی دیتا موجود
                var employeeTimeSheetByYearMonth = _kscHrUnitOfWork.EmployeeTimeSheetRepository.GetEmployeeTimeSheetQueryable(YearMonth_int).ToList();
                var employeeNumbers = trainingData.Select(a => a.EmployeeNumber);
                //تبدیل شماره های پرسنلی به ایدی
                var employee = _kscHrUnitOfWork.EmployeeRepository.WhereQueryable(a => employeeNumbers.Contains(a.EmployeeNumber)).ToList();
                var resultData = (from item in trainingData
                                  join empl in employee on item.EmployeeNumber equals empl.EmployeeNumber
                                  select new EmployeeTimeSheetMonthModel()
                                  {
                                      EmployeeId = empl.Id,
                                      YearMonth = item.YearMonth,
                                      TrainingOverTime = item.TimeRequired * 60, // convert to min
                                  }).ToList();


                foreach (var item in resultData)
                {
                    var employeeTimeSheet = employeeTimeSheetByYearMonth.FirstOrDefault(x => x.EmployeeId == item.EmployeeId);
                    if (employeeTimeSheet != null)
                    {

                        employeeTimeSheet.TrainingOverTime = item.TrainingOverTime;
                        employeeTimeSheet.TrainingOverTimeDuration = Utility.ConvertMinuteToDuration(item.TrainingOverTime.Value);
                        employeeTimeSheet.UpdateDate = today;
                        employeeTimeSheet.UpdateUser = model.CurrentUser;
                    }
                    else
                    {

                        var newRow = _mapper.Map<EmployeeTimeSheet>(item);
                        newRow.TrainingOverTimeDuration = Utility.ConvertMinuteToDuration(item.TrainingOverTime.Value);
                        newRow.InsertDate = today;
                        newRow.InsertUser = model.CurrentUser;
                        employeeTimeSheetList.Add(newRow);
                    }

                }
                if (employeeTimeSheetList.Count() != 0)
                {
                    _kscHrUnitOfWork.EmployeeTimeSheetRepository.AddRange(employeeTimeSheetList);
                }
                try
                {
                    model.Result = "اضافه کار طب صنعتی با موفقیت انجام شد";
                    model.ResultCount = employeeTimeSheetList.Count();

                    var stepresult = await _procedureService.InsertStepProcedure(model);
                    if (stepresult.Errors != null)
                    {
                        result.AddErrors(stepresult.Errors);
                        return result;
                    }
                    await _kscHrUnitOfWork.SaveAsync(checklog: false);
                }
                catch (Exception ex)
                {
                    result.AddError("خطا", ex.Message);
                }
                return result;
            }
            //result.AddError("داده سیستم آموزش", "داده ای در سیستم آموزش موجود نمیباشد");
            try
            {
                model.Result = "داده ای در سیستم آموزش موجود نمیباشد";
                model.ResultCount = employeeTimeSheetList.Count();

                var stepresult = await _procedureService.InsertStepProcedure(model);
                if (stepresult.Errors != null)
                {
                    result.AddErrors(stepresult.Errors);
                    return result;
                }
                await _kscHrUnitOfWork.SaveAsync(checklog: false);
            }
            catch (Exception ex)
            {
                result.AddError("خطا", ex.Message);
            }
            return result;
        }

        public List<GetIndustrialMedicineClassMembersVM> GetIndustrialMedicineClassMembersByAttendingDate(DateTime start, DateTime end)
        {
            try
            {
                //var TrainingApiUrl = "http://localhost:12743";
                var TrainingApiUrl = "https://wapi.ksc.ir/Training";
                var inputModel = new SearchIndustrialMedicineClassMembersVM()
                {
                    End = end,
                    Start = start
                };
                var uri = string.Format("{0}/Api/main/GetIndustrialMedicineClassMembersByAttendingDate", TrainingApiUrl);
               var model= _client.PostServiceByModelAsync<ReturnData<List<GetIndustrialMedicineClassMembersVM>>, SearchIndustrialMedicineClassMembersVM>(uri, inputModel).Result;
               return model.Data;
               
            }
            catch (Exception)
            {


            }
            return new List<GetIndustrialMedicineClassMembersVM>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<KscResult> AddEmployeeCompensatoryOverTime(UpdateStatusByYearMonthProcedureModel model)
        {
            var result = new KscResult();
            var YearMonth = int.Parse(model.Yearmonth);


            try
            {
                if (YearMonth != 140304 && YearMonth != 140305 && YearMonth != 140306)
                {
                    throw new HRBusinessException(Validations.RepetitiveId, "این مرحله برای این ماه فعال نمی باشد");
                }
                //var MaximumOverTimeDuration = "35:00";
                //if (YearMonth == 140304)
                //{
                //    //MaximumOverTimeDuration = "17:00";
                //}
                var MaximumOverTimeDuration = "17:00";
                var systemStatus = await _kscHrUnitOfWork.WorkCalendarRepository.GetMonthTimeSheetStatus(YearMonth);

                if (!systemStatus)
                {
                    throw new HRBusinessException(Validations.RepetitiveId, "برای محاسبه باید سیستم بسته باشد");
                }

                if (_kscHrUnitOfWork.MonthTimeSheetRepository.Any(x => x.YearMonth == YearMonth && x.IsCreatedManual == false))
                {
                    throw new HRBusinessException(Validations.RepetitiveId, "تایم شیت ماه انتخابی ایجاد شده است");
                }
                var today = DateTime.Now;
                List<EmployeeTimeSheet> employeeTimeSheetList = new List<EmployeeTimeSheet>();

                var startAndEndMiladi = _kscHrUnitOfWork.WorkCalendarRepository.GetStartDateAndEndDateWithYearMonth(YearMonth);
                //
                string year = YearMonth.ToString().Substring(0, 4);
                var startYearMonth = year + "01";
                var endYearMonth = year + "03";
                var monthTimeSheet = _kscHrUnitOfWork.MonthTimeSheetRepository.GetMonthTimeSheetByRangeYearMonth(int.Parse(startYearMonth), int.Parse(endYearMonth)).AsNoTracking();
                var monthTimeSheetRollCalls = _kscHrUnitOfWork.MonthTimeSheetRollCallRepository.GetAllMonthTimeSheetRollCall().Where(x => x.IsActive).AsNoTracking();
                var rollCallDefinitionByCeilingOvertime = _kscHrUnitOfWork.IncludedRollCallRepository.GetActiveIncludedRollCallByIncludedIdAsNoTracking(EnumIncludedDefinition.MaximunOverTime.Id);
                var roozkarCode = ((int)WorkTimeCodeEnums.RoozKar);
                var employee = _kscHrUnitOfWork.EmployeeRepository.GetEmploymentPerson().Include(x => x.WorkGroup).Include(x => x.TeamWork)
                      .Where(x => x.PaymentStatusId != EnumPaymentStatus.DismiisalEmployee.Id);//&& x.WorkGroup.WorkTimeId == roozkarCode);

                var data = from rollcall in monthTimeSheetRollCalls
                           join month in monthTimeSheet on rollcall.MonthTimeSheetId equals month.Id
                           join incld in rollCallDefinitionByCeilingOvertime on rollcall.RollCallDefinitionId equals incld.RollCallDefinitionId
                           join empl in employee on month.EmployeeId equals empl.Id
                           select new
                           {
                               EmployeeId = month.EmployeeId,
                               OverTimeDefinitionId = empl.TeamWork.OverTimeDefinitionId,
                               YearMonth = month.YearMonth,
                               DurationInMinut = rollcall.DurationInMinut
                           };
                var dataGroup = data.GroupBy(x => new { EmployeeId = x.EmployeeId, OverTimeDefinitionId = x.OverTimeDefinitionId }).Select(x => new
                {
                    EmployeeId = x.Key.EmployeeId,
                    OverTimeDefinitionId = x.Key.OverTimeDefinitionId,
                    CountYearMonth = x.Select(y => y.YearMonth).Distinct().Count(),
                    SumDurationInMinut = x.Sum(y => y.DurationInMinut),
                    // Average= (x.Sum(y => y.DurationInMinut) / x.Select(y => y.YearMonth).Distinct().Count()) / 2

                }).ToList();
                //

                int maxOverTime = Utility.ConvertDurationToMinute(MaximumOverTimeDuration).Value;
                //int hourOverTimeIn35 = 2100;
                //
                var dataGroupAverage = dataGroup.Select(x => new
                {
                    EmployeeId = x.EmployeeId,
                    OverTimeDefinitionId = x.OverTimeDefinitionId,
                    Average = (x.SumDurationInMinut / x.CountYearMonth) / 2


                });
                //
                //System.IO.StreamWriter overTimePath = new System.IO.StreamWriter("d:\\\\MISTXT\\overTime.txt");
                //foreach (var item in dataGroupAverage)
                //{
                //    overTimePath.WriteLine(item.EmployeeId + "|" + item.Average);
                //}
                //overTimePath.Close();
                //

                var resultData = dataGroupAverage.Select(x => new
                {
                    EmployeeId = x.EmployeeId,
                    OverTimeDefinitionId = x.OverTimeDefinitionId,
                    YearMonth = YearMonth,
                    CompensatoryOverTime = x.Average > maxOverTime ? maxOverTime : x.Average,

                }).ToList();

                var employeeTimeSheetByYearMonth = _kscHrUnitOfWork.EmployeeTimeSheetRepository.GetEmployeeTimeSheetQueryable(YearMonth).ToList();
                var overTimeDefinition = _kscHrUnitOfWork.OverTimeDefinitionRepository.GetAllQueryable().ToList();
                foreach (var item in resultData)
                {
                    var employeeTimeSheet = employeeTimeSheetByYearMonth.SingleOrDefault(x => x.EmployeeId == item.EmployeeId);
                    if (employeeTimeSheet != null)
                    {
                        if (employeeTimeSheet.ExcessOverTime == 0)
                        {
                            //
                            var maximumDurationMinuteTeam = overTimeDefinition.FirstOrDefault(x => x.Id == item.OverTimeDefinitionId).MaximumDurationMinute;
                            if (item.CompensatoryOverTime + employeeTimeSheet.CeilingOvertime > maximumDurationMinuteTeam)
                            {
                                employeeTimeSheet.CompensatoryOverTime = maximumDurationMinuteTeam - employeeTimeSheet.CeilingOvertime;
                            }
                            else
                            {
                                employeeTimeSheet.CompensatoryOverTime = item.CompensatoryOverTime;
                            }
                            employeeTimeSheet.UpdateDate = today;
                            employeeTimeSheet.UpdateUser = model.CurrentUser;
                        }
                    }
                    else
                    {
                        EmployeeTimeSheet newRow = new EmployeeTimeSheet();
                        newRow.CompensatoryOverTime = item.CompensatoryOverTime;
                        newRow.EmployeeId = item.EmployeeId;
                        newRow.YearMonth = item.YearMonth;
                        newRow.InsertDate = today;
                        newRow.InsertUser = model.CurrentUser;
                        newRow.ExcessOverTimeDuration = "000:00";
                        employeeTimeSheetList.Add(newRow);
                    }

                }
                if (employeeTimeSheetList.Count() != 0)
                {
                    await _kscHrUnitOfWork.EmployeeTimeSheetRepository.AddRangeAsync(employeeTimeSheetList);
                }
                model.Result = "محاسبه میانگین سه ماهه اول سال با موفقیت انجام شد";
                model.ResultCount = employeeTimeSheetList.Count();
                result = await _procedureService.InsertStepProcedure(model);
                try
                {
                    if (result.Success)
                        await _kscHrUnitOfWork.SaveAsync(checklog: false);
                }
                catch (Exception ex)
                {
                    result.AddError("خطا", ex.Message);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return result;
        }



    }
}
