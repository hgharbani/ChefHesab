using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.ODSViews;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Domain.Entities.Salary;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Repositories.Pay;
using Ksc.HR.Share.Model.OtherPaymentStatus;
using Ksc.HR.Share.Model.Pay;
using Ksc.HR.Share.Model.PaymentStatus;
using KSC.Common;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Index.HPRtree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.Pay
{

    public class EmployeeOtherPaymentRepository : EfRepository<EmployeeOtherPayment, long>, IEmployeeOtherPaymentRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeeOtherPaymentRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<EmployeeOtherPayment> GetEmployeeOtherPaymentByMonthYear(int startDate, int endDate)
        {
            return _kscHrContext
                .EmployeeOtherPayment

                .Where(a =>
                 ((a.YearMonthStartReport <= startDate && a.YearMonthEndReport >= startDate) ||
                 (a.YearMonthStartReport <= endDate && a.YearMonthEndReport >= endDate) ||
                 (a.YearMonthStartReport >= startDate && a.YearMonthEndReport <= endDate))
            ).Include(x => x.EmployeeOtherPaymentType);
        }
        public IQueryable<EmployeeOtherPayment> GetEmployeeOtherPaymentInMonthYear(int startDate, int endDate, int otherPaymentType)
        {

            return GetEmployeeOtherPaymentByMonthYear(startDate, endDate)
                .Where(a => a.EmployeeOtherPaymentType.Any(x => x.OtherPaymentTypeId == otherPaymentType)
            );
        }


        public IQueryable<EmployeeOtherPayment> GetEmployeeOtherPaymentManagmentByMonthYear(int startDate, int endDate, List<int> ids)
        {
            var rrr = _kscHrContext
                .EmployeeOtherPayment
                .Where(a =>
                 (a.YearMonthStartReport >= startDate && a.YearMonthEndReport <= endDate) &&
                 ids.Contains(a.EmployeeId)
            ).Include(x => x.EmployeeOtherPaymentType);
            return rrr;
        }
        public IQueryable<EmployeeOtherPayment> GetEmployeeOtherPaymentManagmentInMonthYear(int startDate, int endDate, int otherPaymentType, List<int> ids)
        {

            return GetEmployeeOtherPaymentManagmentByMonthYear(startDate, endDate, ids)
                .Where(a => a.EmployeeOtherPaymentType.Any(x => x.OtherPaymentTypeId == otherPaymentType)
            );
        }



        public IQueryable<EmployeeOtherPayment> GetEmployeeOtherPaymentByMonthYear_AccountCode(int startDate, int endDate, int accountCodeId)
        {
            return GetEmployeeOtherPaymentByMonthYear(startDate, endDate)
                 .Include(x => x.Employee)
                 .Include(x => x.OtherPaymentHeader)
                 .Include(x => x.EmployeeOtherPaymentType)
                .Where(a => a.OtherPaymentHeader.AccountCodeId == accountCodeId
            );
        }

        public async Task<IQueryable<EmployeeOtherPayment>> GetEmployeeOtherPaymentReport()
        {
            return _kscHrContext.EmployeeOtherPayment
                                 .Include(x => x.Employee)
                                 .Include(x => x.OtherPaymentHeader)
                                 .ThenInclude(x => x.AccountCode)
                                 .ThenInclude(x => x.PaymentAccountCodes)
                                 .Include(x => x.EmployeeOtherPaymentType)
                                 .ThenInclude(x => x.OtherPaymentType)
                                 .AsNoTracking();
        }


        public bool IsExistEmployeeOtherPaymentInMonthYear(int startDate, int endDate, int otherPaymentType)
        {
            //var isExist = _kscHrContext
            //    .EmployeeOtherPayment
            //    .Any();

            //if (isExist)
            //{
            // var employees = GetEmployeeOtherPaymentByMonthYear(startDate, endDate);
            //    var isExistData = employees
            //                            .Any(a => a.EmployeeOtherPaymentType.Any(x => x.OtherPaymentTypeId == otherPaymentType));
            //    return isExistData;
            //}

            //return isExist;
            int year = int.Parse(startDate.ToString().Substring(0, 4));

            if (otherPaymentType == 5)
            {


                var query = _kscHrContext
                    .OtherPaymentHeader
                    .Include(x => x.OtherPaymentHeaderTypes)
                    .ThenInclude(x => x.OtherPaymentType)
    .Any(x => Convert.ToInt32(x.PaymentYearMonth.ToString().Substring(0, 4)) == year && x.OtherPaymentHeaderTypes.Any(y => y.OtherPaymentTypeId == otherPaymentType && y.OtherPaymentHeaderId == x.Id));

                return query;

            }
            else
            {
                var query = _kscHrContext.OtherPaymentHeader
                    .Include(x => x.OtherPaymentHeaderTypes)
                    .ThenInclude(x => x.OtherPaymentType)
                    .Any(x =>
                     (
                    (x.YearMonthStartReport == null && Convert.ToInt32(x.PaymentYearMonth.ToString().Substring(0, 4)) == year) ||
                     (x.YearMonthStartReport >= startDate ||
                     (x.YearMonthEndReport != null && x.YearMonthEndReport <= endDate && x.YearMonthStartReport >= startDate)
                     ))
                   && x.OtherPaymentHeaderTypes
                   .Any(y => y.OtherPaymentTypeId == otherPaymentType && y.OtherPaymentHeaderId == x.Id));

                return query;
            }

        }

        //در سال ماه مشخص پرداخت متفرقه وجود دارد
        public IQueryable<OtherPaymentHeader> IsExistEmployeeOtherPaymentInMonthYear(int startDate, int endDate, int otherPaymentType, int salaryDate)
        {


            var query = _kscHrContext.OtherPaymentHeader
                .Include(x => x.OtherPaymentHeaderTypes)
                .ThenInclude(x => x.OtherPaymentType)
                .Where(x =>
                 (
                 x.YearMonthStartReport == startDate ||
                 (x.YearMonthEndReport != null && x.YearMonthEndReport == endDate && x.YearMonthStartReport == startDate)
                 )
                 && x.PaymentYearMonth == salaryDate
               && x.OtherPaymentHeaderTypes
               .Any(y => y.OtherPaymentTypeId == otherPaymentType && y.OtherPaymentHeaderId == x.Id));

            return query;


        }


        public IQueryable<EmployeeOtherPayment> GetEmployeeOtherPaymentByHeaderId(int headerId)
        {
            var query = _kscHrContext.EmployeeOtherPayment.Where(x => x.OtherPaymentHeaderId == headerId)
                .Include(x => x.Employee)
                .ThenInclude(x => x.EmploymentType)
            .Include(x => x.Employee).ThenInclude(x => x.GenderType);
            return query;
        }
        public IQueryable<EmployeeOtherPayment> GetEmployeeOtherPaymentByHeaderIdNotIncluded(int headerId)
        {
            var query = _kscHrContext.EmployeeOtherPayment.Where(x => x.OtherPaymentHeaderId == headerId);
            return query;
        }
        public IQueryable<EmployeeOtherPayment> GetEmployeeOtherPaymentByHeaderIdForDelete(int headerId)
        {
            var query = _kscHrContext.EmployeeOtherPayment.Where(x => x.OtherPaymentHeaderId == headerId)
                .Include(x => x.EmployeeOtherPaymentHistory)
                .Include(x => x.EmployeeOtherPaymentType);
            return query;
        }
        public IQueryable<EmployeeOtherPayment> GetEmployeeOtherPaymentByHeaderIdIncludedEmployee(int headerId)
        {
            var query = _kscHrContext.EmployeeOtherPayment.Where(x => x.OtherPaymentHeaderId == headerId)
                .Include(x => x.Employee);
            return query;
        }
        public IQueryable<EmployeeOtherPayment> GetEmployeeOtherPaymentByHeaderIncluded()
        {
            var query = _kscHrContext.EmployeeOtherPayment.Include(x => x.OtherPaymentHeader);
            return query;
        }
        public EmployeeOtherPayment GetEmployeeOtherPaymentIncludeHeaderById(int id)
        {
            var query = GetEmployeeOtherPaymentByHeaderIncluded().FirstOrDefault(x => x.Id == id);
            return query;
        }

        //GetPermitSettingByFilter

        //public bool GetPermitSettingByFilter(int startDate, int endDate, int otherPaymentType)
        //{

        //    var isExist = _kscHrContext
        //        .OtherPaymentSetting
        //        .Any(x=>x.IsActive==true && x.ValidityStartYearMonth<= startDate && x.OtherPaymentTypeId == otherPaymentType);

        //       return isExist;
        //}


        public IQueryable<EmployeeOtherPayment> GetOtherPaymentWithHeaderIdForTextFileBank(int headerId)
        {
            var query = _kscHrContext.EmployeeOtherPayment.Where(x => x.OtherPaymentHeaderId == headerId && x.IsBlacklist == false).Include(x => x.Employee);
            return query;
        }

        public IQueryable<EmployeeOtherPayment> GetEmployeeOtherPaymentForPrivatePortal(int employeeId, int paymnetMonth)
        {
            return _kscHrContext.EmployeeOtherPayment.Where(x => x.EmployeeId == employeeId
              && x.IsBlacklist == false && x.OtherPaymentHeader.PaymentYearMonth == paymnetMonth && x.OtherPaymentHeader.ShowInPortal &&
              x.OtherPaymentHeader.AccountingDocumentNumber != null //&& x.AccountNumber != null
              && x.OtherPaymentHeader.BankId != null && x.InvalidPayment == false
              ).Include(x => x.OtherPaymentHeader).ThenInclude(x => x.AccountBankType).AsNoTracking();
        }
        public IQueryable<EmployeeOtherPayment> GetEmployeeOtherPaymentByHeaderIds(List<int> headerIds)
        {
            return _kscHrContext.EmployeeOtherPayment.Where(x => headerIds.Any(h => h == x.OtherPaymentHeaderId))
                .Include(x => x.Employee).ThenInclude(x => x.MaritalStatus)
                 .Include(x => x.Employee).ThenInclude(x => x.GenderType);

        }

        public async Task<KscResult> EmployeeOtherPaymentManagementVacationSold(OtherPaymentModel model)
        {
            var result = model.IsValid();
            var aacountcodemodel = _kscHrContext.AccountCode.Find(model.AccountCodeId);

            var otherPaymentHeaderQuery = _kscHrContext.OtherPaymentHeader
                .Where(x => x.PaymentYearMonth == model.SalaryDate
                         && x.AccountCodeId == model.AccountCodeId
                         && x.OtherPaymentStatusId != EnumOtherPaymentStatus.SaveDocumentAccounting.Id);

            if (aacountcodemodel.IsValidRepeatableInOtherPaymnetPeriod == false)
            {
                if (otherPaymentHeaderQuery
                    //_kscHrContext.OtherPaymentHeader
                    .Any(x => //x.PaymentYearMonth == model.SalaryDate&&
                            x.OtherPaymentStatusId != EnumOtherPaymentStatus.CreatePayment.Id
                           //&& x.AccountCodeId == model.AccountCodeId
                           ))
                {
                    result.AddError("", "وضعیت پرداخت در حالت ایجاد اطلاعات نمیباشد");
                    return result;
                }
            }
            var salaryyear = Convert.ToInt32(model.SalaryDate.ToString().Substring(0, 4));
            var objHeader = new OtherPaymentHeader();
            bool CanInsert = !otherPaymentHeaderQuery
                    //_kscHrContext.OtherPaymentHeader
                    .Any(x => //x.PaymentYearMonth == model.SalaryDate&&
                       x.OtherPaymentStatusId == EnumOtherPaymentStatus.CreatePayment.Id
                      //&& x.AccountCodeId == model.AccountCodeId
                      );
            if (CanInsert)
            {
                objHeader = new OtherPaymentHeader()
                {
                    AccountCodeId = model.AccountCodeId,
                    PaymentYearMonth = model.SalaryDate,
                    OtherPaymentStatusId = EnumOtherPaymentStatus.CreatePayment.Id,
                };
                objHeader.OtherPaymentHeaderTypes.Add(new OtherPaymentHeaderType()
                {
                    OtherPaymentTypeId = model.OtherPaymentTypeId
                });
            }
            else
            {

                var otherPaymentHeader = _kscHrContext.OtherPaymentHeader.Include(x => x.EmployeeOtherPayment)
                              .Where(x => x.AccountCodeId == model.AccountCodeId && x.PaymentYearMonth == model.SalaryDate
                            && x.OtherPaymentStatusId == EnumOtherPaymentStatus.CreatePayment.Id);
                objHeader = otherPaymentHeader.SingleOrDefault();
            }
            var date = DateTime.Now;
            var paymentAmount = model.PaymentAmount;
            if (!objHeader.EmployeeOtherPayment.Any(x => x.EmployeeId == model.EmployeeId))
            {
                //
                string costCenterCode = _kscHrContext.ViewMisEmployee.FirstOrDefault(x => x.EmployeeId == model.EmployeeId).CostCenterCode;
                //

                var employeeOtherPaymentmodel = new EmployeeOtherPayment()
                {
                    EmployeeId = model.EmployeeId.Value,
                    InsertDate = DateTime.Now,
                    InsertUser = model.CurrentUser,
                    PaymentAmount = model.PaymentAmount,
                    CostCenterCode = costCenterCode,
                    DefaultPaymentAmount = paymentAmount,
                    AuthenticateUserName = model.AuthenticateUserName,
                    // IsBlacklist = blacklist.Any(a => a.EmployeeId == employeereward.EmployeeId)
                };
                employeeOtherPaymentmodel.EmployeeOtherPaymentType.Add(new EmployeeOtherPaymentType()
                {
                    OtherPaymentTypeId = model.OtherPaymentTypeId,
                    PaymnetAmountUnit = paymentAmount,
                    PaymentTypeDate = date,
                    Year = salaryyear
                });
                //;
                objHeader.EmployeeOtherPayment.Add(employeeOtherPaymentmodel);
            }
            else
            {
                var employeeOtherPaymentmodel = objHeader.EmployeeOtherPayment.First(x => x.EmployeeId == model.EmployeeId);
                employeeOtherPaymentmodel.PaymentAmount = paymentAmount;
                employeeOtherPaymentmodel.DefaultPaymentAmount = paymentAmount;
            }

            objHeader.OtherPaymentDetail.Add(new OtherPaymentDetail()
            {
                InsertDate = date,
                InsertUser = model.CurrentUser,
                AuthenticateUserName = model.AuthenticateUserName,
                OtherPaymentStatusId = EnumOtherPaymentStatus.CreatePayment.Id,
                RemoteIpAddress = model.RemoteIpAddress
            });
            if (CanInsert)
                await _kscHrContext.OtherPaymentHeader.AddAsync(objHeader);
            else
            {
                _kscHrContext.OtherPaymentHeader.Update(objHeader);
            }
            return result;
        }
        /// <summary>
        /// افزودن لیست پرسنل به پرداخت متفرقه
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public async Task<KscResult> InsertEmployeesOtherPaymentManagement(OtherPaymentModel model)
        {
            var result = model.IsValid();
            var aacountcodemodel = _kscHrContext.AccountCode.Find(model.AccountCodeId);

            var otherPaymentHeaderQuery = _kscHrContext.OtherPaymentHeader
                .Where(x => x.PaymentYearMonth == model.SalaryDate
                         && x.AccountCodeId == model.AccountCodeId
                         && x.OtherPaymentStatusId != EnumOtherPaymentStatus.SaveDocumentAccounting.Id);

            if (aacountcodemodel.IsValidRepeatableInOtherPaymnetPeriod == false)
            {
                if (otherPaymentHeaderQuery
                    //_kscHrContext.OtherPaymentHeader
                    .Any(x => x.PaymentYearMonth == model.SalaryDate &&
                            x.OtherPaymentStatusId != EnumOtherPaymentStatus.CreatePayment.Id
                           && x.AccountCodeId == model.AccountCodeId
                           ))
                {
                    result.AddError("", "وضعیت پرداخت در حالت ایجاد اطلاعات نمیباشد");
                    return result;
                }
            }
            var salaryyear = Convert.ToInt32(model.SalaryDate.ToString().Substring(0, 4));
            var objHeader = new OtherPaymentHeader();
            bool CanInsert = !otherPaymentHeaderQuery
                    //_kscHrContext.OtherPaymentHeader
                    .Any(x => x.PaymentYearMonth == model.SalaryDate &&
                       x.OtherPaymentStatusId == EnumOtherPaymentStatus.CreatePayment.Id
                      && x.AccountCodeId == model.AccountCodeId
                      );
            var EmpIds = model.Employees.Select(x => x.EmployeeId);
            var ViewMisEmployeequery = _kscHrContext.ViewMisEmployee.Where(a => EmpIds.Contains(a.EmployeeId)).ToList();
            if (CanInsert)
            {
                objHeader = new OtherPaymentHeader()
                {
                    AccountCodeId = model.AccountCodeId,
                    PaymentYearMonth = model.SalaryDate,
                    OtherPaymentStatusId = EnumOtherPaymentStatus.CreatePayment.Id,
                };
                objHeader.OtherPaymentHeaderTypes.Add(new OtherPaymentHeaderType()
                {
                    OtherPaymentTypeId = model.OtherPaymentTypeId
                });
            }
            else
            {

                var otherPaymentHeader = _kscHrContext.OtherPaymentHeader.Include(x => x.EmployeeOtherPayment)
                              .Where(x => x.AccountCodeId == model.AccountCodeId && x.PaymentYearMonth == model.SalaryDate
                            && x.OtherPaymentStatusId == EnumOtherPaymentStatus.CreatePayment.Id);
                objHeader = otherPaymentHeader.SingleOrDefault();
            }
            var date = DateTime.Now;
            var paymentAmount = model.PaymentAmount;
            foreach (var item in model.Employees)
            {
                if (!objHeader.EmployeeOtherPayment.Any(x => x.EmployeeId == item.EmployeeId))
                {
                    //
                    string? costCenterCode = ViewMisEmployeequery.FirstOrDefault(x => x.EmployeeId == item.EmployeeId).CostCenterCode ?? "یافت نشد";
                    //

                    var employeeOtherPaymentmodel = new EmployeeOtherPayment()
                    {
                        EmployeeId = item.EmployeeId,
                        InsertDate = DateTime.Now,
                        InsertUser = model.CurrentUser,
                        PaymentAmount = item.PaymentAmount,
                        CostCenterCode = costCenterCode,
                        DefaultPaymentAmount = item.DefaultPaymentAmount,
                        AuthenticateUserName = model.AuthenticateUserName,
                        // IsBlacklist = blacklist.Any(a => a.EmployeeId == employeereward.EmployeeId)
                    };


                    employeeOtherPaymentmodel.EmployeeOtherPaymentType.Add(new EmployeeOtherPaymentType()
                    {
                        OtherPaymentTypeId = model.OtherPaymentTypeId,
                        PaymnetAmountUnit = item.PaymentAmount,
                        PaymentTypeDate = date,
                        Year = salaryyear
                    });
                    //;
                    objHeader.EmployeeOtherPayment.Add(employeeOtherPaymentmodel);
                }
                else
                {
                    var employeeOtherPaymentmodel = objHeader.EmployeeOtherPayment.First(x => x.EmployeeId == model.EmployeeId);
                    employeeOtherPaymentmodel.PaymentAmount = paymentAmount;
                    employeeOtherPaymentmodel.DefaultPaymentAmount = paymentAmount;
                }
            }

            objHeader.OtherPaymentDetail.Add(new OtherPaymentDetail()
            {
                InsertDate = date,
                InsertUser = model.CurrentUser,
                AuthenticateUserName = model.AuthenticateUserName,
                OtherPaymentStatusId = EnumOtherPaymentStatus.CreatePayment.Id,
                RemoteIpAddress = model.RemoteIpAddress
            });
            if (CanInsert)
                await _kscHrContext.OtherPaymentHeader.AddAsync(objHeader);
            else
            {
                _kscHrContext.OtherPaymentHeader.Update(objHeader);
            }
            return result;
        }

        public async Task<KscResult> InsertSingleEmployeeOtherPaymentManagement(OtherPaymentModel model)
        {
            var result = model.IsValid();
            var aacountcodemodel = _kscHrContext.AccountCode.Find(model.AccountCodeId);

            var otherPaymentHeaderQuery = _kscHrContext.OtherPaymentHeader
                .Where(x => x.PaymentYearMonth == model.SalaryDate
                         && x.AccountCodeId == model.AccountCodeId
                         && x.OtherPaymentStatusId != EnumOtherPaymentStatus.SaveDocumentAccounting.Id);

            if (aacountcodemodel.IsValidRepeatableInOtherPaymnetPeriod == false)
            {
                if (otherPaymentHeaderQuery
                    //_kscHrContext.OtherPaymentHeader
                    .Any(x => x.PaymentYearMonth == model.SalaryDate &&
                            x.OtherPaymentStatusId != EnumOtherPaymentStatus.CreatePayment.Id
                           && x.AccountCodeId == model.AccountCodeId
                           ))
                {
                    result.AddError("", "وضعیت پرداخت در حالت ایجاد اطلاعات نمیباشد");
                    return result;
                }
            }
            var salaryyear = Convert.ToInt32(model.SalaryDate.ToString().Substring(0, 4));
            var objHeader = new OtherPaymentHeader();
            bool CanInsert = !otherPaymentHeaderQuery
                    //_kscHrContext.OtherPaymentHeader
                    .Any(x => x.PaymentYearMonth == model.SalaryDate &&
                       x.OtherPaymentStatusId == EnumOtherPaymentStatus.CreatePayment.Id
                      && x.AccountCodeId == model.AccountCodeId
                      );
            var EmpIds = model.Employees.Select(x => x.EmployeeId);
            var ViewMisEmployeequery = _kscHrContext.ViewMisEmployee.Where(a => EmpIds.Contains(a.EmployeeId)).ToList();
            if (CanInsert)
            {
                objHeader = new OtherPaymentHeader()
                {
                    AccountCodeId = model.AccountCodeId,
                    PaymentYearMonth = model.SalaryDate,
                    OtherPaymentStatusId = EnumOtherPaymentStatus.CreatePayment.Id,
                };
                objHeader.OtherPaymentHeaderTypes.Add(new OtherPaymentHeaderType()
                {
                    OtherPaymentTypeId = model.OtherPaymentTypeId
                });
            }
            else
            {

                var otherPaymentHeader = _kscHrContext.OtherPaymentHeader.Include(x => x.EmployeeOtherPayment)
                              .Where(x => x.AccountCodeId == model.AccountCodeId && x.PaymentYearMonth == model.SalaryDate
                            && x.OtherPaymentStatusId == EnumOtherPaymentStatus.CreatePayment.Id);
                objHeader = otherPaymentHeader.SingleOrDefault();
            }
            var date = DateTime.Now;
            var paymentAmount = model.PaymentAmount;
            //foreach (var item in model.Employees)
            //{
            //    if (!objHeader.EmployeeOtherPayment.Any(x => x.EmployeeId == item.EmployeeId))
            //    {
            //        //
            //        string? costCenterCode = ViewMisEmployeequery.FirstOrDefault(x => x.EmployeeId == item.EmployeeId).CostCenterCode ?? "یافت نشد";
            //        //

            //        var employeeOtherPaymentmodel = new EmployeeOtherPayment()
            //        {
            //            EmployeeId = item.EmployeeId,
            //            InsertDate = DateTime.Now,
            //            InsertUser = model.CurrentUser,
            //            PaymentAmount = item.PaymentAmount,
            //            CostCenterCode = costCenterCode,
            //            DefaultPaymentAmount = item.DefaultPaymentAmount,
            //            AuthenticateUserName = model.AuthenticateUserName,
            //            // IsBlacklist = blacklist.Any(a => a.EmployeeId == employeereward.EmployeeId)
            //        };


            //        employeeOtherPaymentmodel.EmployeeOtherPaymentType.Add(new EmployeeOtherPaymentType()
            //        {
            //            OtherPaymentTypeId = model.OtherPaymentTypeId,
            //            PaymnetAmountUnit = item.PaymentAmount,
            //            PaymentTypeDate = date,
            //            Year = salaryyear
            //        });
            //        //;
            //        objHeader.EmployeeOtherPayment.Add(employeeOtherPaymentmodel);
            //    }
            //    else
            //    {
            //        var employeeOtherPaymentmodel = objHeader.EmployeeOtherPayment.First(x => x.EmployeeId == model.EmployeeId);
            //        employeeOtherPaymentmodel.PaymentAmount = paymentAmount;
            //        employeeOtherPaymentmodel.DefaultPaymentAmount = paymentAmount;
            //    }
            //}

            if (!objHeader.EmployeeOtherPayment.Any(x => x.EmployeeId == model.EmployeeId))
            {
                //
                string costCenterCode = ViewMisEmployeequery.FirstOrDefault(x => x.EmployeeId == model.EmployeeId) != null ? ViewMisEmployeequery.FirstOrDefault(x => x.EmployeeId == model.EmployeeId).CostCenterCode : "یافت نشد";
                //

                var employeeOtherPaymentmodel = new EmployeeOtherPayment()
                {
                    EmployeeId = model.EmployeeId ?? 0,
                    InsertDate = DateTime.Now,
                    InsertUser = model.CurrentUser,
                    PaymentAmount = model.PaymentAmount,
                    CostCenterCode = costCenterCode,
                    DefaultPaymentAmount = model.DefaultPaymentAmount,
                    AuthenticateUserName = model.CurrentUser,
                    // IsBlacklist = blacklist.Any(a => a.EmployeeId == employeereward.EmployeeId)
                };


                employeeOtherPaymentmodel.EmployeeOtherPaymentType.Add(new EmployeeOtherPaymentType()
                {
                    OtherPaymentTypeId = model.OtherPaymentTypeId,
                    PaymnetAmountUnit = model.PaymentAmount,
                    PaymentTypeDate = date,
                    Year = salaryyear
                });
                //;
                objHeader.EmployeeOtherPayment.Add(employeeOtherPaymentmodel);
            }
            else
            {
                var employeeOtherPaymentmodel = objHeader.EmployeeOtherPayment.First(x => x.EmployeeId == model.EmployeeId);
                employeeOtherPaymentmodel.PaymentAmount = paymentAmount;
                employeeOtherPaymentmodel.DefaultPaymentAmount = paymentAmount;
            }

            objHeader.OtherPaymentDetail.Add(new OtherPaymentDetail()
            {
                InsertDate = date,
                InsertUser = model.CurrentUser,
                AuthenticateUserName = model.AuthenticateUserName,
                OtherPaymentStatusId = EnumOtherPaymentStatus.CreatePayment.Id,
                RemoteIpAddress = model.RemoteIpAddress
            });

            if (CanInsert)
                await _kscHrContext.OtherPaymentHeader.AddAsync(objHeader);
            else
            {
                _kscHrContext.OtherPaymentHeader.Update(objHeader);
            }
            return result;
        }
    }
}
