using EFCore.BulkExtensions;
using Ksc.Hr.Domain.Entities;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Domain.Repositories.Pay;
using Ksc.HR.Share.Model.OtherPaymentStatus;
using Ksc.HR.Share.Model.Pay;
using KSC.Infrastructure.Persistance;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.Pay
{
    public class EmployeeDeductionDetailRepository : EfRepository<EmployeeDeductionDetail, long>, IEmployeeDeductionDetailRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeeDeductionDetailRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        /// <summary>
        /// به ازای سال ماه مشخص داده هایی که در حالت ایجاد بدهی هستند پیدا میکند
        /// </summary>
        public IQueryable<EmployeeDeductionDetail> GetGridConfirmDeduction(int yearMonth,List<int> types)
        {
            var query = _kscHrContext.EmployeeDeductionDetail.Where(x => x.InstallmentPaymentDate == yearMonth 
                                                                      && x.EmployeeDeductionStatusId == EnumEmployeeDeductionStatus.DeductionCreate.Id)
                .Include(x => x.EmployeeDeductionHeader)
                .ThenInclude(x => x.Employee)
                .Where(x => types.Contains(x.EmployeeDeductionHeader.EmployeeDeductionTypeId) && x.EmployeeDeductionHeader.IsActive == true)
                //جهت یافتن کد حساب  و شرح کد حساب
                //.ThenInclude(x => x.EmployeeDeductionType).ThenInclude(x => x.AccountCodeDeductionTypes.Where(q => q.IsActive == true)).ThenInclude(x => x.AccountCode)
                ;

            return query;
        }
        public IQueryable<EmployeeDeductionDetail> GetEmployeeDeductionDetailByHeaderId(long headerId)
        {
            var query = _kscHrContext.EmployeeDeductionDetail.Where(x => x.EmployeeDeductionHeaderId == headerId);

            return query;
        }


        public async Task<EmployeeDeductionDetail> GetOne(long id)
        {
            return await GetAllByRelatedGrid().FirstAsync(a => a.Id == id);
        }




        public IQueryable<EmployeeDeductionDetail> GetAllByRelatedGrid()
        {
            var result = _kscHrContext.EmployeeDeductionDetail
                .Include(x => x.EmployeeDeductionHeader).ThenInclude(x => x.AccountCode)
                .Include(x => x.EmployeeDeductionHeader).ThenInclude(x => x.EmployeeDeductionType)
                .Include(x => x.EmployeeDeductionStatus)
                 .Include(x => x.EmployeeDeductionHeader).ThenInclude(x => x.Employee)
                .AsQueryable();
            return result;
        }
        public void UpdateRange(List<EmployeeDeductionDetail> list)
        {
            _kscHrContext.UpdateRange(list);
        }

        public async Task<bool> UpdateBulkAsync(List<EmployeeDeductionDetail> entity)
        {
            try
            {
               
                await _kscHrContext.BulkUpdateAsync(entity, option =>
                {
                    option.IncludeGraph = true;
                    option.IncludeGraph = false;
                    option.UseTempDB = true;
                    option.SetOutputIdentity = true;
                    option.CalculateStats = true;
                    option.BatchSize = 4000;
                    option.PreserveInsertOrder = true;
                });



                return true;
            }
            catch (Exception)
            {
                return false;
            }


        }


    }
}

