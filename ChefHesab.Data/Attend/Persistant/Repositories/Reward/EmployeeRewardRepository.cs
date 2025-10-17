using Ksc.Hr.Domain.Entities;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Chart;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Entities.Reward;
using Ksc.HR.Domain.Entities.Rule;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories.Reward;
using Ksc.HR.DTO.ODSViews.ViewMisJobPosition;
using Ksc.HR.DTO.WorkShift.WorkTime;
using Ksc.HR.Share.General;
using Ksc.HR.Share.Model.EmployeeReward;
using Ksc.HR.Share.Model.Interdict;
using Ksc.HR.Share.Model.JobPositionStatus;
using Ksc.HR.Share.Model.Reward;
using Ksc.HR.Share.Model.RollCallDefinication;
using Ksc.HR.Share.Model.SystemSequenceStatus;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Index.HPRtree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.Reward
{
    public class EmployeeRewardRepository : EfRepository<EmployeeReward, long>, IEmployeeRewardRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeeRewardRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<EmployeeReward> GetEmployeeRewards(int yearmonth, int rewardCategoryId)
        {
            var query = _kscHrContext.EmployeeRewards
                .Where(x => x.YearMonth == yearmonth && x.RewardCategoryId == rewardCategoryId)
                .AsNoTracking();
            return query;
        }
        //----------------تغییر کد بعد از اعمال ضرایب
        public List<EmployeeRewardDto> GetEmployeesWithCalculateRewards11(EmployeeRewardInputModel model)
        {

            try
            {


                if (!model.MonthTimeSheetYearMonth.HasValue)
                {
                    if (!_kscHrContext.WorkCalendars.Any(a => a.YearMonthV1 == model.YearMonth && a.SystemSequenceStatusId.Value == EnumSystemSequenceStatusDailyTimeSheet.TimeSheetIsCreated.Id))
                    {
                        model.MonthTimeSheetYearMonth = DateTimeExtensions.GetPrevMonth(model.YearMonth);
                    }
                    else
                    {
                        model.MonthTimeSheetYearMonth = model.YearMonth;
                    }
                }
                var rewardOutHeaderQuery = _kscHrContext.RewardOutHeaders
                     .Where(a => a.YearMonth == model.YearMonth && a.RewardCategoryId == model.RewardCategoryId)
                     .Include(a => a.RewardOutDetails).FirstOrDefault();
                List<int> paymentStatusIds = new List<int>(4)
                      {1,5,7,8};
                var employeeQuery = _kscHrContext.Employees
                    .Include(a => a.Chart_JobGroup)
                    .ThenInclude(a => a.Chart_CategoryCoefficient)

                   .AsQueryable().AsNoTracking()
                   //.Where(a=>a.EmployeeNumber== "78751")//for test
                   .Where(a => !paymentStatusIds
                   .Contains(a.PaymentStatusId ?? 0) && a.TeamWorkId.HasValue && a.JobPositionId.HasValue)
                   .Select(a => new
                   {
                       a.Id,
                       #region
                       //تغییرات بعد اضافه شدن ضریب  --اگر شخصی در پرسنلی CategoryCoefficientId را داشته باشد اولویت این مقدار است
                       //ضریب
                       //JobGroupScore = a.JobGroupId.HasValue ? a.Chart_JobGroup.Chart_CategoryCoefficient.Score : null,
                       JobGroupScore = a.CategoryCoefficientId.HasValue ? a.Chart_CategoryCoefficient.Score : (a.JobGroupId.HasValue ? a.Chart_JobGroup.Chart_CategoryCoefficient.Score : null),

                       #endregion

                       //.................این دو فیلد قبلا از employee گرفته میشد از طریق فایل ماهانه پر میشد
                       //از این قسمت حذف میشود
                       //a.ProductionEfficiencyId,

                       //JobGroupId = a.JobGroupId,
                       a.EmployeeNumber,
                       FullName = a.Name + " " + a.Family,
                       TeamWorkTitle = a.TeamWork.Title,
                       TeamWorkCode = a.TeamWork.Code,
                       WorkGroupCode = a.WorkGroup.Code,
                       a.JobPositionId
                       // a.EmployeeJobPositions

                   }).ToList();

                var jobpostionsID = employeeQuery.Select(a => a.JobPositionId).ToList();
                var JobPositionList = _kscHrContext.Chart_JobPosition.AsQueryable().AsNoTracking()
                    .Where(a => jobpostionsID.Contains(a.Id))
                    .Include(a => a.ProductionEfficiency)
                    .Include(a => a.Chart_RewardSpecific)
                    .Include(a => a.CategoryCoefficient)
                    .Include(a => a.Chart_JobPositionIncreasePercents)
                    .Select(
                    a => new JobPositionForEmployeeRewardModel
                    {
                        Id = a.Id,
                        //.................این دو فیلد قبلا از employee گرفته میشد از طریق فایل ماهانه پر میشد
                      
                        ProductionEfficiencyId = a.ProductionEfficiency.Id,
                        CategoryCoefficientId = a.CategoryCoefficient.Id,

                        //..
                        RewardSpecificEfficincy = a.RewardSpecificEfficincy,
                        RewardUnitTypeId = a.SpecificRewardId.HasValue ? a.Chart_RewardSpecific.RewardUnitTypeId : null,
                        SpecificRewardId = a.SpecificRewardId,
                        JobPoisitionStatusId = a.JobPoisitionStatusId,
                        CPercent = a.ProductionEfficiency.CPercent,
                        //CategoryCoefficientId = a.CategoryCoefficientId,
                        CofficientProximityProduct = a.CofficientProximityProduct,
                        Title = a.Title,
                        CostCenter = a.CostCenter,
                        MisJobPositionCode = a.MisJobPositionCode,
                        Chart_JobPositionIncreasePercents = a.Chart_JobPositionIncreasePercents.Where(x => x.IsActive)
                    }
                    )

                    .ToList();
                var employeesID = employeeQuery.Select(a => a.Id).ToList();
                /// پاداش ویژه
                //if (model.RewardCategoryId == EnumRewardCategory.SpecialReward.Id)
                //{
                //    JobPositionList = JobPositionList.Where(a => a.JobPoisitionStatusId == EnumJobPositionStatus.Moavenat.Id || a.JobPoisitionStatusId == EnumJobPositionStatus.ModirAmel.Id).ToList();
                //    var JobPositionID = JobPositionList.Select(a => a.Id).ToList();
                //    employeeQuery = employeeQuery.Where(a => JobPositionID.Any(i => i == a.JobPositionId)).ToList();
                //}
                var EfficiencyHistoryList = _kscHrContext.EmployeeEfficiencyHistory.Where(a => a.YearMonth == model.MonthTimeSheetYearMonth && employeesID.Contains(a.EmployeeId)).OrderByDescending(x => x.InsertDate).ToList();
                var EmployeeSafetyDeductionsList = _kscHrContext.EmployeeSafetyDeductions.Where(a => a.YearMonth == model.MonthTimeSheetYearMonth && (a.ActionId == 1 || a.ActionId == 2) && employeesID.Contains(a.EmployeeId.Value)).OrderByDescending(x => x.Id).ToList();
                //روزهای کاری
                var countDaysWork = _kscHrContext.WorkCalendars.Count(a => a.YearMonthV1 == model.MonthTimeSheetYearMonth);
                var monthtimesheetsList = _kscHrContext.MonthTimeSheets
               .Where(a => a.YearMonth == model.MonthTimeSheetYearMonth && employeesID.Contains(a.EmployeeId))
               .Include(a => a.MonthTimeSheetIncludeds)
               .Include(a => a.MonthTimeSheetRollCalls)
               .Select(x => new MonthTimesheetModel
               {
                   EmployeeId = x.EmployeeId,
                   MonthTimeSheetIncludeds = x.MonthTimeSheetIncludeds.ToList(),
                   //x.MonthTimeSheetRollCalls
               }).ToList();
                var list = new List<EmployeeRewardDto>();
                //if (rewardOutHeaderQuery != null)
                foreach (var item in employeeQuery)
                {
                    //if (item.EmployeeNumber == "118966")
                    //{

                    //}
                    var currentJobPosition = JobPositionList.FirstOrDefault(a => a.Id == item.JobPositionId);
                    var currentmonthtimesheet = monthtimesheetsList.FirstOrDefault(a => a.EmployeeId == item.Id);
                    ///مشمول ایام بیمه- پاداش
                    var dailyKarkard = currentmonthtimesheet?.MonthTimeSheetIncludeds.FirstOrDefault(a => a.IncludedDefinitionId == 27 && a.IsActive)?.DurationInDay ?? 0;
                    var durationInHourDeductionRewardmodel = currentmonthtimesheet?.MonthTimeSheetIncludeds.FirstOrDefault(a => a.IncludedDefinitionId == 4 && a.IsActive);
                    //مشمول کسر پاداش روزانه
                    var dailyDeductionReward = currentmonthtimesheet?.MonthTimeSheetIncludeds.FirstOrDefault(a => a.IncludedDefinitionId == 5 && a.IsActive)?.DurationInDay ?? 0;

                    //مشمول کسر پاداش ساعتی
                    var durationInHour = durationInHourDeductionRewardmodel?.DurationInHour ?? 0;
                    var duration = durationInHour != null ? Utility.ConvertDurationInHour(durationInHour).Split(':') : null;
                    int deductionhours = duration != null ? Convert.ToInt32(duration[0]) : 0;
                    int deductionminutes = duration != null ? Convert.ToInt32(duration[1]) : 0;
                    var input = new EmployeeRewardCalcInput();

                    input.EmployeeId = item.Id;
                    input.currentmonthtimesheet = currentmonthtimesheet;
                    input.currentJobPosition = currentJobPosition;
                    input.dailyKarkard = dailyKarkard;
                    input.durationInHourDeductionRewardmodel = durationInHourDeductionRewardmodel;
                    input.dailyDeductionReward = dailyDeductionReward;
                    input.deductionhours = deductionhours;
                    input.ductionInMinutes = durationInHour;
                    input.deductionminutes = deductionminutes;
                    input.rewardOutHeaderQuery = rewardOutHeaderQuery;
                    input.EfficiencyHistoryList = EfficiencyHistoryList;
                    input.increasePercentData = currentJobPosition.Chart_JobPositionIncreasePercents.Where(a => a.IsActive && a.EndDate.HasValue == false).OrderByDescending(a => a.StartDate).FirstOrDefault();
                    input.EmployeeSafetyDeductionsList = EmployeeSafetyDeductionsList;
                    //.................این دو فیلد قبلا از employee گرفته میشد از طریق فایل ماهانه پر میشد
                    input.ProductionEfficiencyId = currentJobPosition.ProductionEfficiencyId;
                    input.JobGroupId = currentJobPosition.CategoryCoefficientId;
                    //.......................

                    input.RewardUnitTypeId = currentJobPosition.RewardUnitTypeId;
                    input.MaxPaymentAmountKpi = rewardOutHeaderQuery?.RewardOutDetails?.Max(a => a.PaymentAmountKpi);
                    input.JobGroupScore = item.JobGroupScore;
                    input.WorkGroupCode = item.WorkGroupCode;
                    input.countDaysWork = countDaysWork;
                    input.EmployeeNumber = item.EmployeeNumber;
                    input.FullName = item.FullName;
                    input.TeamWorkTitle = item.TeamWorkCode + "-" + item.TeamWorkTitle;

                    EmployeeRewardDto calc;
                    if (model.RewardCategoryId == EnumRewardCategory.SpecialReward.Id)
                    {
                        calc = SpecificEmployeeRewardCalculate(input);
                    }
                    else
                    {
                        calc = EmployeeRewardCalculate(input);
                    }
                    list.Add(calc);

                }
                return list;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public List<EmployeeRewardDto> GetEmployeesWithCalculateRewards(EmployeeRewardInputModel model)
        {

            try
            {


                if (!model.MonthTimeSheetYearMonth.HasValue)
                {
                    if (!_kscHrContext.WorkCalendars.Any(a => a.YearMonthV1 == model.YearMonth && a.SystemSequenceStatusId.Value == EnumSystemSequenceStatusDailyTimeSheet.TimeSheetIsCreated.Id))
                    {
                        model.MonthTimeSheetYearMonth = DateTimeExtensions.GetPrevMonth(model.YearMonth);
                    }
                    else
                    {
                        model.MonthTimeSheetYearMonth = model.YearMonth;
                    }
                }
                var rewardOutHeaderQuery = _kscHrContext.RewardOutHeaders
                     .Where(a => a.YearMonth == model.YearMonth && a.RewardCategoryId == model.RewardCategoryId)
                     .Include(a => a.RewardOutDetails).FirstOrDefault();
                List<int> paymentStatusIds = new List<int>(4)
                      {1,5,7,8};
                var employeeQuery = _kscHrContext.Employees.
                    Include(a => a.Chart_JobGroup).
                    ThenInclude(a => a.Chart_CategoryCoefficient)
                   .AsQueryable().AsNoTracking()
                   .Where(a => !paymentStatusIds.Contains(a.PaymentStatusId ?? 0) && a.TeamWorkId.HasValue && a.JobPositionId.HasValue)
                   .Select(a => new
                   {
                       a.Id,
                       JobGroupScore = a.JobGroupId.HasValue ? a.Chart_JobGroup.Chart_CategoryCoefficient.Score : null,
                       JobGroupId = a.JobGroupId,
                       a.EmployeeNumber,
                       a.ProductionEfficiencyId,
                       FullName = a.Name + " " + a.Family,
                       TeamWorkTitle = a.TeamWork.Title,
                       TeamWorkCode = a.TeamWork.Code,
                       WorkGroupCode = a.WorkGroup.Code,
                       a.JobPositionId
                       // a.EmployeeJobPositions

                   }).ToList();

                var jobpostionsID = employeeQuery.Select(a => a.JobPositionId).ToList();
                var JobPositionList = _kscHrContext.Chart_JobPosition.AsQueryable().AsNoTracking()
                    .Where(a => jobpostionsID.Contains(a.Id))
                    .Include(a => a.ProductionEfficiency)
                    .Include(a => a.Chart_RewardSpecific)
                    .Include(a => a.CategoryCoefficient)
                    .Include(a => a.Chart_JobPositionIncreasePercents)
                    .Select(
                    a => new JobPositionForEmployeeRewardModel
                    {
                        Id = a.Id,
                        //a.ProductionEfficiencyId,
                        RewardSpecificEfficincy = a.RewardSpecificEfficincy,
                        RewardUnitTypeId = a.SpecificRewardId.HasValue ? a.Chart_RewardSpecific.RewardUnitTypeId : null,
                        SpecificRewardId = a.SpecificRewardId,
                        JobPoisitionStatusId = a.JobPoisitionStatusId,
                        CPercent = a.ProductionEfficiency.CPercent,
                        CategoryCoefficientId = a.CategoryCoefficientId,
                        CofficientProximityProduct = a.CofficientProximityProduct,
                        Title = a.Title,
                        CostCenter = a.CostCenter,
                        MisJobPositionCode = a.MisJobPositionCode,
                        Chart_JobPositionIncreasePercents = a.Chart_JobPositionIncreasePercents.Where(x => x.IsActive)
                    }
                    )

                    .ToList();
                var employeesID = employeeQuery.Select(a => a.Id).ToList();
                /// پاداش ویژه
                //if (model.RewardCategoryId == EnumRewardCategory.SpecialReward.Id)
                //{
                //    JobPositionList = JobPositionList.Where(a => a.JobPoisitionStatusId == EnumJobPositionStatus.Moavenat.Id || a.JobPoisitionStatusId == EnumJobPositionStatus.ModirAmel.Id).ToList();
                //    var JobPositionID = JobPositionList.Select(a => a.Id).ToList();
                //    employeeQuery = employeeQuery.Where(a => JobPositionID.Any(i => i == a.JobPositionId)).ToList();
                //}
                var EfficiencyHistoryList = _kscHrContext.EmployeeEfficiencyHistory.Where(a => a.YearMonth == model.MonthTimeSheetYearMonth && employeesID.Contains(a.EmployeeId)).OrderByDescending(x => x.InsertDate).ToList();
                var EmployeeSafetyDeductionsList = _kscHrContext.EmployeeSafetyDeductions.Where(a => a.YearMonth == model.MonthTimeSheetYearMonth && (a.ActionId == 1 || a.ActionId == 2) && employeesID.Contains(a.EmployeeId.Value)).OrderByDescending(x => x.Id).ToList();
                //روزهای کاری
                var countDaysWork = _kscHrContext.WorkCalendars.Count(a => a.YearMonthV1 == model.MonthTimeSheetYearMonth);
                var monthtimesheetsList = _kscHrContext.MonthTimeSheets
               .Where(a => a.YearMonth == model.MonthTimeSheetYearMonth && employeesID.Contains(a.EmployeeId))
               .Include(a => a.MonthTimeSheetIncludeds)
               .Include(a => a.MonthTimeSheetRollCalls)
               .Select(x => new MonthTimesheetModel
               {
                   EmployeeId = x.EmployeeId,
                   MonthTimeSheetIncludeds = x.MonthTimeSheetIncludeds.ToList(),
                   //x.MonthTimeSheetRollCalls
               }).ToList();
                var list = new List<EmployeeRewardDto>();
                //if (rewardOutHeaderQuery != null)
                foreach (var item in employeeQuery)
                {
                    //if (item.EmployeeNumber == "118966")
                    //{

                    //}
                    var currentJobPosition = JobPositionList.FirstOrDefault(a => a.Id == item.JobPositionId);
                    var currentmonthtimesheet = monthtimesheetsList.FirstOrDefault(a => a.EmployeeId == item.Id);
                    ///مشمول ایام بیمه- پاداش
                    var dailyKarkard = currentmonthtimesheet?.MonthTimeSheetIncludeds.FirstOrDefault(a => a.IncludedDefinitionId == 27 && a.IsActive)?.DurationInDay ?? 0;
                    var durationInHourDeductionRewardmodel = currentmonthtimesheet?.MonthTimeSheetIncludeds.FirstOrDefault(a => a.IncludedDefinitionId == 4 && a.IsActive);
                    //مشمول کسر پاداش روزانه
                    var dailyDeductionReward = currentmonthtimesheet?.MonthTimeSheetIncludeds.FirstOrDefault(a => a.IncludedDefinitionId == 5 && a.IsActive)?.DurationInDay ?? 0;

                    //مشمول کسر پاداش ساعتی
                    var durationInHour = durationInHourDeductionRewardmodel?.DurationInHour ?? 0;
                    var duration = durationInHour != null ? Utility.ConvertDurationInHour(durationInHour).Split(':') : null;
                    int deductionhours = duration != null ? Convert.ToInt32(duration[0]) : 0;
                    int deductionminutes = duration != null ? Convert.ToInt32(duration[1]) : 0;
                    var input = new EmployeeRewardCalcInput();

                    input.EmployeeId = item.Id;
                    input.currentmonthtimesheet = currentmonthtimesheet;
                    input.currentJobPosition = currentJobPosition;
                    input.dailyKarkard = dailyKarkard;
                    input.durationInHourDeductionRewardmodel = durationInHourDeductionRewardmodel;
                    input.dailyDeductionReward = dailyDeductionReward;
                    input.deductionhours = deductionhours;
                    input.ductionInMinutes = durationInHour;
                    input.deductionminutes = deductionminutes;
                    input.rewardOutHeaderQuery = rewardOutHeaderQuery;
                    input.EfficiencyHistoryList = EfficiencyHistoryList;
                    input.increasePercentData = currentJobPosition.Chart_JobPositionIncreasePercents.Where(a => a.IsActive && a.EndDate.HasValue == false).OrderByDescending(a => a.StartDate).FirstOrDefault();
                    input.EmployeeSafetyDeductionsList = EmployeeSafetyDeductionsList;
                    input.ProductionEfficiencyId = item.ProductionEfficiencyId;
                    input.RewardUnitTypeId = currentJobPosition.RewardUnitTypeId;
                    input.MaxPaymentAmountKpi = rewardOutHeaderQuery?.RewardOutDetails?.Max(a => a.PaymentAmountKpi);
                    input.JobGroupScore = item.JobGroupScore;
                    input.JobGroupId = item.JobGroupId;
                    input.WorkGroupCode = item.WorkGroupCode;
                    input.countDaysWork = countDaysWork;
                    input.EmployeeNumber = item.EmployeeNumber;
                    input.FullName = item.FullName;
                    input.TeamWorkTitle = item.TeamWorkCode + "-" + item.TeamWorkTitle;

                    EmployeeRewardDto calc;
                    if (model.RewardCategoryId == EnumRewardCategory.SpecialReward.Id)
                    {
                        calc = SpecificEmployeeRewardCalculate(input);
                    }
                    else
                    {
                        calc = EmployeeRewardCalculate(input);
                    }
                    list.Add(calc);

                }
                return list;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        /// <summary>
        /// پاداش پرسنل
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private EmployeeRewardDto EmployeeRewardCalculate(EmployeeRewardCalcInput input)
        {
            //if(input.ProductionEfficiencyId==1)
            //{

            //}
            try
            {


                double balanceRate = 1;
                string costCenter = input.currentJobPosition.CostCenter.ToString();
                if (costCenter == "110505")
                {
                    balanceRate = 1.05;
                }
                //مبلغ پاداش
                var rewardOutDetail = input.rewardOutHeaderQuery?.RewardOutDetails?.FirstOrDefault(a => a.ProductionEfficiencyId == input.ProductionEfficiencyId);
                var M = rewardOutDetail?.FinalPaymentAmount ?? 0;
                //ضریب کارایی
                var K = input.EfficiencyHistoryList.FirstOrDefault(a => a.EmployeeId == input.EmployeeId)?.Efficiency ?? 0;
                //ضریب نزدیکی پست به تولید
                var A = (input.currentJobPosition.CofficientProximityProduct ?? 0) / 100;
                //ضریب گروه شغلی
                double G = Convert.ToDouble(input.JobGroupScore ?? 0);

                //ضریب سنوات افزوده
                var increasePercentData = input.increasePercentData;
                double coefficientincreasePercent = 0;
                if (input.WorkGroupCode == "R")
                    coefficientincreasePercent = increasePercentData?.CoefficientYearsDay ?? 0;
                else
                    coefficientincreasePercent = increasePercentData?.CoefficientYearsShift ?? 0;
                ///ضریب تبدیل شده H
                var H = GetIncreasedPercentConverted(coefficientincreasePercent);
                // پاداش تولید پرسنل بر اساس کارکرد ماهانه و ضریب های موثر در پاداش محاسبFirstOrDefaultه می شود
                var P = M * (A * G * H) * (double)K * balanceRate;

                //تعداد روز کسر از پاداش از تعداد روز مشمول پاداش کم می شود:
                //تعداد روز مورد محاسبه = Day - Deduction - Total - Day
                //var calculatedDay = input.dailyKarkard - input.dailyDeductionReward;
                // پاداش   محاسبه شده = (مبلغ پاداش P/ روزهای ماه بر اساس تقویم )  *تعداد روز مبنا پاداش ماه
                //var P1 = (P / input.countDaysWork) * calculatedDay;
                var countdaydeduction = input.countDaysWork - input.dailyKarkard;
                double P1 = P;
                double MD4 = 0;
                if (countdaydeduction > 0)
                {
                    //P1 = P;
                    MD4 = (P / input.countDaysWork) * countdaydeduction;
                }
                else P1 = (P / input.countDaysWork) * input.dailyKarkard;
                //                کسر از پاداش ساعتی : 
                //   ((مقدار ساعت * (پاداش محاسبه شده / 270 ) ) MD1 =
                var MD1 = (input.deductionhours * (P1 / 270));
                //((60 / مقدار دقیقه * (پاداش محاسبه شده / 270 ) )   MD2 =
                var MD2 = ((input.deductionminutes * (P1 / 270)) / 60);
                var MD3 = input.dailyDeductionReward * (P / input.countDaysWork);
                //جمع کسر از پاداش MD = MD1 + MD2
                var MD = MD1 + MD2 + MD3 + MD4;
                //پاداش نهایی جهت پرداخت: P2
                var P2 = P1 - MD;
                //ضریب عملکرد ایمنی
                var safetyDeduction = input.EmployeeSafetyDeductionsList.FirstOrDefault(a => a.EmployeeId == input.EmployeeId);

                //var xx = safetyDeduction.SumPercent;
                //double cofficientDeductionPrice=0;

                //if (safetyDeduction != null)
                //{
                //مبلغ  کسر بابت رفتار غیر ایمن
                // cofficientDeductionPrice = Convert.ToInt64(P2 * (safetyDeduction?.SumPercent ?? 0) / 100);
                //}

                var cofficientDeductionPrice = Convert.ToInt64(P2 * (safetyDeduction?.SumPercent ?? 0) / 100);



                var errorDescription = "";
                if (input.dailyKarkard == 0)
                {
                    errorDescription = "کارکرد فرد صفر است";
                }
                else if (input.currentmonthtimesheet == null)
                {
                    errorDescription = "کارکرد فرد وجود ندارد ";
                }
                else if (K == 0)
                {
                    errorDescription = "ضریب کارائی فرد صفر است";
                }

                var data = new EmployeeRewardDto()
                {
                    EmployeeId = input.EmployeeId,
                    EmployeeNumber = input.EmployeeNumber,
                    FullName = input.FullName,
                    TeamWorkTitle = input.TeamWorkTitle,
                    TPercent = rewardOutDetail?.TPercent,
                    JobPositionId = input.currentJobPosition.Id,
                    JobPositionTitle = input.currentJobPosition.MisJobPositionCode + "-" + input.currentJobPosition.Title,
                    CoefficientJobCategory = G,//
                    CostCenterCode = input.currentJobPosition.CostCenter.ToString(),
                    MisJobPositionCode = input.currentJobPosition.MisJobPositionCode,
                    ProductionEfficiencyId = input.ProductionEfficiencyId,
                    CofficientProximityProduct = (input.currentJobPosition.CofficientProximityProduct ?? 0),
                    IncreasedPercent = coefficientincreasePercent,
                    IncreasedPercentConverted = H,
                    PaymentAmount = M,
                    EfficiencyPerson = K,
                    InsuranceDurationInDay = input.dailyKarkard,
                    DailyDeductionReward = input.dailyDeductionReward,//
                    DeductionRewardPrice = (long)MD,
                    CalculatedRewardPrice = (long)P1,
                    PaymentRewardPrice = (long)P2,
                    HourlyDeductionReward = input.deductionminutes,
                    ErrorDescription = errorDescription,
                    CofficientSafety = safetyDeduction?.SumPercent ?? 0,
                    CofficientDeductionPrice = cofficientDeductionPrice,
                    BalanceRate = (decimal)balanceRate,
                    JobGroupId = input.JobGroupId,
                    DeductionReward = input.dailyDeductionReward + ":" + input.deductionhours + ":" + input.deductionminutes,
                    DuctionInMinutes = input.ductionInMinutes
                };

                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        /// <summary>
        /// پاداش ویژه پرسنل 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private EmployeeRewardDto SpecificEmployeeRewardCalculate(EmployeeRewardCalcInput input)
        {
            //مبلغ پاداش
            var outdetail = input.rewardOutHeaderQuery?.RewardOutDetails?.FirstOrDefault(a => a.RewardUnitTypeId == input.RewardUnitTypeId);
            long M = outdetail?.PaymentAmountKpi ?? 0;
            if (input.currentJobPosition.JobPoisitionStatusId == EnumJobPositionStatus.Moavenat.Id || input.currentJobPosition.JobPoisitionStatusId == EnumJobPositionStatus.ModirAmel.Id)
            {
                M = input.MaxPaymentAmountKpi ?? 0;
            }
            //ضریب کارایی
            var K = Convert.ToDouble(input.EfficiencyHistoryList.FirstOrDefault(a => a.EmployeeId == input.EmployeeId)?.Efficiency ?? 0);
            //ضریب پاداش ویژه
            var B = Convert.ToDouble(input.currentJobPosition.RewardSpecificEfficincy ?? 0);
            //ضریب گروه شغلی
            double G = Convert.ToDouble(input.JobGroupScore ?? 0);
            //ضریب سنوات افزوده
            var increasePercentData = input.increasePercentData;
            //double coefficientincreasePercent = 0;
            //if (input.WorkGroupCode == "R")
            //    coefficientincreasePercent = increasePercentData?.CoefficientYearsDay ?? 0;
            //else
            //    coefficientincreasePercent = increasePercentData?.CoefficientYearsShift ?? 0;
            ///ضریب تبدیل شده H
            //var H = GetIncreasedPercentConverted(coefficientincreasePercent);
            // پاداش تولید پرسنل بر اساس کارکرد ماهانه و ضریب های موثر در پاداش محاسبه می شود
            // var P = K * B * (G * M);
            var P = K * B * (G * M);
            //تعداد روز کسر از پاداش از تعداد روز مشمول پاداش کم می شود:
            //تعداد روز مورد محاسبه = Day - Deduction - Total - Day
            // var calculatedDay = input.dailyKarkard - input.dailyDeductionReward;

            // پاداش   محاسبه شده = (مبلغ پاداش P/ روزهای ماه بر اساس تقویم )  *تعداد روز مبنا پاداش ماه
            //var P1 = (P / input.countDaysWork) * calculatedDay;
            var countdaydeduction = input.countDaysWork - input.dailyKarkard;
            double P1 = P;
            double MD4 = 0;
            if (countdaydeduction > 0)
            {
                //P1 = P;
                MD4 = (P / input.countDaysWork) * countdaydeduction;
            }
            else P1 = (P / input.countDaysWork) * input.dailyKarkard;
            //                کسر از پاداش ساعتی : 
            //   ((مقدار ساعت * (پاداش محاسبه شده / 270 ) ) MD1 =
            var MD1 = (input.deductionhours * (P1 / 270));
            //((60 / مقدار دقیقه * (پاداش محاسبه شده / 270 ) )   MD2 =
            var MD2 = ((input.deductionminutes * (P1 / 270)) / 60);
            var MD3 = input.dailyDeductionReward * (P / input.countDaysWork);
            //جمع کسر از پاداش MD = MD1 + MD2
            var MD = MD1 + MD2 + MD3 + MD4;
            //پاداش نهایی جهت پرداخت: P2
            var P2 = P1 - MD;
            //ضریب عملکرد ایمنی
            //var safetyDeduction = input.EmployeeSafetyDeductionsList.FirstOrDefault(a => a.EmployeeId == input.EmployeeId);
            //مبلغ  کسر بابت رفتار غیر ایمن
            //var calculatedRewardPrice = Convert.ToInt64(P2 * (safetyDeduction?.SumPercent ?? 0) / 100);
            var errorDescription = "";
            if (input.dailyKarkard == 0)
            {
                errorDescription = "کارکرد فرد صفر است";
            }
            else if (input.currentmonthtimesheet == null)
            {
                errorDescription = "کارکرد فرد وجود ندارد ";
            }
            else if (K == 0)
            {
                errorDescription = "ضریب کارائی فرد صفر است";
            }
            var data = new EmployeeRewardDto()
            {
                EmployeeId = input.EmployeeId,
                RewardSpecificEfficiency = B,
                JobGroupId = input.JobGroupId,
                EmployeeNumber = input.EmployeeNumber,
                FullName = input.FullName,
                JobPositionId = input.currentJobPosition.Id,
                JobPositionTitle = input.currentJobPosition.MisJobPositionCode + "-" + input.currentJobPosition.Title,
                SpecificRewardId = input.currentJobPosition.SpecificRewardId,
                PaymentAmountKpi = M,
                CoefficientJobCategory = G,//
                CostCenterCode = input.currentJobPosition.CostCenter.ToString(),
                MisJobPositionCode = input.currentJobPosition.MisJobPositionCode,
                ProductionEfficiencyId = input.ProductionEfficiencyId,
                RewardUnitTypeId = input.RewardUnitTypeId,
                TeamWorkTitle = input.TeamWorkTitle,
                TPercent = outdetail?.TPercent,
                //IncreasedPercent = coefficientincreasePercent,
                //IncreasedPercentConverted = H,
                PaymentAmount = M,
                EfficiencyPerson = (decimal)K,
                InsuranceDurationInDay = input.dailyKarkard,
                DailyDeductionReward = input.dailyDeductionReward,//
                DeductionRewardPrice = (long)MD,
                CalculatedRewardPrice = (long)P1,
                PaymentRewardPrice = (long)P2,
                HourlyDeductionReward = input.deductionminutes,
                ErrorDescription = errorDescription,
                DeductionReward = input.dailyDeductionReward + ":" + input.deductionhours + ":" + input.deductionminutes,
                DuctionInMinutes = input.ductionInMinutes
                //CofficientSafety = safetyDeduction?.SumPercent ?? 0,
                //CofficientDeductionPrice = calculatedRewardPrice,
            };

            return data;
        }

        public double GetIncreasedPercentConverted(double coefficientincreasePercent)
        {
            double increasedPercentConverted = 0;
            if (coefficientincreasePercent == 1.75)
                increasedPercentConverted = 1.24;
            else if (coefficientincreasePercent == 1.6)
                increasedPercentConverted = 1.22;
            else if (coefficientincreasePercent == 1.45)
                increasedPercentConverted = 1.17;
            else if (coefficientincreasePercent == 1.3)
                increasedPercentConverted = 1.15;
            else if (coefficientincreasePercent == 1.2)
                increasedPercentConverted = 1.13;
            else if (coefficientincreasePercent == 1.1)
                increasedPercentConverted = 1.1;
            else if (coefficientincreasePercent == 1.05)
                increasedPercentConverted = 1.07;
            else if (coefficientincreasePercent < 1.05 /*&& coefficientincreasePercent!=0*/)
                increasedPercentConverted = 1;
            return increasedPercentConverted;
        }
    }
    internal class JobPositionForEmployeeRewardModel
    {

        public int Id { get; internal set; }
        public double? CPercent { get; internal set; }
        public int? CategoryCoefficientId { get; internal set; }
        public double? CofficientProximityProduct { get; internal set; }
        public string Title { get; internal set; }
        public decimal? CostCenter { get; internal set; }
        public string MisJobPositionCode { get; internal set; }
        public IEnumerable<Chart_JobPositionIncreasePercent> Chart_JobPositionIncreasePercents { get; internal set; }
        public int? JobPoisitionStatusId { get; internal set; }
        public double? RewardSpecificEfficincy { get; internal set; }
        public int? SpecificRewardId { get; internal set; }
        public int? RewardUnitTypeId { get; internal set; }
        public int? ProductionEfficiencyId { get; internal set; }
    }
    public class MonthTimesheetModel
    {
        public int EmployeeId { get; set; }
        public List<MonthTimeSheetIncluded> MonthTimeSheetIncludeds { get; set; }
    }
    internal class EmployeeRewardCalcInput
    {
        internal int ductionInMinutes;

        public MonthTimesheetModel currentmonthtimesheet { get; set; }

        public Chart_JobPositionIncreasePercent increasePercentData { get; set; }

        public JobPositionForEmployeeRewardModel currentJobPosition { get; set; }
        public int dailyKarkard { get; set; }
        public MonthTimeSheetIncluded durationInHourDeductionRewardmodel { get; set; }
        public int dailyDeductionReward { get; set; }
        public int deductionhours { get; set; }
        public int deductionminutes { get; set; }
        public RewardOutHeader rewardOutHeaderQuery { get; internal set; }
        public List<EmployeeEfficiencyHistory> EfficiencyHistoryList { get; internal set; }
        public List<EmployeeSafetyDeduction> EmployeeSafetyDeductionsList { get; internal set; }
        public int? ProductionEfficiencyId { get; internal set; }
        public int EmployeeId { get; internal set; }
        public decimal? JobGroupScore { get; internal set; }
        public string WorkGroupCode { get; internal set; }
        public int countDaysWork { get; internal set; }
        public string EmployeeNumber { get; internal set; }
        public string FullName { get; internal set; }
        public string TeamWorkTitle { get; internal set; }
        public int? JobGroupId { get; internal set; }
        public int? RewardUnitTypeId { get; internal set; }
        public long? MaxPaymentAmountKpi { get; internal set; }
    }

}
