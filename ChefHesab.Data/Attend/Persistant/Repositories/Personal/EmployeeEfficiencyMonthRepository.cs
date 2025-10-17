using KSC.Infrastructure.Persistance;

using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.ODSViews;
using Ksc.HR.Domain.Entities.Oncall;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Repositories.OnCall;
using Ksc.HR.Domain.Repositories.Personal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Share.Model;
using Ksc.HR.Share.Model.PaymentStatus;
using Ksc.Hr.Domain.Repositories.Personal;

namespace Ksc.HR.Data.Persistant.Repositories.Personal
{
    public class EmployeeEfficiencyMonthRepository : EfRepository<EmployeeEfficiencyMonth, int>, IEmployeeEfficiencyMonthRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeeEfficiencyMonthRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<EmployeeEfficiencyMonth> GetByYearMonth(int yearMonth)
        {
            var result = _kscHrContext.EmployeeEfficiencyMonth.Where(x => x.YearMonth == yearMonth);
            return result;
        }

    }

}
