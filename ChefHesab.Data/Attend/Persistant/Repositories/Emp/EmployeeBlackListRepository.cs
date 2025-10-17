using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities;
using Ksc.HR.Domain.Entities.Emp;
using Ksc.HR.Domain.Repositories.Emp;
using Ksc.HR.Domain.Repositories.EmployeeBase;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.Emp
{
    public class EmployeeBlackListRepository : EfRepository<EmployeeBlackList, int>, IEmployeeBlackListRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeeBlackListRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<EmployeeBlackList> GetAllRelated()
        {
            return _kscHrContext.EmployeeBlackList.Include(x => x.Employee).Include(x => x.OtherPaymentType).AsQueryable();
        }
        public IQueryable<EmployeeBlackList> GetAllByDate(DateTime startdate, DateTime enddate, int[] otherPaymentTypeId)
        {
            //var tt = GetAllRelated().Where(x => x.IsActive && otherPaymentTypeId.Any(o => o == x.OtherPaymentTypeId) &&
            //(
            //    (x.StartDate < startdate && (x.EndDate == null || x.EndDate >= startdate))));
           // var tt1 = GetAllRelated().Where(x => x.IsActive &&
           //(
           //    (x.StartDate >= startdate && x.StartDate <= enddate)));
            return GetAllRelated().Where(x => x.IsActive && otherPaymentTypeId.Any(o => o == x.OtherPaymentTypeId) &&
            (
                (x.StartDate < startdate && (x.EndDate == null || x.EndDate >= startdate))// شروع ماه قبل و پایان  ماه جاری 
                 || (x.StartDate >= startdate && x.StartDate <= enddate)// شروع ماه جاری  و پایان  ماه بعد
            )
            );
        }
        public IQueryable<EmployeeBlackList> GetAllByOtherPaymentTypeId(int[] otherPaymentTypeId)
        {
            var tt = GetAllRelated().Where(x => x.IsActive && otherPaymentTypeId.Any(o => o == x.OtherPaymentTypeId));
            return tt;
        }


    }
}
