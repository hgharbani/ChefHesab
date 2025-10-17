using DNTPersianUtils.Core;
using EFCore.BulkExtensions;
using Ksc.Hr.Domain.Entities;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities;
using Ksc.HR.Domain.Entities.Chart;
using Ksc.HR.Domain.Entities.EmployeeBase;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Entities.Rule;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Repositories.Personal;
using Ksc.HR.Domain.Repositories.Rule;
using Ksc.HR.Share.General;
using Ksc.HR.Share.Model.emp;
using Ksc.HR.Share.Model.Employee;
using Ksc.HR.Share.Model.PaymentStatus;
using Ksc.HR.Share.Model.Reward;
using Ksc.HR.Share.Model.Rule;
using Ksc.HR.Share.Model.RuleCoefficient;
using Ksc.HR.Share.Model.Salary;
using KSC.Common.Filters.Models;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.Rule
{
    public class EmployeeInterdictRepository : EfRepository<EmployeeInterdict, int>, IEmployeeInterdictRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeeInterdictRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<EmployeeInterdict> GetEmployeeInterdict(List<int?> employeeIds)
        {
            var s = _kscHrContext.EmployeeInterdicts
                .Where(x => employeeIds
                .Contains(x.EmployeeId)&&
                    //x.InterdictTypeId == 2 &&
                   x.ExecuteDateYearMonth >= 140301)
                .Include(x => x.Chart_JobPosition).ThenInclude(x => x.Chart_JobPositionScores);


            return s;



        }

        public EmployeeInterdict GetLastInterdictEmployee(int employeeId,bool isAsnoTracking = true)
        {
            var lastEmployeeInterdicts = _kscHrContext.EmployeeInterdicts
               .Where(x => x.EmployeeId== employeeId&&
                  x.PortalFinalConfirmFlag==true).OrderByDescending(a=>a.ExecuteDate)
                  .FirstOrDefault();
            return lastEmployeeInterdicts;
        }
        public IQueryable<EmployeeInterdict> GetAllByRelatedGrid()
        {
            var result = _kscHrContext.EmployeeInterdicts
                .Include(x => x.InterdictDescription).Include(x => x.InterdictType).AsNoTracking().AsQueryable();
            return result;
        }
        public IQueryable<EmployeeInterdict> GetEmployeeInterdictReport(string employeeNumber,
            string firstName,
            string lastName,
            DateTime? startDate,
            DateTime? endDate,
            int? interdictTypeId)
        {

            var result = _kscHrContext.EmployeeInterdicts
                        .Include(x => x.InterdictType)
                        .Include(x => x.Employee)
                        .AsQueryable();
            bool flags = false;

            if (interdictTypeId != null && interdictTypeId > 0)
            {
                result = result.Where(x => x.InterdictTypeId == interdictTypeId.Value);
                flags = true;
            }


            if (!string.IsNullOrEmpty(firstName))
            {
                result = result.Where(x => x.Employee.Name.Contains(firstName));
                flags = true;
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                result = result.Where(x => x.Employee.Family.Contains(lastName));
                flags = true;
            }

            if (!string.IsNullOrEmpty(employeeNumber))
            {
                result = result.Where(x => x.Employee.EmployeeNumber == employeeNumber);
                flags = true;
            }

            if (startDate != null && endDate != null)
            {
                result = result.Where(x => x.IssuanceDate.Value.Date >= startDate.Value.Date &&
                x.IssuanceDate.Value.Date <= endDate.Value.Date);
                flags = true;

            }
            else if (startDate != null)
            {
                result = result.Where(x => x.IssuanceDate.Value.Date >= startDate.Value.Date);
                flags = true;
            }
            else if (endDate != null)
            {
                result = result.Where(x => x.IssuanceDate.Value.Date <= endDate.Value.Date);
                flags = true;
            }
            else if (flags == false)
            {
                //تاریخ پیشفرض hr

                var yearMonth = _kscHrContext.SystemControlDates.FirstOrDefault().SalaryDate;


                var StartgegorianDate = DateTimeExtensions.GetStartDateInYear(yearMonth.ToString());

                var EndgegorianDate = DateTimeExtensions.GetEndDateInYear(yearMonth.ToString());

                result = result.Where(x => x.IssuanceDate.Value.Date >= StartgegorianDate &&
                x.IssuanceDate.Value.Date <= EndgegorianDate);


            }
            return result;
        }


        public void UpdateRange(List<EmployeeInterdict> employeeInterdicts)
        {
            _kscHrContext.EmployeeInterdicts.UpdateRange(employeeInterdicts);
        }


        public IQueryable<EmployeeInterdict> GetAllByRelated()
        {
            var result = _kscHrContext.EmployeeInterdicts
               .Include(x => x.EmployeeInterdictDetails)
               .Where(x => x.EmployeeInterdictDetails.Count == 0)
               .AsQueryable();
            return result;
        }


        public IQueryable<EmployeeInterdict> GetAllReportSteppr()
        {
            var result = _kscHrContext.EmployeeInterdicts
                .Include(x => x.Employee)
                .Include(x => x.InterdictDescription)
                .Include(x => x.InterdictType)
                .AsQueryable();
            return result;
        }

        public IQueryable<EmployeeInterdict> GetAllByIds(List<int> Ids)
        {
            var result = _kscHrContext.EmployeeInterdicts
                .Where(x => Ids.Contains(x.Id))
               .AsQueryable();
            return result;
        }

        public EmployeeInterdict GetandDetailByEmployeeId(int employeeId)
        {
            var result = _kscHrContext.EmployeeInterdicts
               .Include(x => x.EmployeeInterdictDetails)
               .Where(x => x.EmployeeId == employeeId)
               .OrderByDescending(x => x.Id)
               .FirstOrDefault();

            return result;
        }
        public EmployeeInterdict GetandDetailById(int id)
        {
            //var result = _kscHrContext.EmployeeInterdicts
            //   .Include(x => x.EmployeeInterdictDetails)
            //   .FirstOrDefault(x => x.Id == id);




            var result = _kscHrContext.EmployeeInterdicts
                            .Where(x => x.Id == id)
                           .Include(x => x.InterdictDescription)
                           .Include(x => x.InterdictStatus)
                           .Include(x => x.InterdictType)
                           .Include(x => x.Chart_JobPosition)
                           .Include(x => x.CurrentJobGroup)
                           .Include(x => x.EmployeeInterdictDetails)
                           .ThenInclude(x => x.AccountCode)
                           .ThenInclude(x => x.AccountEmploymentTypes)
                           .FirstOrDefault();


            return result;
        }

        public string LastInterdictNumber(int empId)
        {
            var result = _kscHrContext.EmployeeInterdicts.Where(x => x.EmployeeId == empId).OrderByDescending(x => x.ExecuteDate).Select(x => x.InterdictNumber).FirstOrDefault();
            return result;
        }
        public EmployeeInterdict GetRelatedById(int id)
        {
            try
            {
                var result = _kscHrContext.EmployeeInterdicts
                    .Include(a=>a.Employee)
                                          .Include(x => x.InterdictDescription)
                                          .Include(x => x.InterdictStatus)
                                          .Include(x => x.InterdictType)
                                          .Include(x => x.Chart_JobPosition)
                                          .Include(x => x.CurrentJobGroup)
                                          .Include(x => x.EmployeeInterdictDetails)
                                          .ThenInclude(x => x.AccountCode)
                                          .ThenInclude(x => x.AccountCodeCategory)
                                          .FirstOrDefault(x => x.Id == id);

                return result;
            }
            catch (Exception)
            {

                return new EmployeeInterdict();
            }

        }

        public EmployeeInterdict GetlatestRelatedByEmployeeId(int employeeId)
        {
            var result = _kscHrContext.EmployeeInterdicts
                .Where(x => x.EmployeeId == employeeId)
                .Include(a=>a.Employee)
               .Include(x => x.InterdictDescription)
               .Include(x => x.InterdictStatus)
               .Include(x => x.InterdictType)
               .Include(x => x.Chart_JobPosition)
               .Include(x => x.CurrentJobGroup)
               .Include(x => x.EmployeeInterdictDetails)
               .ThenInclude(x => x.AccountCode)
               .ThenInclude(x => x.AccountEmploymentTypes)
               //.AsQueryable()                                                                     
               .OrderByDescending(x => x.ExecuteDate)
               .FirstOrDefault();

            return result;
        }

        public EmployeeInterdict GetlatestRelatedByEmployeeId(int employeeId, DateTime? ExecuteDate, int interdictId)
        {

            if (interdictId > 0)
            {
                var yearmonth = Convert.ToInt32(ExecuteDate.Value.GetYearMonthShamsiByMiladiDate()).ToString();

                //var startDate = DateTimeExtensions.GetStartDateInYear(yearmonth);
                //var endDate = DateTimeExtensions.GetEndDateInYear(yearmonth);


                var s = _kscHrContext.EmployeeInterdicts
                            .Where(x => x.Id == interdictId)
                           .Include(x => x.InterdictDescription)
                           .Include(x => x.InterdictStatus)
                           .Include(x => x.InterdictType)
                           .Include(x => x.Chart_JobPosition)
                           .Include(x => x.CurrentJobGroup)
                           .Include(x => x.EmployeeInterdictDetails)
                           .ThenInclude(x => x.AccountCode)
                           .ThenInclude(x => x.AccountEmploymentTypes)
                   //.Where(x => x.ExecuteDate >= startDate && x.ExecuteDate <= endDate)
                   .OrderByDescending(x => x.ExecuteDate)
                   .FirstOrDefault();
                return s;
            }

            else
            {
                if (ExecuteDate == null)
                {



                    var s = _kscHrContext.EmployeeInterdicts
                                .Where(x => x.EmployeeId == employeeId)
                               .Include(x => x.InterdictDescription)
                               .Include(x => x.InterdictStatus)
                               .Include(x => x.InterdictType)
                               .Include(x => x.Chart_JobPosition)
                               .Include(x => x.CurrentJobGroup)
                               .Include(x => x.EmployeeInterdictDetails)
                               .ThenInclude(x => x.AccountCode)
                               .ThenInclude(x => x.AccountEmploymentTypes).OrderByDescending(x => x.ExecuteDate).FirstOrDefault();

                    return s;
                }
                else
                {
                    var yearmonth = Convert.ToInt32(ExecuteDate.Value.GetYearMonthShamsiByMiladiDate()).ToString();

                    //var startDate = DateTimeExtensions.GetStartDateInYear(yearmonth);
                    //var endDate = DateTimeExtensions.GetEndDateInYear(yearmonth);


                    var s = _kscHrContext.EmployeeInterdicts
                                .Where(x => x.EmployeeId == employeeId)
                               .Include(x => x.InterdictDescription)
                               .Include(x => x.InterdictStatus)
                               .Include(x => x.InterdictType)
                               .Include(x => x.Chart_JobPosition)
                               .Include(x => x.CurrentJobGroup)
                               .Include(x => x.EmployeeInterdictDetails)
                               .ThenInclude(x => x.AccountCode)
                               .ThenInclude(x => x.AccountEmploymentTypes)
                       //.Where(x => x.ExecuteDate >= startDate && x.ExecuteDate <= endDate)
                       .OrderByDescending(x => x.ExecuteDate)
                       .FirstOrDefault();

                    return s;
                }
            }


        }

        public EmployeeInterdict GetRelatedByEmployeeInterdictId(int employeeInterdictId)
        {
            var result = _kscHrContext.EmployeeInterdicts
                .Where(x => x.Id == employeeInterdictId)
               .Include(x => x.Employee)
               .Include(x => x.InterdictType)
               .FirstOrDefault();

            return result;
        }

        public EmployeeInterdict GetlatestRelatedByEmployeeIdForEdit(int employeeId, int? employeeInterdictId)
        {
            var query = _kscHrContext.EmployeeInterdicts
                .Where(x => x.EmployeeId == employeeId)
               .Include(x => x.InterdictDescription)
               .Include(x => x.InterdictStatus)
               .Include(x => x.InterdictType)
               .Include(x => x.Chart_JobPosition)
               .Include(x => x.CurrentJobGroup)
               .Include(x => x.EmployeeInterdictDetails)
               .ThenInclude(x => x.AccountCode)
               //.AsQueryable()
               .OrderByDescending(x => x.ExecuteDate).AsQueryable();

            var find = query.FirstOrDefault(x => x.Id == employeeInterdictId);



            var result = query.FirstOrDefault(a => a.IssuanceDate < find.IssuanceDate);

            return result;
        }

        public EmployeeInterdict GetlatestByEmployeeId(int employeeId)
        {
            var result = _kscHrContext.EmployeeInterdicts.Where(x => x.EmployeeId == employeeId)
               .OrderByDescending(x => x.ExecuteDate)
               .Include(a=>a.Chart_JobPosition)
               .Include(a=>a.Employee)
               .FirstOrDefault();

            return result;
        }

        public EmployeeInterdict GetlatestByEmployeeIdForDetail(int employeeId)
        {
            var result = _kscHrContext.EmployeeInterdicts.AsQueryable().AsNoTracking().OrderByDescending(x => x.ExecuteDate)
                .FirstOrDefault(x => x.EmployeeId == employeeId);

            return result;
        }
        public IQueryable<EmployeeInterdict> GetlatestByEmployeeIds(List<int> employeeId ,bool portalFinalConfirmFlag)
        {
            var MaxExecuteDatePerEmpId = _kscHrContext.EmployeeInterdicts.Where(x => employeeId.Contains(x.EmployeeId.Value)
            && x.PortalFinalConfirmFlag == portalFinalConfirmFlag
            )
                .GroupBy(x => x.EmployeeId)
                .Select(x => new
                {
                    EmployeeId = x.Key,
                    MaxExecuteDate = x.Max(q => q.ExecuteDate)

                })
               .AsQueryable();

            var result = from tbl in _kscHrContext.EmployeeInterdicts.AsQueryable().Include(x => x.Chart_JobPosition)
                         join m in MaxExecuteDatePerEmpId
                         on new { id = tbl.EmployeeId, date = tbl.ExecuteDate } equals
                         new { id = m.EmployeeId, date = m.MaxExecuteDate }
                         select tbl;


            return result;
        }

        //public EmployeeInterdict AddByModel(EmployeeInterdict LastEmployeeInterdict, EmployeeInterdictFamilyViewModel model)
        //{
        //    long SumBase = 0;
        //    long SumOtherBase = 0;
        //    List<EmployeeInterdictDetail> Details = new List<EmployeeInterdictDetail>();

        //    foreach (var detailItem in LastEmployeeInterdict.EmployeeInterdictDetails)
        //    {
        //        if (detailItem.AccountCodeId == model.AccountCodeId) continue; // داده مربوطی که ارسال شده رد کن

        //        if (detailItem.AccountCode.AccountCodeCategoryId == EnumAccountCodeCategroy.BasicSalary.Id)
        //        {
        //            SumBase += detailItem.Amount;
        //        }
        //        else
        //        {
        //            SumOtherBase += detailItem.Amount;
        //        }

        //        var copyDetailsItem = new EmployeeInterdictDetail()
        //        {
        //            AccountCodeId = detailItem.AccountCodeId,
        //            Amount = detailItem.Amount,
        //            IsActive = detailItem.IsActive,
        //            InsertDate = model.InsertDate,
        //            InsertUser = model.InsertUser,
        //        };
        //        //ثبت ردیف های مبلغی حکم
        //        //newEmployeeInterdict.EmployeeInterdictDetails.Add(copyDetailsItem);
        //        Details.Add(copyDetailsItem);
        //    }

        //    if (model.Amount > 0)//در صورتی مبلغ داشته باشد حکم زده میشه
        //    {
        //        if (model.isBase)
        //        {
        //            SumBase += model.Amount;
        //        }
        //        else
        //        {
        //            SumOtherBase += model.Amount;
        //        }

        //        var newDetailsItem = new EmployeeInterdictDetail()
        //        {
        //            AccountCodeId = model.AccountCodeId,
        //            Amount = model.Amount,
        //            IsActive = true,
        //            InsertDate = model.InsertDate,
        //            InsertUser = model.InsertUser
        //        };
        //        //ثبت ردیف مبلغی جدید
        //        //newEmployeeInterdict.EmployeeInterdictDetails.Add(newDetailsItem);
        //        Details.Add(newDetailsItem);
        //    }

        //    var newEmployeeInterdict = new EmployeeInterdict()
        //    {
        //        EmployeeInterdictDetails = Details,//new List<EmployeeInterdictDetail>(),

        //        InsertDate = model.InsertDate,
        //        InsertUser = model.InsertUser,

        //        EmployeeId = LastEmployeeInterdict.EmployeeId,
        //        EmploymentTypeId = LastEmployeeInterdict.EmploymentTypeId,
        //        InterdictTypeId = model.InterdictTypeId,
        //        InterdictNumber = model.InterdictNumber,
        //        IssuanceDate = model.IssuanceDate,
        //        ExecuteDate = model.ExecuteDate,//barresi
        //        InterdictStartDate = LastEmployeeInterdict.InterdictStartDate,
        //        InterdictEndDate = LastEmployeeInterdict.InterdictEndDate,
        //        InterdictStatusId = LastEmployeeInterdict.InterdictStatusId,
        //        JobPositionId = LastEmployeeInterdict.JobPositionId,
        //        SupervisionScore = LastEmployeeInterdict.SupervisionScore,
        //        SpecialLiabilityScore = LastEmployeeInterdict.SpecialLiabilityScore,
        //        HardWorkScore = LastEmployeeInterdict.HardWorkScore,
        //        CurrentJobGroupId = LastEmployeeInterdict.CurrentJobGroupId,
        //        SignatureImageId = LastEmployeeInterdict.SignatureImageId,
        //        ReasonJobMovingId = LastEmployeeInterdict.ReasonJobMovingId,
        //        InterdictDescriptionId = LastEmployeeInterdict.InterdictDescriptionId,
        //        BasijPercentage = LastEmployeeInterdict.BasijPercentage,
        //        MeritPercentage = LastEmployeeInterdict.MeritPercentage,
        //        RadiationPercentage = LastEmployeeInterdict.RadiationPercentage,

        //        PortalFinalConfirmFlag = false,

        //        SumPriceBasisSalary = SumBase,
        //        SumPriceIndependentBasisSalary = SumOtherBase,


        //    };


        //    // حکم جدید فرد
        //    return newEmployeeInterdict;
        //}

        public EmployeeInterdict AddByModelAccountCodes(EmployeeInterdict LastEmployeeInterdict, EmployeeInterdictFamilyViewModel model)
        {
            long SumBase = 0;
            long SumOtherBase = 0;
            List<EmployeeInterdictDetail> Details = new List<EmployeeInterdictDetail>();

            foreach (var detailItem in LastEmployeeInterdict.EmployeeInterdictDetails)
            {
                if (model.SelectedAccountCodes.Select(x => x.AccountCodeId).Contains(detailItem.AccountCodeId))
                    continue; // داده مربوطی که ارسال شده رد کن

                if (detailItem.AccountCode.AccountCodeCategoryId == EnumAccountCodeCategroy.BasicSalary.Id)
                {
                    SumBase += detailItem.Amount;
                }
                else
                {
                    SumOtherBase += detailItem.Amount;
                }

                var copyDetailsItem = new EmployeeInterdictDetail()
                {
                    AccountCodeId = detailItem.AccountCodeId,
                    Amount = detailItem.Amount,
                    IsActive = detailItem.IsActive,
                    InsertDate = model.InsertDate,
                    InsertUser = model.InsertUser,
                };
                //ثبت ردیف های مبلغی حکم
                //newEmployeeInterdict.EmployeeInterdictDetails.Add(copyDetailsItem);
                Details.Add(copyDetailsItem);
            }
            foreach (var accountCodeItem in model.SelectedAccountCodes)
            {
                if (accountCodeItem.Amount > 0)//(model.Amount > 0)//در صورتی مبلغ داشته باشد حکم زده میشه
                {
                    if (accountCodeItem.isBase)//(model.isBase)
                    {
                        SumBase += accountCodeItem.Amount;//model.Amount;
                    }
                    else
                    {
                        SumOtherBase += accountCodeItem.Amount; //model.Amount;
                    }

                    var newDetailsItem = new EmployeeInterdictDetail()
                    {
                        AccountCodeId = accountCodeItem.AccountCodeId, //model.AccountCodeId,
                        Amount = accountCodeItem.Amount, //model.Amount,
                        IsActive = true,
                        InsertDate = model.InsertDate,
                        InsertUser = model.InsertUser
                    };
                    //ثبت ردیف مبلغی جدید
                    //newEmployeeInterdict.EmployeeInterdictDetails.Add(newDetailsItem);
                    Details.Add(newDetailsItem);
                }
            }
            var newEmployeeInterdict = new EmployeeInterdict()
            {
                EmployeeInterdictDetails = Details,//new List<EmployeeInterdictDetail>(),

                InsertDate = model.InsertDate,
                InsertUser = model.InsertUser,

                EmployeeId = LastEmployeeInterdict.EmployeeId,
                EmploymentTypeId = LastEmployeeInterdict.EmploymentTypeId,
                InterdictTypeId = model.InterdictTypeId,
                InterdictNumber = model.InterdictNumber,
                IssuanceDate = model.IssuanceDate,
                ExecuteDate = model.ExecuteDate,//barresi
                ExecuteDateYearMonth = int.Parse($"{model.ExecuteDate.GetPersianYear()}{model.ExecuteDate.GetPersianMonth().Value.ToString("00")}"),
                InterdictStartDate = LastEmployeeInterdict.InterdictStartDate,
                InterdictEndDate = LastEmployeeInterdict.InterdictEndDate,
                InterdictStatusId = LastEmployeeInterdict.InterdictStatusId,
                JobPositionId = LastEmployeeInterdict.JobPositionId,
                SupervisionScore = LastEmployeeInterdict.SupervisionScore,
                SpecialLiabilityScore = LastEmployeeInterdict.SpecialLiabilityScore,
                HardWorkScore = LastEmployeeInterdict.HardWorkScore,
                CurrentJobGroupId = LastEmployeeInterdict.CurrentJobGroupId,
                SignatureImageId = LastEmployeeInterdict.SignatureImageId,
                ReasonJobMovingId = LastEmployeeInterdict.ReasonJobMovingId,
                InterdictDescriptionId = LastEmployeeInterdict.InterdictDescriptionId,
                BasijPercentage = LastEmployeeInterdict.BasijPercentage,
                MeritPercentage = LastEmployeeInterdict.MeritPercentage,
                RadiationPercentage = LastEmployeeInterdict.RadiationPercentage,

                PortalFinalConfirmFlag = false,

                SumPriceBasisSalary = SumBase,
                SumPriceIndependentBasisSalary = SumOtherBase,


            };


            // حکم جدید فرد
            return newEmployeeInterdict;
        }


        public async Task<bool> AddBulkAsync(List<EmployeeInterdict> list)
        {
            try
            {
                await _kscHrContext.BulkInsertOrUpdateAsync(list, option =>
                {
                    option.IncludeGraph = true;
                    option.UseTempDB = true;
                });

                await _kscHrContext.BulkSaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }


        public bool CheckRepateDateInssuingCreate(DateTime InssuingDate, int employeeId)
        {
            var isExist = _kscHrContext.EmployeeInterdicts.Any(x => x.EmployeeId == employeeId && x.IssuanceDate == InssuingDate);
            return isExist;
        }

        public bool CheckRepateDateInssuingUpdate(DateTime InssuingDate, int employeeId, int interdictId)
        {
            var isExist = _kscHrContext.EmployeeInterdicts.Any(x => x.Id != interdictId && x.EmployeeId == employeeId && x.IssuanceDate == InssuingDate);
            return isExist;
        }


        public bool CheckRepateDateExecuteCreate(DateTime ExecuteDate, int employeeId)
        {
            var isExist = _kscHrContext.EmployeeInterdicts.Any(x => x.EmployeeId == employeeId && x.ExecuteDate == ExecuteDate);
            return isExist;
        }

        public bool CheckRepateDateExecuteUpdate(DateTime ExecuteDate, int employeeId, int interdictId, bool isAmendment)
        {
            var isExist = _kscHrContext.EmployeeInterdicts.Any(x => (x.Id != interdictId || isAmendment) && x.EmployeeId == employeeId && x.ExecuteDate == ExecuteDate);
            return isExist;
        }
        public IQueryable<EmployeeInterdict> GetEmployeeInterdictByEmployeeIdForPortal(int employeeId)
        {
            var query = _kscHrContext.EmployeeInterdicts
                  .Where(x => x.EmployeeId == employeeId && x.PortalFinalConfirmFlag.HasValue && x.PortalFinalConfirmFlag.Value)
                  .Include(x => x.InterdictType)
                  .Include(x => x.Chart_JobPosition)
                  ;
            return query;
        }

        public async Task<EmployeeInterdict> GetEmployeeInterdictByIdForPortal(int id)
        {
            var query = await _kscHrContext.EmployeeInterdicts
                 .Include(x => x.InterdictType)
                 .Include(x => x.Chart_JobPosition)
                 .FirstOrDefaultAsync(x => x.Id == id);
            return query;
        }


        public IQueryable<EmployeeInterdict> GetDataByYearMonth(int yearmonth)
        {
            var result = _kscHrContext.EmployeeInterdicts
               .Where(x => x.SendMISYearMonth == yearmonth);

            return result;
        }
        public async Task<EmployeeInterdict> GetEmployeeInterdictByIdForPrint(int id)
        {
            var query = await _kscHrContext.EmployeeInterdicts
                 .Include(x => x.InterdictType)
                 .Include(x => x.EmployeeBase_EmploymentType)
                 .Include(x => x.InterdictDescription)
                 .Include(x => x.Chart_JobPosition)
                 .ThenInclude(x => x.Chart_JobIdentity)
                  .Include(x => x.Employee)
                  .ThenInclude(x => x.EmployeeEducationDegrees)
                  .Include(x => x.Employee)
                  .ThenInclude(x => x.CertificateCity)
                  .Include(x => x.Employee)
                  .ThenInclude(x => x.MaritalStatus)
                   .Include(x => x.Employee)
                  .ThenInclude(x => x.MilitaryStatus)
                   .Include(x => x.Employee)
                   .ThenInclude(x => x.TeamWork)
                    .Include(x => x.Employee)
                   .ThenInclude(x => x.WorkGroup)
                   .ThenInclude(x => x.WorkTime)
                 .FirstOrDefaultAsync(x => x.Id == id);
            return query;
        }




        public async Task<List<EmployeeInterdict>> GetEmployeeInterdictByEmployeementTypeForPrint()
        {
            var firstYearmonth = int.Parse((DateTime.Now.GetPersianYear() + "01"));
            var query = await _kscHrContext.EmployeeInterdicts.Include(x => x.Employee)
                  .ThenInclude(x => x.EmployeeEducationDegrees)
                  .Where(x => x.ExecuteDateYearMonth == firstYearmonth &&
                x.Employee.EmploymentTypeId == 4 &&
                x.InterdictTypeId == EnumInterdictType.AnnualSalaryIncrease.Id)
                 .Include(x => x.InterdictType)
                 .Include(x => x.EmployeeBase_EmploymentType)
                 .Include(x => x.InterdictDescription)
                 .Include(x => x.Chart_JobPosition)
                 .ThenInclude(x => x.Chart_JobIdentity)
                  
                  .Include(x => x.Employee)
                  .ThenInclude(x => x.CertificateCity)
                  .Include(x => x.Employee)
                  .ThenInclude(x => x.MaritalStatus)
                   .Include(x => x.Employee)
                  .ThenInclude(x => x.MilitaryStatus)
                   .Include(x => x.Employee)
                   .ThenInclude(x => x.TeamWork)
                    .Include(x => x.Employee)
                   .ThenInclude(x => x.WorkGroup)
                   .ThenInclude(x => x.WorkTime).AsNoTracking().ToListAsync();
            return query;
        }

        public bool DeleteById(int id)
        {
            var employeeInterdict = _kscHrContext.EmployeeInterdicts
                  .Include(x => x.EmployeeInterdictDetails).FirstOrDefault(a => a.Id == id);
            if (employeeInterdict != null)
            {
                _kscHrContext.EmployeeInterdictDetails.RemoveRange(employeeInterdict.EmployeeInterdictDetails);
                _kscHrContext.EmployeeInterdicts.Remove(employeeInterdict);
                return true;
            }
            else
            {
                return false;
            }

        }
        public IQueryable<EmployeeInterdict> GetEmployeeInterdicttyEmployeeId(int employeeId)
        {
            var result = _kscHrContext.EmployeeInterdicts.Where(x => x.EmployeeId == employeeId);
            return result;
        }


        /// <summary>
        /// مبلغ 1 ساعت اضافه کاری
        /// </summary>
        /// <param name="yearMonth"></param>
        /// <returns></returns>
        public long? OverTimeAmountByHour(int employeeId)
        {

            long? overTimeAmountByHour = 0;
            try
            {
                var employeeInterdict = _kscHrContext.EmployeeInterdicts
                    .Where(x => x.EmployeeId == employeeId).OrderByDescending(x => x.ExecuteDate).FirstOrDefault();

                if (employeeInterdict != null)
                {
                    var sumPriceBasisSalary = employeeInterdict.SumPriceBasisSalary;

                    overTimeAmountByHour = Convert.ToInt64((sumPriceBasisSalary / 220) * 1.4);
                }
            }
            catch (Exception ex)
            {
                return overTimeAmountByHour;

            }

            return overTimeAmountByHour;

        }


        /// <summary>
        ///  مبلغ 1 ساعت اضافه کاری
        /// </summary>
        /// <param name="yearMonth"></param>
        /// <returns></returns>
        public List<Tuple<int, long?>> OverTimeAmountItems(List<int> employeeIds)
        {
            long? overTimeAmountByHour = 0;
            var items = new List<Tuple<int, long?>>();
            try
            {
                var employeeQuery = _kscHrContext.Employees
               .AsQueryable().AsNoTracking()
               .Where(a => employeeIds.Any(x => x == a.Id))
               .Include(a => a.EmployeeInterdicts)
               .Select(a => new
               {
                   a.Id,
                   a.EmployeeNumber,
                   a.EmploymentTypeId,
                   LastEmployeeInterDicts = a.EmployeeInterdicts
                   .OrderByDescending(x => x.ExecuteDate)
                   .FirstOrDefault(),

               })
               .ToList();
                foreach (var employee in employeeQuery)
                {

                    if (employee.LastEmployeeInterDicts != null)
                    {
                        var sumPriceBasisSalary = employee.LastEmployeeInterDicts.SumPriceBasisSalary;

                        overTimeAmountByHour = Convert.ToInt64((sumPriceBasisSalary / 220) * 1.4);

                        var item = new Tuple<int, long?>(employee.Id, overTimeAmountByHour);
                        items.Add(item);
                    }

                }



            }
            catch (Exception ex)
            {
                return items;

            }

            return items;

        }







        //محاسبه مسئولیت خاص
        public long GetSpecialLiability(List<CoefficientSetting> CoefficientSettings, double? specialLiabilityScore, decimal tableFactor)
        {

            //مسئولیت خاص
            var specialLiabilityFactor = CoefficientSettings
                                              .Where(x => x.CoefficientId == EnumRuleCoefficient.SpecialLiability.Id)
                                              .Select(x => x.Value)
                                              .FirstOrDefault();


            var specialLiability = (specialLiabilityScore != null) ? (decimal)specialLiabilityScore.Value : 0;
            var specialLiabilityValue = specialLiability * tableFactor * specialLiabilityFactor * 30;
            var result = (long)Math.Round(specialLiabilityValue);

            return result;


        }

        //محاسبه سختی کار
        public long GetHardWork(List<CoefficientSetting> CoefficientSettings, double? HardWorkScoreFilter, decimal tableFactor)
        {
            #region hardWork
            //ضریب سختی کار
            var hardWorkFactor = CoefficientSettings
            .Where(x => x.CoefficientId == EnumRuleCoefficient.HardWorkFactor.Id)
            .Select(x => x.Value)
            .FirstOrDefault();


            var hardWorkScore = (HardWorkScoreFilter != null) ? (decimal)HardWorkScoreFilter.Value : 0;
            var hardWorkFactorValue = hardWorkScore * tableFactor * hardWorkFactor * 30;
            var result = (long)Math.Round(hardWorkFactorValue);
            return result;

            #endregion
        }

        //حق سرپرستی
        public long GetSupervision(List<CoefficientSetting> CoefficientSettings, double? SupervisionScoreFilter, decimal tableFactor)
        {
            #region Supervision Scor
            var supervisionFactor = CoefficientSettings
                                    .Where(x =>
                                    x.CoefficientId == EnumRuleCoefficient.SupervisionScor.Id)
                                    .Select(x => x.Value)
                                    .FirstOrDefault();


            var supervisionScore = (SupervisionScoreFilter != null) ? (decimal)SupervisionScoreFilter.Value : 0;
            var supervisionScoreValue = supervisionScore * tableFactor * supervisionFactor * 30;
            var result = (long)Math.Round(supervisionScoreValue);
            return result;

            #endregion
        }

        //محاسبه حق اشعه
        public long GetRadiationPercentage(double? RadiationPercentageFilter, decimal GroupJob, decimal years)
        {
            var radiationPercentage = (RadiationPercentageFilter != null) ? (decimal)RadiationPercentageFilter.Value : 0;

            var radiation = (GroupJob + years) * radiationPercentage / 100;
            var result = (long)Math.Floor(radiation);
            return result;
        }

        public IQueryable<EmployeeInterdict> GetLastesInterdictsPerMonthYearForInsert()
        {
            var g = _kscHrContext.EmployeeInterdicts
                .Include(x => x.EmployeeInterdictDetails)
                .ThenInclude(x => x.AccountCode);


            return g;
        }
        public IQueryable<EmployeeInterdict> GetLastesInterdictsPerMonthYear()
        {
            var g = _kscHrContext.EmployeeInterdicts
                .Include(x => x.EmployeeInterdictDetails)
                .ThenInclude(x => x.AccountCode)
                .Include(x => x.Employee)
                .Include(x => x.Chart_JobPosition);


            return g;
        }


        public List<EmployeeInterdict> GetPersonelInterdictChangeJobPosition(List<EmployeeInterdict> interdicts)
        {
            var filnalResult = new List<EmployeeInterdict>();

            try
            {
                var result = interdicts.GroupBy(x => new
                {
                    executeYearMonth = x.ExecuteDateYearMonth,
                    empId = x.EmployeeId
                });
                var farvardinMonth = result.Where(x => x.Key.executeYearMonth.Value.GetMonthFromYearMonth() == 1).Select(x => new
                {
                    maxexecuteDate = x.Max(y => y.ExecuteDate),
                    executeYearMonth = x.Key.executeYearMonth,
                    empId = x.Key.empId,
                    interdict = x.Where(y => y.ExecuteDate == x.Min(y => y.ExecuteDate)).FirstOrDefault()
                }).ToList();
                var otherMonth = result.Where(x => x.Key.executeYearMonth.Value.GetMonthFromYearMonth() > 1).Select(x => new
                {
                    maxexecuteDate = x.Max(y => y.ExecuteDate),
                    executeYearMonth = x.Key.executeYearMonth,
                    empId = x.Key.empId,
                    interdict = x.Where(y =>y.ExecuteDate == x.Max(y => y.ExecuteDate)).FirstOrDefault()
                });
                var interdictsEmployeeForIncreaseFarvadin = farvardinMonth.Select(x => x.interdict).ToList();
                var interdictsEmployeeForIncreaseOther = otherMonth.Select(x => x.interdict).ToList();
                filnalResult.AddRange(interdictsEmployeeForIncreaseFarvadin);
                filnalResult.AddRange(interdictsEmployeeForIncreaseOther);
            }
            catch (Exception ex)
            {

            }


            return filnalResult;
        }

        
    }
}
