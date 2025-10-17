using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Domain.Repositories.Pay;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.Pay
{
    public class EmployeeOtherPaymentTemplateRepository : EfRepository<EmployeeOtherPaymentTemplate, long>, IEmployeeOtherPaymentTemplateRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeeOtherPaymentTemplateRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<EmployeeOtherPaymentTemplate> GetEmployeeOtherPaymentTemplateByFileGuid(Guid fileGuid)
        {
            return _kscHrContext
                .EmployeeOtherPaymentTemplates
                .Where(a => a.FileGuid == fileGuid);
        }
    }
}
