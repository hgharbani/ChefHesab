using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Stepper;
using Ksc.HR.Domain.Repositories.Stepper;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.Stepper
{
    public class Stepper_StatusSystemMonthRepository : EfRepository<Stepper_StatusSystemMonth, int>, IStepper_StatusSystemMonthRepository
    {
        private readonly KscHrContext _kscHrContext;
        public Stepper_StatusSystemMonthRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public Stepper_StatusSystemMonth GetByYearMonth_SystemControlId(int yearMonth, int systemSequenceControlId)
        {
            var result = _kscHrContext.Stepper_StatusSystemMonths.Include(x => x.SystemSequenceStatus).FirstOrDefault(x => x.YearMonth == yearMonth && x.SystemSequenceStatus.SystemSequenceControlId == systemSequenceControlId);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="yearMonth">سال ماه</param>
        /// <param name="systemSequenceControlId">وضعیت سیستم برای مثال وضعیت سیستم پرسنلی</param>
        /// <param name="systemSequenceStatusId">وضعیت سیستم برای مثال سیستم پرسنلی بسته است</param>
        /// <returns></returns>
        public bool CheckYearMonth_SystemControlId(int yearMonth, int systemSequenceControlId,int systemSequenceStatusId)
        {
            var data = GetByYearMonth_SystemControlId(yearMonth, systemSequenceControlId);
            var result = data != null ? data.SystemSequenceStatusId == systemSequenceStatusId : false;
                
            return result;
        }
    }
}
