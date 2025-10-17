using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Emp;
using Ksc.HR.Domain.Entities.Rule;
using Ksc.HR.Domain.Repositories.Rule;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Ksc.HR.Share.General;
using Ksc.HR.Domain.Entities.EmployeeBase;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Repositories;

namespace Ksc.HR.Data.Persistant.Repositories.Rule
{
    public class EmployeePromotionSuggestionRepository : EfRepository<EmployeePromotion, int>, IEmployeePromotionSuggestionRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeePromotionSuggestionRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public List<EmployeePromotion> GetCurrentMonthEmployeeiesPredictionDateUpgrate(List<int> employeeIds, int YearMonth)
        {

            var result = new List<EmployeePromotion>();
            var getEmployee = new List<Tuple<int, DateTime>>();
            var isarStatusIds = new List<int>() { 10, 11, 12, 13, 14, 15, 16, 17, 21, 22, 23, 24 };
            string Errorpath = @"D:/مشترک/employeePromotion_Error.txt";
            var workcalendar = _kscHrContext.WorkCalendars.OrderBy(a=>a.MiladiDateV1).FirstOrDefault(a => a.YearMonthV1 == YearMonth).MiladiDateV1;
      
            var EmployeeInterdictJobPosition = _kscHrContext.EmployeeInterdicts
                .Where(a => a.InterdictTypeId == 3 && a.PortalFinalConfirmFlag == false)
                .Where(a=>a.IssuanceDate>= workcalendar)
                .Select(a => a.EmployeeId.Value).ToList();
            employeeIds = employeeIds.Except(EmployeeInterdictJobPosition).ToList();
            var employeeQuery = _kscHrContext.Employees
                .AsQueryable().AsNoTracking()
                .Where(a => employeeIds.Any(x => x == a.Id))
                .Include(a => a.EmployeeInterdicts)
                .Include(a => a.EmployeePercentMeritHistories).ThenInclude(a => a.MeritRating)
                .Include(a => a.EmployeeDisConnections).ThenInclude(a => a.DisConnectionType)
                .Include(a => a.EmployeeEducationDegrees).ThenInclude(a => a.Education)
                .Include(a => a.EmployeeHistoryTypeEmployements).ThenInclude(a => a.HistoryTypeEmployement)
                .Include(a => a.EmployeeDateInformations).Select(a => new
                {
                    a.Id,
                    a.EmployeeNumber,
                    IsIsar = isarStatusIds.Any(x => x == a.IsarStatusId),

                    MeritRatingCoefficint = a.EmployeePercentMeritHistories
                    .OrderByDescending(a => a.Year).Take(3)
                   
                    .Select(a => a.MeritRating.Coefficint),

                    a.EmploymentTypeId,
                    EmployeeDisConnections = a.EmployeeDisConnections.ToList(),
                    EmployeeEducationDegrees = a.EmployeeEducationDegrees.OrderByDescending(a => a.InsertDate).FirstOrDefault(),
                    EmployeeHistoryTypeEmployements = a.EmployeePercentMeritHistories.OrderByDescending(a => a.Year).FirstOrDefault(),
                    EmployeeDateInformations = a.EmployeeDateInformations.FirstOrDefault(),

                    a.JobPositionId,
                    LastEmployeeInterDicts = a.EmployeeInterdicts.OrderByDescending(x => x.ExecuteDate).FirstOrDefault(),
                    LastEmployeeChangeJobPosition = a.EmployeeInterdicts.OrderBy(x => x.ExecuteDate).FirstOrDefault(x => x.JobPositionId == a.JobPositionId),

                    a.EmploymentDate
                })
                .ToList();
            var MeritRating = _kscHrContext.MeritRatings.ToList();

            //var selectAllEmployeeLastInterDictJobposition = employeeQuery.Where(a => a.LastEmployeeChangeJobPosition != null).Select(a => new
            //{
            //    a.LastEmployeeChangeJobPosition.Id,
            //    a.LastEmployeeChangeJobPosition.EmployeeId,
            //    a.LastEmployeeChangeJobPosition.ExecuteDate,
            //    a.LastEmployeeChangeJobPosition.InterdictTypeId,
            //    a.LastEmployeeChangeJobPosition.CurrentJobGroupId,
            //}).ToList();
            //var employeeInterDicts = _kscHrContext.EmployeeInterdicts.Where(a => employeeIds.Contains(a.EmployeeId.Value)).AsNoTracking().ToList();
            ////برای بررسی اختلاف گروه
            //var allPrevLastJobpositionInterdicts = (from EmployeeInterdicts in employeeInterDicts
            //                                        join AllLast in selectAllEmployeeLastInterDictJobposition on EmployeeInterdicts.EmployeeId equals AllLast.EmployeeId
            //                                        orderby EmployeeInterdicts.ExecuteDate descending
            //                                        where EmployeeInterdicts.ExecuteDate < AllLast.ExecuteDate

            //                                        select new
            //                                        {
            //                                            EmployeeInterdicts.JobPositionId,
            //                                            EmployeeInterdicts.Id,
            //                                            EmployeeInterdicts.EmployeeId,
            //                                            EmployeeInterdicts.CurrentJobGroupId,
            //                                            EmployeeInterdicts.ExecuteDate
            //                                        }).ToList();




            //    _kscHrContext.EmployeeInterdicts.Where(a=> employeeIds.Contains(a.EmployeeId.Value))

            //    .Where(a => 
            //selectAllEmployeeLastInterDictJobposition.Any(x =>a.EmployeeId == x.Id && a.ExecuteDate < x.ExecuteDate))
            //        .OrderByDescending(a => a.ExecuteDate).Select(a => new
            //        {
            //            a.JobPositionId,
            //            a.Id,
            //            a.EmployeeId,
            //            a.CurrentJobGroupId,
            //            a.ExecuteDate
            //        }).ToList();



            var LastJobPositionsEmployees = _kscHrContext.EmployeeJobPositions.Where(a => employeeIds.Contains(a.EmployeeId))
                   .OrderByDescending(a => a.StartDate)
                    .Include(a => a.Chart_JobPosition).ThenInclude(a => a.Chart_JobIdentity)
                    .ThenInclude(a => a.Chart_JobCategory).ThenInclude(a => a.Chart_JobCategoryDefination)
                    .Include(a => a.Chart_JobPosition).ThenInclude(a => a.Chart_JobPositionFields).ThenInclude(a => a.StudyField).AsNoTracking()
                .ToList();

            var allJobPositionCategoryEducation = _kscHrContext.Chart_JobCategoryEducation.AsNoTracking().ToList();
            var selectYear = YearMonth.ToString().Substring(0, 4);
            var selectMonth = YearMonth.ToString().Substring(4, 2);
            var runStepper = $"{selectYear}/{selectMonth}/2";
            var allChart_JobCategoryEducations = _kscHrContext.Chart_JobCategoryEducation
                .Include(a=>a.EducationCategory)
                .Include(a=>a.Chart_JobCategory)
                .ThenInclude(a=>a.Chart_JobCategoryDefination)

                .AsNoTracking().ToList();

            var RollCallDefinitionsIds = new List<int>() { 54, 64 };

            //var monthTimeSheetRollCall = _kscHrContext.MonthTimeSheetRollCalls.AsNoTracking()
            //    .Where(c => c.IsActive && RollCallDefinitionsIds.Contains(c.RollCallDefinitionId)).Include(c => c.MonthTimeSheet)
            //    .Where(c => employeeIds.Any(x => x == c.MonthTimeSheet.EmployeeId)).Select(a => new
            //    {
            //        a.MonthTimeSheet.EmployeeId,
            //        a.RollCallDefinitionId,
            //        a.DurationInMinut
            //    }).ToList();

            var counter = 0;
            foreach (var employee in employeeQuery)
            {
                try
                {
                    var employeenumber = employee.EmployeeNumber;
                    counter++;
                    if (employee.EmployeeEducationDegrees == null)
                    {
                        //var line = $"{employeenumber} ,   اطلاعات مدرک ندارد";
                        //System.IO.File.AppendAllText(Errorpath, line.Trim() + Environment.NewLine);


                        continue;
                    };//مدرک تحصیلی تعریف نشده است
                    if (employee.EmployeeEducationDegrees.Education == null)
                    {
                        //var line = $"{employeenumber} ,   اطلاعات مدرک ندارد";
                        //System.IO.File.AppendAllText(Errorpath, line.Trim() + Environment.NewLine);


                        continue;
                    }; ;

                    var lastEducationCategoryId = employee.EmployeeEducationDegrees.Education.CategoryId;
                    if (employee.IsIsar) lastEducationCategoryId++;
                    var Get2StepJobPosition = LastJobPositionsEmployees.Where(a => a.EmployeeId == employee.Id)
                        .OrderByDescending(a => a.StartDate).Take(2).ToList();
                    var LastJobPositions = Get2StepJobPosition.Where(a => a.IsActive).FirstOrDefault();
                    var PrevPost = Get2StepJobPosition.Where(a => a.IsActive == false).OrderBy(a => a.StartDate).FirstOrDefault();
                    if (LastJobPositions == null)
                    {
                        //var line = $"{employeenumber} ,   اطلاعات مدرک ندارد";
                        //System.IO.File.AppendAllText(Errorpath, line.Trim() + Environment.NewLine);


                        continue;
                    };

                    if (LastJobPositions.Chart_JobPosition.Chart_JobIdentity == null)
                    {
                        //var line = $"{employeenumber} ,   اطلاعات مدرک ندارد";
                        //System.IO.File.AppendAllText(Errorpath, line.Trim() + Environment.NewLine);


                        continue;
                    };




                    var IsChengCategory = false;

                    if (PrevPost != null) //بررسی تفاوت رده
                    {

                        if (PrevPost.Chart_JobPosition.Chart_JobIdentity != null)
                        {

                            var LastPostCategoryId = LastJobPositions.Chart_JobPosition.Chart_JobIdentity.JobCategoryId;
                            var prevPostCategoryId = PrevPost.Chart_JobPosition.Chart_JobIdentity.JobCategoryId;
                            if (LastPostCategoryId != prevPostCategoryId)
                            {
                                IsChengCategory = true;
                            }
                        }


                    }



                    var UpgradeBaseDate = employee.EmployeeDateInformations;

                    var lastJobCategoryId = LastJobPositions.Chart_JobPosition.Chart_JobIdentity.JobCategoryId;



                    var lastEmployeeInterdictUpgradeGrups = employee.LastEmployeeInterDicts;
                    if (lastEmployeeInterdictUpgradeGrups == null)
                    {
                        //var line = $"{employeenumber} ,   اطلاعات مدرک ندارد";
                        //System.IO.File.AppendAllText(Errorpath, line.Trim() + Environment.NewLine);


                        continue;
                    }; //هیچ حکمی ندارد

                    var lastEmployeeInterdictUpgradeGrupsCurrentJobGroupId = lastEmployeeInterdictUpgradeGrups.CurrentJobGroupId;
                    var CurrentJobPositionCategoryEducation = allChart_JobCategoryEducations
                        .FirstOrDefault(a =>
                        a.JobCategoryId == lastJobCategoryId && //شرط شغل
                        a.LevelNumber == lastEmployeeInterdictUpgradeGrupsCurrentJobGroupId);
                    if (CurrentJobPositionCategoryEducation == null)
                    {
                        //var line = $"{employeenumber} ,   اطلاعات مدرک ندارد";
                        //System.IO.File.AppendAllText(Errorpath, line.Trim() + Environment.NewLine);
                        continue;
                    }; //هیچ حکمی ندارد // گروهش کمتر از حد مجاز است

                    var NexLevelNumber = lastEmployeeInterdictUpgradeGrupsCurrentJobGroupId + 1;
                    var NextJobPositionCategoryEducation = allChart_JobCategoryEducations
                        .FirstOrDefault(a =>
                        a.JobCategoryId == lastJobCategoryId && //شرط شغل
                        a.LevelNumber == NexLevelNumber);

                    if (NextJobPositionCategoryEducation == null)
                    {
                        //var line = $"{employeenumber} , {lastEmployeeInterdictUpgradeGrupsCurrentJobGroupId},  به سقف گروه رسیده است";
                        //System.IO.File.AppendAllText(Errorpath, line.Trim() + Environment.NewLine);

                        continue;
                    }; //// یعنی به سقف گروه رسیده است
                  
                    var currentExperience = CurrentJobPositionCategoryEducation != null ? CurrentJobPositionCategoryEducation.Experience.Value : 0;

                    //بدست اوردن مجموع سابقه فرد
                    double ScoreEducation = GetScoreEducationBetweenPostAndEmployee(CurrentJobPositionCategoryEducation.EducationCategoryId, lastEducationCategoryId);

                    // برای رده کاردان و کاردان مسئول به نحوی که اگر هر کدام از کارکنان
                    //دارای رده فوق الذکر دارای مدرک لیسانس بودند ضریب تعدیل مدرک تحصیلی لحاظ نشود
                    //( با توجه به مصوبه خرداد ماه کمیته طبقه بندی مشاغل)
                    //var jobDefinitionSkipt = new List<int> { 5, 6 };
                    //if (lastEducationCategoryId == 6 && jobDefinitionSkipt.Contains(CurrentJobPositionCategoryEducation.Chart_JobCategory.JobCategoryDefinationId))
                    //{
                    //    ScoreEducation = 1;
                    //}

                    // بدست اوردن  مجموعمرخصی بدون حقوق و غیبت روزانه
                    //var dissMonthtimeSheet = monthTimeSheetRollCall.Where(a => a.EmployeeId == employee.Id).Sum(a => a.DurationInMinut).ConvertMinToDaysHour(9).Split(":")[0];
                    var MisdissMonthtimeSheet54 = employee.EmployeeDateInformations == null ? 0 : employee.EmployeeDateInformations.AbsenceDisconnectDurationAsDays.HasValue ? employee.EmployeeDateInformations.AbsenceDisconnectDurationAsDays.Value : 0;
                    var MisdissMonthtimeSheet64 = employee.EmployeeDateInformations == null ? 0 : employee.EmployeeDateInformations.UnPaidDisconnectDurationAsDays.HasValue ? employee.EmployeeDateInformations.UnPaidDisconnectDurationAsDays.Value : 0;

                    var totalDissMonthTimesheet = MisdissMonthtimeSheet54 + MisdissMonthtimeSheet64;
                    if(employee.EmploymentTypeId.HasValue && employee.EmploymentTypeId == 4)                        
                    {
                        //افرادی که قراردادی هستند سابقه داخل شرکت به آنها تعلق نمیگید
                        if (employee.EmployeeDateInformations!=null)employee.EmployeeDateInformations.InCompanyDurationAsDays = 0;
                    }

                    var totalHistoryDay = GetTotalHistory(employee.EmployeeDateInformations, employee.EmploymentDate, ScoreEducation, totalDissMonthTimesheet);//محاسبه مدت سنوات پرسنل
                                                                                                                                                               // بدست اوردن  مجموعمرخصی بدون حقوق و غیبت روزانه



                    //در این بخش باید تاثیر سابقه مورد نیاز با سابقه فرد لحاظ شود
                    //TODO: 

                    var employeeMeritRatingCoefficint = employee.MeritRatingCoefficint;

                    //اختلاف سنوات گروه فعلی و گروه جدید
                    var diffCurrentGroupAndNextGroup = NextJobPositionCategoryEducation.Experience.Value - currentExperience;


                    //لحاظ شدن تاریخ استخدام بر روی ضریب شایستگی
                    var currentYear = DateTime.Now.GetYearShamsi();
                    var yearEmployeeDate=employee.EmploymentDate.Value.GetYearShamsi();
                    var monthEmployeeDate=employee.EmploymentDate.Value.GetMonthShamsi();
                    if (monthEmployeeDate > 6)
                    {
                        var subYear = currentYear - yearEmployeeDate;
                        if (subYear <= 3)
                        {
                            employeeMeritRatingCoefficint = employeeMeritRatingCoefficint.Take(subYear-1).ToList();
                        }
                    }

                    int? performanceRate = GetPerformanceRate(MeritRating, employeeMeritRatingCoefficint, diffCurrentGroupAndNextGroup);
                    

                    var MeritRatingCoefficint = employeeMeritRatingCoefficint.Sum() / employeeMeritRatingCoefficint.Count();
                    MeritRatingCoefficint = Fixcofficint(MeritRatingCoefficint.Value);



                    var nextUpgradeBaseDate = GetTotalDayForEmployeeDatePromotion((NextJobPositionCategoryEducation.Experience * 365).Value, totalHistoryDay, ScoreEducation);


                    nextUpgradeBaseDate = nextUpgradeBaseDate.AddMonthShamsi(performanceRate.Value).Value;



                    //باید تاریخ پسا هایی را به عنوان تاریخ ارتقا در نظر بگیریم که باعث تغییر رده شده اند



                    var lastUpgrade = employee.EmployeeDateInformations == null ? null : employee.EmployeeDateInformations.LastUpgradeDate;


                    DateTime? lstjobpostionStartDate = LastJobPositions.StartDate;


                    //---------------------------------تعیین وضعیت تغییر گروه و گرید بر اساس حکم پست سازمانی----------------------------------------//
                    //var PrevAsLastJobPostion = allPrevLastJobpositionInterdicts.Where(a => a.EmployeeId == employee.Id)
                    //    .OrderByDescending(c => c.ExecuteDate).FirstOrDefault();

                    ////var IsChangeGroupNumber = false;
                    ////if (PrevAsLastJobPostion != null)
                    ////{
                    ////    if (PrevAsLastJobPostion.CurrentJobGroupId < employee.LastEmployeeChangeJobPosition.CurrentJobGroupId)
                    ////    {
                    ////        IsChangeGroupNumber = true;
                    ////    }
                    ////}

                    ////if (IsChengCategory == false)
                    ////{
                    ////    if (IsChangeGroupNumber == true)
                    ////    {
                    ////        lstjobpostionStartDate = LastJobPositions.StartDate.AddDays(365);
                    ////    }
                    ////    else
                    ////    {
                    ////        lstjobpostionStartDate = null;
                    ////    }

                    ////}
                    ////else if (IsChangeGroupNumber == true)
                    ////{
                    ////    lstjobpostionStartDate = LastJobPositions.StartDate.AddDays(365);
                    ////}

                    if (employee.EmployeeDateInformations != null)
                    {
                        if (employee.EmployeeDateInformations.UpgrateCategoryJobDate.HasValue && employee.EmployeeDateInformations.UpgrateGroupDateByChangePost.HasValue)
                        {
                            if (employee.EmployeeDateInformations.UpgrateCategoryJobDate > employee.EmployeeDateInformations.UpgrateGroupDateByChangePost)
                            {
                                lstjobpostionStartDate = employee.EmployeeDateInformations.UpgrateCategoryJobDate.Value;
                            }
                            else
                            {
                                lstjobpostionStartDate = employee.EmployeeDateInformations.UpgrateGroupDateByChangePost.Value.AddDays(365);
                            }
                        }
                        else if (employee.EmployeeDateInformations.UpgrateCategoryJobDate.HasValue)
                        {
                            lstjobpostionStartDate = employee.EmployeeDateInformations.UpgrateCategoryJobDate.Value;
                        }
                        else if (employee.EmployeeDateInformations.UpgrateGroupDateByChangePost.HasValue)
                        {
                            lstjobpostionStartDate = employee.EmployeeDateInformations.UpgrateGroupDateByChangePost.Value.AddDays(365);
                        }
                        else
                        {
                            lstjobpostionStartDate = null;
                        }
                    }
                    else
                    {
                        lstjobpostionStartDate = null;
                    }

                    nextUpgradeBaseDate = FixNextUpgeadeBaseDate(nextUpgradeBaseDate, lastUpgrade, lstjobpostionStartDate);
                    
                    nextUpgradeBaseDate = FixfinalUpgradeDate(nextUpgradeBaseDate);

                    //تاریخ ارتقا گروه نباید کمتر از تاریخ استخدام باشد
                    if (nextUpgradeBaseDate.Date < employee.EmploymentDate.Value.Date) {
                        nextUpgradeBaseDate= employee.EmploymentDate.Value.AddYearShamsi(1).Value;
                        nextUpgradeBaseDate = FixfinalUpgradeDate(nextUpgradeBaseDate);

                    };


                    if ((nextUpgradeBaseDate.GetYearShamsi() == DateTime.Now.GetYearShamsi() &&
                        nextUpgradeBaseDate.GetMonthShamsi() <= DateTime.Now.GetMonthShamsi())
                         || nextUpgradeBaseDate.GetYearShamsi() < DateTime.Now.GetYearShamsi()
                         
                         )
                    {

                        var nextupgradedate = new DateTime(nextUpgradeBaseDate.Year, nextUpgradeBaseDate.Month, nextUpgradeBaseDate.Day, 23, 59, 59, 0);
                        result.Add(new EmployeePromotion()
                        {
                            EmployeeId = employee.Id,
                            CalculateDate = YearMonth,
                            PromotionDate = nextupgradedate,
                            DegreeBalanceRate= ScoreEducation,
                          
                            CurrentJobGroupId = lastEmployeeInterdictUpgradeGrupsCurrentJobGroupId,
                            NewJobGroupId = NextJobPositionCategoryEducation.LevelNumber,
                            EmployeeEducationDegreeId = employee.EmployeeEducationDegrees.Id,
                            JobPositionId = LastJobPositions.JobPositionId,
                            PerformanceRate = MeritRatingCoefficint,
                            TotalHistory = totalHistoryDay,
                            PromotionStatusId = 1,
                            InsertDate = DateTime.Now,
                            IsActive = true,



                        });
                        continue;


                    }
                    continue;
                }
                catch (Exception ex)
                {
                    var x = counter;
                    continue;
                }

            }

            return result;
        }
        /// <summary>
        /// بررسی اختلاف مدرک پست و فرد  
        /// برای بدست آوردن ضریب تعدیل مدرک تحصیلی 
        /// </summary>
        /// <param name="postEducationCategoryId"></param>
        /// <param name="employeeEducationCategoryId"></param>
        /// <returns></returns>
        public double GetScoreEducationBetweenPostAndEmployee(int? postEducationCategoryId, int? employeeEducationCategoryId)
        {

            try
            {
                if (postEducationCategoryId <= employeeEducationCategoryId)
                {
                    return 1;
                }
                else
                {
                    var systemScore = new List<double> { 0.8, 0.85, 0.85, 0.85, 0.93, 0.93 };
                    var EducationCategories = _kscHrContext.EducationCategory.ToList();
                    var getemployeeEducationScore = EducationCategories.FirstOrDefault(a =>
                    a.Id == employeeEducationCategoryId);


                    var differentEducation = postEducationCategoryId - getemployeeEducationScore.LevelNumber;//اختلاف بین مدرک
                    var factor = (differentEducation + getemployeeEducationScore.LevelNumber) - 1;//بدست اوردن ضریب تعدیل
                    var grade = EducationCategories.FirstOrDefault(a => a.LevelNumber == factor);
                    double score = 1;
                    if (getemployeeEducationScore.LevelNumber == factor)
                    {
                        score = grade.Score.Value;
                    }
                    else
                    {
                        for (var i = getemployeeEducationScore.LevelNumber; i < factor; i++)
                        {
                            var grades = EducationCategories.FirstOrDefault(a => a.LevelNumber == i);
                            score = score * grades.Score.Value;
                        }
                    }


                    return Get2decimal(score);
                }
            }
            catch (Exception ex)
            {
                return 0;

            }


        }

        public double Get2decimal(double val)
        {
            var numberSplited = val.ToString().Split(".");
            var Count2 = numberSplited[1].Count() > 1 ? 2 : numberSplited[1].Count();
            var result = $"{numberSplited[0]}.{numberSplited[1].Substring(0, Count2)}";
            return double.Parse(result);
        }

        /// <summary>
        /// بدست اوردن سابقه بر اساس داده های جدول اطلاعات تاریخ
        /// </summary>
        /// <param name="model"></param>
        /// <param name="EmployeementDate"></param>
        /// <returns></returns>
        public int GetTotalHistory(EmployeeDateInformation model, DateTime? EmployeementDate, double? scoreEducation, int sumMonthTimeSheet5464)
        {
            try
            {
                var totalHistory = 0;
                var diffDateNot = 1 - DateTime.Now.Date.GetDayShamsi();
                var filterDateNow = DateTime.Now.Date.AddDays(diffDateNot);
                if (model == null) return (filterDateNow - EmployeementDate.Value).Days;
                if (model != null && model.EducationConvertDate.HasValue)
                {
                    double scoreEducations = scoreEducation.HasValue ? scoreEducation.Value : 1;
                    totalHistory = (filterDateNow - model.EducationConvertDate.Value.Date).Days;

                    totalHistory = model == null ? totalHistory : !model.DisconnectDurationAsDays.HasValue ? totalHistory : totalHistory - model.DisconnectDurationAsDays.Value;
                    totalHistory = totalHistory - sumMonthTimeSheet5464;
                    totalHistory = (int)Math.Round(totalHistory * scoreEducations);
                    totalHistory = model == null ? totalHistory : !model.ValidEmploymentHistoryAsDays.HasValue ? totalHistory : totalHistory + model.ValidEmploymentHistoryAsDays.Value;
                    totalHistory = model == null ? totalHistory : !model.IncreasesYearsAsDays.HasValue ? totalHistory : totalHistory + model.IncreasesYearsAsDays.Value;
                    totalHistory = model == null ? totalHistory : !model.MilitaryDurationAsDays.HasValue ? totalHistory : totalHistory + model.MilitaryDurationAsDays.Value;
                    if (model.DeductionOfFourYearsOutCompanyAsDays == 0 || model.StatusMatch == 2) //کل سابقه لحاظ شود
                    {
                        totalHistory = model == null ? totalHistory : !model.OutCompanyDurationAsDays.HasValue ? totalHistory : totalHistory + model.OutCompanyDurationAsDays.Value;

                    }
                    else if (model.DeductionOfFourYearsOutCompanyAsDays == 1) //سابقه افراد قراردادی 4 سال کسر شود
                    {
                        totalHistory = model == null ? totalHistory : !model.OutCompanyDurationAsDays.HasValue ? totalHistory : totalHistory + model.OutCompanyDurationAsDays.Value;
                        var yeartoDay = 4 * 365;
                        totalHistory = model == null ? totalHistory : totalHistory - yeartoDay;
                    }

                    if (model.DeductionOfFourYearsOutCompanyAsDays == 2) //قراردادی هایی که کمتر از 4 سال دارند کلا لحاظ نشود
                    {

                        if (totalHistory < 1460)
                        {
                            totalHistory = model == null ? totalHistory : !model.OutCompanyDurationAsDays.HasValue ? totalHistory : totalHistory + 0;
                        }
                        else
                        {
                            var outoffCompany = model.OutCompanyDurationAsDays.HasValue ? model.OutCompanyDurationAsDays.Value : 0;
                            // totalHistory = model == null ? totalHistory : !model.OutCompanyDurationAsDays.HasValue ? totalHistory : totalHistory + model.OutCompanyDurationAsDays.Value;
                            if (totalHistory > 1460)
                            {
                                var yeartoDay = 4 * 365;
                                if (outoffCompany >= yeartoDay)
                                {
                                    totalHistory = model == null ? totalHistory : !model.OutCompanyDurationAsDays.HasValue ? totalHistory : totalHistory + model.OutCompanyDurationAsDays.Value;

                                    totalHistory = model == null ? totalHistory : totalHistory - yeartoDay;
                                }

                            }
                        }


                    }


                }
                else
                {
                    totalHistory = (filterDateNow - EmployeementDate.Value.Date).Days;
                    double scoreEducations = scoreEducation.HasValue ? scoreEducation.Value : 1;
                    totalHistory = model == null ? totalHistory : !model.DisconnectDurationAsDays.HasValue ? totalHistory : totalHistory - model.DisconnectDurationAsDays.Value;
                    totalHistory = totalHistory - sumMonthTimeSheet5464;
                    totalHistory = (int)Math.Round(totalHistory * scoreEducations);
                    totalHistory = model == null ? totalHistory : !model.IncreasesYearsAsDays.HasValue ? totalHistory : totalHistory + (model.IncreasesYearsAsDays.Value);
                    totalHistory = model == null ? totalHistory : !model.MilitaryDurationAsDays.HasValue ? totalHistory : totalHistory + model.MilitaryDurationAsDays.Value;
                    totalHistory = model == null ? totalHistory : !model.InCompanyDurationAsDays.HasValue ? totalHistory : totalHistory + model.InCompanyDurationAsDays.Value;

                    if (model.DeductionOfFourYearsOutCompanyAsDays == 0|| model.StatusMatch==2) //کل سابقه لحاظ شود
                    {
                        totalHistory = model == null ? totalHistory : !model.OutCompanyDurationAsDays.HasValue ? totalHistory : totalHistory + model.OutCompanyDurationAsDays.Value;

                    }else  if (model.DeductionOfFourYearsOutCompanyAsDays == 1) //سابقه افراد قراردادی 4 سال کسر شود
                    {
                        totalHistory = model == null ? totalHistory : !model.OutCompanyDurationAsDays.HasValue ? totalHistory : totalHistory + model.OutCompanyDurationAsDays.Value;
                        var yeartoDay = 4 * 365;
                        totalHistory = model == null ? totalHistory : totalHistory - yeartoDay;
                    }

                     if (model.DeductionOfFourYearsOutCompanyAsDays == 2) //قراردادی هایی که کمتر از 4 سال دارند کلا لحاظ نشود
                    {

                        if (totalHistory < 1460)
                        {
                            totalHistory = model == null ? totalHistory : !model.OutCompanyDurationAsDays.HasValue ? totalHistory : totalHistory + 0;
                        }
                        else
                        {
                            var outoffCompany = model.OutCompanyDurationAsDays.HasValue ? model.OutCompanyDurationAsDays.Value : 0;
                           // totalHistory = model == null ? totalHistory : !model.OutCompanyDurationAsDays.HasValue ? totalHistory : totalHistory + model.OutCompanyDurationAsDays.Value;
                            if(totalHistory> 1460)
                            {
                                var yeartoDay = 4 * 365;
                                if (outoffCompany >= yeartoDay)
                                {
                                    totalHistory = model == null ? totalHistory : !model.OutCompanyDurationAsDays.HasValue ? totalHistory : totalHistory + model.OutCompanyDurationAsDays.Value;
                           
                                    totalHistory = model == null ? totalHistory : totalHistory - yeartoDay;
                                }
                                
                            }
                        }
               

                    }
                }
                return totalHistory;
            }
            catch (Exception ex)
            {
                return 0;
            }


        }
        /// <summary>
        /// بدست اوردن مقدار تاریخ تاثیر
        /// </summary>
        /// <param name="MeritRating"></param>
        /// <param name="employeeMeritRatingCoefficint"></param>
        /// <returns></returns>
        public int? GetPerformanceRate(List<MeritRating> MeritRating, IEnumerable<double?> employeeMeritRatingCoefficint, int DiffCurrentGroupAndNexGroup = 0)
        {
            try
            {
                var MeritRatingCoefficint = employeeMeritRatingCoefficint.Sum() / employeeMeritRatingCoefficint.Count();
                MeritRatingCoefficint = Fixcofficint(MeritRatingCoefficint.Value);

                var checkMeritAddMonth = GetPerformanceRating(DiffCurrentGroupAndNexGroup, MeritRatingCoefficint.Value);

                return int.Parse(checkMeritAddMonth.ToString());
            }
            catch (Exception ex)
            {
                return 0;
            }

        }


        /// <summary>
        /// اصلاح مقدار ضرب عملکرد موثر
        /// </summary>
        /// <param name="cofficint"></param>
        /// <returns></returns>
        public double Fixcofficint(double? cofficint)
        {
            if (cofficint.HasValue)
            {
                if (cofficint.Value >= 1.2)
                {
                    return 1.2;
                }
                if (cofficint.Value >= 1.1 && cofficint.Value < 1.2)
                {
                    return 1.1;
                }
                if (cofficint.Value >= 1 && cofficint.Value < 1.1)
                {
                    return 1;
                }
                if (cofficint.Value > 0.8 && cofficint.Value < 1)
                {
                    return 0.9;
                }
                if (cofficint.Value <= 0.8)
                {
                    return 0.8;
                }
            }
            return 0;
        }


        /// <summary>
        /// محاسبه مقدار ماهی موثر بر تاریخ ارتقا
        /// </summary>
        /// <param name="DiffCurrentGroupandNexGroup"></param>
        /// <param name="MeritRatingCoefficint"></param>
        /// <returns></returns>
        public double GetPerformanceRating(int DiffCurrentGroupandNexGroup, double MeritRatingCoefficint)
        {

            var MeritRating = 1 - MeritRatingCoefficint;
            var result = (DiffCurrentGroupandNexGroup * MeritRating) * 12;
            return Math.Round(result);
        }



        /// <summary>
        /// بررسی اختلاف مدرک پست و فرد  
        /// برای بدست آوردن ضریب تعدیل مدرک تحصیلی 
        /// برای بدست آوردن ضریب تعدیل مدرک تحصیلی 
        /// تاثیر ضریب تعدیل برای افرادیس که سابقه آنها
        /// از سابقه مورد نیاز کمتر هست
        /// </summary>
        /// <param name="postEducationCategoryId"></param>
        /// <param name="employeeEducationCategoryId"></param>
        /// <returns></returns>
        public DateTime GetTotalDayForEmployeeDatePromotion(int jobEducationEpherience, int employeeTotalHistory, double educationScore)
        {
            var diffrentDayTotalHistory = jobEducationEpherience - employeeTotalHistory;
            var diffDateNot = 1 - DateTime.Now.Date.GetDayShamsi();
            var filterDateNow = DateTime.Now.Date.AddDays(diffDateNot);
            if (educationScore < 1 && diffrentDayTotalHistory > 0)
            {
                var gettotalHistory = (2 - educationScore) * diffrentDayTotalHistory;
                var checkdata = Math.Round(gettotalHistory);


                return filterDateNow.AddDays(checkdata);
            }
            else
            {
                return filterDateNow.AddDays(diffrentDayTotalHistory);
            }
        }



        public DateTime FixNextUpgeadeBaseDate(DateTime nextUpgradeBaseDate, DateTime? LastUpgradeDate, DateTime? JobPositionStartDate)
        {
            try
            {
                //باید تاریخ پسا هایی را به عنوان تاریخ ارتقا در نظر بگیریم که باعث تغییر رده شده اند

                DateTime? lastUpgradeDate = LastUpgradeDate.HasValue ? LastUpgradeDate.Value.AddDays(365) : null;

                //باید اصلاح شود
                DateTime? jobPositionStartDate = JobPositionStartDate.HasValue ? JobPositionStartDate.Value : null;

                DateTime? selecteNexUpgradeDate = null;
                var isNextUpgradeDate = false;
                var isLastSelected = false;


                if (lastUpgradeDate.HasValue && jobPositionStartDate.HasValue)
                {
                    if (lastUpgradeDate.Value > jobPositionStartDate.Value)
                    {
                        if (nextUpgradeBaseDate > lastUpgradeDate.Value )
                        {
                            return nextUpgradeBaseDate;
                        }
                        else
                        {
                            return lastUpgradeDate.Value;
                        }
                    }
                    else
                    {
                        if (nextUpgradeBaseDate > jobPositionStartDate.Value)
                        {
                            return nextUpgradeBaseDate;
                        }
                        else
                        {
                            return jobPositionStartDate.Value;
                        }
                    }
                }
                else if (lastUpgradeDate.HasValue)
                {
                    if (nextUpgradeBaseDate > lastUpgradeDate.Value)
                    {
                        return nextUpgradeBaseDate;
                    }
                    else
                    {
                        return lastUpgradeDate.Value;
                    }
                }
                else if (jobPositionStartDate.HasValue)
                {
                    if (nextUpgradeBaseDate > jobPositionStartDate.Value)
                    {
                        return nextUpgradeBaseDate;
                    }
                    else
                    {
                        return jobPositionStartDate.Value;
                    }
                }
                else
                {
                    return nextUpgradeBaseDate;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DateTime FixfinalUpgradeDate(DateTime finalUpgradeDate)
        {


            if (finalUpgradeDate.GetDayShamsi() <= 15)
            {
                var diffDay = 1 - finalUpgradeDate.GetDayShamsi();

                finalUpgradeDate = finalUpgradeDate.AddDays(diffDay);
            }
            else
            {
                var diffDay = 1 - finalUpgradeDate.GetDayShamsi();

                finalUpgradeDate = finalUpgradeDate.AddDays(diffDay).AddMonthShamsi(1).Value;
            }

            return finalUpgradeDate;

        }

        public DateTime PredictionDateUpgrate(int employeeId)
        {
            var employeeQuery = _kscHrContext.Employees.AsQueryable().AsNoTracking()
                .Where(a => a.Id == employeeId)
                .Include(a => a.EmployeePercentMeritHistories).ThenInclude(a => a.MeritRating)
                .Include(a => a.EmployeeDisConnections).ThenInclude(a => a.DisConnectionType)
                .Include(a => a.EmployeeEducationDegrees).ThenInclude(a => a.Education)
                .Include(a => a.EmployeeHistoryTypeEmployements).ThenInclude(a => a.HistoryTypeEmployement)
                .Include(a => a.EmployeeDateInformations)
                .FirstOrDefault();

            var LastEmployeeInterdictUpgradeGrup = _kscHrContext.EmployeeInterdicts
                .AsQueryable().AsNoTracking().Where(a =>
                a.EmployeeId == employeeId &&
                a.InterdictTypeId == 2
                )
                .OrderByDescending(x => x.ExecuteDate).FirstOrDefault();

            var LastJobPosition = _kscHrContext.EmployeeJobPositions.AsNoTracking().OrderByDescending(a => a.StartDate)
                .Where(_ => _.EmployeeId == employeeId && _.IsActive && !_.EndDate.HasValue)
                .Include(a => a.Chart_JobPosition).ThenInclude(a => a.Chart_JobIdentity)
                .ThenInclude(a => a.Chart_JobCategory).ThenInclude(a => a.Chart_JobCategoryDefination)
                .FirstOrDefault();

            var lastEducationCategoryId = employeeQuery.EmployeeEducationDegrees.OrderByDescending(a => a.EducationDate)
                .FirstOrDefault(a => a.EducationDate.HasValue).Education.CategoryId;

            var LastJobCategoryId = LastJobPosition.Chart_JobPosition.Chart_JobIdentity.JobCategoryId;

            var CurrentJobPositionCategoryEducation = _kscHrContext.Chart_JobCategoryEducation.AsNoTracking()
                .FirstOrDefault(a =>
                a.EducationCategoryId == lastEducationCategoryId.Value && //شرط تاریخ
                a.JobCategoryId == LastJobCategoryId && //شرط شغل
                a.LevelNumber == LastEmployeeInterdictUpgradeGrup.CurrentJobGroupId);

            var NexLevelNumber = LastEmployeeInterdictUpgradeGrup.CurrentJobGroupId + 1;
            var NextJobPositionCategoryEducation = _kscHrContext.Chart_JobCategoryEducation.AsNoTracking()
                .FirstOrDefault(a =>
                a.EducationCategoryId == lastEducationCategoryId.Value && //شرط تاریخ
                a.JobCategoryId == LastJobCategoryId && //شرط شغل
                a.LevelNumber == NexLevelNumber);

            var DiffExperience = NextJobPositionCategoryEducation.Experience - CurrentJobPositionCategoryEducation.Experience;

            //var UpgradeBaseDate = employeeQuery.EmployeeDateInformations.OrderByDescending(a => a.Id).FirstOrDefault(a => a.UpgradeBaseDate.HasValue);

            //if (UpgradeBaseDate == null)
            //{
            //    var nextUpgradeBaseDate = employeeQuery.EmploymentDate.Value.AddYears(DiffExperience.Value);
            //    return nextUpgradeBaseDate;
            //}
            //else
            //{
            //    var nextUpgradeBaseDate = UpgradeBaseDate.UpgradeBaseDate.Value.AddYears(DiffExperience.Value);

            //    return nextUpgradeBaseDate;
            //}

            return DateTime.Now;
            //بررسی حق شایستگی 3 سال اخیر



            //3 سال اخیر بررسی میانگین آموزش



            //بررسی تعداد سنوات بر اساس میانگین آموزشی

        }






        public int CheckMeritAddMonth(double? cofficint)
        {
            if (cofficint.HasValue)
            {
                if (cofficint.Value >= 1.2)
                {
                    return -10;
                }
                if (cofficint.Value >= 1.1 && cofficint.Value < 1.2)
                {
                    return -5;
                }
                if (cofficint.Value == 1)
                {
                    return 0;
                }
                if (cofficint.Value > 0.8 && cofficint.Value < 1)
                {
                    return 5;
                }
                if (cofficint.Value <= 0.8)
                {
                    return 10;
                }
            }
            return 0;
        }




        /// <summary>
        /// بررسی وضعیت تاریخ پیشنهادی ، تاریخ ارتقا گروه و تاریخ احراز پست
        /// </summary>
        /// <param name="nextUpgradeBaseDate"></param>
        /// <param name="LastUpgradeDate"></param>
        /// <param name="JobPositionStartDate"></param>
        /// <returns></returns>
        public Tuple<DateTime?, DateTime?, bool, bool> SelectBaseUpgradeDate(DateTime nextUpgradeBaseDate, DateTime? LastUpgradeDate, DateTime? JobPositionStartDate)
        {
            try
            {
                //باید تاریخ پسا هایی را به عنوان تاریخ ارتقا در نظر بگیریم که باعث تغییر رده شده اند

                DateTime? lastUpgradeDate = null;

                DateTime? selecteNexUpgradeDate = null;
                var isNextUpgradeDate = false;
                var isLastSelected = false;



                if (LastUpgradeDate.HasValue)
                {
                    if (LastUpgradeDate >= nextUpgradeBaseDate)
                    {
                        isNextUpgradeDate = true;
                        selecteNexUpgradeDate = LastUpgradeDate.Value;
                    }
                    else
                    {
                        selecteNexUpgradeDate = nextUpgradeBaseDate;
                    }
                    if (!JobPositionStartDate.HasValue)
                    {

                        isLastSelected = true;
                        lastUpgradeDate = LastUpgradeDate.Value.Date;
                    }
                    else if (LastUpgradeDate.Value.Date > JobPositionStartDate.Value.Date)
                    {
                        isLastSelected = true;
                        lastUpgradeDate = LastUpgradeDate.Value.Date;
                    }
                    else
                    {
                        lastUpgradeDate = JobPositionStartDate.Value.Date;
                    }

                }
                else
                {
                    selecteNexUpgradeDate = nextUpgradeBaseDate;

                    lastUpgradeDate = JobPositionStartDate.HasValue ? JobPositionStartDate.Value.Date : nextUpgradeBaseDate;
                }



                return new Tuple<DateTime?, DateTime?, bool, bool>(selecteNexUpgradeDate, lastUpgradeDate, isNextUpgradeDate, isLastSelected);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="nextUpgradeBaseDate"></param> 
        /// مقدار اول اخرین تاریخ ارتقا هست یا تاریخ پیشنهادی
        /// <param name="lastOrJobDate"></param>
        /// شامل اخرین تاریخ ارتقا یا تاریخ احراز پست
        /// 
        /// <param name="isNexUpgradeDate"></param> ایا رکورد اول شامل اخرین تاریخ ارتقا گروه هست؟
        /// <param name="islastOrJobDate"></param>ایا رکورد دوم شامل اخرین تاریخ ارتقا گروه هست؟
        /// <returns></returns>
        public DateTime CheckOneYearDiffBetweenUpgradeDateAndNextUpgradeDate(DateTime? nextUpgradeBaseDate, DateTime? lastOrJobDate, bool isNexUpgradeDate, bool islastOrJobDate)
        {

            var DiffCurrentAndLastUpgradeDate = (nextUpgradeBaseDate.Value - lastOrJobDate.Value).Days;

            if (DiffCurrentAndLastUpgradeDate < 365 && DiffCurrentAndLastUpgradeDate != 0)
            {
                if (isNexUpgradeDate)
                {
                    if (nextUpgradeBaseDate > lastOrJobDate)
                    {
                        nextUpgradeBaseDate = nextUpgradeBaseDate.Value.AddDays(365);
                    }
                    else
                    {
                        nextUpgradeBaseDate = lastOrJobDate.Value.AddDays(365);
                    }
                }
                else
                {
                    nextUpgradeBaseDate = lastOrJobDate.Value.AddDays(365);

                }


            }
            return nextUpgradeBaseDate.Value;

        }

        public List<EmployeePromotion> GetEmployeeJoinedToNextLevel(List<int> employeeIds, int YearMonth)
        {

            var result = new List<EmployeePromotion>();
            var getEmployee = new List<Tuple<int, DateTime>>();
            var isarStatusIds = new List<int>() { 10, 11, 12, 13, 14, 15, 16, 17, 21, 22, 23, 24 };
            string Errorpath = @"D:/مشترک/employeePromotion_Error.txt";
            var workcalendar = _kscHrContext.WorkCalendars.OrderBy(a => a.MiladiDateV1).FirstOrDefault(a => a.YearMonthV1 == YearMonth).MiladiDateV1;

            var EmployeeInterdictJobPosition = _kscHrContext.EmployeeInterdicts
                .Where(a => a.InterdictTypeId == 3 && a.PortalFinalConfirmFlag == false)
                .Where(a => a.IssuanceDate >= workcalendar)
                .Select(a => a.EmployeeId.Value).ToList();
            employeeIds = employeeIds.Except(EmployeeInterdictJobPosition).ToList();
            var employeeQuery = _kscHrContext.Employees
                .AsQueryable().AsNoTracking()
                .Where(a => employeeIds.Any(x => x == a.Id))
                .Include(a => a.EmployeeInterdicts)
                .Include(a => a.EmployeePercentMeritHistories).ThenInclude(a => a.MeritRating)
                .Include(a => a.EmployeeDisConnections).ThenInclude(a => a.DisConnectionType)
                .Include(a => a.EmployeeEducationDegrees).ThenInclude(a => a.Education)
                .Include(a => a.EmployeeHistoryTypeEmployements).ThenInclude(a => a.HistoryTypeEmployement)
                .Include(a => a.EmployeeDateInformations).Select(a => new
                {
                    a.Id,
                    a.EmployeeNumber,
                   FullName= a.Name+" "+a.Family,
                    IsIsar = isarStatusIds.Any(x => x == a.IsarStatusId),

                    MeritRatingCoefficint = a.EmployeePercentMeritHistories
                    .OrderByDescending(a => a.Year).Take(3)

                    .Select(a => a.MeritRating.Coefficint),

                    a.EmploymentTypeId,
                    EmployeeDisConnections = a.EmployeeDisConnections.ToList(),
                    EmployeeEducationDegrees = a.EmployeeEducationDegrees.OrderByDescending(a => a.InsertDate).FirstOrDefault(),
                    EmployeeHistoryTypeEmployements = a.EmployeePercentMeritHistories.OrderByDescending(a => a.Year).FirstOrDefault(),
                    EmployeeDateInformations = a.EmployeeDateInformations.FirstOrDefault(),

                    a.JobPositionId,
                    LastEmployeeInterDicts = a.EmployeeInterdicts.OrderByDescending(x => x.ExecuteDate).FirstOrDefault(),
                    LastEmployeeChangeJobPosition = a.EmployeeInterdicts.OrderBy(x => x.ExecuteDate).FirstOrDefault(x => x.JobPositionId == a.JobPositionId),

                    a.EmploymentDate
                })
                .ToList();
            var MeritRating = _kscHrContext.MeritRatings.ToList();

            var LastJobPositionsEmployees = _kscHrContext.EmployeeJobPositions.Where(a => employeeIds.Contains(a.EmployeeId))
                   .OrderByDescending(a => a.StartDate)
                    .Include(a => a.Chart_JobPosition).ThenInclude(a => a.Chart_JobIdentity)
                    .ThenInclude(a => a.Chart_JobCategory).ThenInclude(a => a.Chart_JobCategoryDefination)
                    .Include(a => a.Chart_JobPosition).ThenInclude(a => a.Chart_JobPositionFields).ThenInclude(a => a.StudyField).AsNoTracking()
                .ToList();

            var allJobPositionCategoryEducation = _kscHrContext.Chart_JobCategoryEducation.AsNoTracking().ToList();
            var selectYear = YearMonth.ToString().Substring(0, 4);
            var selectMonth = YearMonth.ToString().Substring(4, 2);
            var runStepper = $"{selectYear}/{selectMonth}/2";
            var allChart_JobCategoryEducations = _kscHrContext.Chart_JobCategoryEducation
                .Include(a => a.EducationCategory)
                .Include(a => a.Chart_JobCategory)
                .ThenInclude(a => a.Chart_JobCategoryDefination)

                .AsNoTracking().ToList();

            var RollCallDefinitionsIds = new List<int>() { 54, 64 };

            var counter = 0;
            foreach (var employee in employeeQuery)
            {
                try
                {
                    var employeenumber = employee.EmployeeNumber;
                 
                    counter++;
                    if (employee.EmployeeEducationDegrees == null)
                    {
                        //var line = $"{employeenumber} ,   اطلاعات مدرک ندارد";
                        //System.IO.File.AppendAllText(Errorpath, line.Trim() + Environment.NewLine);


                        continue;
                    };//مدرک تحصیلی تعریف نشده است
                    if (employee.EmployeeEducationDegrees.Education == null)
                    {
                        //var line = $"{employeenumber} ,   اطلاعات مدرک ندارد";
                        //System.IO.File.AppendAllText(Errorpath, line.Trim() + Environment.NewLine);


                        continue;
                    }; ;

                    var lastEducationCategoryId = employee.EmployeeEducationDegrees.Education.CategoryId;
                    if (employee.IsIsar) lastEducationCategoryId++;
                    var Get2StepJobPosition = LastJobPositionsEmployees.Where(a => a.EmployeeId == employee.Id)
                        .OrderByDescending(a => a.StartDate).Take(2).ToList();
                    var LastJobPositions = Get2StepJobPosition.Where(a => a.IsActive).FirstOrDefault();
                    var PrevPost = Get2StepJobPosition.Where(a => a.IsActive == false).OrderBy(a => a.StartDate).FirstOrDefault();
                    if (LastJobPositions == null)
                    {
                        //var line = $"{employeenumber} ,   اطلاعات مدرک ندارد";
                        //System.IO.File.AppendAllText(Errorpath, line.Trim() + Environment.NewLine);


                        continue;
                    };

                    if (LastJobPositions.Chart_JobPosition.Chart_JobIdentity == null)
                    {
                        //var line = $"{employeenumber} ,   اطلاعات مدرک ندارد";
                        //System.IO.File.AppendAllText(Errorpath, line.Trim() + Environment.NewLine);


                        continue;
                    };




                    var IsChengCategory = false;

                    if (PrevPost != null) //بررسی تفاوت رده
                    {

                        if (PrevPost.Chart_JobPosition.Chart_JobIdentity != null)
                        {

                            var LastPostCategoryId = LastJobPositions.Chart_JobPosition.Chart_JobIdentity.JobCategoryId;
                            var prevPostCategoryId = PrevPost.Chart_JobPosition.Chart_JobIdentity.JobCategoryId;
                            if (LastPostCategoryId != prevPostCategoryId)
                            {
                                IsChengCategory = true;
                            }
                        }


                    }



                    var UpgradeBaseDate = employee.EmployeeDateInformations;

                    var lastJobCategoryId = LastJobPositions.Chart_JobPosition.Chart_JobIdentity.JobCategoryId;



                    var lastEmployeeInterdictUpgradeGrups = employee.LastEmployeeInterDicts;
                    if (lastEmployeeInterdictUpgradeGrups == null)
                    {
                        //var line = $"{employeenumber} ,   اطلاعات مدرک ندارد";
                        //System.IO.File.AppendAllText(Errorpath, line.Trim() + Environment.NewLine);


                        continue;
                    }; //هیچ حکمی ندارد

                    var lastEmployeeInterdictUpgradeGrupsCurrentJobGroupId = lastEmployeeInterdictUpgradeGrups.CurrentJobGroupId;
                    var CurrentJobPositionCategoryEducation = allChart_JobCategoryEducations
                        .FirstOrDefault(a =>
                        a.JobCategoryId == lastJobCategoryId && //شرط شغل
                        a.LevelNumber == lastEmployeeInterdictUpgradeGrupsCurrentJobGroupId);
                    if (CurrentJobPositionCategoryEducation == null)
                    {
                        //var line = $"{employeenumber} ,   اطلاعات مدرک ندارد";
                        //System.IO.File.AppendAllText(Errorpath, line.Trim() + Environment.NewLine);
                        continue;
                    }; //هیچ حکمی ندارد // گروهش کمتر از حد مجاز است

                    var NexLevelNumber = lastEmployeeInterdictUpgradeGrupsCurrentJobGroupId + 1;
                    var NextJobPositionCategoryEducation = allChart_JobCategoryEducations
                        .FirstOrDefault(a =>
                        a.JobCategoryId == lastJobCategoryId && //شرط شغل
                        a.LevelNumber == NexLevelNumber);

                    if (NextJobPositionCategoryEducation == null)
                    {
                        var line = $"{employeenumber},{employee.FullName} " +
                            $",{LastJobPositions.Chart_JobPosition.MisJobPositionCode}" +
                            $",{LastJobPositions.Chart_JobPosition.Title}" +
                            $",{LastJobPositions.Chart_JobPosition.Chart_JobIdentity.Chart_JobCategory.Chart_JobCategoryDefination.Title}" +
                            $",{LastJobPositions.Chart_JobPosition.Chart_JobIdentity.Title}," +
                      
                        
                            $" {lastEmployeeInterdictUpgradeGrupsCurrentJobGroupId}";
                        System.IO.File.AppendAllText(Errorpath, line.Trim() + Environment.NewLine);

                        continue;
                    }; //// یعنی به سقف گروه رسیده است

                  
                    continue;
                }
                catch (Exception ex)
                {
                    var x = counter;
                    continue;
                }

            }

            return result;
        }
    }
}
