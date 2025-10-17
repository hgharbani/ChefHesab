using EFCore.BulkExtensions;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Domain.Repositories.Pay;
using Ksc.HR.DTO.Pay.EmployeeDeductionDetail;
using Ksc.HR.DTO.Pay.EmployeeDeductionHeader;
using Ksc.HR.Share.General;
using Ksc.HR.Share.Model;
using Ksc.HR.Share.Model.DeductionAdditional;
using Ksc.HR.Share.Model.Pay;
using KSC.Common.Filters.Models;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Ksc.HR.Data.Persistant.Repositories.Pay
{
    public class EmployeeDeductionHeaderRepository : EfRepository<EmployeeDeductionHeader, long>, IEmployeeDeductionHeaderRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeeDeductionHeaderRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }


        public long GetHeaderIdBybasisId(int YearMonth, int EmployeeId, int EmployeeDeductionTypeId)
        {
            var query = _kscHrContext.EmployeeDeductionHeader.Where(x => x.YearMonth == YearMonth &&
            x.EmployeeId == EmployeeId
            &&
            x.EmployeeDeductionTypeId == EmployeeDeductionTypeId
            ).Select(x => x.Id).FirstOrDefault();
            return query;
        }

        public long GetHeaderIdByKey(int keyId)
        {
            var query = _kscHrContext.EmployeeDeductionHeader
                .Where(x => x.Id == keyId)
                .Select(x => x.Id)
                .FirstOrDefault();
            return query;
        }


        public IQueryable<EmployeeDeductionHeader> GetEmployeeDeductionHeader()
        {


            var query = _kscHrContext.EmployeeDeductionHeader
                .Include(x => x.EmployeeDeductionType)
                .Include(x => x.Employee)
                 ;
            return query;
        }
        public IQueryable<EmployeeDeductionHeader> GetEmployeeDeductionHeaderByRelated()
        {
            var query = _kscHrContext.EmployeeDeductionHeader
               .Include(x => x.EmployeeDeductionDetails)
               .ThenInclude(z => z.EmployeeDeductionStatus)
               .Include(x => x.EmployeeDeductionInstallmentPutOffs)
               .Include(x => x.AccountCode)
               .Include(x => x.EmployeeDeductionType)
                .Include(x => x.Employee)
            ;

            return query;
        }
        public IQueryable<EmployeeDeductionHeader> GetEmployeeDeductionHeaderByRole(List<string> roles)
        {

            IQueryable<EmployeeDeductionHeader> result = _kscHrContext.EmployeeDeductionHeader
               .Include(x => x.EmployeeDeductionDetails)
               .ThenInclude(z => z.EmployeeDeductionStatus)
               .Include(x => x.EmployeeDeductionInstallmentPutOffs)
               .Include(x => x.AccountCode)
               .Include(x => x.EmployeeDeductionType)
                .Include(x => x.Employee);

            var employeeDeductionTypeAccessId = _kscHrContext.EmployeeDeductionTypeAccess.Where(a => roles.Contains(a.RoleName))
                .Select(x => x.EmployeeDeductionTypeId).Distinct().ToList();

            if (employeeDeductionTypeAccessId.Any())

            {
                result = result.Where(x => employeeDeductionTypeAccessId.Contains(x.EmployeeDeductionTypeId));
            }

            return result;





            //var query = _kscHrContext.EmployeeDeductionHeader
            //   .Include(x => x.EmployeeDeductionDetails)
            //   .ThenInclude(z => z.EmployeeDeductionStatus)
            //   .Include(x => x.EmployeeDeductionInstallmentPutOffs)
            //   .Include(x => x.AccountCode)
            //   .Include(x => x.EmployeeDeductionType)
            //    .Include(x => x.Employee)
            //;

            //return query;
        }
        public IQueryable<EmployeeDeductionHeader> GetEmployeeDeductionHeaderDetail()
        {
            var query = _kscHrContext.EmployeeDeductionHeader
               .Include(x => x.EmployeeDeductionDetails)
               .ThenInclude(z => z.EmployeeDeductionStatus)
               .Include(x => x.AccountCode)
               .Include(x => x.EmployeeDeductionType)
                .Include(x => x.Employee)
            ;

            return query;
        }

        //بدهی
        public void InsertEmployeeDeduction(EmployeeDeductionDto dto)
        {

            //var systemControlDate = _kscHrContext.SystemControlDateRepository.GetActiveData();
            //var YearMonth = systemControlDate.AttendAbsenceItemDate;
            //if (_kscHrContext.DeductionAdditionals.Any(a => a.AccountCodeId == dto.AccountCodeId 
            //&& a.YearMonthStart == dto.YearMonthStart 
            //&& a.YearMonthEnd == dto.YearMonthEnd 
            //&& a.StatusId == EnumDeductionAdditionalStatus.Create.Id))
            //{
            //    var data = _kscHrContext.DeductionAdditionals.Where(a => a.AccountCodeId == dto.AccountCodeId && a.YearMonthStart == dto.YearMonthStart && a.YearMonthEnd == dto.YearMonthEnd && a.StatusId == EnumDeductionAdditionalStatus.Create.Id).Include(a => a.EmployeeDeductionAdditionals).First();
            //    _kscHrContext.EmployeeDeductionAdditionals.RemoveRange(data.EmployeeDeductionAdditionals);
            //    _kscHrContext.DeductionAdditionals.Remove(data);
            //}
            var employeeDeductionmodel = new EmployeeDeductionHeader()
            {
                EmployeeId = dto.EmployeeId,
                EmployeeDeductionTypeId = dto.EmployeeDeductionTypeId,
                AccountCodeId = dto.AccountCodeId,
                TotalDeductionAmount = dto.TotalDeductionAmount,
                InstallmentCount = dto.InstallmentCount,
                InsertDate = DateTime.Now,
                InsertUser = dto.InsertUser,
                YearMonth = dto.YearMonthStart,
                Description = dto.Description,
                BankLoanNumber = dto.BankLoanNumber,
                DomainName = dto.DomainName,
                IsActive = true,
            };
            foreach (var item in dto.DeductionDetails)
            {
                employeeDeductionmodel.EmployeeDeductionDetails.Add(new EmployeeDeductionDetail()
                {

                    InstallmentDate = item.InstallmentDate,
                    InstallmentAmount = item.InstallmentAmount,
                    InstallmentPaymentDate = item.InstallmentDate,
                    EmployeeDeductionStatusId = EnumEmployeeDeductionStatus.DeductionCreate.Id //ایجاد
                });
            }
            _kscHrContext.Add(employeeDeductionmodel);

        }

        public bool RemoveEmployeeDeductionById(long id)
        {

            try
            {


                var employeeDeduction = _kscHrContext.EmployeeDeductionHeader
                      .Include(x => x.EmployeeDeductionDetails)
                      .FirstOrDefault(a => a.Id == id);
                if (employeeDeduction == null)
                {
                    return false;
                }


                //if (employeeDeduction != null && !haspaidStatuse)
                //{
                _kscHrContext.EmployeeDeductionDetail.RemoveRange(employeeDeduction.EmployeeDeductionDetails);
                _kscHrContext.EmployeeDeductionHeader.Remove(employeeDeduction);
                _kscHrContext.SaveChanges();
                return true;
                //}
            }
            catch (Exception ex)
            {
                return false;
            }

        }


        //public List<PaidableEmployeeDeductionListDto> GetPaidableEmployeeDeduction(int installmentPaymentDate)
        //{
        //    var result = new List<PaidableEmployeeDeductionListDto>();
        //    var query = _kscHrContext.EmployeeDeductionHeader.AsNoTracking();

        //    var Details = _kscHrContext.EmployeeDeductionDetail.AsNoTracking();

        //    var GetNotSavingLoad = query.Where(x => x.EmployeeDeductionTypeId != EnumEmployeeDeductionType.SavingLoan.Id)
        //      .Where(x => x.EmployeeDeductionDetails
        //      .Any(z => z.InstallmentPaymentDate == installmentPaymentDate // ماه مرتبط با تاریخ حقوق دستمزد?
        //                && (z.EmployeeDeductionStatusId == EnumEmployeeDeductionStatus.DeductionCreate.Id || z.EmployeeDeductionStatusId == EnumEmployeeDeductionStatus.Confirm.Id))
        //      )
        //      .Select(p => new LoadModel()
        //      {
        //          Id = p.Id,

        //          EmployeeId = p.EmployeeId,
        //          //EmployeeNumber = p.Employee.EmployeeNumber,
        //          //NameFamily = p.Employee.Name + " " + p.Employee.Family,
        //          //کد حساب 
        //          AccountCodeId = p.AccountCodeId,
        //          //AccountCodeTitle = p.AccountCode.Title,
        //          //نوع بدهی
        //          EmployeeDeductionTypeId = p.EmployeeDeductionTypeId,
        //          //EmployeeDeductionTypeTitle = p.EmployeeDeductionType.Title,

        //          TotalDeductionAmount = p.TotalDeductionAmount,//مبلغ کل بدهي 

        //          BankLoanNumber = p.BankLoanNumber,//شماره تسهيلات 

        //          InstallmentCount = p.InstallmentCount, // تعداد کل اقساط 
        //      }).ToList();
        //    //.Include(x => x.Employee)
        //    //.Include(x => x.EmployeeDeductionType)
        //    //.Include(x => x.AccountCode)
        //    ;

        //    var NotSavingLoadheaderId = GetNotSavingLoad.Select(a => a.Id).ToList();
        //    var NotSavingLoadDetails = Details.Where(a => NotSavingLoadheaderId.Contains(a.EmployeeDeductionHeaderId)).ToList();





        //    //.Select(p => GetPaidableEmployeeDeductionListDto(p, installmentPaymentDate))
        //    //.ToList();

        //    var GetSavingLoad = query.Where(x => x.EmployeeDeductionTypeId == EnumEmployeeDeductionType.SavingLoan.Id && x.YearMonth == installmentPaymentDate)
        //         .Select(p => new LoadModel()
        //         {
        //             Id = p.Id,

        //             EmployeeId = p.EmployeeId,
        //             //EmployeeNumber = p.Employee.EmployeeNumber,
        //             //NameFamily = p.Employee.Name + " " + p.Employee.Family,
        //             //کد حساب 
        //             AccountCodeId = p.AccountCodeId,
        //             //AccountCodeTitle = p.AccountCode.Title,
        //             //نوع بدهی
        //             EmployeeDeductionTypeId = p.EmployeeDeductionTypeId,
        //             //EmployeeDeductionTypeTitle = p.EmployeeDeductionType.Title,

        //             TotalDeductionAmount = p.TotalDeductionAmount,//مبلغ کل بدهي 

        //             BankLoanNumber = p.BankLoanNumber,//شماره تسهيلات 

        //             InstallmentCount = p.InstallmentCount, // تعداد کل اقساط 
        //         }).ToList();
        //    //.Include(x => x.Employee)
        //    //.Include(x => x.EmployeeDeductionType)
        //    //.Include(x => x.AccountCode)
        //    ;
        //    var savingLoadheaderId = GetNotSavingLoad.Select(a => a.Id).ToList();
        //    var savingLoadDetails = Details.Where(a => savingLoadheaderId.Contains(a.EmployeeDeductionHeaderId)).ToList();


        //    // .Select(p => GetPaidableEmployeeDeductionListDto(p, installmentPaymentDate))
        //    //.ToList();
        //    var uniondetail = NotSavingLoadDetails.Union(savingLoadDetails).OrderBy(x => x.EmployeeDeductionHeaderId).ToList();


        //    var unionlist_p = (GetNotSavingLoad.Union(GetSavingLoad)).ToList();

        //    //var unionlist = unionlist_p
        //    //    .Select(a=> GetPaidableEmployeeDeductionListDto(a, installmentPaymentDate, uniondetail))
        //    //    .ToList();
        //    //    ;
        //    foreach (var a in unionlist_p)
        //    {
        //        var details = uniondetail.Where(p => p.EmployeeDeductionHeaderId == a.Id).ToList();
        //        var item = GetPaidableEmployeeDeductionListDto(a, installmentPaymentDate, details);
        //        result.Add(item);
        //    }

        //    /////من فقط هدرهایی میخوام که در تاریخ های مد نظرم دیتیل دارند
        //    //var data = GetEmployeeDeductionHeaderDetail().AsNoTracking()
        //    //    .Where(x =>
        //    //    (x.EmployeeDeductionTypeId != EnumEmployeeDeductionType.SavingLoan.Id // بجز وام ها
        //    //    && x.EmployeeDeductionDetails
        //    //    .Any(z => z.InstallmentPaymentDate == installmentPaymentDate // ماه مرتبط با تاریخ حقوق دستمزد?
        //    //              && (z.EmployeeDeductionStatusId == EnumEmployeeDeductionStatus.DeductionCreate.Id || z.EmployeeDeductionStatusId == EnumEmployeeDeductionStatus.Confirm.Id) //// وضعیت ایجاد و تایید باشند
        //    //    ))
        //    //    ||
        //    //    (x.EmployeeDeductionTypeId == EnumEmployeeDeductionType.SavingLoan.Id // وام ها
        //    //    && x.YearMonth == installmentPaymentDate // ماه مرتبط با تاریخ حقوق دستمزد?
        //    //    )
        //    //    )
        //    //    .Select(p => GetPaidableEmployeeDeductionListDto(p, installmentPaymentDate))
        //    //    .ToList();


        //    return result;
        //}

        //public static PaidableEmployeeDeductionListDto GetPaidableEmployeeDeductionListDto(LoadModel p, int installmentPaymentDate, List<EmployeeDeductionDetail> details)
        //{

        //    var result = new PaidableEmployeeDeductionListDto
        //    {
        //        //شماره پرسنلی 
        //        EmployeeId = p.EmployeeId,
        //        //EmployeeNumber = p.Employee.EmployeeNumber,
        //        //NameFamily = p.Employee.Name + " " + p.Employee.Family,
        //        //کد حساب 
        //        AccountCodeId = p.AccountCodeId,
        //        //AccountCodeTitle = p.AccountCode.Title,
        //        //نوع بدهی
        //        EmployeeDeductionTypeId = p.EmployeeDeductionTypeId,
        //        //EmployeeDeductionTypeTitle = p.EmployeeDeductionType.Title,

        //        TotalDeductionAmount = p.TotalDeductionAmount,//مبلغ کل بدهي 

        //        BankLoanNumber = p.BankLoanNumber,//شماره تسهيلات 

        //        InstallmentCount = p.InstallmentCount, // تعداد کل اقساط 

        //        // تاريخ پايان 
        //        InstallmentPaymentDateMax = details.Select(x => x.InstallmentPaymentDate).Max(),

        //        // تعداد اقساط جاري 
        //        CountOfPaidDetails = (details.Where(x => x.EmployeeDeductionStatusId == EnumEmployeeDeductionStatus.Paid.Id).Count()),

        //        // تعداد اقساط جاري تا کنون
        //        CountOfPaidDetailsWithCurrent = 1 + (details.Where(x => x.EmployeeDeductionStatusId == EnumEmployeeDeductionStatus.Paid.Id).Count()),

        //        //تاریخ پرداخت
        //        InstallmentPaymentDate = installmentPaymentDate,

        //        //مبلغ پرداخت شده تا کنون 
        //        SumAmountOfPaidDetails = details.Where(x => x.EmployeeDeductionStatusId == EnumEmployeeDeductionStatus.Paid.Id).Select(x => x.InstallmentAmount).Sum(),

        //        //مبلغ قسط اول 
        //        //مبلغ قسط اول : فقط برای نوع وام ارسال میشود با توجه به جدول جزییات اولین رکورد ارسال میشود. (فقط زمانی وام هر فرد ارسال شود که تمام اقساط در حالت ایجاد هستند)
        //        FirstInstallmentAmount = (details.OrderBy(x => x.InstallmentPaymentDate).Select(x => x.InstallmentAmount).FirstOrDefault()),
        //        //مبلغ ساير اقساط
        //        //مبلغ ساير اقساط : فقط برای نوع وام ارسال میشود مبلغ آخرین قسط (فقط زمانی وام هر فرد ارسال شود که تمام اقساط در حالت ایجاد هستند)
        //        OtherInstallmentAmount = (details.OrderByDescending(x => x.InstallmentPaymentDate).Select(x => x.InstallmentAmount).FirstOrDefault()),

        //        ////مبلغ قسط جاری
        //        InstallmentAmount = details
        //                                                  .Where(d => d.InstallmentPaymentDate == installmentPaymentDate
        //                                                        && (d.EmployeeDeductionStatusId == EnumEmployeeDeductionStatus.DeductionCreate.Id
        //                                                            || d.EmployeeDeductionStatusId == EnumEmployeeDeductionStatus.Confirm.Id)
        //                                                        ).Select(d => d.InstallmentAmount).FirstOrDefault(),


        //    };

        //    return result;
        //}

        public List<PaidableEmployeeDeductionListDto> Sp_PaidableEmployeeDeduction(int startShamsiDate, int accountCodeId, int deductionTypeId)
        {
            var MyList = _kscHrContext
                .CollectionFromSqlReturnGeneric<Sp_PaidableEmployeeDeductionDto>("EXEC [dbo].[Sp_PaidableEmployeeDeduction] @yearMonth, @accountCodeId ,@deductionTypeId",
                new Dictionary<string, object> {
                 { "@yearMonth", startShamsiDate},
                 { "@accountCodeId", accountCodeId},
                 { "@deductionTypeId", deductionTypeId},
                }).ToList();

            var result = MyList.Select(x => new PaidableEmployeeDeductionListDto
            {
                AccountCodeId = x.AccountCodeId,
                AccountCodeTitle = x.AccountCodeTitle,
                BankLoanNumber = x.BankLoanNumber,
                CountOfPaidDetails = x.CountOfPaidDetails,
                DetailId = x.DetailId,
                EmployeeDeductionStatusId = x.EmployeeDeductionStatusId,
                EmployeeDeductionStatusTitle = EnumEmployeeDeductionStatus.GetAll<EnumEmployeeDeductionStatus>().FirstOrDefault(u => u.Id == x.EmployeeDeductionStatusId)?.Name,
                EmployeeDeductionTypeId = x.EmployeeDeductionTypeId,
                HeaderId = x.HeaderId,
                EmployeeId = x.EmployeeId,
                EmployeeDeductionTypeTitle = x.EmployeeDeductionTypeTitle,
                EmployeeNumber = x.EmployeeNumber,
                Family = x.Family,
                InstallmentAmount = x.InstallmentAmount,
                InstallmentCount = x.InstallmentCount,
                InstallmentPaymentDate = x.InstallmentPaymentDate,
                LastInstallmentAmount = x.LastInstallmentAmount,
                LastInstallmentPaymentDate = x.LastInstallmentPaymentDate,
                Name = x.Name,
                TotalDeductionAmount = x.TotalDeductionAmount,
                TotalPaidAmount = x.TotalPaidAmount,
                YearMonth = x.YearMonth,
                Description = x.Description,
                NeedConfirm = x.NeedConfirm,
                StatusForPerferazh = 1,// این اقلام در فیش حقوق باید اضافه شوند یا خیر؟ فقط بخاطر انتقال دیتا این اضافه شده
                MisSync = (x.InsertUser == "MisSync") ? true : false,

            }).ToList();
            return result;
        }



        public async Task<bool> AddBulkAsync(List<EmployeeDeductionHeader> list)
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

    }
    public class LoadModel
    {
        public EmployeeDeductionHeader Header { get; set; }
        public List<EmployeeDeductionDetail> Details { get; set; }
        public long Id { get; internal set; }
        public int EmployeeId { get; internal set; }
        public int? AccountCodeId { get; internal set; }
        public int EmployeeDeductionTypeId { get; internal set; }
        public long? TotalDeductionAmount { get; internal set; }
        public string BankLoanNumber { get; internal set; }
        public int? InstallmentCount { get; internal set; }
    }
}

