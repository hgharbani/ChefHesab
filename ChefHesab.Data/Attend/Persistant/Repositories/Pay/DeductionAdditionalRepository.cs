using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Domain.Entities.Salary;
using Ksc.HR.Domain.Repositories.Pay;
using Ksc.HR.Share.Model.DeductionAdditional;
using Ksc.HR.Share.Model.Pay;
using KSC.Common;
using KSC.Infrastructure.Persistance;
using KSCCommunicationAPI.Models.Class;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.Pay
{
    public class DeductionAdditionalRepository : EfRepository<DeductionAdditional, int>, IDeductionAdditionalRepository
    {
        private readonly KscHrContext _kscHrContext;
        public DeductionAdditionalRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public bool IsExistDeductionAdditional(int accountCodeId, int yearMonth)
        {
            if (_kscHrContext.DeductionAdditionals.Any(a => a.AccountCodeId == accountCodeId &&
            a.YearMonthStart == yearMonth &&
            a.YearMonthEnd == yearMonth &&
            a.StatusId == EnumDeductionAdditionalStatus.Create.Id))
            {
                return true;
            }
            return false;

        }
        public void InsertDeductionAdditional(DeductionAdditionalDto dto)
        {
            if (_kscHrContext.DeductionAdditionals.Any(a => a.AccountCodeId == dto.AccountCodeId && a.YearMonthStart == dto.YearMonthStart && a.YearMonthEnd == dto.YearMonthEnd && a.StatusId == EnumDeductionAdditionalStatus.Create.Id))
            {
                var data = _kscHrContext.DeductionAdditionals.Where(a => a.AccountCodeId == dto.AccountCodeId && a.YearMonthStart == dto.YearMonthStart && a.YearMonthEnd == dto.YearMonthEnd && a.StatusId == EnumDeductionAdditionalStatus.Create.Id).Include(a => a.EmployeeDeductionAdditionals).First();
                _kscHrContext.EmployeeDeductionAdditionals.RemoveRange(data.EmployeeDeductionAdditionals);
                _kscHrContext.DeductionAdditionals.Remove(data);
            }
            var deductionAdditionalmodel = new DeductionAdditional()
            {
                AccountCodeId = dto.AccountCodeId,
                InsertDate = DateTime.Now,
                InsertUser = dto.InsertUser,
                YearMonthStart = dto.YearMonthStart,
                YearMonthEnd = dto.YearMonthEnd,
                StatusId = EnumDeductionAdditionalStatus.Create.Id,
            };
            foreach (var item in dto.Employees)
            {
                deductionAdditionalmodel.EmployeeDeductionAdditionals.Add(new EmployeeDeductionAdditional()
                {
                    YearMonthStart = dto.YearMonthStart,
                    YearMonthEnd = dto.YearMonthEnd,
                    InsertUser = dto.InsertUser,
                    InsertDate = DateTime.Now,
                    RemoteIpAddress = dto.RemoteIpAddress,
                    AuthenticateUserName = dto.AuthenticateUserName,
                    EmployeeId = item.EmployeeId,
                    Amount = item.Amount
                });
            }
            _kscHrContext.Add(deductionAdditionalmodel);

        }
        public bool CheckCanInsertDeductionAdditional(int accountCodeId, int yearMonth)
        {
            if (_kscHrContext.DeductionAdditionals.Any(a => a.AccountCodeId == accountCodeId &&
            a.YearMonthStart == yearMonth &&
            a.YearMonthEnd == yearMonth &&
            a.StatusId == EnumDeductionAdditionalStatus.SendToMis.Id))
                return false;
            else return true;
        }




        public IQueryable<DeductionAdditional> GetByYearMonth(int yearMonth)
        {
            var result = _kscHrContext.DeductionAdditionals.Where(x => x.YearMonthStart == yearMonth)
                .Include(x => x.Salary_AccountCode) //عناوین کد حساب
                                                    //.Include(x => x.Status) // وضعیت
                .Include(x => x.EmployeeDeductionAdditionals); //افراد

            return result;
        }

        /// <summary>
        ///در صورت وجود افراد جدید به لیست قبلی اضافه شوند و ضعیت به صورت ایجاد برمیگردد
        /// </summary>
        /// <param name="dto"></param>
        public void InsertDeductionAdditionalGeneral(DeductionAdditionalDto dto)
        {
            var headerExist = _kscHrContext.DeductionAdditionals.FirstOrDefault(x => x.AccountCodeId == dto.AccountCodeId 
            && x.YearMonthStart == dto.YearMonthStart 
            && x.YearMonthEnd == dto.YearMonthEnd );

            if (headerExist != null)
            {
                //یافتن header  در صورت وجود
                // افراد جدید به لیست قبلی اضافه شوند
                headerExist.StatusId = EnumDeductionAdditionalStatus.Create.Id; 

                foreach (var item in dto.Employees)
                {
                    headerExist.EmployeeDeductionAdditionals.Add(new EmployeeDeductionAdditional()
                    {
                        YearMonthStart = dto.YearMonthStart,
                        YearMonthEnd = dto.YearMonthEnd,
                        InsertUser = dto.InsertUser,
                        InsertDate = DateTime.Now,
                        RemoteIpAddress = dto.RemoteIpAddress,
                        AuthenticateUserName = dto.AuthenticateUserName,
                        EmployeeId = item.EmployeeId,
                        Amount = item.Amount
                    });
                }
                _kscHrContext.Update(headerExist);
            }
            else
            {
                var deductionAdditionalmodel = new DeductionAdditional()
                {
                    AccountCodeId = dto.AccountCodeId,
                    InsertDate = DateTime.Now,
                    InsertUser = dto.InsertUser,
                    YearMonthStart = dto.YearMonthStart,
                    YearMonthEnd = dto.YearMonthEnd,
                    StatusId = EnumDeductionAdditionalStatus.Create.Id,
                };
                foreach (var item in dto.Employees)
                {
                    deductionAdditionalmodel.EmployeeDeductionAdditionals.Add(new EmployeeDeductionAdditional()
                    {
                        YearMonthStart = dto.YearMonthStart,
                        YearMonthEnd = dto.YearMonthEnd,
                        InsertUser = dto.InsertUser,
                        InsertDate = DateTime.Now,
                        RemoteIpAddress = dto.RemoteIpAddress,
                        AuthenticateUserName = dto.AuthenticateUserName,
                        EmployeeId = item.EmployeeId,
                        Amount = item.Amount
                    });
                }
                _kscHrContext.Add(deductionAdditionalmodel);
            }
            

        }


    }
}
