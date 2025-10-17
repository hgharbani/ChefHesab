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
    public class EmployeeEfficiencyHistoryRepository : EfRepository<EmployeeEfficiencyHistory, int>, IEmployeeEfficiencyHistoryRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeeEfficiencyHistoryRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<EmployeeEfficiencyHistory> GetByEmployeeId(int id)
        {
            var result = _kscHrContext.EmployeeEfficiencyHistory.Where(x => x.EmployeeId == id);
            return result;
        }

        public IQueryable<EmployeeEfficiencyHistory> GetLatestData(int yearMonth)
        {
            var result = _kscHrContext.EmployeeEfficiencyHistory.Include(x => x.Employee)
                .Where(x => x.YearMonth == yearMonth && x.IsLatest == true);
            return result;
        }

        public IQueryable<EmployeeEfficiencyHistory> GetKendoGrid(List<int> Ids, int yearMonthDayShamsi_Prev, int yearMonth)
        {
            var result = _kscHrContext.EmployeeEfficiencyHistory.Where(a => Ids.Contains(a.EmployeeId) &&
                                      (a.YearMonth == yearMonthDayShamsi_Prev || a.YearMonth == yearMonth));
            return result;
        }

    }

}
