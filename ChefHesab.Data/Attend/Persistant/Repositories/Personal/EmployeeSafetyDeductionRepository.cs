using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Repositories.Personal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.Hr.Domain.Entities;

namespace Ksc.HR.Data.Persistant.Repositories.Personal
{
    public class EmployeeSafetyDeductionRepository : EfRepository<EmployeeSafetyDeduction, int>, IEmployeeSafetyDeductionRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeeSafetyDeductionRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public void DeleteRange(List<EmployeeSafetyDeduction> list)
        {
            _kscHrContext.EmployeeSafetyDeductions.RemoveRange(list);
        }

    }
}
