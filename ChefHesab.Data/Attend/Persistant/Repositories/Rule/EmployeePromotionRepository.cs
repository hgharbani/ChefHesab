using DNTPersianUtils.Core;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities;
using Ksc.HR.Domain.Entities.Chart;
using Ksc.HR.Domain.Entities.Emp;
using Ksc.HR.Domain.Entities.EmployeeBase;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Entities.Rule;
using Ksc.HR.Domain.Entities.Salary;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Repositories.Rule;
using Ksc.HR.DTO.Rule.PromotionStatus;
using Ksc.HR.Share.General;
using Ksc.HR.Share.Model.emp;
using Ksc.HR.Share.Model.Employee;
using Ksc.HR.Share.Model.IncreaseSalary;
using Ksc.HR.Share.Model.Interdict;
using Ksc.HR.Share.Model.PaymentStatus;
using Ksc.HR.Share.Model.PersonalType;
using Ksc.HR.Share.Model.Rule;
using Ksc.HR.Share.Model.RuleCoefficient;
using Ksc.HR.Share.Model.Salary;
using KSC.Common;
using KSC.Common.Filters.Models;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using NCalc.Domain;
using NetTopologySuite.Index.HPRtree;
using System.Linq;
using System.Xml.Schema;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ksc.HR.Data.Persistant.Repositories.Rule
{
    public class EmployeePromotionRepository : EfRepository<EmployeePromotion, int>, IEmployeePromotionRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeePromotionRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }


        public List<EmployeeInterdict> GetInterdictForUpdateInLastLevelUpdateSum(int calPerianDate)
        {
            var strYearMonth = calPerianDate.ToString();
            var year = strYearMonth.Substring(0, 4);
            var startYearMonth = Convert.ToUInt32($"{year}01");
            var endYearMonth = Convert.ToUInt32($"{year}12");

            //var promotionEmployees = _kscHrContext
            //    .EmployeePromotions
            //    .AsNoTracking()
            //    .Where(x =>
            //    x.CalculateDate == calPerianDate &&
            //    x.PromotionStatusId == EnumPromotionStatus.PromotionSendToSalary.Id)
            //    .Include(x => x.Employee)
            //    //برای یک ماه
            //    //.ThenInclude(x => x.EmployeeInterdicts.Where(a => a.ExecuteDateYearMonth >= startYearMonth && a.ExecuteDateYearMonth <= endYearMonth))
            //    //برای زمانی ک از ماه های قبل مونده و چند ماهن
            //    .ThenInclude(x => x.EmployeeInterdicts)
            //    .ToList();


            var employeeinterdicts = _kscHrContext.EmployeeInterdicts.Include(x => x.EmployeeInterdictDetails)
                .ThenInclude(x => x.AccountCode)
                .Where(a => a.InterdictTypeId == 2 && a.PortalFinalConfirmFlag == false).ToList();


            return employeeinterdicts;

        }
        public List<EmployeePromotion> GetInterdictForUpdateInLastLevel(int calPerianDate)
        {

            var strYearMonth = calPerianDate.ToString();
            var year = strYearMonth.Substring(0, 4);
            var startDate = $"{year}".GetStartDateInYear();
            var endYearMonth = $"{year}12".GetEndDateInYear();
          
            var promotionEmployees = _kscHrContext.EmployeePromotions
                .AsNoTracking()
                .Where(x =>
                x.CalculateDate == calPerianDate &&
                (x.PromotionStatusId == EnumPromotionStatus.InsertPromotionInterdict.Id|| x.PromotionStatusId == EnumPromotionStatus.InsertAmendementInterdict.Id))
                .Include(x => x.Employee)
                .ThenInclude(x => x.EmployeeInterdicts.Where(a => a.IssuanceDate.Value.Date >= startDate.Date && a.IssuanceDate.Value.Date <= endYearMonth.Date))
                .ToList();


            return promotionEmployees;



        }

        /// <summary>
        /// آپدیت شماره حکم
        /// </summary>
        public List<EmployeeInterdict> UpdateInterdictNumber(int calPerianDate)
        {
            var updatedList = new List<EmployeeInterdict>();

            try
            {
                // calPerianDate 140310

                //ارتقا پیدا کنند  لیست افرادی ک باید 
                var listInterdictPromotion = GetInterdictForUpdateInLastLevel(calPerianDate);

                //لیست افراد اصلاحیه رو هم باید به لیست ارتقا اضاف کنم

                var oldlistAll = new List<EmployeeInterdict>();
                var newlistAll = new List<EmployeeInterdict>();


              
                var IsInterdictNeedEdited = listInterdictPromotion.Any(x => x.Employee.EmployeeInterdicts.Any(y => y.InterdictNumber.StartsWith("434") && y.InterdictNumber.EndsWith("90")));

                if (IsInterdictNeedEdited == false)
                {
                    return updatedList;
                }

                var listInterdictNeedEdited = listInterdictPromotion.Select(x => x.Employee.EmployeeInterdicts.Where(y => y.InterdictNumber.StartsWith("434") && y.InterdictNumber.EndsWith("90")).ToList()).ToList();

                foreach (var item in listInterdictNeedEdited)
                {
                    newlistAll.AddRange(item);
                }


                var oldEdited = listInterdictPromotion.Select(x => x.Employee.EmployeeInterdicts.ToList()).ToList();

                foreach (var item in oldEdited)
                {
                    oldlistAll.AddRange(item);
                }

                var oldInterdict = oldlistAll.GroupBy(x => x.EmployeeId);

                //لیست ارتقا
                var newInterdict = newlistAll.Where(x => x.InterdictTypeId == EnumInterdictType.PromotionGroup.Id).ToList();
                var newAmendmentInterdict = newlistAll.Where(x => x.InterdictTypeId == EnumInterdictType.AddAmendment.Id).ToList();

                List<Tuple<int, int>> findNumber = new();
                foreach (var item in oldInterdict)
                {
                    var lastInterdict = item.OrderByDescending(x => x.ExecuteDate).FirstOrDefault(a=>a.PortalFinalConfirmFlag==true);



                    var interdictNumber = lastInterdict.InterdictNumber;

                    var orginInterdict = newInterdict.Where(x => x.EmployeeId == lastInterdict.EmployeeId).OrderByDescending(x => x.ExecuteDate).FirstOrDefault();

                    var splited = interdictNumber.Split('/');
                    var counter = Convert.ToInt32(splited[2]) + 1;

                    var number = $"{splited[0]}/{splited[1]}/{counter}";

                    orginInterdict.InterdictNumber = number;
                    findNumber.Add(new Tuple<int, int>(item.Key.Value, counter));
                    updatedList.Add(orginInterdict);

                }
                var listUpdatedforAmendment = updatedList;
                //لیست اصلاحیه رو هم باید حساب کنم
       

                foreach (var item in newAmendmentInterdict)
                {
                    var findCounter = findNumber.OrderByDescending(a=>a.Item2).FirstOrDefault(a => a.Item1 == item.EmployeeId);
                    var interdictNumber = item.InterdictNumber;

                    var splited = interdictNumber.Split('/');
                    var counter = Convert.ToInt32(findCounter.Item2) + 1;

                    var number = $"{splited[0]}/{splited[1]}/{counter}";
                    findNumber.Add(new Tuple<int, int>(item.EmployeeId.Value, counter));
                    
                    item.InterdictNumber = number;
                    updatedList.Add(item);

                }


                //foreach (var lastInterdict in listUpdatedforAmendment)
                //{


                //    var orginInterdict = newAmendmentInterdict.Where(x => x.EmployeeId == lastInterdict.EmployeeId).OrderByDescending(x => x.ExecuteDate).FirstOrDefault();
                //    if (orginInterdict == null)
                //    {
                //        continue;
                //    }
                //    var interdictNumber = lastInterdict.InterdictNumber;

                //    var splited = interdictNumber.Split('/');
                //    var counter = Convert.ToInt32(splited[2]) + 1;

                //    var number = $"{splited[0]}/{splited[1]}/{counter}";
                //    orginInterdict.InterdictNumber = number;
                //    updatedList.Add(orginInterdict);

                //}


            }
            catch (Exception ex)
            {

                throw ex;
            }

            return updatedList;
        }

        /// <summary>
        /// استپ اول ارتقا گروه
        /// </summary>
        /// <param name="calculateDate">تاریخ صدور ماه و سال</param>
        /// <returns>احکامی ک از یک ماه قبل این تاریخ هستن و ارسال به پرتا شدن
        /// و اونهایی ک نوع حکمشون ابلاغ و باعث تغییر گروه شده
        /// یا اون هایی ک رده اون ها تغییر کرده 
        /// </returns>
        public IQueryable<EmployeeInterdict> GetCaculteWorkYearsFirstLevel(int calculateDate)
        {
            var endDate = DateTime.Now.Date;
            //var endDate = DateTimeExtensions.ToGorgianwithSeprateDate("1403/09/08");
            var calculateDateString = calculateDate.ToString();
            var prevYearMonth = DateTimeExtensions.GetPrevYearMonth(calculateDate).ToString();
            var startDate = $"{prevYearMonth.Substring(0, 4)}/{prevYearMonth.Substring(4, 2)}/01";
            var startDateGegorian = DateTimeExtensions.ToGorgianwithSeprateDate(startDate);

            var list1 = _kscHrContext
                                      .EmployeeInterdicts.Where(x => x.PortalFinalConfirmFlag == true &&
                                      x.IssuanceDate >= startDateGegorian && x.IssuanceDate <= endDate
                                      )
                                                      .Include(x => x.Chart_JobPosition)
                .ThenInclude(x => x.Chart_JobIdentity)
                .ThenInclude(x => x.Chart_JobCategory)
                .ThenInclude(x => x.Chart_JobCategoryDefination);
            return list1;
        }

        //public List<EmployeeInterdict> GetlatestInterdictByEmployeeId(List<EmployeeInterdict> interdicts,List<int> employeeId, List<int> employeeInterdictId)
        //{

        //    //بازه رو بیشتر کنم برای تست
        //    var query = _kscHrContext.EmployeeInterdicts
        //        .Where(x => employeeId.Contains(x.EmployeeId.Value))
        //        .GroupBy(x => x.EmployeeId).ToList();

        //    var lastedInterdic = new List<EmployeeInterdict>();
        //    foreach (var item in query)
        //    {
        //        var empId = item.Key;
        //        var date = item.Where(x => employeeInterdictId.Contains(x.Id)).Select(x => x.ExecuteDate).FirstOrDefault();
        //        var orderItem = item.Where(x => x.ExecuteDate < date).OrderByDescending(x => x.ExecuteDate).FirstOrDefault();
        //        lastedInterdic.Add(orderItem);
        //    }

        //    return lastedInterdic;
        //}

        public bool IsExistExecuteDate(int employeeId, DateTime executeDate)
        {
            var isExist = _kscHrContext.EmployeeInterdicts.Where(x => x.EmployeeId == employeeId).Any(x => x.ExecuteDate == executeDate);
            return false;
        }
        public bool IsExistIssuDate(int employeeId, DateTime IssuDate)
        {
            var isExist = _kscHrContext.EmployeeInterdicts.Where(x => x.EmployeeId == employeeId).Any(x => x.IssuanceDate == IssuDate);
            return false;
        }
        /// <summary>
        /// آخرین حکم هر فرد  قبل از حکم موردنظر
        /// </summary>
        public List<EmployeeInterdict> GetlatestInterdictByEmployeeId(List<EmployeeInterdict> interdicts, List<int> employeeId, List<int> employeeInterdictId)
        {
            var query = _kscHrContext.EmployeeInterdicts
                .Where(x => employeeId.Contains(x.EmployeeId.Value))
                .Include(x => x.Chart_JobPosition)
                .ThenInclude(x => x.Chart_JobIdentity)
                .ThenInclude(x => x.Chart_JobCategory)
                .ThenInclude(x => x.Chart_JobCategoryDefination)
                .GroupBy(x => x.EmployeeId)
                .ToList();

            var lastedInterdic = new List<EmployeeInterdict>();
            foreach (var item in query)
            {
                var d = item.OrderByDescending(x => x.ExecuteDate).ToList();
                var empId = item.Key;
                var date = d.Where(x => employeeInterdictId.Contains(x.Id)).Select(x => x.ExecuteDate).FirstOrDefault();
                var orderItem = d.Where(x => x.ExecuteDate < date).FirstOrDefault();
                if (orderItem != null)
                {
                    foreach (var i in interdicts)
                    {
                        if (i.EmployeeId == orderItem.EmployeeId)
                        {
                            if (i.InterdictTypeId == EnumInterdictType.NotifPost.Id && i.CurrentJobGroupId != orderItem.CurrentJobGroupId)
                            {
                                lastedInterdic.Add(i);
                            }

                            if (i.Chart_JobPosition.Chart_JobIdentity.Chart_JobCategory.JobCategoryDefinationId
                                != orderItem.Chart_JobPosition.Chart_JobIdentity.Chart_JobCategory.JobCategoryDefinationId
                                )
                            {
                                lastedInterdic.Add(i);
                            }
                        }
                    }

                }


            }

            return lastedInterdic;
        }


        /// <summary>
        /// محاسبه تکی ارتقا
        /// </summary>
        public IQueryable<EmployeePromotionInterdicts> CalculateIssuancePromotions(int calculateDate, int? employeeId, int promotionSendToSalaryId, int interdictId, int EnumWorkFlowStatusId)
        {
            var calDate = calculateDate.ToString();
            var calDateGegorian = DateTimeExtensions.YearMonthToGegorianDateFirstMonth(calDate);

            if (employeeId != null)
            {
                var list1 = _kscHrContext
                                        .EmployeePromotionInterdicts
                                        .Include(x => x.EmployeePromotion)
                                        .ThenInclude(x => x.Employee)
                                        .Include(x => x.EmployeeInterdict).ThenInclude(x => x.EmployeeInterdictDetails)
                                        .Include(x => x.EmployeePromotion).ThenInclude(x => x.WF_Request)

                                        .Where(x => x.EmployeePromotion.CalculateDate == calculateDate &&
                                         x.EmployeePromotion.PromotionDate.Value.Date <= calDateGegorian.Value.Date &&
                                         x.EmployeePromotion.EmployeeId == employeeId &&
                                         x.EmployeePromotion.WF_Request.StatusId == EnumWorkFlowStatusId &&
                                         x.EmployeePromotion.PromotionStatusId == promotionSendToSalaryId
                                         && x.EmployeeInterdictId == interdictId
                                         );


                return list1;
            }
            else
            {
                var list1 = _kscHrContext
                                        .EmployeePromotionInterdicts
                                        .Include(x => x.EmployeePromotion)
                                        .ThenInclude(x => x.Employee)
                                        .Include(x => x.EmployeeInterdict)
                                        .ThenInclude(x => x.EmployeeInterdictDetails)
                                        .Where(x => x.EmployeePromotion.CalculateDate == calculateDate &&
                                         x.EmployeePromotion.PromotionDate.Value.Date <= calDateGegorian.Value.Date &&
                                         x.EmployeePromotion.PromotionStatusId == promotionSendToSalaryId
                                         && x.EmployeeInterdictId == interdictId
                                         );
                return list1;

            }


        }

        /// <summary>
        /// لیست افراد شامل اصلاحیه حکم
        /// </summary>
        public IQueryable<EmployeePromotionInterdicts> GetAmendmentPromotions(int calculateDate, int? employeeId, int promotionSendToSalaryId, int EnumWorkFlowStatusId)
        {
            var calDate = calculateDate.ToString();
            var calDateGegorian = DateTimeExtensions.YearMonthToGegorianDateFirstMonth(calDate);

            var list = _kscHrContext.EmployeePromotionInterdicts
                .Include(x => x.EmployeePromotion).ThenInclude(x => x.WF_Request)
                                    .Include(x => x.EmployeeInterdict)
                                    .ThenInclude(x => x.EmployeeInterdictDetails)
                                    .Include(x => x.EmployeePromotion)
                                    .ThenInclude(x => x.PromotionStatus)
                                    .Include(x => x.EmployeePromotion)
                                    .ThenInclude(x => x.Employee)
                                    .ThenInclude(x => x.EmployeeJobPositions)
                                    .ThenInclude(x => x.Chart_JobPosition)
                                    .ThenInclude(x => x.Chart_JobIdentity)
                                    .ThenInclude(x => x.Chart_JobCategory)
                                    .ThenInclude(x => x.Chart_JobCategoryDefination)
                                    .Include(x => x.EmployeePromotion)
                                    .ThenInclude(x => x.CurrentJobGroup)
                                    .Include(x => x.EmployeePromotion)
                                    .ThenInclude(x => x.NewJobGroup)
                                    .Where(x =>
                                    x.EmployeePromotion.PromotionDate.Value.Date < calDateGegorian.Value.Date &&
                                    x.EmployeeInterdict.ExecuteDate.Value.Date <= calDateGegorian.Value.Date &&
                                    x.EmployeeInterdict.ExecuteDate.Value.Date >= x.EmployeePromotion.PromotionDate.Value.Date &&
                                    x.EmployeePromotion.CalculateDate == calculateDate
                                    //&&x.EmployeePromotion.WF_Request.StatusId ==EnumWorkFlowStatusId &&
                                    //x.EmployeePromotion.PromotionStatusId == promotionSendToSalaryId
                                    );



            if (employeeId > 0)
            {
                list.Where(x => x.EmployeeInterdict.EmployeeId == employeeId);

            }

            return list;
        }

        /// <summary>
        /// لیست افراد شامل ارتقا حکم
        /// </summary>
        public IQueryable<EmployeePromotionInterdicts> GetIssuancePromotions(int calculateDate, int promotionSendToSalaryId, int EnumWorkFlowStatusId)
        {

            var result = _kscHrContext.EmployeePromotionInterdicts.Where(x =>
      x.EmployeePromotion.CalculateDate == calculateDate &&
      x.EmployeePromotion.PromotionStatusId == promotionSendToSalaryId &&
      x.EmployeePromotion.WF_Request.StatusId == EnumWorkFlowStatusId
      )
                .Include(x => x.EmployeeInterdict).ThenInclude(x => x.EmployeeInterdictDetails)
                 .Include(x => x.EmployeePromotion.CurrentJobGroup)
                 .Include(x => x.EmployeePromotion.NewJobGroup)
                 .Include(x => x.EmployeePromotion)
                 .ThenInclude(x => x.Employee)
                 .ThenInclude(x => x.EmployeeJobPositions)
                 .ThenInclude(x => x.Chart_JobPosition)
                 .ThenInclude(x => x.Chart_JobIdentity)
                 .ThenInclude(x => x.Chart_JobCategory)
                 .ThenInclude(x => x.Chart_JobCategoryDefination)
                 .Include(x => x.EmployeePromotion)
                 .ThenInclude(x => x.WF_Request)

      //اینجا
      ;


            return result;
        }

        //test
        //public async Task<List<EmployeeInterdictDetail>> Test()
        //{
        //    var last = await _kscHrContext.EmployeeInterdicts.Include(x => x.EmployeeInterdictDetails).Where(x => x.EmployeeInterdictDetails.Count() > 0 && x.ExecuteDateYearMonth >= 140301).FirstOrDefaultAsync();
        //    var sdd = await _kscHrContext.EmployeeInterdicts.Include(x => x.EmployeeInterdictDetails).Where(x => x.EmployeeInterdictDetails.Count() == 0 && x.ExecuteDateYearMonth >= 140306).ToListAsync();
        //    var list = new List<EmployeeInterdictDetail>();
        //    foreach (var item in sdd)
        //    {
        //        foreach (var deta in last.EmployeeInterdictDetails)
        //        {
        //            deta.Id = 0;
        //            deta.EmployeeInterdictId = item.Id;
        //            list.Add(deta);
        //        }

        //    }

        //    return list;
        //}


        /// <summary>
        /// لیست افراد شامل ارتقا حکم برای محاسبه exel  و ثبت
        /// </summary>
        /// <param name="calculateDate"></param>
        /// <param name="promotionSendToSalaryId"></param>
        /// <param name="EnumWorkFlowStatusId"></param>
        /// <param name="IsWantModify">آیا نیاز به اصلاحیه دارد</param>
        /// <returns></returns>
        public async Task<List<PromotionAccountEmploymentTypeInterdictDto>> GetRelatedInterdictWithPromotions(int calculateDate, int promotionSendToSalaryId, int EnumWorkFlowStatusId, bool IsWantModify = false)
        {


            var salaryDate = _kscHrContext.SystemControlDates.FirstOrDefault(a => a.IsActive).SalaryDate;
            var workCalendar = _kscHrContext.WorkCalendars.Where(a => a.YearMonthV1 == salaryDate).ToList();
            var day1 = workCalendar.OrderBy(a=>a.MiladiDateV1).First();
            var viewmodels = new List<PromotionAccountEmploymentTypeInterdictDto>();
            try
            {
                var result = await _kscHrContext.EmployeePromotions.AsQueryable().AsNoTracking()
           .OrderByDescending(a => a.PromotionDate)
           .Where(x => x.CalculateDate == calculateDate

                      && x.PromotionStatusId == promotionSendToSalaryId
                      )
           .Include(x => x.Employee)
                    .ThenInclude(x => x.EmployeeInterdicts)
                    .ThenInclude(x => x.EmployeeInterdictDetails)
                    .ThenInclude(x => x.AccountCode)
           .Include(x => x.Employee)
           .ThenInclude(a => a.Families)
           .Select(a => new
           {
               a.Id,
               a.EmployeeId,
               PromotionDate = a.PromotionDate.Value,
               UvPercent = a.Employee.UVPercent,
               a.CalculateDate,
               a.Employee.EmployeeNumber,
               FullName = a.Employee.Name + " " + a.Employee.Family,
               a.Employee.Name,
               a.Employee.Family,
               a.PromotionDateYearMonth,
               CurrentJobPosition = a.Employee.JobPositionId,
               a.CurrentJobGroupId,
               a.NewJobGroupId,

               IsWantModifyInterdict = a.Employee.EmployeeInterdicts
                   .OrderByDescending(a => a.ExecuteDate)
                   .Any(x =>
                   x.ExecuteDateYearMonth > a.PromotionDateYearMonth),


               AllInterdictWantModify = a.Employee.EmployeeInterdicts
                   .OrderByDescending(a => a.ExecuteDate)
                   .Where(x =>
                   x.ExecuteDateYearMonth > a.PromotionDateYearMonth)
                   .GroupBy(a => a.ExecuteDateYearMonth).Select(a => new
                   {
                       a.Key,
                       MaxEverMonthInterdictList = a.OrderByDescending(a => a.ExecuteDate).FirstOrDefault()
                   })
                   ,



               FamilyCount = a.Employee.Families.Count(c =>
              (c.DependenceTypeId == EnumDependentType.ChildGirl.Id || c.DependenceTypeId == EnumDependentType.ChildBoye.Id)
              && (c.BirthDate <= a.PromotionDate.Value &&
                 (
                 !c.EndDateDependent.HasValue ||
                 ((c.DependenceTypeId == EnumDependentType.ChildGirl.Id && c.IsContinuesDependent == true)
                 || c.EndDateDependent.Value.Date >= a.PromotionDate.Value
                 )))),
               PromotionJobPositionId = a.JobPositionId,
               EmploymentTypeId = a.Employee.EmploymentTypeId.Value,
               MaritalStatusId = a.Employee.MaritalStatusId,
               MarriedDate = a.Employee.MarriedDate,
               WorkCityId = a.Employee.WorkCityId,
           }).ToListAsync();


                var JobPositionsId = result.Select(a => a.PromotionJobPositionId).ToList();

                var ChartJobPositions = _kscHrContext.Chart_JobPosition
                    .Where(a => JobPositionsId.Contains(a.Id)).Include(x => x.Chart_JobIdentity).ThenInclude(x => x.Chart_JobCategory)
                     .ThenInclude(x => x.Chart_JobCategoryDefination)
                     .ThenInclude(x => x.InterdictWeathers).ToList();
                var employeeEmployeeId = result.Select(a => a.EmployeeId).ToList();

                var EmployeeInterdict = _kscHrContext.EmployeeInterdicts
                    .Where(a => employeeEmployeeId.Contains(a.EmployeeId))
                    .Include(a => a.EmployeeInterdictDetails)
                    .ThenInclude(x => x.AccountCode)
                    .AsNoTracking()
                    .ToList();

                var hardWorkFactor = await _kscHrContext.CoefficientSetting.AsNoTracking().ToListAsync();

                var EmploymentTypeIds = result.Select(a => a.EmploymentTypeId);

                var MaritalStatusIds = result.Select(a => a.MaritalStatusId);


                var selectedInterdictCategory = new List<int> {
                EnumInterdictCategory.GroupSalaryAmount.Id,
                EnumInterdictCategory.Merit.Id,
                EnumInterdictCategory.BadWeader.Id,
                EnumInterdictCategory.Years.Id,
            };

                var AccountEmploymentTypes = await _kscHrContext.AccountEmploymentType
                    .Where(x => EmploymentTypeIds.Contains(x.EmploymentTypeId) &&
                                                x.AccountCode.IsActive == true &&
                                                x.IsActive == true)
                                 .AsQueryable().AsNoTracking()
                                 .Include(a => a.AccountCode)
                                .ThenInclude(a => a.InterdictMaritalSettingDetails)
                                .ThenInclude(a => a.InterdictMaritalSetting)
                                .Include(a => a.AccountCode)
                                .ThenInclude(a => a.InterdictCategory)
                                .Where(a => a.AccountCode.InterdictCategoryId != null)
                                .Select(a => new AccountEmployeeCalssModel
                                {
                                    EmploymentTypeId = a.EmploymentTypeId,
                                    AccountCode = a.AccountCode,
                                    AccountCodeId = a.AccountCodeId,
                                    AccountCodeCategoryId = a.AccountCode.AccountCodeCategoryId,
                                    AccountCodeCategoryTitle = a.AccountCode.AccountCodeCategory.Title,
                                    IsEditablePrice = a.AccountCode.InterdictCategory.IsEditablePrice,
                                    InterdictCategoryId = a.AccountCode.InterdictCategoryId,
                                    InterdictCategoryTitle = a.AccountCode.InterdictCategory.Title,
                                    InterdictMaritalSettingDetails = a.AccountCode.InterdictMaritalSettingDetails
                                }).ToListAsync();



                var BasisSalaryItems = await _kscHrContext.BasisSalaryItems
                    .Where(x => EmploymentTypeIds.Contains(x.EmploymentTypeId) && x.IsConfirmed)
                    .Include(a => a.BasisSalaryItemPerGroups)
                    .OrderByDescending(x => x.Id)
                    .AsNoTracking()
                    .ToListAsync();



                var meritFactorValue = await _kscHrContext.CoefficientSetting
                    .Where(x => x.CoefficientId == EnumRuleCoefficient.MeritScor.Id && x.IsActive == true)
                    .ToListAsync();

                if (IsWantModify)
                {
                    result = result.Where(a => a.IsWantModifyInterdict).ToList();
                }
            
                var insuranceDateList = new List<string>();
                foreach (var promotion in result.ToList())
                {

                    var ListISuranceDate = EmployeeInterdict.Where(a => a.IssuanceDate.Value.Date >= day1.MiladiDateV1 && a.EmployeeId == promotion.EmployeeId).ToList();


                    var AllIssuanceDateUser = ListISuranceDate.Select(a => a.IssuanceDate.Value.Date).ToList();
                    var concatWorkCalendar = workCalendar.Where(a => !AllIssuanceDateUser.Contains(a.MiladiDateV1)).ToList();

                    //آخرین حکم
                    var LastInterdict = EmployeeInterdict
                        .AsQueryable()
                        .AsTracking()
                        .OrderByDescending(a => a.ExecuteDate)
                        .Where(x =>
                            x.EmployeeId == promotion.EmployeeId &&
                            x.ExecuteDateYearMonth <= promotion.PromotionDateYearMonth
                            && x.PortalFinalConfirmFlag.Value == true)
                        .FirstOrDefault();

                    if (LastInterdict != null)
                    {
                        var ExecuteDateNew = LastInterdict.ExecuteDate.Value.Date;
                        var IssuranceDateNew = LastInterdict.IssuanceDate.Value.Date;


                        //اصلاحیه
                        if (IsWantModify)
                        {
                            if (promotion.IsWantModifyInterdict == false) continue;





                            var AllInterDictModiy = promotion
                                                            .AllInterdictWantModify
                                                            .Select(a => a.MaxEverMonthInterdictList)
                                                            .ToList();

                            foreach (var interDictWantModify in AllInterDictModiy)
                            {
                                insuranceDateList.Add(promotion.EmployeeNumber);
                                var viewmodel = new PromotionAccountEmploymentTypeInterdictDto();

                                viewmodel.PromotionId = promotion.Id;
                                viewmodel.EmployeeId = promotion.EmployeeId.Value;
                                viewmodel.EmployeeNumber = promotion.EmployeeNumber;
                                viewmodel.EmployeeFullName = promotion.FullName;
                                viewmodel.Name = promotion.Name;
                                viewmodel.Family = promotion.Family;
                                viewmodel.IsWantModifyInterdict = promotion.IsWantModifyInterdict;
                                viewmodel.InterdictId = interDictWantModify.Id;
                                viewmodel.EmploymentTypeId = interDictWantModify.EmploymentTypeId;
                                viewmodel.ExecuteDate = interDictWantModify.ExecuteDate;
                                viewmodel.HardWorkScore = interDictWantModify.HardWorkScore;
                                viewmodel.InterdictDescriptionId = interDictWantModify.InterdictDescriptionId;
                                viewmodel.InterdictEndDate = interDictWantModify.InterdictEndDate;
                                viewmodel.InterdictNumber = interDictWantModify.InterdictNumber;
                                viewmodel.JobPositionId = promotion.PromotionJobPositionId;
                                viewmodel.NewBasisPrice = (interDictWantModify.SumPriceBasisSalary != null) ? interDictWantModify.SumPriceBasisSalary.Value : 0;
                                viewmodel.NewDiffBasisPrice = (interDictWantModify.SumPriceIndependentBasisSalary != null) ? interDictWantModify.SumPriceIndependentBasisSalary.Value : 0;
                                viewmodel.CurrentJobGroupId = promotion.CurrentJobGroupId.Value;
                                viewmodel.NewJobGroupId = promotion.NewJobGroupId;
                                viewmodel.PromotionDate = promotion.PromotionDate;
                                viewmodel.RadiationPercentage = interDictWantModify.RadiationPercentage;
                                viewmodel.ReasonJobMovingId = interDictWantModify.ReasonJobMovingId;
                                viewmodel.SpecialLiabilityScore = interDictWantModify.SpecialLiabilityScore;
                                viewmodel.SupervisionScore = interDictWantModify.SupervisionScore;
                                viewmodel.FamilyCount = promotion.FamilyCount;
                                viewmodel.PromotionDateYearMonth = promotion.PromotionDateYearMonth;
                                viewmodel.WorkCityId = promotion.WorkCityId;
                                viewmodel.IsWantModify = IsWantModify;
                                viewmodel.UvPercent = (promotion.UvPercent != null) ? promotion.UvPercent.Value : 0;
                                viewmodel.ExcuteDateInterDictAddOneDays = interDictWantModify.ExecuteDate.Value.AddDays(1);
                                viewmodel.LastIssuranceDate = IssuranceDateNew;



                                var findIssanceadded = viewmodels.Where(a => a.EmployeeNumber == promotion.EmployeeNumber)
                     .Select(a => a.IssuranceDateInterDictAddOneDays.Date).ToList();

                                if (findIssanceadded.Any())
                                {
                                    concatWorkCalendar = concatWorkCalendar.Where(a => !findIssanceadded.Contains(a.MiladiDateV1)).ToList();
                                }

                                viewmodel.IssuranceDateInterDictAddOneDays = concatWorkCalendar.OrderBy(a => a.MiladiDateV1).First().MiladiDateV1;



                                var ChartJobPosition = ChartJobPositions.FirstOrDefault(a => a.Id == promotion.PromotionJobPositionId);
                                viewmodels.Add(CalculateAmendmentPromotions(ChartJobPosition, hardWorkFactor, AccountEmploymentTypes, BasisSalaryItems, viewmodel, interDictWantModify, meritFactorValue));

                            }

                        }
                        //صدور ارتقا
                        else
                        {

                            var viewmodel = new PromotionAccountEmploymentTypeInterdictDto();

                            viewmodel.PromotionId = promotion.Id;
                            viewmodel.EmployeeId = promotion.EmployeeId.Value;
                            viewmodel.EmployeeNumber = promotion.EmployeeNumber;
                            viewmodel.EmployeeFullName = promotion.FullName;
                            viewmodel.Name = promotion.Name;
                            viewmodel.Family = promotion.Family;
                            viewmodel.IsWantModifyInterdict = promotion.IsWantModifyInterdict;
                            viewmodel.InterdictId = LastInterdict.Id;
                            viewmodel.EmploymentTypeId = LastInterdict.EmploymentTypeId;
                            viewmodel.ExecuteDate = LastInterdict.ExecuteDate.Value.Date;
                            viewmodel.HardWorkScore = LastInterdict.HardWorkScore;
                            viewmodel.InterdictDescriptionId = LastInterdict.InterdictDescriptionId;
                            viewmodel.InterdictEndDate = LastInterdict.InterdictEndDate;
                            viewmodel.InterdictNumber = LastInterdict.InterdictNumber;
                            viewmodel.JobPositionId = promotion.PromotionJobPositionId;


                            //اینجا
                            viewmodel.NewBasisPrice = (LastInterdict.SumPriceBasisSalary != null) ? LastInterdict.SumPriceBasisSalary.Value : 0;
                            viewmodel.NewDiffBasisPrice = (LastInterdict.SumPriceIndependentBasisSalary != null) ? LastInterdict.SumPriceIndependentBasisSalary.Value : 0;
                            viewmodel.CurrentJobGroupId = promotion.CurrentJobGroupId.Value;
                            viewmodel.NewJobGroupId = promotion.NewJobGroupId;
                            viewmodel.PromotionDate = promotion.PromotionDate;
                            viewmodel.RadiationPercentage = LastInterdict.RadiationPercentage;
                            viewmodel.ReasonJobMovingId = LastInterdict.ReasonJobMovingId;
                            viewmodel.SpecialLiabilityScore = LastInterdict.SpecialLiabilityScore;
                            viewmodel.SupervisionScore = LastInterdict.SupervisionScore;
                            viewmodel.WorkCityId = promotion.WorkCityId;
                            viewmodel.PromotionDateYearMonth = promotion.PromotionDateYearMonth;
                            viewmodel.FamilyCount = promotion.FamilyCount;
                            viewmodel.IsWantModify = IsWantModify;
                            viewmodel.UvPercent = (promotion.UvPercent != null) ? promotion.UvPercent.Value : 0;
                            viewmodel.LastIssuranceDate = IssuranceDateNew;


                            viewmodel.IssuranceDateInterDictAddOneDays = concatWorkCalendar.OrderBy(a => a.MiladiDateV1).First().MiladiDateV1;

                            viewmodel.ExcuteDateInterDictAddOneDays =
                                                                    ExecuteDateNew >= promotion.PromotionDate.Date ?
                                                                    ExecuteDateNew.AddDays(1) :
                                                                    promotion.PromotionDate.Date;



                            var ChartJobPosition = ChartJobPositions.FirstOrDefault(a => a.Id == promotion.PromotionJobPositionId);
                            var getPromotionCal = CalculateAmendmentPromotions(ChartJobPosition, hardWorkFactor, AccountEmploymentTypes, BasisSalaryItems, viewmodel, LastInterdict, meritFactorValue);
                            viewmodels.Add(getPromotionCal);

                        }
                    }


                }
                //var maritaldetailsquery = _kscHrUnitOfWork.InterdictMaritalSettingDetailRepository.GetAllQueryable().Include(x => x.AccountCode).Include(x => x.InterdictMaritalSetting).ToList();

                //var BasisSalaryItems = _kscHrUnitOfWork.BasisSalaryItemRepository.GetAll()
                //    .OrderByDescending(x => x.Id);
                //var hardWorkFactor = _kscHrUnitOfWork.CoefficientSettingRepository.GetAll();


                return viewmodels;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        
        }







        private PromotionAccountEmploymentTypeInterdictDto CalculateAmendmentPromotions(Chart_JobPosition ChartJobPositions,
            List<CoefficientSetting> hardWorkFactor, List<AccountEmployeeCalssModel> AccountEmploymentTypes,
            List<BasisSalaryItem> BasisSalaryItems,
            PromotionAccountEmploymentTypeInterdictDto viewmodel
            , EmployeeInterdict LastInterdict,
           List<CoefficientSetting> CoefficientSettings
            )
        {

            viewmodel.InterdictTypeId = EnumInterdictType.PromotionGroup.Id;
            var jobpositionEmployee = ChartJobPositions;
            var jobCategoryDefination = jobpositionEmployee.Chart_JobIdentity.Chart_JobCategory.Chart_JobCategoryDefination;
            viewmodel.JobPositionTitle = jobpositionEmployee.MisJobPositionCode + "-" + jobpositionEmployee.Title;
            viewmodel.jobCategoryDefinationTitle = jobCategoryDefination.Title;
            var listAccountEmploymentTypeInterdict = new List<AccountEmploymentTypeInterdictDto>();




            var workCityId = viewmodel.WorkCityId;

            int yearmonth = 0;
            int ExecuteDateyear = 0;
            decimal TableFactor = 0;
            //مزد گروه شغل
            long years = 0;
            long merit = 0;

            merit = LastInterdict.EmployeeInterdictDetails.Where(x => x.AccountCodeId == EnumSalaryAcountCode.Merit.Id).Select(x => x.Amount).FirstOrDefault();

            try
            {

                ExecuteDateyear = Convert.ToInt32(LastInterdict.ExecuteDate.Value.GetPersianYear());
                yearmonth = Convert.ToInt32(LastInterdict.ExecuteDate.Value.GetYearMonthShamsiByMiladiDate());
             

                var GetLastBasisSalaryItemByYearMonth = BasisSalaryItems
                    .FirstOrDefault(x =>
                                    x.EmploymentTypeId == viewmodel.EmploymentTypeId &&
                                    x.StartDate <= yearmonth && x.EndDate >= yearmonth
                                    );

               

                //ضریب جدول
                TableFactor = GetLastBasisSalaryItemByYearMonth == null ? 0 : (decimal)(GetLastBasisSalaryItemByYearMonth.TableCoefficient);


                var countchilds = viewmodel.FamilyCount;
                var coefficientSettings = hardWorkFactor.Where(x => x.Year == ExecuteDateyear);

                var ListAccountTypeByEmployeementTypeId = AccountEmploymentTypes
                    .Where(a => a.EmploymentTypeId == viewmodel.EmploymentTypeId).ToList();


                var iswantCalculate = viewmodel.IsWantModify &&
                    (viewmodel.PromotionDate.GetPersianYear() < DateTime.Now.GetPersianYear());

                var GroupJobAmounts = GetLastBasisSalaryItemByYearMonth.BasisSalaryItemPerGroups
                                 .Where(x => x.WorkGroupId == viewmodel.NewJobGroupId)
                                 .OrderByDescending(x => x.Id).ToList();




                var lastYearAmount = LastInterdict.EmployeeInterdictDetails
                    .FirstOrDefault(x => x.AccountCode.InterdictCategoryId == EnumInterdictCategory.Years.Id);

                var lastGroup = LastInterdict.CurrentJobGroupId;
                var LastbaseAmountYear = GetLastBasisSalaryItemByYearMonth.BasisSalaryItemPerGroups.FirstOrDefault(a => a.WorkGroupId == lastGroup)?.BaseAmountYears;
                var newBaseAmountYEar= GetLastBasisSalaryItemByYearMonth.BasisSalaryItemPerGroups.FirstOrDefault(a => a.WorkGroupId == viewmodel.NewJobGroupId)?.BaseAmountYears;

                var newYarAmount =  lastYearAmount !=null? GetYearsAmount(newBaseAmountYEar.Value,lastYearAmount.Amount, LastbaseAmountYear.Value)  :0;
                 var YearsAmount = iswantCalculate == false ? lastYearAmount.Amount :
                    newYarAmount;











                var LastMeritAmount = LastInterdict.EmployeeInterdictDetails
                    .FirstOrDefault(x => x.AccountCode.InterdictCategoryId == EnumInterdictCategory.Merit.Id);
                var meritFactorValue = CoefficientSettings.FirstOrDefault(a => a.Year == ExecuteDateyear);
                var LastMerit = LastMeritAmount.Amount;
                var MeritPercentage = LastInterdict.MeritPercentage;
                var oldGroupJobAmounts = GetLastBasisSalaryItemByYearMonth.BasisSalaryItemPerGroups
                                 .Where(x => x.WorkGroupId == lastGroup)
                                 .OrderByDescending(x => x.Id).ToList();

                var newMeritAmount = Convert.ToInt64((GroupJobAmounts.First().GroupSalaryAmount * MeritPercentage) / 100); //حق شاستگی جدید

                var oldMeritAmount = Convert.ToInt64((oldGroupJobAmounts.First().GroupSalaryAmount * MeritPercentage) / 100);//حق شایستگی قدیم








                var MeritAmount = iswantCalculate == false ? LastMeritAmount.Amount :
                       (LastMerit - oldMeritAmount) + newMeritAmount;
                   // GetMeritAmount(LastMeritAmount.Amount, meritFactorValue.Value, GroupJobAmounts.First().GroupSalaryAmount);


                foreach (var item in ListAccountTypeByEmployeementTypeId)
                {

                    var accountEmploymentTypeInterdict = new AccountEmploymentTypeInterdictDto();
                    accountEmploymentTypeInterdict.AccountCodeId = item.AccountCodeId;
                    accountEmploymentTypeInterdict.InterdictId = (item.InterdictCategoryId != null) ? item.InterdictCategoryId.Value : 0;
                    accountEmploymentTypeInterdict.AccountCodeDesc = item.AccountCodeId + " - " + item.AccountCode.Title;
                    accountEmploymentTypeInterdict.InterdictCategoryId = item.InterdictCategoryId.HasValue ? item.InterdictCategoryId.Value : 0;
                    accountEmploymentTypeInterdict.InterdictCategoryTitle = item.InterdictCategoryId.HasValue ? item.InterdictCategoryTitle : "";


                    var isbase = false;
                    isbase = (item.AccountCode.AccountCodeCategoryId == EnumAccountCodeCategroy.BasicSalary.Id) ? true : false;
                    accountEmploymentTypeInterdict.IsBase = isbase;
                    accountEmploymentTypeInterdict.IsEditablePrice = item.IsEditablePrice;

                    ////ستون مبلغ حکم قبلی
                    var lastEmployeeInterdictDetails = LastInterdict?.EmployeeInterdictDetails.FirstOrDefault(x => x.AccountCodeId == item.AccountCodeId);
                    accountEmploymentTypeInterdict.PaymentpriceLatest = lastEmployeeInterdictDetails != null ? lastEmployeeInterdictDetails.Amount : 0;
                    accountEmploymentTypeInterdict.Paymentprice = lastEmployeeInterdictDetails != null ? lastEmployeeInterdictDetails.Amount : 0;


                    if (item.AccountCode.InterdictCategoryId == EnumInterdictCategory.Years.Id)
                    {

                        accountEmploymentTypeInterdict.Paymentprice = YearsAmount;
                        years = accountEmploymentTypeInterdict.Paymentprice;
                    }

                    //مزد گروه شغل
                    else if (item.InterdictCategoryId == EnumInterdictCategory.GroupSalaryAmount.Id)
                    {

                        var findejobGroupAccount = GroupJobAmounts.FirstOrDefault(a => a.SalaryAccountCodeId == item.AccountCodeId);
                        accountEmploymentTypeInterdict.Paymentprice = findejobGroupAccount != null ? findejobGroupAccount.GroupSalaryAmount : 0;
                    }

                    else if (item.InterdictCategoryId == EnumInterdictCategory.BadWeader.Id)
                    {

                        var findejobGroupAccount = GroupJobAmounts.FirstOrDefault();
                        var GroupSalaryAmount = findejobGroupAccount != null ? findejobGroupAccount.GroupSalaryAmount : 0;
                        var LastYearsAmount = YearsAmount != null ? YearsAmount : 0;


                        accountEmploymentTypeInterdict.Paymentprice = GetBadWeader(workCityId, yearmonth, jobCategoryDefination, GroupSalaryAmount, LastYearsAmount, MeritAmount);

                    }
                    else if (item.InterdictCategoryId == EnumInterdictCategory.RadiationPercentage.Id)
                    {

                        var findejobGroupAccount = GroupJobAmounts.FirstOrDefault();
                        //مزد گروه شغل
                        var GroupSalaryAmount = findejobGroupAccount != null ? findejobGroupAccount.GroupSalaryAmount : 0;
                        //سنوات
                        var LastYearsAmount = YearsAmount != null ? YearsAmount : 0;

                        //درصد اشعه
                        var radiationPercentage = viewmodel.UvPercent;
                        if (radiationPercentage != 0)
                        {
                            accountEmploymentTypeInterdict.Paymentprice = GetRadiationPercentage(radiationPercentage, GroupSalaryAmount, LastYearsAmount);

                        }

                    }
                    else if (item.InterdictCategoryId == EnumInterdictCategory.Merit.Id)
                    {
                        accountEmploymentTypeInterdict.Paymentprice = MeritAmount;
                    }


                    if (accountEmploymentTypeInterdict.Paymentprice == 0 && accountEmploymentTypeInterdict.PaymentpriceLatest == 0)
                    {
                        continue;
                    }



                    listAccountEmploymentTypeInterdict.Add(accountEmploymentTypeInterdict);
                }

                foreach (var i in listAccountEmploymentTypeInterdict)
                {


                    if (i.IsBase == true)
                    {

                        viewmodel.OldSumBase += i.Paymentprice;
                        viewmodel.OldSum += i.Paymentprice;


                    }
                    else
                    {
                        viewmodel.OldSumOtherBase += i.Paymentprice;
                        viewmodel.OldSum += i.Paymentprice;

                    }
                }
            }
            catch (Exception ex)
            {

            }


            if (listAccountEmploymentTypeInterdict != null)
            {
                listAccountEmploymentTypeInterdict = listAccountEmploymentTypeInterdict.OrderBy(x => x.InterdictId).ToList();

            }
            viewmodel.EmploymentInterdictDetails = listAccountEmploymentTypeInterdict;
            return viewmodel;
        }
        private long GetYearsAmount(long NewYarAmount , long oldYearAmount , long YearGroupAmount)
        {

            var newAmountSalary = (oldYearAmount- YearGroupAmount)+ NewYarAmount;

            return newAmountSalary;
        }
        private long GetMeritAmount(long currentMerit, decimal meritFactorValue, long groupSalary)
        {
            var result = currentMerit + (groupSalary * meritFactorValue);
            var finalResult = (long)Math.Round(result);
            return finalResult;
        }

        private long GetBadWeader(int? workCityId, int yearmonth, Chart_JobCategoryDefination jobCategoryDefination, decimal GroupJob, decimal years, decimal merit)
        {

            var weader = GetWeatherWithCityAndCategoryDefination(workCityId.Value, yearmonth, jobCategoryDefination).Select(x => x.Value).FirstOrDefault();
            var result = (GroupJob + years + merit) * weader;
            var finalResult = (long)Math.Round(result);
            return finalResult;
        }
        private long GetRadiationPercentage(double radiationPercentage, decimal GroupJob, decimal years)
        {
            var d = Convert.ToDecimal(radiationPercentage);
            var result = ((GroupJob + years) * d) / 100;
            var finalResult = (long)Math.Floor(result);
            return finalResult;
        }
        public List<InterdictWeather> GetWeatherWithCityAndCategoryDefination(int workCityId, int yearmonth, Chart_JobCategoryDefination jobCategoryDefination)
        {
            var weader = jobCategoryDefination.InterdictWeathers.Where(x => x.WorkCityId == workCityId && x.StartDate <= yearmonth && x.EndDate >= yearmonth).ToList();
            return weader;
        }

        private long GetGeneralByAccountCode(EmployeeInterdict lastEmployeeInterdict, int accountCodeId)
        {
            var result = (lastEmployeeInterdict != null) ? lastEmployeeInterdict.EmployeeInterdictDetails.Where(x => x.AccountCodeId == accountCodeId).Select(x => x.Amount).FirstOrDefault() : 0;

            return result;

        }


        /// پر کردن جدول واسط حکم و پیش بینی ارتقا
        public IQueryable<EmployeePromotionInterdicts> GetDataForPromotionInterdict(int calculateDate, int promotionSendToSalaryId, int EnumWorkFlowStatusId)
        {

            var result = _kscHrContext.EmployeePromotions
                .Include(x => x.WF_Request)
                                    .Where(x => x.CalculateDate == calculateDate &&
                                    (x.PromotionStatusId == promotionSendToSalaryId &&
                                    x.WF_Request.StatusId == EnumWorkFlowStatusId)
                                    ||
                                      (x.PromotionStatusId == promotionSendToSalaryId &&
                                      x.WfRequestId == null)
                                    )
                                    .Join(_kscHrContext.EmployeeInterdicts,
                                            p => p.EmployeeId, i => i.EmployeeId,
                                            (promotion, interdict) => new EmployeePromotionInterdicts()
                                            {
                                                EmployeeInterdictId = interdict.Id,
                                                EmployeePromotionId = promotion.Id,
                                            })
                                    ;


            //ی شرط دیگ باید اضاف کنم ک توی جدول واسط نباشه interdictId






            return result;





        }


        // ثبت پر کردن جدول واسط حکم و پیش بینی ارتقا
        public List<EmployeePromotionInterdicts> SetDataForPromotionInterdict(List<EmployeePromotionInterdicts> promotionInterdicts)
        {


            //var list1 = new List<int>() { 4, 5, 6, 7 };
            //var list2 = new List<int>() { 5, 6 };
            //var list3 = list1.Except(list2).ToList();

            var list1 = promotionInterdicts.Select(x => x.EmployeeInterdictId).ToList();
            var list2 = _kscHrContext.EmployeePromotionInterdicts.Select(x => x.EmployeeInterdictId).ToList();

            var exist = list1.Except(list2).ToList();

            var finalResult = promotionInterdicts.Where(x => exist.Contains(x.EmployeeInterdictId)).ToList();
            //این کوری رو چک کنم
            //var rersult = promotionInterdicts.Where(x => _kscHrContext.EmployeePromotionInterdicts.Any(y => y.EmployeeInterdictId != x.EmployeeInterdictId)).ToList();
            //var deleted = _kscHrContext.EmployeePromotionInterdicts.Where(x=> !interdictIds.Contains(x.EmployeeInterdictId)).ToList();

            return finalResult;
        }

        public IQueryable<EmployeePromotion> GetPromotionsByCalDate(int calDate)
        {
            var result = _kscHrContext.EmployeePromotions
                //.Include(x=>x.WF_Request)
                .Where(x => x.CalculateDate == calDate

                && x.PromotionStatusId == EnumPromotionStatus.InsertPromotionInterdict.Id || 
                x.PromotionStatusId == EnumPromotionStatus.InsertAmendementInterdict.Id


                )
                .Include(x => x.Employee)
                .ThenInclude(x => x.EmployeeInterdicts)
                ;
            return result;

        }

        public IQueryable<EmployeePromotion> GetAllByRelatedGrid()
        {
            var result = _kscHrContext.EmployeePromotions.Include(a => a.PromotionStatus)
                .Include(x => x.Employee).Include(x => x.NewJobGroup)
                .Include(x => x.CurrentJobGroup)
                .Include(x => x.Chart_JobPosition)
                .Include(x => x.EmployeeEducationDegree).ThenInclude(a => a.Education)
                .Include(x => x.PromotionRejectReason)
                .Include(a=>a.WF_Request).ThenInclude(a=>a.WF_Status)


                /*.AsNoTracking()*/.AsQueryable();
            return result;
        }
        public IQueryable<EmployeePromotion> GetAllByRelatedGridForDetails(int? calculateDate, int? statusId)
        {
            var result = _kscHrContext.EmployeePromotions
            .Where(a => a.CalculateDate == calculateDate)
            .Include(a => a.PromotionStatus)
                .Include(x => x.Employee).Include(x => x.NewJobGroup)
                .Include(x => x.Employee).ThenInclude(x => x.EmployeeDateInformations)

                .Include(x => x.CurrentJobGroup)
                .Include(x => x.Chart_JobPosition)
                .Include(x => x.EmployeeEducationDegree).ThenInclude(a => a.Education)
                .Include(x => x.PromotionRejectReason)


                /*.AsNoTracking()*/.AsQueryable();
            return result;
        }
        public IQueryable<EmployeePromotion> GetAllByStatusId(int statusid)
        {
            var result = GetAllByRelatedGrid().Where(a => a.PromotionStatusId == statusid)
                .AsQueryable();
            return result;
        }

        public IQueryable<EmployeePromotion> GetAllByPromotionIds(int[] ids)
        {
            var result = _kscHrContext.EmployeePromotions.Where(i => ids.Any(a => a == i.Id)).Include(x => x.Employee)
                .ThenInclude(x => x.EmployeeInterdicts)
                .AsQueryable();
            return result;
        }


        public EmployeePromotion GetPromotionConfirmRequest(int id)
        {
            return GetAllByRelatedGrid()
                .Include(x => x.WF_Request).Include(x => x.EmployeeEducationDegree).ThenInclude(x => x.Education).FirstOrDefault(i => i.WfRequestId == id);

        }

        public void UpdateRange(List<EmployeePromotion> list)
        {
            _kscHrContext.EmployeePromotions.UpdateRange(list);
        }
        public void RemoveRange(List<EmployeePromotion> list)
        {
            _kscHrContext.EmployeePromotions.RemoveRange(list);
        }
        public async Task<EmployeePromotion> GetOne(int id)
        {
            return await GetAllByRelatedGrid()
                .Include(x => x.WF_Request).FirstAsync(a => a.Id == id);
        }

        public IQueryable<EmployeePromotionInterdicts> CalculateAmendmentPromotions(int calculateDate, int? employeeId, int promotionSendToSalaryId, int interdictId, int EnumWorkFlowStatusId)
        {
            var calDate = calculateDate.ToString();
            var calDateGegorian = DateTimeExtensions.YearMonthToGegorianDateFirstMonth(calDate);

            var list1 = _kscHrContext
                          .EmployeePromotionInterdicts
                          .Include(x => x.EmployeePromotion)
                          .ThenInclude(x => x.Employee)
                          .Include(x => x.EmployeeInterdict)
                          .ThenInclude(x => x.EmployeeInterdictDetails)
                           .Include(x => x.EmployeePromotion).ThenInclude(x => x.WF_Request)

                          .Where(x => x.EmployeePromotion.CalculateDate == calculateDate &&
                           x.EmployeePromotion.PromotionDate.Value.Date <= calDateGegorian.Value.Date &&
                           x.EmployeeInterdict.ExecuteDate > x.EmployeePromotion.PromotionDate.Value.Date &&
                          x.EmployeeInterdict.ExecuteDate.Value.Date < calDateGegorian.Value.Date &&
                            //     x.EmployeePromotion.WF_Request.StatusId == EnumWorkFlowStatusId &&

                            //x.EmployeePromotion.PromotionStatusId == promotionSendToSalaryId&&
                            x.EmployeeInterdictId == interdictId
                           );

            return list1;
        }

        public IQueryable<EmployeePromotion> GetRemoveInterdictNumberto90(int calculateDate)
        {
            var list1 = _kscHrContext
                         .EmployeePromotions
                         .Include(x => x.Employee)
                         .ThenInclude(x => x.EmployeeInterdicts)
                         .Where(x => x.CalculateDate == calculateDate && x.PromotionStatusId == 5);
            return list1;
        }

    }
    public class AccountEmployeeCalssModel
    {
        public int EmploymentTypeId { get; set; }
        public int AccountCodeId { get; set; }
        public AccountCode AccountCode { get; set; }
        public int? AccountCodeCategoryId { get; set; }
        public string AccountCodeCategoryTitle { get; set; }
        public bool IsEditablePrice { get; set; }
        public int? InterdictCategoryId { get; set; }
        public string InterdictCategoryTitle { get; set; }
        public ICollection<InterdictMaritalSettingDetail> InterdictMaritalSettingDetails { get; set; }

    }

}