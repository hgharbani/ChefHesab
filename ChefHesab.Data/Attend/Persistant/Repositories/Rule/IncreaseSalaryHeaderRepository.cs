using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Reward;
using Ksc.HR.Domain.Entities.Rule;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Repositories.Rule;
using KSC.Common;
using Ksc.HR.DTO.Emp;
using Ksc.HR.Share.Model.Rule;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.DTO.Rule.EmployeePercentMeritHistory;
using DNTPersianUtils.Core;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Share.Model.emp;
using Ksc.HR.Share.Model.Employee;
using Ksc.HR.Share.Model.IncreaseSalary;
using Ksc.HR.Share.Model.Interdict;
using Ksc.HR.Share.Model.PaymentStatus;
using Ksc.HR.Share.Model.PersonalType;
using Ksc.HR.Share.Model.RuleCoefficient;
using Ksc.HR.Share.Model.Salary;
using Ksc.HR.Share.General;
using Ksc.HR.Domain.Entities.EmployeeBase;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Entities.Salary;
using Ksc.HR.DTO.Rule.IncreaseSalary;
using NetTopologySuite.Index.HPRtree;
using KSC.Common.Filters.Models;
using System.Linq.Dynamic.Core;

namespace Ksc.HR.Data.Persistant.Repositories.Rule
{
    public class IncreaseSalaryHeaderRepository : EfRepository<IncreaseSalaryHeader, int>, IIncreaseSalaryHeaderRepository
    {
        private readonly KscHrContext _kscHrContext;
        public IncreaseSalaryHeaderRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IncreaseSalaryHeader GetLatestIncreaseSalaryHeader(int yearMonth)
        {
            var model = _kscHrContext.IncreaseSalaryHeaders
                .Where(x => x.ValidStartDate == yearMonth)
                .OrderByDescending(x => x.ExecuteDate)
                .Include(x => x.IncreaseSalaryDetails)
                .FirstOrDefault();
            return model;

        }


        //public List<EmployeeInterdict> GetPersonelInterdictChangeJobPosition(List<int> employeeNumbers,DateTime startDate, DateTime endDate)
        //{
        //    //بازه زمانیش رو هم اعمل کنم
        //    var s = _kscHrContext.EmployeeInterdicts
        //        .Where(x => employeeNumbers
        //        .Contains(x.EmployeeId.Value) && x.ExecuteDate<=endDate && x.ExecuteDate>=startDate)
        //        .Include(x=>x.EmployeeInterdictDetails)
        //        .ThenInclude(x=>x.AccountCode)
        //        .ToList();
        //    return s;
        //}


        public IQueryable<IncreaseSalaryHeader> GetIncreaseSalaryHeaderByRelated(int? year)
        {
            var query = _kscHrContext.IncreaseSalaryHeaders
                .Include(x => x.IncreaseSalaryDetails)

                .Where(x => x.Year == year)
                .AsNoTracking();
            return query;
        }
        #region IncreaseSalary
        public IQueryable<Employee> GetEmployeeForIncreaseSalary()
        {
            List<int> enumEmploymentType = [
                EnumEmploymentType.Tamin.Id,
                EnumEmploymentType.Gharardadi.Id,
                EnumEmploymentType.Daem.Id,

            ];
            List<int> enumPaymentStatus = [
                EnumPaymentStatus.NewEmployeeOrganization.Id,
                EnumPaymentStatus.CurrentEmployee.Id,
                EnumPaymentStatus.Return.Id,
                EnumPaymentStatus.EducationMission.Id,
                //EnumPaymentStatus.LeavedEmployee.Id
                ];
            return _kscHrContext.Employees.Where(x =>
            x.PersonalTypeId == EnumPersonalType.EmploymentPerson.Id //استخدام شرکتی
           && enumEmploymentType.Contains(x.EmploymentTypeId.Value)
           && enumPaymentStatus.Contains(x.PaymentStatusId.Value)

            );
        }
        public IncreaseSalaryVM GetIncreaseSalary(DateTime? executeDate)
        {
            var executeDateYear = executeDate.GetPersianYear();
            int CalculateYear = executeDateYear.Value - 1;
            int yearmonthExecuteDate = int.Parse($"{executeDateYear}{executeDate.GetPersianMonth():00}");
            int yearmonthEndExecuteDate = int.Parse($"{executeDateYear}{12}");
            var workCalendar = _kscHrContext.WorkCalendars.Where(x => x.YyyyShamsi == executeDateYear || x.YyyyShamsi == CalculateYear).Select(x => new { x.YyyyShamsi, x.DateKey, x.MiladiDateV1 }).ToList();
            var MabnaTakafol = int.Parse($"{yearmonthExecuteDate}16");
            var MabnaTakafol_Miladi = workCalendar.FirstOrDefault(x => x.YyyyShamsi == executeDateYear && x.DateKey == MabnaTakafol)?.MiladiDateV1;
            //var employeePercentMeritHistory = _kscHrContext.EmployeePercentMeritHistories.Where(a => a.Year == CalculateYear).ToList();
            var allPerson = GetEmployeeForIncreaseSalary();
            var queryEmploymentPersonCurrent = allPerson
                ////.Where(x => x.EmployeeNumber == "420964") //// برای تست اطلاعات یک نفر
                .Where(x => x.EmploymentDate < executeDate //	تاریخ استخدام کوچکتر از تاریخ اجرا حکم باشد  
                && (x.PaymentStatusId == EnumPaymentStatus.LeavedEmployee.Id ? x.DismissalDate >= executeDate : true)//	اگر فردی وضعیت اشتغال آن 4 باشد  چک میشود تاریخ ترک خدمت  بزرگتر  از تاریخ اجرا حکم باشد
                )
                .Include(a => a.Families)
                .Include(a => a.EmploymentType)
                .Include(a => a.EmployeeInterdicts)
                .ThenInclude(a => a.Chart_JobPosition)
                .ThenInclude(a => a.Chart_JobIdentity)
                .ThenInclude(a => a.Chart_JobCategory)
                //.ThenInclude(a => a.EmployeeInterdictDetails).ThenInclude(a => a.AccountCode)

                .GroupBy(x => new
                {
                    x.Id,
                    x.EmployeeNumber,
                    x.Name,
                    x.Family,
                    x.EmploymentTypeId,
                    x.MaritalStatusId,
                    x.MarriedDate,
                    x.WorkCityId,
                    x.EmploymentDate,
                    EmploymentTypeTitle = x.EmploymentType.Title
                })
                .Select(a => new
                {
                    EmployeeId = a.Key.Id,
                    EmployeeNumber = a.Key.EmployeeNumber,
                    NameFamily = a.Key.Name + " " + a.Key.Family,
                    EmploymentTypeId = a.Key.EmploymentTypeId.Value,
                    EmploymentTypeTitle = a.Key.EmploymentTypeTitle,
                    MaritalStatusId = a.Key.MaritalStatusId,
                    MarriedDate = a.Key.MarriedDate,
                    EmploymentDate = a.Key.EmploymentDate,
                    WorkCityId = a.Key.WorkCityId,
                    LastEmployeeInterdicts = a.Select(x => x.EmployeeInterdicts.OrderByDescending(a => a.ExecuteDate).FirstOrDefault()),
                    FamilyCount = a.Select(a => a.Families.Count(c =>
                    (c.DependenceTypeId == EnumDependentType.ChildGirl.Id || c.DependenceTypeId == EnumDependentType.ChildBoye.Id)
                    && (c.BirthDate <= MabnaTakafol_Miladi.Value &&
                       (
                       !c.EndDateDependent.HasValue ||
                       ((c.DependenceTypeId == EnumDependentType.ChildGirl.Id && c.IsContinuesDependent == true)
                       || c.EndDateDependent.Value.Date >= MabnaTakafol_Miladi.Value
                       ))))).FirstOrDefault(),
                }
                )
                .ToList();
            var persianYearStartAndEndDates = CalculateYear.GetPersianYearStartAndEndDates();
            var coefficientSettingData = _kscHrContext.CoefficientSetting.FirstOrDefault(x => x.CoefficientId == EnumRuleCoefficient.IncreasedFactor.Id && x.Year == executeDateYear && x.IsActive == true);

            var model = new IncreaseSalaryVM()
            {
                Year = executeDateYear.Value,
                ValidStartDate = yearmonthExecuteDate,
                ValidEndDate = yearmonthEndExecuteDate,
                CalculateStartDate = persianYearStartAndEndDates.StartDate,
                CalculateEndDate = persianYearStartAndEndDates.EndDate,
                IncreasePercent = coefficientSettingData?.Value ?? 0,
                BaseDateforMarital = MabnaTakafol_Miladi.Value,
                ExecuteDate = executeDate.Value,
                InterdictTypeId = EnumInterdictType.AnnualSalaryIncrease.Id,
                //increaseSalaryDetails
            };
            foreach (var item in queryEmploymentPersonCurrent)
            {
                var detail = new IncreaseSalaryDetailVM();
                var LastEmployeeInterdict = item.LastEmployeeInterdicts.FirstOrDefault();
                if (LastEmployeeInterdict == null) continue; //اگر حکم قبلی پیدا نشد رد کن
                detail.EmployeeNumber = item.EmployeeNumber;
                detail.FullName = item.NameFamily;
                detail.EmployeeInterdictId = LastEmployeeInterdict.Id;
                detail.JobGroupId = LastEmployeeInterdict.CurrentJobGroupId;
                detail.JobCategoryDefinationId = LastEmployeeInterdict.Chart_JobPosition.Chart_JobIdentity.Chart_JobCategory.JobCategoryDefinationId;
                detail.EmployeeId = item.EmployeeId;
                detail.EmploymentTypeId = item.EmploymentTypeId;
                detail.WorkCityId = item.WorkCityId;
                detail.NumberOfChild = item.FamilyCount;
                int calculateDay = 0;
                if (item.EmploymentDate < model.CalculateStartDate)
                {
                    calculateDay = workCalendar.Count(x => x.YyyyShamsi == CalculateYear);
                }
                else
                {
                    calculateDay = workCalendar.Where(x => x.YyyyShamsi == CalculateYear && x.MiladiDateV1 >= item.EmploymentDate).Count();
                }
                detail.CalculateDay = calculateDay;
                detail.MaritalStatusId = item.MaritalStatusId;
                detail.MarriedDate = item.MarriedDate;
                detail.EmploymentDate = item.EmploymentDate;
                detail.EmploymentTypeTitle = item.EmploymentTypeTitle;

                model.IncreaseSalaryDetails.Add(detail);
            }

            return model;
        }
        public IncreaseSalaryHeader GetLatestIncreaseSalaryHeaderWithRelation(int year)
        {
            var increaseSalaryHeader = _kscHrContext.IncreaseSalaryHeaders.AsNoTracking().FirstOrDefault(x => x.Year == year && x.IsFinal == false);
            return increaseSalaryHeader;
        }




        public async Task<List<IncreaseSalaryAccountEmploymentTypeInterdictDto>> GetInterdictsForIncreaseSalary(IncreaseSalaryHeader increaseSalaryHeader, bool hasdetails, EmployeeSearchModel searchModel)
        {
            var listAccountEmploymentTypeInterdictDto = new List<IncreaseSalaryAccountEmploymentTypeInterdictDto>();

            var IncreaseSalaryDetailQuery = _kscHrContext.IncreaseSalaryDetails
                .Where(x => x.IncreasedSalaryHeaderId == increaseSalaryHeader.Id
                       //&& (x.Employee.EmployeeNumber == "14031246" || x.Employee.EmployeeNumber == "14030061" 
                       //|| x.Employee.EmployeeNumber == "84255"|| x.Employee.EmployeeNumber == "111406")
                       );

            var EmployeeQuery = GetEmployeeForIncreaseSalary();
            if (searchModel != null)
            {
                if (searchModel.EmployeeId > 0)
                    EmployeeQuery = EmployeeQuery.Where(x => x.Id == searchModel.EmployeeId);

                if (!string.IsNullOrEmpty(searchModel.EmployeeNumber))
                    EmployeeQuery = EmployeeQuery.Where(x => x.EmployeeNumber == searchModel.EmployeeNumber);

                if (!string.IsNullOrEmpty(searchModel.Name))
                    EmployeeQuery = EmployeeQuery.Where(x => x.Name.Contains(searchModel.Name));

                if (!string.IsNullOrEmpty(searchModel.Family))
                    EmployeeQuery = EmployeeQuery.Where(x => x.Family.Contains(searchModel.Family));


                var EmployeeIds = EmployeeQuery.Select(x => x.Id).ToList();
                IncreaseSalaryDetailQuery = IncreaseSalaryDetailQuery.Where(x => EmployeeIds.Contains(x.EmployeeId ?? 0));
            }



            var increaseSalarydetails = IncreaseSalaryDetailQuery.AsNoTracking()
               .Include(a => a.EmploymentType)
               .Include(a => a.Rule_EmployeeInterdict)
               .ToList();
            if (increaseSalaryHeader != null)
            {
                var salayDetails = increaseSalarydetails;
                var employmentTypeIds = salayDetails.Select(x => x.EmploymentTypeId).Distinct().ToList();
                var AccountEmploymentTypes = await _kscHrContext.AccountEmploymentType
                    .Where(x => employmentTypeIds.Contains(x.EmploymentTypeId) && x.IsActive == true)
                                 .AsQueryable().AsNoTracking()
                                 .Include(a => a.AccountCode)
                                 // .ThenInclude(a => a.InterdictMaritalSettingDetails)
                                 //.ThenInclude(a => a.InterdictMaritalSetting)
                                 .Where(x => x.AccountCode.IsActive == true)
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
                                    //IsEditablePrice = a.AccountCode.InterdictCategory.IsEditablePrice,
                                    InterdictCategoryId = a.AccountCode.InterdictCategoryId,
                                    InterdictCategoryTitle = a.AccountCode.InterdictCategory.Title,
                                    //InterdictMaritalSettingDetails = a.AccountCode.InterdictMaritalSettingDetails
                                }).ToListAsync();
                var JobPositionIds = salayDetails.Select(x => x.Rule_EmployeeInterdict.JobPositionId).Distinct().ToList();
                var jobpositions = _kscHrContext.Chart_JobPosition
                    .Where(a => JobPositionIds.Any(x => x == a.Id))
                    .Select(x => new JobPositionVM
                    {
                        Id = x.Id,
                        Title = x.Title,
                        MisJobPositionCode = x.MisJobPositionCode,
                        CostCenter = x.CostCenter,
                    }).AsNoTracking().ToList();
                var employeeIds = salayDetails.Select(x => x.Rule_EmployeeInterdict.EmployeeId).Distinct().ToList();
                var employees = _kscHrContext.Employees
                    .Where(a => employeeIds.Any(x => x == a.Id))
                    .Select(x => new EmployeeVM
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Family = x.Family,
                        EmployeeNumber = x.EmployeeNumber,
                        EmploymentDate = x.EmploymentDate,
                    }).AsNoTracking().ToList();
                int ExecuteDateyear = Convert.ToInt32(increaseSalaryHeader.ExecuteDate.GetPersianYear());
                int yearmonth = Convert.ToInt32(increaseSalaryHeader.ExecuteDate.GetYearMonthShamsiByMiladiDate());
                var BasisSalaryItems = await _kscHrContext.BasisSalaryItems
                   .Where(x => employmentTypeIds.Contains(x.EmploymentTypeId) && x.IsConfirmed && x.IsActive &&
                   x.StartDate <= yearmonth && x.EndDate >= yearmonth
                   )
                   .Include(a => a.BasisSalaryItemPerGroups.Where(a => a.IsActive == true))
                   .OrderByDescending(x => x.Id)
                   .AsNoTracking()
                   .ToListAsync();
                var InterdictMaritalSettingDetails = await _kscHrContext.InterdictMaritalSetting
                .Where(x => employmentTypeIds.Contains(x.EmploymentTypeId) && x.IsConfirmed && x.IsActive &&
                x.StartDate <= yearmonth && x.EndDate >= yearmonth
                )
                .Include(a => a.InterdictMaritalSettingDetails.Where(a => a.IsActive == true))
                .OrderByDescending(x => x.Id)
                .AsNoTracking()
                .ToListAsync();

                var coefficientSettings = await _kscHrContext.CoefficientSetting.AsNoTracking()
                    .Where(x => x.IsActive == true && x.Year == increaseSalaryHeader.Year)
                    .ToListAsync();

                var InterdictWeathers = _kscHrContext.InterdictWeather.AsNoTracking().Where(x => x.StartDate <= increaseSalaryHeader.ValidStartDate && x.EndDate >= increaseSalaryHeader.ValidEndDate).ToList();
                var EmployeeInterdictIds = salayDetails.Select(x => x.EmployeeInterdictId).Distinct().ToList();

                var EmployeeInterdictDetails = _kscHrContext.EmployeeInterdictDetails.AsNoTracking().Where(a => EmployeeInterdictIds.Any(x => x == a.EmployeeInterdictId)).Include(a => a.AccountCode).ToList();

                var workCalendar = _kscHrContext.WorkCalendars.AsNoTracking().Where(x => x.YyyyShamsi == ExecuteDateyear - 1).Select(x => new WorkCalendar { DateKey = x.DateKey, MiladiDateV1 = x.MiladiDateV1 }).ToList();

                foreach (var item in salayDetails)
                {
                    var curentjobposition = jobpositions.FirstOrDefault(a => a.Id == item.Rule_EmployeeInterdict.JobPositionId);
                    var curentEmployee = employees.FirstOrDefault(a => a.Id == item.Rule_EmployeeInterdict.EmployeeId);
                    var curentEmployeeInterdictDetails = EmployeeInterdictDetails.Where(a => a.EmployeeInterdictId == item.EmployeeInterdictId).Select(x => new EmployeeInterdictDetailVm
                    {
                        AccountCodeId = x.AccountCodeId,
                        EmployeeInterdictId = x.EmployeeInterdictId,
                        Amount = x.Amount,
                        AccountCode = x.AccountCode
                    }).ToList();
                    var lastYearAmount = curentEmployeeInterdictDetails
                   .FirstOrDefault(x => x.AccountCode.InterdictCategoryId == EnumInterdictCategory.Years.Id);
                    var result = CalculateincreaseSalary(new OutIncreaseSalary()
                    {

                        lastYearAmount = lastYearAmount,
                        workCalendar = workCalendar,
                        AccountEmploymentTypes = AccountEmploymentTypes,
                        BasisSalaryItems = BasisSalaryItems,
                        InterdictMaritalSettingDetails = InterdictMaritalSettingDetails,
                        CoefficientSettings = coefficientSettings,
                        increaseSalaryHeader = increaseSalaryHeader,
                        salarydetail = item,
                        interdictWeathers = InterdictWeathers,
                        curentjobposition = curentjobposition,
                        curentEmployee = curentEmployee,
                        EmployeeInterdictDetails = curentEmployeeInterdictDetails,
                        hasdetails = hasdetails
                    });

                    listAccountEmploymentTypeInterdictDto.Add(result);
                }
            }
            return listAccountEmploymentTypeInterdictDto.OrderBy(x => x.InterdictId).ToList();

        }
        private IncreaseSalaryAccountEmploymentTypeInterdictDto CalculateincreaseSalary(
         OutIncreaseSalary outIncreaseSalary
            )
        {
            var viewmodel = new IncreaseSalaryAccountEmploymentTypeInterdictDto();// دیتای هدر حکم
            try
            {
                viewmodel.InterdictTypeId = outIncreaseSalary.increaseSalaryHeader.InterdictTypeId;
                viewmodel.EmployeeId = outIncreaseSalary.salarydetail.EmployeeId.Value;
                viewmodel.EmployeeNumber = outIncreaseSalary.curentEmployee.EmployeeNumber;
                viewmodel.Name = outIncreaseSalary.curentEmployee.Name;
                viewmodel.Family = outIncreaseSalary.curentEmployee.Family;
                viewmodel.EmploymentDate = outIncreaseSalary.curentEmployee.EmploymentDate;
                viewmodel.MarriedDate = outIncreaseSalary.salarydetail.MarriedDate;
                viewmodel.EmployeeFullName = $"{outIncreaseSalary.curentEmployee.Name} {outIncreaseSalary.curentEmployee.Family}";
                viewmodel.WorkCityId = outIncreaseSalary.salarydetail.WorkCityId;
                viewmodel.EmploymentTypeId = outIncreaseSalary.salarydetail.EmploymentTypeId;
                viewmodel.HardWorkScore = outIncreaseSalary.salarydetail.Rule_EmployeeInterdict.HardWorkScore;
                viewmodel.InterdictDescriptionId = outIncreaseSalary.salarydetail.Rule_EmployeeInterdict.InterdictDescriptionId;
                viewmodel.MeritPercent = outIncreaseSalary.salarydetail.MeritPercent;
                viewmodel.BasijPercent = outIncreaseSalary.salarydetail.BasijPercent;
                viewmodel.AmountCalculate = outIncreaseSalary.salarydetail.AmountCalculate;
                viewmodel.EmploymentTypeTitle = outIncreaseSalary.salarydetail.EmploymentType.Title;
                viewmodel.JobPositionId = outIncreaseSalary.salarydetail.Rule_EmployeeInterdict.JobPositionId;
                viewmodel.CurrentJobGroupId = outIncreaseSalary.salarydetail.Rule_EmployeeInterdict.CurrentJobGroupId.Value;
                viewmodel.RadiationPercentage = outIncreaseSalary.salarydetail.Rule_EmployeeInterdict.RadiationPercentage;
                viewmodel.ReasonJobMovingId = outIncreaseSalary.salarydetail.Rule_EmployeeInterdict.ReasonJobMovingId;
                viewmodel.SpecialLiabilityScore = outIncreaseSalary.salarydetail.Rule_EmployeeInterdict.SpecialLiabilityScore;

                //viewmodel.PromotionId = promotion.Id;
                //viewmodel.IsWantModifyInterdict = promotion.IsWantModifyInterdict;
                viewmodel.InterdictId = outIncreaseSalary.salarydetail.Rule_EmployeeInterdict.Id;
                viewmodel.ExecuteDateLatest = outIncreaseSalary.salarydetail.Rule_EmployeeInterdict.ExecuteDate;
                viewmodel.ExecuteDate = outIncreaseSalary.increaseSalaryHeader.ExecuteDate.Date;
                viewmodel.InterdictDescriptionId = outIncreaseSalary.salarydetail.Rule_EmployeeInterdict.InterdictDescriptionId;
                viewmodel.JobPositionId = outIncreaseSalary.salarydetail.Rule_EmployeeInterdict.JobPositionId;

                viewmodel.InterdictNumber = $"434/{outIncreaseSalary.curentEmployee.EmployeeNumber}/1";

                if (outIncreaseSalary.salarydetail.EmploymentTypeId == EnumEmploymentType.Gharardadi.Id)
                {
                    viewmodel.InterdictStartDate = outIncreaseSalary.increaseSalaryHeader.ExecuteDate.Date;// مطابق ExecuteDate
                    viewmodel.InterdictEndDate = outIncreaseSalary.increaseSalaryHeader.Year.GetPersianYearStartAndEndDates().EndDate; // تاریخ آخر سال
                }
                //تاریخ اول سال حکم داشته باشد چک شود
                if (outIncreaseSalary.salarydetail.Rule_EmployeeInterdict.ExecuteDate.HasValue == true &&
                    outIncreaseSalary.increaseSalaryHeader.ExecuteDate.Date == outIncreaseSalary.salarydetail.Rule_EmployeeInterdict.ExecuteDate?.Date)
                {//اگر تاریخ شروع آخرین حکم فرد مطابق تاریخ اول سال باشد 
                    #region ایجاد شماره حکم
                    var latestInterdictNumber = outIncreaseSalary.salarydetail.Rule_EmployeeInterdict.InterdictNumber;
                    var employeeNumber = outIncreaseSalary.curentEmployee.EmployeeNumber;
                    var newInterdictNumber = Ksc.HR.Share.General.Utility.GenerationInterdictNumber(employeeNumber, latestInterdictNumber);
                    #endregion
                    viewmodel.InterdictNumber = newInterdictNumber;
                    viewmodel.ExecuteDate = outIncreaseSalary.increaseSalaryHeader.ExecuteDate.Date.AddDays(1);
                    if (outIncreaseSalary.salarydetail.EmploymentTypeId == EnumEmploymentType.Gharardadi.Id)
                    {
                        //viewmodel.InterdictStartDate = outIncreaseSalary.increaseSalaryHeader.ExecuteDate.Date.AddDays(1);
                        viewmodel.InterdictEndDate = outIncreaseSalary.salarydetail.Rule_EmployeeInterdict.InterdictEndDate; // تاریخ پایان اخرین قرارداد
                    }
                }




                ////اینجا
                //viewmodel.NewBasisPrice = (LastInterdict.SumPriceBasisSalary != null) ? LastInterdict.SumPriceBasisSalary.Value : 0;
                //viewmodel.NewDiffBasisPrice = (LastInterdict.SumPriceIndependentBasisSalary != null) ? LastInterdict.SumPriceIndependentBasisSalary.Value : 0;
                //
                viewmodel.NewJobGroupId = outIncreaseSalary.salarydetail.Rule_EmployeeInterdict.CurrentJobGroupId;
                //viewmodel.PromotionDate = promotion.PromotionDate;

                viewmodel.SupervisionScore = outIncreaseSalary.salarydetail.Rule_EmployeeInterdict.SupervisionScore;
                viewmodel.WorkCityId = outIncreaseSalary.salarydetail.WorkCityId;
                //viewmodel.PromotionDateYearMonth = promotion.PromotionDateYearMonth;
                viewmodel.FamilyCount = outIncreaseSalary.salarydetail.NumberOfChild ?? 0;
                //viewmodel.IsWantModify = IsWantModify;
                viewmodel.RadiationPercentage = outIncreaseSalary.salarydetail.Rule_EmployeeInterdict.RadiationPercentage ?? 0;
                //viewmodel.LastIssuranceDate = IssuranceDateNew;

                //viewmodel.IssuranceDateInterDictAddOneDays = IssuranceDateNew == day1 ? day1.AddDays(1) : day1;

                //viewmodel.ExcuteDateInterDictAddOneDays =
                //                                        ExecuteDateNew >= promotion.PromotionDate.Date ?
                //                                        ExecuteDateNew.AddDays(1) :
                //                                        promotion.PromotionDate.Date;


                //var jobpositionEmployee = ChartJobPositions;
                var jobCategoryDefinationId = outIncreaseSalary.salarydetail.JobCategoryDefinationId;
                viewmodel.JobPositionTitle = outIncreaseSalary.curentjobposition.MisJobPositionCode + "-" + outIncreaseSalary.curentjobposition.Title;
                viewmodel.CostCenter = outIncreaseSalary.curentjobposition.CostCenter;
                //viewmodel.jobCategoryDefinationTitle = jobCategoryDefination.Title;
                var listAccountEmploymentTypeInterdict = new List<AccountEmploymentTypeInterdictDto>();// دیتای جزییات حکم

                decimal TableFactor = 0;
                long NewPriceyears = 0;
                long NewPriceMerit = 0;

                var GetLastBasisSalaryItemByYearMonth = outIncreaseSalary.BasisSalaryItems
                    .FirstOrDefault(x => x.EmploymentTypeId == outIncreaseSalary.salarydetail.EmploymentTypeId
                                    );
                //ضریب جدول
                TableFactor = GetLastBasisSalaryItemByYearMonth == null ? 0 : (decimal)(GetLastBasisSalaryItemByYearMonth.TableCoefficient);


                var ListAccountTypeByEmployeementTypeId = outIncreaseSalary.AccountEmploymentTypes
                    .Where(a => a.EmploymentTypeId == viewmodel.EmploymentTypeId).ToList();


                List<int> enummaritalsetting = [EnumInterdictCategory.ChildAmount.Id, EnumInterdictCategory.Married.Id, EnumInterdictCategory.Grocery.Id,
                    EnumInterdictCategory.House.Id,EnumInterdictCategory.Tashilat.Id,EnumInterdictCategory.Bon.Id
                    ];
                //[12, 13, 14, 15, 21, 22]; ;
                if (outIncreaseSalary.hasdetails)
                    foreach (var item in ListAccountTypeByEmployeementTypeId.OrderBy(a => a.AccountCode.InterdictCategoryId))
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
                        var lastEmployeeInterdictDetails = outIncreaseSalary.EmployeeInterdictDetails.FirstOrDefault(x => x.AccountCodeId == item.AccountCodeId);
                        accountEmploymentTypeInterdict.PaymentpriceLatest = lastEmployeeInterdictDetails != null ? lastEmployeeInterdictDetails.Amount : 0;
                        //accountEmploymentTypeInterdict.Paymentprice = lastEmployeeInterdictDetails != null ? lastEmployeeInterdictDetails.Amount : 0;
                        var basisSalaryItemPerGroups = GetLastBasisSalaryItemByYearMonth.BasisSalaryItemPerGroups.FirstOrDefault(a => a.WorkGroupId == outIncreaseSalary.salarydetail?.Rule_EmployeeInterdict?.CurrentJobGroupId);
                        var NewjobGroupAccount = basisSalaryItemPerGroups?.GroupSalaryAmount ?? 0;

                        if (item.InterdictCategoryId == EnumInterdictCategory.GroupSalaryAmount.Id)
                        {

                            accountEmploymentTypeInterdict.Paymentprice = NewjobGroupAccount;
                        }
                        //// مزد سنوات
                        else if (item.AccountCode.InterdictCategoryId == EnumInterdictCategory.Years.Id)
                        {
                            ///(مزد سنوات سال قبل * درصد افزایش حقوق )+ (پایه سنوات گروه شغلی * 30)
                            var baseAmountYears = basisSalaryItemPerGroups?.BaseAmountYears ?? 0;
                            ///نسبت به مدت اشتغال فرد در سال قبل
                            var calculatebaseAmountYears = (long?)(((decimal)baseAmountYears / outIncreaseSalary.workCalendar.Count()) * outIncreaseSalary.salarydetail?.CalculateDay);
                            accountEmploymentTypeInterdict.Paymentprice = Convert.ToInt64((outIncreaseSalary.lastYearAmount?.Amount ?? 0) * outIncreaseSalary.increaseSalaryHeader?.IncreasePercent) + ((calculatebaseAmountYears ?? 0) * 30);
                            NewPriceyears = accountEmploymentTypeInterdict.Paymentprice;
                        }
                        //// حق شایستگی
                        else if (item.InterdictCategoryId == EnumInterdictCategory.Merit.Id)
                        {
                            ////(حق شایستگی سال قبل* درصد افزایش حقوق )+
                            //// ((مزد گروه شغل جدید* ضریب شایستگی)/ 100  )
                            var accountCodeId = ListAccountTypeByEmployeementTypeId.FirstOrDefault(item => item.InterdictCategoryId == EnumInterdictCategory.Merit.Id)?.AccountCodeId ?? null;

                            var lastAmount = outIncreaseSalary?.EmployeeInterdictDetails.FirstOrDefault(x => x.AccountCodeId == accountCodeId)?.Amount ?? 0;

                            ////     نسبت به مدت اشتغال فرد در سال قبل
                            var calculateNewjobGroupAccount = (long?)(((decimal)NewjobGroupAccount / outIncreaseSalary.workCalendar.Count()) * outIncreaseSalary.salarydetail?.CalculateDay);

                            accountEmploymentTypeInterdict.Paymentprice = Convert.ToInt64(
                              (lastAmount * outIncreaseSalary?.increaseSalaryHeader?.IncreasePercent) +
                             ((calculateNewjobGroupAccount * (outIncreaseSalary?.salarydetail?.MeritPercent ?? 0)) / 100));

                            NewPriceMerit = accountEmploymentTypeInterdict.Paymentprice;
                        }
                        //// "مسئولیت خاص"
                        else if (item.InterdictCategoryId == EnumInterdictCategory.SpecialLiability.Id)
                        {
                            var factorSpecialLiability = outIncreaseSalary.CoefficientSettings.FirstOrDefault(a => a.CoefficientId == EnumRuleCoefficient.SpecialLiability.Id)?.Value;
                            // امتیاز مسئولیت *ضریب جدول* عدد 1.76 * 30
                            var amount = (outIncreaseSalary.salarydetail.Rule_EmployeeInterdict.SpecialLiabilityScore ?? 0) * Convert.ToDouble(TableFactor) * Convert.ToDouble(factorSpecialLiability) * 30;
                            accountEmploymentTypeInterdict.Paymentprice = (long)amount;
                        }
                        //مقطع تحصيلي بالاتر
                        else if (item.InterdictCategoryId == EnumInterdictCategory.HigherDegree.Id)
                        {
                            var accountCodeId = ListAccountTypeByEmployeementTypeId.FirstOrDefault(item => item.InterdictCategoryId == EnumInterdictCategory.HigherDegree.Id)?.AccountCodeId ?? null;
                            //مقطع تحصيلي بالاتر * ضریب افزایش سال
                            var lastAmount = outIncreaseSalary?.EmployeeInterdictDetails?.FirstOrDefault(x => x.AccountCodeId == accountCodeId)?.Amount ?? 0;
                            accountEmploymentTypeInterdict.Paymentprice = Convert.ToInt64(lastAmount * outIncreaseSalary.increaseSalaryHeader.IncreasePercent);
                        }
                        ////   حق ایثار گری
                        else if (item.InterdictCategoryId == EnumInterdictCategory.Sacrifice.Id)
                        {
                            var accountCodeId = ListAccountTypeByEmployeementTypeId?.FirstOrDefault(item => item.InterdictCategoryId == EnumInterdictCategory.Sacrifice.Id)?.AccountCodeId ?? null;
                            //(حق ایثار گری قبلی *  ضریب افزایش سال (
                            //+(درصد بسیج سال جدید* مزد گروه شغل گروه یک(1)  )

                            var lastAmount = outIncreaseSalary.EmployeeInterdictDetails?.FirstOrDefault(x => x.AccountCodeId == accountCodeId)?.Amount ?? 0;
                            var groupSalaryAmount1 = GetLastBasisSalaryItemByYearMonth?.BasisSalaryItemPerGroups?.FirstOrDefault(a => a.WorkGroupId == 1)?.GroupSalaryAmount;
                            //درصد بسیج برای پرسنلی که نوع استخدام آنها 4 نباشد  لحاظ می شود 
                            var basijPercent_d = outIncreaseSalary?.salarydetail?.EmploymentTypeId != EnumEmploymentType.Gharardadi.Id ? outIncreaseSalary?.salarydetail?.BasijPercent : 0;
                            var basijPercent = basijPercent_d / 100;
                            accountEmploymentTypeInterdict.Paymentprice = Convert.ToInt64(
                                (lastAmount * outIncreaseSalary?.increaseSalaryHeader?.IncreasePercent)
                                + (basijPercent * groupSalaryAmount1));
                        }
                        ////  "سرپرستی"
                        else if (item.InterdictCategoryId == EnumInterdictCategory.Supervision.Id)
                        {
                            var factorSupervision = outIncreaseSalary.CoefficientSettings.FirstOrDefault(a => a.CoefficientId == EnumRuleCoefficient.SupervisionScor.Id)?.Value;
                            // امتیاز سرپرستی * ضریب جدول *  عدد 1.76   * 30
                            var amount = (outIncreaseSalary?.salarydetail?.Rule_EmployeeInterdict?.SupervisionScore ?? 0) * Convert.ToDouble(TableFactor) * Convert.ToDouble(factorSupervision) * 30;
                            accountEmploymentTypeInterdict.Paymentprice = (long)amount;
                        }
                        /////   "شرایط کار"
                        else if (item.InterdictCategoryId == EnumInterdictCategory.HardWork.Id)
                        {
                            var factorHardWork = outIncreaseSalary.CoefficientSettings.FirstOrDefault(a => a.CoefficientId == EnumRuleCoefficient.HardWorkFactor.Id)?.Value;
                            // امتیاز سختی کار * ضریب جدول * عدد 3.1   * 30
                            var amount = (outIncreaseSalary?.salarydetail?.Rule_EmployeeInterdict.HardWorkScore ?? 0) * Convert.ToDouble(TableFactor) * Convert.ToDouble(factorHardWork) * 30;
                            accountEmploymentTypeInterdict.Paymentprice = (long)amount;
                        }
                        ////   فوق العاده افزايش سنواتی بسيج
                        else if (item.InterdictCategoryId == EnumInterdictCategory.ExtraYearsBasij.Id)
                        {
                            var accountCodeId = ListAccountTypeByEmployeementTypeId.FirstOrDefault(item => item.InterdictCategoryId == EnumInterdictCategory.ExtraYearsBasij.Id)?.AccountCodeId ?? null;
                            //(حق ایثار گری قبلی *  ضریب افزایش سال (
                            //+(درصد بسیج سال جدید* مزد گروه شغل گروه یک(1)  )

                            var lastAmount = outIncreaseSalary?.EmployeeInterdictDetails?.FirstOrDefault(x => x.AccountCodeId == accountCodeId)?.Amount ?? 0;
                            var groupSalaryAmount1 = GetLastBasisSalaryItemByYearMonth?.BasisSalaryItemPerGroups?.FirstOrDefault(a => a.WorkGroupId == 1)?.GroupSalaryAmount;
                            //درصد بسیج برای پرسنلی که نوع استخدام آنها 4 باشد  لحاظ می شود 
                            var basijPercent_d = outIncreaseSalary?.salarydetail?.EmploymentTypeId == EnumEmploymentType.Gharardadi.Id ? outIncreaseSalary?.salarydetail?.BasijPercent : 0;
                            var basijPercent = basijPercent_d / 100;
                            accountEmploymentTypeInterdict.Paymentprice = Convert.ToInt64(
                                (lastAmount * outIncreaseSalary?.increaseSalaryHeader?.IncreasePercent)
                                + (basijPercent * groupSalaryAmount1));
                        }
                        //// فوق العاده حق اشعه
                        else if (item.InterdictCategoryId == EnumInterdictCategory.RadiationPercentage.Id)
                        {
                            //درصد اشعه مربوط به فرد * (مزد گروه شغل جدید+ مزد سنوات جدید)
                            //مزد گروه شغل
                            var GroupSalaryAmount = NewjobGroupAccount;

                            ////سنوات
                            //var LastYearsAmount = NewPriceyears;

                            //درصد اشعه
                            var radiationPercentage = outIncreaseSalary?.salarydetail.Rule_EmployeeInterdict?.RadiationPercentage ?? 0;
                            if (radiationPercentage != 0)
                            {
                                accountEmploymentTypeInterdict.Paymentprice = GetRadiationPercentage(radiationPercentage, GroupSalaryAmount, NewPriceyears);

                            }

                        }
                        //فوق العاده بيماري زائی
                        else if (item.InterdictCategoryId == EnumInterdictCategory.Disease.Id)
                        {
                            var accountCodeId = ListAccountTypeByEmployeementTypeId.FirstOrDefault(item => item.InterdictCategoryId == EnumInterdictCategory.Disease.Id)?.AccountCodeId ?? null;
                            //بیماری زائی قبلی *   ضریب افزایش سال
                            var lastAmount = outIncreaseSalary?.EmployeeInterdictDetails?.FirstOrDefault(x => x.AccountCodeId == accountCodeId)?.Amount ?? 0;
                            accountEmploymentTypeInterdict.Paymentprice = Convert.ToInt64(lastAmount * outIncreaseSalary.increaseSalaryHeader.IncreasePercent);
                        }
                        /////  حق مسکن-حق اولاد - بن خوارو بار -بن نقدی - تسهیلات رفاهی - حق تاهل
                        else if (enummaritalsetting.Contains(item.InterdictCategoryId.Value))
                        {
                            var AccountTypeByEmployeementType = ListAccountTypeByEmployeementTypeId.FirstOrDefault(i => i.InterdictCategoryId == item.InterdictCategoryId.Value);
                            var amount = outIncreaseSalary.InterdictMaritalSettingDetails?
                                .FirstOrDefault(a => a.EmploymentTypeId == AccountTypeByEmployeementType.EmploymentTypeId)
                                .InterdictMaritalSettingDetails?
                                .FirstOrDefault(m => m.AccountCodeId == AccountTypeByEmployeementType.AccountCodeId
                                && (m.MaritalStatusId == null ? true : m.MaritalStatusId == outIncreaseSalary.salarydetail?.MaritalStatusId))?.Amount ?? null;

                            var selectedInterdictCategory = new List<int> { EnumInterdictCategory.ChildAmount.Id, EnumInterdictCategory.Married.Id, EnumInterdictCategory.Grocery.Id };
                            accountEmploymentTypeInterdict.Paymentprice = amount ?? 0;

                            if (selectedInterdictCategory.Contains(item.InterdictCategoryId.Value) == false)
                                accountEmploymentTypeInterdict.Paymentprice = amount ?? 0;
                            else
                            {
                                if (EnumInterdictCategory.ChildAmount.Id == item.InterdictCategoryId.Value)
                                {
                                    accountEmploymentTypeInterdict.Paymentprice = accountEmploymentTypeInterdict.Paymentprice * (outIncreaseSalary?.salarydetail?.NumberOfChild ?? 0);
                                }
                                else if (EnumInterdictCategory.Married.Id == item.InterdictCategoryId.Value)
                                {
                                    // افرادی که تاریخ ازدواجشان کوچکتر مساوی 16 ماه جاری باشد بایستی بررسی شوند
                                    // یا تاریخ ازدواج ندارند
                                    if ((outIncreaseSalary.salarydetail.MarriedDate >= outIncreaseSalary?.increaseSalaryHeader?.BaseDateforMarital || outIncreaseSalary?.salarydetail?.MarriedDate == null))
                                    {
                                        accountEmploymentTypeInterdict.Paymentprice = 0;
                                    }
                                }
                                //else if (EnumInterdictCategory.Grocery.Id == item.InterdictCategoryId.Value)
                                //{

                                //    //if (!(salarydetail.MarriedDate <= salarydetail.Rule_IncreaseSalaryHeader.BaseDateforMarital || salarydetail.MarriedDate == null))
                                //    //{
                                //    //    accountEmploymentTypeInterdict.Paymentprice = 800;
                                //    //}
                                //}
                            }
                        }
                        ////  حق مدرک تحصیلی مازاد
                        else if (item.InterdictCategoryId == EnumInterdictCategory.ExtraDegree.Id)
                        {
                            var accountCodeId = ListAccountTypeByEmployeementTypeId.FirstOrDefault(item => item.InterdictCategoryId == EnumInterdictCategory.ExtraDegree.Id)?.AccountCodeId ?? null;
                            //حق مدرک مازاد قبلی *  ضریب افزایش سال
                            var lastAmount = outIncreaseSalary?.EmployeeInterdictDetails?.FirstOrDefault(x => x.AccountCodeId == accountCodeId)?.Amount ?? 0;
                            accountEmploymentTypeInterdict.Paymentprice = Convert.ToInt64(lastAmount * outIncreaseSalary?.increaseSalaryHeader?.IncreasePercent);
                        }
                        //// فوق العاده ايثارگری
                        else if (item.InterdictCategoryId == EnumInterdictCategory.ExtraSacrifice.Id)
                        {
                            var accountCodeId = ListAccountTypeByEmployeementTypeId.FirstOrDefault(item => item.InterdictCategoryId == EnumInterdictCategory.ExtraSacrifice.Id)?.AccountCodeId ?? null;
                            var lastAmount = outIncreaseSalary?.EmployeeInterdictDetails?.FirstOrDefault(x => x.AccountCodeId == accountCodeId)?.Amount ?? 0;
                            /////افرادی که قبلا این مبلغ میگرفتند مبلغ جدید برایشان انتساب داده میشود
                            if (lastAmount != 0)
                            {
                                var priceExtraSacrifice = outIncreaseSalary.CoefficientSettings.FirstOrDefault(a => a.CoefficientId == EnumRuleCoefficient.ExtraSacrifice.Id)?.Value;
                                accountEmploymentTypeInterdict.Paymentprice = Convert.ToInt64(priceExtraSacrifice);
                            }
                        }
                        ////حق ماندگاری
                        else if (item.InterdictCategoryId == EnumInterdictCategory.Durability.Id)
                        {

                            var accountCodeId = ListAccountTypeByEmployeementTypeId.FirstOrDefault(item => item.InterdictCategoryId == EnumInterdictCategory.Durability.Id)?.AccountCodeId ?? null;
                            //(حق ماندگاری سال قبل * درصد افزایش حقوق )+ 
                            //مبلغ محاسبه شده ماندگاری جدید
                            var lastAmount = outIncreaseSalary.EmployeeInterdictDetails?.FirstOrDefault(x => x.AccountCodeId == accountCodeId)?.Amount ?? 0;
                            accountEmploymentTypeInterdict.Paymentprice = Convert.ToInt64(
                            (lastAmount * outIncreaseSalary?.increaseSalaryHeader?.IncreasePercent) +
                            (outIncreaseSalary.salarydetail?.AmountCalculate ?? 0));
                        }
                        ////فوق العاده بدي آب و هوا
                        else if (item.InterdictCategoryId == EnumInterdictCategory.BadWeader.Id)
                        {
                            //(مزد گروه جدید + مزد سنوات جدید + حق شایستگی جدید  ) * ضریب بدی آب و هوا  

                            var weatherFactor = outIncreaseSalary.interdictWeathers?.FirstOrDefault(x => x.WorkCityId == outIncreaseSalary.salarydetail.WorkCityId && x.JobCategoryDefinationId == outIncreaseSalary.salarydetail.JobCategoryDefinationId)?.Value ?? 0;
                            accountEmploymentTypeInterdict.Paymentprice = (long)((NewjobGroupAccount + NewPriceyears + NewPriceMerit) * weatherFactor);

                        }
                        ///ارتقا افقی
                        else if (item.InterdictCategoryId == EnumInterdictCategory.HorizontalUpgrade.Id)
                        {
                            var accountCodeId = ListAccountTypeByEmployeementTypeId.FirstOrDefault(item => item.InterdictCategoryId == EnumInterdictCategory.HorizontalUpgrade.Id)?.AccountCodeId ?? null;
                            //ارتقا افقی قبلی *  ضریب افزایش سال
                            var lastAmount = outIncreaseSalary?.EmployeeInterdictDetails?.FirstOrDefault(x => x.AccountCodeId == accountCodeId)?.Amount ?? 0;
                            accountEmploymentTypeInterdict.Paymentprice = Convert.ToInt64(lastAmount * outIncreaseSalary.increaseSalaryHeader?.IncreasePercent);
                        }


                        //if (accountEmploymentTypeInterdict.Paymentprice == 0 && accountEmploymentTypeInterdict.PaymentpriceLatest == 0)
                        //{
                        //    continue;
                        //}
                        ////بعدا از طریق کلاسش که پراپرتی IsBase دارم تصیمیم گیری میکنم
                        //if (accountEmploymentTypeInterdict.IsBase == true)
                        //{
                        //    viewmodel.OldSumBase += accountEmploymentTypeInterdict.Paymentprice;
                        //    viewmodel.OldSum += accountEmploymentTypeInterdict.Paymentprice;
                        //}
                        //else
                        //{
                        //    viewmodel.OldSumOtherBase += accountEmploymentTypeInterdict.Paymentprice;
                        //    viewmodel.OldSum += accountEmploymentTypeInterdict.Paymentprice;
                        //}
                        ////listAccountEmploymentTypeInterdict.Add();
                        viewmodel.EmploymentInterdictDetails.Add(accountEmploymentTypeInterdict);

                    }

                //foreach (var i in listAccountEmploymentTypeInterdict)
                //{
                //    if (i.IsBase == true)
                //    {
                //        viewmodel.OldSumBase += i.Paymentprice;
                //        viewmodel.OldSum += i.Paymentprice;
                //    }
                //    else
                //    {
                //        viewmodel.OldSumOtherBase += i.Paymentprice;
                //        viewmodel.OldSum += i.Paymentprice;
                //    }
                //}
                //if (viewmodel.EmploymentInterdictDetails != null)
                //{
                //    viewmodel.EmploymentInterdictDetails = viewmodel.EmploymentInterdictDetails.OrderBy(x => x.InterdictId).ToList();

                //}

            }

            catch (Exception ex)
            {

            }

            return viewmodel;



        }
        public class OutIncreaseSalary
        {
            public EmployeeInterdictDetailVm lastYearAmount { set; get; }
            public List<WorkCalendar> workCalendar { set; get; }

            public List<AccountEmployeeCalssModel> AccountEmploymentTypes { set; get; }
            public List<BasisSalaryItem> BasisSalaryItems { set; get; }
            public List<CoefficientSetting> CoefficientSettings { set; get; }
            public IncreaseSalaryHeader increaseSalaryHeader { set; get; }
            public IncreaseSalaryDetail salarydetail { set; get; }
            public List<InterdictWeather> interdictWeathers { set; get; }
            public JobPositionVM curentjobposition { set; get; }
            public EmployeeVM curentEmployee { set; get; }
            public List<EmployeeInterdictDetailVm> EmployeeInterdictDetails { set; get; }
            public bool hasdetails { set; get; }
            public List<InterdictMaritalSetting> InterdictMaritalSettingDetails { get; set; }
        }
        public long GetRadiationPercentage(double radiationPercentage, decimal GroupJob, decimal years)
        {
            var d = Convert.ToDecimal(radiationPercentage);
            var result = ((GroupJob + years) * d) / 100;
            var finalResult = (long)Math.Floor(result);
            return finalResult;
        }

        public class JobPositionVM
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string MisJobPositionCode { get; internal set; }
            public decimal? CostCenter { get; set; }
        }

        public class EmployeeVM
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Family { get; set; }
            public string EmployeeNumber { get; internal set; }
            public DateTime? EmploymentDate { get; internal set; }
        }

        public class EmployeeInterdictDetailVm
        {
            public int EmployeeInterdictId { get; internal set; }
            public int AccountCodeId { get; set; }
            public long Amount { get; internal set; }
            public AccountCode AccountCode { get; internal set; }
        }
        #endregion

    }
}
